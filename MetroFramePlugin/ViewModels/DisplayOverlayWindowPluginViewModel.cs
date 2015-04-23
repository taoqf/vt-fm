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
using Victop.Frame.Units;


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
       /// 左翻页
       /// </summary>
       private Image imageLeft;
       /// <summary>
       /// 右翻页
       /// </summary>
       private Image imageRight;
       /// <summary>
       /// 内容容器
       /// </summary>
       private Canvas canvasPageContent;
       private RectangleGeometry canvasPageRectangle;
       private WrapPanel wrapPanelPages;
       private UnitPageBar pageBar1;

       /// <summary>
       /// 存储插件信息
       /// </summary>
       private List<Dictionary<string, object>> pluginList;
       private ObservableCollection<VicTabItemNormal> tabItemList;
       private string tPid;
       private int pageCount;
       private int totalPage;
       private int pageSize =8;
       private int currentPage = 1;
       private VicTabControlNormal tabCtr;
       #endregion

       #region 分页动画属性

       int pageSelect = 0;

       bool isDown = false;
       double down_pX = 0;
       double down_pY = 0;
       bool isMoveSure = false;
       double oldX = 0;

       bool isInMove = false;
       object changeLock = new object();
       System.Windows.Media.Animation.Storyboard sboard;

       System.Windows.Media.Animation.Storyboard sboardLeftIamge;
       System.Windows.Media.Animation.Storyboard sboardRightIamge;
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
       private bool nextPageVis;
       public bool NextPageVis
       {
           get
           {
               return nextPageVis;
           }
           set
           {
               if (nextPageVis != value)
               {
                   nextPageVis = value;
                   RaisePropertyChanged("NextPageVis");
               }
           }
       }

       private bool upPageVis;
       public bool UpPageVis
       {
           get
           {
               return upPageVis;
           }
           set
           {
               if (upPageVis != value)
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
           get
           {
               return new RelayCommand<object>((x) =>
               {
                   displayOverlayWindow = (Window)x;
                   imageLeft = (Image)displayOverlayWindow.FindName("imageLeft");
                   imageRight = (Image)displayOverlayWindow.FindName("imageRight");
                   wrapPanelPages = (WrapPanel)displayOverlayWindow.FindName("wrapPanelPages");
                   canvasPageContent = (Canvas)displayOverlayWindow.FindName("canvasPageContent");
                   canvasPageRectangle = (RectangleGeometry)displayOverlayWindow.FindName("canvasPageRectangle");
                   tabCtr = OverlayWindow.VicTabCtrl;
                   statePanel = (VicStackPanelNormal)displayOverlayWindow.FindName("statePanel");
                   pageBar1 = (UnitPageBar)displayOverlayWindow.FindName("pageBar1");
                   sboardLeftIamge = (System.Windows.Media.Animation.Storyboard)displayOverlayWindow.FindResource("StoryboardLeftImage");
                   sboardRightIamge = (System.Windows.Media.Animation.Storyboard)displayOverlayWindow.FindResource("StoryboardRightImage");
                   #region 分屏按钮事件
                   imageLeft.MouseEnter += imageLeft_MouseEnter;
                   imageLeft.MouseLeave += imageLeft_MouseLeave;
                   imageLeft.MouseLeftButtonDown += imageLeft_MouseLeftButtonDown;
                   imageRight.MouseEnter += imageRight_MouseEnter;
                   imageRight.MouseLeave += imageRight_MouseLeave;
                   imageRight.MouseLeftButtonDown += imageRight_MouseLeftButtonDown;
                   #endregion
                   #region 内容容器事件
                   canvasPageContent.PreviewMouseLeftButtonDown += canvasPageContent_PreviewMouseLeftButtonDown;
                   canvasPageContent.PreviewMouseLeftButtonUp += canvasPageContent_PreviewMouseLeftButtonUp;
                   canvasPageContent.PreviewMouseMove += canvasPageContent_PreviewMouseMove;
                   canvasPageContent.MouseLeave += canvasPageContent_MouseLeave;
                   canvasPageContent.SizeChanged += canvasPageContent_SizeChanged;
                   #endregion
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
                           menumodel.MenuName = Plugin.PluginTitle;
                           menumodel.ActionType = Plugin.ShowType.ToString();
                          // menumodel.ResourceName = Plugin.PluginName;
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
                         //      menumodel.ResourceName = Plugin.PluginName;
                               menumodel.Uid = PluginInfo["ObjectId"].ToString();
                               menumodel.ShowType = Plugin.ShowType.ToString();
                           }
                           else
                           {
                               menumodel.MenuName = Plugin.PluginTitle;
                               menumodel.ActionType = Plugin.ShowType.ToString();
                            //   menumodel.ResourceName = Plugin.PluginName;
                               menumodel.Uid = PluginInfo["ObjectId"].ToString();
                               menumodel.ShowType = Plugin.ShowType.ToString();
                           }
                           SystemFourthLevelMenuList.Add(menumodel);
                       }
                   }
                   if (SystemFourthLevelMenuList.Count == 0)
                   {
                       VicLabelNormal lbl = new VicLabelNormal();
                       lbl.Content = "暂无打开的活动插件";
                       lbl.Foreground = Brushes.Red;
                       wrapPanelPages.Children.Add(lbl);
                       return;
                   }

                   totalPage = SystemFourthLevelMenuList.Count / pageSize;
                   if ((SystemFourthLevelMenuList.Count % pageSize) == 0)
                   {
                       totalPage = SystemFourthLevelMenuList.Count / pageSize; //正好8项是1页
                   }
                   else
                   {
                       totalPage = SystemFourthLevelMenuList.Count / pageSize + 1; ;// 非8项，
                   }

                   //for (int i = 0; i < SystemFourthLevelMenuList.Count; i++)
                   //{
                   //    ListBox lbox = new ListBox();
                   //    lbox.ItemsSource = SystemFourthLevelMenuList;
                   //    lbox.SetResourceReference(ListBox.StyleProperty, "OverlayPluginListStyle");
                   //    statePanel.Children.Add(lbox);

                   //}
                   setInit(totalPage);
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

                       totalPage = SystemFourthLevelMenuList.Count / pageSize;
                       if ((SystemFourthLevelMenuList.Count % pageSize) == 0)
                       {
                           totalPage = SystemFourthLevelMenuList.Count / pageSize; //正好8项是1页
                       }
                       else
                       {
                           totalPage = SystemFourthLevelMenuList.Count / pageSize + 1; ;// 非8项，
                       }


                       //statePanel.Children.Clear();
                       //setPage(totalPage);


                       wrapPanelPages.Children.Clear();
                       imageLeft.Visibility = System.Windows.Visibility.Hidden;
                       imageRight.Visibility = System.Windows.Visibility.Hidden;

                       setInit(totalPage);
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
                       UpPageVis = false;

                       if (SystemFourthLevelMenuList.Count <= pageSize)
                       {
                           NextPageVis = false;
                       }

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
                               currentPageList.Add(SystemFourthLevelMenuList[(((currentPage - 2) * pageSize) + i)]);

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
                       //判断上一页按钮是否有效
                       if (currentPage <= 1)
                       {
                           UpPageVis = false;
                       }
                       else
                       {
                           UpPageVis = true;
                       }
                       //判断下一页按钮是否有效
                       if (currentPage < totalPage)
                       {
                           NextPageVis = true;
                       }
                       else
                       {
                           NextPageVis = false;
                       }

                   }
               });
           }
       }

       /// <summary>
       /// 下一页
       /// </summary>
       public ICommand btnNextPageClickCommand
       {
           get
           {
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
                   if (currentPage >= totalPage)
                   {
                       NextPageVis = false;
                   }
                   else
                   {
                       NextPageVis = true;
                   }
                   UpPageVis = true;

               });
           }
       }
       #endregion


       #region 打开Json菜单下的插件
       /// <summary>
       /// 打开Json菜单下的插件
       /// </summary>
    
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

       #region 无动画效果方法
       private void setPage(int totalPage)
       {
           ObservableCollection<MenuModel> sumPageList = new ObservableCollection<MenuModel>();
           for (int i = 0; i < SystemFourthLevelMenuList.Count; i++)
           {
               sumPageList.Add(SystemFourthLevelMenuList[i]);
           }
           int articleWindowCount = sumPageList.Count;
           UpPageVis = false;
           if (articleWindowCount <= pageSize)
           {
               NextPageVis = false;
           }
           else
           {
               NextPageVis = true;
           }
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

       #region 添加分屏动画私方法
       private void setInit(int totalPage)
       {
           //设置wrapPanelPages宽度
           wrapPanelPages.Width = canvasPageContent.ActualWidth * totalPage;

           ObservableCollection<MenuModel> sumPageList = new ObservableCollection<MenuModel>();
           for (int i = 0; i < SystemFourthLevelMenuList.Count; i++)
           {
               sumPageList.Add(SystemFourthLevelMenuList[i]);
           }
           int articleWindowCount = sumPageList.Count;

           for (int i = 0; i < totalPage; i++)
           {
               WrapPanel rectangle = new WrapPanel();
               rectangle.Orientation = Orientation.Vertical;
               rectangle.Width = 800;
               rectangle.Height = 350;

               ObservableCollection<MenuModel> currentPageList = new ObservableCollection<MenuModel>();
               ListBox lbox = new ListBox();
               if (articleWindowCount > 0)
               {
                   foreach (var menuModel in sumPageList.Skip(i * pageSize).Take(pageSize).ToList<MenuModel>())
                   {
                       if (menuModel != null)
                       {
                           currentPageList.Add(menuModel);
                       }
                   }
               }
               lbox.ItemsSource = currentPageList;
               lbox.SetResourceReference(ListBox.StyleProperty, "OverlayPluginListStyle");
               rectangle.Children.Add(lbox);

               Viewbox viewbox = new Viewbox();
               viewbox.Stretch = Stretch.Fill;
               viewbox.Width = canvasPageContent.ActualWidth;
               viewbox.Height = canvasPageContent.ActualHeight;
               viewbox.Child = rectangle;
               wrapPanelPages.Children.Add(viewbox);

               //wrapPanelPages.Children.Add(rectangle);
           }

           ////改变页
           //if (defaultPageNum > 0)
           //    pageSelect = defaultPageNum;
           if (pageSelect > totalPage)
               pageSelect = totalPage;
           if (pageSelect == 0)
               pageSelect = 1;
           Canvas.SetLeft(wrapPanelPages, -canvasPageContent.ActualWidth * (pageSelect - 1));
           //改变按钮状态
           ChangeButtonStatus();
           pageBar1.CreatePageEllipse(totalPage);
           pageBar1.SelectPage(pageSelect);
           pageBar1.Visibility = System.Windows.Visibility.Visible;


       }


       #region 辅助方法
       #region 翻页实现
       /// <summary>
       /// 翻页实现
       /// </summary>
       /// <param name="page"></param>
       /// <param name="needAnimation"></param>
       private void ChangePage(bool isRight)
       {
           double pageWidth = canvasPageContent.ActualWidth;
           lock (changeLock)
           {
               if (isInMove)
                   return;
               isInMove = true;
           }
           if (isRight)
           {
               if (pageSelect == totalPage)
               {
                   lock (changeLock)
                   {
                       isInMove = false;
                   }
                   return;
               }
               else
               {

                   isInMove = true;
                   double listLeft_now = Canvas.GetLeft(wrapPanelPages);
                   double listLeft_sur = -(pageSelect - 1) * pageWidth;
                   double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                   double toX = -pageWidth;
                   double time = 1000 * Math.Abs(Math.Abs(toX) - Math.Abs(formX)) / pageWidth;
                   sboardRightBegin(formX, toX, time);
               }
           }
           else
           {
               if (pageSelect == 1)
               {
                   lock (changeLock)
                   {
                       isInMove = false;
                   }
                   return;
               }
               else
               {
                   isInMove = true;
                   double listLeft_now = Canvas.GetLeft(wrapPanelPages);
                   double listLeft_sur = -(pageSelect - 1) * pageWidth;
                   //启动左翻动画-翻页
                   double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                   double toX = pageWidth;
                   double time = 1000 * Math.Abs(Math.Abs(toX) - Math.Abs(formX)) / pageWidth;
                   sboardLeftBegin(formX, toX, time);
               }
           }
       }
       #endregion

       #region 改变翻页按钮状态
       /// <summary>
       /// 改变翻页按钮状态
       /// </summary>
       private void ChangeButtonStatus()
       {
           if (totalPage == 0)
           {
               return;
           }
           if (pageSelect == 1 && totalPage == 1)
           {
               imageLeft.Visibility = System.Windows.Visibility.Hidden;
               imageRight.Visibility = System.Windows.Visibility.Hidden;
           }
           else if (pageSelect == 1 && totalPage > 1)
           {
               imageLeft.Visibility = System.Windows.Visibility.Hidden;
               imageRight.Visibility = System.Windows.Visibility.Visible;
           }
           else
           {
               if (pageSelect == totalPage)
               {
                   imageLeft.Visibility = System.Windows.Visibility.Visible;
                   imageRight.Visibility = System.Windows.Visibility.Hidden;
               }
               else
               {
                   imageLeft.Visibility = System.Windows.Visibility.Visible;
                   imageRight.Visibility = System.Windows.Visibility.Visible;
               }
           }
       }
       #endregion

       #region 拖拽实现

       private void canvasPageContent_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
           isDown = true;
           System.Windows.Point position = e.GetPosition(canvasPageContent);
           down_pX = position.X;
           down_pY = position.Y;
           oldX = down_pX;
       }

       private void canvasPageContent_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
       {
           if (isDown)
           {
               e.Handled = isMoveSure;
               changePos();
           }
       }

       private void canvasPageContent_PreviewMouseMove(object sender, MouseEventArgs e)
       {
           if (isDown)
           {
               System.Windows.Point position = e.GetPosition(canvasPageContent);
               Canvas.SetLeft(wrapPanelPages, Canvas.GetLeft(wrapPanelPages) + (position.X - oldX));
               oldX = position.X;

               if (Math.Abs(down_pX - position.X) > 150)
               {
                   isMoveSure = true;
               }
           }
       }

       private void canvasPageContent_MouseLeave(object sender, MouseEventArgs e)
       {
           if (isDown)
           {
               changePos();
           }
       }

       private void changePos()
       {
           double pageWidth = canvasPageContent.ActualWidth;
           isDown = false;
           isMoveSure = false;
           double listLeft_now = Canvas.GetLeft(wrapPanelPages);
           double listLeft_sur = -(pageSelect - 1) * pageWidth;
           //右翻页动作
           if (listLeft_now < listLeft_sur)
           {
               if (pageSelect == totalPage)
               {
                   //已经达到最大页面-回滚
                   isInMove = true;
                   double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                   double toX = 0;
                   double time = 1000 * Math.Abs(formX) / pageWidth;
                   Canvas.SetLeft(wrapPanelPages, listLeft_sur);
                   sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
                   sboard.Completed += sboardNoChange_Completed;
                   sboard.Begin();
               }
               else
               {
                   bool SureRight = false;
                   //移动距离
                   double dis = Math.Abs(listLeft_now - listLeft_sur);
                   //达到翻页确认标准
                   if (dis >= 150)
                       SureRight = true;
                   if (SureRight)
                   {
                       //启动右翻动画-翻页
                       isInMove = true;
                       double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                       double toX = -pageWidth;
                       double time = 1000 * Math.Abs(Math.Abs(toX) - Math.Abs(formX)) / pageWidth;
                       Canvas.SetLeft(wrapPanelPages, listLeft_sur);
                       sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
                       sboard.Completed += sboardRight_Completed;
                       sboard.Begin();
                   }
                   else
                   {
                       //未达到翻页要求-回滚
                       isInMove = true;
                       double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                       double toX = 0;
                       double time = 1000 * Math.Abs(formX) / pageWidth;
                       Canvas.SetLeft(wrapPanelPages, listLeft_sur);
                       sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
                       sboard.Completed += sboardNoChange_Completed;
                       sboard.Begin();
                   }
               }
           }
           //左翻页确认
           else
           {
               //第一页左翻-回滚
               if (pageSelect == 1)
               {
                   isInMove = true;
                   double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                   double toX = 0;
                   double time = 1000 * Math.Abs(formX) / pageWidth;
                   Canvas.SetLeft(wrapPanelPages, listLeft_sur);
                   sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
                   sboard.Completed += sboardNoChange_Completed;
                   sboard.Begin();
               }
               else
               {
                   //确认翻页参数
                   bool SureLeft = false;
                   //移动距离
                   double dis = Math.Abs(listLeft_now - listLeft_sur);
                   //达到翻页确认标准
                   if (dis >= 150)
                       SureLeft = true;
                   if (SureLeft)
                   {
                       isInMove = true;
                       //启动左翻动画-翻页
                       double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                       double toX = pageWidth;
                       double time = 1000 * Math.Abs(Math.Abs(toX) - Math.Abs(formX)) / pageWidth;
                       Canvas.SetLeft(wrapPanelPages, listLeft_sur);
                       sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
                       sboard.Completed += sboardLeft_Completed;
                       sboard.Begin();
                   }
                   else
                   {
                       isInMove = true;
                       //未达到翻页要求-回滚
                       double formX = listLeft_now + (pageSelect - 1) * pageWidth;
                       double toX = 0;
                       double time = 1000 * Math.Abs(formX) / pageWidth;
                       Canvas.SetLeft(wrapPanelPages, listLeft_sur);
                       sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
                       sboard.Completed += sboardNoChange_Completed;
                       sboard.Begin();
                   }
               }
           }
       }
       #endregion

       #endregion





       #region 分屏按钮方法
       private void imageRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
           sboardRightIamge.Begin();
           ChangePage(true);
       }

       private void imageRight_MouseLeave(object sender, MouseEventArgs e)
       {
           imageRight.Effect = null;
       }

       private void imageRight_MouseEnter(object sender, MouseEventArgs e)
       {
           imageRight.Effect = new System.Windows.Media.Effects.DropShadowEffect();
       }

       private void imageLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
           sboardLeftIamge.Begin();
           ChangePage(false);
       }

       private void imageLeft_MouseLeave(object sender, MouseEventArgs e)
       {
           imageLeft.Effect = null;
       }

       private void imageLeft_MouseEnter(object sender, MouseEventArgs e)
       {
           imageLeft.Effect = new System.Windows.Media.Effects.DropShadowEffect();
       }

       #endregion


       #region 动画处理

       //左翻动画
       private void sboardLeftBegin(double formX, double toX, double time)
       {
           sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
           sboard.Completed += sboardLeft_Completed;
           sboard.Begin();
       }

       //右翻动画
       private void sboardRightBegin(double formX, double toX, double time)
       {
           sboard = ShareClass.CeaterAnimation_Xmove(wrapPanelPages, formX, toX, time, 0);
           sboard.Completed += sboardRight_Completed;
           sboard.Begin();
       }

       //翻页回滚结束
       private void sboardNoChange_Completed(object sender, EventArgs e)
       {
           lock (changeLock)
           {
               isInMove = false;
           }
       }

       //右翻页结束
       private void sboardRight_Completed(object sender, EventArgs e)
       {
           pageSelect++;
           pageBar1.SelectPage(pageSelect);
           sboard.Stop();
           ChangeButtonStatus();
           Canvas.SetLeft(wrapPanelPages, -(pageSelect - 1) * canvasPageContent.ActualWidth);
           lock (changeLock)
           {
               isInMove = false;
           }
       }

       //左翻页结束
       private void sboardLeft_Completed(object sender, EventArgs e)
       {
           pageSelect--;
           pageBar1.SelectPage(pageSelect);
           sboard.Stop();
           ChangeButtonStatus();
           Canvas.SetLeft(wrapPanelPages, -(pageSelect - 1) * canvasPageContent.ActualWidth);
           lock (changeLock)
           {
               isInMove = false;
           }
       }
       #endregion

       #region 页面大小变化
       //页面大小变化，修改canvasPageContent的裁剪范围
       private void canvasPageContent_SizeChanged(object sender, SizeChangedEventArgs e)
       {
           canvasPageRectangle.Rect = new Rect(0, 0, canvasPageContent.ActualWidth, canvasPageContent.ActualHeight);
       }
       #endregion

       #endregion
       #endregion
   }
}
