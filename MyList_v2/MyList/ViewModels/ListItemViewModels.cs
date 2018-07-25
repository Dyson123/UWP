using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using MyList.Models;
using SQLitePCL;

namespace MyList.ViewModels
{

    class ListItemViewModels
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        private static ListItemViewModels viewModel;

        private List<Models.TodoItem> newlist = new List<Models.TodoItem>();
        private ListItemViewModels()
        {   
        }
        
        public static ListItemViewModels getInstance()
        {
            if(viewModel == null)
            {
                viewModel = new ListItemViewModels();
                var db = App.conn;
                using(var statement = db.Prepare(App.SELECT))
                {
                   while(SQLiteResult.ROW == statement.Step())
                    {
                        DateTime date = Convert.ToDateTime(statement[3].ToString());
                        ImageSource defaultUrl = new BitmapImage(new Uri(statement[5].ToString()));
                        viewModel.allItems.Add(new Models.TodoItem(statement[0].ToString(), statement[1].ToString(),statement[2].ToString(), date, statement[4].ToString(), defaultUrl));
                    }
                }
               
                return viewModel;
            }
            else
            {
                return viewModel;
            }
        }

        public void AddTodoItem(string title, string description, DateTime dueDate,ImageSource defaultUrl)
        {
            String id = Guid.NewGuid().ToString(); 
            TodoItem newItem = new TodoItem(id,title, description, dueDate, "false", defaultUrl);
            this.allItems.Add(newItem);

            String _completed = "false";
            BitmapImage bitmap = (BitmapImage)defaultUrl;
            String path = bitmap.UriSource.ToString  ();
            String date = dueDate.ToString();
            
            var db = App.conn;
            try
            {
                using (var statement = db.Prepare(App.INSERT))
                {

                    statement.Bind(1, id);
                    statement.Bind(2, title);
                    statement.Bind(3, description);
                    statement.Bind(4, date);
                    statement.Bind(5, _completed);
                    statement.Bind(6, path);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            
        }

        public void RemoveTodoItem()
        {
            var db = App.conn;
            try
            {
                using (var statement = db.Prepare(App.DELETE))
                {
                    statement.Bind(1, this.selectedItem.getId());
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            this.allItems.Remove(this.selectedItem);
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string title, string description, DateTime dueDate, ImageSource url)
        {
            //DIY
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.date = dueDate;
            this.selectedItem.imagerUrl = url;
            
            String _completed = "";
            if (this.selectedItem.completed)
            {
                _completed = "true";
            }
            else
            {
                _completed = "false";
            }
            BitmapImage bitmap = (BitmapImage)url;
            String path = bitmap.UriSource.ToString();
            String date = dueDate.ToString();
            String id = this.selectedItem.getId();
            var db = App.conn;
            try
            {
                using(var statement = db.Prepare(App.UPDATE))
                {
                    
                    statement.Bind(1, title);
                    statement.Bind(2, description);
                    statement.Bind(3, date);
                    statement.Bind(4, _completed);
                    statement.Bind(5, path);
                    statement.Bind(6, id);
                    statement.Step();
                }
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            //set selectedItem to null after update
            this.selectedItem = null;
        }

        public void UpdateTile()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueueForWide310x150(true);
            updater.EnableNotificationQueueForSquare310x310(true);
            updater.EnableNotificationQueueForSquare150x150(true);
            updater.EnableNotificationQueue(true);
            updater.Clear();
            string AdaptiveTile = @"
            <tile>
              <visual>

                <binding template='TileSmall' branding='name'>
                  <image src='{2}' placement='background' />
                </binding>
    
                <binding template='TileMedium' branding='name'>
                  <image src='{2}' placement='background' />
                  <group>
                    <subgroup hint-weight='40'>
                      <text hint-style='subtitle'>{0}</text>
                      <text hint-style='captionsubtle' hint-wrap='true'>{1}</text>
                    </subgroup>
                  </group>
                </binding>

                <binding template='TileWide' branding='nameAndLogo'>
                  <image src='{2}' placement='background' />
                  <group>
                    <subgroup hint-weight='45'>
                      <text hint-style='subtitle'>{0}</text>
                      <text hint-style='captionsubtle' hint-wrap='true'>{1}</text>
                    </subgroup>
                  </group>
                </binding>

                <binding template='TileLarge' branding='nameAndLogo'>
                  <image src='{2}' placement='background' />
                  <group>
                    <subgroup hint-weight='45'>
                      <text hint-style='subtitle'>{0}</text>
                      <text hint-style='captionsubtle' hint-wrap='true'>{1}</text>
                    </subgroup>
                  </group>
                </binding>

              </visual>
            </tile>";

            foreach (var n in allItems)
            {
                var doc = new Windows.Data.Xml.Dom.XmlDocument();
                BitmapImage bitmap = (BitmapImage)n.imagerUrl;//change background dynamic
                var xml = string.Format(AdaptiveTile, n.title, n.description, bitmap.UriSource);
                doc.LoadXml(WebUtility.HtmlDecode(xml), new XmlLoadSettings
                {
                    ProhibitDtd = false,
                    ValidateOnParse = false,
                    ElementContentWhiteSpace = false,
                    ResolveExternals = false
                });

                updater.Update(new TileNotification(doc));
            }
        }
    }
}
