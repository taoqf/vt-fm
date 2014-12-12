using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Component.Runtime
{
    /// <summary>
    /// 组件定义-View实体
    /// </summary>
    public class DefinViewsModel
    {
        /// <summary>
        /// View名称
        /// </summary>
        [JsonProperty(PropertyName = "viewname")]
        public string ViewName
        {
            get;
            set;
        }
        /// <summary>
        /// 模型编号
        /// </summary>
        [JsonProperty(PropertyName = "modelid")]
        public string ModelId
        {
            get;
            set;
        }
        /// <summary>
        /// 数据文件名称
        /// </summary>
        [JsonProperty(PropertyName = "datafilename")]
        public string DataFileName
        {
            get;
            set;
        }
        /// <summary>
        /// 块集合
        /// </summary>
        [JsonProperty(PropertyName = "blocks")]
        public List<ViewsBlockModel> Blocks
        {
            get;
            set;
        }
        /// <summary>
        /// 数据通道标识
        /// </summary>
        [JsonIgnore]
        public string ViewId
        {
            get;
            set;
        }
        /// <summary>
        /// 重构View下所有Block
        /// </summary>
        public void RebuildAllDataPath()
        {
 
        }
    }
}
