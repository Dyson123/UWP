using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using SQLitePCL;
using Windows.UI.Xaml.Media.Imaging;

namespace MyList
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public bool issuspend = false;
        public static SQLiteConnection conn;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.LoadDatabase();
        }
        public static String CREATE = @"CREATE TABLE IF NOT EXISTS TodoList(Id VARCHAR(140) PRIMARY KEY NOT NULL,Title VARCHAR(140), Description VARCHAR(140),DueDate VARCHAR(140), Completed VARCHAR(140), ImageUrl VARCHAR(140));";
        public static String INSERT = @"INSERT INTO TodoList(Id,Title,Description,DueDate,Completed,ImageUrl) VALUES(?,?,?,?,?,?);";
        public static String SELECT = @"SELECT Id,Title,Description,DueDate,Completed,ImageUrl FROM TodoList;";
        public static String UPDATE = @"UPDATE Todolist SET Title=?,Description=?,DueDate=?,Completed=?,ImageUrl=? WHERE Id=?;";
        public static String DELETE = @"DELETE FROM TodoList WHERE Id=?";
        public static String SEARCH = @"SELECT Title,Description,DueDate FROM TodoList WHERE Title LIKE ? OR Description LIKE ? OR DueDate LIKE ?";
        private void LoadDatabase()
        {
            conn = new SQLiteConnection("TodoList.db");
            using (var statement = conn.Prepare(CREATE))
            {
                statement.Step();
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NavigationState"))
                    {
                        rootFrame.SetNavigationState((string)ApplicationData.Current.LocalSettings.Values["NavigationState"]);
                    }
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigated += OnNavigated;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            issuspend = true;
            Frame frame = Window.Current.Content as Frame;
            ApplicationData.Current.LocalSettings.Values["NavigationState"] = frame.GetNavigationState();

            deferral.Complete();
        }

        private void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            
            if(rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                Windows.UI.Core.AppViewBackButtonVisibility.Visible :
                Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
        }
    }
}
