﻿#pragma checksum "F:\大二下\现操\作业\mediaPlayer\mediaPlayer\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7C95B43695380A20705DC49179FDF677"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mediaPlayer
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // MainPage.xaml line 25
                {
                    this.ellipse = (global::Windows.UI.Xaml.Shapes.Ellipse)(target);
                }
                break;
            case 2: // MainPage.xaml line 40
                {
                    this.mediaElement = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.mediaElement).MediaOpened += this.mediaElement_MediaOpened;
                }
                break;
            case 3: // MainPage.xaml line 45
                {
                    this.mediaSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.mediaSlider).ValueChanged += this.mediaSlider_ValueChanged;
                }
                break;
            case 4: // MainPage.xaml line 30
                {
                    this.ellipseStoryBoard = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 5: // MainPage.xaml line 36
                {
                    this.picture = (global::Windows.UI.Xaml.Media.ImageBrush)(target);
                }
                break;
            case 6: // MainPage.xaml line 50
                {
                    this.play = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.play).Click += this.play_Click;
                }
                break;
            case 7: // MainPage.xaml line 51
                {
                    this.pause = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.pause).Click += this.pause_Click;
                }
                break;
            case 8: // MainPage.xaml line 52
                {
                    this.stop = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.stop).Click += this.stop_Click;
                }
                break;
            case 9: // MainPage.xaml line 53
                {
                    this.select = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.select).Click += this.select_Click;
                }
                break;
            case 10: // MainPage.xaml line 54
                {
                    this.fullScreen = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.fullScreen).Click += this.fullScreen_Click;
                }
                break;
            case 11: // MainPage.xaml line 55
                {
                    this.volume = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                }
                break;
            case 12: // MainPage.xaml line 58
                {
                    this.volumeSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.volumeSlider).ValueChanged += this.volumeSlider_ValueChanged;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

