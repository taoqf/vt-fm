using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using Victop.Wpf.Controls;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.DataMessageManager;
using Victop.Server.Controls;
using Victop.Frame.PublicLib.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MetroFramePlugin.ViewModels
{
   public class DisplayOverlayWindowPluginViewModel:ModelBase
   {
       #region 字段
       private Window displayOverlayWindow;
       /// <summary>
       /// 存储插件信息
       /// </summary>
       private List<Dictionary<string, object>> pluginList;
       private ObservableCollection<VicTabItemNormal> tabItemList;
       
       #endregion

       #region 属性
       #endregion

       #region 命令
       public ICommand displayOverlayWindowPluginCommand
       {
           get {
               return new RelayCommand<object>((x) =>
               {
                   displayOverlayWindow = (Window)x;
                   VicWrapPanelNormal PluginListContent = new VicWrapPanelNormal();
                   //获取插件信息
                   DataMessageOperation dataop = new DataMessageOperation();
                   pluginList = dataop.GetPluginInfo();
                   foreach (Dictionary<string, object> PluginInfo in pluginList)
                   {
                       VicButtonNormal btn = new VicButtonNormal();
                       btn.Width = 160;
                       btn.Height = 120;
                       Thickness thick = new Thickness(10);
                       btn.Margin = thick;
                       IPlugin Plugin = PluginInfo["IPlugin"] as IPlugin;
                       if (Plugin.ShowType == 0)
                       {

                           if (Plugin.ParamDict.ContainsKey("Title"))
                           {
                               if (Plugin.ParamDict["Title"].ToString().Length > 8)
                               {
                                   btn.Content = Plugin.ParamDict["Title"].ToString().Substring(0, 6) + "...";
                                   btn.ToolTip = Plugin.ParamDict["Title"];
                               }
                               else
                               {
                                   btn.Content = Plugin.ParamDict["Title"];
                               }
                           }
                           else
                               btn.Content = Plugin.PluginTitle;
                           btn.Tag = PluginInfo;
                           btn.Click += ActivatePlugin_Click;
                           PluginListContent.Children.Add(btn);
                       }
                       else
                       {
                           btn.Content = Plugin.PluginTitle;
                           btn.Tag = PluginInfo;
                           btn.Click += ActivatePlugin_Click;
                           PluginListContent.Children.Add(btn);
                       }
                       displayOverlayWindow.Content = PluginListContent;
                   }
               });
           }
       }
       #endregion

       #region 私方法
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
                           switch (WinCollection[i].ResizeMode)
                           {
                               case ResizeMode.NoResize:
                               case ResizeMode.CanMinimize:
                                   WinCollection[i].WindowState = WindowState.Normal;
                                   break;
                               case ResizeMode.CanResize:
                               case ResizeMode.CanResizeWithGrip:
                                   WinCollection[i].WindowState = WindowState.Maximized;
                                   break;
                           }
                           WinCollection[i].Activate();
                           pluginExistFlag = true;
                           break;
                       }
                   }
               //    if (!pluginExistFlag)
               //    {
               //        MessageBoxResult result = VicMessageBoxNormal.Show("插件不可用，是否卸载？", "提醒", MessageBoxButton.YesNo, MessageBoxImage.Question);
               //        if (result == MessageBoxResult.Yes)
               //        {
               //            DataMessageOperation dataOp = new DataMessageOperation();
               //            dataOp.StopPlugin(PluginUid);
               //        }
               //    }
               }
               else
               {
                   this.displayOverlayWindow.WindowState = WindowState.Minimized;
                   VicTabItemNormal tabItem = TabItemList.FirstOrDefault(it => it.Uid.Equals(PluginUid));
                   if (tabItem != null)
                   {
                       tabItem.IsSelected = true;
                       tabItem.Focus();
                   }
               }
           }
           catch (Exception ex)
           {
               LoggerHelper.ErrorFormat("活动插件激活异常:{0}", ex.Message);
           }
       }

       /// <summary>选项卡集合 </summary>
       public ObservableCollection<VicTabItemNormal> TabItemList
       {
           get
           {
               if (tabItemList == null)
               {
                   tabItemList = new ObservableCollection<VicTabItemNormal>();
                   VicTabItemNormal homeItem = new VicTabItemNormal();
                   homeItem.Name = "homeItem";
                   homeItem.AllowDelete = false;
                   homeItem.Header = "飞道科技";
                   WebBrowser browser = new WebBrowser();
                   //browser.Source = new Uri("http://www.baidu.com");
                   homeItem.Content = browser;
                   tabItemList.Add(homeItem);
               }
               return tabItemList;
           }
           set
           {
               if (tabItemList != value)
               {
                   tabItemList = value;
                   RaisePropertyChanged("TabItemList");
               }
           }
       }
       #endregion
   }
}
