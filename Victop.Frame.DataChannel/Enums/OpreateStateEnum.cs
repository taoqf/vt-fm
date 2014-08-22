using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.DataChannel.Enums
{
    /// <summary>
    /// 操作状态枚举
    /// </summary>
    internal enum OpreateStateEnum : int
    {
        /// <summary>
        /// 添加
        /// </summary>
        Added = 4,
        /// <summary>
        /// 修改
        /// </summary>
        Modified = 8,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 2,
        /// <summary>
        /// 无
        /// </summary>
        None = 1
    }
}
