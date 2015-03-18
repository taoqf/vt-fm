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
using MetroFramePlugin.Models;
using MetroFramePlugin.Views;

namespace MetroFramePlugin.ViewModels
{
   public class DisplayOverlayWindowPluginViewModel:ModelBase
   {
       #region 字段
       private Window displayOverlayWindow;
       private ObservableCollection<MenuModel> systemFourthLevelMenuList;
       private Grid grid;
       /// <summary>
       /// 存储插件信息
       /// </summary>
       private List<Dictionary<string, object>> pluginList;
       private ObservableCollection<VicTabItemNormal> tabItemList;
       private string tPid;
       #endregion

       #region 属性
       /// <summary>
       /// 活动插件数目
       /// </summary>
       private long activePluginNum;
       public long ActivePluginNum
       {
           get
           {
               return activePluginNum;
           }
           set
           {
               if (activePluginNum != value)
               {
                   activePluginNum = value;
                   RaisePropertyChanged("ActivePluginNum");
               }
           }
       }
       public ObservableCollection<MenuModel> SystemFourthLevelMenuList
       {
           get
           {
               if (systemFourthLevelMenuList == null)
                   systemFourthLevelMenuList = new ObservableCollection<MenuModel>();
               return systemFourthLevelMenuList;
           }
           set
           {
               if (systemFourthLevelMenuList != value)
               {
                   systemFourthLevelMenuList = value;
                   RaisePropertyChanged("SystemFourthLevelMenuList");
               }
           }
       }
       #endregion

       #region 命令
       public ICommand displayOverlayWindowPluginCommand
       {
           get {
               return new RelayCommand<object>((x) =>
               {
                   displayOverlayWindow = (Window)x;
                   grid =(Grid) displayOverlayWindow.FindName("grid");
                   //获取插件信息
                   DataMessageOperation dataop = new DataMessageOperation();
                   pluginList = dataop.GetPluginInfo();
                   foreach (Dictionary<string, object> PluginInfo in pluginList)
                   {
                       IPlugin Plugin = PluginInfo["IPlugin"] as IPlugin;
                       string pid = PluginInfo["ObjectId"].ToString();
                       MenuModel menumodel = new MenuModel();
                       if (Plugin.ShowType == 0)//窗口
                       {

                           if (Plugin.ParamDict.ContainsKey("Title"))
                           {
                               if (Plugin.ParamDict["Title"].ToString().Length > 10)
                               {
                                   menumodel.MenuName = Plugin.ParamDict["Title"].ToString().Substring(0, 10) + "...";
                                   
                               }
                               else
                               {
                                   menumodel.MenuName = Plugin.ParamDict["Title"].ToString();
                               }
                               SystemFourthLevelMenuList.Add(menumodel);
                           }
                       }
                       else
                       {
                           if (Plugin.PluginTitle.Length >= 10)
                           {
                               menumodel.MenuName = Plugin.PluginTitle.ToString().Substring(0, 10) + "...";
                               menumodel.ActionType = Plugin.ShowType.ToString();
                               menumodel.ResourceName = Plugin.PluginName;
                               menumodel.Uid = PluginInfo["ObjectId"].ToString();
                               menumodel.ShowType = Plugin.ShowType.ToString();
                           }
                           else
                           {
                               menumodel.MenuName = Plugin.PluginTitle;
                               menumodel.ActionType = Plugin.ShowType.ToString();
                               menumodel.ResourceName = Plugin.PluginName;
                               menumodel.Uid = PluginInfo["ObjectId"].ToString();
                               menumodel.ShowType = Plugin.ShowType.ToString();
                           }
                           SystemFourthLevelMenuList.Add(menumodel);
                       }
                       
                   }
                    //<ListBox Grid.Row="1" Name="listBoxOverPlugin" SelectedIndex="0" Style="{DynamicResource OverlayPluginListStyle}" ItemsSource="{Binding SystemFourthLevelMenuList,UpdateSourceTrigger=PropertyChanged}"/>
                   for (int i = 0; i < SystemFourthLevelMenuList.Count; i++)
                   {
                       ListBox lbox = new ListBox();
                       lbox.ItemsSource = SystemFourthLevelMenuList;
                      
                       lbox.Style = displayOverlayWindow.FindResource("OverlayPluginListStyle") as Style;
                       grid.Children.Add(lbox);
                   }
                  
               });
           }
       }

       #region 单击插件运行命令
       public ICommand btnPluginRunClickCommand
       {
           get
           {
               return new RelayCommand<object>((x) =>
               {
                   if (x != null)
                   {
                       MenuModel menuModel = (MenuModel)x;
                       
                       VicTabControlNormal tabCtr = OverlayWindow.VicTabCtrl;
                       for (int i = 0; i < tabCtr.Items.Count; i++)
                       {
                           VicTabItemNormal tabItem = tabCtr.Items[i] as VicTabItemNormal;
                           string res = tabItem.Uid;
                           if (tabItem.Uid.Equals(menuModel.Uid))
                           {
                               tabItem.IsSelected = true;
                               tabItem.Focus();
                           }
                       }
                   }
               });
           }
       }
       #endregion

       #region 打开Json菜单下的插件
       /// <summary>
       /// 打开Json菜单下的插件
       /// </summary>
       private void OpenJsonMenuPlugin(MenuModel selectedFourthMenu)
       {
           if (TabItemList.FirstOrDefault(it => it.Header.Equals(selectedFourthMenu.MenuName)) != null)
           {
               TabItemList.FirstOrDefault(it => it.Header.Equals(selectedFourthMenu.MenuName)).IsSelected = true;
               return;
           }
           if (selectedFourthMenu.ResourceName != null)
           {
               if (selectedFourthMenu.ActionType == "1")//启动插件
               {
                   DataMessageOperation pluginOp = new DataMessageOperation();
                   Dictionary<string, object> paramDic = new Dictionary<string, object>();
                   paramDic.Add("systemid", selectedFourthMenu.BzSystemId);
                   paramDic.Add("configsystemid", selectedFourthMenu.ConfigSystemId);
                   paramDic.Add("spaceid", selectedFourthMenu.SpaceId);
                   paramDic.Add("menuno", selectedFourthMenu.MenuNo);
                   paramDic.Add("menucode", selectedFourthMenu.MenuCode);
                   // paramDic.Add("authoritycode", selectedFourthMenu.HomeId);
                   paramDic.Add("fitdata", selectedFourthMenu.FitDataPath);
                   paramDic.Add("cadname", selectedFourthMenu.ActionCADName);
                   PluginModel pluginModel = pluginOp.StratPlugin(selectedFourthMenu.ResourceName, paramDic);
                   if (string.IsNullOrEmpty(pluginModel.ErrorMsg))
                   {
                       PluginShow(pluginModel, selectedFourthMenu.MenuName);
                   }
                   else
                   {
                       VicMessageBoxNormal.Show(pluginModel.ErrorMsg);
                       return;
                   }
               }
           }
       }
       #endregion

       #region 加载插件
       private void PluginShow(PluginModel pluginModel, string HeaderTitle = null)
       {
           try
           {
               DataMessageOperation pluginOp = new DataMessageOperation();
               switch (pluginModel.PluginInterface.ShowType)
               {
                   case 0:
                       Window pluginWin = pluginModel.PluginInterface.StartWindow;
                       pluginWin.Uid = pluginModel.ObjectId;
                       //pluginWin.Owner = mainWindow;
                       //ActivePluginNum = pluginOp.GetPluginInfo().Count;
                       pluginWin.Show();
                       displayOverlayWindow.WindowState = WindowState.Minimized;
                       //SendPluginCloseMessage(pluginModel);
                       break;
                   case 1:
                       UserControl pluginCtrl = pluginModel.PluginInterface.StartControl;
                       pluginCtrl.Uid = pluginModel.ObjectId;
                       VicTabItemNormal tabItem = new VicTabItemNormal();
                       tabItem.Name = pluginModel.AppId;
                       tabItem.Header = string.IsNullOrEmpty(HeaderTitle) ? pluginModel.PluginInterface.PluginTitle : HeaderTitle;
                       tabItem.Uid = pluginModel.ObjectId;
                       tabItem.Content = pluginCtrl;
                       tabItem.AllowDelete = true;
                       tabItem.IsSelected = true;
                       TabItemList.Add(tabItem);
                       break;
                   default:
                       break;
               }
               //ActivePluginNum = pluginOp.GetPluginInfo().Count;
           }
           catch (Exception ex)
           {
               VicMessageBoxNormal.Show(ex.Message);
           }
       }

       #endregion
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
