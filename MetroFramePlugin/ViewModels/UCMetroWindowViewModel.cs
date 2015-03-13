﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml.Linq;
using GalaSoft.MvvmLight.Command;
using MetroFramePlugin.Models;
using MetroFramePlugin.Views;
using Victop.Frame.CoreLibrary;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace MetroFramePlugin.ViewModels
{
    public class UCMetroWindowViewModel : ModelBase
    {
        #region 字段
        private Window mainWindow;
        private VicButtonNormal btnPluginList;

        private Window win_PluginList;
        private List<Dictionary<string, object>> pluginList;
        private ObservableCollection<MenuModel> systemMenuListEnterprise;
        private ObservableCollection<MenuModel> systemMenuListLocal;

        private ObservableCollection<MenuModel> systemThirdLevelMenuList;
        private ObservableCollection<MenuModel> systemFourthLevelMenuList;

        private MenuModel selectedSecondMenuModel;
        private MenuModel selectedThirdMenuModel;

        private ObservableCollection<VicTabItemNormal> tabItemList;
        private VicTabItemNormal selectedTabItem;
        private VicTabControlNormal mainTabControl;
        /// <summary>是否首次登录 </summary>
        private bool isFirstLogin = true;
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;
        /// <summary>
        /// 用户头像
        /// </summary>
        private string userImg;
        /// <summary>
        /// 活动插件数目
        /// </summary>
        private long activePluginNum;
        /// <summary>
        /// 本地插件集合
        /// </summary>
        private ObservableCollection<MenuModel> localMenuListEx = new ObservableCollection<MenuModel>();
        #endregion

        #region 属性
        /// <summary>本地菜单列表 </summary>
        public ObservableCollection<MenuModel> SystemMenuListLocal
        {
            get
            {
                if (systemMenuListLocal == null)
                    systemMenuListLocal = new ObservableCollection<MenuModel>();
                return systemMenuListLocal;
            }
            set
            {
                if (systemMenuListLocal != value)
                {
                    systemMenuListLocal = value;
                    RaisePropertyChanged("SystemMenuListLocal");
                }
            }
        }

        /// <summary>企业菜单列表 </summary>
        public ObservableCollection<MenuModel> SystemMenuListEnterprise
        {
            get
            {
                if (systemMenuListEnterprise == null)
                    systemMenuListEnterprise = new ObservableCollection<MenuModel>();
                return systemMenuListEnterprise;
            }
            set
            {
                if (systemMenuListEnterprise != value)
                {
                    systemMenuListEnterprise = value;
                    RaisePropertyChanged("SystemMenuListEnterprise");
                }
            }
        }

        /// <summary>三级菜单列表 </summary>
        public ObservableCollection<MenuModel> SystemThirdLevelMenuList
        {
            get
            {
                if (systemThirdLevelMenuList == null)
                    systemThirdLevelMenuList = new ObservableCollection<MenuModel>();
                return systemThirdLevelMenuList;
            }
            set
            {
                SelectedThirdMenuModel = null;
                systemThirdLevelMenuList = value;
                SelectedThirdMenuModel = systemThirdLevelMenuList.First();
                RaisePropertyChanged("SystemThirdLevelMenuList");
            }
        }

        /// <summary>四级菜单列表 </summary>
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

        /// <summary>当前选中的二级菜单 </summary>
        public MenuModel SelectedSecondMenuModel
        {
            get { return selectedSecondMenuModel; }
            set
            {
                if (value == null)
                {
                    return;
                }
                selectedSecondMenuModel = value;
                RaisePropertyChanged("SelectedSecondMenuModel");
            }
        }

        /// <summary>当前选中的三级菜单 </summary>
        public MenuModel SelectedThirdMenuModel
        {
            get { return selectedThirdMenuModel; }
            set
            {
                selectedThirdMenuModel = value;
                RaisePropertyChanged("SelectedThirdMenuModel");
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

        /// <summary>当前选项卡 </summary>
        public VicTabItemNormal SelectedTabItem
        {
            get { return selectedTabItem; }
            set
            {
                if (selectedTabItem != value)
                {
                    selectedTabItem = value;
                    RaisePropertyChanged("SelectedTabItem");
                }
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(userName))
                {
                    Random rd = new Random();
                    userName = "User " + rd.Next() + "，欢迎登录系统";
                }
                return userName;
            }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImg
        {
            get
            {
                return userImg;
            }
            set
            {
                if (userImg != value)
                {
                    userImg = value;
                    RaisePropertyChanged("UserImg");
                }
            }
        }
        /// <summary>
        /// 活动插件数目
        /// </summary>
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

        #endregion

        #region 命令

        #region 窗体加载命令

        /// <summary>窗体加载命令 </summary>
        public ICommand gridMainLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    mainWindow = (Window)x;
                    mainWindow.Uid = "mainWindow";
                    _panel = mainWindow.FindName("bigPanel") as Canvas;//添加新区域面板
                    btnPluginList = mainWindow.FindName("btnPluginList") as VicButtonNormal;
                    mainWindow.MouseDown += mainWindow_MouseDown;
                    Rect rect = SystemParameters.WorkArea;
                    mainWindow.MaxWidth = rect.Width;
                    mainWindow.MaxHeight = rect.Height;
                    mainWindow.WindowState = WindowState.Maximized;
                    ChangeFrameWorkTheme();
                    //LoadMenuListLocal();
                    LoadJsonMenuListLocal();
                    OverlayWindow overlayWin = new OverlayWindow();
                    overlayWin.Show();
                    UserLogin();
                });
            }
        }
        #region 插件按钮鼠标进入事件命令
        /// 
        /// <summary>
        /// 插件按钮鼠标进入事件命令 
        /// </summary>
        public ICommand btnPluginListMouseEnterCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //获取插件信息
                    DataMessageOperation dataop = new DataMessageOperation();
                    pluginList = dataop.GetPluginInfo();
                    //创建窗体
                    if (win_PluginList == null)
                    {
                        win_PluginList = new Window();
                        //状态栏不显示
                        win_PluginList.ShowInTaskbar = false;
                        //窗体大小自适应其内容
                        win_PluginList.MouseLeave += win_PluginList_MouseLeave;
                        win_PluginList.SizeToContent = SizeToContent.Manual;
                        win_PluginList.SizeToContent = SizeToContent.Width;
                        win_PluginList.SizeToContent = SizeToContent.Height;
                        win_PluginList.SizeToContent = SizeToContent.WidthAndHeight;
                        win_PluginList.WindowStyle = WindowStyle.None;
                        win_PluginList.ResizeMode = ResizeMode.NoResize;
                        win_PluginList.Visibility = Visibility.Visible;
                    }
                    if (pluginList.Count < 1)
                    {
                        win_PluginList.Visibility = Visibility.Hidden;
                        return;
                    }
                    win_PluginList.Content = GetActivePluginInfo();
                    this.win_PluginList.Visibility = Visibility.Visible;
                    if (!win_PluginList.IsActive)
                    {
                        //设置相对于“新单”按钮的位置
                        System.Windows.Point point = btnPluginList.PointToScreen(new System.Windows.Point(0, 0));//当前组件相对屏幕左上角的坐标               

                        win_PluginList.Left = point.X;

                        win_PluginList.Top = point.Y + btnPluginList.Height + 5;

                        win_PluginList.Show();
                        win_PluginList.Activate();
                    }
                });
            }
        }

        private void win_PluginList_MouseLeave(object sender, MouseEventArgs e)
        {
            this.win_PluginList.Visibility = Visibility.Collapsed;
        }
        #endregion

        private void ChangeFrameWorkTheme()
        {
            string messageType = "ServerCenterService.ChangeTheme";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("ServiceParams", "");
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendAsyncMessage(messageType, contentDic);
            ChangeFrameWorkLanguage();
        }
        private void ChangeFrameWorkLanguage()
        {
            string messageType = "ServerCenterService.ChangeLanguage";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("ServiceParams", "");
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendAsyncMessage(messageType, contentDic);
        }
        #endregion

        #region 窗体移动命令

        void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mainWindow.DragMove();
            }
        }
        #endregion

        #region 换肤命令
        /// <summary>用户登录命令 </summary>
        public ICommand btnChangeSkinClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ChangeTheme();
                });
            }
        }
        #endregion

        #region 用户登录命令
        /// <summary>用户登录命令 </summary>
        public ICommand btnUserLoginClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //if (isFirstLogin)
                    //{
                    //    UserLogin();
                    //}
                    UserLogin();
                });
            }
        }
        #endregion

        #region 窗体最小化命令
        /// <summary>窗体最小化命令 </summary>
        public ICommand btnMiniClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    mainWindow.WindowState = WindowState.Minimized;
                });
            }
        }
        #endregion

        #region 窗体最大化命令
        /// <summary>窗体最大化命令 </summary>
        public ICommand btnMaxClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    Button btnMax = (Button)x;
                    if (mainWindow.WindowState == WindowState.Normal)
                    {
                        btnMax.SetResourceReference(Button.StyleProperty, "btnRestoreStyle");
                        mainWindow.WindowState = WindowState.Maximized;
                        mainWindow.Left = 0;
                        mainWindow.Top = 0;
                    }
                    else
                    {
                        btnMax.SetResourceReference(Button.StyleProperty, "btnMaxStyle");
                        mainWindow.WindowState = WindowState.Normal;
                    }
                });
            }
        }
        #endregion

        #region 窗体关闭命令
        /// <summary>窗体关闭命令 </summary>
        public ICommand btnCloseClickCommand
        {

            get
            {
                return new RelayCommand(() =>
                {
                    MessageBoxResult result = VicMessageBoxNormal.Show("确定要退出么？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        //TabItemList.Clear();
                        mainWindow.Close();
                        FrameInit.GetInstance().FrameUnload();
                        GC.Collect();
                    }
                });
            }
        }
        #endregion

        #region 本地按钮点击命令
        public ICommand localBtnClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CreateBrowser("www.baidu.com", "百度搜索");
                });
            }
        }
        #endregion

        #region 本地选中菜单改变命令
        public ICommand listSecondMenuLocalSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        //if (!ConfigurationManager.AppSettings["DevelopMode"].Equals("Debug"))
                        //SetCurrentGallery("VICTOP");
                        BuildPluginContainer(x);
                    }
                });
            }
        }
        /// <summary>
        /// 构建插件容器
        /// </summary>
        /// <param name="x"></param>
        private void BuildPluginContainer(object x)
        {
            if (TabItemList[0].Content.GetType().Name.Equals("WebBrowser"))
            {
                UCPersonPluginContainer pluginContainer = new UCPersonPluginContainer();
                TabItemList[0].Content = pluginContainer;
                TabItemList[0].Header = "个人收藏";
            }

            MenuModel menuModel = (MenuModel)x;
            SystemThirdLevelMenuList = menuModel.SystemMenuList;
            if (SystemThirdLevelMenuList.Count > 0)
            {
                SelectedThirdMenuModel = SystemThirdLevelMenuList[0];
                SystemFourthLevelMenuList = SelectedThirdMenuModel.SystemMenuList;

            }

            SelectedTabItem = TabItemList[0];
        }
        #endregion

        #region 企业云选中菜单改变命令
        public ICommand listSecondMenuEnterpriseSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        //SetCurrentGallery("ENTERPRISE");
                        BuildPluginContainer(x);
                    }
                });
            }
        }
        #endregion

        #region 三级菜单改变命令
        public ICommand listBoxThirdMenuListSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        SystemFourthLevelMenuList = ((MenuModel)x).SystemMenuList;
                    }
                });
            }
        }
        #endregion

        #region 单击插件图标命令
        public ICommand btnPluginIcoClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        MenuModel menuModel = (MenuModel)x;
                        //LoadPlugin(menuModel);
                        OpenJsonMenuPlugin(menuModel);
                    }
                });
            }
        }
        #endregion

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
                        //LoadPlugin(menuModel);
                        OpenJsonMenuPlugin(menuModel);
                    }
                });
            }
        }

        public ICommand enterPriseBtnClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //CreateBrowser("www.victop.com", "飞道科技");
                });
            }
        }
        #endregion

        #region TabItem关闭命令
        public ICommand tbctrlTabItemClosingCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UserControl tabCtrl = (UserControl)(SelectedTabItem.Content);
                    DataMessageOperation messageOp = new DataMessageOperation();
                    if (!string.IsNullOrEmpty(tabCtrl.Uid))
                    {
                        string messageType = "PluginService.PluginStop";
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("ObjectId", tabCtrl.Uid);
                        messageOp.SendAsyncMessage(messageType, contentDic);
                    }
                    ActivePluginNum = messageOp.GetPluginInfo().Count;
                });
            }
        }
        #endregion

        #endregion

        #region 自定义方法

        #region 加载企业云标准化菜单
        /// <summary>加载标准化菜单</summary>
        private void LoadStandardMenu()
        {
            //LoadMenuListEnterprise();已经删除
        }
        #endregion


        #region 加载本地Json菜单集合 (2014-08-29 新增)
        /// <summary>加载本地Json菜单集合 </summary>
        private void LoadJsonMenuListLocal()
        {
            this.SystemMenuListLocal.Clear();
            string menuList = string.Empty;
            string menuPath = AppDomain.CurrentDomain.BaseDirectory + "menu.json";
            if (File.Exists(menuPath))
            {
                menuList = File.ReadAllText(menuPath, Encoding.GetEncoding("gb2312"));
                menuList = JsonHelper.ReadJsonString(menuList, "menu");
            }
            this.SystemMenuListLocal = JsonHelper.ToObject<ObservableCollection<MenuModel>>(menuList);
            localMenuListEx = JsonHelper.ToObject<ObservableCollection<MenuModel>>(menuList);
            //if (!ConfigurationManager.AppSettings["DevelopMode"].Equals("Debug"))
            //{
            //    this.SystemMenuListLocal.Clear();

            ///2015-03-05:得到弹窗中一、二级树型菜单并绑定到前台
            foreach (MenuModel menuModel in SystemMenuListLocal)
            {
                MenuModel newModel = menuModel.Copy();
                newModel.SystemMenuList = new ObservableCollection<MenuModel>();
                if (menuModel.SystemMenuList.Count > 0)
                {
                    foreach (MenuModel childModel in menuModel.SystemMenuList)
                    {
                        MenuModel childNewModel = childModel.Copy();
                        childNewModel.SystemMenuList = new ObservableCollection<MenuModel>();
                        newModel.SystemMenuList.Add(childNewModel);
                    }
                }
                NewMenuListLocal.Add(newModel);
            }
        }
        #endregion

        #region 打开Json菜单下的插件(2014-08-29 新增)
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
                        ActivePluginNum = pluginOp.GetPluginInfo().Count;
                        pluginWin.Show();
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
                ActivePluginNum = pluginOp.GetPluginInfo().Count;
            }
            catch (Exception ex)
            {
                VicMessageBoxNormal.Show(ex.Message);
            }
        }

        #endregion

        #region 用户登录
        private void UserLogin()
        {
            DataMessageOperation pluginOp = new DataMessageOperation();
            string loginPlugin = ConfigurationManager.AppSettings["loginWindow"];
            PluginModel pluginModel = pluginOp.StratPlugin(loginPlugin);
            IPlugin PluginInstance = pluginModel.PluginInterface;
            Window loginWin = PluginInstance.StartWindow;
            loginWin.Uid = pluginModel.ObjectId;
            loginWin.Owner = mainWindow;
            bool? result = loginWin.ShowDialog();
            if (result == true)
            {
                Dictionary<string, object> userDic = pluginOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                if (userDic != null)
                {
                    UserName = JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "UserName");
                    this.UserImg = this.DownLoadUserImg(JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "UserCode"), JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "UserImg"));
                }
                isFirstLogin = false;
                LoadStandardMenu();
                //NotificationCenter notifyCenter = new NotificationCenter();
                //notifyCenter.Show();
            }

        }

        private string DownLoadUserImg(string userCode, string fileInfo)
        {
            string path = string.Empty;
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VictopPartner\\UserPhoto\\";
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }

            path = dir + userCode + ".jpg";
            if (File.Exists(path))
            {
                return path;
            }

            if (string.IsNullOrWhiteSpace(fileInfo) == false)
            {
                DataMessageOperation messageOperation = new DataMessageOperation();
                Dictionary<string, object> messageContent = new Dictionary<string, object>();
                Dictionary<string, string> address = new Dictionary<string, string>();
                address.Add("DownloadFileId", fileInfo);
                address.Add("DownloadToPath", path);
                messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                Dictionary<string, object> downResult = messageOperation.SendSyncMessage("ServerCenterService.DownloadDocument", messageContent);
                if (downResult != null)
                {
                    if (downResult["ReplyMode"].ToString() == "1")
                    {
                        return path;
                    }
                }
            }

            return string.Empty;
        }
        #endregion

        #region 换肤
        private void ChangeTheme()
        {
            DataMessageOperation pluginOp = new DataMessageOperation();
            PluginModel pluginModel = pluginOp.StratPlugin("ThemeManagerPlugin");
            IPlugin PluginInstance = pluginModel.PluginInterface;
            Window themeWin = PluginInstance.StartWindow;
            themeWin.Uid = pluginModel.ObjectId;
            themeWin.Owner = mainWindow;
            themeWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            themeWin.ShowDialog();
        }

        #endregion

        #region 弹窗显示获取的插件信息
        /// <summary>
        /// 弹窗显示获取的插件信息
        /// </summary>
        /// <param name="PluginInfoList"></param>
        /// <returns></returns>
        private VicStackPanelNormal GetActivePluginInfo()
        {
            VicStackPanelNormal PluginListContent = new VicStackPanelNormal();
            PluginListContent.Orientation = Orientation.Vertical;
            PluginListContent.Width = 120;
            foreach (Dictionary<string, object> PluginInfo in pluginList)
            {
                VicButtonNormal btn = new VicButtonNormal();
                btn.Width = 120;
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
                else
                {
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
        #endregion
        #endregion

        #region 关于WebBrowser相关的操作

        /// <summary>
        /// 创建浏览器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        private void CreateBrowser(string url, string title)
        {
            WebBrowser browser = new WebBrowser();
            browser.Navigating += browser_Navigating;
            browser.Source = new Uri(string.Format("http://{0}", url));
            TabItemList[0].Content = browser;
            TabItemList[0].Header = title;
        }
        void browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SuppressScriptErrors((WebBrowser)sender, true);
        }

        private void SuppressScriptErrors(WebBrowser webBrowser, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }
        #endregion

        ///<summary>
        /// 20150305添加菜单应用弹窗相关代码
        /// </summary> 
        #region

        #region 字段&属性
        private UserControl area;//当前用户控件
        private Canvas _panel;//主区域面板
        private ListBox _listbox;//弹窗展示菜单列表
        private string menuPath;//文件路径
        private string selectAreaId;//保存当前选中的要添加应用的区域
        /// <summary>添加应用弹框菜单列表 </summary>
        private ObservableCollection<MenuModel> newMenuListLocal;
        public ObservableCollection<MenuModel> NewMenuListLocal
        {
            get
            {
                if (newMenuListLocal == null)
                    newMenuListLocal = new ObservableCollection<MenuModel>();
                return newMenuListLocal;
            }
            set
            {
                if (newMenuListLocal != value)
                {
                    systemMenuListLocal = value;
                    RaisePropertyChanged("NewMenuListLocal");
                }
            }
        }
        /// <summary>添加应用弹窗四级菜单列表 </summary>
        private ObservableCollection<MenuModel> newSystemFourthLevelMenuList;
        public ObservableCollection<MenuModel> NewSystemFourthLevelMenuList
        {
            get
            {
                if (newSystemFourthLevelMenuList == null)
                    newSystemFourthLevelMenuList = new ObservableCollection<MenuModel>();
                return newSystemFourthLevelMenuList;
            }
            set
            {
                if (newSystemFourthLevelMenuList != value)
                {
                    newSystemFourthLevelMenuList = value;
                    RaisePropertyChanged("NewSystemFourthLevelMenuList");
                }
            }
        }

        /// <summary>当前选中的应用弹窗菜单列表 </summary>
        private ObservableCollection<MenuModel> selectPopupMenuList;
        public ObservableCollection<MenuModel> SelectPopupMenuList
        {
            get
            {
                if (selectPopupMenuList == null)
                    selectPopupMenuList = new ObservableCollection<MenuModel>();
                return selectPopupMenuList;
            }
            set
            {
                if (selectPopupMenuList != value)
                {
                    selectPopupMenuList = value;
                    RaisePropertyChanged("SelectPopupMenuList");
                }
            }
        }
        /// <summary>
        /// 新区域集合
        /// </summary>
        private ObservableCollection<AreaMenu> newArea;
        public ObservableCollection<AreaMenu> NewArea
        {
            get
            {
                if (newArea == null)
                    newArea = new ObservableCollection<AreaMenu>();
                return newArea;
            }
            set
            {
                if (newArea != value)
                {
                    newArea = value;
                }
            }
        }
        /// <summary>
        /// 添加应用弹窗是否显示
        /// </summary>
        private bool popupIsShow;
        public bool PopupIsShow
        {
            get
            {
                return popupIsShow;
            }
            set
            {
                if (popupIsShow != value)
                {
                    popupIsShow = value;
                    RaisePropertyChanged("PopupIsShow");
                }
            }
        }
        #endregion

        #region 命令
        /// <summary>
        /// 区域加载命令
        ///  </summary>
        public ICommand PersonAreaLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    area = (UserControl)x;
                    _panel = area.FindName("bigPanel") as Canvas;//找到“添加新区域面板”
                    _listbox = area.FindName("listBoxPopupMenuList") as ListBox;
                    DrawingPanelArea();//读文件并渲染区域
                });
            }
        }

        /// <summary>
        /// 单击“添加应用”
        /// </summary>
        public ICommand btnAddApplyClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                    {

                        VicRadioButtonNormal btn = (VicRadioButtonNormal)x;
                        DockPanel parentPanel = GetParentObject<DockPanel>(btn);
                        selectAreaId = parentPanel.Uid;//得到选中区域ID
                        _panel.IsEnabled = false;
                        PopupIsShow = true;
                    });
            }
        }

        /// <summary>
        ///弹框菜单全选
        /// </summary>
        public ICommand btnAllSelectClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //一下全部选中
                });
            }
        }
        /// <summary>
        /// 应用弹窗确定
        /// </summary>
        public ICommand btnAffirmClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                   
                    SelectPopupMenuList.Clear();
                    int k = 0;
                    for (int i = 0; i < NewArea.Count; i++)
                    {
                        if (NewArea[i].AreaID == selectAreaId)
                        {
                            foreach (MenuModel menuModel in _listbox.SelectedItems)
                            {
                                foreach (MenuModel addedPlugin in NewArea[i].PluginList)
                                {
                                    if (addedPlugin.MenuName != menuModel.MenuName) k++;
                                }
                                if (k == NewArea[i].PluginList.Count) SelectPopupMenuList.Add(menuModel);
                            }
                            if (SelectPopupMenuList != null)
                            {
                                foreach (MenuModel menuModel in SelectPopupMenuList)
                                {
                                    NewArea[i].PluginList.Add(menuModel);
                                }
                            }
                            break;
                        }
                    }

                    PopupIsShow = false;
                    _panel.IsEnabled = true;
                    StreamWriter sw = new StreamWriter(menuPath, false);
                    sw.Write(JsonHelper.ToJson(NewArea));
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                    DrawingPanelArea();

                });
            }
        }
        /// <summary>
        /// 应用弹窗关闭
        /// </summary>
        public ICommand btnAddApplyCloseClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    PopupIsShow = false;
                    _panel.IsEnabled = true;
                });
            }
        }
        /// <summary>
        /// 应用弹窗树型菜单选项改变
        /// </summary>
        public ICommand treeviewViewSelectedItemChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                    {
                        MenuModel _menuName = (MenuModel)x;
                        foreach (MenuModel menuModel in SystemMenuListLocal)
                        {
                            MenuModel childNewModel = new MenuModel();
                            childNewModel = menuModel.SystemMenuList.FirstOrDefault(it => it.MenuName.Equals(_menuName.MenuName));
                            if (childNewModel != null)
                            {
                                NewSystemFourthLevelMenuList = childNewModel.SystemMenuList;
                                break;
                            }
                            else
                            {
                                NewSystemFourthLevelMenuList = null;
                                break;
                            }
                        }
                    });
            }
        }
        /// <summary>
        /// 新建区域
        /// </summary>
        public ICommand btnAddAreaClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AreaMenu _areaMenu = new AreaMenu();
                    TextBox _title = new TextBox();
                    DockPanel.SetDock(_title, Dock.Top);
                    _title.Text = "My often Fun";
                    _title.VerticalContentAlignment = VerticalAlignment.Center;
                    _title.HorizontalContentAlignment = HorizontalAlignment.Center;
                    _title.Height = 30;
                    _title.Background = Brushes.Gainsboro;

                    ListBox menuList = new ListBox();
                    ListBoxItem _item = new ListBoxItem();
                    menuList.Items.Add(_item);
                    menuList.Style = area.FindResource("addApply") as Style;
                    DockPanel _newPanel = new DockPanel();
                    _newPanel.Uid = Guid.NewGuid().ToString();
                    _newPanel.Width = _areaMenu.AreaWidth;
                    _newPanel.Height = _areaMenu.AreaHeight;
                    _newPanel.Children.Add(_title);
                    _newPanel.Children.Add(menuList);

                    Canvas.SetLeft(_newPanel, _areaMenu.LeftSpan + NewArea.Count * 10);
                    Canvas.SetTop(_newPanel, _areaMenu.TopSpan + NewArea.Count * 10);
                    _panel.Children.Add(_newPanel);

                    //把添加的区域写到JSON文件中
                    _areaMenu.AreaName = _title.Text;
                    _areaMenu.AreaID = _newPanel.Uid;
                    _areaMenu.LeftSpan += NewArea.Count * 10;
                    _areaMenu.TopSpan += NewArea.Count * 10;
                    NewArea.Add(_areaMenu);
                    StreamWriter sw = new StreamWriter(menuPath, false);
                    sw.Write(JsonHelper.ToJson(NewArea));
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                });
            }
        }
        #endregion

        #region  方法&事件

        /// <summary>
        ///  渲染区域方法
        /// </summary>
        private void DrawingPanelArea()
        {
            NewArea.Clear();
            //读取myMenu.json文件并展示
            string areaMenuList = string.Empty;
            menuPath = AppDomain.CurrentDomain.BaseDirectory + "mymenu.json";
            if (File.Exists(menuPath))
            {
                areaMenuList = File.ReadAllText(menuPath, Encoding.GetEncoding("UTF-8"));
            }
            this.NewArea = JsonHelper.ToObject<ObservableCollection<AreaMenu>>(areaMenuList);

            for (int i = 0; i < NewArea.Count; i++)
            {
                TextBox _title = new TextBox();
                DockPanel.SetDock(_title, Dock.Top);
                _title.Text = NewArea[i].AreaName;
                _title.VerticalContentAlignment = VerticalAlignment.Center;
                _title.HorizontalContentAlignment = HorizontalAlignment.Center;
                _title.Height = 30;
                _title.Background = Brushes.Gainsboro;
                ListBox menuListArea = new ListBox();
                menuListArea.Background = Brushes.WhiteSmoke;

                WrapPanel pluginPanel = new WrapPanel();
                pluginPanel.Orientation = Orientation.Horizontal;
                ListBox addapplyStyle = new ListBox();
                ListBoxItem _item = new ListBoxItem();
                addapplyStyle.Items.Add(_item);
                addapplyStyle.Style = area.FindResource("addApply") as Style;
                pluginPanel.Children.Add(addapplyStyle);
                if (NewArea[i].PluginList.Count != 0)
                {
                    ListBox pluginlist = new ListBox();
                    pluginlist.ItemsSource = NewArea[i].PluginList;
                    pluginlist.Style = mainWindow.FindResource("ListBoxFourthMenuListStyle") as Style;
                    pluginPanel.Children.Insert(pluginPanel.Children.Count - 1, pluginlist);
                }

                menuListArea.Items.Add(pluginPanel);
                DockPanel _newPanel = new DockPanel();
                _newPanel.Uid = NewArea[i].AreaID;
                _newPanel.MinWidth = NewArea[i].AreaWidth;
                _newPanel.MinHeight = NewArea[i].AreaHeight;
                _newPanel.Children.Add(_title);
                _newPanel.Children.Add(menuListArea);
                Canvas.SetLeft(_newPanel, NewArea[i].LeftSpan);
                Canvas.SetTop(_newPanel, NewArea[i].TopSpan);
                _panel.Children.Add(_newPanel);
            }
        }
        /// <summary>
        /// 获取父级控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
        #endregion

        #endregion
    }
}