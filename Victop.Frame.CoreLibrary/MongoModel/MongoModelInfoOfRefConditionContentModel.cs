using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 引用条件内容实体
    /// </summary>
    public class MongoModelInfoOfRefConditionContentModel
    {
        /// <summary>
        /// 内容left
        /// </summary>
        private string contentLeft;
        /// <summary>
        /// 内容left
        /// </summary>
        [JsonProperty(PropertyName="left")]
        public string ContentLeft
        {
            get { return contentLeft; }
            set { contentLeft = value; }
        }
        /// <summary>
        /// 内容right
        /// </summary>
        private string contentRight;
        /// <summary>
        /// 内容right
        /// </summary>
        [JsonProperty(PropertyName="right")]
        public string ContentRight
        {
            get { return contentRight; }
            set { contentRight = value; }
        }
        /// <summary>
        /// 引用字段类型
        /// </summary>
        private string contentType;
        /// <summary>
        /// 引用字段类型
        /// </summary>
        [JsonProperty(PropertyName="type")]
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
    }
}
