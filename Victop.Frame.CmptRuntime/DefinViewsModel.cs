using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 组件定义视图层实体
    /// </summary>
    public class DefinViewsModel
    {
        /// <summary>
        /// 视图名称
        /// </summary>
        [JsonProperty(PropertyName = "viewname")]
        private string viewName;
        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }
        /// <summary>
        /// 视图模型
        /// </summary>
        [JsonProperty(PropertyName = "modelid")]
        private string modelId;
        /// <summary>
        /// 视图模型
        /// </summary>
        public string ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }
        /// <summary>
        /// 加载数据方式 0不加载, 1带缓存, 2不使用缓存 
        /// </summary>
        [JsonProperty(PropertyName = "loaddata")]
        private int loadData;
        /// <summary>
        /// 加载数据方式 0不加载, 1带缓存, 2不使用缓存 
        /// </summary>
        public int LoadData
        {
            get { return loadData; }
            set { loadData = value; }
        }
        /// <summary>
        /// 是否编辑
        /// </summary>
        [JsonProperty(PropertyName = "editable")]
        private bool editAble;
        /// <summary>
        /// 是否编辑
        /// </summary>
        public bool EditAble
        {
            get { return editAble; }
            set { editAble = value; }
        }

        /// <summary>
        /// 视图Block
        /// </summary>
        [JsonProperty(PropertyName = "blocks")]
        private List<object> viewBlocks;
        /// <summary>
        /// 视图Block
        /// </summary>
        public List<object> ViewBlocks
        {
            get { return viewBlocks; }
            set { viewBlocks = value; }
        }
    }
}
