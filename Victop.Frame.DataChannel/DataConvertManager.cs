using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.DataChannel
{
    /// <summary>
    /// 数据转换器
    /// </summary>
    public class DataConvertManager
    {
        /// <summary>
        /// 获取数据DataTable
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string viewId, string dataPath, DataTable structDt)
        {
            if (string.IsNullOrEmpty(dataPath))
                return null;
            string jsonData = string.Empty;
            List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
            DataTable newDt = structDt.Copy();
            newDt.Clear();
            jsonData = DataTool.GetDataByPath(viewId, dataPath);
            if (!string.IsNullOrEmpty(jsonData))
            {
                Dictionary<string, object> jsonDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonData);
                if (pathList.Count % 2 == 1)
                {
                    jsonData = jsonDic["dataArray"].ToString();
                    List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(jsonData);
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
            return newDt;
        }
    }
}
