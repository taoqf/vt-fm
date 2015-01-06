﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// 简单引用Array实体
    /// </summary>
    public class MongoSimpleRefInfoOfArrayModel
    {
        /// <summary>
        /// 属性集合
        /// </summary>
        private List<MongoSimpleRefInfoOfArrayModel> arrayProperty;
        /// <summary>
        /// 属性集合
        /// </summary>
        [JsonProperty(PropertyName = "property")]
        public List<MongoSimpleRefInfoOfArrayModel> ArrayProperty
        {
            get
            {
                if (arrayProperty == null)
                    arrayProperty = new List<MongoSimpleRefInfoOfArrayModel>();
                return arrayProperty;
            }
            set { arrayProperty = value; }
        }
        /// <summary>
        /// 属性值集合
        /// </summary>
        private Dictionary<string, object> arrayValueList;
        /// <summary>
        /// 属性值集合
        /// </summary>
        [JsonProperty(PropertyName = "valueList")]
        public Dictionary<string, object> ArrayValueList
        {
            get
            {
                if (arrayValueList == null)
                    arrayValueList = new Dictionary<string, object>();
                return arrayValueList;
            }
            set { arrayValueList = value; }
        }
    }
}
