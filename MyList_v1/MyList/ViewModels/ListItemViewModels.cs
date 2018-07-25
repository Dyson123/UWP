using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MyList.ViewModels
{
    class ListItemViewModels
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public ListItemViewModels()
        {
            // 加入两个用来测试的item
            ImageSource defaultUrl = new BitmapImage(new Uri("ms-appx:///Assets/background.jpg"));
            this.allItems.Add(new Models.TodoItem("作业1", "我的作业1", DateTime.Now,defaultUrl));
            this.allItems.Add(new Models.TodoItem("作业2", "我的作业2",DateTime.Now,defaultUrl));
        }

        public void AddTodoItem(string title, string description, DateTime dueDate,ImageSource defaultUrl)
        {
            this.allItems.Add(new Models.TodoItem(title, description,dueDate, defaultUrl));
        }

        public void RemoveTodoItem(string id)
        {
            //DIY
            this.allItems.Remove(this.selectedItem);
            //set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string description, DateTime dueDate, ImageSource url)
        {
            //DIY
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.date = dueDate;
            this.selectedItem.imagerUrl = url;
            //set selectedItem to null after update
            this.selectedItem = null;
        }
    }
}
