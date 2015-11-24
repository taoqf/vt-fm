using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Server.Controls.Models
{
    /// <summary>
    /// 对象属性事实实体类
    /// </summary>
    public class OAVModel
    {
        /// <summary>
        /// 对象名称
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public string AtrributeName { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public object AtrributeValue { get; set; }
    }
}
