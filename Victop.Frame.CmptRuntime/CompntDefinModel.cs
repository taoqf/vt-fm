using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 组件定义
    /// </summary>
    public class CompntDefinModel
    {
        /// <summary>
        /// 组件视图层
        /// </summary>
        [JsonProperty(PropertyName="views")]
        private object compntViews;
        /// <summary>
        /// 组件视图
        /// </summary>
        public object CompntViews
        {
            get { return compntViews; }
            set { compntViews = value; }
        }
        /// <summary>
        /// 组件展示层
        /// </summary>
        [JsonProperty(PropertyName = "presentation")]
        private object compntPresentation;
        /// <summary>
        /// 组件展示层
        /// </summary>
        public object CompntPresentation
        {
            get { return compntPresentation; }
            set { compntPresentation = value; }
        }
    }
}
