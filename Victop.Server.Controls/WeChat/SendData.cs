using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Server.Controls.WeChat
{
    public static class SendData
    {
        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string GetMd5Str32(string str) //MD5摘要算法
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.  
            char[] temp = str.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data   
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.  
            return sBuilder.ToString();
        }

        public static string AppId = string.Empty;
        public static string AppSecrect = string.Empty;
        public static void SetAppIdAndAppSecrect()
        {

            CookieContainer cookie = null;
            string token = null;
            cookie = SendData.LoginInfo.LoginCookie;//取得cookie
            token = SendData.LoginInfo.Token;//取得token
            /* 1.token此参数为上面的token 2.pagesize此参数为每一页显示的记录条数
            3.pageid为当前的页数，4.groupid为微信公众平台的用户分组的组id*/
            string Url = "https://mp.weixin.qq.com/advanced/advanced?action=dev&t=advanced/dev&token=" + SendData.LoginInfo.Token + "&lang=zh_CN";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);//Url为获取用户信息的链接
            webRequest.CookieContainer = cookie;
            webRequest.ContentType = "text/html; charset=UTF-8";
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string text = sr.ReadToEnd();
            MatchCollection mc;
            Regex Rex = new Regex("(?<=(" + "\"AppId\",value:\"" + "))[.\\s\\S]*?(?=(" + "\"" + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            mc = Rex.Matches(text);
            if (mc.Count != 0)
            {
                SendData.AppId = mc[0].Value;
            }
            MatchCollection mc1;
            Regex Rex1 = new Regex("(?<=(" + "\"AppSecret\",value:\"" + "))[.\\s\\S]*?(?=(" + "\"" + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            mc1 = Rex1.Matches(text);
            if (mc.Count != 0)
            {
                SendData.AppSecrect = mc1[0].Value;
            }
        }

        public static bool ExecLogin(string name, string pass, string imgcode)
        {

            bool result = false;
            string password = GetMd5Str32(pass).ToUpper();
            string padata = "username=" + name + "&pwd=" + password + "&imgcode=" + imgcode + "&f=json";
            string url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";//请求登录的URL
            CookieContainer cc = new CookieContainer();
            try
            {

                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                webRequest2.CookieContainer = cc;                                      //保存cookie 
                webRequest2.Method = "POST";                                          //请求方式是POST
                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded
                webRequest2.Referer = "https://mp.weixin.qq.com/";//request的referer地址，网络上的版本因为这句没写所以会出现invalid referrer
                webRequest2.ContentLength = byteArray.Length;
                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。
                // Send the data.
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);
                string text2 = sr2.ReadToEnd();
                //此处用到了newtonsoft来序列化
                WeiXinRetInfo retinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiXinRetInfo>(text2);
                string token = string.Empty;
                if (retinfo.redirect_url != null && retinfo.redirect_url.Length > 0)
                {

                    token = retinfo.redirect_url.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得令牌

                    LoginInfo.LoginCookie = cc;

                    LoginInfo.CreateDate = DateTime.Now;

                    LoginInfo.Token = token;

                    result = true;

                }

            }

            catch (Exception ex)
            {
                //throw new Exception(ex.StackTrace);

            }
            return result;
        }
        //public static CookieContainer cc = new CookieContainer();//接收缓存
        public static BitmapImage GetimgeCode()
        {

            string url = "https://mp.weixin.qq.com/cgi-bin/verifycode?username=szbenyukeji@163.com&r=" + DateTime.Now;//请求登录的URL

            try
            {
                //byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                CookieContainer cc = new CookieContainer();
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.Referer = "https://mp.weixin.qq.com/";//request的referer地址，网络上的版本因为这句没写所以会出现invalid referrer

                //webRequest2.ContentLength = byteArray.Length;

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                //newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                Stream stream = response2.GetResponseStream();


                byte[] myReadBuffer = new byte[8024];
                int numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                //此处用到了newtonsoft来序列化
                BitmapImage bmi = new BitmapImage();

                MemoryStream ms = new MemoryStream(myReadBuffer);//imgdata为从数据库中获取的byte[]数组
                bmi.BeginInit();
                bmi.StreamSource = ms;
                bmi.EndInit();
                stream.Close();
                response2.Close();
                return bmi;
            }

            catch (Exception ex)
            {
                //throw new Exception(ex.StackTrace);

            }
            return null;
        }
        public class WeiXinRetInfo//网络上是另一个版本，微信更新后换结构了
        {

            public base_resp base_resp { get; set; }

            public string redirect_url { get; set; }

        }
        public static class LoginInfo//保存登陆后返回的信息
        {
            /// <summary>
            /// 登录后得到的令牌
            /// </summary>        
            public static string Token { get; set; }
            /// <summary>
            /// 登录后得到的cookie
            /// </summary>
            public static CookieContainer LoginCookie { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public static DateTime CreateDate { get; set; }

            public static string Err { get; set; }


        }

        public static string AccessToken = string.Empty;

        public class base_resp
        {

            public string ret { get; set; }

            public string err_msg { get; set; }

        }

        public static string GetAccesstoken()
        {

            bool result = false;
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecrect;//请求登录的URL
            string token = string.Empty;
            try
            {

                CookieContainer cc = new CookieContainer();//接收缓存

                //byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化

                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                //webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                //newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化


                //WeiXinRetInfo retinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiXinRetInfo>(text2);

                token = JsonHelper.ReadJsonString(text2.ToString(), "access_token");

                //if (retinfo.ErrMsg.Length > 0)
                //{

                //    if (retinfo.ErrMsg.Contains("ok"))
                //    {

                //        token = retinfo.ErrMsg.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得token

                //        LoginInfo.LoginCookie = cc;

                //        LoginInfo.CreateDate = DateTime.Now;

                //        LoginInfo.Token = token;

                //        result = true;

                //    }

                //    else
                //    {


                //        result = false;

                //    }

                //}

            }

            catch (Exception ex)
            {

            }
            AccessToken = token;
            return token;

        }

        public static bool PostAddMenuBody(string json)
        {

            bool result = false;


            string padata = json;

            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();//接收缓存

                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化

                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                result = true;
            }

            catch (Exception ex)
            {


                throw new Exception(ex.StackTrace);

            }

            return result;

        }

        public static string GetMenuBody()
        {

            string url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();//接收缓存

                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "get";                                          //请求方式是POST
                webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                return text2;
            }

            catch (Exception ex)
            {
            }

            return string.Empty;

        }

        public static bool DelMenuBody()
        {

            string url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();//接收缓存

                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "get";                                          //请求方式是POST
                webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化
                string relst = JsonHelper.ReadJsonString(text2.ToString(), "errmsg");
                if (relst == "ok")
                {
                    return true;
                }
                return false;
            }

            catch (Exception ex)
            {

            }

            return false;

        }

        public static string GetgroupsBody()
        {

            string url = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {



                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = LoginInfo.LoginCookie;                                      //保存cookie  

                webRequest2.Method = "get";                                          //请求方式是POST
                webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化
                //string relst = JsonHelper.ReadJsonString(text2.ToString(), "errmsg");

                return text2;
            }

            catch (Exception ex)
            {

            }

            return string.Empty;

        }

        public static bool CreategroupsBody(string json)
        {
            bool result = false;
            string url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();
                string padata = json;
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                result = true;
            }

            catch (Exception ex)
            {

            }

            return result;

        }

        public static bool UpdategroupsBody(string json)
        {
            bool result = false;
            string url = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();
                string padata = json;
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                result = true;
            }

            catch (Exception ex)
            {

            }

            return result;

        }


        public static string GetusersBody()
        {

            string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + SendData.AccessToken + "&next_openid=";//请求登录的URL

            try
            {



                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = LoginInfo.LoginCookie;                                      //保存cookie  

                webRequest2.Method = "get";                                          //请求方式是POST
                webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化
                //string relst = JsonHelper.ReadJsonString(text2.ToString(), "errmsg");

                return text2;
            }

            catch (Exception ex)
            {

            }

            return string.Empty;

        }

        public static string GetuserInfoBody(string OPENID)
        {

            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + SendData.AccessToken + "&openid=" + OPENID + "&lang=zh_CN";//请求登录的URL

            try
            {



                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = LoginInfo.LoginCookie;                                      //保存cookie  

                webRequest2.Method = "get";                                          //请求方式是POST
                webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化
                //string relst = JsonHelper.ReadJsonString(text2.ToString(), "errmsg");

                return text2;
            }

            catch (Exception ex)
            {

            }

            return string.Empty;

        }

        public static bool setuserInfoBody(string json)
        {
            bool result = false;
            string url = "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();
                string padata = json;
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                string text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                result = true;
            }

            catch (Exception ex)
            {

            }

            return result;

        }

        public static string upLoadnewsBody(string json)
        {

            string url = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=" + SendData.AccessToken;//请求登录的URL
            string text2 = string.Empty;
            try
            {

                CookieContainer cc = new CookieContainer();
                string padata = json;
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                return text2;
            }

            catch (Exception ex)
            {

            }

            return text2;

        }



        /// <summary>
        /// 服务号：上传多媒体文件
        /// </summary>
        /// <param name="accesstoken">调用接口凭据</param>
        /// <param name="type">图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="filename">文件路径</param>
        /// <param name="contenttype">文件Content-Type类型(例如：image/jpeg、audio/mpeg)</param>
        /// <returns></returns>
        public static string upLoadthumbBody(string accesstoken, string type, string filename, string contenttype)
        {
            //文件
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fileStream);
            byte[] buffer = br.ReadBytes(Convert.ToInt32(fileStream.Length));

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //请求
            WebRequest req = WebRequest.Create(@"http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + accesstoken + "&type=" + type);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            //组织表单数据
            StringBuilder sb = new StringBuilder();
            sb.Append("--" + boundary + "\r\n");
            sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + filename + "\"; filelength=\"" + fileStream.Length + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: " + contenttype);
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


        public static string GetmediaBody(string MEDIA_ID, string path)
        {

            string url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + SendData.AccessToken + "&media_id=" + MEDIA_ID;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();//接收缓存

                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "get";                                          //请求方式是POST
                webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                Stream sr2 = response2.GetResponseStream();

                FileStream fs = new FileStream(path, FileMode.CreateNew);
                byte[] buffer = new byte[1024];
             
                int l;

                do
                {

                    l = sr2.Read(buffer, 0, buffer.Length);

                    if (l > 0)

                        fs.Write(buffer, 0, l);

                }

                while (l > 0);


                fs.Close();

                sr2.Close();




            }

            catch (Exception ex)
            {
            }

            return string.Empty;

        }

        public static string SendMessgetoUser(string json)
        {

            string text2 = string.Empty;


            string padata = json;

            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();//接收缓存

                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化

                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                return text2;
            }

            catch (Exception ex)
            {


                throw new Exception(ex.StackTrace);

            }

            return text2;
        }

        public static string CreateCode(string json)
        {
            string text2 = string.Empty;


            string padata = json;

            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + SendData.AccessToken;//请求登录的URL

            try
            {

                CookieContainer cc = new CookieContainer();//接收缓存

                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化

                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url

                webRequest2.CookieContainer = cc;                                      //保存cookie  

                webRequest2.Method = "POST";                                          //请求方式是POST

                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded

                webRequest2.ContentLength = byteArray.Length;

                webRequest2.Referer = "https://mp.weixin.qq.com/";

                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。

                // Send the data.

                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数

                newStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

                text2 = sr2.ReadToEnd();

                //此处用到了newtonsoft来序列化

                return text2;
            }

            catch (Exception ex)
            {


                throw new Exception(ex.StackTrace);

            }

            return text2;
        }
    }

}
