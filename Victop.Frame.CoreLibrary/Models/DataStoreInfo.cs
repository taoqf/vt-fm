using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Victop.Frame.CoreLibrary.Models
{
    /// <summary>
    /// 数据仓库信息
    /// </summary>
    public class DataStoreInfo
    {
        /// <summary>
        /// 实际数据信息
        /// </summary>
        private DataSet actualDataInfo;
        /// <summary>
        /// 实际数据信息
        /// </summary>
        public DataSet ActualDataInfo
        {
            get { return actualDataInfo; }
            set { actualDataInfo = value; }
        }

        /// <summary>
        /// 数据引用关系数据信息
        /// </summary>
        private List<RefRelationInfo> refDataInfo;
        /// <summary>
        /// 数据引用关系数据信息
        /// </summary>
        public List<RefRelationInfo> RefDataInfo
        {
            get
            {
                if (refDataInfo == null)
                {
                    refDataInfo = new List<RefRelationInfo>();
                }
                return refDataInfo;
            }
            set { refDataInfo = value; }
        }
        private int bakFlag = 0;
        /// <summary>
        /// 备份表标识
        /// </summary>
        public int BakFlag
        {
            get
            {
                return bakFlag;
            }
            set
            {
                bakFlag = value;
            }
        }
    }
}
