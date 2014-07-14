using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Victop.Server.Controls;

namespace DocumentManagerService
{
    public class Service : IService
    {
        /// <summary>
        /// 自动加载
        /// </summary>
        public int AutoInit
        {
            get { return 0; }
        }

        public string ServiceName
        {
            get { return "DocumentManagerService"; }
        }

        private List<string> serviceReceiptMessageType;

        public List<string> ServiceReceiptMessageType
        {
            get
            {
                if (serviceReceiptMessageType == null)
                {
                    serviceReceiptMessageType = new List<string>();
                    serviceReceiptMessageType.Add("ServerCenterService.UploadDocument");
                    serviceReceiptMessageType.Add("ServiceCenterService.DownloadDocument");
                }
                return serviceReceiptMessageType;
            }
        }

        public string CurrentMessageType
        {
            get;
            set;
        }

        public string ServiceParams
        {
            get;
            set;
        }

        public string ServiceDescription
        {
            get { return "文件管理服务"; }
        }

        public bool ServiceRun()
        {
            bool result = false;
            try
            {
                WebClient webClient = new WebClient();
                
                
            }
            catch (Exception ex)
            {
               
            }
            return result;
        }

        public string ReplyContent
        {
            get { return string.Empty; }
        }
    }
}
