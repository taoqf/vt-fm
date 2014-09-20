using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Frame.DataChannel.Enums;
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
                        bool existFlag = false;
                        switch (jsonDic[item].GetType().Name)
                        {
                            case "JArray":
                                List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonDic[item].ToString());
                                if (arrayList == null || arrayList.Count == 0)
                                    continue;
                                itemDt = GetDataTableStruct(item, arrayList[0], newDs, out existFlag);
                                foreach (Dictionary<string, object> rowItem in arrayList)
                                {
                                    DataRow arrayDr = itemDt.NewRow();
                                    foreach (DataColumn dtCol in itemDt.Columns)
                                    {
                                        if (rowItem.ContainsKey(dtCol.ColumnName))
                                        {
                                            if (rowItem[dtCol.ColumnName] != null && !rowItem[dtCol.ColumnName].GetType().Name.Equals("String"))
                                            {
                                                if (!delColList.Contains(dtCol.ColumnName))
                                                    delColList.Add(dtCol.ColumnName);
                                            }
                                            arrayDr[dtCol.ColumnName] = rowItem[dtCol.ColumnName];
                                        }
                                    }
                                    itemDt.Rows.Add(arrayDr);
                                }
                                break;
                            case "JObject":
                                Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonDic[item].ToString());
                                itemDt = GetDataTableStruct(item, itemDic, newDs, out existFlag);
                                DataRow objectDr = itemDt.NewRow();
                                foreach (DataColumn dtCol in itemDt.Columns)
                                {
                                    if (itemDic.ContainsKey(dtCol.ColumnName))
                                    {
                                        if (itemDic[dtCol.ColumnName] != null && !itemDic[dtCol.ColumnName].GetType().Name.Equals("String"))
                                        {
                                            if (!delColList.Contains(dtCol.ColumnName))
                                                delColList.Add(dtCol.ColumnName);
                                        }
                                        objectDr[dtCol.ColumnName] = itemDic[dtCol.ColumnName];
                                    }
                                }
                                itemDt.Rows.Add(objectDr);
                                break;
                            default:
                                break;
                        }
                        if (delColList.Count > 0)
                        {
                            foreach (string delColItem in delColList)
                            {
                                if (itemDt.Columns.Contains(delColItem))
                                {
                                    itemDt.Columns.Remove(delColItem);
                                }
                            }
                        }
                        itemDt.AcceptChanges();
                        if (existFlag)
                        {
                            newDs.Tables.Remove(item);
                        }
                        newDs.Tables.Add(itemDt);
                    }
                    #endregion
                }
                else//获取行数据
                {
                    #region 组织行数据
                    DataTable itemDt = new DataTable();
                    bool existFlag = false;
                    itemDt = GetDataTableStruct("dataArray", jsonDic, newDs, out existFlag);
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
                    if (existFlag)
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
                newDs.Tables.Add(itemDt);
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
        /// 构建Datatable结构
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Keys"></param>
        /// <param name="newDs"></param>
        /// <returns></returns>
        private DataTable GetDataTableStruct(string item, Dictionary<string, object> rowData, DataSet newDs, out bool existFlag)
        {
            DataTable itemDt = new DataTable();
            if (newDs.Tables.Contains(item))
            {
                itemDt = newDs.Tables[item];
                existFlag = true;
            }
            else
            {
                itemDt.TableName = item;
                foreach (string colItem in rowData.Keys)
                {
                    DataColumn dc = new DataColumn(colItem);
                        itemDt.Columns.Add(dc);
                }
                existFlag = false;
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
                                if (dr[dc.ColumnName].GetType().Name.Equals("JObject"))
                                    continue;
                                addDic.Add(dc.ColumnName, dr[dc.ColumnName]);
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
                                modDic.Add(dc.ColumnName, dr[dc.ColumnName]);
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
