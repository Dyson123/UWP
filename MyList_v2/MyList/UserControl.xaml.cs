using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyList.ViewModels;
using MyList.Models;
using Windows.UI.Popups;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MyList
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    
    public sealed partial class UserControl : Page
    {
        public Models.TodoItem TodoItem { get { return this.DataContext as Models.TodoItem; } }
        
        ViewModels.ListItemViewModels ViewModel;
        
        public UserControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
            ViewModel = ViewModels.ListItemViewModels.getInstance();
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            dynamic x = e.OriginalSource;
            ViewModel.SelectedItem = (Models.TodoItem)x.DataContext;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic x = e.OriginalSource;
            ViewModel.SelectedItem = (Models.TodoItem)x.DataContext;

            /*
            if (Window.Current.Bounds.Width > 800)
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                Frame.Navigate(typeof(NewPage));
            }*/
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic x = e.OriginalSource;
            ViewModel.SelectedItem = (Models.TodoItem)x.DataContext;
            ViewModel.RemoveTodoItem();
            MessageDialog errorMessage = new MessageDialog("Delete successfully!\n");
            var result = errorMessage.ShowAsync();
        }
    }

    public class BoolToIsChecked : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool && (bool)value)
            {
                return true;
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((bool)value == true);
        }
    }
}
