﻿using Noesis.Javascript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Victop.Frame.CoreLibrary.Enums;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.CoreLibrary.MongoModel;
using Victop.Frame.DataChannel.Enums;
using Victop.Frame.DataChannel.Models;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.DataChannel
{
    /// <summary>
    /// 数据转换器
    /// </summary>
    public class DataConvertManager
    {
        private static Hashtable jsonTableMap;

        public static Hashtable JsonTableMap
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
            if (string.IsNullOrEmpty(dataPath) || string.IsNullOrEmpty(viewId))
                return null;
            DataSet newDs = structDs == null ? new DataSet() : structDs.Copy();
            newDs.Clear();
            List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
            string tableName = pathList[pathList.Count - 1].GetType().Name.Equals("String") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString();
            #region 获取模型定义及简单引用
            DataChannelManager dataChannelManager = new DataChannelManager();
            Hashtable hashData = dataChannelManager.GetData(viewId);
            ChannelData channelData = hashData["Data"] as ChannelData;
            MongoModelInfoModel modelDefInfo = channelData.ModelDefInfo;
            MongoSimpleRefInfoModel simpleRefInfo = channelData.SimpleRefInfo;
            #endregion
            string jsonData = DataTool.GetDataByPath(viewId, dataPath);
            if (!string.IsNullOrEmpty(jsonData))
            {
                Dictionary<string, object> jsonDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonData);
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
                itemDt = GetDataTableStructByModel(modelDefInfo, simpleRefInfo, tableName, viewId, dataPath);
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
                    DataStoreInfo storeInfo = JsonTableMap[item] as DataStoreInfo;
                    storeInfo.ActualDataInfo = newDs;
                    JsonTableMap[item] = storeInfo;
                    checkFlag = true;
                    break;
                }
            }
            if (!checkFlag)
            {
                JsonMapKey mapKey = new JsonMapKey() { ViewId = viewId, DataPath = dataPath };
                DataStoreInfo storeInfo = new DataStoreInfo() { ActualDataInfo = newDs };
                if (modelDefInfo != null)
                {
                    MongoModelInfoOfTablesModel tableModel = modelDefInfo.ModelTables.FirstOrDefault(it => it.TableName.Equals(tableName));
                    if (tableModel != null)
                    {
                        storeInfo.BakFlag = tableModel.BakFlag;
                    }
                }
                JsonTableMap.Add(mapKey, storeInfo);
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
                    else if (dtCol.ExtendedProperties["ColType"] != null)
                    {
                        CreateDataRowValue(jsonDic, objectDr, dtCol);
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
                if (jsonDic[item] != null)
                {
                    try
                    {
                        switch (jsonDic[item].GetType().Name)
                        {
                            case "JArray":
                            case "Object[]":
                                List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ToJson(jsonDic[item]));
                                if (!newDs.Tables.Contains(item))
                                {
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
                                }
                                else
                                {
                                    itemDt = newDs.Tables[item];
                                }
                                foreach (Dictionary<string, object> rowItem in arrayList)
                                {
                                    if (rowItem.ContainsKey("_id") && string.IsNullOrEmpty(rowItem["_id"].ToString()))
                                    {
                                        continue;
                                    }
                                    DataRow arrayDr = itemDt.NewRow();
                                    foreach (DataColumn dtCol in itemDt.Columns)
                                    {
                                        if (rowItem.ContainsKey(dtCol.ColumnName))
                                        {
                                            if (rowItem[dtCol.ColumnName] == null)
                                            {
                                                arrayDr[dtCol.ColumnName] = DBNull.Value;
                                            }
                                            else if (dtCol.ExtendedProperties["ColType"] != null)
                                            {
                                                CreateDataRowValue(rowItem, arrayDr, dtCol);
                                            }
                                        }
                                    }
                                    itemDt.Rows.Add(arrayDr);
                                }
                                break;
                            case "JObject":
                            case "Object":
                                Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonDic[item].ToString());
                                if (!newDs.Tables.Contains(item))
                                {
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
                                }
                                else
                                {
                                    itemDt = newDs.Tables[item];
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
                                        else if (dtCol.ExtendedProperties["ColType"] != null)
                                        {
                                            CreateDataRowValue(itemDic, objectDr, dtCol);
                                        }
                                    }
                                }
                                itemDt.Rows.Add(objectDr);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        itemDt.TableName = item;
                    }
                }
                else
                {
                    itemDt.TableName = item;
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
        /// 添加数据
        /// 3.0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newData"></param>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        internal bool AddData<T>(T newData, string viewId, List<object> dataPath)
        {
            string dataStr = JsonHelper.ToJson(newData);
            Dictionary<string, object> addDic = JsonHelper.ToObject<Dictionary<string, object>>(dataStr);
            return DataTool.SaveCurdDataByPath(viewId, dataPath, addDic, null, OpreateStateEnum.Added);
        }
        /// <summary>
        /// 修改数据
        /// 3.0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modifyData"></param>
        /// <param name="viewId"></param>
        /// <param name="blockPath"></param>
        /// <returns></returns>
        internal bool ModifyData<T>(T modifyData, string viewId, List<object> blockPath)
        {
            string dataStr = JsonHelper.ToJson(modifyData);
            Dictionary<string, object> modifyDic = JsonHelper.ToObject<Dictionary<string, object>>(dataStr);
            if (modifyDic != null && modifyDic.ContainsKey("_id"))
            {
                List<object> newPath = new List<object>();
                foreach (var item in blockPath)
                {
                    newPath.Add(item);
                }
                Dictionary<string, object> idDic = new Dictionary<string, object>();
                idDic.Add("key", "_id");
                idDic.Add("value", modifyDic["_id"]);
                newPath.Add(idDic);
                return DataTool.SaveCurdDataByPath(viewId, newPath, modifyDic, null, OpreateStateEnum.Modified);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除数据
        /// 3.0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deleteData"></param>
        /// <param name="viewId"></param>
        /// <param name="blockPath"></param>
        /// <returns></returns>
        internal bool DeleteData<T>(T deleteData, string viewId, List<object> blockPath)
        {
            string dataStr = JsonHelper.ToJson(deleteData);
            Dictionary<string, object> deleteDic = JsonHelper.ToObject<Dictionary<string, object>>(dataStr);
            return DataTool.SaveCurdDataByPath(viewId, blockPath, deleteDic, null, OpreateStateEnum.Deleted);
        }
        private static void CreateDataRowValue(Dictionary<string, object> itemDic, DataRow objectDr, DataColumn dtCol, bool refTypeFlag = false)
        {
            try
            {
                switch (refTypeFlag ? dtCol.ExtendedProperties["RefType"].ToString() : dtCol.ExtendedProperties["ColType"].ToString())
                {
                    case "int":
                        objectDr[dtCol.ColumnName] = string.IsNullOrEmpty(itemDic[dtCol.ColumnName].ToString()) ? (int)0 : Convert.ToInt32(itemDic[dtCol.ColumnName].ToString());
                        break;
                    case "long":
                        objectDr[dtCol.ColumnName] = string.IsNullOrEmpty(itemDic[dtCol.ColumnName].ToString()) ? (long)0 : Convert.ToInt64(itemDic[dtCol.ColumnName].ToString());
                        break;
                    case "double":
                        objectDr[dtCol.ColumnName] = string.IsNullOrEmpty(itemDic[dtCol.ColumnName].ToString()) ? (double)0.00 : Convert.ToDouble(itemDic[dtCol.ColumnName].ToString());
                        break;
                    case "float":
                        objectDr[dtCol.ColumnName] = string.IsNullOrEmpty(itemDic[dtCol.ColumnName].ToString()) ? (decimal)0.00 : Convert.ToDecimal(itemDic[dtCol.ColumnName].ToString());
                        break;
                    case "boolean":
                        objectDr[dtCol.ColumnName] = string.IsNullOrEmpty(itemDic[dtCol.ColumnName].ToString()) ? false : Convert.ToBoolean(itemDic[dtCol.ColumnName]);
                        break;
                    case "timestamp":
                        if (Convert.ToInt64(itemDic[dtCol.ColumnName].ToString()) == 0)
                        {
                            objectDr[dtCol.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
                            dt = dt.AddMilliseconds(Convert.ToInt64(itemDic[dtCol.ColumnName].ToString()));
                            objectDr[dtCol.ColumnName] = dt;
                        }
                        break;
                    case "date":
                        if (string.IsNullOrEmpty(itemDic[dtCol.ColumnName].ToString()))
                        {
                            objectDr[dtCol.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            objectDr[dtCol.ColumnName] = itemDic[dtCol.ColumnName];
                        }
                        break;
                    case "ref":
                        CreateDataRowValue(itemDic, objectDr, dtCol, true);
                        break;
                    case "string":
                    default:
                        objectDr[dtCol.ColumnName] = itemDic[dtCol.ColumnName];
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("数据生成异常!字段{0}的类型为{1},对应数据为:{2}", dtCol.ColumnName, refTypeFlag ? dtCol.ExtendedProperties["RefType"].ToString() : dtCol.ExtendedProperties["ColType"].ToString(), itemDic[dtCol.ColumnName]);
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
            if (modelDefInfo.ModelVersion != null && modelDefInfo.ModelVersion.StartsWith("2."))
            {
                BuildColumnsOfDataTableV2(modelDefInfo, simpleRefInfo, tableName, dataPath, newDt);
            }
            else
            {
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
            }
            return newDt;
        }
        /// <summary>
        /// 构建模型2.0的表结构
        /// </summary>
        /// <param name="modelDefInfo"></param>
        /// <param name="simpleRefInfo"></param>
        /// <param name="tableName"></param>
        /// <param name="dataPath"></param>
        /// <param name="newDt"></param>
        private void BuildColumnsOfDataTableV2(MongoModelInfoModel modelDefInfo, MongoSimpleRefInfoModel simpleRefInfo, string tableName, string dataPath, DataTable newDt)
        {
            List<object> dataPathList = JsonHelper.ToObject<List<object>>(dataPath);
            string RebuildPath = string.Empty;
            #region 构建Path
            for (int i = 0; i < dataPathList.Count; i++)
            {
                if (dataPathList[i].GetType().Name.Equals("String"))
                {
                    RebuildPath += dataPathList[i].ToString() + ".";
                    if (i == dataPathList.Count - 1)
                    {
                        RebuildPath += "dataArray.";
                    }
                }
                else
                {
                    RebuildPath += "dataArray.";
                }
            }
            #endregion
            MongoModelInfoOfTablesModel modelInfoOfTableInfo = modelDefInfo.ModelTables.FirstOrDefault(it => it.TableName.Equals(tableName) && it.EntityFlag == 1);
            #region 表结构字段
            if (modelInfoOfTableInfo != null)
            {
                foreach (var item in modelInfoOfTableInfo.TableStructure)
                {
                    if (item.FieldValue.ValueSelectFlag == 1)
                    {
                        DataColumn dc = new DataColumn(item.FieldKey);
                        if (modelDefInfo.ModelClientRef != null && modelDefInfo.ModelClientRef.Count > 0)
                        {
                            MongoModelInfoOfClientRefModel clientRefInfo = modelDefInfo.ModelClientRef.Find(it => (
                                it.ClientRefField.Equals(RebuildPath + item.FieldKey)));
                            if (clientRefInfo != null)
                            {
                                dc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefInfo));
                            }
                        }
                        SetDataColumnDataType(item, dc);
                        DataSet ds = GetSimpleRef(simpleRefInfo, dataPath, item.FieldKey, null);
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
            #endregion
            #region 引用字段
            foreach (var item in modelDefInfo.ModelExtInfo.Where(it => it.ExtPath.Contains(tableName)))
            {
                foreach (var attrItem in item.ExtAttr.Where(it => it.AttrRefFlag == 1 && it.AttrLeft.StartsWith(tableName)))
                {
                    string[] leftStrs = attrItem.AttrLeft.Split('.');
                    string[] rightStrs = attrItem.AttrRight.Split('.');
                    DataColumn refDc = new DataColumn(leftStrs[1]);
                    refDc.ExtendedProperties.Add("ColType", "ref");
                    if (modelDefInfo.ModelClientRef != null && modelDefInfo.ModelClientRef.Count > 0)
                    {
                        MongoModelInfoOfClientRefModel clientRefInfo = modelDefInfo.ModelClientRef.Find(it => (
                            it.ClientRefField.Equals(RebuildPath + leftStrs[1])));
                        if (clientRefInfo != null)
                        {
                            refDc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefInfo));
                        }
                    }
                    MongoModelInfoOfTableStructureModel refFieldModel = modelDefInfo.ModelTables.First(it => it.TableName.Equals(rightStrs[0])).TableStructure.First(it => it.FieldKey.Equals(rightStrs[1]));
                    refDc.ExtendedProperties.Add("RefType", refFieldModel.FieldValue.ValueType);
                    SetDataColumnDataType(refFieldModel, refDc, false);
                    DataSet ds = GetSimpleRef(simpleRefInfo, dataPath, leftStrs[1], null);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("dataArray"))
                    {
                        refDc.ExtendedProperties.Add("ComboBox", ds.Tables["dataArray"]);
                    }
                    if (!newDt.Columns.Contains(refDc.ColumnName))
                    {
                        newDt.Columns.Add(refDc);
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 设置数据列数据类型
        /// </summary>
        /// <param name="item">列信息</param>
        /// <param name="dc">数据列</param>
        /// <param name="entityField">是否为实体字段</param>
        private static void SetDataColumnDataType(MongoModelInfoOfTableStructureModel item, DataColumn dc, bool entityField = true)
        {
            switch (item.FieldValue.ValueType)
            {
                case "int":
                    dc.DataType = typeof(int);
                    break;
                case "long":
                    dc.DataType = typeof(long);
                    break;
                case "double":
                    dc.DataType = typeof(double);
                    break;
                case "float":
                    dc.DataType = typeof(decimal);
                    break;
                case "boolean":
                    dc.DataType = typeof(bool);
                    break;
                case "date":
                    dc.DataType = typeof(DateTime);
                    break;
                case "timestamp":
                    dc.DataType = typeof(DateTime);
                    break;
                case "array":
                    dc.DataType = typeof(string);
                    break;
                case "document":
                    dc.DataType = typeof(string);
                    break;
                case "string":
                default:
                    dc.DataType = typeof(string);
                    break;
            }
            if (entityField)
                dc.ExtendedProperties.Add("ColType", item.FieldValue.ValueType);
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
            string RebuildPath = string.Empty;
            #region 构建Path
            for (int i = 0; i < dataPathList.Count; i++)
            {
                if (dataPathList[i].GetType().Name.Equals("String"))
                {
                    RebuildPath += dataPathList[i].ToString() + ".";
                    if (i == dataPathList.Count - 1)
                    {
                        RebuildPath += "dataArray.";
                    }
                }
                else
                {
                    RebuildPath += "dataArray.";
                }
            }
            #endregion
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
                                MongoModelInfoOfClientRefModel clientRefInfo = modelDefInfo.ModelClientRef.Find(it => (
                                    it.ClientRefField.Equals(RebuildPath + keyStr)));
                                if (clientRefInfo != null)
                                {
                                    dc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefInfo));
                                }
                            }
                            SetDataColumnDataType(item, dc);
                            DataSet ds = GetSimpleRef(simpleRefList, dataPath, keyStr, null);
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
                                MongoModelInfoOfClientRefModel clientRefInfo = modelDefInfo.ModelClientRef.Find(it => (
                                   it.ClientRefField.Equals(RebuildPath + item.FieldKey)));
                                if (clientRefInfo != null)
                                {
                                    dc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefInfo));
                                }
                            }
                            SetDataColumnDataType(item, dc, true);
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
                            if (refitem.ContentLeft.StartsWith(RebuildPath) && !refitem.ContentLeft.Substring(RebuildPath.Length).Contains("."))
                            {
                                string leftStr = refitem.ContentLeft.Substring(refitem.ContentLeft.LastIndexOf('.') + 1);
                                if (newDt.Columns.Contains(leftStr))
                                    continue;
                                DataColumn dc = new DataColumn(leftStr);
                                if (modelDefInfo.ModelClientRef != null && modelDefInfo.ModelClientRef.Count > 0)
                                {
                                    MongoModelInfoOfClientRefModel clientRefInfo = modelDefInfo.ModelClientRef.Find(it => (
                                    it.ClientRefField.Equals(RebuildPath + leftStr)));
                                    if (clientRefInfo != null)
                                    {
                                        dc.ExtendedProperties.Add("DataReference", JsonHelper.ToJson(clientRefInfo));
                                    }
                                }
                                dc.ExtendedProperties.Add("ColType", "ref");
                                dc.ExtendedProperties.Add("RefType", refitem.ContentType);
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
                                        case "double":
                                            dc.DataType = typeof(Double);
                                            break;
                                        case "float":
                                            dc.DataType = typeof(float);
                                            break;
                                        case "boolean":
                                            dc.DataType = typeof(Boolean);
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
                    Dictionary<string, object> newDependDic = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(propertyModel.PropertyDepend))
                    {
                        string dependColumn = propertyModel.PropertyDepend.Substring(propertyModel.PropertyDepend.IndexOf(":") + 1);
                        dependColumn = dependColumn.Substring(0, dependColumn.IndexOf("]"));

                        string parentConstructPath = propertyModel.PropertyDepend.Substring(0, propertyModel.PropertyDepend.IndexOf(":") + 1) + dependColumn + propertyModel.PropertyDepend.Substring(propertyModel.PropertyDepend.IndexOf("]"));
                        MongoSimpleRefInfoOfArrayPropertyModel parentPropertyModel = item.ArrayProperty.FirstOrDefault(it => it.PropertyKey.Equals(parentConstructPath));
                        if (parentPropertyModel != null)
                        {
                            if (!string.IsNullOrEmpty(parentPropertyModel.PropertyDepend))
                            {
                                string parentDependColumn = parentPropertyModel.PropertyDepend.Substring(parentPropertyModel.PropertyDepend.IndexOf(":") + 1);
                                parentDependColumn = parentDependColumn.Substring(0, parentDependColumn.IndexOf("]"));
                                if (dependDic.ContainsKey(parentDependColumn))
                                {
                                    newDependDic.Add(parentDependColumn, dependDic[parentDependColumn]);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                        if (dependDic.ContainsKey(dependColumn))
                        {
                            newDependDic.Add(dependColumn, dependDic[dependColumn]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    DataTable dt = GetDataByConsturctPath(propertyModel.PropertyValue, propertyModel.PropertyValue, item.ArrayValueList, newDependDic);
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
            int bakFlag = 0;
            DataTable dt = new DataTable();
            List<SaveDataModel> saveDataList = new List<SaveDataModel>();
            foreach (JsonMapKey item in JsonTableMap.Keys)
            {
                if (item.ViewId.Equals(viewId) && item.DataPath.Equals(dataPath))
                {
                    DataStoreInfo storeInfo = JsonTableMap[item] as DataStoreInfo;
                    dt = storeInfo.ActualDataInfo.Tables["dataArray"];
                    bakFlag = storeInfo.BakFlag;
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
                                            case "long":
                                                addDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? 0 : dr[dc.ColumnName]);
                                                break;
                                            case "double":
                                            case "float":
                                                addDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? 0 : Convert.ToDecimal(dr[dc.ColumnName]));
                                                break;
                                            case "date":
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? DateTime.Now : dr[dc.ColumnName]);
                                                break;
                                            case "timestamp":
                                                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
                                                if (dr[dc.ColumnName] != null && !string.IsNullOrWhiteSpace(dr[dc.ColumnName].ToString()))
                                                {
                                                    DateTime currentTime = (DateTime)dr[dc.ColumnName];
                                                    addDic.Add(dc.ColumnName, (long)(currentTime.ToUniversalTime() - startTime.ToUniversalTime()).TotalMilliseconds);
                                                }
                                                else
                                                {
                                                    addDic.Add(dc.ColumnName, (long)0);
                                                }
                                                break;
                                            case "array":
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new List<object>() : dr[dc.ColumnName]);
                                                break;
                                            case "document":
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new Dictionary<string, object>() : dr[dc.ColumnName]);
                                                break;
                                            case "boolean":
                                                addDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? false : Convert.ToBoolean(dr[dc.ColumnName]));
                                                break;
                                            case "string":
                                            default:
                                                addDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? string.Empty : dr[dc.ColumnName]);
                                                break;
                                        }
                                    }
                                }

                            }
                            editFlag = DataTool.SaveCurdDataByPath(viewId, JsonHelper.ToObject<List<object>>(dataPath), addDic, null, OpreateStateEnum.Added);
                            //saveDataList.Add(new SaveDataModel() { OpStatus = OpreateStateEnum.Added, DataPath = JsonHelper.ToObject<List<object>>(dataPath), SaveDataDic = addDic, OriginalDataDic = null });
                            break;
                        case DataRowState.Deleted:
                            Dictionary<string, object> delDic = new Dictionary<string, object>();
                            delDic.Add("_id", dr["_id", DataRowVersion.Original]);
                            editFlag = DataTool.SaveCurdDataByPath(viewId, JsonHelper.ToObject<List<object>>(dataPath), delDic, null, OpreateStateEnum.Deleted);
                            //saveDataList.Add(new SaveDataModel() { OpStatus = OpreateStateEnum.Deleted, DataPath = JsonHelper.ToObject<List<object>>(dataPath), SaveDataDic = delDic, OriginalDataDic = null });
                            break;
                        case DataRowState.Detached:
                            break;
                        case DataRowState.Modified:
                            Dictionary<string, object> modDic = new Dictionary<string, object>();
                            Dictionary<string, object> originDic = new Dictionary<string, object>();
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
                                            case "long":
                                                modDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? 0 : dr[dc.ColumnName]);
                                                originDic.Add(dc.ColumnName, (dr[dc.ColumnName, DataRowVersion.Original] == null || string.IsNullOrEmpty(dr[dc.ColumnName, DataRowVersion.Original].ToString())) ? 0 : dr[dc.ColumnName, DataRowVersion.Original]);
                                                break;
                                            case "double":
                                            case "float":
                                                modDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? 0 : Convert.ToDecimal(dr[dc.ColumnName]));
                                                originDic.Add(dc.ColumnName, (dr[dc.ColumnName, DataRowVersion.Original] == null || string.IsNullOrEmpty(dr[dc.ColumnName, DataRowVersion.Original].ToString())) ? 0 : Convert.ToDecimal(dr[dc.ColumnName, DataRowVersion.Original]));
                                                break;
                                            case "date":
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? DateTime.Now : dr[dc.ColumnName]);
                                                originDic.Add(dc.ColumnName, dr[dc.ColumnName, DataRowVersion.Original]);
                                                break;
                                            case "timestamp":
                                                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
                                                if (dr[dc.ColumnName] != null && !string.IsNullOrWhiteSpace(dr[dc.ColumnName].ToString()))
                                                {
                                                    DateTime currentTime = (DateTime)dr[dc.ColumnName];
                                                    modDic.Add(dc.ColumnName, (long)(currentTime.ToUniversalTime() - startTime.ToUniversalTime()).TotalMilliseconds);
                                                    if (dr[dc.ColumnName, DataRowVersion.Original] != null && !string.IsNullOrEmpty(dr[dc.ColumnName, DataRowVersion.Original].ToString()))
                                                    {
                                                        DateTime originTime = (DateTime)dr[dc.ColumnName, DataRowVersion.Original];
                                                        originDic.Add(dc.ColumnName, (long)(originTime.ToUniversalTime() - startTime.ToUniversalTime()).TotalMilliseconds);
                                                    }
                                                    else
                                                    {
                                                        originDic.Add(dc.ColumnName, (long)0);
                                                    }

                                                }
                                                else
                                                {
                                                    modDic.Add(dc.ColumnName, (long)0);
                                                    originDic.Add(dc.ColumnName, (long)0);
                                                }
                                                break;
                                            case "array":
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new List<object>() : dr[dc.ColumnName]);
                                                originDic.Add(dc.ColumnName, dr[dc.ColumnName, DataRowVersion.Original] == null ? new List<object>() : dr[dc.ColumnName, DataRowVersion.Original]);
                                                break;
                                            case "document":
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? new Dictionary<string, object>() : dr[dc.ColumnName]);
                                                originDic.Add(dc.ColumnName, dr[dc.ColumnName, DataRowVersion.Original] == null ? new Dictionary<string, object>() : dr[dc.ColumnName, DataRowVersion.Original]);
                                                break;
                                            case "boolean":
                                                modDic.Add(dc.ColumnName, (dr[dc.ColumnName] == null || string.IsNullOrEmpty(dr[dc.ColumnName].ToString())) ? false : Convert.ToBoolean(dr[dc.ColumnName]));
                                                originDic.Add(dc.ColumnName, dr[dc.ColumnName, DataRowVersion.Original] == null ? new Dictionary<string, object>() : dr[dc.ColumnName, DataRowVersion.Original]);
                                                break;
                                            case "string":
                                            default:
                                                modDic.Add(dc.ColumnName, dr[dc.ColumnName] == null ? string.Empty : dr[dc.ColumnName]);
                                                originDic.Add(dc.ColumnName, dr[dc.ColumnName, DataRowVersion.Original] == null ? string.Empty : dr[dc.ColumnName, DataRowVersion.Original]);
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
                            editFlag = DataTool.SaveCurdDataByPath(viewId, pathList, modDic, originDic, OpreateStateEnum.Modified);
                            //saveDataList.Add(new SaveDataModel() { OpStatus = OpreateStateEnum.Modified, DataPath = pathList, SaveDataDic = modDic, OriginalDataDic = originDic });
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
            //if (saveDataList.Count > 0)
            //{
            //    //TODO:保存数据
            //    editFlag = DataTool.SaveCurdDataByPath(viewId, saveDataList, bakFlag);
            //}
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
}
