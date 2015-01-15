using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 关系主表实体
    /// </summary>
    public class MongoModelInfoOfRelationMasterModel
    {
        /// <summary>
        /// 主表名称
        /// </summary>
        private string masterTable;
        /// <summary>
        /// 主表名称
        /// </summary>
        [JsonProperty(PropertyName="table")]
        public string MasterTable
        {
            get { return masterTable; }
            set { masterTable = value; }
        }
        /// <summary>
        /// 主键
        /// </summary>
        private string masterPK;
        /// <summary>
        /// 主键
        /// </summary>
        [JsonProperty(PropertyName="pk")]
        public string MasterPK
        {
            get { return masterPK; }
            set { masterPK = value; }
        }
    }
}
