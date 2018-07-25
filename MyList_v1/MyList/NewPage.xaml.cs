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
using Windows.UI.Xaml.Media.Imaging;
using MyList.ViewModels;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MyList
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
            
        }

        private ViewModels.ListItemViewModels ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = ((ViewModels.ListItemViewModels)e.Parameter);
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
                    ViewModel.AddTodoItem(title.Text, details.Text, DueDate.Date.DateTime,defaultUrl);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
                else
                {
                    MessageDialog errorMessage = new MessageDialog("Item updated successfully!\n");
                    var result = errorMessage.ShowAsync();
                    ImageSource defaultUrl = itemImage.Source;
                    ViewModel.UpdateTodoItem("id", title.Text, details.Text, DueDate.Date.DateTime, defaultUrl);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
                
            }
        }
        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((String)createButton.Content == "Create")
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
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 600;
                    await bitmapImage.SetSourceAsync(fileStream);
                    itemImage.Source = bitmapImage;
                }
            }
        }

        private void deleteButtonClick(object sender, RoutedEventArgs e)
        {
            if(ViewModel.SelectedItem != null)
            {
                MessageDialog errorMessage = new MessageDialog("Delete successfully!\n");
                var result = errorMessage.ShowAsync();
                ViewModel.RemoveTodoItem("id");
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }
    }
}
