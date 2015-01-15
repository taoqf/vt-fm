using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// ClientRef条件实体
    /// </summary>
    public class MongoModelInfoOfClientRefConditionModel
    {
        /// <summary>
        /// 条件Left
        /// </summary>
        private string conditionLeft;
        /// <summary>
        /// 提交Left
        /// </summary>
        [JsonProperty(PropertyName = "left")]
        public string ConditionLeft
        {
            get { return conditionLeft; }
            set { conditionLeft = value; }
        }
        /// <summary>
        /// 条件Right
        /// </summary>
        private string conditionRight;
        /// <summary>
        /// 条件Right
        /// </summary>
        [JsonProperty(PropertyName = "right")]
        public string ConditionRight
        {
            get { return conditionRight; }
            set { conditionRight = value; }
        }
        private string conditionData;
        [JsonProperty(PropertyName = "data")]
        public string ConditionData
        {
            get { return conditionData; }
            set { conditionData = value; }
        }
        /// <summary>
        /// 该条件是否引用字段的依赖条件，0：否；1：是
        /// </summary>
        private int conditionDependFlag = 0;
        /// <summary>
        /// 该条件是否引用字段的依赖条件，0：否；1：是
        /// </summary>
        [JsonProperty(PropertyName = "dependFlags")]
        public int ConditionDependFlag
        {
            get { return conditionDependFlag; }
            set { conditionDependFlag = value; }
        }
    }
}
