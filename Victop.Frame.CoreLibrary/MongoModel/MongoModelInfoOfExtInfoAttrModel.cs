using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 扩展信息中扩展属性实体
    /// </summary>
    public class MongoModelInfoOfExtInfoAttrModel
    {
        private string attrLeft;
        /// <summary>
        /// 属性左
        /// </summary>
        [JsonProperty(PropertyName = "left")]
        public string AttrLeft
        {
            get
            {
                return attrLeft;
            }

            set
            {
                attrLeft = value;
            }
        }
        private string attrRight;
        /// <summary>
        /// 属性右
        /// </summary>
        [JsonProperty(PropertyName = "right")]
        public string AttrRight
        {
            get
            {
                return attrRight;
            }

            set
            {
                attrRight = value;
            }
        }
        private int attrRefFlag;
        /// <summary>
        /// 属性引用标识
        /// </summary>
        [JsonProperty(PropertyName = "refFlag")]
        public int AttrRefFlag
        {
            get
            {
                return attrRefFlag;
            }
            set
            {
                attrRefFlag = value;
            }
        }
    }
}
