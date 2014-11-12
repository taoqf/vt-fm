using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.MessageManager;
using System.Data;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using Victop.Wpf.Controls;
using Victop.Server.Controls;
using System.Threading;
using PortalFramePlugin.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Reflection;
using PortalFramePlugin.Views;
using Victop.Frame.SyncOperation;
using System.Windows.Navigation;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Drawing;


namespace PortalFramePlugin.ViewModels
{
    public class UCPortalWindowViewModel : ModelBase
    {
        #region 字段
        private Window mainWindow;
        private Grid gridTitle;
        private ObservableCollection<MenuModel> systemMenuListEnterprise;
        private ObservableCollection<MenuModel> systemMenuListLocal;
        private ObservableCollection<MenuModel> systemThirdLevelMenuList;
        private ObservableCollection<MenuModel> systemFourthLevelMenuList;
        private MenuModel selectedSecondMenuModel;
        private MenuModel selectedThirdMenuModel;
        private ObservableCollection<VicTabItemNormal> tabItemList;
        private VicTabItemNormal selectedTabItem;
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
        private List<MenuModel> localMenuList = new List<MenuModel>();

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
                    //browser.Source = new Uri("http://www.victop.com");
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
                    Rect rect = SystemParameters.WorkArea;
                    mainWindow.MaxWidth = rect.Width;
                    mainWindow.MaxHeight = rect.Height;
                    mainWindow.WindowState = WindowState.Maximized;
                    ChangeFrameWorkTheme();
                    //LoadMenuListLocal();
                    LoadJsonMenuListLocal();
                    UserLogin();
                });
            }
        }
        private void ChangeFrameWorkTheme()
        {
            string messageType = "ServerCenterService.ChangeTheme";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("ServiceParams", "");
            MessageOperation messageOp = new MessageOperation();
            messageOp.SendMessage(messageType, contentDic);
            ChangeFrameWorkLanguage();
        }
        private void ChangeFrameWorkLanguage()
        {
            string messageType = "ServerCenterService.ChangeLanguage";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("ServiceParams", "");
            MessageOperation messageOp = new MessageOperation();
            messageOp.SendMessage(messageType, contentDic);
        }
        #endregion

        #region 窗体移动命令
        /// <summary>窗体移动命令 </summary>
        public ICommand gridTitleMouseMoveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    mainWindow.DragMove();
                });
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
                        btnMax.SetResourceReference(Button.StyleProperty, "btnMaxiStyle");
                        mainWindow.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        btnMax.SetResourceReference(Button.StyleProperty, "btnMaxiStyle");
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
                ScrollViewer scroll = new ScrollViewer();
                scroll.Content = new UCPluginContainer();
                TabItemList[0].Content = scroll;
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
                    if (!string.IsNullOrEmpty(tabCtrl.Uid))
                    {
                        MessageOperation messageOp = new MessageOperation();
                        string messageType = "PluginService.PluginStop";
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("ObjectId", tabCtrl.Uid);
                        messageOp.SendMessage(messageType, contentDic);
                    }
                    PluginOperation pluginOp = new PluginOperation();
                    ActivePluginNum = pluginOp.GetActivePluginList().Count;
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
            BaseResourceInfo resourceInfo = new BaseResourceManager().GetCurrentGalleryBaseResource();
            SystemMenuListEnterprise.Clear();
            if (resourceInfo != null && resourceInfo.ResourceMnenus.Count > 0)
            {
                foreach (MenuInfo item in resourceInfo.ResourceMnenus.Where(it => it.ParentMenu.Equals("0")))
                {
                    MenuModel menuModel = GetMenuModel(item);
                    menuModel = CreateMenuList(item.MenuId, resourceInfo.ResourceMnenus, menuModel);
                    SystemMenuListEnterprise.Add(menuModel);
                }
            }
            GetStandardMenuList(SystemMenuListEnterprise);
        }
        /// <summary>创建完整的菜单模型 </summary>
        private MenuModel CreateMenuList(string parentMenu, List<MenuInfo> fullMenuList, MenuModel parentModel)
        {
            foreach (MenuInfo item in fullMenuList.Where(it => it.ParentMenu.Equals(parentMenu)))
            {
                MenuModel menuModel = GetMenuModel(item);
                menuModel = CreateMenuList(item.MenuId, fullMenuList, menuModel);
                parentModel.SystemMenuList.Add(menuModel);
            }
            return parentModel;
        }
        /// <summary>获得菜单模型实例</summary>
        private MenuModel GetMenuModel(MenuInfo item)
        {
            MenuModel menuModel = new MenuModel();
            menuModel.MenuId = item.MenuId;
            menuModel.MenuName = item.MenuName;
            menuModel.Actived = item.Actived;
            menuModel.AutoOpenFlag = item.AutoOpenFlag;
            menuModel.BzSystemId = item.BzSystemId;
            menuModel.Compatible = item.Compatible;
            menuModel.DataFormId = item.DataFormId;
            menuModel.DefaultPrintTemplate = item.DefaultPrintTemplate;
            menuModel.DisplayType = item.DisplayType;
            menuModel.DocStatus = item.DocStatus;
            menuModel.EndPoint = item.EndPoint;
            menuModel.EndPointParam = item.EndPointParam;
            menuModel.ConfigSystemId = item.FormId;
            menuModel.FormMemo = item.FormMemo;
            menuModel.FormName = item.FormName;
            menuModel.HomeId = item.HomeId;
            menuModel.Id = item.Id;
            menuModel.MaxPrintCount = item.MaxPrintCount;
            menuModel.Memo = item.Memo;
            menuModel.OpenType = item.OpenType;
            menuModel.ParentMenu = item.ParentMenu;
            menuModel.PredocStatus = item.PredocStatus;
            menuModel.ResourceName = item.ResourceName;
            menuModel.ResourceTree = item.ResourceTree;
            menuModel.ResourceType = item.ResourceType;
            menuModel.SaveProject = item.SaveProject;
            menuModel.Sequence = item.Sequence;
            menuModel.Stamp = item.Stamp;
            menuModel.MenuCode = item.menuCode;
            MenuModel localModel = GetLocalMenuResoureName(item.menu_name, localMenuListEx);
            if (localModel != null)
            {
                menuModel.ResourceName = localModel.ResourceName;
                menuModel.ActionType = localModel.ActionType;
                menuModel.BzSystemId = localModel.BzSystemId;
                menuModel.FitDataPath = localModel.FitDataPath;
                menuModel.ActionCADName = localModel.ActionCADName;
                menuModel.ConfigSystemId = localModel.ConfigSystemId;
                menuModel.SpaceId = localModel.SpaceId;
                menuModel.MenuNo = localModel.MenuNo;

            }
            return menuModel;
        }
        #endregion

        private MenuModel GetLocalMenuResoureName(string MenuName, ObservableCollection<MenuModel> MenuList)
        {
            MenuModel menuModel = MenuList.FirstOrDefault(it => it.MenuName.Equals(MenuName));
            if (menuModel == null)
            {
                foreach (MenuModel item in MenuList)
                {
                    menuModel = GetLocalMenuResoureName(MenuName, item.SystemMenuList);
                    if (menuModel != null)
                        break;
                }
            }
            return menuModel;
        }

        #region 加载本地菜单集合
        private MenuModel CreatLocalMenuModel(XElement element, MenuModel menuModel)
        {
            foreach (var item in element.Elements())
            {
                MenuModel childMenuModel = GetPluginInfoModel(item);
                if (item.Name.LocalName.Equals("Trade"))
                {
                    localMenuList.Add(childMenuModel);
                }
                childMenuModel = CreatLocalMenuModel(item, childMenuModel);
                menuModel.SystemMenuList.Add(childMenuModel);
            }
            return menuModel;
        }
        /// <summary>根据节点信息获取菜单实例</summary>
        private MenuModel GetPluginInfoModel(XElement element)
        {
            MenuModel plugin = new MenuModel();
            plugin.MenuName = element.Attribute("title").Value;
            plugin.IconUrl = element.Attribute("imageurl").Value;
            plugin.ResourceName = element.Attribute("action").Value;
            return plugin;
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
            #region 手动解析树型Json(暂不使用)
            //List<object> objList = JsonHelper.ToObject<List<object>>(menuList);
            //foreach (object obj in objList)
            //{
            //    MenuModel model = CreateMenuModel(obj.ToString());
            //    SystemMenuListLocal.Add(model);
            //}
            #endregion
        }
        #endregion

        #region 手动解析树型Json(2014-08-29 暂不使用)
        private ObservableCollection<MenuModel> CreateChildrenMenuList(string childStr)
        {
            ObservableCollection<MenuModel> childrenMenuList = new ObservableCollection<MenuModel>();
            if (string.IsNullOrEmpty(childStr)) return childrenMenuList;
            List<object> strList = JsonHelper.ToObject<List<object>>(childStr);
            foreach (object obj in strList)
            {
                MenuModel model = CreateMenuModel(obj.ToString());
                childrenMenuList.Add(model);
            }
            return childrenMenuList;
        }
        private MenuModel CreateMenuModel(string str)
        {
            MenuModel model = new MenuModel();
            model.MenuName = JsonHelper.ReadJsonString(str, "title");
            model.ActionType = JsonHelper.ReadJsonString(str, "actionType");
            model.ResourceName = JsonHelper.ReadJsonString(str, "actionName");
            model.ShowType = JsonHelper.ReadJsonString(str, "showType");
            model.IconUrl = JsonHelper.ReadJsonString(str, "iconUrl");
            model.BzSystemId = JsonHelper.ReadJsonString(str, "systemId");
            model.ConfigSystemId = JsonHelper.ReadJsonString(str, "formId");
            model.SpaceId = JsonHelper.ReadJsonString(str, "modelId");
            model.MenuNo = JsonHelper.ReadJsonString(str, "masterName");
            model.FitDataPath = JsonHelper.ReadJsonObject<List<Dictionary<string, object>>>(str, "fitDataPath");
            model.SystemMenuList = CreateChildrenMenuList(JsonHelper.ReadJsonString(str, "children"));
            return model;
        }
        #endregion

        #region 打开Json菜单下的插件(2014-08-29 新增)
        /// <summary>
        /// 打开Json菜单下的插件
        /// </summary>
        private void OpenJsonMenuPlugin(MenuModel selectedFourthMenu)
        {
            if (selectedFourthMenu.ResourceName == null)
                selectedFourthMenu.ResourceName = ConfigurationManager.AppSettings["runplugin"];
            if (selectedFourthMenu.ResourceName != null)
            {
                if (selectedFourthMenu.ActionType == "1")//启动插件
                {
                    PluginOperation pluginOp = new PluginOperation();
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add("systemid", selectedFourthMenu.BzSystemId);
                    paramDic.Add("configsytemid", selectedFourthMenu.ConfigSystemId);
                    paramDic.Add("spaceid", selectedFourthMenu.SpaceId);
                    paramDic.Add("menuno", selectedFourthMenu.MenuNo);
                    paramDic.Add("menucode", selectedFourthMenu.MenuCode);
                    paramDic.Add("authoritycode", selectedFourthMenu.HomeId);
                    paramDic.Add("fitdata", selectedFourthMenu.FitDataPath);
                    paramDic.Add("cadname", selectedFourthMenu.ActionCADName);
                    PluginModel pluginModel = pluginOp.StratPlugin(selectedFourthMenu.ResourceName, paramDic);
                    if (string.IsNullOrEmpty(pluginModel.ErrorMsg))
                    {
                        PluginShow(pluginModel,selectedFourthMenu.MenuName);
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
            newThirdLevelMenu.MenuId = new Guid().ToString();
            for (int i = secondLevelMenu.SystemMenuList.Count - 1; i >= 0; i--)
            {
                MenuModel thirdLevelMenu = secondLevelMenu.SystemMenuList[i];
                if (thirdLevelMenu.SystemMenuList.Count == 0)
                {
                    secondLevelMenu.SystemMenuList.Remove(thirdLevelMenu);

                    thirdLevelMenu.ParentMenu = newThirdLevelMenu.MenuId;
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
                    item.ParentMenu = thirdLevelMenu.MenuId;
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
                PluginOperation pluginOp = new PluginOperation();
                switch (pluginModel.PluginInterface.ShowType)
                {
                    case 0:
                        Window pluginWin = pluginModel.PluginInterface.StartWindow;
                        pluginWin.Uid = pluginModel.ObjectId;
                        pluginWin.Owner = mainWindow;
                        ActivePluginNum = pluginOp.GetActivePluginList().Count;
                        pluginWin.ShowDialog();
                        SendPluginCloseMessage(pluginModel);
                        break;
                    case 1:
                        UserControl pluginCtrl = pluginModel.PluginInterface.StartControl;
                        pluginCtrl.Uid = pluginModel.ObjectId;
                        VicTabItemNormal tabItem = new VicTabItemNormal();
                        tabItem.Header = string.IsNullOrEmpty(HeaderTitle) ? pluginModel.PluginInterface.PluginTitle : HeaderTitle;
                        tabItem.Content = pluginCtrl;
                        tabItem.AllowDelete = true;
                        tabItem.IsSelected = true;
                        TabItemList.Add(tabItem);
                        break;
                    default:
                        break;
                }
                ActivePluginNum = pluginOp.GetActivePluginList().Count;
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
            MessageOperation messageOp = new MessageOperation();
            string messageType = "PluginService.PluginStop";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("ObjectId", pluginModel.ObjectId);
            messageOp.SendMessage(messageType, contentDic);
        }
        #endregion

        #region 用户登录
        private void UserLogin()
        {
            PluginOperation pluginOp = new PluginOperation();
            string loginPlugin = ConfigurationManager.AppSettings["loginWindow"];
            PluginModel pluginModel = pluginOp.StratPlugin(loginPlugin);
            IPlugin PluginInstance = pluginModel.PluginInterface;
            Window loginWin = PluginInstance.StartWindow;
            loginWin.Uid = pluginModel.ObjectId;
            loginWin.Owner = mainWindow;
            bool? result = loginWin.ShowDialog();
            if (result == true)
            {
                MessageOperation messageOp = new MessageOperation();
                Dictionary<string, object> userDic = messageOp.SendMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                if (userDic != null)
                {
                    UserName = JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "UserName");
                    this.UserImg = this.DownLoadUserImg(JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "UserCode"), JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "UserImg"));
                }
                isFirstLogin = false;
                LoadStandardMenu();
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
                MessageOperation messageOperation = new MessageOperation();
                Dictionary<string, object> messageContent = new Dictionary<string, object>();
                Dictionary<string, string> address = new Dictionary<string, string>();
                address.Add("DownloadUrl", ConfigurationManager.AppSettings.Get("fileserverhttp") + "getfile?id=" + fileInfo);
                address.Add("DownloadToPath", path);
                messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                Dictionary<string, object> downResult = messageOperation.SendMessage("ServerCenterService.DownloadDocument", messageContent);
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
            PluginOperation pluginOp = new PluginOperation();
            PluginModel pluginModel = pluginOp.StratPlugin("ThemeManagerPlugin");
            IPlugin PluginInstance = pluginModel.PluginInterface;
            Window themeWin = PluginInstance.StartWindow;
            themeWin.Owner = mainWindow;
            themeWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            themeWin.ShowDialog();
        }

        #endregion

        #region 获取活动插件信息
        private void GetActivePluginInfo()
        {

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
            MessageOperation messageOp = new MessageOperation();
            messageOp.SendMessage(messageType, contentDic);
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
    }
}
