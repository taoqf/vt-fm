using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.Adapter
{
    public class TimeSet
    {
        /// <summary>
        /// session超时
        /// </summary>
        public const long CLEAR_TIMEOUT = 5 * 60 * 1000;
        /// <summary>
        /// 清理Session的间隔时间
        /// </summary>
        public const long CLEAR_SLEEP_TIME = 5 * 1000;
    }
}
