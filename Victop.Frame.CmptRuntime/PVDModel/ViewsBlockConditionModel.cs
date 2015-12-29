using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// Block中条件实体
    /// </summary>
    public class ViewsBlockConditionModel
    {
        /// <summary>
        /// 表条件
        /// </summary>
        private Dictionary<string, object> tableCondition;
        /// <summary>
        /// 表条件
        /// </summary>
        [JsonProperty(PropertyName = "tablecondition")]
        public Dictionary<string, object> TableCondition
        {
            get { return tableCondition; }
            set { tableCondition = value; }
        }
        /// <summary>
        /// 表排序
        /// </summary>
        private Dictionary<string, object> tableSort;
        /// <summary>
        /// 表排序
        /// </summary>
        [JsonProperty(PropertyName = "sort")]
        public Dictionary<string, object> TableSort
        {
            get { return tableSort; }
            set { tableSort = value; }
        }
        /// <summary>
        /// 页面内容大小
        /// </summary>
        private int pageSize = 0;
        /// <summary>
        /// 页面内容大小
        /// </summary>
        [JsonProperty(PropertyName = "pagesize")]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        /// <summary>
        /// 页码
        /// </summary>
        private int pageIndex = 1;
        /// <summary>
        /// 页码
        /// </summary>
        [JsonProperty(PropertyName = "pageindex")]
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
    }
}
