using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 简单引用信息实体
    /// </summary>
    public class MongoSimpleRefInfoModel
    {
        /// <summary>
        /// 简单引用
        /// </summary>
        private List<MongoSimpleRefInfoOfArrayModel> simpleDataArray;
        /// <summary>
        /// 简单引用
        /// </summary>
        [JsonProperty(PropertyName = "dataArray")]
        public List<MongoSimpleRefInfoOfArrayModel> SimpleDataArray
        {
            get
            {
                if (simpleDataArray == null)
                    simpleDataArray = new List<MongoSimpleRefInfoOfArrayModel>();
                return simpleDataArray;
            }
            set { simpleDataArray = value; }
        }
    }
}
