using NRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 基类规则
    /// </summary>
    public abstract class BaseRule : NRules.Fluent.Dsl.Rule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseRule()
        {
        }
    }
}
