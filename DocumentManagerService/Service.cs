
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;
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
                    serviceReceiptMessageType.Add("ServerCenterService.DownloadDocument");
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
                Dictionary<string, object> returnDic = new Dictionary<string, object>();
                try
                {
                    WebClient webClient = new WebClient();
                    if (CurrentMessageType == "ServerCenterService.DownloadDocument")
                    {
                        string path = JsonHelper.ReadJsonString(ServiceParams, "downloadToAddresss");
                        webClient.DownloadFileAsync(new Uri(JsonHelper.ReadJsonString(ServiceParams, "downloadFromAddress")), path);
                        returnDic.Add("ReplyContent", "下载成功");
                        returnDic.Add("ReplyMode", 1);
                        result = true;
                    }
                    if (CurrentMessageType == "ServerCenterService.UploadDocument")
                    {
                        webClient.UploadFileAsync(new Uri(JsonHelper.ReadJsonString(ServiceParams, "UploadToAddresss")), JsonHelper.ReadJsonString(ServiceParams, "UploadFromAddress"));
                        returnDic.Add("ReplyContent", "上传成功");
                        returnDic.Add("ReplyMode", 1);
                        result = true;
                    }

                }
                catch (Exception ex)
                {
                    returnDic.Add("ReplyContent", ex.Message);
                    returnDic.Add("ReplyMode", 0);
                    result = false;
                }
                finally
                {
                    replyContent = JsonHelper.ToJson(returnDic);
                }
                return result;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private string replyContent;
        public string ReplyContent
        {
            get { return this.replyContent; }
        }
    }
}
