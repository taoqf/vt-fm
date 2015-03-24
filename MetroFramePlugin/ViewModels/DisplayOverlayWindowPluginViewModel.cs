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
using System.Windows.Media;


namespace MetroFramePlugin.ViewModels
{
   public class DisplayOverlayWindowPluginViewModel:ModelBase
   {
       #region 字段
       private Window displayOverlayWindow;
       private ObservableCollection<MenuModel> systemFourthLevelMenuList;
       private Grid grid;
       private VicStackPanelNormal statePanel;
       /// <summary>
       /// 存储插件信息
       /// </summary>
       private List<Dictionary<string, object>> pluginList;
       private ObservableCollection<VicTabItemNormal> tabItemList;
       private string tPid;
       private int pageCount;
       private int totalPage;
       private int pageSize =1;
       private int currentPage=1;
       private VicTabControlNormal tabCtr;
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
       private Visibility nextPageVis;
       public Visibility NextPageVis
       {
           get 
           {
               return nextPageVis;
           }
           set 
           { 
            if(nextPageVis!=value)
            {
                nextPageVis = value;
                RaisePropertyChanged("NextPageVis");
            }
          }
       }

       private Visibility upPageVis;
       public Visibility UpPageVis
       {
           get {
               return upPageVis;
           }
           set { 
            if(upPageVis!=value)
            {
                upPageVis = value;
                RaisePropertyChanged("UpPageVis");
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
                   tabCtr = OverlayWindow.VicTabCtrl;
                   statePanel = (VicStackPanelNormal)displayOverlayWindow.FindName("statePanel");                   //获取插件信息
                   DataMessageOperation dataop = new DataMessageOperation();
                   pluginList = dataop.GetPluginInfo();
                   foreach (Dictionary<string, object> PluginInfo in pluginList)
                   {
                       IPlugin Plugin = PluginInfo["IPlugin"] as IPlugin;
                       string pid = PluginInfo["ObjectId"].ToString();
                       MenuModel menumodel = new MenuModel();
                       if (Plugin.ShowType == 0)//窗口
                       {
                           menumodel.MenuName = Plugin.PluginTitle;
                           menumodel.ActionType = Plugin.ShowType.ToString();
                           menumodel.ResourceName = Plugin.PluginName;
                           menumodel.Uid = PluginInfo["ObjectId"].ToString();
                           menumodel.ShowType = Plugin.ShowType.ToString();
                           SystemFourthLevelMenuList.Add(menumodel);
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
                   if (SystemFourthLevelMenuList.Count == 0) return;

                   totalPage = SystemFourthLevelMenuList.Count / pageSize;
                   if ((SystemFourthLevelMenuList.Count % pageSize) == 0)
                   {
                       totalPage = SystemFourthLevelMenuList.Count / pageSize; //正好8项是1页
                   }
                   else
                   {
                       totalPage = SystemFourthLevelMenuList.Count / pageSize + 1; ;// 非8项，
                   }

                   setPage(totalPage);
                   if (SystemFourthLevelMenuList.Count <=0)
                   {
                       VicLabelNormal lbl = new VicLabelNormal();
                       lbl.Content = "暂无打开的活动插件";
                       lbl.Foreground = Brushes.Red;
                       grid.Children.Add(lbl);
                       //for (int i = 0; i < SystemFourthLevelMenuList.Count; i++)
                       //{
                       //    ListBox lbox = new ListBox();
                       //    lbox.ItemsSource = SystemFourthLevelMenuList;
                       //    lbox.SetResourceReference(ListBox.StyleProperty, "OverlayPluginListStyle");
                       //    statePanel.Children.Add(lbox);
                       
                       //}
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
                       
                       if (menuModel.ShowType == "0")//窗口
                       {
                           WindowCollection WinCollection = Application.Current.Windows;

                           for (int i = 0; i < WinCollection.Count; i++)
                           {
                               if (WinCollection[i].Uid.Equals(menuModel.Uid))
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
                                   break;
                               }
                           }
                           
                       }
                       else
                       {
                           WindowCollection WinCollection = Application.Current.Windows;
                           foreach (Window item in WinCollection)
                           {
                               if (item.Uid.Equals("mainWindow"))
                               {
                                   if (item.IsActive == false)
                                   {
                                       item.WindowState = WindowState.Maximized;
                                       item.Activate();
                                       break;
                                   }
                               }
                           }
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
                   }
               });
           }
       }
       /// <summary>
       /// 单击关闭
       /// </summary>
       public ICommand btnPluginCloseClickCommand
       {
           get
           {
               return new RelayCommand<object>((x) =>
               {
                   if (x != null)
                   {
                       MenuModel menuModel = (MenuModel)x;
                       for (int i = 0; i < SystemFourthLevelMenuList.Count; i++)
                       {
                           if (menuModel.MenuName.Equals(SystemFourthLevelMenuList[i].MenuName))
                           {
                               SystemFourthLevelMenuList.Remove(menuModel);
                           }
                       }
                       statePanel.Children.Clear();
                       setPage(totalPage);
                   }
               });
           }
       }
       /// <summary>
       /// 上一页
       /// </summary>
       public ICommand btnUpPageClickCommand
       {
           get
           {
               return new RelayCommand<object>((x) =>
               {
                   if (currentPage == 1)
                   {
                       return;
                   }
                   else if (currentPage > 1)
                   {

                       if (SystemFourthLevelMenuList.Count <= pageSize)
                       {

                       }
                       else
                       {
                           Grid gridPanel = new Grid();

                           gridPanel.Height = 300;
                           gridPanel.Margin = new Thickness(0, 20, 0, 20);
                           ObservableCollection<MenuModel> currentPageList = new ObservableCollection<MenuModel>();
                           for (int i = 0; i < pageSize; i++)
                           {
                               currentPageList.Add(SystemFourthLevelMenuList[(((currentPage - 2) * pageSize)+i)]);
                                
                           }
                           ListBox lbox = new ListBox();
                           lbox.ItemsSource = currentPageList;
                           lbox.SetResourceReference(ListBox.StyleProperty, "OverlayPluginListStyle");
                           gridPanel.Children.Add(lbox);
                           statePanel.Children.Insert(0, gridPanel);
                       }

                       //Grid gridPanel = new Grid();
                       //gridPanel.Height = 300;
                       //gridPanel.Background = Brushes.Red;
                       //gridPanel.Margin = new Thickness(0, 20, 0, 20);
                       //  ObservableCollection<MenuModel> currentPageList = new ObservableCollection<MenuModel>();
                       //  currentPageList.Add(SystemFourthLevelMenuList[currentPage - 2]);
                       //  ListBox lbox = new ListBox();
                       //  lbox.ItemsSource = currentPageList;
                       //  lbox.SetResourceReference(ListBox.StyleProperty, "OverlayPluginListStyle");
                       //  gridPanel.Children.Add(lbox);
                       //statePanel.Children.Insert(0,gridPanel);

                       currentPage--;

                   }
               });
           }
       }

       /// <summary>
       /// 下一页
       /// </summary>
       public ICommand btnNextPageClickCommand
       {
           get {
               return new RelayCommand<object>((x) =>
               {
                   if (currentPage == totalPage)
                   {
                       return;
                   }

                   //if (SystemFourthLevelMenuList.Count <= pageSize)
                   //{

                   //}
                   //else
                   //{
                   //    Grid gridPanel = new Grid();

                   //    gridPanel.Height = 300;
                   //    gridPanel.Background = Brushes.Red;
                   //    gridPanel.Margin = new Thickness(0, 20, 0, 20);

                   //    ObservableCollection<MenuModel> currentPageList = new ObservableCollection<MenuModel>();
                   //    for (int i = 0; i < pageSize; i++)
                   //    {
                   //        currentPageList.Add(SystemFourthLevelMenuList[(((currentPage - 1) * pageSize) + i)]);

                   //    }
                   //    ListBox lbox = new ListBox();
                   //    lbox.ItemsSource = currentPageList;
                   //    lbox.SetResourceReference(ListBox.StyleProperty, "OverlayPluginListStyle");
                   //    gridPanel.Children.Add(lbox);

                   //    statePanel.Children.Insert(0, gridPanel);
                   //}




                       if (statePanel.Children.Count > 0)
                       {
                           statePanel.Children.RemoveAt(0);
                       }

                       currentPage++;

                   
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
       private void setPage(int totalPage)
       {
           ObservableCollection<MenuModel> sumPageList = new ObservableCollection<MenuModel>();
           for (int i = 0; i < SystemFourthLevelMenuList.Count; i++)
           {
               sumPageList.Add(SystemFourthLevelMenuList[i]);
           }
           int articleWindowCount = sumPageList.Count;
           for (int i = 0; i < totalPage; i++)
           {
               Grid gridPanel = new Grid();
               gridPanel.Height = 300;
               gridPanel.Margin = new Thickness(0, 20, 0, 20);

               ObservableCollection<MenuModel> currentPageList = new ObservableCollection<MenuModel>();
               for (int k = 0; k < articleWindowCount; k++)
               {
                   ListBox lbox = new ListBox();
                   currentPageList.Add(sumPageList[0]);
                   sumPageList.RemoveAt(0);
                   articleWindowCount--;
                   k--;
                   lbox.ItemsSource = currentPageList;
                   lbox.SetResourceReference(ListBox.StyleProperty, "OverlayPluginListStyle");
                   gridPanel.Children.Add(lbox);
                   if (gridPanel.Children.Count == pageSize) { break; }
               }
               statePanel.Children.Add(gridPanel);
           }
           
           
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
