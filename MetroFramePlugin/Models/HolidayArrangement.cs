using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroFramePlugin.Models
{
    /// <summary>
    /// 假日安排
    /// </summary>
    class HolidayArrangement
    {
        /// <summary>
        /// 假日名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 放假时间
        /// </summary>
        public DateTime Holiday { get; set; }
        /// <summary>
        /// 放假天数
        /// </summary>
        public int NumberOfDays { get; set; }
        /// <summary>
        /// 调休上班日期
        /// </summary>
        public List<DateTime> Workday { get; set; }
    }
}
