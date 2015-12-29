using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 状态定义实体
    /// </summary>
    public class StateDefineModel
    {
        private object defEvents;
        /// <summary>
        /// 事件
        /// </summary>
        [JsonProperty(PropertyName = "events")]
        public object DefEvents
        {
            get
            {
                return defEvents;
            }
            set
            {
                defEvents = value;
            }
        }

        private object defStates;
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty(PropertyName = "states")]
        public object DefStates
        {
            get
            {
                return defStates;
            }
            set
            {
                defStates = value;
            }
        }
        private object defTransitions;
        /// <summary>
        /// 转移
        /// </summary>
        [JsonProperty(PropertyName = "transitions")]
        public object DefTransitions
        {
            get
            {
                return defTransitions;
            }
            set
            {
                defTransitions = value;
            }
        }
    }
}
