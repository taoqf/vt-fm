using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Victop.Frame.CoreLibrary.Common;
using Victop.Frame.Units;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using Victop.Wpf.Controls;
using Victop.Server.Controls;
using MetroFramePlugin.Models;
using System.Collections.ObjectModel;
using System.Reflection;
using MetroFramePlugin.Views;
using System.Windows.Navigation;
using System.IO;
using System.Text;
using Victop.Frame.DataMessageManager;
using System.Xml;
using System.Text.RegularExpressions;
using Victop.Frame.PublicLib.Managers;

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
        private bool poPupState;
        /// <summary>是否首次登录 </summary>
        private bool isFirstLogin = true;
        private bool isFirstLoad = true;
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;
        /// <summary>
        /// 用户头像
        /// </summary>
        private string userImg;
        /// <summary>
        /// 用户全拼
        /// </summary>
        private string userCode;
        public string UserCode
        {
            get
            {
                return userCode;
            }
            set
            {
                if (userCode != value)
                {
                    userCode = value;
                    RaisePropertyChanged("UserCode");
                }
            }
        }
        /// <summary>
        /// 用户ProductId
        /// </summary>
        private string productId;
        public string ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                if (productId != value)
                {
                    productId = value;
                    RaisePropertyChanged("ProductId");
                }
            }
        }
        private string clientId;
        public string ClientId
        {
            get
            {
                return clientId;
            }
            set
            {
                if (clientId != value)
                {
                    clientId = value;
                    RaisePropertyChanged("ClientId");
                }
            }
        }
        /// <summary>
        /// 活动插件数目
        /// </summary>
        private long activePluginNum;
        /// <summary>
        /// 本地插件集合
        /// </summary>
        private ObservableCollection<MenuModel> localMenuListEx = new ObservableCollection<MenuModel>();
        /// <summary>
        /// 应用程序版本编号
        /// </summary>
        private string appVersionCode;

        /// <summary>
        /// 点击头像状态
        /// </summary>
        public bool PoPupState
        {
            get
            {
                return poPupState;
            }
            set
            {
                if (poPupState != value)
                {
                    poPupState = value;
                    RaisePropertyChanged("PoPupState");
                }
            }
        }
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
                    VicTabItemNormal personItem = new VicTabItemNormal();
                    personItem.Name = "homeItem";
                    personItem.AllowDelete = false;
                    personItem.Header = "个人收藏";
                    UCPersonPluginContainer personPluginContainer = new UCPersonPluginContainer();
                    personItem.Content = personPluginContainer;
                    tabItemList.Add(personItem);

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
        /// 用户角色
        /// </summary>
        private string userRole;
        /// <summary>
        /// 用户角色
        /// </summary>
        public string UserRole
        {
            get
            {
                return userRole;
            }
            set
            {
                if (userRole != value)
                {
                    userRole = value;
                    RaisePropertyChanged("UserRole");
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
        /// <summary>
        /// 应用程序版本编号
        /// </summary>
        public string AppVersionCode
        {
            get
            {
                return appVersionCode;
            }
            set
            {
                if (appVersionCode != value)
                {
                    appVersionCode = value;
                    RaisePropertyChanged("AppVersionCode");
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
                    mainTabControl = (VicTabControlNormal)mainWindow.FindName("MainTabControl");
                    btnPluginList = mainWindow.FindName("btnPluginList") as VicButtonNormal;
                    mainWindow.MouseDown += mainWindow_MouseDown;
                    Rect rect = SystemParameters.WorkArea;
                    mainWindow.MaxWidth = rect.Width;
                    mainWindow.MaxHeight = rect.Height;
                    mainWindow.WindowState = WindowState.Maximized;
                    ChangeFrameWorkTheme();
                    AppVersionCode = GetAppVersion();
                    LoadJsonMenuListLocal();
                    OverlayWindow overlayWin = new OverlayWindow();
                    OverlayWindow.VicTabCtrl = mainTabControl;
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
                    //  UserLogin();
                    PoPupState = true;
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
        #region 切换用户命令
        public ICommand btnChangeUserClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    PoPupState = false;
                    isChangeUser = true; //切换用户时，从服务器拉取菜单必要操作
                    UserLogin();
                    DrawingPanelArea();
                   
                });
            }
        }
        #endregion
        #region 修改密码
        public ICommand btnModifiPassClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ClientId = ConfigManager.GetAttributeOfNodeByName("UserInfo", "ClientId");
                    PoPupState = false;
                    Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = string.Format("{0}?userCode={1}&ClientId={2}&ProductId={3}", ConfigurationManager.AppSettings["updatepwdhttp"], UserCode, string.Format("{0}::{1}", ClientId, ProductId), ProductId);
                    proc.Start();
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
                        
                        DataMessageOperation dataMsgOp = new DataMessageOperation();
                        dataMsgOp.RemoveDataLock();
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
                UCPluginContainer pluginContainer = new UCPluginContainer();
                TabItemList[0].Content = pluginContainer;
                TabItemList[0].Header = "功能列表";
              
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

        #region 加载标准化菜单
        /// <summary>加载标准化菜单</summary>
        private void LoadStandardMenu()
        {

            LoadMenuListEnterprise();
        }
        /// <summary>加载企业云菜单集合 </summary>
        private void LoadMenuListEnterprise()
        {
            NewMenuListEnterprise.Clear();//避免未关闭情况下二次单击登录头像，出现菜单项重复添加了

            BaseResourceInfo resourceInfo = new BaseResourceManager().GetCurrentGalleryBaseResource();
            SystemMenuListEnterprise.Clear();
            if (resourceInfo != null && resourceInfo.ResourceMnenus.Count > 0)
            {
                foreach (MenuInfo item in resourceInfo.ResourceMnenus.Where(it => it.Parent_no.Equals("0") || string.IsNullOrEmpty(it.Parent_no)))
                {
                    MenuModel menuModel = GetMenuModel(item);
                    menuModel = CreateMenuList(item.Menu_no, resourceInfo.ResourceMnenus, menuModel);
                    SystemMenuListEnterprise.Add(menuModel);
                }
            }
            GetStandardMenuList(SystemMenuListEnterprise);



            //2015-04-27:得到弹窗中企业云一、二级树型菜单并绑定到前台
            foreach (MenuModel menuModel in SystemMenuListEnterprise)
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
                NewMenuListEnterprise.Add(newModel);
            }




        }
        /// <summary>创建完整的菜单模型 </summary>
        private MenuModel CreateMenuList(string parentMenu, List<MenuInfo> fullMenuList, MenuModel parentModel)
        {
            foreach (MenuInfo item in fullMenuList.Where(it => it.Parent_no.Equals(parentMenu)))
            {
                MenuModel menuModel = GetMenuModel(item);
                menuModel = CreateMenuList(item.Menu_no, fullMenuList, menuModel);
                parentModel.SystemMenuList.Add(menuModel);
            }
            return parentModel;
        }
        /// <summary>获得菜单模型实例</summary>
        private MenuModel GetMenuModel(MenuInfo item)
        {
            MenuModel menuModel = new MenuModel();
            menuModel.Id = item.Id;
            menuModel.MenuNo = item.Menu_no;
            menuModel.MenuName = item.Menu_name;
            menuModel.SystemId = item.Systemid;
            menuModel.FormId = item.Formid;
            menuModel.ShowType = "0";
            menuModel.PackageUrl = item.Package_url;
            menuModel.ShowType = string.IsNullOrEmpty(item.Show_type) ? "1" : item.Show_type;
            menuModel.Icon = !string.IsNullOrEmpty(item.Icon) ? Regex.Unescape(item.Icon) : item.Icon;
            menuModel.PluginBG = item.Background;
            menuModel.Description = item.Description;
            if (item.Roles != null && item.Roles.Count > 0)
            {
                foreach (MenuRoleInfo roleitem in item.Roles)
                {
                    menuModel.RoleAuthList.Add(new MenuRoleAuth() { Role_No = roleitem.Role_no, AuthCode = roleitem.Auth_code });
                }
            }
            return menuModel;
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
            if (!ConfigurationManager.AppSettings["DevelopMode"].Equals("Debug"))
            {
                this.SystemMenuListLocal.Clear();
            }

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
            MenuRoleAuth roleAuth = selectedFourthMenu.RoleAuthList.FirstOrDefault(it => it.Role_No.Equals(UserRole));
            if (roleAuth != null)
            {
                selectedFourthMenu.AuthorityCode = roleAuth.AuthCode;
            }
            if (!ConfigurationManager.AppSettings["DevelopMode"].Equals("Debug"))
            {
                if (roleAuth == null)
                {
                    VicMessageBoxNormal.Show("当前角色无启动此功能的权限");
                    return;
                }
            }
            if (TabItemList.FirstOrDefault(it => it.Header.Equals(selectedFourthMenu.MenuName)) != null)
            {
                TabItemList.FirstOrDefault(it => it.Header.Equals(selectedFourthMenu.MenuName)).IsSelected = true;
                return;
            }
            if (selectedFourthMenu.PackageUrl != null)
            {
                DataMessageOperation pluginOp = new DataMessageOperation();
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("systemid", selectedFourthMenu.SystemId);
                paramDic.Add("configsystemid", "11");
                paramDic.Add("formid", selectedFourthMenu.FormId);
                paramDic.Add("authoritycode", selectedFourthMenu.AuthorityCode);
                PluginModel pluginModel = pluginOp.StratPlugin(selectedFourthMenu.PackageUrl, paramDic);
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
        #endregion

        #region 转换为标准的四级菜单
        /// <summary>获取标准的菜单集合 </summary>
        private void GetStandardMenuList(ObservableCollection<MenuModel> disStandardMenuList)
        {
            foreach (MenuModel secondLevelMenu in disStandardMenuList)
            {
                GetStandardSecondLevelMenu(secondLevelMenu);
            }
        }
        /// <summary>获取标准的二级菜单</summary>
        private void GetStandardSecondLevelMenu(MenuModel secondLevelMenu)
        {
            MenuModel newThirdLevelMenu = new MenuModel();
            newThirdLevelMenu.Id = new Guid().ToString();
            for (int i = secondLevelMenu.SystemMenuList.Count - 1; i >= 0; i--)
            {
                MenuModel thirdLevelMenu = secondLevelMenu.SystemMenuList[i];
                if (thirdLevelMenu.SystemMenuList.Count == 0)
                {
                    secondLevelMenu.SystemMenuList.Remove(thirdLevelMenu);
                    thirdLevelMenu.ParentId = newThirdLevelMenu.Id;
                    newThirdLevelMenu.SystemMenuList.Add(thirdLevelMenu);
                }
                else
                {
                    GetStandardThirdLevelMenu(thirdLevelMenu);
                }
            }
            if (newThirdLevelMenu.SystemMenuList.Count > 0)
            {
                newThirdLevelMenu.Id = new Guid().ToString();
                newThirdLevelMenu.MenuName = "其他";
            }
            secondLevelMenu.SystemMenuList.Add(newThirdLevelMenu);
        }
        /// <summary>获取标准的三级菜单</summary>
        private void GetStandardThirdLevelMenu(MenuModel thirdLevelMenu)
        {
            for (int i = thirdLevelMenu.SystemMenuList.Count - 1; i >= 0; i--)
            {
                MenuModel fourthLevelMenu = thirdLevelMenu.SystemMenuList[i];
                foreach (MenuModel item in fourthLevelMenu.SystemMenuList)
                {
                    thirdLevelMenu.SystemMenuList.Remove(fourthLevelMenu);
                    item.ParentId = thirdLevelMenu.Id;
                    thirdLevelMenu.SystemMenuList.Add(item);
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
                        pluginWin.Owner = mainWindow;
                        ActivePluginNum = pluginOp.GetPluginInfo().Count;
                        pluginWin.Show();
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
        /// <summary>
        /// 发送关闭插件消息
        /// </summary>
        /// <param name="pluginModel"></param>
        private static void SendPluginCloseMessage(PluginModel pluginModel)
        {
            DataMessageOperation pluginOp = new DataMessageOperation();
            pluginOp.StopPlugin(pluginModel.ObjectId);
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
                    userRole = JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "CurrentRole");
                    UserCode = JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "UserCode");
                    ProductId = JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "ProductId");
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

        #region 设置当前通道信息
        /// <summary>
        /// 设置当前通道信息
        /// </summary>
        /// <param name="GaleryKey">通道key值</param>
        private void SetCurrentGallery(string GaleryKey)
        {
            string messageType = "GalleryService.SetGalleryInfo";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("GalleryKey", GaleryKey);
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendAsyncMessage(messageType, contentDic);
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

        #region 版本相关
        /// <summary>
        /// 获取应用程序版本
        /// </summary>
        /// <returns></returns>
        private string GetAppVersion()
        {
            string versionStr = "已是最新版本";
            try
            {
                XmlDocument xml = new XmlDocument();
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                if (File.Exists(Path.Combine(appPath, "AutoUpdate.exe.config")))
                {
                    xml.Load(Path.Combine(appPath, "AutoUpdate.exe.config"));
                    XmlElement xnode = (XmlElement)xml.SelectSingleNode("/configuration/appSettings/add[@key='Version']");
                    string UpDate = xnode.GetAttribute("value");
                    return string.Format("应用程序(版本号:{0}){1}", UpDate, versionStr);
                }
                else
                {
                    return string.Format("应用程序{0}", versionStr);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("获取应用程序版本错误：{0}", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                return string.Format("应用程序{0}", versionStr);
            }

        }
        #endregion

        ///<summary>
        /// 20150305添加菜单应用弹窗相关代码
        /// </summary> 

        #region

        #region 字段&属性
        private bool isChangeUser = false;//控制切换用户时从服务器下载菜单
        private bool isOverRender = false;//控制重绘
        private UserControl area;//当前用户控件
        private Canvas _panel;//主区域面板
        private ListBox _listbox;//弹窗展示菜单列表
        private VicButtonNormal _allSelectBtn;//弹窗"全选"
        private string menuPath = AppDomain.CurrentDomain.BaseDirectory + "mymenu.json";//文件路径
        private string selectAreaId;//保存当前选中的要添加应用的区域
        /// <summary>添加应用弹框本地菜单列表 </summary>  
        private ObservableCollection<MenuModel> newMenuListLocal;
        public ObservableCollection<MenuModel> NewMenuListLocal   //如果用“本地”菜单，XAML中树的菜单绑这个属性
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
                    newMenuListLocal = value;
                    RaisePropertyChanged("NewMenuListLocal");
                }
            }
        }

        /// <summary>添加应用弹框企业云菜单列表 </summary>  
        private ObservableCollection<MenuModel> newMenuListEnterprise;
        public ObservableCollection<MenuModel> NewMenuListEnterprise   //如果用“企业云”菜单，XAML中树的菜单绑这个属性
        {
            get
            {
                if (newMenuListEnterprise == null)
                    newMenuListEnterprise = new ObservableCollection<MenuModel>();
                return newMenuListEnterprise;
            }
            set
            {
                if (newMenuListEnterprise != value)
                {
                    newMenuListEnterprise = value;
                    RaisePropertyChanged("NewMenuListEnterprise");
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
                    if (!isFirstLoad)
                        return;

                    area = (UserControl)x;
                    _panel = area.FindName("bigPanel") as Canvas;//找到“添加新区域面板”
                    _listbox = area.FindName("listBoxPopupMenuList") as ListBox;//找到“添加应用中的菜单列表”
                    _allSelectBtn = area.FindName("btnAllSelect") as VicButtonNormal;//找到添加应用弹窗“全部选中按钮”
                    DrawingPanelArea();//读文件并渲染区域
                    isFirstLoad = false;
                });
            }
        }

        /// <summary>
        /// 根据搜索框搜索,实现弹窗列表四级菜单展示
        ///  </summary>
        public ICommand VicTextBoxSeachClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    ObservableCollection<MenuModel> SeachedFourthLevelMenuList = new ObservableCollection<MenuModel>();//调试心得：如果没有前台绑定的，不必要添加属性通知，也没必要设为全局变量，不然值改，哪里都跟着改的。
                    VicTextBoxSeach aa = (VicTextBoxSeach)x;
                    string keyTxt = aa.VicText.ToString();
                    if (!string.IsNullOrEmpty(keyTxt))
                    {
                        foreach (MenuModel pluginModel in _listbox.ItemsSource)
                        {
                            if (pluginModel.MenuName.Contains(keyTxt)) SeachedFourthLevelMenuList.Add(pluginModel);
                        }
                        _listbox.ItemsSource = SeachedFourthLevelMenuList;
                    }
                    else
                    {
                        _listbox.ItemsSource = NewSystemFourthLevelMenuList;
                    }
                });
            }
        }

        /// <summary>
        /// 根据搜索框清空,实现弹窗列表四级菜单展示
        ///  </summary>
        public ICommand VicTextBoxClearClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {

                    VicTextBoxSeach aa = (VicTextBoxSeach)x;
                    string keyTxt = aa.VicText.ToString();
                    if (string.IsNullOrEmpty(keyTxt))
                    {
                        _listbox.ItemsSource = NewSystemFourthLevelMenuList;
                    }
                });
            }
        }
        /// <summary>
        /// 在解锁状态下不能删除，因为拖动把它屏蔽了，单击“删除”某个显示插件
        /// </summary>
        public ICommand BtnDelPluginCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    VicButtonNormal btn = (VicButtonNormal)x;
                    DockPanel parentPanel = GetParentObject<DockPanel>(btn);
                    ListBoxItem nowSelectDelPlugin = GetParentObject<ListBoxItem>(btn);
                    MenuModel nowPlugin = (MenuModel)nowSelectDelPlugin.DataContext;
                    string eidtAreaId = parentPanel.Uid;//得到当前选中的区域ID
                    AreaMenu NowArea = NewArea.FirstOrDefault(it => it.AreaID.Equals(eidtAreaId));
                    MenuModel areaPlugin = new MenuModel();
                    areaPlugin = NowArea.PluginList.FirstOrDefault(it => it.MenuName.Equals(nowPlugin.MenuName));
                    NowArea.PluginList.Remove(areaPlugin);
                    WriteFile();
                    //重新改变插件显示的数据源，还是编辑状态，想还原，单击大中小图标
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == selectAreaId)
                        {
                            WrapPanel wrapPanelArea = GetChildObject<WrapPanel>(panel, panel.Uid);
                            if (wrapPanelArea != null && wrapPanelArea.Children.Count == 2)
                            {
                                ListBox pluginArea = wrapPanelArea.Children[0] as ListBox;
                                pluginArea.ItemsSource = null;
                                pluginArea.ItemsSource = NowArea.PluginList;
                                break;
                            }
                        }
                    }
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
                        _allSelectBtn.Content = "全部选中";//每次打开弹框，全选按钮名称统一
                        _listbox.SelectedItems.Clear();//每次打开弹框，去掉之前所选的
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
                return new RelayCommand<object>((x) =>
                    {
                        VicButtonNormal btn = (VicButtonNormal)x;
                        if (btn.Content.ToString() == "全部选中")
                        {
                            _listbox.SelectAll();
                            btn.Content = "取消全选";

                        }

                        else
                        {
                            _listbox.SelectedItem = null;
                            btn.Content = "全部选中";

                        }

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
                    AreaMenu NowArea = NewArea.FirstOrDefault(it => it.AreaID.Equals(selectAreaId));
                    foreach (MenuModel menuModel in _listbox.SelectedItems)
                    {
                        k = 0;//马虎：万不能少，调试出来的
                        foreach (MenuModel addedPlugin in NowArea.PluginList)
                        {
                            if (addedPlugin.MenuName != menuModel.MenuName) k++;
                        }
                        if (k == NowArea.PluginList.Count) SelectPopupMenuList.Add(menuModel);
                    }
                    if (SelectPopupMenuList != null)
                    {
                        foreach (MenuModel menuModel in SelectPopupMenuList)
                        {
                            NowArea.PluginList.Add(menuModel);
                        }
                    }
                    PopupIsShow = false;
                    _panel.IsEnabled = true;
                    WriteFile();
                    //重绘当前面板
                    isOverRender = false;
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == selectAreaId)
                        {
                            OverRideDrawingPanelArea(panel);
                            ThumbCanvas();
                            break;
                        }
                    }
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
                    if (x != null)
                    {
                        MenuModel _menuName = (MenuModel)x;

                        if (_menuName.MenuName == "飞道工具")
                        {
                            VicMessageBoxNormal.Show("注意：此选项没有展示菜单，请选择其子菜单！", "提示");
                            return;
                        }
                        foreach (MenuModel menuModel in SystemMenuListEnterprise)
                        {
                            MenuModel childNewModel = new MenuModel();
                            childNewModel = menuModel.SystemMenuList.FirstOrDefault(it => it.MenuName.Equals(_menuName.MenuName));
                            if (childNewModel != null)
                            {
                                NewSystemFourthLevelMenuList = childNewModel.SystemMenuList;
                                _listbox.ItemsSource = NewSystemFourthLevelMenuList;//调试心得：这个必须有，不然搜索后清空_listbox的值，不会自动绑上了。
                                break;
                            }
                            else
                            {
                                NewSystemFourthLevelMenuList = null;
                                break;
                            }
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
                    UnitAreaSeting _title = new UnitAreaSeting();

                    DockPanel.SetDock(_title, Dock.Top);
                    _title.ParamsModel.BtnDeblockingClick += BtnClick;
                    _title.MenuItemIcoClick += MenuItemClick;
                    _title.ParamsModel.TextChangedClick += ParamsModel_TextChangedClick;
                    _title.SecondMenuItemIcoClick += SecondMenuItemClick;
                    _title.ParamsModel.TitleWidth = _areaMenu.AreaWidth;
                    _title.ParamsModel.AreaName = _areaMenu.AreaName;
                    _title.ParamsModel.ThumbDragMoveClick += ThumbDragMove;//控制区域拖动
                    _title.VerticalContentAlignment = VerticalAlignment.Center;
                    _title.HorizontalContentAlignment = HorizontalAlignment.Center;
                    _title.Background = mainWindow.FindResource("MetroBGColor") as Brush;


                    ListBox menuList = new ListBox();
                    menuList.Background = mainWindow.FindResource("MetroBGColor") as Brush;
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
                    _areaMenu.AreaName = _title.ParamsModel.AreaName;
                    _areaMenu.AreaID = _newPanel.Uid;
                    _title.Uid = _newPanel.Uid;
                    _areaMenu.LeftSpan += NewArea.Count * 10;
                    _areaMenu.TopSpan += NewArea.Count * 10;
                    NewArea.Add(_areaMenu);

                    WriteFile(); //把新建的区域保存到服务器中
                    ThumbCanvas(_newPanel, false);//实现拖动
                });
            }
        }

        public ICommand SavePersonalFavorites
        {
            get { 
                 return new RelayCommand(() =>
                     {
                         SavePersonMenu();
                     });
            }
        }

        #endregion

        #region  方法&事件

        /// <summary>
        /// 读取myMenu.json文件
        /// </summary>
        void ReadMenuJsonFile()
        {
            string areaMenuList = string.Empty;

            if (File.Exists(menuPath))
            {
                areaMenuList = File.ReadAllText(menuPath, Encoding.GetEncoding("UTF-8"));
            }
            NewArea = JsonHelper.ToObject<ObservableCollection<AreaMenu>>(areaMenuList);

        }


        /// <summary>
        ///  渲染区域方法
        /// </summary>
        private void DrawingPanelArea()
        {
            NewArea.Clear();
            if (_panel != null) _panel.Children.Clear();

            if (isFirstLoad)
            {
                GetPersonMenu();
            }
            else if (isChangeUser)
            {
                GetPersonMenu();
                isChangeUser = false;
            }
            else
            {
                ReadMenuJsonFile();
            }
            for (int i = 0; i < NewArea.Count; i++)
            {
                UnitAreaSeting _title = new UnitAreaSeting();
                _title.ParamsModel.TitleWidth = NewArea[i].AreaWidth;
                _title.Uid = NewArea[i].AreaID;
                DockPanel.SetDock(_title, Dock.Top);
                _title.ParamsModel.BtnDeblockingClick += BtnClick;
               // _title.ParamsModel.ThumbDragMoveClick += ThumbDragMove;//因为初始默认了锁定，所以初始绘制去掉拖动
                _title.MenuItemIcoClick += MenuItemClick;
                _title.ParamsModel.TextChangedClick += ParamsModel_TextChangedClick;
                _title.SecondMenuItemIcoClick += SecondMenuItemClick;
                _title.ParamsModel.AreaName = NewArea[i].AreaName;
                _title.VerticalContentAlignment = VerticalAlignment.Center;
                _title.HorizontalContentAlignment = HorizontalAlignment.Center;
                _title.Foreground = mainWindow.FindResource("MetroFGColor") as Brush;

                WrapPanel pluginPanel = new WrapPanel();
                pluginPanel.Background = mainWindow.FindResource("MetroBGColor") as Brush;
                pluginPanel.Uid = NewArea[i].AreaID;
                pluginPanel.Orientation = Orientation.Horizontal;
                ListBox addapplyStyle = new ListBox();
                ListBoxItem _item = new ListBoxItem();
                addapplyStyle.Items.Add(_item);
                if (NewArea[i].MenuForm == "normal")
                {
                    addapplyStyle.Style = area.FindResource("addApply") as Style;
                }
                else if (NewArea[i].MenuForm == "large")
                {
                    addapplyStyle.Style = area.FindResource("addLargeApply") as Style;
                }
                else if (NewArea[i].MenuForm == "small")
                {
                    addapplyStyle.Style = area.FindResource("addSmallApply") as Style;
                }
                pluginPanel.Children.Add(addapplyStyle);

                if (NewArea[i].PluginList.Count != 0)
                {
                    ListBox pluginlist = new ListBox();

                    //pluginlist.PreviewMouseMove += menuList_PreviewMouseMove;//因为初始默认了锁定，所以初始绘制去掉内部的插件拖动
                    pluginlist.Drop += menuList_Drop;
                    pluginlist.ItemsSource = NewArea[i].PluginList;
                    if (NewArea[i].MenuForm == "normal")
                    {
                        pluginlist.Style = mainWindow.FindResource("MetroListBoxFourthMenuListStyle") as Style;
                    }
                    else if (NewArea[i].MenuForm == "large")
                    {
                        pluginlist.Style = mainWindow.FindResource("LargeListBoxFourthMenuListStyle") as Style;
                    }
                    else if (NewArea[i].MenuForm == "small")
                    {
                        pluginlist.Style = mainWindow.FindResource("SmallListBoxFourthMenuListStyle") as Style;
                    }
                    pluginPanel.Children.Insert(pluginPanel.Children.Count - 1, pluginlist);//一个WrapPanel里添加了两个ListBox
                }

                DockPanel _newPanel = new DockPanel();
                _newPanel.Uid = NewArea[i].AreaID;
                _newPanel.Width = NewArea[i].AreaWidth;
                _newPanel.Height = NewArea[i].AreaHeight;
                _newPanel.Children.Add(_title);
                _newPanel.Children.Add(pluginPanel);//一个DockPanel里添加了一个UnitAreaSeting和一个WrapPanel
                Canvas.SetLeft(_newPanel, NewArea[i].LeftSpan);
                Canvas.SetTop(_newPanel, NewArea[i].TopSpan);
                _panel.Children.Add(_newPanel);
            }
            ThumbCanvas();
        }

        void ParamsModel_TextChangedClick(object sender, TextChangedEventArgs e)
        {
            UnitAreaSeting areaParent = ((((TextBox)sender).Parent as DockPanel).Parent as Grid).Parent as UnitAreaSeting;
            if (areaParent != null)
            {
                AreaMenu nowArea = NewArea.FirstOrDefault(it => it.AreaID.Equals(areaParent.Uid));
                nowArea.AreaName = areaParent.ParamsModel.AreaName;
                WriteFile();
                areaParent.Focus();
            }
        }


        private void ThumbDragMove(object sender, DragDeltaEventArgs e)
        {
            DockPanel parentPanel = GetParentObject<DockPanel>(sender as Thumb);
            if (!isOverRender)
            {
                //Canvas.SetLeft(parentPanel, Canvas.GetLeft(parentPanel) + e.HorizontalChange);
                //Canvas.SetTop(parentPanel, Canvas.GetTop(parentPanel) + e.VerticalChange);
                if (Canvas.GetLeft(parentPanel) + e.HorizontalChange < 0)
                {
                    Canvas.SetLeft(parentPanel, 5);
                }
                else
                {
                    if (Canvas.GetLeft(parentPanel) + parentPanel.ActualWidth > (parentPanel.Parent as Canvas).ActualWidth)
                    {
                        Canvas.SetLeft(parentPanel, (parentPanel.Parent as Canvas).ActualWidth - parentPanel.ActualWidth);
                    }
                    else Canvas.SetLeft(parentPanel, Canvas.GetLeft(parentPanel) + e.HorizontalChange);
                }
                if (Canvas.GetTop(parentPanel) + e.VerticalChange < 0)
                {
                    Canvas.SetTop(parentPanel, 5);
                }
                else
                {
                    Canvas.SetTop(parentPanel, Canvas.GetTop(parentPanel) + e.VerticalChange);
                }
                //保存拖动位置
                NewArea.FirstOrDefault(it => it.AreaID.Equals(parentPanel.Uid)).LeftSpan = Canvas.GetLeft(parentPanel);
                NewArea.FirstOrDefault(it => it.AreaID.Equals(parentPanel.Uid)).TopSpan = Canvas.GetTop(parentPanel);
                WriteFile();
            }

        }

        private ListBoxItem mListItem = null;
        private ListBox TestPanel = null;
        /// <summary>
        /// 鼠标拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            TestPanel = sender as ListBox;
            TestPanel.AllowDrop = true;
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;
            Point pos = e.GetPosition(TestPanel);
            HitTestResult result = VisualTreeHelper.HitTest(TestPanel, pos);
            if (result == null)
                return;

            mListItem = GetParentObject<ListBoxItem>(result.VisualHit); // Find your actual visual you want to drag
            if (mListItem == null)
                return;

            DataObject dataObject = new DataObject(mListItem.Content);
            // Here, we should notice that dragsource param will specify on which 
            // control the drag&drop event will be fired
            System.Windows.DragDrop.DoDragDrop(TestPanel, dataObject, DragDropEffects.Copy);//开始拖动

        }
        /// <summary>
        /// 拖动后放下的位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuList_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (TestPanel == null)
                    return;
                Point pos = e.GetPosition(TestPanel);
                HitTestResult result = VisualTreeHelper.HitTest(TestPanel, pos);
                if (result == null)
                    return;

                ListBoxItem selectedItem = GetParentObject<ListBoxItem>(result.VisualHit);
                if (selectedItem == null)
                    return;
                //把拖动控件移动到制定位置，且移除被拖动的项 操作数据源
                ObservableCollection<MenuModel> DataSource = TestPanel.ItemsSource as ObservableCollection<MenuModel>;
                DataSource.Insert(DataSource.IndexOf(selectedItem.Content as MenuModel), (mListItem.Content as MenuModel).Copy());
                DataSource.Remove(mListItem.Content as MenuModel);
                TestPanel.ItemsSource = null;
                TestPanel.ItemsSource = DataSource;
                mListItem = null;
                WriteFile();
            }
            catch (Exception)
            {

            }
        }

        ///<summary>
        /// 区域改变大小和拖动
        /// </summary>
        private void ThumbCanvas(UIElement lElementName = null, bool IsLock = true)
        {
            //实现拖动和改变大小
            if (_panel == null) return;
            var layer = AdornerLayer.GetAdornerLayer(_panel);
            if (lElementName == null)
            {
                foreach (UIElement ui in _panel.Children)
                {
                    MyCanvasAdorner MyCanvas = new MyCanvasAdorner(ui, !IsLock);
                    MyCanvas.CurrentUElementSizeChanged += MyCanvas_CurrentUElementSizeChanged;
                    layer.Add(MyCanvas);

                }
            }
            else
            {
                MyCanvasAdorner MyCanvas = new MyCanvasAdorner(lElementName, !IsLock);
                MyCanvas.CurrentUElementSizeChanged += MyCanvas_CurrentUElementSizeChanged;
                layer.Add(MyCanvas);
            }
        }

        ///<summary>
        ///区域改变存入文件
        /// </summary>
        private void WriteFile()
        {

            StreamWriter sw = new StreamWriter(menuPath, false);
            sw.Write(JsonHelper.ToJson(NewArea));
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

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

        /// <summary>
        /// 获得子控件
        /// </summary>
        /// <typeparam name="T">要获得控件类名</typeparam>
        /// <param name="obj">当前控件名</param>
        /// <param name="name">要查询子控件名</param>
        /// <returns>要获得控件类名</returns>
        private T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Uid == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        ///<summary>
        ///回调事件，存下区域改变的大小
        /// </summary>
        void MyCanvas_CurrentUElementSizeChanged(object sender)
        {
            DockPanel newArea = sender as DockPanel;
            if (newArea != null)
            {
                UnitAreaSeting UnitArea = GetChildObject<UnitAreaSeting>(newArea, newArea.Uid);
                UnitArea.Width = newArea.ActualWidth;
                NewArea.FirstOrDefault(it => it.AreaID.Equals(newArea.Uid)).AreaWidth = newArea.ActualWidth;
                NewArea.FirstOrDefault(it => it.AreaID.Equals(newArea.Uid)).AreaHeight = newArea.ActualHeight;
                NewArea.FirstOrDefault(it => it.AreaID.Equals(newArea.Uid)).LeftSpan = Canvas.GetLeft(newArea);
                NewArea.FirstOrDefault(it => it.AreaID.Equals(newArea.Uid)).TopSpan = Canvas.GetTop(newArea);
                WriteFile();
                //newArea.UpdateLayout();
                //isOverRender = false;
                //OverRideDrawingPanelArea(newArea);
                DrawingPanelArea();
            }
        }

        ///<summary>
        ///区域title“设置”按钮回调事件
        /// </summary>
        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            string res = ((VicMenuItemNormal)sender).Tag.ToString();
            UnitAreaSeting areaParent = ((((((((VicMenuItemNormal)sender).Parent as VicMenuItemNormal).Parent as VicMenuItemNormal).Parent as VicMenuNormal).Parent as DockPanel)).Parent as Grid).Parent as UnitAreaSeting;
            if (res.Equals("Largeico"))
            {
                if (areaParent != null)
                {
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == areaParent.Uid)
                        {

                            WrapPanel wrapPanelArea = GetChildObject<WrapPanel>(panel, panel.Uid);
                            if (wrapPanelArea != null && wrapPanelArea.Children.Count == 2)
                            {
                                ListBox pluginArea = wrapPanelArea.Children[0] as ListBox;
                                if (pluginArea != null) pluginArea.Style = mainWindow.FindResource("LargeListBoxFourthMenuListStyle") as Style;
                                ListBox AddApplyArea = wrapPanelArea.Children[1] as ListBox;
                                AddApplyArea.Style = area.FindResource("addLargeApply") as Style;

                            }
                            else if (wrapPanelArea != null && wrapPanelArea.Children.Count == 1)
                            {

                                ListBox AddApplyArea = wrapPanelArea.Children[0] as ListBox;
                                AddApplyArea.Style = area.FindResource("addLargeApply") as Style;
                            }
                            NewArea.FirstOrDefault(it => it.AreaID.Equals(areaParent.Uid)).MenuForm = "large";
                        }
                    }
                }

            }
            if (res.Equals("Mediamicl"))
            {
                if (areaParent != null)
                {
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == areaParent.Uid)
                        {
                            WrapPanel wrapPanelArea = GetChildObject<WrapPanel>(panel, panel.Uid);
                            if (wrapPanelArea != null && wrapPanelArea.Children.Count == 2)
                            {
                                ListBox pluginArea = wrapPanelArea.Children[0] as ListBox;
                                if (pluginArea != null) pluginArea.Style = mainWindow.FindResource("MetroListBoxFourthMenuListStyle") as Style;
                                ListBox AddApplyArea = wrapPanelArea.Children[1] as ListBox;
                                AddApplyArea.Style = area.FindResource("addApply") as Style;

                            }
                            else if (wrapPanelArea != null && wrapPanelArea.Children.Count == 1)
                            {

                                ListBox AddApplyArea = wrapPanelArea.Children[0] as ListBox;
                                AddApplyArea.Style = area.FindResource("addApply") as Style;
                            }
                            NewArea.FirstOrDefault(it => it.AreaID.Equals(areaParent.Uid)).MenuForm = "normal";
                        }
                    }
                }
            }
            if (res.Equals("Smallico"))
            {
                if (areaParent != null)
                {
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == areaParent.Uid)
                        {
                            WrapPanel wrapPanelArea = GetChildObject<WrapPanel>(panel, panel.Uid);
                            if (wrapPanelArea != null && wrapPanelArea.Children.Count == 2)
                            {
                                ListBox pluginArea = wrapPanelArea.Children[0] as ListBox;
                                if (pluginArea != null) pluginArea.Style = mainWindow.FindResource("SmallListBoxFourthMenuListStyle") as Style;
                                ListBox AddApplyArea = wrapPanelArea.Children[1] as ListBox;
                                AddApplyArea.Style = area.FindResource("addSmallApply") as Style;

                            }
                            else if (wrapPanelArea != null && wrapPanelArea.Children.Count == 1)
                            {
                                ListBox AddApplyArea = wrapPanelArea.Children[0] as ListBox;
                                AddApplyArea.Style = area.FindResource("addSmallApply") as Style;
                            }
                            NewArea.FirstOrDefault(it => it.AreaID.Equals(areaParent.Uid)).MenuForm = "small";
                        }
                    }
                }
            }
            WriteFile();
        }

        private void SecondMenuItemClick(object sender, RoutedEventArgs e)
        {
            string res = ((VicMenuItemNormal)sender).Tag.ToString();
            UnitAreaSeting areaParent = (((((((VicMenuItemNormal)sender).Parent as VicMenuItemNormal).Parent as VicMenuNormal).Parent as DockPanel)).Parent as Grid).Parent as UnitAreaSeting;
            //重命名区域 
            if (res.Equals("RenName"))
            {
                //在此只是让文本框聚集和文本选中，不做任何事情
            }
            //编辑 
            if (res.Equals("Compile"))
            {
                if (areaParent != null)
                {
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == areaParent.Uid)
                        {
                            WrapPanel wrapPanelArea = GetChildObject<WrapPanel>(panel, panel.Uid);
                            if (wrapPanelArea != null && wrapPanelArea.Children.Count == 2)
                            {
                                ListBox pluginArea = wrapPanelArea.Children[0] as ListBox;
                                AreaMenu NowArea = new AreaMenu();
                                NowArea = NewArea.FirstOrDefault(it => it.AreaID.Equals(areaParent.Uid));

                                if (NowArea.MenuForm == "normal")
                                {
                                    pluginArea.Style = area.FindResource("PopupMenuDelListPluginStyle") as Style;
                                }
                                else if (NowArea.MenuForm == "large")
                                {
                                    pluginArea.Style = area.FindResource("PopupMenuDelListLargePluginStyle") as Style;
                                }
                                else if (NowArea.MenuForm == "small")
                                {
                                    pluginArea.Style = area.FindResource("PopupMenuDelListSmallPluginStyle") as Style;
                                }


                            }
                            else
                            {
                                VicMessageBoxNormal.Show("当前没有要编辑的插件", "消息提示框");
                            }

                        }

                    }
                }

            }
            //删除区域 
            if (res.Equals("DeleteArea"))
            {
                if (VicMessageBoxNormal.Show("确定要删除当前区域吗？删除后，区域中的应用也将同时删除！", "消息提示框", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    AreaMenu area = new AreaMenu();
                    area = NewArea.FirstOrDefault(it => it.AreaID.Equals(areaParent.Uid));
                    NewArea.Remove(area);
                }
                WriteFile();
                DrawingPanelArea();
            }
        }

        ///<summary>
        ///区域title“锁定”按钮和“折叠/展开”回调事件
        /// </summary>
        private void BtnClick(object sender, RoutedEventArgs e)
        {
            string res = ((VicButtonNormal)sender).Tag.ToString();
            UnitAreaSeting areaParent = (((((VicButtonNormal)sender).Parent as StackPanel).Parent as DockPanel).Parent as Grid).Parent as UnitAreaSeting;
            if (areaParent == null)
                return;
            if (res.Equals("btnDeblocking"))//单击解锁图标
            {
                isOverRender = true;

                areaParent.ParamsModel.DeblockingState = Visibility.Collapsed;
                areaParent.ParamsModel.LockingState = Visibility.Visible;
                areaParent.ParamsModel.IsEditItem = Visibility.Visible;
                //重绘，去掉拖动
                OverRideDrawingPanelArea(areaParent.Parent as DockPanel);

            }
            if (res.Equals("btnLocking"))//单击锁定图标
            {
                isOverRender = false;
                areaParent.ParamsModel.DeblockingState = Visibility.Visible;
                areaParent.ParamsModel.LockingState = Visibility.Collapsed;
                areaParent.ParamsModel.IsEditItem = Visibility.Collapsed;
                //添加拖动事件
                WrapPanel wrapPanelArea = GetChildObject<WrapPanel>(areaParent.Parent as DockPanel, (areaParent.Parent as DockPanel).Uid);
                if (wrapPanelArea != null && wrapPanelArea.Children.Count == 2)
                {
                    ListBox pluginArea = wrapPanelArea.Children[0] as ListBox;
                    pluginArea.PreviewMouseMove += menuList_PreviewMouseMove;
                }
                areaParent.ParamsModel.ThumbDragMoveClick += ThumbDragMove;
                ThumbCanvas(areaParent.Parent as DockPanel, false);
            }

            if (res.Equals("btnFold"))
            {
                if (areaParent != null)
                {
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == areaParent.Uid)
                        {
                            panel.Children[1].Visibility = Visibility.Collapsed;
                            areaParent.ParamsModel.FoldState = Visibility.Collapsed;
                            areaParent.ParamsModel.UnfoldState = Visibility.Visible;
                            break;
                        }

                    }
                }
            }
            if (res.Equals("btnUnfold"))
            {
                if (areaParent != null)
                {
                    foreach (DockPanel panel in _panel.Children)
                    {
                        if (panel.Uid == areaParent.Uid)
                        {
                            panel.Children[1].Visibility = Visibility.Visible;
                            areaParent.ParamsModel.FoldState = Visibility.Visible;
                            areaParent.ParamsModel.UnfoldState = Visibility.Collapsed;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  重新渲染当前区域
        /// </summary>
        private void OverRideDrawingPanelArea(UIElement dockPanel)
        {

            ReadMenuJsonFile();
            //展示myMenu.json文件
            for (int i = 0; i < NewArea.Count; i++)
            {
                if (NewArea[i].AreaID == dockPanel.Uid)
                {

                    UnitAreaSeting _title = new UnitAreaSeting();
                    _title.ParamsModel.TitleWidth = NewArea[i].AreaWidth;
                    _title.Uid = NewArea[i].AreaID;
                    if (isOverRender)
                    {
                        _title.ParamsModel.DeblockingState = Visibility.Collapsed;
                        _title.ParamsModel.LockingState = Visibility.Visible;
                        _title.ParamsModel.IsEditItem = Visibility.Visible;
                    }
                    else
                    {
                        _title.ParamsModel.ThumbDragMoveClick += ThumbDragMove;
                    }
                    DockPanel.SetDock(_title, Dock.Top);
                    _title.ParamsModel.BtnDeblockingClick += BtnClick;
                    _title.MenuItemIcoClick += MenuItemClick;
                    _title.ParamsModel.TextChangedClick += ParamsModel_TextChangedClick;
                    _title.SecondMenuItemIcoClick += SecondMenuItemClick;
                    _title.ParamsModel.AreaName = NewArea[i].AreaName;
                    _title.VerticalContentAlignment = VerticalAlignment.Center;
                    _title.HorizontalContentAlignment = HorizontalAlignment.Center;
                    _title.Foreground = mainWindow.FindResource("MetroFGColor") as Brush;



                    WrapPanel pluginPanel = new WrapPanel();
                    pluginPanel.Background = mainWindow.FindResource("MetroBGColor") as Brush;
                    pluginPanel.Uid = NewArea[i].AreaID;
                    pluginPanel.Orientation = Orientation.Horizontal;
                    ListBox addapplyStyle = new ListBox();
                    ListBoxItem _item = new ListBoxItem();
                    addapplyStyle.Items.Add(_item);
                    if (NewArea[i].MenuForm == "normal")
                    {
                        addapplyStyle.Style = area.FindResource("addApply") as Style;
                    }
                    else if (NewArea[i].MenuForm == "large")
                    {
                        addapplyStyle.Style = area.FindResource("addLargeApply") as Style;
                    }
                    else if (NewArea[i].MenuForm == "small")
                    {
                        addapplyStyle.Style = area.FindResource("addSmallApply") as Style;
                    }
                    pluginPanel.Children.Add(addapplyStyle);

                    if (NewArea[i].PluginList.Count != 0)
                    {
                        ListBox pluginlist = new ListBox();
                        pluginlist.Drop += menuList_Drop;
                        if (!isOverRender)
                        {
                            pluginlist.PreviewMouseMove += menuList_PreviewMouseMove;
                        }
                        pluginlist.ItemsSource = NewArea[i].PluginList;
                        if (NewArea[i].MenuForm == "normal")
                        {
                            pluginlist.Style = mainWindow.FindResource("MetroListBoxFourthMenuListStyle") as Style;
                        }
                        else if (NewArea[i].MenuForm == "large")
                        {
                            pluginlist.Style = mainWindow.FindResource("LargeListBoxFourthMenuListStyle") as Style;
                        }
                        else if (NewArea[i].MenuForm == "small")
                        {
                            pluginlist.Style = mainWindow.FindResource("SmallListBoxFourthMenuListStyle") as Style;
                        }
                        pluginPanel.Children.Insert(pluginPanel.Children.Count - 1, pluginlist);//一个WrapPanel里添加了两个ListBox
                    }

                    DockPanel _newPanel = new DockPanel();
                    _newPanel.Uid = NewArea[i].AreaID;
                    _newPanel.Width = NewArea[i].AreaWidth;
                    _newPanel.Height = NewArea[i].AreaHeight;
                    _newPanel.Children.Add(_title);
                    _newPanel.Children.Add(pluginPanel);//一个DockPanel里添加了一个UnitAreaSeting和一个WrapPanel
                    Canvas.SetLeft(_newPanel, NewArea[i].LeftSpan);
                    Canvas.SetTop(_newPanel, NewArea[i].TopSpan);
                    _panel.Children.Add(_newPanel);
                    _panel.Children.Remove(dockPanel);

                    break;
                }
            }
        }

        ///<summary>
        /// 根据UserCode和ProductId取菜单
        /// </summary>
        private string channelId = string.Empty;
        private void GetPersonMenu()
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "12");
            contentDic.Add("configsystemid", "11");
            contentDic.Add("modelid", "feidao-model-pub_user_setting-0001");
            List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "pub_user_setting");
            List<Dictionary<string, object>> tableConList = new List<Dictionary<string, object>>();
            Dictionary<string, object> tableConDic = new Dictionary<string, object>();
            tableConDic.Add("productid", ProductId);
            tableConDic.Add("userCode", UserCode);
            tableConList.Add(tableConDic);
            conDic.Add("tablecondition", tableConList);
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"pub_user_setting\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                DataRow[] staffDrs = dt.Select(string.Format("userCode = '{0}'", UserCode));
                if (staffDrs.Count() != 0)//从服器取
                {
                    string areaMenuList = staffDrs[0]["custom_menu"].ToString();
                    NewArea = JsonHelper.ToObject<ObservableCollection<AreaMenu>>(areaMenuList);
                }
            }

        }

        ///<summary>
        /// 保存个人菜单到数据库
        /// </summary>
        private void SavePersonMenu()
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "12");
            contentDic.Add("configsystemid", "11");
            contentDic.Add("modelid", "feidao-model-pub_user_setting-0001");
            List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "pub_user_setting");
            List<Dictionary<string, object>> tableConList = new List<Dictionary<string, object>>();
            Dictionary<string, object> tableConDic = new Dictionary<string, object>();
            tableConDic.Add("productid", ProductId);
            tableConDic.Add("userCode", UserCode);
            tableConList.Add(tableConDic);
            conDic.Add("tablecondition", tableConList);
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"pub_user_setting\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                DataRow[] staffDrs = dt.Select(string.Format("userCode = '{0}'", UserCode));
                if (dt.Rows.Count == 0 || staffDrs.Count() == 0) //第一次无记录时先存到服务器
                {

                    DataRow dr = dt.NewRow();
                    dr["_id"] = Guid.NewGuid();
                    dr["userCode"] = UserCode;
                    dr["productid"] = ProductId;
                    dr["custom_menu"] = JsonHelper.ToJson(NewArea);
                    dt.Rows.Add(dr);
                }
                else if (staffDrs.Count() == 1)  //用户多次修改Plugin列表时存到服务器
                {
                    staffDrs[0]["custom_menu"] = JsonHelper.ToJson(NewArea);
                }

                DataMessageOperation dataOp = new DataMessageOperation();
                dataOp.SaveData(channelId, "[\"pub_user_setting\"]");
                string MessageType1 = "MongoDataChannelService.saveBusiData";
                Dictionary<string, object> contentDic1 = new Dictionary<string, object>();
                contentDic1.Add("systemid", "12");
                contentDic1.Add("configsystemid", "11");
                contentDic1.Add("modelid", "feidao-model-pub_user_setting-0001");
                contentDic1.Add("DataChannelId", channelId);
                Dictionary<string, object> resultDic = messageOp.SendSyncMessage(MessageType1, contentDic1, "JSON");

               
                if (resultDic != null && !resultDic["ReplyMode"].ToString().Equals("0"))
                {
                    Dictionary<string, object> replyContent = new Dictionary<string, object>();
                    replyContent = JsonHelper.ToObject<Dictionary<string, object>>(resultDic["ReplyContent"].ToString());
                    if (replyContent != null && replyContent.Keys.Contains("msg"))
                    {

                        VicMessageBoxNormal.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        VicMessageBoxNormal.Show("保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                        return;
                    }

                }
                else
                {
                    VicMessageBoxNormal.Show("保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //删除本地文件
                if (File.Exists(menuPath))
                {
                    FileInfo fi = new FileInfo(menuPath);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    {
                        fi.Attributes = FileAttributes.Normal;
                    }
                    File.Delete(menuPath);//直接删除本地文件   
                }
            }
        }

        #endregion

        #endregion
    }
}
