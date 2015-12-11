using SystemTestingPlugin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using Victop.Frame.DataMessageManager;
using SystemTestingPlugin.Views;
using System.Threading;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows;
using Victop.Server.Controls.MVVM;

namespace SystemTestingPlugin.ViewModels
{
    public class UCAreaWindowViewModel : ModelBase
    {
        #region 字段定义
        /// <summary>
        /// 数据信息实体实例
        /// </summary>
        private DataSearchInfoModel dataInfoModel;

        /// <summary>
        /// 选定表结构信息
        /// </summary>
        private TableStructModel selectedStructInfo;
        /// <summary>
        /// 表结构集合
        /// </summary>
        private ObservableCollection<TableStructModel> tableStructInfoList;

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
        /// <summary>
        /// 下载信息实体实例
        /// </summary>
        private DownLoadInfoModel downInfoModel;
        /// <summary>
        /// 查询列表
        /// </summary>
        private VicDataGrid searchDataGrid;
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
                    RaisePropertyChanged(()=> DataInfoModel);
                }
            }
        }
        /// <summary>
        /// 选定表结构信息
        /// </summary>
        public TableStructModel SelectedStructInfo
        {
            get
            {
                return selectedStructInfo;
            }
            set
            {
                if (selectedStructInfo != value)
                {
                    selectedStructInfo = value;
                    RaisePropertyChanged(()=> SelectedStructInfo);
                }
            }
        }


        /// <summary>
        /// 表结构信息集合
        /// </summary>
        public ObservableCollection<TableStructModel> TableStructInfoList
        {
            get
            {
                if (tableStructInfoList == null)
                    tableStructInfoList = new ObservableCollection<TableStructModel>();
                return tableStructInfoList;
            }
            set
            {
                if (tableStructInfoList != value)
                {
                    tableStructInfoList = value;
                    RaisePropertyChanged(()=> TableStructInfoList);
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
                    RaisePropertyChanged(()=> CodeInfoModel);
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
                    RaisePropertyChanged(()=> UserInfoModel);
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
                    RaisePropertyChanged(()=> OtherInfoModel);
                }
            }
        }
        /// <summary>
        /// 下载信息实体实例
        /// </summary>
        public DownLoadInfoModel DownInfoModel
        {
            get
            {
                if (downInfoModel == null)
                    downInfoModel = new DownLoadInfoModel();
                return downInfoModel;
            }
            set
            {
                if (downInfoModel != value)
                {
                    downInfoModel = value;
                    RaisePropertyChanged(()=> DownInfoModel);
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
                return new RelayCommand<Object>((x) =>
                {
                    try
                    {
                        searchDataGrid = (VicDataGrid)x;
                        searchDataGrid.DataGridComboBoxOpened += searchDataGrid_DataGridComboBoxOpened;
                        searchDataGrid.DataGridComboBoxClosed += searchDataGrid_DataGridComboBoxClosed;
                        searchDataGrid.DataReferenceColumnClick += searchDataGrid_DataReferenceColumnClick;
                        string MessageType = "MongoDataChannelService.findBusiData";
                        DataMessageOperation messageOp = new DataMessageOperation();
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", DataInfoModel.SystemId.Trim());
                        //contentDic.Add("configsystemid", DataInfoModel.ConfigsystemId);
                        contentDic.Add("refsystemid", string.IsNullOrEmpty(DataInfoModel.RefSystemId) ? DataInfoModel.SystemId.Trim() : DataInfoModel.RefSystemId.Trim());
                        contentDic.Add("emptydataflag", DataInfoModel.EmptyFlag ? 0 : 1);
                        contentDic.Add("modelid", DataInfoModel.ModelId.Trim());
                        if (!string.IsNullOrEmpty(DataInfoModel.SpaceId))
                        {
                            contentDic.Add("spaceid", DataInfoModel.SpaceId.Trim());
                        }
                        if (!string.IsNullOrEmpty(DataInfoModel.ConditionStr))
                        {
                            List<Dictionary<string, object>> conList = JsonHelper.ToObject<List<Dictionary<string, object>>>(DataInfoModel.ConditionStr);
                            if (conList != null)
                            {
                                contentDic.Add("conditions", conList);
                            }
                        }
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                        if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
                        {
                            DataInfoModel.ChannelId = returnDic["DataChannelId"].ToString();
                            if (string.IsNullOrEmpty(DataInfoModel.DataPath))
                            {
                                List<object> pathList = new List<object>();
                                pathList.Add(DataInfoModel.TableName);
                                DataInfoModel.DataPath = JsonHelper.ToJson(pathList);
                            }
                            DataSet mastDs = new DataSet();
                            mastDs = messageOp.GetData(DataInfoModel.ChannelId, DataInfoModel.DataPath, mastDs);
                            DataTable dt = mastDs.Tables["dataArray"];
                            DataInfoModel.ResultDataTable = dt;
                        }
                        else
                        {
                            if (returnDic != null && returnDic.ContainsKey("ReplyAlertMessage"))
                            {
                                DataInfoModel.JsonData = returnDic["ReplyAlertMessage"].ToString();
                                VicMessageBoxNormal.Show(returnDic["ReplyAlertMessage"].ToString());
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, (x) =>
                {
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

        public ICommand btnGetDataClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataMessageOperation messageOp = new DataMessageOperation();
                    DataSet mastDs = new DataSet();
                    mastDs = messageOp.GetData(DataInfoModel.ChannelId, DataInfoModel.DataPath, mastDs);
                    DataTable dt = mastDs.Tables["dataArray"];
                    DataInfoModel.ResultDataTable = dt;
                }, () =>
                {
                    return !string.IsNullOrEmpty(DataInfoModel.ChannelId);
                });
            }
        }

        void searchDataGrid_DataReferenceColumnClick(object sender, string columnName, string columnCaption)
        {
            VicTextBox vicTbox = (VicTextBox)sender;
            RefDataModel refData = new RefDataModel() { SystemId = DataInfoModel.SystemId, RefSystemId = DataInfoModel.RefSystemId, ConfigSystemId = DataInfoModel.ConfigsystemId, RefFieldCaption = columnCaption, ViewId = DataInfoModel.ChannelId, DataPath = DataInfoModel.DataPath, FieldName = columnName, RowValue = DataInfoModel.GridSelectedValue.ToString(), RefCallBack = new WaitCallback(SetData), RefFieldValue = vicTbox.VicText };
            UCUniversalRefWindow refWndow = new UCUniversalRefWindow(refData);
            VicWindowNormal window = new VicWindowNormal();
            window.SetResourceReference(VicWindowNormal.StyleProperty, "WindowMessageSkin");
            window.Title = columnCaption + "数据引用";
            window.Content = refWndow;
            window.Show();
        }

        private void SetData(object refData)
        {
            VicMessageBoxNormal.Show(JsonHelper.ToJson(refData));
        }


        void searchDataGrid_DataGridComboBoxClosed(object sender, string columnName)
        {
            //TODO:回填数据
            VicComboBoxNormal boxCol = (VicComboBoxNormal)sender;
            DataMessageOperation dataOp = new DataMessageOperation();
            string resultMessage = dataOp.SetRefData(DataInfoModel.ChannelId, DataInfoModel.DataPath, columnName, DataInfoModel.GridSelectedValue.ToString(), boxCol.SelectedValue);
        }

        void searchDataGrid_DataGridComboBoxOpened(object sender, string columnName)
        {
            VicComboBoxNormal boxCol = (VicComboBoxNormal)sender;
            DataMessageOperation dataOp = new DataMessageOperation();
            DataSet ds = new DataSet();
            string resultMessage = dataOp.GetRefData(DataInfoModel.ChannelId, DataInfoModel.DataPath, columnName, DataInfoModel.GridSelectedValue.ToString(), out ds);
            Dictionary<string, object> resultDic = JsonHelper.ToObject<Dictionary<string, object>>(resultMessage);
            if (resultDic["ReplyMode"].ToString().Equals("2"))
            {
                DataInfoModel.RefDataTable = ds.Tables["dataArray"];
            }
            if (resultDic["ReplyMode"].ToString().Equals("0"))
            {
                VicMessageBoxNormal.Show(resultDic["ReplyAlertMessage"].ToString());
            }

        }
        public ICommand btnAddDataClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataRow dr = DataInfoModel.ResultDataTable.NewRow();
                    if (DataInfoModel.ResultDataTable.Columns.Contains("_id"))
                    {
                        dr["_id"] = Guid.NewGuid().ToString();
                    }
                    DataInfoModel.ResultDataTable.Rows.Add(dr);
                });
            }
        }
        public ICommand btnSaveDataClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataMessageOperation dataOp = new DataMessageOperation();
                    dataOp.SaveData(DataInfoModel.ChannelId, DataInfoModel.DataPath);
                });
            }
        }

        public ICommand btnResetDataClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataMessageOperation dataOp = new DataMessageOperation();
                    bool result = dataOp.ResetData(DataInfoModel.ChannelId, DataInfoModel.DataPath);
                    VicMessageBoxNormal.Show(result.ToString());
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
                return new RelayCommand(() =>
                {
                    try
                    {
                        DataMessageOperation dataOp = new DataMessageOperation();
                        DataInfoModel.JsonData = dataOp.GetJSONData(DataInfoModel.ChannelId);
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, () =>
                {
                    return !string.IsNullOrEmpty(DataInfoModel.ChannelId);
                });
            }
        }

        public ICommand btnExportExcelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "Excel文件|*.xlsx;*.xls";
                    dlg.FileName = DataInfoModel.TableName;
                    Nullable<bool> result = dlg.ShowDialog();
                    if (result == true)
                    {
                        string filename = dlg.FileName;
                        DataTable exportDt = DataInfoModel.ResultDataTable.Copy();
                        if (exportDt.Columns.Contains("VicCheckFlag"))
                        {
                            exportDt.Columns.Remove("VicCheckFlag");
                        }
                        bool excelResult = DataTabletoExcel(exportDt, filename, DataInfoModel.TableName);
                        if (excelResult)
                        {
                            VicMessageBoxNormal.Show("导出成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            VicMessageBoxNormal.Show("导出失败!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 查看表结构
        /// </summary>
        public ICommand btnViewTableStructClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataMessageOperation dataOp = new DataMessageOperation();
                    string tableStructString = dataOp.GetModelInfo(DataInfoModel.ChannelId, "tables");
                    TableStructInfoList.Clear();
                    List<Dictionary<string, object>> tableList = JsonHelper.ToObject<List<Dictionary<string, object>>>(tableStructString);
                    foreach (Dictionary<string, object> item in tableList)
                    {
                        TableStructModel infoModel = new TableStructModel() { TableName = item["name"].ToString(), TableTitle = item["name"].ToString() };
                        TableStructInfoList.Add(infoModel);
                    }
                }, () =>
                {
                    return !string.IsNullOrEmpty(DataInfoModel.ChannelId);
                });
            }
        }

        /// <summary>
        /// 表结构选择改变
        /// </summary>
        public ICommand lboxTableStructSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedStructInfo != null)
                    {
                        string MessageType = "MongoDataChannelService.findBusiData";
                        DataMessageOperation messageOp = new DataMessageOperation();
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", "11");
                        contentDic.Add("configsystemid", "11");
                        contentDic.Add("modelid", "feidao-model-base_data_struct-0001");
                        if (!string.IsNullOrEmpty(DataInfoModel.SpaceId))
                        {
                            contentDic.Add("spaceId", DataInfoModel.SpaceId);
                        }
                        List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
                        Dictionary<string, object> conDic = new Dictionary<string, object>();
                        conDic.Add("name", "base_data_struct");
                        List<Dictionary<string, object>> tableConList = new List<Dictionary<string, object>>();
                        Dictionary<string, object> tableConDic = new Dictionary<string, object>();
                        tableConDic.Add("tablename", SelectedStructInfo.TableName);
                        string productid = "feidao";
                        if (!string.IsNullOrEmpty(DataInfoModel.SpaceId))
                        {
                            if (DataInfoModel.SpaceId.Contains("::"))
                            {
                                productid = DataInfoModel.SpaceId.Substring(DataInfoModel.SpaceId.IndexOf("::") + 2);
                            }
                            else
                            {
                                productid = DataInfoModel.SpaceId;
                            }
                        }
                        tableConDic.Add("productid", productid);
                        tableConList.Add(tableConDic);
                        conDic.Add("tablecondition", tableConList);
                        conList.Add(conDic);
                        contentDic.Add("conditions", conList);
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                        if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
                        {
                            string channelId = returnDic["DataChannelId"].ToString();
                            DataSet mastDs = new DataSet();
                            mastDs = messageOp.GetData(channelId, "[\"base_data_struct\"]", mastDs);
                            DataTable dt = mastDs.Tables["dataArray"];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                SelectedStructInfo.TableFields = JsonHelper.ReadJsonString(dt.Rows[0]["fields"].ToString(), "dataArray");
                            }
                            else
                            {
                                VicMessageBoxNormal.Show(string.Format("未查询到产品编号为{0},表名为{1}的表结构信息", productid, SelectedStructInfo.TableName));
                            }
                        }
                        else
                        {
                            VicMessageBoxNormal.Show(returnDic["ReplyAlertMessage"].ToString());
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 窄表引用
        /// </summary>
        public ICommand btnViewNarrowDataClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataMessageOperation dataOp = new DataMessageOperation();
                    DataSet ds = new DataSet();
                    string resultMessage = dataOp.GetRefData(DataInfoModel.ChannelId, DataInfoModel.DataPath, DataInfoModel.NarrowRefField, DataInfoModel.NarrowRowValue, out ds, null, DataInfoModel.SystemId, DataInfoModel.ConfigsystemId);
                    Dictionary<string, object> resultDic = JsonHelper.ToObject<Dictionary<string, object>>(resultMessage);
                    if (resultDic["ReplyMode"].ToString().Equals("2"))
                    {
                        DataInfoModel.RefDataTable = ds.Tables["dataArray"];
                    }
                    if (resultDic["ReplyMode"].ToString().Equals("0"))
                    {
                        VicMessageBoxNormal.Show(resultDic["ReplyAlertMessage"].ToString());
                    }
                }, () =>
                {
                    return !string.IsNullOrEmpty(DataInfoModel.ChannelId);
                });
            }
        }

        public ICommand btnSetNarrowDataClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    //DataMessageOperation dataOp = new DataMessageOperation();
                    //dataOp.SetRefData(DataInfoModel.ChannelId, DataInfoModel.DataPath, DataInfoModel.NarrowRefField, DataInfoModel.NarrowRowValue, DataInfoModel.NarrowGridSelectedValue);
                    DataTable dt = new DataTable();

                    DataColumn dcprice1 = new DataColumn("price1");
                    dcprice1.DataType = typeof(decimal);
                    dt.Columns.Add(dcprice1);

                    DataColumn dcprice2 = new DataColumn("price2");
                    dcprice2.DataType = typeof(decimal);
                    dt.Columns.Add(dcprice2);

                    DataColumn dcprice3 = new DataColumn("price3");
                    dcprice3.DataType = typeof(decimal);
                    dcprice3.Expression = "IIF(price1>price2,price2,price1)";
                    dt.Columns.Add(dcprice3);

                    DataRow dr = dt.NewRow();
                    dr["price1"] = 0;
                    dr["price2"] = 0;
                    dr["price3"] = 0;
                    dt.Rows.Add(dr);

                    DataInfoModel.RefDataTable = dt;

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
                    try
                    {
                        DataMessageOperation messageOp = new DataMessageOperation();
                        string MessageType = "MongoDataChannelService.findDocCode";
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", CodeInfoModel.SystemId);
                        contentDic.Add("configsystemid", CodeInfoModel.ConfigsystemId);
                        contentDic.Add("pname", CodeInfoModel.PName);
                        contentDic.Add("setinfo", CodeInfoModel.SetInfo);
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                        if (returnDic != null)
                        {
                            CodeInfoModel.ResultData = returnDic["ReplyContent"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, () =>
                {
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
                    try
                    {
                        DataMessageOperation messageOp = new DataMessageOperation();
                        string MessageType = "MongoDataChannelService.afterLogin";
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", UserInfoModel.SystemId);
                        contentDic.Add("configsystemid", UserInfoModel.ConfigsystemId);
                        contentDic.Add("clientType", UserInfoModel.ClientType);
                        contentDic.Add("userCode", UserInfoModel.UserCode);
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                        if (returnDic != null)
                        {
                            UserInfoModel.ResultData = returnDic["ReplyContent"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, () =>
                {
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
                return new RelayCommand(() =>
                {
                    try
                    {
                        DataMessageOperation messageOp = new DataMessageOperation();
                        Dictionary<string, object> contentDic = string.IsNullOrEmpty(OtherInfoModel.OtherConditionData) ? new Dictionary<string, object>() : JsonHelper.ToObject<Dictionary<string, object>>(OtherInfoModel.OtherConditionData);
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(OtherInfoModel.MessageType, contentDic);
                        if (returnDic != null)
                        {
                            OtherInfoModel.OtherResultData = returnDic["ReplyContent"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                });
            }
        }
        /// <summary>
        /// 下载服务
        /// </summary>
        public ICommand btnDownLoadClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Dictionary<string, object> downloadMessageContent = new Dictionary<string, object>();
                    Dictionary<string, string> downloadAddress = new Dictionary<string, string>();
                    downloadAddress.Add("DownloadFileId", DownInfoModel.FieldId);
                    downloadAddress.Add("DownloadToPath", DownInfoModel.DownLoadPath);
                    downloadAddress.Add("ProductId", DownInfoModel.ProductId);
                    downloadMessageContent.Add("ServiceParams", JsonHelper.ToJson(downloadAddress));
                    DataMessageOperation messageOperation = new DataMessageOperation();
                    Dictionary<string, object> downloadResult = messageOperation.SendSyncMessage("ServerCenterService.DownloadDocument",
                                                           downloadMessageContent);
                    if (downloadResult != null)
                    {
                        DownInfoModel.DownLoadResult = JsonHelper.ToJson(downloadResult);
                    }
                    else
                    {
                        VicMessageBoxNormal.Show("执行下载服务出错");
                    }
                }, () =>
                {
                    bool result = true;
                    if (string.IsNullOrEmpty(DownInfoModel.FieldId))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(DownInfoModel.DownLoadPath))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(DownInfoModel.ProductId))
                    {
                        return false;
                    }
                    return result;
                });
            }
        }


        public ICommand UCMainUnloadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    //string Uid = (string)x;
                    //DataMessageOperation messageOp = new DataMessageOperation();
                    //if (!string.IsNullOrEmpty(Uid))
                    //{
                    //    string messageType = "PluginService.PluginStop";
                    //    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    //    contentDic.Add("ObjectId", Uid);
                    //    messageOp.SendAsyncMessage(messageType, contentDic);
                    //}
                });
            }
        }

        #endregion
        #region 私有方法
        public bool DataTabletoExcel(System.Data.DataTable tmpDataTable, string strFileName, string sheetName)
        {
            bool flag = false;
            if (tmpDataTable == null)
            {
                return false;
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                int rowNum = tmpDataTable.Rows.Count;
                int columnNum = tmpDataTable.Columns.Count;
                int rowIndex = 1;
                int columnIndex = 0;

                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = false;
                xlApp.AlertBeforeOverwriting = false;
                xlApp.SheetsInNewWorkbook = 1;

                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = xlBook.Sheets.get_Item(1) as Microsoft.Office.Interop.Excel.Worksheet;
                worksheet.Name = sheetName;
                //将DataTable的列名导入Excel表第一行
                foreach (DataColumn dc in tmpDataTable.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.Caption + "[" + dc.ColumnName + "]";
                }
                //将DataTable中的数据导入Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        xlApp.Cells[rowIndex, columnIndex] = tmpDataTable.Rows[i][j].ToString();
                    }
                }
                //xlBook.SaveCopyAs(HttpUtility.UrlDecode(strFileName, System.Text.Encoding.UTF8));
                xlBook.SaveCopyAs(strFileName);

                flag = true;
            }

            catch (Exception)
            {
                return false;
            }
            finally
            {
                xlApp.Quit();
                xlApp = null;
                System.GC.Collect();
                KillProcess("EXCEL");
            }
            return flag;
        }
        /// <summary>
        /// 根据进程名称杀死进程 
        /// </summary>
        /// <param name=" ProcessName "> DataTable</param>
        public void KillProcess(string ProcessName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            try
            {

                foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcessesByName(ProcessName))
                {
                    if (!thisproc.CloseMainWindow())
                    {
                        thisproc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("杀死进程异常：{0}", ex.Message);
            }
        }
        #endregion
    }
}
