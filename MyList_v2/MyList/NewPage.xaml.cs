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
using Windows.Storage.AccessCache;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MyList
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        ViewModels.ListItemViewModels ViewModel;

        public NewPage()
        {
            this.InitializeComponent();
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

            if (e.NavigationMode == NavigationMode.New)
            {
                //If this is new navigation, there is no need to reload any state. So remove it. 
                ApplicationData.Current.LocalSettings.Values.Remove("newpage");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("newpage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["newpage"] as ApplicationDataCompositeValue;
                    title.Text = (string)composite["title"];
                    details.Text = (string)composite["details"];
                    DueDate.Date = (DateTimeOffset)composite["date"];
                    StorageFile theFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync((string)ApplicationData.Current.LocalSettings.Values["image"]);
                    using (IRandomAccessStream fileStream = await theFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.DecodePixelWidth = 600;
                        await bitmapImage.SetSourceAsync(fileStream);
                        itemImage.Source = bitmapImage;
                    }
                    ApplicationData.Current.LocalSettings.Values.Remove("newpage");

                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                
                composite["title"] = title.Text;
                composite["details"] = details.Text;
                composite["date"] = DueDate.Date;
                ApplicationData.Current.LocalSettings.Values["newpage"] = composite;
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
                    ViewModel.UpdateTodoItem(title.Text, details.Text, DueDate.Date.DateTime, defaultUrl);
                    Frame.Navigate(typeof(MainPage));
                }
                ViewModel.UpdateTile();

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
                ApplicationData.Current.LocalSettings.Values["image"] = StorageApplicationPermissions.FutureAccessList.Add(file);
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
                ViewModel.RemoveTodoItem();
                ViewModel.UpdateTile();
                Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
