using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.DataChannel.Enums;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.DataChannel
{
    /// <summary>
    /// JSON数据操作类
    /// </summary>
    public class JSONDataOperation
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="viewId">数据标识</param>
        /// <param name="dataPath">数据路径</param>
        /// <param name="newData">新增数据</param>
        /// <returns></returns>
        public virtual bool AddData(string viewId, string dataPath,string newData)
        {
            bool falg = false;
            try
            {
                string currentData = DataTool.GetDataByPath(viewId, dataPath);
                List<object> dataList = JsonHelper.ToObject<List<object>>(JsonHelper.ReadJsonString(currentData, "dataArray"));
                dataList.Add(newData);
                falg = DataTool.SaveCurdDataByPath(viewId, dataPath, newData, OpreateStateEnum.Added);
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("添加Json数据:{0}", ex.Message);
            }
            return falg;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="viewId">数据标识</param>
        /// <param name="dataPath">数据路径</param>
        /// <param name="newData">调整数据</param>
        /// <returns></returns>
        public virtual bool ModifyData(string viewId, string dataPath, string modifyData)
        {
            return DataTool.SaveCurdDataByPath(viewId, dataPath, modifyData, OpreateStateEnum.Modified); ;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="viewId">数据标识</param>
        /// <param name="dataPath">数据路径</param>
        /// <param name="delData">删除数据</param>
        /// <returns></returns>
        public virtual bool DeleteData(string viewId, string dataPath, string delData)
        {
            return DataTool.SaveCurdDataByPath(viewId, dataPath, delData, OpreateStateEnum.Deleted); ;
        }
    }
}
