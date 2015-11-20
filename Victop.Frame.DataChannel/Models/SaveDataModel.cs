using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.DataChannel.Enums;

namespace Victop.Frame.DataChannel.Models
{
    /// <summary>
    /// 保存数据实体
    /// </summary>
    internal class SaveDataModel
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        private OpreateStateEnum opStatus;
        /// <summary>
        /// 操作状态
        /// </summary>
        public OpreateStateEnum OpStatus
        {
            get
            {
                return opStatus;
            }

            set
            {
                opStatus = value;
            }
        }
        /// <summary>
        /// 数据路径
        /// </summary>
        private List<object> dataPath;
        /// <summary>
        /// 数据路径
        /// </summary>
        public List<object> DataPath
        {
            get
            {
                if (dataPath == null)
                    dataPath = new List<object>();
                return dataPath;
            }

            set
            {
                dataPath = value;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        private Dictionary<string, object> saveDataDic;

        /// <summary>
        /// 添加数据集合
        /// </summary>
        public Dictionary<string, object> SaveDataDic
        {
            get
            {
                if (saveDataDic == null)
                    saveDataDic = new Dictionary<string, object>();
                return saveDataDic;
            }
            set
            {
                saveDataDic = value;
            }
        }
        /// <summary>
        /// 原始数据集合
        /// </summary>
        private Dictionary<string, object> originalDataDic;
        /// <summary>
        /// 原始数据集合
        /// </summary>
        public Dictionary<string, object> OriginalDataDic
        {
            get
            {
                return originalDataDic;
            }

            set
            {
                originalDataDic = value;
            }
        }
    }
}
