using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.CoreLibrary.Models
{
    /// <summary>
    /// 飞道数据实体
    /// </summary>
    public class FeiDaoDBModel
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }
        /// <summary>
        /// 有效时间(时间戳)
        /// </summary>
        public long ValidDate { get; set; }
    }
}
