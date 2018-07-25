using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace mediaPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //the media is video type;
        private bool flag = true;

        public MainPage()
        {
            this.InitializeComponent();
            ellipseStoryBoard.Stop();
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
            ellipseStoryBoard.Begin();
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
            ellipseStoryBoard.Pause();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
            ellipseStoryBoard.Stop();
        }

        private void fullScreen_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            if (!flag)
            {
                return;
            }
            if (view.IsFullScreenMode)
            {
                mediaElement.IsFullWindow = false;
                view.ExitFullScreenMode();
            }
            else
            {
                mediaElement.IsFullWindow = true;

                view.TryEnterFullScreenMode();
            }
        }

        private async void select_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();

            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".wmv");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".wma");

            StorageFile file = await picker.PickSingleFileAsync();
            if(file != null)
            {
                await file.CopyAsync(ApplicationData.Current.LocalFolder, file.Name, NameCollisionOption.ReplaceExisting);
                mediaElement.Source = new Uri("ms-appdata:///local/" + file.Name);
                if(file.FileType == ".mp4" || file.FileType == ".wmv")
                {
                    ellipse.Visibility = Visibility.Collapsed;
                    mediaElement.Visibility = Visibility.Visible;
                    flag = true;
                }
                else
                {
                    ellipse.Visibility = Visibility.Visible;
                    mediaElement.Visibility = Visibility.Collapsed;
                    flag = false;
                }
            }
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            mediaSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            mediaElement.Volume = (double)volumeSlider.Value/10;
            ellipseStoryBoard.Stop();
        }

        private void mediaSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            int sliderValue = (int)mediaSlider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, sliderValue);
            mediaElement.Position = ts;
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaElement.Volume = (double)volumeSlider.Value/10;
        }
    }
}
