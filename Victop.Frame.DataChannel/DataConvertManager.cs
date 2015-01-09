using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Victop.Frame.DataChannel.Enums;
using Victop.Frame.DataChannel.MongoModel;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.DataChannel
{
    /// <summary>
    /// 数据转换器
    /// </summary>
    public class DataConvertManager
    {
        private static Hashtable jsonTableMap;

        internal static Hashtable JsonTableMap
        {
            get
            {
                if (jsonTableMap == null)
                    jsonTableMap = new Hashtable();
                return jsonTableMap;
            }
            set
            {
                jsonTableMap = value;
            }
        }
        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <param name="structDs"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string viewId, string dataPath, DataSet structDs = null)
        {
            if (string.IsNullOrEmpty(dataPath))
                return null;
            DataSet newDs = structDs == null ? new DataSet() : structDs.Copy();
            newDs.Clear();
            List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
            #region 获取模型定义及简单引用
            DataChannelManager dataChannelManager = new DataChannelManager();
            Hashtable hashData = dataChannelManager.GetData(viewId);
            ChannelData channelData = hashData["Data"] as ChannelData;
            MongoModelInfoModel modelDefInfo = channelData.ModelDefInfo;
            MongoSimpleRefInfoModel simpleRefInfo = channelData.SimpleRefInfo;
            #endregion
            object jsonData = DataTool.GetDataObjectByPath(viewId, dataPath);
            if (jsonData != null)
            {
                Dictionary<string, object> jsonDic = (Dictionary<string, object>)jsonData;
                if (pathList.Count % 2 == 1)//获取表数据
                {
                    #region 组织表数据
                    OriginzeDataOfTable(newDs, pathList, modelDefInfo, simpleRefInfo, jsonDic, viewId, dataPath);
                    #endregion
                }
                else//获取行数据
                {
                    #region 组织行数据
                    OriginzieDataOfRow(newDs, pathList, modelDefInfo, simpleRefInfo, jsonDic, viewId);
                    #endregion
                }
            }
            else
            {
                DataTable itemDt = new DataTable("dataArray");
                itemDt = GetDataTableStructByModel(modelDefInfo, simpleRefInfo, pathList[pathList.Count - 1].GetType().Name.Equals("String") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString(), viewId, dataPath);
                itemDt.AcceptChanges();
                if (!newDs.Tables.Contains("dataArray"))
                {
                    newDs.Tables.Add(itemDt);
                }
            }
            bool checkFlag = false;
            foreach (JsonMapKey item in JsonTableMap.Keys)
            {
                if (item.ViewId == viewId && item.DataPath == dataPath)
                {
                    JsonTableMap[item] = newDs;
                    checkFlag = true;
                    break;
                }
            }
            if (!checkFlag)
            {
                JsonMapKey mapKey = new JsonMapKey() { ViewId = viewId, DataPath = dataPath };
                JsonTableMap.Add(mapKey, newDs);
            }
            return newDs;
        }

        /// <summary>
        /// 获取Json数据
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public string GetJsonData(string viewId, string dataPath)
        {
            return DataTool.GetDataByPath(viewId, dataPath);
        }

        private static void UpdateDataTableRow(DataTable itemDt, Dictionary<string, object> rowItem)
        {
            if (rowItem != null && rowItem.ContainsKey("_id") && string.IsNullOrEmpty(rowItem["_id"].ToString()))
            {
                return;
            }
            DataRow arrayDr = itemDt.NewRow();
            foreach (DataColumn dtCol in itemDt.Columns)
            {
                if (rowItem.ContainsKey(dtCol.ColumnName))
                {
                    if (rowItem[dtCol.ColumnName] != null)
                    {
                        if (!dtCol.ExtendedProperties.ContainsKey("ColType"))
                        {
                            dtCol.ExtendedProperties.Add("ColType", rowItem[dtCol.ColumnName].GetType().Name);
                        }
                        if (dtCol.DataType == typeof(DateTime) && dtCol.ExtendedProperties["ColType"] != null)
                        {
                            switch (dtCol.ExtendedProperties["ColType"].ToString())
                            {
                                case "timestamp":
                                    if (rowItem[dtCol.ColumnName].ToString().Equals("0"))
                                    {
                                        arrayDr[dtCol.ColumnName] = DBNull.Value;
                                    }
                                    else
                                    {
                                        DateTime dt = new DateTime(1970, 1, 1);
                                        dt = dt.AddMilliseconds(Convert.ToInt64(rowItem[dtCol.ColumnName].ToString()));
                                        arrayDr[dtCol.ColumnName] = dt;
                                    }
                                    break;
                                case "date":
                                default:
                                    if (string.IsNullOrEmpty(rowItem[dtCol.ColumnName].ToString()))
                                    {
                                        arrayDr[dtCol.ColumnName] = DBNull.Value;
                                    }
                                    else
                                    {
                                        arrayDr[dtCol.ColumnName] = rowItem[dtCol.ColumnName];
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            arrayDr[dtCol.ColumnName] = rowItem[dtCol.ColumnName];
                        }
                    }
                    else
                    {
                        if (!dtCol.ExtendedProperties.ContainsKey("ColType"))
                        {
                            dtCol.ExtendedProperties.Add("ColType", "string");
                        }
                        arrayDr[dtCol.ColumnName] = DBNull.Value;
                    }
                }
                else
                {
                    arrayDr[dtCol.ColumnName] = DBNull.Value;
                }
            }
            itemDt.Rows.Add(arrayDr);
        }

        /// <summary>
        /// 组织行数据
        /// </summary>
        /// <param name="newDs"></param>
        /// <param name="pathList"></param>
        /// <param name="modelData"></param>
        /// <param name="jsonDic"></param>
        private void OriginzieDataOfRow(DataSet newDs, List<object> pathList, MongoModelInfoModel modelData, MongoSimpleRefInfoModel simpleRefData, Dictionary<string, object> jsonDic, string viewId)
        {
            DataTable itemDt = new DataTable("dataArray");
            if (modelData != null)
            {
                itemDt = GetDataTableStructByModel(modelData, simpleRefData, pathList[pathList.Count - 1].GetType().Name.Equals("String") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString(), viewId, JsonHelper.ToJson(pathList));
            }
            if (itemDt.Columns.Count <= 0)
            {
                itemDt = GetDataTableStruct("dataArray", jsonDic, newDs);
            }
            itemDt.AcceptChanges();
            if (!newDs.Tables.Contains("dataArray"))
            {
                newDs.Tables.Add(itemDt);
            }
            DataRow objectDr = itemDt.NewRow();
            foreach (DataColumn dtCol in itemDt.Columns)
            {
                if (jsonDic.ContainsKey(dtCol.ColumnName))
                {
                    if (jsonDic[dtCol.ColumnName] == null)
                    {
                        objectDr[dtCol.ColumnName] = DBNull.Value;
                    }
                    else if (dtCol.DataType == typeof(DateTime) && dtCol.ExtendedProperties["ColType"] != null)
                    {
                        switch (dtCol.ExtendedProperties["ColType"].ToString())
                        {
                            case "timestamp":
                                if (Convert.ToInt64(jsonDic[dtCol.ColumnName].ToString()) == 0)
                                {
                                    objectDr[dtCol.ColumnName] = DBNull.Value;
                                }
                                else
                                {
                                    DateTime dt = new DateTime(1970, 1, 1);
                                    dt = dt.AddMilliseconds(Convert.ToInt64(jsonDic[dtCol.ColumnName].ToString()));
                                    objectDr[dtCol.ColumnName] = dt;
                                }
                                break;
                            case "date":
                            default:
                                if (string.IsNullOrEmpty(jsonDic[dtCol.ColumnName].ToString()))
                                {
                                    objectDr[dtCol.ColumnName] = DBNull.Value;
                                }
                                else
                                {
                                    objectDr[dtCol.ColumnName] = jsonDic[dtCol.ColumnName];
                                }
                                break;
                        }
                    }
                    else
                    {
                        objectDr[dtCol.ColumnName] = jsonDic[dtCol.ColumnName];
                    }
                }
            }
            itemDt.Rows.Add(objectDr);
            itemDt.AcceptChanges();

            if (newDs.Tables.Contains(itemDt.TableName))
            {
                newDs.Tables.Remove("dataArray");
            }
            newDs.Tables.Add(itemDt);
        }
        /// <summary>
        /// 构建表的数据
        /// </summary>
        /// <param name="newDs"></param>
        /// <param name="pathList"></param>
        /// <param name="modelData"></param>
        /// <param name="jsonDic"></param>
        private void OriginzeDataOfTable(DataSet newDs, List<object> pathList, MongoModelInfoModel modelData, MongoSimpleRefInfoModel simpleRefData, Dictionary<string, object> jsonDic, string viewId, string dataPath)
        {
            foreach (string item in jsonDic.Keys)
            {
                List<string> delColList = new List<string>();
                DataTable itemDt = new DataTable();
                switch (jsonDic[item].GetType().Name)
                {
                    case "JArray":
                    case "Object[]":
                        List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ToJson(jsonDic[item]));
                        itemDt = new DataTable(item);
                        if (modelData != null && item.Equals("dataArray"))
                        {
                            itemDt = GetDataTableStructByModel(modelData, simpleRefData, pathList[pathList.Count - 1].GetType().Name.Equals("String") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString(), viewId, dataPath);
                        }
                        if (itemDt.Columns.Count <= 0)
                        {
                            itemDt = GetDataTableStruct(item, arrayList.Count > 0 ? arrayList[0] : null, newDs);
                        }
                        itemDt.AcceptChanges();
                        if (!newDs.Tables.Contains(item))
                        {
                            newDs.Tables.Add(itemDt);
                        }
                        foreach (Dictionary<string, object> rowItem in arrayList)
                        {
                            DataRow arrayDr = itemDt.NewRow();
                            foreach (DataColumn dtCol in itemDt.Columns)
                            {
                                if (rowItem.ContainsKey(dtCol.ColumnName))
                                {
                                    if (rowItem[dtCol.ColumnName] == null)
                                    {
                                        arrayDr[dtCol.ColumnName] = DBNull.Value;
                                    }
                                    else if (dtCol.DataType == typeof(DateTime) && dtCol.ExtendedProperties["ColType"] != null)
                                    {
                                        switch (dtCol.ExtendedProperties["ColType"].ToString())
                                        {
                                            case "timestamp":
                                                if (Convert.ToInt64(rowItem[dtCol.ColumnName].ToString()) == 0)
                                                {
                                                    arrayDr[dtCol.ColumnName] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    DateTime dt = new DateTime(1970, 1, 1);
                                                    dt = dt.AddMilliseconds(Convert.ToInt64(rowItem[dtCol.ColumnName].ToString()));
                                                    arrayDr[dtCol.ColumnName] = dt;
                                                }
                                                break;
                                            case "date":
                                            default:
                                                if (string.IsNullOrEmpty(jsonDic[dtCol.ColumnName].ToString()))
                                                {
                                                    arrayDr[dtCol.ColumnName] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    arrayDr[dtCol.ColumnName] = rowItem[dtCol.ColumnName];
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        arrayDr[dtCol.ColumnName] = rowItem[dtCol.ColumnName];
                                    }
                                }
                            }
                            itemDt.Rows.Add(arrayDr);
                        }
                        break;
                    case "JObject":
                    case "Object":
                        Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonDic[item].ToString());
                        if (modelData != null && item.Equals("dataArray"))
                        {
                            itemDt = GetDataTableStructByModel(modelData, simpleRefData, pathList[pathList.Count - 1].GetType().Name.Equals("string") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString(), viewId, dataPath);
                        }
                        if (itemDt.Columns.Count <= 0)
                        {
                            itemDt = GetDataTableStruct(item, itemDic != null ? itemDic : null, newDs);
                        }
                        itemDt.AcceptChanges();
                        if (!newDs.Tables.Contains(item))
                        {
                            newDs.Tables.Add(itemDt);
                        }
                        DataRow objectDr = itemDt.NewRow();
                        foreach (DataColumn dtCol in itemDt.Columns)
                        {
                            if (itemDic.ContainsKey(dtCol.ColumnName))
                            {
                                if (itemDic[dtCol.ColumnName] != null && !itemDic[dtCol.ColumnName].GetType().Name.Equals("String"))
                                {
                                    if (!dtCol.ExtendedProperties.ContainsKey("ColType"))
                                    {
                                        dtCol.ExtendedProperties.Add("ColType", itemDic[dtCol.ColumnName].GetType().Name);
                                    }
                                }
                                else
                                {
                                    if (!dtCol.ExtendedProperties.ContainsKey("ColType"))
                                    {
                                        dtCol.ExtendedProperties.Add("ColType", "string");
                                    }
                                }
                                if (itemDic[dtCol.ColumnName] == null)
                                {
                                    objectDr[dtCol.ColumnName] = DBNull.Value;
                                }
                                else if (dtCol.DataType == typeof(DateTime) && dtCol.ExtendedProperties["ColType"] != null)
                                {
                                    switch (dtCol.ExtendedProperties["ColType"].ToString())
                                    {
                                        case "timestamp":
                                            if (Convert.ToInt64(itemDic[dtCol.ColumnName].ToString()) == 0)
                                            {
                                                objectDr[dtCol.ColumnName] = DBNull.Value;
                                            }
                                            else
                                            {
                                                DateTime dt = new DateTime(1970, 1, 1);
                                                dt = dt.AddMilliseconds(Convert.ToInt64(itemDic[dtCol.ColumnName].ToString()));
                                                objectDr[dtCol.ColumnName] = dt;
                                            }
                                            break;
                                        case "date":
                                        default:
                                            if (string.IsNullOrEmpty(itemDic[dtCol.ColumnName].ToString()))
                                            {
                                                objectDr[dtCol.ColumnName] = DBNull.Value;
                                            }
                                            else
                                            {
                                                objectDr[dtCol.ColumnName] = itemDic[dtCol.ColumnName];
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    objectDr[dtCol.ColumnName] = itemDic[dtCol.ColumnName];
                                }
                            }
                        }
                        itemDt.Rows.Add(objectDr);
                        break;
                    default:
                        break;
                }
                itemDt.AcceptChanges();
                if (newDs.Tables.Contains(itemDt.TableName))
                {
                    newDs.Tables.Remove(itemDt.TableName);
                }
                newDs.Tables.Add(itemDt);
            }
        }

        /// <summary>
        /// 根据模型定义生成表结构
        /// </summary>
        /// <param name="modelJson"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private DataTable GetDataTableStructByModel(MongoModelInfoModel modelDefInfo, MongoSimpleRefInfoModel simpleRefInfo, string tableName, string viewId, string dataPath)
        {
            DataTable newDt = new DataTable("dataArray");
            if (modelDefInfo != null)
            {
                MongoModelInfoOfTablesModel modelInfoOfTableInfo = null;
                if (modelDefInfo.ModelTables != null && modelDefInfo.ModelTables.Count > 0)
                {
                    modelInfoOfTableInfo = modelDefInfo.ModelTables.FirstOrDefault(it => it.TableName.Equals(tableName));
                    if (modelInfoOfTableInfo != null)
                    {
                        BuildColumnsOfDataTable(tableName, newDt, modelInfoOfTableInfo.TableStructure, modelDefInfo, simpleRefInfo, viewId, dataPath);
                    }
                    else
                    {
                        foreach (MongoModelInfoOfTablesModel item in modelDefInfo.ModelTables)
                        {
                            BuildColumnsOfDataTable(tableName, newDt, item.TableStructure, modelDefInfo, simpleRefInfo, viewId, dataPath, false);
                        }
                    }

                }
            }
            return newDt;
        }
        /// <summary>
        /// 构建DataTable的列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="newDt"></param>
        /// <param name="tableList"></param>
        /// <param name="clientrefList"></param>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <param name="masterFlag"></param>
        private void BuildColumnsOfDataTable(string tableName, DataTable newDt, List<MongoModelInfoOfTableStructureModel> tableList, MongoModelInfoModel modelDefInfo, MongoSimpleRefInfoModel simpleRefList, string viewId, string dataPath, bool masterFlag = true)
        {
            List<object> dataPathList = JsonHelper.ToObject<List<object>>(dataPath);
            #region 组织table中字段
            foreach (MongoModelInfoOfTableStructureModel item in tableList)
            {
                string tablePath = string.Format("{0}.dataArray.", tableName);
                if (item.FieldKey.Contains(tablePath))
                {
                    int index = item.FieldKey.ToString().IndexOf(tablePath);
                    string tableFieldStr = item.FieldKey.ToString().Substring(index + tablePath.Length);
                    if (tableFieldStr.Contains("."))
                    {
                        continue;
                    }
                    int keyIndex = item.FieldKey.ToString().LastIndexOf('.');
                    string keyStr = item.FieldKey.ToString().Substring(keyIndex + 1);
                    if (item.FieldValue != null)
                    {
                        if (item.FieldValue.ValueSelectFlag == 1)
                        {
                            DataColumn dc = new DataColumn(keyStr);
                            if (modelDefInfo.ModelClientRef != null && modelDefInfo.ModelClientRef.Count > 0)
                            {
                                MongoModelInfoOfClientRefModel clientRefModel = modelDefInfo.ModelClientRef.FirstOrDefault(it => (it.ClientRefField.Equals(string.Format("{0}.dataArray.{1}", masterFlag ? tableName : dataPathList[0].ToString(), item.FieldKey))));
                                if (clientRefModel != null)
                                {
                                    dc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefModel));
                                }
                            }
                            switch (item.FieldValue.ValueType)
                            {
                                case "int":
                                    dc.DataType = typeof(Int32);
                                    dc.ExtendedProperties.Add("ColType", "int");
                                    break;
                                case "long":
                                    dc.DataType = typeof(Int64);
                                    dc.ExtendedProperties.Add("ColType", "long");
                                    break;
                                case "date":
                                    dc.DataType = typeof(DateTime);
                                    dc.ExtendedProperties.Add("ColType", "date");
                                    break;
                                case "timestamp":
                                    dc.DataType = typeof(DateTime);
                                    dc.ExtendedProperties.Add("ColType", "timestamp");
                                    break;
                                case "array":
                                    dc.DataType = typeof(String);
                                    dc.ExtendedProperties.Add("ColType", "array");
                                    break;
                                case "document":
                                    dc.DataType = typeof(String);
                                    dc.ExtendedProperties.Add("ColType", "document");
                                    break;
                                case "string":
                                default:
                                    dc.DataType = typeof(String);
                                    dc.ExtendedProperties.Add("ColType", "string");
                                    break;
                            }
                            if (!newDt.Columns.Contains(dc.ColumnName))
                            {
                                newDt.Columns.Add(dc);
                            }
                        }
                    }
                }
                else
                {
                    if (!masterFlag || item.FieldKey.Contains('.'))
                        continue;
                    if (item.FieldValue != null)
                    {
                        if (item.FieldValue.ValueSelectFlag == 1)
                        {
                            DataColumn dc = new DataColumn(item.FieldKey);
                            if (modelDefInfo.ModelClientRef != null && modelDefInfo.ModelClientRef.Count > 0)
                            {
                                MongoModelInfoOfClientRefModel clientRefModel = modelDefInfo.ModelClientRef.FirstOrDefault(it => (it.ClientRefField.Equals(string.Format("{0}.dataArray.{1}", masterFlag ? tableName : dataPathList[0].ToString(), item.FieldKey))));
                                if (clientRefModel != null)
                                {
                                    dc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefModel));
                                }
                            }
                            switch (item.FieldValue.ValueType)
                            {
                                case "int":
                                    dc.DataType = typeof(Int32);
                                    dc.ExtendedProperties.Add("ColType", "int");
                                    break;
                                case "long":
                                    dc.DataType = typeof(Int64);
                                    dc.ExtendedProperties.Add("ColType", "long");
                                    break;
                                case "date":
                                    dc.DataType = typeof(DateTime);
                                    dc.ExtendedProperties.Add("ColType", "date");
                                    break;
                                case "timestamp":
                                    dc.DataType = typeof(DateTime);
                                    dc.ExtendedProperties.Add("ColType", "timestamp");
                                    break;
                                case "array":
                                    dc.DataType = typeof(String);
                                    dc.ExtendedProperties.Add("ColType", "array");
                                    break;
                                case "document":
                                    dc.DataType = typeof(String);
                                    dc.ExtendedProperties.Add("ColType", "document");
                                    break;
                                case "string":
                                default:
                                    dc.DataType = typeof(String);
                                    dc.ExtendedProperties.Add("ColType", "string");
                                    break;
                            }
                            DataSet ds = GetSimpleRef(simpleRefList, dataPath, item.FieldKey, null);
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("dataArray"))
                            {
                                dc.ExtendedProperties.Add("ComboBox", ds.Tables["dataArray"]);
                            }
                            if (!newDt.Columns.Contains(dc.ColumnName))
                            {
                                newDt.Columns.Add(dc);
                            }
                        }
                    }
                }
            }
            #endregion
            #region 组织ref中字段
            if (modelDefInfo.ModelRef != null && modelDefInfo.ModelRef.Count > 0)
            {
                foreach (MongoModelInfoOfRefModel item in modelDefInfo.ModelRef)
                {
                    if (item.RefView != null)
                    {
                        foreach (MongoModelInfoOfRefConditionContentModel refitem in item.RefView)
                        {
                            if (refitem.ContentLeft.StartsWith(tableName))
                            {
                                string leftStr = refitem.ContentLeft.Substring(refitem.ContentLeft.IndexOf('.') + 1);
                                if (newDt.Columns.Contains(leftStr))
                                    continue;
                                DataColumn dc = new DataColumn(leftStr);
                                if (modelDefInfo.ModelClientRef != null && modelDefInfo.ModelClientRef.Count > 0)
                                {
                                    MongoModelInfoOfClientRefModel clientRefInfo = modelDefInfo.ModelClientRef.Find(it => (
                                    it.ClientRefField.Equals(string.Format("{0}.dataArray.{1}", masterFlag ? tableName : dataPathList[0].ToString(), leftStr))));
                                    if (clientRefInfo != null)
                                    {
                                        dc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefInfo));
                                    }
                                }
                                dc.ExtendedProperties.Add("ColType", "ref");
                                if (!string.IsNullOrEmpty(refitem.ContentType))
                                {
                                    switch (refitem.ContentType)
                                    {
                                        case "int":
                                            dc.DataType = typeof(Int32);
                                            break;
                                        case "long":
                                            dc.DataType = typeof(Int64);
                                            break;
                                        case "date":
                                            dc.DataType = typeof(DateTime);
                                            break;
                                        case "timestamp":
                                            dc.DataType = typeof(DateTime);
                                            break;
                                        case "string":
                                        default:
                                            dc.DataType = typeof(String);
                                            break;
                                    }
                                }
                                DataSet ds = GetSimpleRef(simpleRefList, dataPath, leftStr, null);
                                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("dataArray"))
                                {
                                    dc.ExtendedProperties.Add("ComboBox", ds.Tables["dataArray"]);
                                }
                                newDt.Columns.Add(dc);
                            }
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取简单引用数据
        /// </summary>
        /// <param name="viewId"></param>
        public DataSet GetSimpleRef(string viewId, string dataPath, string columnPath, string target, string targetValue, Dictionary<string, object> dependDic)
        {
            DataSet ds = new DataSet();
            string constructPath = string.Empty;
            #region 构建结构Path
            List<object> pathList = JsonHelper.ToObject<List<Object>>(dataPath);
            for (int i = 0; i < pathList.Count; i++)
            {
                if (pathList[i].GetType().Name.Equals("String"))
                {
                    if (i == pathList.Count - 1)
                    {
                        constructPath += pathList[i].ToString() + ".";
                    }
                    else
                    {
                        constructPath += pathList[i].ToString() + ".dataArray.";
                    }
                }
                else
                {
                    constructPath += "dataArray.";
                }
            }
            constructPath += "[" + target + ":" + columnPath + "]." + targetValue;
            #endregion
            #region 获取简单引用定义
            DataChannelManager dataChannelManager = new DataChannelManager();
            Hashtable hashData = dataChannelManager.GetData(viewId);
            ChannelData channelData = hashData["Data"] as ChannelData;
            MongoSimpleRefInfoModel simpleRefInfo = channelData.SimpleRefInfo;
            if (simpleRefInfo != null)
            {
                foreach (MongoSimpleRefInfoOfArrayModel item in simpleRefInfo.SimpleDataArray)
                {
                    MongoSimpleRefInfoOfArrayPropertyModel propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(constructPath));
                    if (propertyModel == null)
                        break;
                    DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, dependDic);
                    ds.Tables.Add(dt);
                    break;
                }
            }
            #endregion
            return ds;
        }

        /// <summary>
        /// 获取简单引用数据
        /// </summary>
        /// <param name="viewId"></param>
        public DataSet GetSimpleRef(string viewId, string dataPath, string columnPath, Dictionary<string, object> dependDic)
        {
            DataSet ds = new DataSet();
            string constructPath = string.Empty;
            #region 构建结构Path
            List<object> pathList = JsonHelper.ToObject<List<Object>>(dataPath);
            for (int i = 0; i < pathList.Count; i++)
            {
                if (pathList[i].GetType().Name.Equals("String"))
                {
                    constructPath += pathList[i].ToString() + ".";
                    if (i == pathList.Count - 1)
                    {
                        constructPath += "dataArray.";
                    }
                }
                else
                {
                    constructPath += "dataArray.";
                }
            }
            constructPath += columnPath;
            #endregion
            #region 获取简单引用定义
            DataChannelManager dataChannelManager = new DataChannelManager();
            Hashtable hashData = dataChannelManager.GetData(viewId);
            ChannelData channelData = hashData["Data"] as ChannelData;
            MongoSimpleRefInfoModel simpleRefInfo = channelData.SimpleRefInfo;
            if (simpleRefInfo!=null)
            {
                foreach (MongoSimpleRefInfoOfArrayModel item in simpleRefInfo.SimpleDataArray)
                {
                    MongoSimpleRefInfoOfArrayPropertyModel propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(constructPath));
                    if (propertyModel == null)
                        break;
                    DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, dependDic);
                    ds.Tables.Add(dt);
                    break;
                }
            }
            #endregion
            return ds;
        }

        /// <summary>
        /// 获取简单引用数据
        /// </summary>
        /// <param name="viewId"></param>
        private DataSet GetSimpleRef(MongoSimpleRefInfoModel simpleRefList, string dataPath, string columnPath, Dictionary<string, object> dependDic)
        {
            DataSet ds = new DataSet();
            string constructPath = string.Empty;
            #region 构建结构Path
            List<object> pathList = JsonHelper.ToObject<List<Object>>(dataPath);
            for (int i = 0; i < pathList.Count; i++)
            {
                if (pathList[i].GetType().Name.Equals("String"))
                {
                    constructPath += pathList[i].ToString() + ".";
                    if (i == pathList.Count - 1)
                    {
                        constructPath += "dataArray.";
                    }
                }
                else
                {
                    constructPath += "dataArray.";
                }
            }
            constructPath += columnPath;
            #endregion
            #region 获取简单引用定义

            if (simpleRefList != null)
            {
                foreach (MongoSimpleRefInfoOfArrayModel item in simpleRefList.SimpleDataArray)
                {
                    MongoSimpleRefInfoOfArrayPropertyModel propertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(constructPath));
                    if (propertyModel == null)
                        break;
                    DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, dependDic);
                    ds.Tables.Add(dt);
                    break;
                }
            }
            #endregion
            return ds;
        }
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

        /// <summary>
        /// 构建Datatable结构
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Keys"></param>
        /// <param name="newDs"></param>
        /// <returns></returns>
        private DataTable GetDataTableStruct(string item, Dictionary<string, object> rowData, DataSet newDs)
        {
            DataTable itemDt = new DataTable();
            if (newDs.Tables.Contains(item))
            {
                itemDt = newDs.Tables[item];
            }
            else
            {
                itemDt.TableName = item;
                if (rowData != null)
                {
                    foreach (string colItem in rowData.Keys)
                    {
                        DataColumn dc = new DataColumn(colItem);
                        dc.ExtendedProperties.Add("ColType", "string");
                        itemDt.Columns.Add(dc);
                    }
                }
            }
            return itemDt;
        }
        /// <summary> 
        /// 保存数据集
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public bool SaveData(string viewId, string dataPath)
        {
            bool editFlag = true;
            DataTable dt = new DataTable();
            foreach (JsonMapKey item in JsonTableMap.Keys)
            {
                if (item.ViewId.Equals(viewId) && item.DataPath.Equals(dataPath))
                {
                    dt = ((DataSet)JsonTableMap[item]).Tables["dataArray"];
                    break;
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (editFlag)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                            Dictionary<string, object> addDic = new Dictionary<string, object>();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.ExtendedProperties.ContainsKey("ColType") && !dc.ExtendedProperties["ColType"].ToString().Equals("ref"))
                                {
                                    if (dc.ExtendedProperties["ColType"].ToString().Equals("document"))
                                    {
                                        addDic.Add(dc.ColumnName, JsonHelper.ToObject<Dictionary<string, object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else if (dc.ExtendedProperties["ColType"].ToString().Equals("array"))
                                    {
                                        addDic.Add(dc.ColumnName, JsonHelper.ToObject<List<object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else
                                    {
                                        switch (dc.ExtendedProperties["ColType"].ToString())
                                        {
                                            case "int":
                                                addDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? 0 : dr[dc.ColumnName]);
                                                break;
                                            case "date":
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? DateTime.Now : dr[dc.ColumnName]);
                                                break;
                                            case "timestamp":
                                                DateTime startTime = new DateTime(1970, 1, 1);
                                                if (dr[dc.ColumnName] != null && !string.IsNullOrWhiteSpace(dr[dc.ColumnName].ToString()))
                                                {
                                                    DateTime currentTime = (DateTime)dr[dc.ColumnName];
                                                    addDic.Add(dc.ColumnName, (long)(currentTime - startTime).TotalMilliseconds);
                                                }
                                                else
                                                {
                                                    addDic.Add(dc.ColumnName, 0);
                                                }
                                                break;
                                            case "array":
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new List<object>() : dr[dc.ColumnName]);
                                                break;
                                            case "document":
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new Dictionary<string, object>() : dr[dc.ColumnName]);
                                                break;
                                            case "string":
                                            default:
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? string.Empty : dr[dc.ColumnName]);
                                                break;
                                        }
                                    }
                                }

                            }
                            editFlag = DataTool.SaveCurdDataByPath(viewId, JsonHelper.ToObject<List<object>>(dataPath), addDic, OpreateStateEnum.Added);
                            break;
                        case DataRowState.Deleted:
                            Dictionary<string, object> delDic = new Dictionary<string, object>();
                            delDic.Add("_id", dr["_id", DataRowVersion.Original]);
                            editFlag = DataTool.SaveCurdDataByPath(viewId, JsonHelper.ToObject<List<object>>(dataPath), delDic, OpreateStateEnum.Deleted);
                            break;
                        case DataRowState.Detached:
                            break;
                        case DataRowState.Modified:
                            Dictionary<string, object> modDic = new Dictionary<string, object>();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.ExtendedProperties.ContainsKey("ColType") && !dc.ExtendedProperties["ColType"].ToString().Equals("ref"))
                                {
                                    if (dc.ExtendedProperties["ColType"].ToString().Equals("document"))
                                    {
                                        modDic.Add(dc.ColumnName, JsonHelper.ToObject<Dictionary<string, object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else if (dc.ExtendedProperties["ColType"].ToString().Equals("array"))
                                    {
                                        modDic.Add(dc.ColumnName, JsonHelper.ToObject<List<object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else
                                    {
                                        switch (dc.ExtendedProperties["ColType"].ToString())
                                        {
                                            case "int":
                                                modDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? 0 : dr[dc.ColumnName]);
                                                break;
                                            case "date":
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? DateTime.Now : dr[dc.ColumnName]);
                                                break;
                                            case "timestamp":
                                                DateTime startTime = new DateTime(1970, 1, 1);

                                                if (dr[dc.ColumnName] != null && !string.IsNullOrWhiteSpace(dr[dc.ColumnName].ToString()))
                                                {
                                                    DateTime currentTime = (DateTime)dr[dc.ColumnName];
                                                    modDic.Add(dc.ColumnName, (long)(currentTime - startTime).TotalMilliseconds);
                                                }
                                                else
                                                {
                                                    modDic.Add(dc.ColumnName, 0);
                                                }
                                                break;
                                            case "array":
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new List<object>() : dr[dc.ColumnName]);
                                                break;
                                            case "document":
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new Dictionary<string, object>() : dr[dc.ColumnName]);
                                                break;
                                            case "string":
                                            default:
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? string.Empty : dr[dc.ColumnName]);
                                                break;
                                        }
                                    }
                                }
                            }
                            List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
                            if (pathList[pathList.Count - 1].GetType().Name.Equals("String"))
                            {
                                Dictionary<string, object> pathDic = new Dictionary<string, object>();
                                pathDic.Add("key", "_id");
                                pathDic.Add("value", dr["_id"]);
                                pathList.Add(pathDic);
                            }
                            editFlag = DataTool.SaveCurdDataByPath(viewId, pathList, modDic, OpreateStateEnum.Modified);
                            break;
                        case DataRowState.Unchanged:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
            if (editFlag)
            {
                dt.AcceptChanges();
            }
            return editFlag;
        }
        /// <summary>
        /// 释放数据
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public bool DisposeData(string viewId)
        {
            bool existFlag = false;
            try
            {
                foreach (JsonMapKey item in JsonTableMap)
                {
                    if (item.ViewId.Equals(viewId))
                    {
                        JsonTableMap.Remove(item);
                        existFlag = true;
                    }
                }
                return existFlag;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取模型关系
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public string GetModelRelation(string viewId)
        {
            string modelData = DataTool.GetDataByPath(viewId, "[\"model\"]");
            if (!string.IsNullOrEmpty(modelData))
            {
                return JsonHelper.ReadJsonString(modelData, "relation");
            }
            else
            {
                return string.Empty;
            }

        }
    }

    internal class JsonMapKey
    {
        internal string ViewId { get; set; }
        internal string DataPath { get; set; }
    }
}
