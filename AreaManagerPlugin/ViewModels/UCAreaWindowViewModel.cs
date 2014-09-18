using AreaManagerPlugin.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Victop.Frame.DataChannel;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls.Models;
using System.IO;
using System.Net;
using Victop.Server.Controls.Runtime;

namespace AreaManagerPlugin.ViewModels
{
    public class UCAreaWindowViewModel : ModelBase
    {
        private string DataChannelId = string.Empty;
        private DataTable myDt;
        private DataTable jsonDataTable;

        public DataTable JsonDataTable
        {
            get
            {
                if (jsonDataTable == null)
                    jsonDataTable = new DataTable();
                return jsonDataTable;
            }
            set
            {
                jsonDataTable = value;
                RaisePropertyChanged("JsonDataTable");
            }
        }
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
        private System.Windows.Controls.Grid gridDoc;
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
                    //MessageOperation messageOp = new MessageOperation();
                    //Dictionary<string, object> result = messageOp.SendMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                    //string temp = string.Empty;

                    CurdJSONDataOperation jsonOp = new CurdJSONDataOperation();


                });
            }
        }

        string fileId = string.Empty;
        string fileSuffix = string.Empty;

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
                    address.Add("UploadFromPath", uploadFromPath);
                    address.Add("UploadUrl", "http://192.168.40.191:8080/fsweb/upload?mode_id=1");
                    messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                    Dictionary<string, object> result = new MessageOperation().SendMessage("ServerCenterService.UploadDocument", messageContent);
                    System.Windows.Forms.MessageBox.Show(JsonHelper.ToJson(result));

                    Dictionary<string, object> replyContent = JsonHelper.ToObject<Dictionary<string, object>>(result["ReplyContent"].ToString());
                    this.fileId = replyContent["fileId"].ToString();
                    this.fileSuffix = replyContent["fileSuffix"].ToString();
                });
            }
        }

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
                        address.Add("DownloadUrl", @"http://192.168.40.191:8080/fsweb/getfile?id=" + this.fileId);
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
                    string MessageType = "MongoDataChannelService.findBusiData";
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", "100");
                    contentDic.Add("configsystemid", "101");
                    contentDic.Add("spaceid", "victop_core");
                    contentDic.Add("modelid", "table::industry");
                    List<object> conlist = new List<object>();
                    Dictionary<string, object> conDic = new Dictionary<string, object>();
                    conDic.Add("name", "industry");
                    List<object> tableConList = new List<object>();
                    Dictionary<string, object> tableConDic = new Dictionary<string, object>();
                    tableConDic.Add("name", RegexHelper.StartWith("电"));
                    tableConList.Add(tableConDic);
                    conDic.Add("tablecondition", tableConList);
                    conlist.Add(conDic);
                    contentDic.Add("conditions", conlist);

                    Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                    if (returnDic != null)
                    {
                        viewId = returnDic["DataChannelId"].ToString();
                        dataPath = "[\"industry\"]";
                        DataOperation dataOp = new DataOperation();
                        JsonDataTable = dataOp.GetData(viewId, dataPath, null);
                        //string temp = dataOp.GetJSONData(viewId);
                        //dynamic dyc = JsonHelper.DeserializeObject(temp);
                        //dyc.docDataStore.busi_point.summary.totalRow = 3;
                        //string temp1 = JsonHelper.ToJson(dyc);
                        //string temp2 = dataOp.GetJSONData(viewId);
                    }
                });
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        public ICommand btnAddJsonClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //DataOperation dataOp = new DataOperation();
                    //bool result = dataOp.SaveData(viewId, dataPath);
                    //return;
                    MessageOperation messageOp = new MessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("DataChannelId", viewId);
                    contentDic.Add("modelid", "table::industry");
                    contentDic.Add("systemid", "100");
                    contentDic.Add("configsystemid", "101");
                    contentDic.Add("spaceid", "victop_core");
                    Dictionary<string, object> resultDic = messageOp.SendMessage("MongoDataChannelService.saveBusiData", contentDic, "JSON");
                    string temp1 = resultDic["ReplyMode"].ToString();
                    //JSONDataOperation jsonOp = new JSONDataOperation();
                    //Dictionary<string, object> newDic = new Dictionary<string, object>();
                    //newDic.Add("_id", "A0002");
                    //newDic.Add("name", "法国");
                    //newDic.Add("englishName", "France");
                    //newDic.Add("region", null);
                    //newDic.Add("order", null);
                    //bool result = jsonOp.AddData(viewId, "[\"area\"]", JsonHelper.ToJson(newDic));
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
                    DataRow dr = JsonDataTable.NewRow();
                    JsonDataTable.Rows.Add(dr);
                    //JSONDataOperation jsonOp = new JSONDataOperation();
                    //jsonOp.ModifyData(viewId, "[\"area\",{\"key\":\"_id\",\"value\":\"A0001\"},\"order\",{\"key\":\"_id\",\"value\":\"ASDLFKJ-KJLSDJF\"}]", "{\"name\":\"商品订单\"}");
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
                    JSONDataOperation jsonOp = new JSONDataOperation();
                    jsonOp.DeleteData(viewId, "[\"area\"]", "{\"_id\":\"A0002\"}");
                });
            }
        }
        #endregion

        #region 组件测试
        public ICommand btnCpntClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    //CompntDataGrid grid = (CompntDataGrid)x;
                    //grid.SettingModel = JsonHelper.ToObject<CompntSettingModel>(FileHelper.ReadFitData("testsetting"));
                    
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
        #endregion
    }
}
