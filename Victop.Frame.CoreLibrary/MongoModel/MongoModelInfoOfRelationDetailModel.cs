using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 关系从表实体
    /// </summary>
    public class MongoModelInfoOfRelationDetailModel
    {
        /// <summary>
        /// 从表表名
        /// </summary>
        private string detailTable;
        /// <summary>
        /// 从表表名
        /// </summary>
        [JsonProperty(PropertyName="table")]
        public string DetailTable
        {
            get { return detailTable; }
            set { detailTable = value; }
        }
        /// <summary>
        /// 外键
        /// </summary>
        private object detailFK;
        /// <summary>
        /// 外键
        /// </summary>
        [JsonProperty(PropertyName="fk")]
        public object DetailFK
        {
            get { return detailFK; }
            set { detailFK = value; }
        }
    }
}
