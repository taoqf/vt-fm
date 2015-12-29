using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 状态执行实体
    /// </summary>
    public class StateExucteModel
    {
        private List<string> stateGuard;
        /// <summary>
        /// 警戒条件
        /// </summary>
        [JsonProperty(PropertyName = "guard")]
        public List<string> StateGuard
        {
            get
            {
                if (stateGuard == null)
                    stateGuard = new List<string>();
                return stateGuard;
            }
            set
            {
                stateGuard = value;
            }
        }

        private List<string> stateExit;
        /// <summary>
        /// 离开
        /// </summary>
        [JsonProperty(PropertyName = "exit")]
        public List<string> StateExit
        {
            get
            {
                if (stateExit == null)
                    stateExit = new List<string>();
                return stateExit;
            }
            set
            {
                stateExit = value;
            }
        }
        private List<string> stateEntry;
        /// <summary>
        /// 进入
        /// </summary>
        [JsonProperty(PropertyName = "entry")]
        public List<string> StateEntry
        {
            get
            {
                if (stateEntry == null)
                    stateEntry = new List<string>();
                return stateEntry;
            }
            set
            {
                stateEntry = value;
            }
        }
        private List<string> stateDo;
        /// <summary>
        /// Do
        /// </summary>
        [JsonProperty(PropertyName = "do")]
        public List<string> StateDo
        {
            get
            {
                if (stateDo == null)
                    stateDo = new List<string>();
                return stateDo;
            }
            set
            {
                stateDo = value;
            }
        }
        private List<string> stateDone;
        /// <summary>
        /// Done
        /// </summary>
        [JsonProperty(PropertyName = "done")]
        public List<string> StateDone
        {
            get
            {
                if (stateDone == null)
                    stateDone = new List<string>();
                return stateDone;
            }
            set
            {
                stateDone = value;
            }
        }
    }
}
