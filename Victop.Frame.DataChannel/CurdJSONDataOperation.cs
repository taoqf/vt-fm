using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.DataChannel
{
    /// <summary>
    ///JSON数据操作类
    /// </summary>
    public class CurdJSONDataOperation:JSONDataOperation
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public virtual string InitData(string viewId)
        {
            return string.Empty;
        }
    }
}
