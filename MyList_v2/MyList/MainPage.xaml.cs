using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using MyList.ViewModels;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage.AccessCache;
using Windows.ApplicationModel.DataTransfer;
using System.Text;
using SQLitePCL;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MyList
{
    /// <summary>
    //F:\代码\现操\MyList\MyList\Assets\    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ViewModels.ListItemViewModels ViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel = ViewModels.ListItemViewModels.getInstance();
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (ViewModel.SelectedItem == null)
            {
                createButton.Content = "Create";
                title.Text = "";
                details.Text = "";
                DueDate.Date = DateTime.Now;
            }
            else
            {
                createButton.Content = "Update";
                itemImage.Source = ViewModel.SelectedItem.imagerUrl;
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.description;
                DueDate.Date = ViewModel.SelectedItem.date;
            }

            if(e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("mainpage");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("mainpage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["mainpage"] as ApplicationDataCompositeValue;
                    for(int i = 0;i < ViewModels.ListItemViewModels.getInstance().AllItems.Count(); i++)
                    {
                        ViewModels.ListItemViewModels.getInstance().AllItems[i].completed = (bool)composite["isChecked"+i];
                    }
                    title.Text = (string)composite["title"];
                    details.Text = (string)composite["details"];
                    DueDate.Date = (DateTimeOffset)composite["date"];

                    StorageFile theFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync((string)ApplicationData.Current.LocalSettings.Values["image"]);
                    if(theFile != null)
                    {
                        await theFile.CopyAsync(ApplicationData.Current.LocalFolder, theFile.Name, NameCollisionOption.ReplaceExisting);
                        BitmapImage bitmapImage = new BitmapImage(new Uri("ms-appdata:///local/" + theFile.Name));
                        itemImage.Source = bitmapImage;
                    }
                    
                    ApplicationData.Current.LocalSettings.Values.Remove("mainpage");
                }
            }

            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();  
                for (int i = 0;i < ViewModel.AllItems.Count(); i++)
                {
                    composite["isChecked"+i] = ViewModel.AllItems[i].completed;
                }
                composite["title"] = title.Text;
                composite["details"] = details.Text;
                composite["date"] = DueDate.Date;
                ApplicationData.Current.LocalSettings.Values["mainpage"] = composite;

            }
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;

            for (int i = 0; i < ViewModel.AllItems.Count(); i++)
            {
                String date = ViewModel.AllItems[i].date.ToString();
                BitmapImage bitmap = (BitmapImage)ViewModel.AllItems[i].imagerUrl;
                String path = bitmap.UriSource.ToString();
                String _completed = "";
                if (ViewModel.AllItems[i].completed)
                {
                    _completed = "true";
                }
                else
                {
                    _completed = "false";
                }
                var db = App.conn;
                try
                {
                    using (var statement = db.Prepare(App.UPDATE))
                    {

                        statement.Bind(1, ViewModel.AllItems[i].title);
                        statement.Bind(2, ViewModel.AllItems[i].description);
                        statement.Bind(3, date);
                        statement.Bind(4, _completed);
                        statement.Bind(5, path);
                        statement.Bind(6, ViewModel.AllItems[i].getId());
                        statement.Step();
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            BitmapImage bitmap = (BitmapImage)ViewModel.SelectedItem.imagerUrl;
            var photoFile = await StorageFile.GetFileFromApplicationUriAsync(bitmap.UriSource);
            dp.Properties.Title = ViewModel.SelectedItem.title;
            dp.Properties.Description = ViewModel.SelectedItem.description;
            dp.SetStorageItems(new List<StorageFile> { photoFile });
            dp.SetWebLink(new Uri("http://seattletimes.com/ABPub/2006/01/10/2002732410.jpg"));
            deferral.Complete();      
        }

        private void addAppBarButtonClicked(object sender, RoutedEventArgs e)
        {
            if(Window.Current.Bounds.Width > 800)
            {
                ViewModel.SelectedItem = null;
                createButton.Content = "Create";
                title.Text = "";
                details.Text = "";
                DueDate.Date = DateTime.Now;
            }
            else
            {
                ViewModel.SelectedItem = null;
                Frame.Navigate(typeof(NewPage));
            }
            
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            
            if (Window.Current.Bounds.Width > 800)
            {
                createButton.Content = "Update";
                itemImage.Source = ViewModel.SelectedItem.imagerUrl;
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.description;
                DueDate.Date = ViewModel.SelectedItem.date;
            }
            else
            {
                Frame.Navigate(typeof(NewPage));
            }
        }

        private void CreateButtonClicked(object sender, RoutedEventArgs e)
        {
            var messageContent = "";

            if (title.Text == "")
            {
                messageContent += "Title cannot be empty!";
            }
            if (details.Text == "")
            {
                messageContent += "\nDescription cannot be empty!";
            }
            if (DueDate.Date < DateTime.Today)
            {
                messageContent += "\nDue Date is illegal!";
            }
            if (messageContent != "")
            {
                MessageDialog errorMessage = new MessageDialog(messageContent);
                var result = errorMessage.ShowAsync();
            }
            else
            {
                var str = (String)createButton.Content;
                if (str == "Create")
                {
                    MessageDialog errorMessage = new MessageDialog("Item created successfully!\n");
                    var result = errorMessage.ShowAsync();
                    ImageSource defaultUrl = itemImage.Source;
                   
                    ViewModel.AddTodoItem(title.Text, details.Text, DueDate.Date.DateTime, defaultUrl);
                    
                    Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    MessageDialog errorMessage = new MessageDialog("Item updated successfully!\n");
                    var result = errorMessage.ShowAsync();
                    ImageSource defaultUrl = itemImage.Source;

                    ViewModel.UpdateTodoItem(title.Text, details.Text, DueDate.Date.DateTime,defaultUrl);
                    Frame.Navigate(typeof(MainPage));
                }
                ViewModel.UpdateTile();
            }
            
        }
        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            if((String)createButton.Content == "Create")
            {
                title.Text = "";
                details.Text = "";
                DueDate.Date = DateTime.Today;
            }
            else
            {
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.description;
                DueDate.Date = ViewModel.SelectedItem.date;
                itemImage.Source = ViewModel.SelectedItem.imagerUrl;
            }
        }

        private async void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                //挂起并关闭时保存到LocalSettings中
                ApplicationData.Current.LocalSettings.Values["image"] = StorageApplicationPermissions.FutureAccessList.Add(file);

                //图片保存到LocalFolder中，避免访问权限的问题。
                await file.CopyAsync(ApplicationData.Current.LocalFolder, file.Name, NameCollisionOption.ReplaceExisting);
                BitmapImage bitmapImage = new BitmapImage(new Uri("ms-appdata:///local/" + file.Name));
                itemImage.Source = bitmapImage;
            }
            
        }

        private void deleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                MessageDialog errorMessage = new MessageDialog("Delete successfully!\n");
                var result = errorMessage.ShowAsync();
                ViewModel.RemoveTodoItem();
                Frame.Navigate(typeof(MainPage));
                ViewModel.UpdateTile();
            }
            else
            {
                MessageDialog errorMessage = new MessageDialog("Please select an item.\n");
                var result = errorMessage.ShowAsync();
            }
            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //%是通配符，可代替所有字符
            StringBuilder searchInfo = new StringBuilder("%%");
            searchInfo.Insert(1, searchBox.Text);
            String searchResult = String.Empty;

            var db = App.conn;
            using(var statement = db.Prepare(App.SEARCH))
            {
                statement.Bind(1, searchInfo.ToString());
                statement.Bind(2, searchInfo.ToString());
                statement.Bind(3, searchInfo.ToString());
                while(SQLiteResult.ROW == statement.Step())
                {
                    searchResult += "Title: " + statement[0].ToString() + " ";
                    searchResult += "Details: " + statement[1].ToString() + " ";
                    searchResult += "DueDate: " + statement[2].ToString() + "\n";
                }
            }
            if(searchResult == "")
            {
                var result = new MessageDialog("No Match!\n").ShowAsync();
            }
            else
            {
                var result = new MessageDialog(searchResult).ShowAsync();
            }
        }
    }
}
