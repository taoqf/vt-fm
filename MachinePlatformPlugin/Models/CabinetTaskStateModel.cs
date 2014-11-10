using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachinePlatformPlugin.Models
{
    /// <summary>
    /// 机台状态实体
    /// </summary>
    public class CabinetTaskStateModel
    {
        /// <summary>
        /// 未派工
        /// </summary>
        public int UnDivideCode{get;set;}
        /// <summary>
        /// 已派工
        /// </summary>
        public int DivideCode{get;set;}
        /// <summary>
        /// 已开工
        /// </summary>
        public int StartWork{get;set;}

        /// <summary>
        /// 挂起
        /// </summary>
        public int SuspendWork { get; set; }
        /// <summary>
        /// 已交工
        /// </summary>
        public int EndWork { get; set; }
        /// <summary>
        /// 已审核
        /// </summary>
        public int EndReview { get; set; }
    }
}
