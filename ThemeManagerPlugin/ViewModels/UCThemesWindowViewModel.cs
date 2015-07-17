using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using ThemeManagerPlugin.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Victop.Frame.DataMessageManager;
using System.Net;
using System.Xml;
using System.Windows.Threading;
using Victop.Frame.Units;

namespace ThemeManagerPlugin.ViewModels
{
    public class UCThemesWindowViewModel : ModelBase
    {
        #region 字段&属性

        #region 分屏字段属性
        /// <summary>
        /// 左翻页
        /// </summary>
        private Button imageLeft;
        /// <summary>
        /// 右翻页
        /// </summary>
        private Button imageRight;
        /// <summary>
        /// 内容容器
        /// </summary>
        private Canvas canvasPageContent;
        private RectangleGeometry canvasPageRectangle;
        private WrapPanel wrapPanelPages;
        private UnitPageBar pageBar1;
        private int pageCount;
        private int totalPage;
        private int pageSize =9;
        private int currentPage = 1;
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

        Storyboard stdEnd;
        private int currentRate = 0;
        private DispatcherTimer timer;
        private Window portalWindow;
        private TabControl ThemeTabControl;
        /// <summary>皮肤列表 </summary>
        private ObservableCollection<ThemeModel> _systemThemeList;
        public ObservableCollection<ThemeModel> SystemThemeList
        {
            get
            {
                if (_systemThemeList == null)
                    _systemThemeList = new ObservableCollection<ThemeModel>();
                return _systemThemeList;
            }
            set
            {
                if (_systemThemeList != value)
                {
                    _systemThemeList = value;
                    RaisePropertyChanged("SystemThemeList");
                }
            }
        }
        /// <summary>
        /// 皮肤分屏集合
        /// </summary>
        private ObservableCollection<ThemeModel> sumPageThemeList;
        public ObservableCollection<ThemeModel> SumPageThemeList
        {
            get
            {
                if (sumPageThemeList == null)
                    sumPageThemeList = new ObservableCollection<ThemeModel>();
                return sumPageThemeList;
            }
            set
            {
                if (sumPageThemeList != value)
                {
                    sumPageThemeList = value;
                    RaisePropertyChanged("SumPageThemeList");
                }
            }
        }
        /// <summary>当前选中主题 </summary>
        private ThemeModel _selectedItem;
        public ThemeModel SelectedListBoxItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged("SelectedListBoxItem");
                }
            }
        }

        /// <summary>壁纸列表 </summary>
        private ObservableCollection<WallPaperModel> systemWallPaperList;
        public ObservableCollection<WallPaperModel> SystemWallPaperList
        {
            get
            {
                if (systemWallPaperList == null)
                    systemWallPaperList = new ObservableCollection<WallPaperModel>();
                return systemWallPaperList;
            }
            set
            {
                if (systemWallPaperList != value)
                {
                    systemWallPaperList = value;
                    RaisePropertyChanged("SystemWallPaperList");
                }
            }
        }
        /// <summary>壁纸分类列表 </summary>
        private ObservableCollection<WallPaperCategory> wallPaperCategoryList;
        public ObservableCollection<WallPaperCategory> WallPaperCategoryList
        {
            get
            {
                if (wallPaperCategoryList == null)
                    wallPaperCategoryList = new ObservableCollection<WallPaperCategory>();
                return wallPaperCategoryList;
            }
            set
            {
                if (wallPaperCategoryList != value)
                {
                    wallPaperCategoryList = value;
                    RaisePropertyChanged("WallPaperCategoryList");
                }
            }
        }
        /// <summary>当前选项卡壁纸列表 </summary>
        private ObservableCollection<WallPaperModel> seletetedTabControlWallPaperList;
        public ObservableCollection<WallPaperModel> SeletetedTabControlWallPaperList
        {
            get
            {
                if (seletetedTabControlWallPaperList == null)
                    seletetedTabControlWallPaperList = new ObservableCollection<WallPaperModel>();
                return seletetedTabControlWallPaperList;
            }
            set
            {
                if (seletetedTabControlWallPaperList != value)
                {
                    seletetedTabControlWallPaperList = value;
                    RaisePropertyChanged("SeletetedTabControlWallPaperList");
                }
            }
        }
        /// <summary>在线皮肤列表</summary>
        private ObservableCollection<OnLineModel> _systemOnLineList;
        public ObservableCollection<OnLineModel> SystemOnLineList
        {
            get
            {
                if (_systemOnLineList == null)
                    _systemOnLineList = new ObservableCollection<OnLineModel>();
                return _systemOnLineList;
            }
            set
            {
                if (_systemOnLineList != value)
                {
                    _systemOnLineList = value;
                    RaisePropertyChanged("SystemOnLineList");
                }
            }
        }
        /// <summary>在线皮肤分类列表 </summary>
        private ObservableCollection<OnLineCategory> _systemOnLineCategoryList;
        public ObservableCollection<OnLineCategory> SystemOnLineCategoryList
        {
            get
            {
                if (_systemOnLineCategoryList == null)
                    _systemOnLineCategoryList = new ObservableCollection<OnLineCategory>();
                return _systemOnLineCategoryList;
            }
            set
            {
                if (_systemOnLineCategoryList != value)
                {
                    _systemOnLineCategoryList = value;
                    RaisePropertyChanged("SystemOnLineCategoryList");
                }
            }
        }
        /// <summary>N款皮肤 </summary>
        private int _skinNum;
        public int SkinNum
        {
            get { return _skinNum; }
            set
            {
                if (_skinNum != value)
                {
                    _skinNum = value;
                    RaisePropertyChanged("SkinNum");
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

                    portalWindow = (Window)x;
                    ThemeTabControl = (TabControl)portalWindow.FindName("ThemeTabControl");
                    wrapPanelPages = (WrapPanel)portalWindow.FindName("wrapPanelPages");
                    canvasPageContent = (Canvas)portalWindow.FindName("canvasPageContent");
                    imageLeft = (Button)portalWindow.FindName("imageLeft");
                    imageRight = (Button)portalWindow.FindName("imageRight");
                    canvasPageRectangle = (RectangleGeometry)portalWindow.FindName("canvasPageRectangle");
                    sboardLeftIamge = (System.Windows.Media.Animation.Storyboard)portalWindow.FindResource("StoryboardLeftImage");
                    sboardRightIamge = (System.Windows.Media.Animation.Storyboard)portalWindow.FindResource("StoryboardRightImage");
                    pageBar1 = (UnitPageBar)portalWindow.FindName("pageBar1");
                    stdEnd = (Storyboard)portalWindow.Resources["end"];
                    #region 翻页按钮事件
                    imageLeft.MouseEnter += imageLeft_MouseEnter;
                    imageLeft.MouseLeave += imageLeft_MouseLeave;
                    imageLeft.Click += imageLeft_Click;
                    imageRight.MouseEnter += imageRight_MouseEnter;
                    imageRight.MouseLeave += imageRight_MouseLeave;
                    imageRight.Click += imageRight_Click;
                    #endregion
                    #region 内容容器事件
                    canvasPageContent.PreviewMouseLeftButtonDown += canvasPageContent_PreviewMouseLeftButtonDown;
                    canvasPageContent.PreviewMouseLeftButtonUp += canvasPageContent_PreviewMouseLeftButtonUp;
                    canvasPageContent.PreviewMouseMove += canvasPageContent_PreviewMouseMove;
                    canvasPageContent.MouseLeave += canvasPageContent_MouseLeave;
                    canvasPageContent.SizeChanged += canvasPageContent_SizeChanged;
                    canvasPageContent.MouseUp += canvasPageContent_MouseUp;
                    #endregion
                    stdEnd.Completed += (c, d) =>
                    {
                        portalWindow.Close();
                    };
                    GetProductId();
                    GetThemeSkinNum();
                    GetDefaultThemeSkin();
                    GetOnLineCategory();
                    if (SystemOnLineCategoryList.Count == 0) return;//服务器登录不上时异常控制
                    GetOnLineTheme(SystemOnLineCategoryList[0].Category_No);
                    GetWallPaperCategory();
                    GetWallPaperDisplay();//一次从服务器取到所有壁纸
                    if (WallPaperCategoryList.Count == 0) return;
                    GetSelectedTabControlWallPaperDisplay(WallPaperCategoryList[0].Category_No);
                    totalPage = SystemThemeList.Count / pageSize;
                    if ((SystemThemeList.Count % pageSize) == 0)
                    {
                        totalPage = SystemThemeList.Count / pageSize;
                    }
                    else
                    {
                        totalPage = SystemThemeList.Count / pageSize + 1;
                    }
                    setInit(totalPage);
                });
            }
        }

        
        public ICommand ThemeTabControlSelectionChanged
        {
            get {
                return new RelayCommand<object>((x) =>
                {
                    
                    ThemeTabControl = (TabControl)x;
                    if(ThemeTabControl.SelectedIndex==0)
                    {
                        wrapPanelPages.Children.Clear();
                        //GetThemeSkinNum();
                        //GetDefaultThemeSkin();
                        totalPage = SystemThemeList.Count / pageSize;
                        if ((SystemThemeList.Count % pageSize) == 0)
                        {
                            totalPage = SystemThemeList.Count / pageSize;
                        }
                        else
                        {
                            totalPage = SystemThemeList.Count / pageSize + 1; ;
                        }
                        setInit(totalPage);
                    }
                  
                });
            }
        }

        public ICommand gridMainUnloadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    DataMessageOperation pluginOp = new DataMessageOperation();
                    pluginOp.StopPlugin(x as string);
                });
            }
        }

          #endregion
        #region 窗体关闭命令
        public ICommand btnCloseClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //MessageBoxResult result = VicMessageBoxNormal.Show("确定要退出么？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    //if (result == MessageBoxResult.Yes)
                    //{
                    stdEnd.Begin();
                    //}
                });
            }
        }
        #endregion


        #region 在主题列表选中主题皮肤触发命令
        public ICommand listBoxSystemThemeListSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedListBoxItem != null)
                    {
                        try
                        {
                            ChangeFrameWorkTheme();
                            this.UpdateDefaultSkin();
                        }
                        catch (Exception ex)
                        {
                            VicMessageBoxNormal.Show("Change error: " + ex.Message);
                        }
                    }
                });
            }
        }
        #endregion

        #region 单击下载壁纸命令
        public ICommand btnDownLoadCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        WallPaperModel wallModel = (WallPaperModel)x;

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Title = "下载到";
                        saveFileDialog.Filter = string.Format("{0}文件|*{0}", wallModel.WllPaperType);
                        saveFileDialog.FileName = wallModel.WllPaperName;
                        string path = "";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            path = saveFileDialog.FileName;
                            Dictionary<string, object> downloadMessageContent = new Dictionary<string, object>();
                            Dictionary<string, string> downloadAddress = new Dictionary<string, string>();
                            downloadAddress.Add("DownloadFileId", wallModel.FilePath);
                            downloadAddress.Add("DownloadToPath", path);
                            downloadAddress.Add("ProductId", ProductId);
                            downloadMessageContent.Add("ServiceParams", JsonHelper.ToJson(downloadAddress));
                            DataMessageOperation messageOperation = new DataMessageOperation();
                            Dictionary<string, object> downloadResult = messageOperation.SendSyncMessage("ServerCenterService.DownloadDocument",
                                                                   downloadMessageContent);
                            if (downloadResult != null && !downloadResult["ReplyMode"].ToString().Equals("0"))
                            {
                                VicMessageBoxNormal.Show(downloadResult["ReplyAlertMessage"].ToString(), "标题");
                            }

                        }
                        if (path == "")  //下载其间，不下载了，直接返回
                        {
                            return;
                        }
                    }
                });
            }
        }
        #endregion

        #region 根据所选分类展示皮肤
        public ICommand OnLineListBoxSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    OnLineCategory model = (OnLineCategory)x;
                    SystemOnLineList.Clear();
                    GetOnLineTheme(model.Category_No);

                });
            }
        }
        #endregion
        #region 在线皮肤应用
        public ICommand btnOnLineUseCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    OnLineModel model = (OnLineModel)x;
                    foreach (ThemeModel skinModel in SystemThemeList)
                    {
                        if (skinModel.SkinName.Equals(model.OnLineName))
                        {
                            try
                            {
                                string messageType = "ServerCenterService.ChangeThemeByDll";
                                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                                Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
                                ServiceParams.Add("SourceName", skinModel.ThemeName);
                                ServiceParams.Add("SkinPath", skinModel.SkinPath);
                                contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
                                DataMessageOperation messageOp = new DataMessageOperation();
                                messageOp.SendAsyncMessage(messageType, contentDic);
                                return;
                            }
                            catch (Exception ex)
                            {
                                VicMessageBoxNormal.Show("Change error: " + ex.Message);
                            }
                        }
                    }
                    string localityUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme", model.FileName + ".dll");
                    Dictionary<string, object> downloadMessageContent = new Dictionary<string, object>();
                    Dictionary<string, string> downloadAddress = new Dictionary<string, string>();
                    downloadAddress.Add("DownloadFileId", model.FilePath);
                    downloadAddress.Add("DownloadToPath", localityUrl);
                    downloadAddress.Add("ProductId", ProductId);
                    downloadMessageContent.Add("ServiceParams", JsonHelper.ToJson(downloadAddress));
                    DataMessageOperation messageOperation = new DataMessageOperation();
                    Dictionary<string, object> downloadResult = messageOperation.SendSyncMessage("ServerCenterService.DownloadDocument",
                                                           downloadMessageContent);
                    SystemThemeList.Clear();
                    GetThemeSkinNum();
                    foreach (ThemeModel skinModel in SystemThemeList)
                    {
                        if (skinModel.SkinName.Equals(model.OnLineName))
                        {
                            try
                            {
                                string messageType = "ServerCenterService.ChangeThemeByDll";
                                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                                Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
                                ServiceParams.Add("SourceName", skinModel.ThemeName);
                                ServiceParams.Add("SkinPath", skinModel.SkinPath);
                                contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
                                DataMessageOperation messageOp = new DataMessageOperation();
                                messageOp.SendAsyncMessage(messageType, contentDic);
                                return;
                            }
                            catch (Exception ex)
                            {
                                VicMessageBoxNormal.Show("Change error: " + ex.Message);
                            }
                        }
                    }
                });
            }
        }
        #endregion
        #region 选中ListBox在线换肤
        public ICommand OnLineListBoxSkinSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if(x!=null)
                    {
                        OnLineModel model = (OnLineModel)x;
                        foreach (ThemeModel skinModel in SystemThemeList)
                        {
                            if (skinModel.SkinName.Equals(model.OnLineName))
                            {
                                try
                                {
                                    string messageType = "ServerCenterService.ChangeThemeByDll";
                                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                                    Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
                                    ServiceParams.Add("SourceName", skinModel.ThemeName);
                                    ServiceParams.Add("SkinPath", skinModel.SkinPath);
                                    contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
                                    DataMessageOperation messageOp = new DataMessageOperation();
                                    messageOp.SendAsyncMessage(messageType, contentDic);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    VicMessageBoxNormal.Show("Change error: " + ex.Message);
                                }
                            }
                        }
                        string localityUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme", model.FileName + ".dll");
                        Dictionary<string, object> downloadMessageContent = new Dictionary<string, object>();
                        Dictionary<string, string> downloadAddress = new Dictionary<string, string>();
                        downloadAddress.Add("DownloadFileId", model.FilePath);
                        downloadAddress.Add("DownloadToPath", localityUrl);
                        downloadMessageContent.Add("ServiceParams", JsonHelper.ToJson(downloadAddress));
                        DataMessageOperation messageOperation = new DataMessageOperation();
                        Dictionary<string, object> downloadResult = messageOperation.SendSyncMessage("ServerCenterService.DownloadDocument",
                                                               downloadMessageContent);
                        SystemThemeList.Clear();
                        GetThemeSkinNum();

                        foreach (ThemeModel skinModel in SystemThemeList)
                        {
                            if (skinModel.SkinName.Equals(model.OnLineName))
                            {
                                try
                                {
                                    string messageType = "ServerCenterService.ChangeThemeByDll";
                                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                                    Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
                                    ServiceParams.Add("SourceName", skinModel.ThemeName);
                                    ServiceParams.Add("SkinPath", skinModel.SkinPath);
                                    contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
                                    DataMessageOperation messageOp = new DataMessageOperation();
                                    messageOp.SendAsyncMessage(messageType, contentDic);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    VicMessageBoxNormal.Show("Change error: " + ex.Message);
                                }
                            }
                        }
                    }
                   
                });
            }
        }
        #endregion
        #region 在线皮肤下载命令
        public ICommand btnOnLineDownloadCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    OnLineModel model = (OnLineModel)x;
                    foreach (ThemeModel skinModel in SystemThemeList)
                    {
                        if (skinModel.SkinName.Equals(model.OnLineName))
                        {
                            VicMessageBoxNormal.Show("此皮肤本地已存在");
                            return;
                        }
                    }
                    string localityUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme", model.FileName + ".dll");
                    Dictionary<string, object> downloadMessageContent = new Dictionary<string, object>();
                    Dictionary<string, string> downloadAddress = new Dictionary<string, string>();
                    downloadAddress.Add("DownloadFileId", model.FilePath);
                    downloadAddress.Add("DownloadToPath", localityUrl);
                    downloadAddress.Add("ProductId",ProductId);
                    downloadMessageContent.Add("ServiceParams", JsonHelper.ToJson(downloadAddress));
                    DataMessageOperation messageOperation = new DataMessageOperation();
                    Dictionary<string, object> downloadResult = messageOperation.SendSyncMessage("ServerCenterService.DownloadDocument",
                                                           downloadMessageContent);
                    if (downloadResult != null && !downloadResult["ReplyMode"].ToString().Equals("0"))
                    {
                        VicMessageBoxNormal.Show(downloadResult["ReplyAlertMessage"].ToString(), "标题");
                    }
                    //清空本地皮肤重新加载
                    SystemThemeList.Clear();
                    GetThemeSkinNum();
                });
            }
        }
        #endregion

        #region  选择不同壁纸分类展示相关壁纸列表
        public ICommand WallPaperCategoryListSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    WallPaperCategory model = (WallPaperCategory)x;
                    SeletetedTabControlWallPaperList.Clear();
                    GetSelectedTabControlWallPaperDisplay(model.Category_No);
                });
            }
        }
        #endregion
        #endregion

        #region 私有方法
        /// <summary>
        /// 主题皮肤改变发送消息
        /// </summary>
        /// <param name="model"></param>
        private void ChangeFrameWorkTheme()
        {
            string messageType = "ServerCenterService.ChangeThemeByDll";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
            ServiceParams.Add("SourceName", this.SelectedListBoxItem.ThemeName);
            ServiceParams.Add("SkinPath", this.SelectedListBoxItem.SkinPath);
            contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendAsyncMessage(messageType, contentDic);

        }

        /// <summary> 
        /// 利用反射获取程序集中类
        /// </summary> 
        private void ReflectorInfo(string dllPath, string skinNamespace, ThemeModel model)
        {
            Assembly ass = Assembly.LoadFrom(dllPath); // 加载程序集 
            Type type = ass.GetType(skinNamespace); // 获取该程序集所包含的类型 
            object obj = Activator.CreateInstance(type);
            model.SkinOrder = (int)type.GetField("SkinOrder").GetValue(obj);
            model.SkinName = type.GetField("SkinName").GetValue(obj).ToString();
            model.ThemeName = type.GetField("ThemeName").GetValue(obj).ToString();
            model.SkinFace = type.GetField("SkinFace").GetValue(obj).ToString();
        }

        /// <summary>
        /// 修改默认皮肤
        /// </summary>
        private void UpdateDefaultSkin()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["skinurl"].Value = this.SelectedListBoxItem.SkinPath;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
      
        /// <summary>
        /// 获取主题文件夹中默认皮肤路径
        /// </summary>
        private void GetDefaultThemeSkin()
        {
            /*读取配置文件中的默认皮肤路径*/
            string skinDefaultName = ConfigurationManager.AppSettings.Get("skinurl");
            if (this.SystemThemeList.Count > 0)
            {
                foreach (ThemeModel model in SystemThemeList)
                {
                    if (model.SkinPath == skinDefaultName)
                    {
                        SelectedListBoxItem = model;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取主题文件夹中皮肤个数和皮肤列表
        /// </summary>
        private void GetThemeSkinNum()
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "theme", "*Skin.dll");
            SkinNum = files.Count();
            for (int j = 0; j < files.Count(); j++)
            {

                string skinNamespace = Path.GetFileNameWithoutExtension(files[j]);//得到皮肤命名空间
                ThemeModel model = new ThemeModel();
                this.ReflectorInfo(files[j], skinNamespace + ".Skin", model);
                model.SkinPath = @"theme\" + skinNamespace + ".dll";
                SystemThemeList.Add(model);
            }

            if (this.SystemThemeList.Count > 0)
            {
                List<ThemeModel> list = this.SystemThemeList.ToList();
                list.Sort();
                this.SystemThemeList = new ObservableCollection<ThemeModel>(list);
            }
        }

        /// <summary>
        /// 发送消息，得到ProductId
        /// </summary>
        private void GetProductId()
        {
            DataMessageOperation ThemePluginOp = new DataMessageOperation();
            Dictionary<string, object> userDic = ThemePluginOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
            if (userDic != null)
            {
                ProductId = JsonHelper.ReadJsonString(userDic["ReplyContent"].ToString(), "SpaceId");
            }
        }
        /// <summary>
        /// 壁纸分类展示
        /// </summary>
        private void GetWallPaperCategory()
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            string channelId = string.Empty;
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "18");
            contentDic.Add("configsystemid", "11");
            contentDic.Add("modelid", "feidao-model-fd_wallpaper_category-0001");
            List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "fd_wallpaper_category");
            List<Dictionary<string, object>> tableConList = new List<Dictionary<string, object>>();
            Dictionary<string, object> tableConDic = new Dictionary<string, object>();
            tableConList.Add(tableConDic);
            conDic.Add("tablecondition", tableConList);
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"fd_wallpaper_category\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                foreach (DataRow row in dt.Rows)
                {
                    WallPaperCategory model = new WallPaperCategory();
                    model.Category_No = row["category_no"].ToString();
                    model.Category_Name = row["category_name"].ToString();
                    WallPaperCategoryList.Add(model);
                }
            }
        }
        /// <summary>
        /// 壁纸列表展示
        /// </summary>
        private void GetWallPaperDisplay()
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            string channelId = string.Empty;
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "18");
            contentDic.Add("configsystemid", "11");
            contentDic.Add("modelid", "feidao-model-fd_wallpaper-0001");
            List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "fd_wallpaper");
            List<Dictionary<string, object>> tableConList = new List<Dictionary<string, object>>();
            Dictionary<string, object> tableConDic = new Dictionary<string, object>();
            tableConList.Add(tableConDic);
            conDic.Add("tablecondition", tableConList);
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"fd_wallpaper\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                foreach (DataRow row in dt.Rows)
                {
                    string previewUrl = ConfigurationManager.AppSettings.Get("downloadfilehttp") + "getfile?id=" + row["preview"] + "&productid=" + ProductId;
                    WallPaperModel model = new WallPaperModel();
                    model.FilePath = row["file_path"].ToString();
                    model.WallPreview = previewUrl;
                    model.WllPaperName = row["wallpaper_name"].ToString();
                    model.WllPaperType = row["file_type"].ToString();
                    model.Category_No = row["category_no"].ToString();
                    SystemWallPaperList.Add(model);
                }
            }
        }


        /// <summary>
        /// 当前选项卡壁纸列表展示
        /// </summary>
        private void GetSelectedTabControlWallPaperDisplay(string categoryNo)
        {
            foreach (WallPaperModel model in SystemWallPaperList)
            {
                if (model.Category_No.Equals(categoryNo))   
                    SeletetedTabControlWallPaperList.Add(model);
            }
           
        }

        #region 在线皮肤分类
        private void GetOnLineCategory()
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            string channelId = string.Empty;
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "18");
            contentDic.Add("configsystemid", "11");
            contentDic.Add("modelid", "feidao-model-fd_skin_category-0002");
            List<object> conList = new List<object>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "fd_skin_category");
            List<Dictionary<string, object>> tableConList = new List<Dictionary<string, object>>();
            Dictionary<string, object> tableConDic = new Dictionary<string, object>();
            tableConList.Add(tableConDic);
            conDic.Add("tablecondition", tableConList);
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"fd_skin_category\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                foreach (DataRow row in dt.Rows)
                {
                    OnLineCategory model = new OnLineCategory();
                    model.Category_No = row["category_no"].ToString();
                    model.Category_Name = row["category_name"].ToString();
                    SystemOnLineCategoryList.Add(model);
                }
            }
        }
        #endregion
        #region 在线皮肤展示
        private void GetOnLineTheme(string categoryNo)
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            string channelId = string.Empty;
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "18");
            contentDic.Add("configsystemid", "11");
            contentDic.Add("modelid", "feidao-model-fd_skin-0001");
            List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "fd_skin");
            List<object> tableConList = new List<object>();
            Dictionary<string, object> tableConDic = new Dictionary<string, object>();
            tableConDic.Add("category_no", categoryNo);
            tableConList.Add(tableConDic);
            conDic.Add("tablecondition", tableConList);
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"fd_skin\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                foreach (DataRow row in dt.Rows)
                {
                    OnLineModel model = new OnLineModel();
                    string previewUrl = ConfigurationManager.AppSettings.Get("downloadfilehttp") + "getfile?id=" + row["img_url"] + "&productid=" + ProductId;
                    
                    model.OnLineNo = row["skin_no"].ToString();
                    model.OnLineName = row["skin_name"].ToString();
                    model.TypeNo = row["category_no"].ToString();
                    model.OnLinePreview = previewUrl;
                    model.OnLineImgType = row["img_type"].ToString();
                    model.FileName = row["file_name"].ToString();
                    model.FilePath = row["file_path"].ToString();
                    model.FileType = row["file_type"].ToString();
                   
                    SystemOnLineList.Add(model);
                }
              
            }
        }
        #endregion

        #region 分屏方法
        private void setInit(int totalPage)
        {
            //设置wrapPanelPages宽度
            wrapPanelPages.Width = canvasPageContent.ActualWidth * totalPage;
            SumPageThemeList.Clear();
            for (int i = 0; i < SystemThemeList.Count; i++)
            {
                SumPageThemeList.Add(SystemThemeList[i]);
            }
            int articleWindowCount = SumPageThemeList.Count;

            for (int i = 0; i < totalPage; i++)
            {
                WrapPanel rectangle = new WrapPanel();
                rectangle.Orientation = Orientation.Vertical;
                rectangle.Width = 700;
                rectangle.Height = 460;
                ObservableCollection<ThemeModel> currentPageList = new ObservableCollection<ThemeModel>();
                ListBox lbox = new ListBox();
                if (articleWindowCount > 0)
                {
                    foreach (var menuModel in SystemThemeList.Skip(i * pageSize).Take(pageSize).ToList<ThemeModel>())
                    {
                        if (menuModel != null)
                        {
                            currentPageList.Add(menuModel);
                        }
                    }
                }
                lbox.ItemsSource = currentPageList;
                lbox.SelectedItem = SelectedListBoxItem;
                lbox.SelectionChanged += lbox_SelectionChanged;
                lbox.SetResourceReference(ListBox.StyleProperty, "RadioButtonListStyle");
                rectangle.Children.Add(lbox);

                Viewbox viewbox = new Viewbox();
                viewbox.Stretch = Stretch.Fill;
                viewbox.Width = canvasPageContent.ActualWidth;
                viewbox.Height = canvasPageContent.ActualHeight;
                viewbox.Child = rectangle;
                wrapPanelPages.Children.Add(viewbox);
            }

            //改变页   pageSelect = defaultPageNum;
            if (pageSelect > totalPage)
                pageSelect = totalPage;
            if (pageSelect == 0)
                pageSelect = 1;
            Canvas.SetLeft(wrapPanelPages, -canvasPageContent.ActualWidth * (pageSelect - 1));
            pageBar1.CreatePageEllipse(totalPage);
            pageBar1.SelectPage(pageSelect);
            pageBar1.Visibility = System.Windows.Visibility.Visible;


        }
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
        void canvasPageContent_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);

        }
        #region 页面大小变化
        private void canvasPageContent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasPageRectangle.Rect = new Rect(0, 0, canvasPageContent.ActualWidth, canvasPageContent.ActualHeight);
        }
        #endregion
        private void changePos()
        {
            double pageWidth = canvasPageContent.ActualWidth;
            isDown = false;
            isMoveSure = false;
            double listLeft_now = Canvas.GetLeft(wrapPanelPages);
            double listLeft_sur = -(pageSelect - 1) * pageWidth;
            if (listLeft_now < listLeft_sur)
            {
                if (pageSelect == totalPage)
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
                    bool SureRight = false;
                    double dis = Math.Abs(listLeft_now - listLeft_sur);
                    if (dis >= 150)
                        SureRight = true;
                    if (SureRight)
                    {
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
            else
            {
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
                    bool SureLeft = false;
                    double dis = Math.Abs(listLeft_now - listLeft_sur);
                    if (dis >= 150)
                        SureLeft = true;
                    if (SureLeft)
                    {
                        isInMove = true;
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
     
        /// <summary>
        /// 点击ListBoxItem换肤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private  void lbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            SelectedListBoxItem = (ThemeModel)listBox.SelectedItem;
            ChangeFrameWorkTheme();
            this.UpdateDefaultSkin();
        }

      
       private void imageRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
           sboardRightIamge.Begin();
           ChangePage(true);
       }
       void imageRight_Click(object sender, RoutedEventArgs e)
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
           //imageRight.Effect = new System.Windows.Media.Effects.DropShadowEffect();
       }

       private void imageLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
           sboardLeftIamge.Begin();
           ChangePage(false);
       }
       void imageLeft_Click(object sender, RoutedEventArgs e)
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
          // imageLeft.Effect = new System.Windows.Media.Effects.DropShadowEffect();
       }

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

        #endregion
    }
}
