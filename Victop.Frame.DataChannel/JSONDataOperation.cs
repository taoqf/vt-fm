using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return true;
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
            return true;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="viewId">数据标识</param>
        /// <param name="dataPath">数据路径</param>
        /// <param name="delDataKey">删除数据的标识</param>
        /// <returns></returns>
        public virtual bool DeleteData(string viewId, string dataPath, string delDataKey)
        {
            return true;
        }
        /// <summary>
        /// 依据路径获取数据
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public virtual string GetDataByPath(string viewId, string dataPath)
        {
            return string.Empty;
        }
    }
}
