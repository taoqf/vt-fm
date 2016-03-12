using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls;
using Victop.Wpf.Controls;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 单据模板类
    /// </summary>
    public class BillTemplateControl : TemplateControl
    {
        private TemplateControl MainView;
        BillDefinModel definModel = new BillDefinModel();
        /// <summary>
        /// 凭证
        /// </summary>
        public BillDefinModel DefinModel
        {
            get { return definModel; }
            set { definModel = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="groupName">规则分组名</param>
        /// <param name="mainView">主应用程序</param>
        public BillTemplateControl(TemplateControl mainView)
        {
            MainView = mainView;
            string pvdPath = string.Format("{0}.PVD.{1}.json", mainView.GetType().Assembly.GetName().Name, "billsetting");
            Stream pvdStream = mainView.GetType().Assembly.GetManifestResourceStream(pvdPath);
            if (pvdStream != null)
            {
                string pvdStr = FileHelper.ReadText(pvdStream);
                DefinModel = JsonHelper.ToObject<BillDefinModel>(pvdStr);//反解析JSON串
            }
        }

        #region 发送获取编码消息
        /// <summary>
        /// 发送获取编码消息
        /// </summary>
        /// <returns>返回编码值</returns>
        public string SendGetCodeMessage()
        {
            string codeNo = string.Empty;
            try
            {
                if (DefinModel == null)
                {
                    return string.Empty;
                }
                string relStr = JsonHelper.ReadJsonString(DefinModel.EncodedService.ToString(), "dataArray");
                if (!string.IsNullOrEmpty(relStr))
                {
                    List<EncodedServiceModel> codeList = JsonHelper.ToObject<List<EncodedServiceModel>>(relStr);
                    if (codeList != null)
                    {
                        string MessageType = "MongoDataChannelService.findDocCode";
                        DataMessageOperation messageOp = new DataMessageOperation();
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", "11");
                        foreach (EncodedServiceModel model in codeList)
                        {
                            switch (model.ParamType)
                            {
                                case "field":
                                    break;
                                case "syspan":
                                    contentDic.Add("setinfo", model.ParamName);
                                    break;
                                case "const":
                                    contentDic.Add("pname", model.ParamName);
                                    break;
                            }
                        }
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                        if (returnDic != null || returnDic.ContainsKey("ReplyContent"))
                        {
                            codeNo = JsonHelper.ReadJsonString(returnDic["ReplyContent"].ToString(), "result");
                        }
                    }
                }
                return codeNo;
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("发送获取编码消息异常（string SendGetCodeMessage）：{0}", ex.Message);
                return string.Empty;
            }
        }
        #endregion

        #region 单据状态获取
        /// <summary>
        /// 单据状态获取
        /// </summary>
        /// <returns>返回键值数据，key=单据名称，value=单据码</returns>
        public Dictionary<string, object> GetDocState()
        {
            Dictionary<string, object> docStateDic = new Dictionary<string, object>();
            string relStr = JsonHelper.ReadJsonString(DefinModel.Gsdocstate.ToString(), "dataArray");
            if (!string.IsNullOrEmpty(relStr))
            {
                List<GsdocstateModel> docStateList = JsonHelper.ToObject<List<GsdocstateModel>>(relStr);
                if (docStateList != null)
                {
                    foreach (GsdocstateModel model in docStateList)
                    {
                        if (!docStateDic.Keys.Contains(model.StatusName))
                        {
                            docStateDic.Add(model.StatusName, model.StatusCode);
                        }
                    }
                }
            }
            return docStateDic;
        }
        #endregion

        #region 动态字段赋值展示
        /// <summary>
        /// 动态字段赋值展示
        /// </summary>
        /// <param name="dtData">添加动态字段数据源</param>
        private void DynamicFieldsAssignment(DataTable dtData)
        {
            if (dtData.Columns.Contains("dynamic"))
            {
                string dynStr = JsonHelper.ReadJsonString(dtData.Rows[0]["dynamic"].ToString(), "dataArray");
                string relStr = JsonHelper.ReadJsonString(DefinModel.DynamicFields.ToString(), "dataArray");
                if (!string.IsNullOrEmpty(dynStr) && !string.IsNullOrEmpty(relStr))
                {
                    Dictionary<string, object> dynamicDic = JsonHelper.ToObject<Dictionary<string, object>>(dynStr);
                    List<DynamicFieldsModel> dynamicFieldsList = JsonHelper.ToObject<List<DynamicFieldsModel>>(relStr);
                    if (dynamicFieldsList != null)
                    {
                        foreach (string key in dynamicDic.Keys)
                        {
                            DynamicFieldsModel model = dynamicFieldsList.FirstOrDefault(it => it.FieldName.Equals(key));
                            if (model != null && !dtData.Columns.Contains(model.FieldName))
                            {
                                #region 添加字段
                                DataColumn dc = new DataColumn();
                                dc.ColumnName = model.FieldName;
                                switch (model.FieldType)
                                {
                                    case "Boolean":
                                        dc.DataType = typeof(bool);
                                        break;
                                    case "DateTime":
                                        dc.DataType = typeof(DateTime);
                                        break;
                                    case "Double":
                                        dc.DataType = typeof(double);
                                        break;
                                    case "float":
                                        dc.DataType = typeof(float);
                                        break;
                                    case "Int32":
                                        dc.DataType = typeof(Int32);
                                        break;
                                    case "Int64":
                                        dc.DataType = typeof(Int64);
                                        break;
                                    case "String":
                                    default:
                                        dc.DataType = typeof(string);
                                        break;
                                }
                                dtData.Columns.Add(dc);
                                #endregion
                                #region 赋值
                                if (dtData.Rows.Count > 0)
                                {
                                    DataRow dr = dtData.Rows[0];
                                    switch (model.FieldType)
                                    {
                                        case "Boolean":
                                            dr[key] = string.IsNullOrEmpty(dynamicDic[key].ToString()) ? Convert.ToBoolean(model.FieldValue) : Convert.ToBoolean(dynamicDic[key]);
                                            break;
                                        case "DateTime":
                                            if (string.IsNullOrEmpty(dynamicDic[key].ToString()))
                                            {
                                                dr[key] = DBNull.Value;
                                            }
                                            else
                                            {
                                                dr[key] = dynamicDic[key];
                                            }
                                            break;
                                        case "Double":
                                            dr[key] = string.IsNullOrEmpty(dynamicDic[key].ToString()) ? Convert.ToDouble(model.FieldValue.ToString()) : Convert.ToDouble(dynamicDic[key].ToString());
                                            break;
                                        case "float":
                                            dr[key] = string.IsNullOrEmpty(dynamicDic[key].ToString()) ? Convert.ToDecimal(model.FieldValue.ToString()) : Convert.ToDecimal(dynamicDic[key].ToString());
                                            break;
                                        case "Int32":
                                            dr[key] = string.IsNullOrEmpty(dynamicDic[key].ToString()) ? Convert.ToInt32(model.FieldValue.ToString()) : Convert.ToInt32(dynamicDic[key].ToString());
                                            break;
                                        case "Int64":
                                            dr[key] = string.IsNullOrEmpty(dynamicDic[key].ToString()) ? Convert.ToInt64(model.FieldValue.ToString()) : Convert.ToInt64(dynamicDic[key].ToString());
                                            break;
                                        case "String":
                                        default:
                                            dr[key] = string.IsNullOrEmpty(dynamicDic[key].ToString()) ? model.FieldValue.ToString() : dynamicDic[key].ToString();
                                            break;
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
