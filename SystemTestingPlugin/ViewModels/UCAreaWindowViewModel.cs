using SystemTestingPlugin.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using System.Diagnostics;
using Victop.Frame.DataMessageManager;
using SystemTestingPlugin.Views;
using System.Threading;
using System.IO;

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
                        contentDic.Add("systemid", DataInfoModel.SystemId);
                        contentDic.Add("configsystemid", DataInfoModel.ConfigsystemId);
                        contentDic.Add("refsystemid", string.IsNullOrEmpty(DataInfoModel.RefSystemId) ? DataInfoModel.SystemId : DataInfoModel.RefSystemId);
                        contentDic.Add("emptydataflag", DataInfoModel.EmptyFlag ? 0 : 1);
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
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
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
                            DataTable structdt = new DataTable("dataArray");
                            DataColumn iddc = new DataColumn("_id", typeof(string));
                            iddc.ExtendedProperties.Add("ColType", "string");
                            structdt.Columns.Add(iddc);

                            DataColumn nameDc = new DataColumn("user_name",typeof(string));
                            nameDc.ExtendedProperties.Add("ColType", "string");
                            structdt.Columns.Add(nameDc);

                            DataColumn birdc = new DataColumn("birthday", typeof(DateTime));
                            birdc.ExtendedProperties.Add("ColType", "timestamp");
                            structdt.Columns.Add(birdc);

                            DataColumn regdc = new DataColumn("regist_date", typeof(DateTime));
                            regdc.ExtendedProperties.Add("ColType", "timestamp");
                            structdt.Columns.Add(regdc);

                            mastDs.Tables.Add(structdt);

                            mastDs = messageOp.GetData(DataInfoModel.ChannelId, DataInfoModel.DataPath, mastDs);
                            DataTable dt = mastDs.Tables["dataArray"];
                            DataInfoModel.ResultDataTable = dt;
                        }
                        else
                        {
                            if (returnDic != null && returnDic.ContainsKey("ReplyAlertMessage"))
                            {
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
                    dr["_id"] = Guid.NewGuid().ToString();
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
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
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
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
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
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(OtherInfoModel.MessageType, contentDic, "JSON");
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
    }
}
