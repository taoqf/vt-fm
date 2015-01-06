using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// 引用字段与引用数据
    /// </summary>
    public class MongoModelInfoOfClientRefPropertyModel
    {
        /// <summary>
        /// Key
        /// </summary>
        private string propertyKey;
        /// <summary>
        /// key
        /// </summary>
        [JsonProperty(PropertyName="key")]
        public string PropertyKey
        {
            get { return propertyKey; }
            set { propertyKey = value; }
        }
        /// <summary>
        /// Value
        /// </summary>
        private string propertyValue;
        /// <summary>
        /// Value
        /// </summary>
        [JsonProperty(PropertyName="value")]
        public string PropertyValue
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        }
    }
}
