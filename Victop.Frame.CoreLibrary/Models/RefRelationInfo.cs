using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Frame.CoreLibrary.Enums;

namespace Victop.Frame.CoreLibrary.Models
{
    /// <summary>
    /// 引用关系信息
    /// </summary>
    public class RefRelationInfo
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id;
        /// <summary>
        /// 引用类型
        /// </summary>
        public RefTypeEnum RefType;
        /// <summary>
        /// 引用触发表
        /// </summary>
        public string TriggerTable;
        /// <summary>
        /// 行Id
        /// </summary>
        public string RowId;
        /// <summary>
        /// 引用触发字段
        /// </summary>
        public string TriggerField;
        /// <summary>
        /// 字段值
        /// </summary>
        public object FieldValue;
        /// <summary>
        /// 依赖标识
        /// </summary>
        public string DependId;
        /// <summary>
        /// 数据通道标识
        /// </summary>
        public string DataChannelId;
    }
}
