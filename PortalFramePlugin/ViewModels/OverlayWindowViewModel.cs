using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using PortalFramePlugin.Views;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace PortalFramePlugin.ViewModels
{
    public class OverlayWindowViewModel:ModelBase
    {
        private OverlayWindow overlayWin;
        private bool exitFlag = false;

        public ICommand mainWindowLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    overlayWin = (OverlayWindow)x;
                    overlayWin.MouseDown += overlayWin_MouseDown;
                    overlayWin.Closing += overlayWin_Closing;
                    Rect workingRectangle = SystemParameters.WorkArea;
                    overlayWin.Left = workingRectangle.Width - overlayWin.Width;
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
                return new RelayCommand(() => {
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
                return new RelayCommand(() => { 
                
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
                return new RelayCommand(() => {
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
    }
}
