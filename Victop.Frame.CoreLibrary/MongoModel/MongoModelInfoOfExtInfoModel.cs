using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 扩展信息实体
    /// </summary>
    public class MongoModelInfoOfExtInfoModel
    {
        private string extPath;
        [JsonProperty(PropertyName = "path")]
        public string ExtPath
        {
            get
            {
                return extPath;
            }
            set
            {
                extPath = value;
            }
        }

        private List<MongoModelInfoOfExtInfoAttrModel> extAttr;
        /// <summary>
        /// 扩展属性
        /// </summary>
        [JsonProperty(PropertyName = "attr")]
        public List<MongoModelInfoOfExtInfoAttrModel> ExtAttr
        {
            get
            {
                if (extAttr == null)
                    extAttr = new List<MongoModelInfoOfExtInfoAttrModel>();
                return extAttr;
            }

            set
            {
                extAttr = value;
            }
        }
    }
}
