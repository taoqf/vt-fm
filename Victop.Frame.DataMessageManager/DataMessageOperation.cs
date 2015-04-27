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
using Victop.Frame.DataMessageManager.Enums;
using Victop.Frame.DataMessageManager.Models;
using Victop.Frame.DataMessageManager.StaticClass;
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
        public virtual bool SaveData(string viewId, string dataPath)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.SaveData(viewId, dataPath);
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
        /// 获取简单引用数据
        /// </summary>
        /// <param name="viewId">通道标识</param>
        /// <param name="dataPath">路径</param>
        /// <param name="columnName">列名</param>
        /// <param name="target">目标列</param>
        /// <param name="targetValue">目标值</param>
        /// <param name="dependDic">依赖值</param>
        /// <returns></returns>
        public virtual DataSet GetSimpDefData(string viewId, string dataPath, string columnName, string target, string targetValue, Dictionary<string, object> dependDic = null)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.GetSimpDefData(viewId, dataPath, columnName, target, targetValue, dependDic);
        }
        /// <summary>
        /// 获取简单引用数据
        /// </summary>
        /// <param name="viewId">通道标识</param>
        /// <param name="dataPath">路径</param>
        /// <param name="columnName">列名</param>
        /// <param name="dependDic">依赖值</param>
        /// <returns></returns>
        public virtual DataSet GetSimpDefData(string viewId, string dataPath, string columnName, Dictionary<string, object> dependDic = null)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.GetSimpDefData(viewId, dataPath, columnName, dependDic);
        }
        /// <summary>
        /// 释放数据
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public virtual bool DisposeData(string viewId)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.DisposeData(viewId);
        }
        /// <summary>
        /// 获取模型关系
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        public virtual string GetModelRelation(string viewId)
        {
            DataOperation dataOp = new DataOperation();
            return dataOp.GetModelRelation(viewId);
        }
        /// <summary>
        /// 同步消息发送
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="messageContent">消息体</param>
        /// <param name="dataForm">数据格式(JSON/DATASET)</param>
        /// <param name="waiteTime">同步等待时间</param>
        /// <returns>应答消息内容</returns>
        public Dictionary<string, object> SendSyncMessage(string messageType, Dictionary<string, object> messageContent, string dataForm = "JSON", int waiteTime = 15)
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
        public void SendAsyncMessage(string messageType, Dictionary<string, object> messageContent, WaitCallback replyCallBack = null, string dataForm = "JSON", long validTime = 15)
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
        /// <param name="defaultCondition">默认条件</param>
        /// <param name="systemId">系统Id</param>
        /// <param name="configsystemId">配置系统Id</param>
        /// <param name="foreRunnerFlag">前导标签</param>
        /// <param name="refsystemId">引用系统Id</param>
        /// <returns>引用消息</returns>
        public string GetRefData(string viewId, string dataPath, string fieldName, string rowValue, out DataSet RefDataSet, List<Dictionary<string, object>> defaultCondition = null, string systemId = null, string configsystemId = null, bool foreRunnerFlag = true, string refsystemId = null)
        {
            RefDataSet = new DataSet();
            bool wideRefFlag = true;
            string resultMessage = string.Empty;
            DataStoreInfo storeInfo = null;
            string RebuildPath = string.Empty;
            string wideFieldFullName = string.Empty;
            string narrowFieldFullName = string.Empty;
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
                wideFieldFullName = RebuildPath + fieldName;
                narrowFieldFullName = RebuildPath.Substring(0, RebuildPath.LastIndexOf("dataArray")) + string.Format("[target:{0}].target_value", fieldName);
                #endregion
                RefRelationInfo relationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(wideFieldFullName) && it.RowId.Equals(rowValue));
                if (relationInfo == null)
                {
                    wideRefFlag = false;
                    relationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(narrowFieldFullName) && it.RowId.Equals(rowValue));
                }
                if (relationInfo != null && defaultCondition == null)
                {
                    GetExistRefData(rowValue, ref RefDataSet, ref wideRefFlag, ref resultMessage, storeInfo, wideFieldFullName, narrowFieldFullName, dataOp, channelData, relationInfo);
                }
                else
                {
                    bool simpleRefFlag = false;
                    GetNoExistSimpleRefData(rowValue, RefDataSet, ref wideRefFlag, ref resultMessage, storeInfo, wideFieldFullName, narrowFieldFullName, channelData, ref relationInfo, ref simpleRefFlag);
                    if (!simpleRefFlag)//当简单引用判断为不存在是，判定是否有标准引用
                    {
                        MongoModelInfoOfClientRefModel clientRefModel = channelData.ModelDefInfo.ModelClientRef.FirstOrDefault(it => it.ClientRefField.Equals(wideFieldFullName));
                        if (clientRefModel == null)
                        {
                            wideRefFlag = false;
                            clientRefModel = channelData.ModelDefInfo.ModelClientRef.FirstOrDefault(it => it.ClientRefField.Equals(narrowFieldFullName));
                        }
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
                                    contentDic.Add("refsystemid", string.IsNullOrEmpty(refsystemId) ? systemId : refsystemId);
                                    contentDic.Add("modelid", clientRefModel.ClientRefModel);
                                    if (clientRefModel.ClientRefConditionFirst.Count > 0)
                                    {
                                        refTableName = string.IsNullOrEmpty(clientRefModel.ClientRefTableName) ? clientRefModel.ClientRefConditionFirst[0].ConditionRight.Substring(0, clientRefModel.ClientRefConditionFirst[0].ConditionRight.IndexOf('.')) : clientRefModel.ClientRefTableName;
                                        List<Dictionary<string, object>> conditionList = defaultCondition == null ? new List<Dictionary<string, object>>() : defaultCondition;
                                        DataRow[] drs = storeInfo.ActualDataInfo.Tables["dataArray"].Select(string.Format("_id='{0}'", rowValue));
                                        if (drs != null && drs.Count() > 0)
                                        {

                                            Dictionary<string, object> tableDic = conditionList.FirstOrDefault(it => it["name"].Equals(refTableName)) == null ? new Dictionary<string, object>() : conditionList.FirstOrDefault(it => it["name"].Equals(refTableName));
                                            if (!tableDic.ContainsKey("name"))
                                            {
                                                tableDic.Add("name", refTableName);
                                            }
                                            List<Dictionary<string, object>> tableconditionList = tableDic.ContainsKey("tablecondition") ? JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ToJson(tableDic["tablecondition"])) : new List<Dictionary<string, object>>();
                                            foreach (MongoModelInfoOfClientRefConditionModel item in clientRefModel.ClientRefConditionFirst)
                                            {
                                                string refRightField = item.ConditionRight.Substring(item.ConditionRight.LastIndexOf('.') + 1);
                                                string refLeftField = item.ConditionLeft.Substring(item.ConditionLeft.LastIndexOf('.') + 1);
                                                if (item.ConditionDependFlag == 1)
                                                {
                                                    //TODO:处理依赖关系
                                                    if (drs[0].Table.Columns.Contains(refLeftField) && !string.IsNullOrEmpty(drs[0][refLeftField].ToString()))
                                                    {
                                                        if (tableconditionList.FirstOrDefault(it => it.ContainsKey(refLeftField)) != null)
                                                        {
                                                            tableconditionList.FirstOrDefault(it => it.ContainsKey(refLeftField))[refLeftField] = drs[0][refLeftField];
                                                        }
                                                        else
                                                        {
                                                            Dictionary<string, object> itemDic = new Dictionary<string, object>();
                                                            itemDic.Add(refRightField, drs[0][refLeftField]);
                                                            tableconditionList.Add(itemDic);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "引用依赖字段" + refLeftField + "无值" };
                                                        resultMessage = JsonHelper.ToJson(replyMessage);
                                                        return resultMessage;
                                                    }
                                                }
                                                else
                                                {
                                                    if (tableconditionList.FirstOrDefault(it => it.ContainsKey(refRightField)) != null)
                                                    {
                                                        if (!clientRefModel.ClientRefField.Equals(item.ConditionLeft))
                                                        {
                                                            tableconditionList.FirstOrDefault(it => it.ContainsKey(refRightField))[refRightField] = drs[0][refLeftField];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Dictionary<string, object> itemDic = new Dictionary<string, object>();
                                                        itemDic.Add(refRightField, drs[0][refLeftField]);
                                                        tableconditionList.Add(itemDic);
                                                    }
                                                }
                                            }
                                            if (tableDic.ContainsKey("tablecondition"))
                                            {
                                                tableDic["tablecondition"] = tableconditionList;
                                            }
                                            else
                                            {
                                                tableDic.Add("tablecondition", tableconditionList);
                                            }
                                            if (defaultCondition == null)
                                            {
                                                conditionList.Add(tableDic);
                                            }
                                            contentDic.Add("conditions", conditionList);
                                        }
                                        else
                                        {
                                            ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.CAST, ReplyAlertMessage = "数据不存在" };
                                            resultMessage = JsonHelper.ToJson(replyMessage);
                                            return resultMessage;
                                        }
                                    }
                                    //TODO:去取数
                                    if (string.IsNullOrEmpty(refTableName))
                                    {
                                        refTableName = string.IsNullOrEmpty(clientRefModel.ClientRefTableName) ? clientRefModel.ClientRefProperty[0].PropertyValue.Substring(0, clientRefModel.ClientRefProperty[0].PropertyValue.IndexOf('.')) : clientRefModel.ClientRefTableName;
                                    }
                                    if (defaultCondition != null && !contentDic.ContainsKey("conditions"))
                                    {
                                        contentDic.Add("conditions", defaultCondition);
                                    }
                                    Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic, "JSON");
                                    if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
                                    {
                                        relationInfo.DataChannelId = returnDic["DataChannelId"].ToString();
                                        relationInfo.TriggerTable = refTableName;
                                        RefDataSet = dataOp.GetData(returnDic["DataChannelId"].ToString(), string.Format("[\"{0}\"]", refTableName));
                                        if (RefDataSet != null && RefDataSet.Tables.Count > 0 && RefDataSet.Tables.Contains("dataArray") && RefDataSet.Tables["dataArray"].Columns.Count > 0 && clientRefModel.ClientRefPopupSetting.SettingColumns.Count > 0)
                                        {
                                            foreach (DataColumn item in RefDataSet.Tables["dataArray"].Columns)
                                            {
                                                MongoModelInfoOfClientRefPopupSettingColumnsModel columnModel = clientRefModel.ClientRefPopupSetting.SettingColumns.FirstOrDefault(it => it.ColumnField.Equals(item.ColumnName));
                                                if (columnModel != null && !string.IsNullOrEmpty(columnModel.ColumnLabel))
                                                {
                                                    item.Caption = columnModel.ColumnLabel;
                                                }
                                                else
                                                {
                                                    item.ExtendedProperties.Add("Visible", "false");
                                                }
                                            }
                                        }
                                        ReplyMessage replyMessage = new ReplyMessage() { ReplyMode = ReplyModeEnum.ASYNC, ReplyContent = JsonHelper.ToJson(clientRefModel) };
                                        resultMessage = JsonHelper.ToJson(replyMessage);
                                        if (storeInfo.RefDataInfo.FirstOrDefault(it => it.RowId.Equals(rowValue) && it.TriggerField.Equals(clientRefModel.ClientRefField)) == null)
                                        {
                                            storeInfo.RefDataInfo.Add(relationInfo);
                                        }
                                        else
                                        {
                                            storeInfo.RefDataInfo.FirstOrDefault(it => it.RowId.Equals(rowValue) && it.TriggerField.Equals(clientRefModel.ClientRefField)).DataChannelId = returnDic["DataChannelId"].ToString();
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
        /// 获取映射表中不存在的数据引用数据
        /// </summary>
        /// <param name="rowValue">行值</param>
        /// <param name="RefDataSet">引用数据集合</param>
        /// <param name="wideRefFlag">宽表引用标识</param>
        /// <param name="resultMessage">返回消息</param>
        /// <param name="storeInfo">数据集存储信息</param>
        /// <param name="wideFieldFullName">宽表字符全名</param>
        /// <param name="narrowFieldFullName">窄表字符全名</param>
        /// <param name="dataOp">数据操作对象</param>
        /// <param name="channelData">数据集合</param>
        /// <param name="relationInfo">关系信息</param>
        /// <param name="simpleRefFlag">简单引用标识</param>
        private void GetNoExistSimpleRefData(string rowValue, DataSet RefDataSet, ref bool wideRefFlag, ref string resultMessage, DataStoreInfo storeInfo, string wideFieldFullName, string narrowFieldFullName, ChannelData channelData, ref RefRelationInfo relationInfo, ref bool simpleRefFlag)
        {
            foreach (MongoSimpleRefInfoOfArrayModel item in channelData.SimpleRefInfo.SimpleDataArray)
            {
                MongoSimpleRefInfoOfArrayPropertyModel propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(wideFieldFullName));
                if (propertyModel == null)
                {
                    wideRefFlag = false;
                    propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(narrowFieldFullName));
                }
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
                        //TODO:relationInfo的DependId赋值
                        Dictionary<string, object> dependDic = new Dictionary<string, object>();
                        if (GetSimpleRefDependDic(storeInfo, relationInfo, propertyModel.PropertyDepend, rowValue, dependDic))
                        {
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
                    if (storeInfo.RefDataInfo.FirstOrDefault(it => it.RowId.Equals(rowValue) && it.TriggerField.Equals(propertyModel.PropertyKey)) == null)
                    {
                        storeInfo.RefDataInfo.Add(relationInfo);
                    }
                    simpleRefFlag = true;
                    break;
                }
            }
        }
        /// <summary>
        /// 获取存在的数据引用
        /// </summary>
        /// <param name="rowValue">行值</param>
        /// <param name="RefDataSet">引用数据集合</param>
        /// <param name="wideRefFlag">宽表引用标识</param>
        /// <param name="resultMessage">返回消息</param>
        /// <param name="storeInfo">数据集存储信息</param>
        /// <param name="wideFieldFullName">宽表字符全名</param>
        /// <param name="narrowFieldFullName">窄表字符全名</param>
        /// <param name="dataOp">数据操作对象</param>
        /// <param name="channelData">数据集合</param>
        /// <param name="relationInfo">关系信息</param>
        private void GetExistRefData(string rowValue, ref DataSet RefDataSet, ref bool wideRefFlag, ref string resultMessage, DataStoreInfo storeInfo, string wideFieldFullName, string narrowFieldFullName, DataOperation dataOp, ChannelData channelData, RefRelationInfo relationInfo)
        {
            switch (relationInfo.RefType)
            {
                case RefTypeEnum.SIMPLEREF:
                    foreach (MongoSimpleRefInfoOfArrayModel item in channelData.SimpleRefInfo.SimpleDataArray)
                    {
                        MongoSimpleRefInfoOfArrayPropertyModel propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(wideFieldFullName));
                        if (propertyModel == null)
                        {
                            wideRefFlag = false;
                            propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(narrowFieldFullName));
                        }
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
                                Dictionary<string, object> dependDic = new Dictionary<string, object>();
                                if (GetSimpleRefDependDic(storeInfo, relationInfo, propertyModel.PropertyDepend, rowValue, dependDic))
                                {
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
                    if (RefDataSet != null && RefDataSet.Tables.Count > 0 && RefDataSet.Tables.Contains("dataArray") && RefDataSet.Tables["dataArray"].Columns.Count > 0 && clientRefModel.ClientRefPopupSetting.SettingColumns.Count > 0)
                    {
                        foreach (DataColumn item in RefDataSet.Tables["dataArray"].Columns)
                        {
                            MongoModelInfoOfClientRefPopupSettingColumnsModel columnModel = clientRefModel.ClientRefPopupSetting.SettingColumns.FirstOrDefault(it => it.ColumnField.Equals(item.ColumnName));
                            if (columnModel != null && !string.IsNullOrEmpty(columnModel.ColumnLabel))
                            {
                                item.Caption = columnModel.ColumnLabel;
                            }
                            else
                            {
                                item.ExtendedProperties.Add("Visible", "false");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private bool GetSimpleRefDependDic(DataStoreInfo datastoreInfo, RefRelationInfo relationInfo, string propertyDepend, string rowValue, Dictionary<string, object> dependDic)
        {
            if (dependDic == null)
                dependDic = new Dictionary<string, object>();
            RefRelationInfo dependRelationInfo = datastoreInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(propertyDepend) && it.RowId.Equals(rowValue));
            if (dependRelationInfo != null && dependRelationInfo.FieldValue != null)
            {
                bool result = true;
                if (dependRelationInfo.DependId != null)
                {
                    RefRelationInfo dependInfo = datastoreInfo.RefDataInfo.Find(it => it.Id.Equals(dependRelationInfo.DependId));
                    result = GetSimpleRefDependDic(datastoreInfo, dependRelationInfo, dependInfo.TriggerField, rowValue, dependDic);
                }
                else
                {
                    relationInfo.DependId = dependRelationInfo.Id;
                }
                dependDic.Add(dependRelationInfo.TriggerField, dependRelationInfo.FieldValue);
                return result;
            }
            else
            {
                return false;
            }
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
            string wideFieldFullName = string.Empty;
            string narrowFieldFullName = string.Empty;
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
                wideFieldFullName = RebuildPath + fieldName;
                narrowFieldFullName = RebuildPath.Substring(0, RebuildPath.LastIndexOf("dataArray")) + string.Format("[target:{0}].target_value", fieldName);
                #endregion
                RefRelationInfo relationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(wideFieldFullName) && it.RowId.Equals(rowValue));
                if (relationInfo == null)
                {
                    relationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(narrowFieldFullName) && it.RowId.Equals(rowValue));
                }
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
        /// <param name="pluginName">插件名称</param>
        /// <param name="paramDic">参数键值对</param>
        /// <param name="waitTime">同步等待时间(秒)</param>
        /// <returns></returns>
        public PluginModel StratPlugin(string pluginName, Dictionary<string, object> paramDic = null,string showTitle = null)
        {
            PluginModel pluginModel = new PluginModel();
            MessageOperation messageOp = new MessageOperation();
            string messageType = "PluginService.PluginRun";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("PluginName", pluginName);
            contentDic.Add("ShowTitle", showTitle);
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

        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <typeparam name="T">字段数据类型</typeparam>
        /// <param name="viewId">通道标识</param>
        /// <param name="dataPath">数据路径</param>
        /// <param name="rowKey">行主键值</param>
        /// <param name="fieldPath">字段路径</param>
        /// <returns></returns>
        public virtual T GetFieldValue<T>(string viewId, string dataPath, string rowKey, string fieldPath)
        {
            try
            {
                DataStoreInfo storeInfo = new DataStoreInfo();
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
                    RefRelationInfo relationInfo = storeInfo.RefDataInfo.FirstOrDefault(it => it.TriggerField.Equals(fieldPath) && it.RowId.Equals(rowKey));
                    if (relationInfo != null)
                    {
                        return (T)relationInfo.FieldValue;
                    }
                    else
                    {
                        return default(T);
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("获取默认值出错：{0}", ex.Message);
                return default(T);
            }
        }

        /// <summary>
        /// 发送数据锁定消息
        /// </summary>
        /// <param name="lockInfo"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> SendDataLockMessage(LockInfoModel lockInfo)
        {
            MessageOperation messageOp = new MessageOperation();
            string messageType = "MongoDataChannelService.dataLock";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("_id", lockInfo.Id);
            contentDic.Add("tablename", lockInfo.TableName);
            contentDic.Add("locktimeout", lockInfo.LockTimeOut);
            contentDic.Add("systemid", lockInfo.SystemId);
            contentDic.Add("configsystemid", lockInfo.ConfigSystemId);
            contentDic.Add("spaceId", lockInfo.SpaceId);
            contentDic.Add("userCode", lockInfo.UserCode);
            contentDic.Add("operflag", (int)lockInfo.OpenFlag);
            Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic, "JSON");
            if (returnDic != null && returnDic["ReplyMode"].ToString() != "0")
            {
                switch (lockInfo.OpenFlag)
                {
                    case LockStatusEnum.锁定:
                        DataLockInfoClass.LockInfoList.Add(lockInfo);
                        break;
                    case LockStatusEnum.解锁:
                        LockInfoModel unLockInfo = DataLockInfoClass.LockInfoList.FirstOrDefault(it => it.Id.Equals(lockInfo.Id));
                        if (unLockInfo != null)
                        {
                            DataLockInfoClass.LockInfoList.Remove(unLockInfo);
                        }
                        break;
                    default:
                        break;
                }
            }
            return returnDic;
        }

        public virtual string GetCurdListData(string viewId)
        {
            DataOperation dataOp = new DataOperation();
            return JsonHelper.ToJson(dataOp.GetCurdJSONData(viewId));
        }

        /// <summary>
        /// 移除数据锁定
        /// </summary>
        public virtual void RemoveDataLock()
        {
            foreach (LockInfoModel item in DataLockInfoClass.LockInfoList)
            {
                string messageType = "MongoDataChannelService.dataLock";
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("_id", item.Id);
                contentDic.Add("tablename", item.TableName);
                contentDic.Add("locktimeout", item.LockTimeOut);
                contentDic.Add("systemid", item.SystemId);
                contentDic.Add("configsystemid", item.ConfigSystemId);
                contentDic.Add("spaceId", item.SpaceId);
                contentDic.Add("userCode", item.UserCode);
                contentDic.Add("operflag", LockStatusEnum.解锁);
                SendSyncMessage(messageType, contentDic);
            }
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
