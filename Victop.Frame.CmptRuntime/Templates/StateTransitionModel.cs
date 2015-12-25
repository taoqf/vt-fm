﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 状态转移实体
    /// </summary>
    public class StateTransitionModel
    {
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set;}
        /// <summary>
        /// 动作源元素
        /// </summary>
        public FrameworkElement ActionSourceElement { get; set; }
        /// <summary>
        /// 主程序页
        /// </summary>
        public TemplateControl MainView { get; set; }
    }
}
