using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Victop.Frame.CoreLibrary.Enums;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.CoreLibrary.MongoModel;
using Victop.Frame.DataChannel;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls;
using Victop.Server.Controls.Models;

namespace Victop.Frame.DataMessageManager
{
    /// <summary>
    /// 数据引用操作
    /// </summary>
    public class DataMessageOperation
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="viewId">通道标识</param>
        /// <param name="dataPath">路径</param>
        /// <param name="structDs">结构Dataset</param>
        /// <returns></returns>
        public DataSet GetData(string viewId, string dataPath, DataSet structDs = null)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.GetData(viewId, dataPath, structDs);
        }
        /// <summary>
        /// 保存数据到数据通道
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="dataPath">路径</param>
        /// <returns></returns>
        public virtual bool SaveData(string channelId, string dataPath)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.SaveData(channelId, dataPath);
        }
        /// <summary>
        /// 获取JSON数据
        /// </summary>
        /// <param name="viewId">通道标识</param>
        /// <returns></returns>
        public string GetJSONData(string viewId)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.GetJSONData(viewId);
        }
        /// <summary>
        /// 依据路径获取JSON数据
        /// </summary>
        /// <param name="viewId">通道标识</param>
        /// <param name="dataPath">路径</param>
        /// <returns></returns>
        public string GetJSONData(string viewId, string dataPath)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.GetJSONData(viewId, dataPath);
        }
        /// <summary>
        /// 释放数据
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public virtual bool DisposeData(string channelId)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.DisposeData(channelId);
        }
        /// <summary>
        /// 获取模型关系
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public virtual string GetModelRelation(string channelId)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.GetModelRelation(channelId);
        }
        /// <summary>
        /// 同步消息发送
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="messageContent">消息体</param>
        /// <param name="dataForm">数据格式(JSON/DATASET)</param>
        /// <param name="waiteTime">同步等待时间</param>
        /// <returns>应答消息内容</returns>
        public Dictionary<string, object> SendSyncMessage(string messageType, Dictionary<string, object> messageContent, string dataForm="JSON", int waiteTime = 15)
        {
            MessageOperation messageOp = new MessageOperation();
            return messageOp.SendMessage(messageType, messageContent, dataForm, waiteTime);
        }
        /// <summary>
        /// 异步消息发送
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="messageContent">消息体</param>
        /// <param name="replyCallBack">回调方法</param>
        /// <param name="dataForm">数据格式</param>
        /// <param name="validTime">超时时间</param>
        public void SendAsyncMessage(string messageType, Dictionary<string, object> messageContent, WaitCallback replyCallBack=null, string dataForm="JSON", long validTime = 15)
        {
            PluginMessage pluginMessage = new PluginMessage();
            pluginMessage.SendMessage(messageType, messageContent, replyCallBack, dataForm.Equals("JSON") ? DataFormEnum.JSON : DataFormEnum.DATASET, validTime);
        }

        /// <summary>
        /// 获取引用数据
        /// </summary>
        /// <param name="viewId">通道标识</param>
        /// <param name="dataPath">路径</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="rowValue">行值</param>
        /// <param name="RefDataSet">返回</param>
        /// <param name="systemId">系统Id</param>
        /// <param name="configsystemId">配置系统Id</param>
        /// <param name="foreRunnerFlag">前导标签</param>
        /// <returns>引用消息</returns>
        public string GetRefData(string viewId, string dataPath, string fieldName, string rowValue, out DataSet RefDataSet, string systemId = null, string configsystemId = null, bool foreRunnerFlag = true)
        {
            RefDataSet = new DataSet();
            string resultMessage = string.Empty;
            DataStoreInfo storeInfo = null;
            string RebuildPath = string.Empty;
            DataOperation dataOp = new DataOperation();
            ChannelData channelData = dataOp.GetChannelData(viewId);
            foreach (JsonMapKey item in DataConvertManager.JsonTableMap.Keys)
            {
                if (item.ViewId.Equals(viewId) && item.DataPath.Equals(dataPath))
                {
                    storeInfo = DataConvertManager.JsonTableMap[item] as DataStoreInfo;
                    break;
                }
            }
            if (storeInfo.RefDataInfo != null)
            {
                #region 构建Path
                List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
                for (int i = 0; i < pathList.Count; i++)
                {
                    if (pathList[i].GetType().Name.Equals("String"))
                    {
                        RebuildPath += pathList[i].ToString() + ".";
                        if (i == pathList.Count - 1)
                        {
                            RebuildPath += "dataArray.";
                        }
                    }
                    else
                    {
                        RebuildPath += "dataArray.";
                    }
                }
                RebuildPath += fieldName;
                #endregion
                RefRelationInfo relationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(RebuildPath) && it.RowId.Equals(rowValue));
                if (relationInfo != null)
                {
                    //TODO:已存在于引用映射关系中，对简单引用和标准引用进行分别处理
                    switch (relationInfo.RefType)
                    {
                        case RefTypeEnum.SIMPLEREF:
                            foreach (MongoSimpleRefInfoOfArrayModel item in channelData.SimpleRefInfo.SimpleDataArray)
                            {
                                MongoSimpleRefInfoOfArrayPropertyModel propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(RebuildPath));
                                if (propertyModel != null)
                                {
                                    if (string.IsNullOrEmpty(propertyModel.PropertyDepend))
                                    {
                                        DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, null);
                                        RefDataSet.Tables.Add(dt);
                                        ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC };
                                        resultMessage = JsonHelper.ToJson(replyMessage);
                                    }
                                    else
                                    {
                                        RefRelationInfo dependRelationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(propertyModel.PropertyKey) && it.RowId.Equals(rowValue));
                                        if (dependRelationInfo != null && dependRelationInfo.FieldValue != null)
                                        {
                                            Dictionary<string, object> dependDic = new Dictionary<string, object>();
                                            dependDic.Add(dependRelationInfo.TriggerField, dependRelationInfo.FieldValue);
                                            DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, dependDic);
                                            RefDataSet.Tables.Add(dt);
                                            ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC };
                                            resultMessage = JsonHelper.ToJson(replyMessage);
                                        }
                                        else
                                        {
                                            ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "依赖字段无值" };
                                            resultMessage = JsonHelper.ToJson(replyMessage);
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        case RefTypeEnum.CLIENTREF:
                            RefDataSet = dataOp.GetData(relationInfo.DataChannelId, string.Format("[\"{0}\"]", relationInfo.TriggerTable));
                            MongoModelInfoOfClientRefModel clientRefModel = channelData.ModelDefInfo.ModelClientRef.FirstOrDefault(it => it.ClientRefField == relationInfo.TriggerField);
                            resultMessage = JsonHelper.ToJson(new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC, ReplyContent = JsonHelper.ToJson(clientRefModel) });
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    bool simpleRefFlag = false;
                    foreach (MongoSimpleRefInfoOfArrayModel item in channelData.SimpleRefInfo.SimpleDataArray)
                    {
                        MongoSimpleRefInfoOfArrayPropertyModel propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(RebuildPath));
                        //TODO:检索简单引用，判断是否有依赖，无依赖则检索简单引用数据，有依赖则检索实际表中的依赖字段是否有值，若无值则返回依赖无值提示，有值，则根据依赖值获取数据
                        if (propertyModel != null)
                        {
                            relationInfo = new RefRelationInfo() { Id = Guid.NewGuid().ToString(), RowId = rowValue, RefType = RefTypeEnum.SIMPLEREF, TriggerField = propertyModel.PropertyKey };
                            if (string.IsNullOrEmpty(propertyModel.PropertyDepend))
                            {
                                DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, null);
                                RefDataSet.Tables.Add(dt);
                                ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC };
                                resultMessage = JsonHelper.ToJson(replyMessage);
                            }
                            else
                            {
                                RefRelationInfo dependRelationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(propertyModel.PropertyKey) && it.RowId.Equals(rowValue));
                                if (dependRelationInfo != null && dependRelationInfo.FieldValue != null)
                                {
                                    relationInfo.DependId = dependRelationInfo.Id;
                                    Dictionary<string, object> dependDic = new Dictionary<string, object>();
                                    dependDic.Add(dependRelationInfo.TriggerField, dependRelationInfo.FieldValue);
                                    DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, dependDic);
                                    RefDataSet.Tables.Add(dt);
                                    ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC };
                                    resultMessage = JsonHelper.ToJson(replyMessage);
                                }
                                else
                                {
                                    ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "依赖字段无值" };
                                    resultMessage = JsonHelper.ToJson(replyMessage);
                                }
                            }
                            if (storeInfo.RefDataInfo.FirstOrDefault(it => it.RowId.Equals(rowValue) && it.TriggerField.Equals(RebuildPath)) == null)
                            {
                                storeInfo.RefDataInfo.Add(relationInfo);
                            }
                            simpleRefFlag = true;
                            break;
                        }
                    }
                    if (!simpleRefFlag)//当简单引用判断为不存在是，判定是否有标准引用
                    {
                        MongoModelInfoOfClientRefModel clientRefModel = channelData.ModelDefInfo.ModelClientRef.FirstOrDefault(it => it.ClientRefField.Equals(RebuildPath));
                        if (clientRefModel != null)
                        {
                            relationInfo = new RefRelationInfo() { Id = Guid.NewGuid().ToString(), TriggerField = clientRefModel.ClientRefField, RefType = RefTypeEnum.CLIENTREF, RowId = rowValue };
                            if (clientRefModel.ClientRefForeRunner != null && !string.IsNullOrEmpty(clientRefModel.ClientRefForeRunner.RefModel) && !foreRunnerFlag)
                            {
                                //TODO:根据前导决定是否自动查询
                                ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ROUTER, ReplyContent = JsonHelper.ToJson(clientRefModel) };
                                resultMessage = JsonHelper.ToJson(replyMessage);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(systemId) || string.IsNullOrEmpty(configsystemId))
                                {
                                    ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "请传入正确的systemId及ConfigsystemId" };
                                    resultMessage = JsonHelper.ToJson(replyMessage);
                                }
                                else
                                {
                                    string messageType = "MongoDataChannelService.findBusiData";
                                    string refTableName = string.Empty;
                                    MessageOperation messageOp = new MessageOperation();
                                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                                    contentDic.Add("systemid", systemId);
                                    contentDic.Add("configsystemid", configsystemId);
                                    contentDic.Add("modelid", clientRefModel.ClientRefModel);
                                    if (clientRefModel.ClientRefConditionFirst.Count > 0)
                                    {
                                        refTableName = clientRefModel.ClientRefConditionFirst[0].ConditionRight.Substring(0, clientRefModel.ClientRefConditionFirst[0].ConditionRight.IndexOf('.'));
                                        List<Dictionary<string, object>> conditionList = new List<Dictionary<string, object>>();
                                        DataRow[] drs = storeInfo.ActualDataInfo.Tables["dataArray"].Select(string.Format("_id='{0}'", rowValue));
                                        if (drs != null && drs.Count() > 0)
                                        {
                                            Dictionary<string, object> tableDic = new Dictionary<string, object>();
                                            tableDic.Add("name", refTableName);
                                            List<Dictionary<string, object>> tableconditionList = new List<Dictionary<string, object>>();
                                            foreach (MongoModelInfoOfClientRefConditionModel item in clientRefModel.ClientRefConditionFirst)
                                            {
                                                string refRightField = item.ConditionRight.Substring(item.ConditionRight.LastIndexOf('.') + 1);
                                                string refLeftField = item.ConditionLeft.Substring(item.ConditionLeft.LastIndexOf('.') + 1);
                                                if (item.ConditionDependFlag == 1)
                                                {
                                                    //TODO:处理依赖关系
                                                    if (drs[0].Table.Columns.Contains(refLeftField) && drs[0][refLeftField] != null)
                                                    {
                                                        Dictionary<string, object> itemDic = new Dictionary<string, object>();
                                                        itemDic.Add(refRightField, drs[0][refLeftField]);
                                                        tableconditionList.Add(itemDic);
                                                    }
                                                    else
                                                    {
                                                        ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "引用依赖字段" + refLeftField + "无值" };
                                                        resultMessage = JsonHelper.ToJson(replyMessage);
                                                    }
                                                }
                                                else
                                                {
                                                    Dictionary<string, object> itemDic = new Dictionary<string, object>();
                                                    itemDic.Add(refRightField, drs[0][refLeftField]);
                                                    tableconditionList.Add(itemDic);
                                                }
                                            }
                                            tableDic.Add("tablecondition", tableconditionList);
                                            conditionList.Add(tableDic);
                                            contentDic.Add("conditions", conditionList);
                                        }
                                        else
                                        {
                                            ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "数据不存在" };
                                            resultMessage = JsonHelper.ToJson(replyMessage);
                                        }
                                    }
                                    //TODO:去取数
                                    Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic, "JSON");
                                    if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
                                    {
                                        relationInfo.DataChannelId = returnDic["DataChannelId"].ToString();
                                        relationInfo.TriggerTable = refTableName;
                                        RefDataSet = dataOp.GetData(returnDic["DataChannelId"].ToString(), string.Format("[\"{0}\"]", refTableName));
                                        ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC, ReplyContent = JsonHelper.ToJson(clientRefModel) };
                                        resultMessage = JsonHelper.ToJson(replyMessage);
                                        if (storeInfo.RefDataInfo.FirstOrDefault(it => it.RowId.Equals(rowValue) && it.TriggerField.Equals(RebuildPath)) == null)
                                        {
                                            storeInfo.RefDataInfo.Add(relationInfo);
                                        }
                                    }
                                    else
                                    {
                                        resultMessage = JsonHelper.ToJson(returnDic);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "不存在引用" };
                            resultMessage = JsonHelper.ToJson(replyMessage);
                        }
                    }
                }
            }
            return resultMessage;
        }
        /// <summary>
        /// 回填引用数据
        /// </summary>
        /// <param name="viewId">通道标识</param>
        /// <param name="dataPath">路径</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="rowValue">行值</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public string SetRefData(string viewId, string dataPath, string fieldName, string rowValue, object fieldValue)
        {
            string resultMessage = string.Empty;
            string RebuildPath = string.Empty;
            DataStoreInfo storeInfo = null;
            DataOperation dataOp = new DataOperation();
            ChannelData channelData = dataOp.GetChannelData(viewId);
            foreach (JsonMapKey item in DataConvertManager.JsonTableMap.Keys)
            {
                if (item.ViewId.Equals(viewId) && item.DataPath.Equals(dataPath))
                {
                    storeInfo = DataConvertManager.JsonTableMap[item] as DataStoreInfo;
                    break;
                }
            }
            if (storeInfo.RefDataInfo != null)
            {
                #region 构建Path
                List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
                for (int i = 0; i < pathList.Count; i++)
                {
                    if (pathList[i].GetType().Name.Equals("String"))
                    {
                        RebuildPath += pathList[i].ToString() + ".";
                        if (i == pathList.Count - 1)
                        {
                            RebuildPath += "dataArray.";
                        }
                    }
                    else
                    {
                        RebuildPath += "dataArray.";
                    }
                }
                RebuildPath += fieldName;
                #endregion
                RefRelationInfo relationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(RebuildPath) && it.RowId.Equals(rowValue));
                if (relationInfo != null)
                {
                    relationInfo.FieldValue = fieldValue;
                    ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC };
                    resultMessage = JsonHelper.ToJson(replyMessage);
                }
                else
                {
                    ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "不存在引用" };
                    resultMessage = JsonHelper.ToJson(replyMessage);
                }
            }
            return resultMessage;
        }

        /// <summary>
        /// 启动窗口插件
        /// </summary>
        /// <param name="PluginName">插件名称</param>
        /// <param name="paramDic">参数键值对</param>
        /// <param name="waitTime">同步等待时间(秒)</param>
        /// <returns></returns>
        public PluginModel StratPlugin(string PluginName, Dictionary<string, object> paramDic = null, long waitTime = 15)
        {
            PluginModel pluginModel = new PluginModel();
            MessageOperation messageOp = new MessageOperation();
            string messageType = "PluginService.PluginRun";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("PluginName", PluginName);
            contentDic.Add("PluginPath", "");
            contentDic.Add("PluginParam", JsonHelper.ToJson(paramDic));
            Dictionary<string, object> resultDic = messageOp.SendMessage(messageType, contentDic);
            if (!resultDic["ReplyMode"].ToString().Equals("0"))
            {
                string messageId = resultDic["MessageId"].ToString();
                DataOperation PluginOper = new DataOperation();
                List<Dictionary<string, object>> pluginList = PluginOper.GetPluginInfo();
                foreach (var item in pluginList)
                {
                    if (item["ObjectId"].ToString().Equals(messageId))
                    {
                        pluginModel.PluginInterface = item["IPlugin"] as IPlugin;
                        pluginModel.AppId = item["AppId"].ToString();
                        pluginModel.ObjectId = item["ObjectId"].ToString();
                        break;
                    }
                }
            }
            else
            {
                pluginModel = new PluginModel()
                {
                    ErrorMsg = resultDic["ReplyAlertMessage"].ToString(),
                    ObjectId = string.Empty
                };
            }
            return pluginModel;
        }
        /// <summary>
        /// 释放插件
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public bool StopPlugin(string ObjectId, int waitTime = 15)
        {
            bool result = false;
            MessageOperation messageOp = new MessageOperation();
            string messageType = "PluginService.PluginStop";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("ObjectId", ObjectId);
            Dictionary<string, object> resultDic = messageOp.SendMessage(messageType, contentDic, waitTime);
            if (resultDic != null)
            {
                result = resultDic["ReplyMode"].ToString().Equals("0") ? false : true;
            }
            return result;
        }
        /// <summary>
        /// 获取活动插件信息
        /// </summary>
        /// <param name="objectId">对象id</param>
        /// <returns></returns>
        public virtual List<Dictionary<string, object>> GetPluginInfo()
        {
            DataOperation PluginOper = new DataOperation();
            List<Dictionary<string, object>> pluginInfo = PluginOper.GetPluginInfo();
            return pluginInfo;
        }
        /// <summary>
        /// 根据BusinessKey获取活动插件
        /// </summary>
        /// <param name="businessKey"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetPluginInfoByBusinessKey(string businessKey)
        {
            DataOperation pluginOper = new DataOperation();
            return pluginOper.GetPluginInfoByBusinessKey(businessKey);
        }

        #region 私有方法
        /// <summary>
        /// 根据结构Path获取数据Table
        /// </summary>
        /// <param name="constructPath"></param>
        /// <param name="dependPath"></param>
        /// <param name="jsonData"></param>
        /// <param name="dependDic"></param>
        /// <returns></returns>
        private DataTable GetDataByConsturctPath(string constructPath, string valuePath, Dictionary<string, object> jsonDataDic, Dictionary<string, object> dependDic)
        {
            DataTable dt = new DataTable("dataArray");
            dt.Columns.Add("txt");
            dt.Columns.Add("val");
            if (dependDic != null)
            {
                foreach (string item in dependDic.Keys)
                {
                    string replaceStr = valuePath.Substring(0, valuePath.IndexOf("dataArray") + 9);
                    Dictionary<string, object> itemValueDic = new Dictionary<string, object>();
                    itemValueDic.Add("key", "val");
                    itemValueDic.Add("value", dependDic[item].ToString());
                    valuePath = valuePath.Replace(replaceStr, string.Format("{0}.{1}", replaceStr.Substring(0, replaceStr.LastIndexOf(".")), JsonHelper.ToJson(itemValueDic)));
                }
            }
            valuePath = valuePath.Substring(0, valuePath.LastIndexOf('.'));
            string jsonData = GetSimpeRefJsonData(jsonDataDic, valuePath);
            List<Dictionary<string, object>> valueList = new List<Dictionary<string, object>>();
            valueList = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ReadJsonString(jsonData, "dataArray"));
            if (valueList == null)
            {
                valueList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonData);
            }
            if (valueList != null)
            {
                foreach (Dictionary<string, object> item in valueList)
                {
                    DataRow dr = dt.NewRow();
                    dr["txt"] = item["txt"];
                    dr["val"] = item["val"];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        /// <summary>
        /// 获取简单引用的Json数据
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        private string GetSimpeRefJsonData(Dictionary<string, object> jsonDataDic, string dataPath)
        {
            try
            {
                if (dataPath.EndsWith("dataArray"))
                {
                    dataPath = dataPath.Remove(dataPath.LastIndexOf("dataArray") - 1);
                }
                string jsonData = jsonDataDic["dataArray"].ToString();
                List<Dictionary<string, object>> valueList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonData);
                string[] pathList = dataPath.Split('.');
                if (pathList != null)
                {
                    for (int i = 0; i < pathList.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            foreach (Dictionary<string, object> item in valueList)
                            {
                                if (item.ContainsKey(pathList[i]))
                                {
                                    jsonData = item[pathList[i]].ToString();
                                }
                            }
                            if (i == pathList.Length - 1)
                            {
                                return jsonData;
                            }
                            jsonData = JsonHelper.ReadJsonString(jsonData, "dataArray");
                        }
                        else if (i % 2 == 1)
                        {
                            Dictionary<string, string> pathDic = JsonHelper.ToObject<Dictionary<string, string>>(pathList[i].ToString());
                            if (pathDic != null)
                            {
                                List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonData);
                                if (arrayList != null)
                                {
                                    for (int j = 0; j < arrayList.Count; j++)
                                    {
                                        if (arrayList[j].ContainsKey(pathDic["key"]) && arrayList[j][pathDic["key"]].ToString().Equals(pathDic["value"]))
                                        {
                                            jsonData = JsonHelper.ToJson(arrayList[j]);
                                            if (i == pathList.Length - 1)
                                            {
                                                return jsonData;
                                            }
                                            else
                                            {
                                                valueList.Clear();
                                                valueList.Add(JsonHelper.ToObject<Dictionary<string, object>>(jsonData));
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return jsonData;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
