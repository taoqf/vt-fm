using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MachinePlatformPlugin
{
    public class FtpUpLoadOrDownLoadHelper
    {

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="localFileName"></param>
        /// <param name="ftpPath"></param>
        /// <param name="ftpFileName"></param>
        /// <returns></returns>
        public static bool fileDownload(string localPath, string ftpPath)
        {
            bool success = false;
            WebRequest ftpWebRequest = null;
            WebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            FileStream outputStream = null;

            try
            {
                outputStream = new FileStream(localPath, FileMode.Create);

                ftpWebRequest = (WebRequest)WebRequest.Create(new Uri(ftpPath));
                ftpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                // ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                ftpWebResponse = (WebResponse)ftpWebRequest.GetResponse();

                ftpResponseStream = ftpWebResponse.GetResponseStream();
                long contentLength = ftpWebResponse.ContentLength;

                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int readCount;

                readCount = ftpResponseStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpResponseStream.Read(buffer, 0, bufferSize);
                }
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (outputStream != null)
                {
                    outputStream.Close();
                }

                if (ftpResponseStream != null)
                {
                    ftpResponseStream.Close();
                }

                if (ftpWebResponse != null)
                {
                    ftpWebResponse.Close();
                }
            }

            return success;
        }
        /// <summary>
        /// 文件替换上传
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <param name="filename">要删除的图片(文件)名称（不需要后缀）</param>
        /// <param name="contenttype">文件类型</param>
        /// <returns></returns>
        public static string ReUploadFile(string localFileName, string filename, string contenttype)
        {
            //文件
            try
            {
                FileStream fileStream = new FileStream(localFileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fileStream);
                byte[] buffer = br.ReadBytes(Convert.ToInt32(fileStream.Length));

                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                //请求
                //WebRequest req = WebRequest.Create(@"http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + accesstoken + "&type=" + type);
                WebRequest req = WebRequest.Create(@"http://192.168.40.191:8080/fsweb/reupload?delfile_name=" + filename+" &file_name = "+localFileName.Substring(localFileName.LastIndexOf("\\") + 1));//+ "," + type + "," + "" + ","+"");//mode_id=1,1,,1此处的字符串表示共上传4个文件，其中第1、2、4个文件需要按模式1做缩略图处理
                req.Method = "POST";
                req.ContentType = "multipart/form-data; boundary=" + boundary; //组织表单数据
                StringBuilder sb = new StringBuilder();
                sb.Append("--" + boundary + "\r\n");
                sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + localFileName.Substring(localFileName.LastIndexOf("\\") + 1) + "\"; filelength=\"" + fileStream.Length + "\"");
                sb.Append("\r\n");
                //sb.Append("Content-Type: " + contenttype);
                //sb.Append("Content-Type: " + "application/vnd.visio");
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
            catch
            {
                return string.Empty;
            }
        }
    }
}
