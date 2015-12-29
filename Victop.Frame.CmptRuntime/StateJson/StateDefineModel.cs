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
        private Dictionary<string, List<string>> defEvents;
        /// <summary>
        /// 事件
        /// </summary>
        [JsonProperty(PropertyName = "events")]
        public Dictionary<string, List<string>> DefEvents
        {
            get
            {
                if (defEvents == null)
                    defEvents = new Dictionary<string, List<string>>();
                return defEvents;
            }
            set
            {
                defEvents = value;
            }
        }

        private Dictionary<string, StateExucteModel> defStates;
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty(PropertyName = "states")]
        public Dictionary<string, StateExucteModel> DefStates
        {
            get
            {
                if (defStates == null)
                    defStates = new Dictionary<string, StateExucteModel>();
                return defStates;
            }
            set
            {
                defStates = value;
            }
        }
        private List<StateTransitionInfoModel> defTransitions;
        /// <summary>
        /// 转移
        /// </summary>
        [JsonProperty(PropertyName = "transitions")]
        public List<StateTransitionInfoModel> DefTransitions
        {
            get
            {
                if (defTransitions == null)
                    defTransitions = new List<StateTransitionInfoModel>();
                return defTransitions;
            }
            set
            {
                defTransitions = value;
            }
        }
    }
}
