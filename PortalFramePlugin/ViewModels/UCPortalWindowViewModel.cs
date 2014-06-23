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


namespace PortalFramePlugin.ViewModels
{
    public class UCPortalWindowViewModel:ModelBase
    {
        #region 字段
        private Window mainWindow;
        private Grid gridTitle;
        private VicComboBoxNormal cmboxLanguage;
        #endregion

        #region 属性
        private ObservableCollection<MenuModel> systemMenuList;
        /// <summary>
        /// 菜单列表
        /// </summary>
        public ObservableCollection<MenuModel> SystemMenuList
        {
            get
            {
                if (systemMenuList == null)
                    systemMenuList = new ObservableCollection<MenuModel>();
                return systemMenuList;
            }
            set
            {
                if (systemMenuList != value)
                {
                    systemMenuList = value;
                    RaisePropertyChanged("SystemMenuList");
                }
            }
        }

        private MenuInfo selectedMenu;
        /// <summary>选定菜单 </summary>
        public MenuInfo SelectedMenu
        {
            get { return selectedMenu; }
            set
            {
                if (selectedMenu != value)
                {
                    selectedMenu = value;
                    RaisePropertyChanged("SelectedMenu");
                }
            }
        }

        private string infoMsg;
        /// <summary> 提示信息 </summary>
        public string InfoMsg
        {
            get
            {
                return infoMsg;
            }
            set
            {
                if (infoMsg != value)
                {
                    infoMsg = value;
                    RaisePropertyChanged("InfoMsg");
                }
            }
        }

        private DataTable myDt;
        public DataTable MyDt
        {
            get
            {
                if (myDt == null)
                    myDt = new DataTable();
                return myDt;
            }
            set
            {
                if (myDt != value)
                {
                    myDt = value;
                    RaisePropertyChanged("MyDt");
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
            pluginMessage.SendMessage("", messageStr,null);
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
                    }
                });
            }
        }
        #endregion

        private ObservableCollection<TableModel> tableShowList;

        public ObservableCollection<TableModel> TableShowList
        {
            get
            {
                if (tableShowList == null)
                    tableShowList = new ObservableCollection<TableModel>();
                return tableShowList;
            }
            set
            {
                if (tableShowList != value)
                {
                    tableShowList = value;
                    RaisePropertyChanged("TableShowList");
                }
            }
        }
        private TableModel selectedTable;

        public TableModel SelectedTable
        {
            get
            {
                if (selectedTable == null)
                    selectedTable = new TableModel();
                return selectedTable;
            }
            set
            {
                if (selectedTable != value)
                {
                    selectedTable = value;
                    RaisePropertyChanged("SelectedTable");
                }
            }
        }

        private ObservableCollection<Victop.Wpf.Controls.TabItem> tabItemList;

        public ObservableCollection<Victop.Wpf.Controls.TabItem> TabItemList
        {
            get
            {
                if (tabItemList == null)
                    tabItemList = new ObservableCollection<Victop.Wpf.Controls.TabItem>();
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

        public ICommand tviewMenuLoadedCommand
        {
            get
            {
                return new RelayCommand(() => {
                    UpdateMenu();
                });
            }
        }

        public ICommand tviewDoubleClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    if (x != null)
                    {
                        MenuModel menuModel = (MenuModel)x;
                        Dictionary<string, string> messageDic = new Dictionary<string, string>();
                        messageDic.Add("MessageType", "PluginService.PluginRun");
                        Dictionary<string, string> contentDic = new Dictionary<string, string>();
                        string plugin = ConfigurationManager.AppSettings["runplugin"];
                        contentDic.Add("PluginName", plugin);
                        contentDic.Add("PluginPath", "");
                        messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
                        new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(PluginShow));
                    }
                });
            }
        }

        public ICommand btnViewMenuClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeRequestData(), new System.Threading.WaitCallback(SearchData));
                });
            }
        }
        private MenuModel GetChildMenuList(List<MenuInfo> menuResource, string parentMenuId,MenuModel parentMenu)
        {
            foreach (MenuInfo item in menuResource.Where(it => it.ParentMenu == parentMenuId).OrderBy(it => it.Sequence))
            {
                MenuModel menu = new MenuModel()
                {
                    Id = item.Id,
                    MenuId = item.MenuId,
                    MenuName = item.MenuName,
                    DataFormId = item.DataFormId
                };
                menu = GetChildMenuList(menuResource, item.MenuId, menu);
                parentMenu.SystemMenuList.Add(menu);
            }
            return parentMenu;
        }
        #endregion

        #region 自定义方法
        private void UpdateMenu()
        {
            BaseResourceInfo resourceInfo = new BaseResourceManager().GetCurrentGalleryBaseResource();
            if (resourceInfo != null && resourceInfo.ResourceMnenus.Count > 0)
            {
                SystemMenuList.Clear();
                foreach (MenuInfo item in resourceInfo.ResourceMnenus.Where(it=>it.ParentMenu.Equals("0")))
                {
                    MenuModel menuModel = new MenuModel()
                    {
                        MenuId = item.MenuId,
                        MenuName = item.MenuName
                    };
                    menuModel = CreateMenuList(item.MenuId, resourceInfo.ResourceMnenus, menuModel);
                    SystemMenuList.Add(menuModel);
                }
            }
        }
        private MenuModel CreateMenuList(string parentMenu, List<MenuInfo> fullMenuList, MenuModel parentModel)
        {
            foreach (MenuInfo item in fullMenuList.Where(it=>it.ParentMenu.Equals(parentMenu)))
            {
                MenuModel menuModel = new MenuModel()
                {
                    MenuId = item.MenuId,
                    MenuName = item.MenuName
                };
                menuModel = CreateMenuList(item.MenuId, fullMenuList, menuModel);
                parentModel.SystemMenuList.Add(menuModel);
            }
            return parentModel;
        }
        private void SearchData(object message)
        {
            try
            {
                DataOperation operateData = new DataOperation();
                DataSet ds = operateData.GetData(JsonHelper.ReadJsonString(message.ToString(), "DataChannelId"));
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WaitCallback(UpdateTableList), ds);
                UpdateTableList(ds);
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
            }
            
        }

        private void UpdateTableList(object ds)
        {
            DataSet tables = (DataSet)ds;
            if (tables != null && tables.Tables.Count > 0)
            {
                TableShowList.Clear();
                for (int i = 0; i < tables.Tables.Count; i++)
                {
                    TableShowList.Add(new TableModel()
                    {
                        TableId = i,
                        TableName = tables.Tables[i].TableName,
                        DataInfo = tables.Tables[i]
                    });
                }
                SelectedTable = TableShowList[0];
            }
        }
        private string OrganizeRequestData()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getMasterPropDataAsync");
            string content="{\"openType\":null,\"bzsystemid\":\"905\",\"formid\":null,\"dataSetID\":null,\"reportID\":null,\"modelId\":null,\"fieldName\":null,\"masterOnly\":false,\"dataparam\":{\"isdata\":\"0\",\"mastername\":\"地区管理\",\"wheresql\":\"1=1\",\"prooplist\":null,\"proplisted\":null,\"dataed\":null,\"pageno\":\"-1\",\"ispage\":\"1\",\"getset\":\"1\"},\"whereArr\":null,\"masterParam\":null,\"deltaXml\":null,\"runUser\":\"test7\",\"shareFlag\":null,\"treeStr\":null,\"saveType\":null,\"doccode\":null,\"clientId\":\"byerp\"}";
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
        }

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
            new GalleryManager().SetCurrentGalleryId(Victop.Frame.CoreLibrary.Enums.GalleryEnum.ENTERPRISE);
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginRun");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("PluginName", "UserLoginPlugin");
            contentDic.Add("PluginPath", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(PluginShow));
        }
        private void PluginShow(object message)
        {
            if (JsonHelper.ReadJsonString(message.ToString(), "ReplyMode").Equals("0"))
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
        /// <summary>打开插件</summary>
        private void DoWork(object pluginInfo)
        {
            Window win = ((IPlugin)((ActivePluginInfo)pluginInfo).PluginInstance).StartWindow;
            win.Uid = ((ActivePluginInfo)pluginInfo).ObjectId;
            bool? result = win.ShowDialog();
            UpdateMenu();
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

        #endregion
    }
}
