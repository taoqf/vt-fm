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
using Victop.Frame.DataChannel;
using System.Data;
using System.Windows;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Windows.Controls;
using Victop.Wpf.Controls;
using Victop.Server.Controls;
using System.Threading;
using PortalFramePlugin.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Reflection;
using PortalFramePlugin.Views;


namespace PortalFramePlugin.ViewModels
{
    public class UCPortalWindowViewModel : ModelBase
    {
        #region 字段
        private Window mainWindow;
        private Grid gridTitle;
        private ObservableCollection<MenuModel>  systemMenuListEnterprise;
        private ObservableCollection<MenuModel> systemMenuListLocal;
        private ObservableCollection<MenuModel> systemThirdLevelMenuList;
        private ObservableCollection<MenuModel> systemFourthLevelMenuList;
        private MenuModel selectedSecondMenuModel;
        private MenuModel selectedThirdMenuModel;
        private ObservableCollection<Victop.Wpf.Controls.TabItem> tabItemList;
        private Victop.Wpf.Controls.TabItem selectedTabItem;

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
        public ObservableCollection<Victop.Wpf.Controls.TabItem> TabItemList
        {
            get
            {
                if (tabItemList == null)
                {
                    tabItemList = new ObservableCollection<Victop.Wpf.Controls.TabItem>();
                    Victop.Wpf.Controls.TabItem homeItem = new Victop.Wpf.Controls.TabItem();
                    homeItem.Name = "homeItem";
                    homeItem.AllowDelete = false;
                    homeItem.Header = "ERP主页";
                    ScrollViewer scroll = new ScrollViewer();
                    scroll.Content = new UCPluginContainer();
                    homeItem.Content = scroll;
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
        public Victop.Wpf.Controls.TabItem SelectedTabItem
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
                    ChangeFrameWorkTheme();
                    LoadMenuListLocal();
                });
            }
        }
        private void ChangeFrameWorkTheme()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "ServerCenterService.ChangeTheme");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("ServiceParams", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            string messageStr = JsonHelper.ToJson(messageDic);
            PluginMessage pluginMessage = new PluginMessage();
            pluginMessage.SendMessage("", messageStr, new WaitCallback(ChangeFrameWorkLanguage));
        }
        private void ChangeFrameWorkLanguage(object message)
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "ServerCenterService.ChangeLanguage");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("ServiceParams", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            string messageStr = JsonHelper.ToJson(messageDic);
            PluginMessage pluginMessage = new PluginMessage();
            pluginMessage.SendMessage("", messageStr, null);
        }
        #endregion

        #region 窗体移动命令
        /// <summary>窗体移动命令 </summary>
        public ICommand gridTitleMouseMoveCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    gridTitle = (Grid)x;
                    gridTitle.MouseMove += gridTitle_MouseMove;
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

        #region 窗体关闭命令
        /// <summary>窗体关闭命令 </summary>
        public ICommand btnCloseClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MessageBoxResult result = MessageBox.Show("确定要退出么？", "", MessageBoxButton.YesNo);
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

        #region 本地选中菜单改变命令
        public ICommand listSecondMenuLocalSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        MenuModel menuModel = (MenuModel)x;
                        SystemThirdLevelMenuList = menuModel.SystemMenuList;
                        SelectedTabItem = TabItemList[0];
                    }
                });
            }
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
                        MenuModel menuModel = (MenuModel)x;
                        SystemThirdLevelMenuList = menuModel.SystemMenuList;
                        SelectedTabItem = TabItemList[0];
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
                return new RelayCommand(() =>
                {
                    if(SelectedThirdMenuModel!=null)
                        SystemFourthLevelMenuList = SelectedThirdMenuModel.SystemMenuList;
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
                        LoadPlugin(menuModel);
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
                        LoadPlugin(menuModel);
                    }
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
            //LoadMenuListLocal();
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
            //DataTable dt = FillDataTable(SystemMenuListEnterprise);
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
            menuModel.FormId = item.FormId;
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
            return menuModel;
        }
        #endregion

        #region 加载本地菜单集合
        /// <summary>加载本地菜单集合 </summary>
        private void LoadMenuListLocal()
        {
            SystemMenuListLocal.Clear();
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["userplugins"];
            ReadLocalMenuListFromXml(FilePath);
        }
        /// <summary>读取菜单信息</summary>
        private void ReadLocalMenuListFromXml(string PluginFilePath)
        {
            XDocument xDoc = XDocument.Load(PluginFilePath);
            XElement root = xDoc.Element("MenuInfo");
            foreach (var item in root.Elements())
            {
                MenuModel menuModel = GetPluginInfoModel(item);
                menuModel = CreatLocalMenuModel(item,menuModel);
                SystemMenuListLocal.Add(menuModel);
            }
        }
        private MenuModel CreatLocalMenuModel(XElement element, MenuModel menuModel)
        {
            foreach (var item in element.Elements())
            {
                MenuModel childMenuModel = GetPluginInfoModel(item);
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
            //plugin.MenuId = element.Attribute("action").Value;
            plugin.ResourceName = element.Attribute("action").Value;
            return plugin;
        }
        #endregion

        #region 将菜单模型转成DataTable
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        private DataTable FillDataTable(ObservableCollection<MenuModel> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);
            foreach (MenuModel model in modelList)
            {
                dt = GetDataTable(dt, model);
            }
            return dt;
        }
        private DataTable GetDataTable(DataTable dt,MenuModel menuModel)
        {
            foreach (MenuModel item in menuModel.SystemMenuList)
            {
                dt = GetDataTable(dt, item); 
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(MenuModel).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(item, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private DataTable CreateData(MenuModel model)
        {
            DataTable dataTable = new DataTable(typeof(MenuModel).Name);
            foreach (PropertyInfo propertyInfo in typeof(MenuModel).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
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
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="selectedFourthMenu">当前选择的四级菜单</param>
        private void LoadPlugin(MenuModel selectedFourthMenu)
        {
            if (selectedFourthMenu.ResourceName == null)
                selectedFourthMenu.ResourceName = ConfigurationManager.AppSettings["runplugin"];
            if (selectedFourthMenu.ResourceName != null && selectedFourthMenu.ResourceName.Contains("Plugin"))
            {
                Dictionary<string, string> messageDic = new Dictionary<string, string>();
                messageDic.Add("MessageType", "PluginService.PluginRun");
                Dictionary<string, string> contentDic = new Dictionary<string, string>();
                contentDic.Add("PluginName", selectedFourthMenu.ResourceName);
                contentDic.Add("PluginPath", "");
                messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
                new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(PluginShow));
            }
        }

        private void PluginShow(object message)
        {
            if (JsonHelper.ReadJsonString(message.ToString(), "ReplyMode").Equals("1"))
            {
                ActivePluginManager pluginManager = new ActivePluginManager();
                ActivePluginInfo pluginInfo = pluginManager.GetActivePlugins()[JsonHelper.ReadJsonString(message.ToString(), "MessageId")];
                switch (pluginInfo.ShowType)
                {
                    case 0:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new WaitCallback(DoWork), pluginInfo);
                        break;
                    case 1:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new WaitCallback(CtrlDoWork), pluginInfo);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show(JsonHelper.ReadJsonString(message.ToString(), "ReplyAlertMessage"));
            }
        }
        private void CtrlDoWork(object pluginInfo)
        {
            UserControl userctrl = ((IPlugin)((ActivePluginInfo)pluginInfo).PluginInstance).StartControl;
            userctrl.Uid = ((ActivePluginInfo)pluginInfo).ObjectId;
            Victop.Wpf.Controls.TabItem tabItem = new Victop.Wpf.Controls.TabItem();
            tabItem.Header = ((IPlugin)((ActivePluginInfo)pluginInfo).PluginInstance).PluginTitle;
            tabItem.Content = userctrl;
            tabItem.AllowDelete = true;
            TabItemList.Add(tabItem);
        }
        #endregion

        #region 窗体移动
        void gridTitle_MouseMove(object sender, MouseEventArgs e)
        {
            gridTitle.AllowDrop = true;
            if (e.LeftButton == MouseButtonState.Pressed)
                mainWindow.DragMove();
        }
        #endregion

        #region 用户登录
        private void UserLogin()
        {
            //new GalleryManager().SetCurrentGalleryId(Victop.Frame.CoreLibrary.Enums.GalleryEnum.ENTERPRISE);
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginRun");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("PluginName", "UserLoginPlugin");
            contentDic.Add("PluginPath", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(PluginShow));
        }
        /// <summary>打开插件</summary>
        private void DoWork(object pluginInfo)
        {
            Window win = ((IPlugin)((ActivePluginInfo)pluginInfo).PluginInstance).StartWindow;
            win.Uid = ((ActivePluginInfo)pluginInfo).ObjectId;
            bool? result = win.ShowDialog();
            //UpdateMenu();
            LoadStandardMenu();
        }

        #endregion
        #endregion

    }
}
