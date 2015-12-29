using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 状态转移信息
    /// </summary>
    public class StateTransitionInfoModel
    {
        private string infoName;
        /// <summary>
        /// 事件名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string InfoName
        {
            get
            {
                return infoName;
            }
            set
            {
                infoName = value;
            }
        }
        private string infoFrom;
        /// <summary>
        /// 信息来源
        /// </summary>
        [JsonProperty(PropertyName = "from")]
        public string InfoFrom
        {
            get
            {
                return infoFrom;
            }
            set
            {
                infoFrom = value;
            }
        }
        private string infoTo;
        /// <summary>
        /// 信息来源
        /// </summary>
        [JsonProperty(PropertyName = "to")]
        public string InfoTo
        {
            get
            {
                return infoTo;
            }
            set
            {
                infoTo = value;
            }
        }
    }
}
