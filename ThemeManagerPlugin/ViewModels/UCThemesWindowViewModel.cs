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

namespace ThemeManagerPlugin.ViewModels
{
    public class UCThemesWindowViewModel : ModelBase
    {
        #region 字段&属性
        Storyboard stdEnd;
        private Window portalWindow;
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
        private ObservableCollection<WallPaperModel> _systemWallPaperList;
        public ObservableCollection<WallPaperModel> SystemWallPaperList
        {
            get
            {
                if (_systemWallPaperList == null)
                    _systemWallPaperList = new ObservableCollection<WallPaperModel>();
                return _systemWallPaperList;
            }
            set
            {
                if (_systemWallPaperList != value)
                {
                    _systemWallPaperList = value;
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

        #endregion

        #region 窗体加载命令
        /// <summary>窗体加载命令 </summary>
        public ICommand gridMainLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {

                    portalWindow = (Window)x;
                    stdEnd = (Storyboard)portalWindow.Resources["end"];
                    stdEnd.Completed += (c, d) =>
                    {
                        portalWindow.Close();
                    };

                    GetThemeSkinNum();
                    GetDefaultThemeSkin();
                    GetOnLineCategory();

                    GetWallPaperCategory();
                    GetWallPaperDisplay();
                   
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
                            downloadAddress.Add("DownloadFileId", wallModel.WallDisplay);
                            downloadAddress.Add("DownloadToPath", path);
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
        public ICommand btnOnLineByCategoryCommand
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
                    //string serverUrl = ConfigurationManager.AppSettings["fileserverhttp"];
                    string localityUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme", model.FileName + ".dll");
                    Dictionary<string, object> downloadMessageContent = new Dictionary<string, object>();
                    Dictionary<string, string> downloadAddress = new Dictionary<string, string>();
                    downloadAddress.Add("DownloadFileId", model.OnLinePreview);
                    downloadAddress.Add("DownloadToPath", localityUrl);
                    downloadMessageContent.Add("ServiceParams", JsonHelper.ToJson(downloadAddress));
                    DataMessageOperation messageOperation = new DataMessageOperation();
                    Dictionary<string, object> downloadResult = messageOperation.SendSyncMessage("ServerCenterService.DownloadDocument",
                                                           downloadMessageContent);
                    if (downloadResult != null && !downloadResult["ReplyMode"].ToString().Equals("0"))
                    {
                        VicMessageBoxNormal.Show(downloadResult["ReplyAlertMessage"].ToString(), "标题");
                    }

                    ThemeModel themeModel = new ThemeModel();
                    themeModel.SkinPath = @"theme\" + model.FileName + ".dll";
                    SystemThemeList.Add(themeModel);
                    MessageBox.Show(SystemThemeList.Count.ToString());
                });
            }
        }
        #endregion

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
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
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
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"fd_wallpaper\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                foreach (DataRow row in dt.Rows)
                {
                    string previewUrl = ConfigurationManager.AppSettings.Get("fileserverhttp") + "getfile?id=" + row["preview"];
                    WallPaperModel model = new WallPaperModel();
                    model.WallDisplay = row["display"].ToString();
                    model.WallPreview = previewUrl;
                    model.WllPaperName = row["wallpaper_name"].ToString();
                    model.WllPaperType = row["img_type"].ToString();
                    SystemWallPaperList.Add(model);
                }
            }
        }



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
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
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
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
                DataSet MenuDs = messageOp.GetData(channelId, "[\"fd_skin\"]");
                DataTable dt = MenuDs.Tables["dataArray"];
                foreach (DataRow row in dt.Rows)
                {
                    string previewUrl = ConfigurationManager.AppSettings.Get("fileserverhttp") + "getfile?id=" + row["img_url"];
                    OnLineModel model = new OnLineModel();
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
    }
}
