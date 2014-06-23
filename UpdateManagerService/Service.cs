using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls;

namespace UpdateManagerService
{
    public class Service:IService
    {
        /// <summary>
        /// 自动加载
        /// </summary>
        public int AutoInit
        {
            get { return 0; }
        }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get { return "UpdateManagerService"; }
        }
        private List<string> serviceReceiptMessageType;
        /// <summary>
        /// 服务接收消息类型
        /// </summary>
        public List<string> ServiceReceiptMessageType
        {
            get
            {
                if (serviceReceiptMessageType == null)
                {
                    serviceReceiptMessageType = new List<string>();
                    //TODO:添加消息类型
                }
                return serviceReceiptMessageType;
            }
        }

        /// <summary>
        /// 当前消息类型
        /// </summary>
        public string CurrentMessageType
        {
            get;
            set;
        }

        /// <summary>
        /// 服务参数
        /// </summary>
        public string ServiceParams
        {
            get;
            set;
        }
        /// <summary>
        /// 服务描述
        /// </summary>
        public string ServiceDescription
        {
            get { return "更新管理服务"; }
        }
        /// <summary>
        /// 服务运行
        /// </summary>
        /// <returns></returns>
        public bool ServiceRun()
        {
            //TODO:开启更新的独立线定时向更新服务发送请求获取应用程序(框架/插件)版本，若有新版本则启动内置
            //更新插件启动
            throw new NotImplementedException();
        }
    }
}
