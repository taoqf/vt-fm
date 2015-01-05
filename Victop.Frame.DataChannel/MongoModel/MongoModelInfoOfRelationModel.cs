using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// 模型定义关系实体
    /// </summary>
    public class MongoModelInfoOfRelationModel
    {
        /// <summary>
        /// 关系主表信息
        /// </summary>
        private MongoModelInfoOfRelationMasterModel relationMaster;
        /// <summary>
        /// 关系主表信息
        /// </summary>
        public MongoModelInfoOfRelationMasterModel RelationMaster
        {
            get
            {
                if (relationMaster == null)
                    relationMaster = new MongoModelInfoOfRelationMasterModel();
                return relationMaster;
            }
            set
            {
                relationMaster = value;
            }
        }
        /// <summary>
        /// 关系从表信息
        /// </summary>
        private MongoModelInfoOfRelationDetailModel relationDetail;
        /// <summary>
        /// 关系从表信息
        /// </summary>
        public MongoModelInfoOfRelationDetailModel RelationDetail
        {
            get
            {
                if (relationDetail == null)
                    relationDetail = new MongoModelInfoOfRelationDetailModel();
                return relationDetail;
            }
            set { relationDetail = value; }
        }
        /// <summary>
        /// 定义从表查询模式
        /// </summary>
        private int relationDetailMode = 1;
        /// <summary>
        /// 定义从表查询模式
        /// </summary>
        public int RelationDetailMode
        {
            get { return relationDetailMode; }
            set { relationDetailMode = value; }
        } 
    }
}
