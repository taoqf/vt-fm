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
        [JsonProperty(PropertyName = "tablecondition")]
        private Dictionary<string, object> tableCondition;
        /// <summary>
        /// 表条件
        /// </summary>
        public Dictionary<string, object> TableCondition
        {
            get { return tableCondition; }
            set { tableCondition = value; }
        }
        /// <summary>
        /// 表排序
        /// </summary>
        [JsonProperty(PropertyName = "sort")]
        private Dictionary<string, object> tableSort;
        /// <summary>
        /// 表排序
        /// </summary>
        public Dictionary<string, object> TableSort
        {
            get { return tableSort; }
            set { tableSort = value; }
        }
        /// <summary>
        /// 页面内容大小
        /// </summary>
        [JsonProperty(PropertyName = "pagesize")]
        private int pageSize = -1;
        /// <summary>
        /// 页面内容大小
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        /// <summary>
        /// 页码
        /// </summary>
        [JsonProperty(PropertyName="pageindex")]
        private int pageIndex = 1;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
    }
}
