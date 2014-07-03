using AreaManagerPlugin.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Victop.Frame.DataChannel;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;

namespace AreaManagerPlugin.ViewModels
{
    public class UCAreaWindowViewModel:ModelBase
    {
        private bool IsLoaded = true;
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

        /// <summary>
        /// comboBox选择改变
        /// </summary>
        public ICommand cboxtableSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() => {
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
                return new RelayCommand(() => {
                    //if (IsLoaded)
                    //{
                    //    PluginMessage pluginMessage = new PluginMessage();
                    //    pluginMessage.SendMessage("", OrganizeRequestMessage(), new System.Threading.WaitCallback(SearchData));
                    //    IsLoaded = false;
                    //}
                });
            }
        }

        public ICommand gridUnloadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    UserControl userCtrl = (UserControl)x;
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeCloseMessage(userCtrl.Uid), null);
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
                return new RelayCommand(() => {
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeCommonRequestMessageMaster(), new System.Threading.WaitCallback(SearchData));
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
                return new RelayCommand(() => {
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeMasterSaveMessage(), new WaitCallback(SaveDataSuccess));
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
                return new RelayCommand(() => {
                    PluginMessage pluginMessage = new PluginMessage();
                    //pluginMessage.SendMessage("", OrganizeModelRequestMessage(), new System.Threading.WaitCallback(SearchData));
                    pluginMessage.SendMessage("", OrganizeCommonRequestMessage(), new System.Threading.WaitCallback(SearchData));
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
                return new RelayCommand(() => {
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
                return new RelayCommand(() => {
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeReferenceMessage(), new System.Threading.WaitCallback(SearchData));
                });
            }
        }

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
        private string OrganizeMasterRequestMessage()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getMasterPropDataAsync");
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
            paramsDic.Add("mastername", "地区管理");
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
            contentDic.Add("runUser", "test7");
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            contentDic.Add("clientId", "byerp");
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
        }

        /// <summary>
        /// 主档
        /// </summary>
        /// <returns></returns>
        private string OrganizeCommonRequestMessageMaster()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.loadDataByModelAsync");
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
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
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
        private string OrganizeModelRequestMessage()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getFormBusiDataAsync");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("openType", null);
            contentDic.Add("bzsystemid", "905");
            contentDic.Add("formid", "12103");
            contentDic.Add("dataSetID", null);
            contentDic.Add("reportID", null);
            contentDic.Add("modelId", "ERP12103");
            contentDic.Add("fieldName", null);
            contentDic.Add("masterOnly", false);
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("isdata", "0");
            contentDic.Add("dataparam", paramsDic);
            Dictionary<string, string> sqlstrDic = new Dictionary<string, string>();
            sqlstrDic.Add("1", " 1=1  and docdate >=cast('2014-06-13' as VARCHAR(10)) and docdate <cast('2014-06-21' as VARCHAR(10))");
            contentDic.Add("whereArr", sqlstrDic);
            contentDic.Add("masterParam", null);
            contentDic.Add("deltaXml", null);
            contentDic.Add("runUser", "test7");
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            contentDic.Add("clientId", "byerp");
            string content = JsonHelper.ToJson(contentDic);
            //string content = "{\"openType\":null,\"bzsystemid\":\"905\",\"formid\":null,\"dataSetID\":null,\"reportID\":null,\"modelId\":\"ERP11101\",\"fieldName\":null,\"masterOnly\":false,\"dataparam\":{\"isdata\":\"0\"},\"whereArr\":{\"1\":\" 1=1  and docdate >=cast('2014-06-09' as VARCHAR(10)) and docdate <cast('2014-06-18' as VARCHAR(10))\"},\"masterParam\":null,\"deltaXml\":null,\"runUser\":\"zzq\",\"shareFlag\":null,\"treeStr\":null,\"saveType\":null,\"doccode\":null,\"clientId\":\"byerp\"}";
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
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
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WaitCallback(UpdateTableList), ds);
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
