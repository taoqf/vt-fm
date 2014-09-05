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
                newDt = new DataTable(pathList[pathList.Count - 1].ToString());
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
                    foreach (string item in arrayList[0].Keys)
                    {
                        if (!newDt.Columns.Contains(item))
                        {
                            DataColumn dc = new DataColumn(item);
                            newDt.Columns.Add(dc);
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
                else
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
        /// 保存数据DataTable
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public bool SaveDataTable(string viewId, string dataPath)
        {
            bool editFlag = true;
            DataTable dt = new DataTable();
            foreach (JsonMapKey item in JsonTableMap.Keys)
            {
                if (item.ViewId.Equals(viewId) && item.DataPath.Equals(dataPath))
                {
                    dt = JsonTableMap[item] as DataTable;
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
                            Dictionary<string, object> pathDic = new Dictionary<string, object>();
                            pathDic.Add("key", "_id");
                            pathDic.Add("value", dr["_id"]);
                            pathList.Add(pathDic);
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
