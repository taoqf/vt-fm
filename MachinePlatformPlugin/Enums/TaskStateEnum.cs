using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachinePlatformPlugin.Enums
{
    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum TaskStateEnum
    {
        未派工 = 1,
        已派工 = 2,
        已开工 = 6,
        挂起 = 3,
        已交工 = 4,
        已审核 = 5
    }
}
