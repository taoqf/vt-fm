using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Server.Controls.Models
{
    public class PluginModel
    {
        private string appId;
        /// <summary>
        /// 应用程序id
        /// </summary>
        public string AppId
        {
            get { return appId; }
            set { appId = value; }
        }
        private IPlugin pluginInterface;
        /// <summary>
        /// 插件接口
        /// </summary>
        public IPlugin PluginInterface
        {
            get { return pluginInterface; }
            set { pluginInterface = value; }
        }
        private string objectId;
        /// <summary>
        /// 对象标识
        /// </summary>
        public string ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }
        private string errorMsg;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get { return errorMsg; }
            set { errorMsg = value; }
        }
    }
}
