using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// dummyRelation实体类
    /// </summary>
    public class MongoModelInfoOfDummyRelationModel
    {
        /// <summary>
        /// 关系主表信息
        /// </summary>
        private MongoModelInfoOfDummyRelationMasterModel dummyRelationMaster;
        /// <summary>
        /// 关系主表信息
        /// </summary>
        [JsonProperty(PropertyName="master")]
        public MongoModelInfoOfDummyRelationMasterModel DummyRelationMaster
        {
            get
            {
                if (dummyRelationMaster == null)
                    dummyRelationMaster = new MongoModelInfoOfDummyRelationMasterModel();
                return dummyRelationMaster;
            }
            set { dummyRelationMaster = value; }
        }
        /// <summary>
        /// 关系从表信息
        /// </summary>
        private MongoModelInfoOfDummyRelationModel dummyRelationDetail;
        /// <summary>
        /// 关系从表信息
        /// </summary>
        [JsonProperty(PropertyName="detail")]
        public MongoModelInfoOfDummyRelationModel DummyRelationDetail
        {
            get
            {
                if (dummyRelationDetail == null)
                    dummyRelationDetail = new MongoModelInfoOfDummyRelationModel();
                return dummyRelationDetail;
            }
            set { dummyRelationDetail = value; }
        }
    }
}
