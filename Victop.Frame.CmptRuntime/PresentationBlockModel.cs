using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    public class PresentationBlockModel
    {
        /// <summary>
        /// 区块名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        private string blockName;
        /// <summary>
        /// 区块名称
        /// </summary>
        public string BlockName
        {
            get { return blockName; }
            set { blockName = value; }
        }
        /// <summary>
        /// 上级Block名称
        /// </summary>
        [JsonProperty(PropertyName = "superiors")]
        private string superiors;
        /// <summary>
        /// 上级Block名称
        /// </summary>
        public string Superiors
        {
            get { return superiors; }
            set { superiors = value; }
        }
        /// <summary>
        /// 方法
        /// </summary>
        [JsonProperty(PropertyName = "method")]
        private string method;
        /// <summary>
        /// 方法
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        /// <summary>
        /// View层名称
        /// </summary>
        [JsonProperty(PropertyName = "view")]
        private string viewName;
        /// <summary>
        /// View层名称
        /// </summary>
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }
        /// <summary>
        /// 自动加载
        /// </summary>
        [JsonProperty(PropertyName = "autorender")]
        private bool autoRender;
        /// <summary>
        /// 自动加载
        /// </summary>
        public bool AutoRender
        {
            get { return autoRender; }
            set { autoRender = value; }
        }
        /// <summary>
        /// 绑定View中的Block名称
        /// </summary>
        [JsonProperty(PropertyName = "binding")]
        private string bindingBlock;
        /// <summary>
        /// 绑定View中的Block名称
        /// </summary>
        public string BindingBlock
        {
            get { return bindingBlock; }
            set { bindingBlock = value; }
        }
        /// <summary>
        /// 呈现层对应的视图层Block
        /// </summary>
        [JsonIgnore]
        public ViewsBlockModel ViewBlock
        {
            get;
            set;
        }
    }
}
