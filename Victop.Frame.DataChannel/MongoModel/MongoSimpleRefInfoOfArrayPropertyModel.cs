using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    public class MongoSimpleRefInfoOfArrayPropertyModel
    {
        /// <summary>
        /// 属性键
        /// </summary>
        private string propertyKey;
        /// <summary>
        /// 属性键
        /// </summary>
        [JsonProperty(PropertyName="key")]
        public string PropertyKey
        {
            get { return propertyKey; }
            set { propertyKey = value; }
        }
        /// <summary>
        /// 属性值
        /// </summary>
        private string propertyValue;
        /// <summary>
        /// 属性值
        /// </summary>
        [JsonProperty(PropertyName="value")]
        public string PropertyValue
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        }
        /// <summary>
        /// 属性依赖项
        /// </summary>
        private string propertyDepend;
        /// <summary>
        /// 属性依赖项
        /// </summary>
        [JsonProperty(PropertyName="depend")]
        public string PropertyDepend
        {
            get { return propertyDepend; }
            set { propertyDepend = value; }
        }
    }
}
