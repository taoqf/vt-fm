using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        /// 获取数据DataTable
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string viewId, string dataPath, DataTable structDt = null)
        {
            if (string.IsNullOrEmpty(dataPath))
                return null;
            string jsonData = string.Empty;
            List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
            DataTable newDt;
            if (structDt == null)
            {
                string tableName = pathList[pathList.Count - 1].ToString();
                if (!string.IsNullOrEmpty(JsonHelper.ReadJsonString(pathList[pathList.Count - 1].ToString(), "key")))
                {
                    tableName = pathList[pathList.Count - 2].ToString() + "_row";
                }
                newDt = new DataTable(tableName);
            }
            else
            {
                newDt = structDt.Copy();
            }
            newDt.Clear();
            jsonData = DataTool.GetDataByPath(viewId, dataPath);
            if (!string.IsNullOrEmpty(jsonData))
            {
                Dictionary<string, object> jsonDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonData);
                if (pathList.Count % 2 == 1)
                {
                    jsonData = jsonDic["dataArray"].ToString();
                    List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonData);
                    if (arrayList != null && arrayList.Count > 0)
                    {
                        if (structDt == null)
                        {
                            foreach (string item in arrayList[0].Keys)
                            {
                                if (!newDt.Columns.Contains(item))
                                {
                                    DataColumn dc = new DataColumn(item);
                                    newDt.Columns.Add(dc);
                                }
                            }
                        }
                        foreach (Dictionary<string, object> item in arrayList)
                        {
                            DataRow dr = newDt.NewRow();
                            foreach (DataColumn dtCol in newDt.Columns)
                            {
                                if (item.ContainsKey(dtCol.ColumnName))
                                {
                                    dr[dtCol.ColumnName] = item[dtCol.ColumnName];
                                }
                            }
                            newDt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    if (structDt != null)
                    {
                        DataRow dr = newDt.NewRow();
                        foreach (DataColumn dtCol in newDt.Columns)
                        {
                            if (jsonDic.ContainsKey(dtCol.ColumnName))
                            {
                                dr[dtCol.ColumnName] = jsonDic[dtCol.ColumnName];
                            }
                        }
                        newDt.Rows.Add(dr);
                    }
                    else
                    {
                        foreach (string item in jsonDic.Keys)
                        {
                            if (!newDt.Columns.Contains(item))
                            {
                                DataColumn dc = new DataColumn(item);
                                newDt.Columns.Add(dc);
                            }
                        }
                        DataRow dr = newDt.NewRow();
                        foreach (string item in jsonDic.Keys)
                        {
                            dr[item] = jsonDic[item];
                        }
                        newDt.Rows.Add(dr);
                    }
                }
            }
            newDt.AcceptChanges();
            bool existFlag = false;
            foreach (JsonMapKey item in JsonTableMap.Keys)
            {
                if (item.ViewId == viewId && item.DataPath == dataPath)
                {
                    JsonTableMap[item] = newDt;
                    existFlag = true;
                    break;
                }
            }
            if (!existFlag)
            {
                JsonMapKey mapKey = new JsonMapKey() { ViewId = viewId, DataPath = dataPath };
                JsonTableMap.Add(mapKey, newDt);
            }
            return newDt;
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
            string modelData = DataTool.GetDataByPath(viewId, "[\"model\"]");
            string jsonData = DataTool.GetDataByPath(viewId, dataPath);
            if (!string.IsNullOrEmpty(jsonData))
            {
                Dictionary<string, object> jsonDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonData);
                if (pathList.Count % 2 == 1)//获取表数据
                {
                    #region 组织表数据
                    foreach (string item in jsonDic.Keys)
                    {
                        List<string> delColList = new List<string>();
                        DataTable itemDt = new DataTable();
                        switch (jsonDic[item].GetType().Name)
                        {
                            case "JArray":
                                List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonDic[item].ToString());
                                itemDt = new DataTable(item);
                                if (!string.IsNullOrEmpty(modelData) && item.Equals("dataArray"))
                                {
                                    itemDt = GetDataTableStructByModel(modelData, pathList[pathList.Count - 1].GetType().Name.Equals("String") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString());
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
                                            if (rowItem[dtCol.ColumnName] != null && !rowItem[dtCol.ColumnName].GetType().Name.Equals("String"))
                                            {
                                                if (!dtCol.ExtendedProperties.ContainsKey("ColType"))
                                                {
                                                    dtCol.ExtendedProperties.Add("ColType", rowItem[dtCol.ColumnName].GetType().Name);
                                                }
                                                else
                                                {
                                                    dtCol.ExtendedProperties["ColType"] = rowItem[dtCol.ColumnName].GetType().Name;
                                                }
                                            }
                                            else
                                            {
                                                if (!dtCol.ExtendedProperties.ContainsKey("ColType"))
                                                {
                                                    dtCol.ExtendedProperties.Add("ColType", "String");
                                                }
                                                else
                                                {
                                                    dtCol.ExtendedProperties["ColType"] = "String";
                                                }
                                            }
                                            arrayDr[dtCol.ColumnName] = rowItem[dtCol.ColumnName];
                                        }
                                    }
                                    itemDt.Rows.Add(arrayDr);
                                }
                                break;
                            case "JObject":
                                Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonDic[item].ToString());
                                if (!string.IsNullOrEmpty(modelData) && item.Equals("dataArray"))
                                {
                                    itemDt = GetDataTableStructByModel(modelData, pathList[pathList.Count - 1].GetType().Name.Equals("String") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString());
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
                                            else
                                            {
                                                dtCol.ExtendedProperties["ColType"] = itemDic[dtCol.ColumnName].GetType().Name;
                                            }
                                        }
                                        else
                                        {
                                            if (!dtCol.ExtendedProperties.ContainsKey("ColType"))
                                            {
                                                dtCol.ExtendedProperties.Add("ColType", "String");
                                            }
                                            else
                                            {
                                                dtCol.ExtendedProperties["ColType"] = "String";
                                            }
                                        }
                                        objectDr[dtCol.ColumnName] = itemDic[dtCol.ColumnName];
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
                    #endregion
                }
                else//获取行数据
                {
                    #region 组织行数据
                    DataTable itemDt = new DataTable("dataArray");
                    if (!string.IsNullOrEmpty(modelData))
                    {
                        itemDt = GetDataTableStructByModel(modelData, pathList[pathList.Count - 1].GetType().Name.Equals("String") ? pathList[pathList.Count - 1].ToString() : pathList[pathList.Count - 2].ToString());
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
                            objectDr[dtCol.ColumnName] = jsonDic[dtCol.ColumnName];
                        }
                    }
                    itemDt.Rows.Add(objectDr);
                    itemDt.AcceptChanges();

                    if (newDs.Tables.Contains(itemDt.TableName))
                    {
                        newDs.Tables.Remove("dataArray");
                    }
                    newDs.Tables.Add(itemDt);
                    #endregion
                }
            }
            else
            {
                DataTable itemDt = new DataTable("dataArray");
                //TODO:构建表结构
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
        /// 根据模型定义生成表结构
        /// </summary>
        /// <param name="modelJson"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private DataTable GetDataTableStructByModel(string modelJson, string tableName)
        {
            DataTable newDt = new DataTable("dataArray");
            if (!string.IsNullOrEmpty(modelJson))
            {
                List<Dictionary<string, object>> tableList = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ReadJsonString(modelJson, "tables"));
                if (tableList != null && tableList.Count > 0)
                {
                    tableList = JsonHelper.ToObject<List<Dictionary<string, object>>>(tableList.Find(it => it["name"].ToString().Equals(tableName))["structure"].ToString());
                    foreach (Dictionary<string, object> item in tableList)
                    {
                        if (item.ContainsKey("value"))
                        {
                            string selectFlag = JsonHelper.ReadJsonString(item["value"].ToString(), "selectFlag");
                            if (selectFlag.Equals("0") || selectFlag == null)
                            {

                            }
                            else
                            {
                                DataColumn dc = new DataColumn(item["key"].ToString());
                                switch (JsonHelper.ReadJsonString(item["value"].ToString(), "type"))
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
                                    case "string":
                                    default:
                                        dc.DataType = typeof(String);
                                        break;
                                }
                                newDt.Columns.Add(dc);
                            }
                        }
                    }
                }
            }
            return newDt;
        }

        /// <summary>
        /// 获取简单引用数据
        /// </summary>
        /// <param name="viewId"></param>
        public DataSet GetSimpleRef(string viewId, string dataPath, string columnPath, string dependValue)
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
            string jsonData = DataTool.GetDataByPath(viewId, "[\"simpleRef\"]");
            if (!string.IsNullOrEmpty(jsonData))
            {
                string dataArrayJson = JsonHelper.ReadJsonString(jsonData, "dataArray");
                foreach (Dictionary<string, object> item in JsonHelper.ToObject<List<Dictionary<string, object>>>(dataArrayJson))
                {
                    List<SimRefPropertyModel> propertyModelList = JsonHelper.ToObject<List<SimRefPropertyModel>>(item["property"].ToString());
                    SimRefPropertyModel propertyModel = propertyModelList.FirstOrDefault(it => it.key.Equals(constructPath));
                    string dependPath = string.Empty;
                    if (propertyModel != null)
                    {
                        if (propertyModel.depend != null)
                        {
                            dependPath = propertyModelList.Find(it => it.key.Equals(propertyModel.depend)).value;
                        }
                        DataTable dt = GetDataByConsturctPath(propertyModel.value, dependPath, item["valueList"].ToString(), dependValue);
                        ds.Tables.Add(dt);
                        break;
                    }
                }
            }
            #endregion
            return ds;
        }

        private DataTable GetDataByConsturctPath(string constructPath, string dependPath, string jsonData, string dependValue)
        {
            DataTable dt = new DataTable("dataArray");
            dt.Columns.Add("txt");
            dt.Columns.Add("val");
            string[] pathList = constructPath.Split('.');
            string[] dependList = null;
            if (!string.IsNullOrEmpty(dependPath))
            {
                dependList = dependPath.Split('.');
            }
            jsonData = JsonHelper.ReadJsonString(jsonData, "dataArray");
            List<Dictionary<string, object>> valueList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonData);
            for (int i = 0; i < pathList.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (valueList[0].ContainsKey(pathList[i]))
                    {
                        valueList = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ReadJsonString(valueList[0][pathList[i]].ToString(), "dataArray"));
                        if (dependList != null && i <= dependList.Length - 1 && pathList[i].ToString().Equals(dependList[i].ToString()))
                        {
                            valueList = valueList.FindAll(it => it[dependList[dependList.Length - 1]].ToString().Equals(dependValue));
                        }
                        if (i == pathList.Length - 3)
                        {
                            foreach (Dictionary<string, object> item in valueList)
                            {
                                DataRow dr = dt.NewRow();
                                dr["txt"] = item["txt"];
                                dr["val"] = item["val"];
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            return dt;
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
                                if (dc.ExtendedProperties.ContainsKey("ColType"))
                                {
                                    if (dc.ExtendedProperties["ColType"].ToString().Equals("JObject"))
                                    {
                                        addDic.Add(dc.ColumnName, JsonHelper.ToObject<Dictionary<string, object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else if (dc.ExtendedProperties["ColType"].ToString().Equals("JArray"))
                                    {
                                        addDic.Add(dc.ColumnName, JsonHelper.ToObject<List<object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else
                                    {
                                        addDic.Add(dc.ColumnName, dr[dc.ColumnName]);
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
                                if (dc.ExtendedProperties.ContainsKey("ColType"))
                                {
                                    if (dc.ExtendedProperties["ColType"].ToString().Equals("JObject"))
                                    {
                                        modDic.Add(dc.ColumnName, JsonHelper.ToObject<Dictionary<string, object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else if (dc.ExtendedProperties["ColType"].ToString().Equals("JArray"))
                                    {
                                        modDic.Add(dc.ColumnName, JsonHelper.ToObject<List<object>>(dr[dc.ColumnName].ToString()));
                                    }
                                    else
                                    {
                                        modDic.Add(dc.ColumnName, dr[dc.ColumnName]);
                                    }
                                }
                                else
                                {
                                    modDic.Add(dc.ColumnName, dr[dc.ColumnName]);
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
    }
    internal class JsonMapKey
    {
        internal string ViewId { get; set; }
        internal string DataPath { get; set; }
    }
}
