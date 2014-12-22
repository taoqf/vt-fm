using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// IP操作辅助类
    /// </summary>
    public class IPOperationHelper
    {
        /// <summary>
        /// 获取公网IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetPublicNetWorkIp()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://www.ip138.com/ips138.asp");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据

                int start = all.IndexOf("您的IP地址是：[") + 9;
                int end = all.IndexOf("]", start);
                tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return tempip;
        }
    }
}
