using System;
using System.Collections.Generic;
using Victop.Server.Controls.Models;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using Victop.Wpf.Controls;
using Victop.Server.Controls;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.DataMessageManager;
using MetroFramePlugin.Views;
using Victop.Server.Controls.MVVM;

namespace MetroFramePlugin.ViewModels
{
    public class OverlayWindowViewModel:ModelBase
    {
        private OverlayWindow overlayWin;
        private VicTabControlNormal mainTabCtrl;
        private bool exitFlag = false;


        private bool pluginListShow = false;
        /// <summary>
        /// 插件列表显示
        /// </summary>
        public bool PluginListShow
        {
            get
            {
                return pluginListShow;
            }
            set
            {
                if (pluginListShow != value)
                {
                    pluginListShow = value;
                    RaisePropertyChanged("PluginListShow");
                }
            }
        }

        public ICommand mainWindowLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    //mainTabCtrl
                    overlayWin = (OverlayWindow)x;
                    overlayWin.MouseDown += overlayWin_MouseDown;
                    overlayWin.Closing += overlayWin_Closing;
                    Rect workingRectangle = SystemParameters.WorkArea;
                    overlayWin.Left = workingRectangle.Width - overlayWin.Width - 10;
                    overlayWin.Top = overlayWin.Height;
                   
                });
            }
        }
        /// <summary>
        /// 显示主窗口
        /// </summary>
        public ICommand menuItemActiveWinClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    WindowCollection WinCollection = Application.Current.Windows;
                    foreach (Window item in WinCollection)
                    {
                        if (item.Uid.Equals("mainWindow"))
                        {
                            item.WindowState = WindowState.Maximized;
                            item.Activate();
                            break;
                        }
                    }
                });
            }
        }

        public ICommand menuItemPluginListClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    VicGridNormal gridNormal = (VicGridNormal)overlayWin.FindName("girPluginList");
                    gridNormal.Children.Add(GetActivePluginInfo());
                    gridNormal.Children.Clear();
                    PluginListShow = true;
                    DisplayOverlayWindowPlugin overlay = new DisplayOverlayWindowPlugin();
                    overlay.Show();
                });
            }
        }
        public ICommand menuItemPCloseWinClickCommand
        {
            get {
                return new RelayCommand(() =>
                {

                    WindowCollection WinCollection = Application.Current.Windows;
                    foreach (Window item in WinCollection)
                    {
                        if (item.Name.Equals("overlayWindow"))
                        {
                            item.Hide();
                            break;
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 退出应用程序
        /// </summary>
        public ICommand menuItemExitClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    WindowCollection WinCollection = Application.Current.Windows;
                    foreach (Window item in WinCollection)
                    {
                        if (item.Uid.Equals("mainWindow"))
                        {
                            exitFlag = true;
                            item.Close();
                            break;
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void overlayWin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                overlayWin.DragMove();
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void overlayWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!exitFlag)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
        private VicStackPanelNormal GetActivePluginInfo()
        {
            DataMessageOperation dataop = new DataMessageOperation();
            List<Dictionary<string, object>> pluginList = dataop.GetPluginInfo();
            VicStackPanelNormal PluginListContent = new VicStackPanelNormal();
            PluginListContent.Orientation = Orientation.Vertical;
            PluginListContent.Width = 120;
            foreach (Dictionary<string, object> PluginInfo in pluginList)
            {
                VicButtonNormal btn = new VicButtonNormal();
                btn.Width = 120;
                IPlugin Plugin = PluginInfo["IPlugin"] as IPlugin;
                if (Plugin.ShowType.Equals(0))
                {
                    btn.Content = Plugin.PluginTitle;
                    btn.Tag = PluginInfo;
                    btn.Click += ActivatePlugin_Click;
                    PluginListContent.Children.Add(btn);
                }
            }
            return PluginListContent;
        }

        private void ActivatePlugin_Click(object sender, RoutedEventArgs e)
        {
            VicButtonNormal btn = sender as VicButtonNormal;
            Dictionary<string, object> pluginInfo = (Dictionary<string, object>)btn.Tag;
            IPlugin Plugin = pluginInfo["IPlugin"] as IPlugin;
            string PluginUid = pluginInfo["ObjectId"].ToString();
            bool pluginExistFlag = false;
            try
            {
                if (Plugin.ShowType == 0)//窗口
                {
                    WindowCollection WinCollection = Application.Current.Windows;

                    for (int i = 0; i < WinCollection.Count; i++)
                    {
                        if (WinCollection[i].Uid.Equals(PluginUid))
                        {
                            //switch (WinCollection[i].ResizeMode)
                            //{
                            //    case ResizeMode.NoResize:
                            //    case ResizeMode.CanMinimize:
                            //        WinCollection[i].WindowState = WindowState.Normal;
                            //        break;
                            //    case ResizeMode.CanResize:
                            //    case ResizeMode.CanResizeWithGrip:
                            //        WinCollection[i].WindowState = WindowState.Maximized;
                            //        break;
                            //}
                            WinCollection[i].Activate();
                            pluginExistFlag = true;
                            break;
                        }
                    }
                    if (!pluginExistFlag)
                    {
                        MessageBoxResult result = VicMessageBoxNormal.Show("插件不可用，是否卸载？", "提醒", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            DataMessageOperation dataOp = new DataMessageOperation();
                            dataOp.StopPlugin(PluginUid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("活动插件激活异常:{0}", ex.Message);
            }
        }
    }
}
