using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.CommonBase
{
    /// <summary>
    /// 通用插件基类
    /// </summary>
    public class CommonPluginBase
    {
        /// <summary>
        /// ConfigSystemId
        /// </summary>
        private string configSystemId;
        /// <summary>
        /// ConfigSystemId
        /// </summary>
        public string ConfigSystemId
        {
            get { return configSystemId; }
            set { configSystemId = value; }
        }

    }
}
