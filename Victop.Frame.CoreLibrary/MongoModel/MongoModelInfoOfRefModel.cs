using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 模型定义视图引用实体
    /// </summary>
    public class MongoModelInfoOfRefModel
    {
        /// <summary>
        /// 引用条件
        /// </summary>
        private List<MongoModelInfoOfRefConditionContentModel> refCondition;
        /// <summary>
        /// 引用条件
        /// </summary>
        [JsonProperty(PropertyName = "condition")]
        public List<MongoModelInfoOfRefConditionContentModel> RefCondition
        {
            get
            {
                if (refCondition == null)
                    refCondition = new List<MongoModelInfoOfRefConditionContentModel>();
                return refCondition;
            }
            set { refCondition = value; }
        }
        /// <summary>
        /// 引用View
        /// </summary>
        private List<MongoModelInfoOfRefConditionContentModel> refView;
        /// <summary>
        /// 引用View
        /// </summary>
        [JsonProperty(PropertyName="view")]
        public List<MongoModelInfoOfRefConditionContentModel> RefView
        {
            get
            {
                if (refView == null)
                    refView = new List<MongoModelInfoOfRefConditionContentModel>();
                return refView;
            }
            set { refView = value; }
        }
    }
}
