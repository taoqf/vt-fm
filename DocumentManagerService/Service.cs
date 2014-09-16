
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
                        string downloadUrl = JsonHelper.ReadJsonString(ServiceParams, "DownloadUrl");
                        string downloadToPath = JsonHelper.ReadJsonString(ServiceParams, "DownloadToPath");
                        this.DownloadFile(downloadUrl, downloadToPath);
                        returnDic.Add("ReplyContent", "下载成功");
                        returnDic.Add("ReplyMode", 1);
                        result = true;
                    }
                    if (CurrentMessageType == "ServerCenterService.UploadDocument")
                    {
                        string uploadFromPath = JsonHelper.ReadJsonString(ServiceParams, "UploadFromPath");
                        string uploadUrl = JsonHelper.ReadJsonString(ServiceParams, "UploadUrl");
                        returnDic.Add("ReplyContent", this.Upload(uploadUrl, uploadFromPath));
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

        #region 上传文件
        /// <summary> 
        /// 上传并返回所需参数
        /// </summary> 
        /// <param name="iUploadUrl">上传页面Url</param> 
        /// <param name="iUploadFromPath">文件路径</param> 
        /// <param name="iSuffname">文件后缀名</param> 
        private Dictionary<string, object> Upload(string iUploadUrl, string iUploadFromPath)
        {
            Dictionary<string, object> returnDic = new Dictionary<string, object>();
            try
            {
                if (iUploadFromPath != "")
                {
                    int i = iUploadFromPath.LastIndexOf('.');
                    string suffname = iUploadFromPath.Substring(i + 1);
                    string myrequest = UploadFile(iUploadUrl, iUploadFromPath, suffname);//返回的Jason字符串 

                    //string myrequest = "{\"1\":\"" + "1=1" + "\",\"2\":\"" + "1=0" + "1080"+",'thumbnail"; 
                    ////[{"filename":"95714ce8-1df8-4534-9f89-0c5a25a30aa9.jpg","width":1920,"height":1080,"thumbnail":[{"filename":"95714ce8-1df8-4534-9f89-0c5a25a30aa9_1.jpg","standardId":"1","width":80,"height":80},{"filename":"95714ce8-1df8-4534-9f89-0c5a25a30aa9_2.jpg","standardId":"2","width":120,"height":120},{"filename":"95714ce8-1df8-4534-9f89-0c5a25a30aa9_3.jpg","standardId":"3","width":200,"height":200}]}] 
                    //string kickString = JsonHelper.ReadJsonString(myrequest, "filename"); 
                    //string[] file = JsonHelper.ReadJsonObject<string[]>(myrequest, "filename"); 
                    //Dictionary<string, string> rqobj = JsonHelper.ToObject<Dictionary<string, string>>(split[0]); 
                    myrequest = myrequest.Replace("[", "");
                    myrequest = myrequest.Replace("]", "");
                    Match m = Regex.Match(myrequest, @"([\s\S]*?)thumbnail");
                    if (m.Success)
                    {
                        myrequest = m.Result("$1");
                        myrequest = myrequest.Substring(0, myrequest.Length - 2);//不必 
                        string[] split = myrequest.Split(':');
                        split[1] = split[1].Replace("\\", "");
                        string[] myname = split[1].Split(',');
                        string[] mywidth = split[2].Split(',');
                        string[] myhigh = split[3].Split(',');
                        string fileName = myname[0].ToString().Replace("\"", "");
                        returnDic.Add("imgurl", fileName);
                        returnDic.Add("imgname", fileName);
                        returnDic.Add("imgwidth", mywidth[0].ToString());
                        returnDic.Add("imghigh", myhigh[0].ToString());
                        returnDic.Add("imgrule", "2");
                        returnDic.Add("fileId", fileName.Substring(0, fileName.LastIndexOf(".")));
                        returnDic.Add("fileSuffix", fileName.Substring(fileName.LastIndexOf(".")));
                    }
                    else
                    {
                        Dictionary<string, string> rqobj = JsonHelper.ToObject<Dictionary<string, string>>(myrequest);
                        string fileName = rqobj["filename"].ToString();
                        returnDic.Add("imgurl", fileName);
                        returnDic.Add("imgname", fileName);
                        returnDic.Add("fileId", fileName.Substring(0, fileName.LastIndexOf(".")));
                        returnDic.Add("fileSuffix", fileName.Substring(fileName.LastIndexOf(".")));
                    }
                }
            }
            catch (System.Exception ex) { }

            return returnDic;
        }

        /// <summary> 
        /// 上传文件 
        /// </summary> 
        /// <param name="iUploadUrl">上传页面Url</param> 
        /// <param name="iUploadFromPath">文件路径</param>  
        /// <param name="iSuffname">文件后缀名</param> 
        /// <returns></returns> 
        private string UploadFile(string iUploadUrl, string iUploadFromPath, string iSuffname)
        {
            //文件 
            FileStream fileStream = new FileStream(iUploadFromPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fileStream);
            byte[] buffer = br.ReadBytes(Convert.ToInt32(fileStream.Length));

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            // mode_id=1,1,,1此处的字符串表示共上传4个文件，其中第1、2、4个文件需要按模式1做缩略图处理
            WebRequest req = WebRequest.Create(iUploadUrl);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary; //组织表单数据 
            StringBuilder sb = new StringBuilder();
            sb.Append("--" + boundary + "\r\n");
            sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + iUploadFromPath + "\"; filelength=\"" + fileStream.Length + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: " + string.Format("image/{0}", iSuffname));
            sb.Append("\r\n\r\n");
            string head = sb.ToString();
            byte[] form_data = Encoding.UTF8.GetBytes(head);

            //结尾 
            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            //post总长度 
            long length = form_data.Length + fileStream.Length + foot_data.Length;

            req.ContentLength = length;

            Stream requestStream = req.GetRequestStream();
            //这里要注意一下发送顺序，先发送form_data > buffer > foot_data 
            //发送表单参数 
            requestStream.Write(form_data, 0, form_data.Length);
            //发送文件内容 
            requestStream.Write(buffer, 0, buffer.Length);
            //结尾 
            requestStream.Write(foot_data, 0, foot_data.Length);

            requestStream.Close();
            fileStream.Close();
            fileStream.Dispose();
            br.Close();
            br.Dispose();
            //响应 
            WebResponse pos = req.GetResponse();
            StreamReader sr = new StreamReader(pos.GetResponseStream(), Encoding.UTF8);
            string html = sr.ReadToEnd().Trim();
            sr.Close();
            sr.Dispose();
            if (pos != null)
            {
                pos.Close();
                pos = null;
            }
            if (req != null)
            {
                req = null;
            }
            return html;
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="iDownloadUrl">下载Url</param>
        /// <param name="iDownloadToPath">本地保存路径</param>
        private void DownloadFile(string iDownloadUrl, string iDownloadToPath)
        {
            WebRequest request = WebRequest.Create(iDownloadUrl);

            //perform　the　GET　request
            WebResponse response = request.GetResponse();

            //get　stream　containing　received　data
            Stream s = response.GetResponseStream();

            //open　filestream　for　the　output　file
            FileStream fs = new FileStream(iDownloadToPath, FileMode.Create, FileAccess.Write);

            //copy　until　all　data　is　read　标准的缓存读取格式
            byte[] buffer = new byte[1024];
            int bytesRead = s.Read(buffer, 0, buffer.Length);
            while (bytesRead > 0)
            {
                fs.Write(buffer, 0, bytesRead);
                bytesRead = s.Read(buffer, 0, buffer.Length);
            }

            //close　both　streams
            fs.Close();
            s.Close();
            response.Close();
            if (request != null)
            {
                request = null;
            }
        }
        #endregion
    }
}
