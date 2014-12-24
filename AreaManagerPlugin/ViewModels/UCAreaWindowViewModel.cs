using AreaManagerPlugin.Models;
using AreaManagerPlugin.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Victop.Frame.DataChannel;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace AreaManagerPlugin.ViewModels
{
    public class UCAreaWindowViewModel : ModelBase
    {
        #region 字段定义
        /// <summary>
        /// 数据信息实体实例
        /// </summary>
        private DataSearchInfoModel dataInfoModel;
        /// <summary>
        /// 编码服务实体实例
        /// </summary>
        private CodeServiceInfoModel codeInfoModel;
        /// <summary>
        /// 用户信息实体实例
        /// </summary>
        private UserMenuInfoModel userInfoModel;
        /// <summary>
        /// 其他信息实体实例
        /// </summary>
        private OtherInfoModel otherInfoModel;
        #endregion
        #region 属性定义
        /// <summary>
        /// 数据信息实体实例
        /// </summary>
        public DataSearchInfoModel DataInfoModel
        {
            get
            {
                if (dataInfoModel == null)
                    dataInfoModel = new DataSearchInfoModel();
                return dataInfoModel;
            }
            set
            {
                if (dataInfoModel != value)
                {
                    dataInfoModel = value;
                    RaisePropertyChanged("DataInfoModel");
                }
            }
        }
        /// <summary>
        /// 编码服务实体实例
        /// </summary>
        public CodeServiceInfoModel CodeInfoModel
        {
            get
            {
                if (codeInfoModel == null)
                    codeInfoModel = new CodeServiceInfoModel();
                return codeInfoModel;
            }
            set
            {
                if (codeInfoModel != value)
                {
                    codeInfoModel = value;
                    RaisePropertyChanged("CodeInfoModel");
                }
            }
        }
        /// <summary>
        /// 用户信息实体实例
        /// </summary>
        public UserMenuInfoModel UserInfoModel
        {
            get
            {
                if (userInfoModel == null)
                    userInfoModel = new UserMenuInfoModel();
                return userInfoModel;
            }
            set
            {
                if (userInfoModel != value)
                {
                    userInfoModel = value;
                    RaisePropertyChanged("UserInfoModel");
                }
            }
        }
        /// <summary>
        /// 其他消息实体实例
        /// </summary>
        public OtherInfoModel OtherInfoModel
        {
            get
            {
                if (otherInfoModel == null)
                    otherInfoModel = new OtherInfoModel();
                return otherInfoModel;
            }
            set
            {
                if (otherInfoModel != value)
                {
                    otherInfoModel = value;
                    RaisePropertyChanged("OtherInfoModel");
                }
            }
        }
        #endregion
        #region 命令
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand btnSearchClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string MessageType = "MongoDataChannelService.findBusiData";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", DataInfoModel.SystemId);
                    contentDic.Add("configsystemid", DataInfoModel.ConfigsystemId);
                    contentDic.Add("modelid", DataInfoModel.ModelId);
                    if (!string.IsNullOrEmpty(DataInfoModel.SpaceId))
                    {
                        contentDic.Add("spaceId", DataInfoModel.SpaceId);
                    }
                    if (!string.IsNullOrEmpty(DataInfoModel.ConditionStr))
                    {
                        List<Dictionary<string, object>> conList = JsonHelper.ToObject<List<Dictionary<string, object>>>(DataInfoModel.ConditionStr);
                        if (conList != null)
                        {
                            contentDic.Add("conditions", conList);
                        }
                    }
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {
                        DataOperation dataOp = new DataOperation();
                        DataInfoModel.ChannelId = returnDic["DataChannelId"].ToString();
                        List<object> pathList = new List<object>();
                        pathList.Add(DataInfoModel.TableName);
                        dataPath = JsonHelper.ToJson(pathList);
                        DataSet mastDs = new DataSet();
                        mastDs = dataOp.GetData(DataInfoModel.ChannelId, dataPath);
                        DataInfoModel.ResultDataTable = mastDs.Tables["dataArray"];
                    }
                }, () => {
                    bool result = true;
                    if (string.IsNullOrEmpty(DataInfoModel.ModelId))
                    {
                        DataInfoModel.VertifyMsg = "请输入ModelId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(DataInfoModel.SystemId))
                    {
                        DataInfoModel.VertifyMsg = "请输入SystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(DataInfoModel.ConfigsystemId))
                    {
                        DataInfoModel.VertifyMsg = "请输入ConfigsystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(DataInfoModel.TableName))
                    {
                        DataInfoModel.VertifyMsg = "请输入TableName";
                        return false;
                    }
                    DataInfoModel.VertifyMsg = string.Empty;
                    return result;
                });
            }
        }
        /// <summary>
        /// 查看Json数据
        /// </summary>
        public ICommand btnViewJsonDataClickCommand
        {
            get
            {
                return new RelayCommand(() => {
                    DataOperation dataOp = new DataOperation();
                    DataInfoModel.JsonData = dataOp.GetJSONData(DataInfoModel.ChannelId);
                });
            }
        }
        /// <summary>
        /// 执行编码服务
        /// </summary>
        public ICommand btnExcuteCodeServiceClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MessageOperation messageOp = new MessageOperation();
                    string MessageType = "MongoDataChannelService.findDocCode";
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", CodeInfoModel.SystemId);
                    contentDic.Add("configsystemid", CodeInfoModel.ConfigsystemId);
                    contentDic.Add("pname", CodeInfoModel.PName);
                    contentDic.Add("setinfo", CodeInfoModel.SetInfo);
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {
                        CodeInfoModel.ResultData = returnDic["ReplyContent"].ToString();
                    }
                }, () => {
                    bool result = true;
                    if (string.IsNullOrEmpty(CodeInfoModel.SystemId))
                    {
                        CodeInfoModel.VertifyMsg = "请输入SystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(CodeInfoModel.ConfigsystemId))
                    {
                        CodeInfoModel.VertifyMsg = "请输入ConfigsystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(CodeInfoModel.PName))
                    {
                        CodeInfoModel.VertifyMsg = "请输入PName";
                        return false;
                    }
                    if (string.IsNullOrEmpty(CodeInfoModel.SetInfo))
                    {
                        CodeInfoModel.VertifyMsg = "请输入SetInfo";
                        return false;
                    }
                    CodeInfoModel.VertifyMsg = string.Empty;
                    return result;
                });
            }
        }
        /// <summary>
        /// 用户菜单信息
        /// </summary>
        public ICommand tBoxUserSearchClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MessageOperation messageOp = new MessageOperation();
                    string MessageType = "MongoDataChannelService.afterLogin";
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", UserInfoModel.SystemId);
                    contentDic.Add("configsystemid", UserInfoModel.ConfigsystemId);
                    contentDic.Add("client_type_val", UserInfoModel.ClientType);
                    contentDic.Add("userCode", UserInfoModel.UserCode);
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {
                        UserInfoModel.ResultData = returnDic["ReplyContent"].ToString();
                    }
                }, () => {
                    bool result = true;
                    if (string.IsNullOrEmpty(UserInfoModel.SystemId))
                    {
                        UserInfoModel.VertifyMsg = "请输入SystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(UserInfoModel.ConfigsystemId))
                    {
                        UserInfoModel.VertifyMsg = "请输入ConfigsystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(UserInfoModel.ClientType))
                    {
                        UserInfoModel.VertifyMsg = "请输入ClientType";
                        return false;
                    }
                    if (string.IsNullOrEmpty(UserInfoModel.UserCode))
                    {
                        UserInfoModel.VertifyMsg = "请输入UserCode";
                        return false;
                    }
                    UserInfoModel.VertifyMsg = string.Empty;
                    return result;
                });
            }
        }
        /// <summary>
        /// 其他信息
        /// </summary>
        public ICommand btnOtherExcuteClickCommand
        {
            get
            {
                return new RelayCommand(() => {
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = JsonHelper.ToObject<Dictionary<string, object>>(OtherInfoModel.OtherConditionData);
                    Dictionary<string, object> returnDic = messageOp.SendMessage(OtherInfoModel.MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {
                        OtherInfoModel.OtherResultData = returnDic["ReplyContent"].ToString();
                    }
                });
            }
        }

        #endregion
        private string DataChannelId = string.Empty;
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
        private static System.Windows.Controls.ProgressBar progressBar;
        /// <summary>
        /// comboBox选择改变
        /// </summary>
        public ICommand cboxtableSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MyDt = SelectedTable.DataInfo;
                });
            }
        }
        /// <summary>
        /// Grid加载
        /// </summary>
        public ICommand gridLoadedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //if (IsLoaded)
                    //{
                    //    PluginMessage pluginMessage = new PluginMessage();
                    //    pluginMessage.SendMessage("", OrganizeRequestMessage(), new System.Threading.WaitCallback(SearchData));
                    //    IsLoaded = false;
                    //}
                    
                });
            }
        }
        /// <summary>
        /// 主档取数
        /// </summary>
        public ICommand btnGetMasterClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string MessageType = "DataChannelService.getMasterPropDataAsync";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, OrganizeMasterRequestMessage());
                    if (!(returnDic["ReplyMode"].ToString().Equals("0")))
                    {
                        if (returnDic.ContainsKey("DataChannelId") && returnDic["DataChannelId"] != null)
                        {
                            DataOperation operateData = new DataOperation();
                            DataSet ds = operateData.GetData(returnDic["DataChannelId"].ToString());
                            UpdateTableList(ds);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(returnDic["ReplyAlertMessage"].ToString());
                    }
                });
            }
        }

        private string newChannelId = string.Empty;
        /// <summary>
        /// 新主档取数
        /// </summary>
        public ICommand btnGetMasterNewClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string MessageType = "DataChannelService.loadDataByModelAsync";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, OrganizeCommonRequestMessageMaster());
                    if (returnDic.ContainsKey("DataChannelId") && returnDic["DataChannelId"] != null)
                    {
                        newChannelId = returnDic["DataChannelId"].ToString();
                        DataOperation operateData = new DataOperation();
                        DataSet ds = operateData.GetData(returnDic["DataChannelId"].ToString());
                        UpdateTableList(ds);
                    }
                });
            }
        }

        /// <summary>
        /// 主档保存
        /// </summary>
        public ICommand btnSaveClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeMasterSaveMessage(), new WaitCallback(SaveDataSuccess));
                });
            }
        }
        /// <summary>
        /// 主档保存新
        /// </summary>
        public ICommand btnSaveNewClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string MessageType = "DataChannelService.saveDataByModelAsync";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("modelType", "主档");
                    contentDic.Add("modelId", "地区管理");
                    contentDic.Add("bysystemid", "905");
                    contentDic.Add("formid", null);
                    contentDic.Add("controlid", null);
                    contentDic.Add("DataChannelId", newChannelId);
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic);
                    string temp = string.Empty;
                });
            }
        }

        /// <summary>
        /// 模型取数
        /// </summary>
        public ICommand btnGetModelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string MessageType = "DataChannelService.getFormBusiDataAsync";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, OrganizeModelRequestMessage());
                    if (returnDic.ContainsKey("DataChannelId") && returnDic["DataChannelId"] != null)
                    {
                        DataOperation operateData = new DataOperation();
                        DataSet ds = operateData.GetData(returnDic["DataChannelId"].ToString());
                        UpdateTableList(ds);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(returnDic["ReplyAlertMessage"].ToString());
                    }
                });
            }
        }
        /// <summary>
        /// 模型保存
        /// </summary>
        public ICommand btnSaveModelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeModelSaveMessage(), new WaitCallback(SaveDataSuccess));
                });
            }
        }
        /// <summary>
        /// 数据引用
        /// </summary>
        public ICommand btnDataReferenceClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string MessageType = "DataChannelService.getFormReferenceSpecial";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, OrganizeModelRequestMessage());
                    if (returnDic.ContainsKey("DataChannelId") && returnDic["DataChannelId"] != null)
                    {
                        DataOperation operateData = new DataOperation();
                        DataSet ds = operateData.GetData(returnDic["DataChannelId"].ToString());
                        UpdateTableList(ds);
                    }
                });
            }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public ICommand btnGetUserInfoClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> result = messageOp.SendMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                    string fileInfo = JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserImg");
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "下载到";
                    saveFileDialog.Filter = string.Format("{0}文件|*{0}", string.Empty);
                    //saveFileDialog.DefaultExt = "jpg";
                    string path = "";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = saveFileDialog.FileName;
                    }
                    if (path == "")
                    {
                        return;
                    }
                    MessageOperation messageOperation = new MessageOperation();
                    Dictionary<string, object> messageContent = new Dictionary<string, object>();
                    Dictionary<string, string> address = new Dictionary<string, string>();
                    address.Add("DownloadUrl", ConfigurationManager.AppSettings.Get("fileserverhttp") + "getfile?id=" + fileInfo);
                    address.Add("DownloadToPath", path);
                    messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                    Dictionary<string, object> downResult = messageOperation.SendMessage("ServerCenterService.DownloadDocument", messageContent);
                    MessageBox.Show(JsonHelper.ToJson(downResult));
                });
            }
        }
        public ICommand btnUploadFile1ClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string uploadFromPath = "";
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Title = "选择上传的文件";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        uploadFromPath = openFileDialog.FileName;
                    }

                    Dictionary<string, object> messageContent = new Dictionary<string, object>();
                    Dictionary<string, string> address = new Dictionary<string, string>();
                    address.Add("UploadMode", "wpf");
                    address.Add("UploadFromPath", uploadFromPath);
                    address.Add("DelFileId", this.fileId);
                    messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                    Dictionary<string, object> result = new MessageOperation().SendMessage("ServerCenterService.UploadDocument", messageContent);
                    Dictionary<string, object> replyContent = JsonHelper.ToObject<Dictionary<string, object>>(result["ReplyContent"].ToString());
                    this.fileId = replyContent["fileId"].ToString();
                    this.fileSuffix = replyContent["fileSuffix"].ToString();
                });
            }
        }

        string fileId = string.Empty;
        string fileSuffix = string.Empty;
        public ICommand btnDownloadFile1ClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (this.fileId.Length > 0)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Title = "下载到";
                        saveFileDialog.Filter = string.Format("{0}文件|*{0}", this.fileSuffix);
                        string path = "";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            path = saveFileDialog.FileName;
                        }
                        if (path == "")
                        {
                            return;
                        }

                        MessageOperation messageOperation = new MessageOperation();
                        Dictionary<string, object> messageContent = new Dictionary<string, object>();
                        Dictionary<string, string> address = new Dictionary<string, string>();
                        address.Add("DownloadFileId", this.fileId);
                        address.Add("DownloadToPath", path);
                        messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                        Dictionary<string, object> result = messageOperation.SendMessage("ServerCenterService.DownloadDocument", messageContent);
                        System.Windows.MessageBox.Show(JsonHelper.ToJson(result));
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("文件不存在");
                    }
                });
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        public ICommand btnDownloadFileClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    string fromPath = @"http://c.hiphotos.baidu.com/image/pic/item/d043ad4bd11373f05044406aa60f4bfbfaed04cf.jpg";
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.Description = "下载到";
                    string path = "";
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = folderBrowserDialog.SelectedPath;
                    }
                    if (path == "")
                    {
                        return;
                    }
                    path = Path.Combine(path, Path.GetFileName(fromPath));
                    MessageOperation messageOperation = new MessageOperation();
                    Dictionary<string, object> messageContent = new Dictionary<string, object>();
                    Dictionary<string, string> address = new Dictionary<string, string>();
                    address.Add("downloadToAddresss", path);
                    address.Add("downloadFromAddress", fromPath);
                    messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                    Dictionary<string, object> result = messageOperation.SendMessage("ServerCenterService.DownloadDocument", messageContent);
                    System.Windows.MessageBox.Show(JsonHelper.ToJson(result));
                });
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        public ICommand btnUploadFileClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string uploadToPath = "";
                    string uploadFromPath = "";
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Title = "选择上传的文件";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        uploadFromPath = openFileDialog.FileName;
                    }
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.Description = "上传到";
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        uploadToPath = folderBrowserDialog.SelectedPath;
                    }
                    uploadToPath = Path.Combine(uploadToPath, Path.GetFileName(uploadFromPath));
                    Dictionary<string, object> messageContent = new Dictionary<string, object>();
                    Dictionary<string, string> address = new Dictionary<string, string>();
                    address.Add("UploadToAddresss", uploadToPath);
                    address.Add("UploadFromAddress", uploadFromPath);
                    messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                    Dictionary<string, object> result = new MessageOperation().SendMessage("ServerCenterService.UploadDocument", messageContent);
                    System.Windows.Forms.MessageBox.Show(JsonHelper.ToJson(result));
                });
            }
        }

        #region JSON操作命令

        string viewId = string.Empty;
        string dataPath = null;
        /// <summary>
        /// 检索
        /// </summary>
        public ICommand btnSearchJsonClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                       
                        #region 获取编号
                        //Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        //contentDic.Add("configsystemid", "906");
                        ////contentDic.Add("spaceid", "tbs");
                        //contentDic.Add("pname", "BH005");
                        //contentDic.Add("setinfo", "B04,1");
                        //Dictionary<string, object> returnDic = messageOp.SendMessage("MongoDataChannelService.findDocCode", contentDic, "JSON");
                        //if (returnDic != null)
                        //{
                        //    viewId = returnDic["DataChannelId"].ToString();
                        //    DataOperation dataOp = new DataOperation();
                        //    string temp = dataOp.GetJSONData(viewId);
                        //    List<object> tempList = new List<object>();
                        //    tempList.Add("simpleRef");
                        //    //JsonDataTable = dataOp.GetData(viewId, JsonHelper.ToJson(tempList), null);
                        //}
                        #endregion
                        #region TinyServer测试
                        //Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        //contentDic.Add("systemid", "906");
                        //contentDic.Add("configsystemid", "905");
                        ////contentDic.Add("spaceid", "tianlong");
                        //contentDic.Add("modelid", "ak00048");
                        //Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                        //if (returnDic != null)
                        //{
                        //    List<Object> pathList = new List<object>();
                        //    pathList.Add("area");
                        //    dataPath = JsonHelper.ToJson(pathList);
                        //    viewId = returnDic["DataChannelId"].ToString();
                        //    DataOperation dataOp = new DataOperation();
                        //    JsonDataTable = dataOp.GetData(viewId, dataPath).Tables["dataArray"];
                        //}
                        #endregion
                        #region 获取系统时间
                        //MessageType = "MongoDataChannelService.fetchSystime";
                        //Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        //Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                        //if (returnDic != null)
                        //{

                        //}
                        #endregion

                        #region 获取菜单
                        //MessageType = "MongoDataChannelService.conveyor";
                        //Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        //contentDic.Add("systemid", "906");
                        //contentDic.Add("configsystemid", "101");
                        //contentDic.Add("spaceId", "victop_core");
                        //contentDic.Add("conveyor_id", "71a204e860619946553bb421");
                        //List<object> list = new List<object>();
                        //list.Add("9aba5asfer7b6gy5");
                        //contentDic.Add("params", list);
                        //Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                        //viewId = returnDic["DataChannelId"].ToString();
                        //DataOperation dataOp = new DataOperation();
                        //string dataStr = dataOp.GetJSONData(viewId);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string temp = ex.Message;
                    }
                });
            }
        }

        public ICommand datagridSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() => {
                    string temp = "a";
                });
            }
        }

        public ICommand jsonSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    VicDataGrid grid = (VicDataGrid)x;

                    string MessageType = "ThirdPayService.creatPayUrl";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", "906");
                    contentDic.Add("configsystemid", "905");
                    contentDic.Add("spaceId", "tianlong");
                    contentDic.Add("out_trade_no", "2014122000006");//订单号
                    contentDic.Add("total_fee", "1");//总金额(单位是分)
                    contentDic.Add("body", "测试商品6");//商品名称
                    contentDic.Add("subject", "速递");//附属信息
                    contentDic.Add("ip", IPOperationHelper.GetPublicNetWorkIp());//获取本机公网IP地址
                    contentDic.Add("attach", "tianlong|101|12");//附加信息
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {
                        returnDic["ReplyContent"] = "";
                    }
                });
            }
        }

        UCAreaWindowData areaData;
        string buinessKey = string.Empty;
        /// <summary>
        /// 添加
        /// </summary>
        public ICommand btnAddJsonClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    areaData = (UCAreaWindowData)x;
                    string MessageType = "PluginService.SetPluginInfo";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("ObjectId", areaData.Uid);
                    buinessKey = Guid.NewGuid().ToString();
                    contentDic.Add("BusinessKey", buinessKey);
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {

                    }
                });
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        public ICommand btnUpdateJsonClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataOperation dataOp = new DataOperation();
                    Dictionary<string, object> resultDic = dataOp.GetPluginInfoByBusinessKey(buinessKey);
                    IPlugin pluginInfo = resultDic["IPlugin"] as IPlugin;
                    pluginInfo.Init();
                });
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public ICommand btnDeleteJsonClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //string temp = GetWebStatusCode("http://localhost/VictopPartner/AutoUpdater/AutoUpdater.xml", 3000);
                    //MessageBox.Show(temp);
                });
            }
        }
        #endregion

        #region 组件测试
        public ICommand btnCpntClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    string MessageType = "MongoDataChannelService.findBusiData";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", "100");
                    contentDic.Add("configsystemid", "101");
                    contentDic.Add("modelid", "ak10002");
                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {
                        viewId = returnDic["DataChannelId"].ToString();
                        DataOperation dataOp = new DataOperation();
                        string temp = dataOp.GetJSONData(viewId);
                    }
                });
            }
        }
        #endregion

        #region Combobox处理

        string comboxSelectedValue = string.Empty;
        public ICommand testCboxDropDownOpenedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    DataSet ds = new DataSet();
                    System.Windows.Controls.ComboBox cBox = (System.Windows.Controls.ComboBox)x;
                    if (cBox.ItemsSource == null)
                    {
                        DataOperation dataOp = new DataOperation();
                        ds = dataOp.GetSimpDefData(viewId, "[\"customer\"]", "id_area_id");
                        //ds = dataOp.GetSimpDefData(viewId, "[\"id_area\",{\"key\":\"_id\",\"value\":\"afdsa\"},\"id_city\"]", "id_area_id");
                        cBox.DisplayMemberPath = "txt";
                        cBox.SelectedValuePath = "val";
                        cBox.ItemsSource = ds.Tables["dataArray"].DefaultView;
                    }
                });
            }
        }
        public ICommand testCboxSelectionChanged
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    System.Windows.Controls.ComboBox cBox = (System.Windows.Controls.ComboBox)x;
                    comboxSelectedValue = cBox.SelectedValue.ToString();
                });
            }
        }

        public ICommand testCbox1DropDownOpenedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    DataSet ds = new DataSet();
                    System.Windows.Controls.ComboBox cBox = (System.Windows.Controls.ComboBox)x;
                    DataOperation dataOp = new DataOperation();
                    ds = dataOp.GetSimpDefData(viewId, "[\"customer\"]", "id_city_id");
                    //ds = dataOp.GetSimpDefData(viewId, "[\"id_area\",{\"key\":\"_id\",\"value\":\"afdsa\"},\"id_city\"]", "id_area_id");
                    cBox.DisplayMemberPath = "txt";
                    cBox.SelectedValuePath = "val";
                    cBox.ItemsSource = ds.Tables["dataArray"].DefaultView;
                });
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 组织关闭消息
        /// </summary>
        /// <param name="ObjectId">关闭对象的标识(Uid)</param>
        /// <returns></returns>
        private string OrganizeCloseMessage(string ObjectId)
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginStop");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("ObjectId", ObjectId);
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            return JsonHelper.ToJson(messageDic);
        }
        /// <summary>
        /// 组织主档取数消息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> OrganizeMasterRequestMessage()
        {
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("openType", null);
            contentDic.Add("bzsystemid", "905");
            contentDic.Add("formid", null);
            contentDic.Add("dataSetID", null);
            contentDic.Add("reportID", null);
            contentDic.Add("modelId", null);
            contentDic.Add("fieldName", null);
            contentDic.Add("masterOnly", "false");
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("isdata", "0");
            paramsDic.Add("mastername", "区域管理");
            paramsDic.Add("wheresql", "1=1");
            paramsDic.Add("pageno", "-1");
            paramsDic.Add("prooplist", null);
            paramsDic.Add("proplisted", null);
            paramsDic.Add("dataed", null);
            paramsDic.Add("ispage", "1");
            paramsDic.Add("getset", "1");
            contentDic.Add("dataparam", paramsDic);
            contentDic.Add("whereArr", null);
            contentDic.Add("masterParam", null);
            contentDic.Add("deltaXml", null);
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            return contentDic;
        }

        /// <summary>
        /// 主档
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> OrganizeCommonRequestMessageMaster()
        {
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("openType", null);
            contentDic.Add("bzsystemid", null);
            contentDic.Add("formid", null);
            contentDic.Add("dataSetID", null);
            contentDic.Add("reportID", null);
            contentDic.Add("modelId", null);
            contentDic.Add("fieldName", null);
            contentDic.Add("masterOnly", false);
            contentDic.Add("dataparam", null);
            contentDic.Add("whereArr", null);
            contentDic.Add("masterParam", null);
            List<Dictionary<string, object>> detaList = new List<Dictionary<string, object>>();
            Dictionary<string, object> detaXmlDic = new Dictionary<string, object>();
            detaXmlDic.Add("modeltype", "主档");
            detaXmlDic.Add("modelid", "地区管理");
            detaXmlDic.Add("systemid", "905");
            detaXmlDic.Add("formid", null);
            detaXmlDic.Add("tableid", null);
            detaXmlDic.Add("formattype", null);
            detaXmlDic.Add("ispage", "1");
            detaXmlDic.Add("pageno", "-1");
            detaXmlDic.Add("pagesize", "10");
            detaXmlDic.Add("wherearr", null);
            detaXmlDic.Add("dataparam", null);
            detaXmlDic.Add("doccode", null);
            detaXmlDic.Add("controlid", null);
            detaXmlDic.Add("detail", "data,meta");
            detaList.Add(detaXmlDic);
            contentDic.Add("deltaXml", JsonHelper.ToJson(detaList));
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            contentDic.Add("clientId", "byerp");
            contentDic.Add("runUser", "test7");
            return contentDic;
        }
        /// <summary>
        /// 统一取数消息
        /// </summary>
        /// <returns></returns>
        private string OrganizeCommonRequestMessage()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.loadDataByModelAsync");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("openType", null);
            contentDic.Add("bzsystemid", "905");
            contentDic.Add("formid", "12103");
            contentDic.Add("dataSetID", null);
            contentDic.Add("reportID", null);
            contentDic.Add("modelId", "ERP12103");
            contentDic.Add("fieldName", null);
            contentDic.Add("masterOnly", false);
            contentDic.Add("dataparam", null);
            contentDic.Add("whereArr", null);
            contentDic.Add("masterParam", null);
            List<Dictionary<string, object>> detaList = new List<Dictionary<string, object>>();
            Dictionary<string, object> detaXmlDic = new Dictionary<string, object>();
            detaXmlDic.Add("modeltype", "数据模型");
            detaXmlDic.Add("modelid", "ERP12103");
            detaXmlDic.Add("systemid", "905");
            detaXmlDic.Add("formid", "12103");
            detaXmlDic.Add("tableid", null);
            detaXmlDic.Add("formattype", null);
            detaXmlDic.Add("ispage", "1");
            detaXmlDic.Add("pageno", "-1");
            detaXmlDic.Add("pagesize", "10");
            detaXmlDic.Add("wherearr", null);
            detaXmlDic.Add("dataparam", null);
            detaXmlDic.Add("doccode", null);
            detaXmlDic.Add("controlid", null);
            detaXmlDic.Add("detail", "data,meta");
            detaList.Add(detaXmlDic);
            contentDic.Add("deltaXml", JsonHelper.ToJson(detaList));
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            contentDic.Add("clientId", "byerp");
            contentDic.Add("runUser", "test7");
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
        }
        /// <summary>
        /// 组织模型取数消息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> OrganizeModelRequestMessage()
        {
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("openType", null);
            contentDic.Add("bzsystemid", "905");
            contentDic.Add("formid", "11101");
            contentDic.Add("dataSetID", null);
            contentDic.Add("reportID", null);
            contentDic.Add("modelId", "ERP11101");
            contentDic.Add("fieldName", null);
            contentDic.Add("masterOnly", false);
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("isdata", "0");
            contentDic.Add("dataparam", paramsDic);
            contentDic.Add("whereArr", null);
            contentDic.Add("masterParam", null);
            contentDic.Add("deltaXml", null);
            contentDic.Add("runUser", "test7");
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            contentDic.Add("clientId", "byerp");
            return contentDic;
        }
        /// <summary>
        /// 组织模型保存消息
        /// </summary>
        /// <returns></returns>
        private string OrganizeModelSaveMessage()
        {
            return null;
        }
        /// <summary>
        /// 组织数据引用
        /// </summary>
        /// <returns></returns>
        private string OrganizeReferenceMessage()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getFormReferenceSpecial");
            RequestMessageModel messageModel = new RequestMessageModel();
            messageModel.bzsystemid = "905";
            messageModel.formid = "13104";
            messageModel.modelId = "ERP13104";
            messageModel.fieldName = "stcode";
            messageModel.dataparam = new Dictionary<string, object>();
            messageModel.dataparam.Add("datasourcetype", null);
            messageModel.dataparam.Add("datasourcename", null);
            messageModel.dataparam.Add("bsname", "店面管理查询");
            messageModel.dataparam.Add("bsgroupname", null);
            messageModel.dataparam.Add("isdata", "1");
            messageModel.dataparam.Add("isbs", null);
            messageModel.dataparam.Add("pageno", "-1");
            messageDic.Add("MessageContent", JsonHelper.ToJson(messageModel));
            string messageStr = JsonHelper.ToJson(messageDic);
            return messageStr;
        }
        /// <summary>
        /// 数据应用回调
        /// </summary>
        /// <param name="message"></param>
        private void DataReferenceData(object message)
        {
            string temp = message.ToString();
        }

        /// <summary>
        /// 请求成功后，根据返回结构向数据通道请求数据
        /// </summary>
        /// <param name="message"></param>
        private void SearchData(object message)
        {
            try
            {
                DataChannelId = JsonHelper.ReadJsonString(message.ToString(), "DataChannelId");
                DataOperation operateData = new DataOperation();
                DataSet ds = operateData.GetData(DataChannelId);
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WaitCallback(UpdateTableList), ds);
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
            }

        }
        /// <summary>
        /// 组织保存消息
        /// </summary>
        /// <returns></returns>
        private string OrganizeMasterSaveMessage()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getSaveMasterDataAsync");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("DataChannelId", DataChannelId);
            contentDic.Add("bzsystemid", "905");
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("mastername", "地区管理");
            contentDic.Add("dataparam", paramsDic);
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
        }
        private void SaveDataSuccess(object message)
        {
            string replyMessage = message.ToString();
        }
        /// <summary>
        /// 更新数据表列表
        /// </summary>
        /// <param name="ds"></param>
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


        private string GetWebStatusCode(string url, int timeout)
        {
            HttpWebRequest req = null;
            try
            {
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";  //这是关键        
                req.Timeout = timeout;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                return Convert.ToInt32(res.StatusCode).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
            }

        }
        #endregion
    }
}
