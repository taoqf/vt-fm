using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// 模型定义ClientRef实体
    /// </summary>
    public class MongoModelInfoOfClientRefModel
    {
        /// <summary>
        /// 引用字段
        /// </summary>
        private string clientRefField;
        /// <summary>
        /// 引用字段
        /// </summary>
        [JsonProperty(PropertyName = "refField")]
        public string ClientRefField
        {
            get { return clientRefField; }
            set { clientRefField = value; }
        }
        /// <summary>
        /// 引用字段的正则匹配查询
        /// </summary>
        private string clientRefRegStrategy;
        /// <summary>
        /// 引用字段的正则匹配查询
        /// </summary>
        [JsonProperty(PropertyName = "regStrategy")]
        public string ClientRefRegStrategy
        {
            get { return clientRefRegStrategy; }
            set { clientRefRegStrategy = value; }
        }
        /// <summary>
        /// 数据引用的ModelId
        /// </summary>
        private string clientRefModel;
        [JsonProperty(PropertyName = "refModel")]
        public string ClientRefModel
        {
            get { return clientRefModel; }
            set { clientRefModel = value; }
        }


    }
}
