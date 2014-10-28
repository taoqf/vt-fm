using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Server.Controls.WeChat
{
    public class WeChatOperation
    {
        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string GetMd5Str32(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            char[] temp = str.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// 微信登陆
        /// </summary>
        /// <param name="name">y用户名</param>
        /// <param name="pass">密码</param>
        /// <param name="imgcode">验证码</param>
        /// <returns></returns>
        public static bool UserLogin(string name, string pass, string imgcode)
        {
            bool result = false;
            string password = GetMd5Str32(pass).ToUpper();
            string padata = "username=" + name + "&pwd=" + password + "&imgcode=" + imgcode + "&f=json";
            string url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";//请求登录的URL
            try
            {
                CookieContainer cookieContain = new CookieContainer();//接收缓存
                string loginStr = SendMessageToWeChatServer(url, "post", padata, ref cookieContain);
                WeChatRetInfo retinfo = JsonHelper.ToObject<WeChatRetInfo>(loginStr);
                string token = string.Empty;
                if (retinfo.redirect_url != null && retinfo.redirect_url.Length > 0)
                {
                    token = retinfo.redirect_url.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得令牌
                    LoginInfo.LoginCookie = cookieContain;
                    LoginInfo.CreateDate = DateTime.Now;
                    LoginInfo.Token = token;
                    result = true;

                }
            }

            catch (Exception ex)
            {

            }
            return result;
        }
        /// <summary>
        /// 下载验证码
        /// </summary>
        /// <returns></returns>
        public static BitmapImage GetimgeCode(string strUserName)
        {

            string url = "https://mp.weixin.qq.com/cgi-bin/verifycode?username=" + strUserName + "&r=" + DateTime.Now;//请求登录的URL
            try
            {
                CookieContainer cookieContain = new CookieContainer();
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                httpRequest.CookieContainer = cookieContain;                                      //保存cookie  
                httpRequest.Method = "POST";                                          //请求方式是POST
                httpRequest.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded
                httpRequest.Referer = "https://mp.weixin.qq.com/";//request的referer地址，网络上的版本因为这句没写所以会出现invalid referrer
                Stream newStream = httpRequest.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。
                newStream.Close();
                HttpWebResponse httpResp = (HttpWebResponse)httpRequest.GetResponse();
                Stream stream = httpResp.GetResponseStream();
                byte[] myReadBuffer = new byte[8024];
                int numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                BitmapImage bmi = new BitmapImage();
                MemoryStream ms = new MemoryStream(myReadBuffer);//imgdata为从数据库中获取的byte[]数组
                bmi.BeginInit();
                bmi.StreamSource = ms;
                bmi.EndInit();
                stream.Close();
                httpResp.Close();
                return bmi;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        /// <summary>
        /// 更新App信息
        /// </summary>
        public static void UpdateAppInfo()
        {
            CookieContainer cookie = null;
            string token = null;
            cookie = LoginInfo.LoginCookie;//取得cookie
            token = LoginInfo.Token;//取得token
            /* 1.token此参数为上面的token 2.pagesize此参数为每一页显示的记录条数
            3.pageid为当前的页数，4.groupid为微信公众平台的用户分组的组id*/
            string Url = "https://mp.weixin.qq.com/advanced/advanced?action=dev&t=advanced/dev&token=" + LoginInfo.Token + "&lang=zh_CN";
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
                LoginInfo.AppId = mc[0].Value;
            }
            MatchCollection mc1;
            Regex Rex1 = new Regex("(?<=(" + "\"AppSecret\",value:\"" + "))[.\\s\\S]*?(?=(" + "\"" + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            mc1 = Rex1.Matches(text);
            if (mc.Count != 0)
            {
                LoginInfo.AppSecrect = mc1[0].Value;
            }
        }
        /// <summary>
        /// 获取所有用户组信息
        /// </summary>
        /// <returns></returns>
        public static List<SingleGroup> getAllGroupInfo()//获取所有分组数据存储在List里
        {
            try
            {
                CookieContainer cookie = null;
                //LoginInfo.Token = GetAccesstoken();
                cookie = LoginInfo.LoginCookie;//取得cookie
                string token = LoginInfo.Token;//取得token
                /* 1.token此参数为上面的token 2.pagesize此参数为每一页显示的记录条数
                3.pageid为当前的页数，4.groupid为微信公众平台的用户分组的组id*/
                string Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=10&pageidx=0&type=0&groupid=0&token=" + token + "&lang=zh_CN";
                string text = SendMessageToWeChatServer(Url, "get", null, ref cookie);
                MatchCollection mcGroup;
                Regex GroupRex = new Regex(@"(?<=""groups"":).*(?=\}\).groups)");
                mcGroup = GroupRex.Matches(text);
                List<SingleGroup> allgroupinfo = new List<SingleGroup>();
                if (mcGroup.Count != 0)
                {
                    List<Dictionary<string, object>> groupjarray = JsonHelper.ToObject<List<Dictionary<string, object>>>(mcGroup[0].Value);
                    for (int i = 0; i < groupjarray.Count; i++)
                    {
                        getEachGroupInfo(groupjarray[i]["id"].ToString(), groupjarray[i]["cnt"].ToString(), groupjarray[i]["name"].ToString(), ref allgroupinfo);
                    }
                }
                return allgroupinfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }
        /// <summary>
        /// 获取单个分组数据
        /// </summary>
        /// <param name="groupid">分组id</param>
        /// <param name="count">分组下用户数</param>
        /// <param name="group_name">分组名称</param>
        /// <param name="groupdata">分组数据</param>
        public static void getEachGroupInfo(string groupid, string count, string group_name, ref List<SingleGroup> groupdata)
        {
            CookieContainer cookie = null;
            string token = null;
            cookie = LoginInfo.LoginCookie;//取得cookie
            token = LoginInfo.Token;//取得token    
            SingleGroup obj_single = new SingleGroup();
            obj_single.group_name = group_name;
            string TotalUser;
            if (count != "0")
            {
                TotalUser = count;
            }
            else
            {
                return;
            }
            string Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=" + TotalUser + "&pageidx=0&type=0&groupid=" + groupid.Trim() + "&token=" + token + "&lang=zh_CN";
            string htmlContent = SendMessageToWeChatServer(Url, "get", null, ref cookie);
            MatchCollection mcJsonData;
            Regex rexJsonData = new Regex(@"(?<=friendsList : \({""contacts"":).*(?=}\).contacts)");
            mcJsonData = rexJsonData.Matches(htmlContent);
            if (mcJsonData.Count != 0)
            {
                List<Dictionary<string, object>> groupjarray = JsonHelper.ToObject<List<Dictionary<string, object>>>(mcJsonData[0].Value);
                obj_single.groupdata = groupjarray;
                groupdata.Add(obj_single);
            }


        }
        #region 自定义菜单管理
        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        public static string GetMenuInfo()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + GetAccesstoken();//请求的URL
            try
            {
                CookieContainer container = new CookieContainer();//接收缓存
                string htmlContent = SendMessageToWeChatServer(url, "get", null, ref container);
                return htmlContent;
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }
        /// <summary>
        /// 移除菜单信息
        /// </summary>
        /// <returns></returns>
        public static string DelMenuInfo()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + GetAccesstoken();//请求的URL
            try
            {
                CookieContainer container = new CookieContainer();//接收缓存
                string htmlContent = SendMessageToWeChatServer(url, "get", null, ref container);
                return htmlContent;
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }
        /// <summary>
        /// 创建菜单信息
        /// </summary>
        /// <returns></returns>
        public static string CreateMenuInfo(List<WeChatMenuItemModel> menuList)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + GetAccesstoken();
            Dictionary<string, object> btnDic = new Dictionary<string, object>();
            btnDic.Add("button", menuList);
            string padata = JsonHelper.ToJson(btnDic);
            CookieContainer container = new CookieContainer();//接收缓存
            string resultStr = SendMessageToWeChatServer(url, "post", padata, ref container);
            return resultStr;
        }
        #endregion
        #region 用户组管理
        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <returns></returns>
        public static string GetGroupInfo()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=" + GetAccesstoken();
            try
            {
                CookieContainer container = new CookieContainer();//接收缓存
                string htmlContent = SendMessageToWeChatServer(url, "get", null, ref container);
                return htmlContent;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <returns></returns>
        public static string GetAllUserInfo()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + GetAccesstoken();

            try
            {
                CookieContainer container = new CookieContainer();//接收缓存
                string padata = null;
                string htmlContent = SendMessageToWeChatServer(url, "get", padata, ref container);
                return htmlContent;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 根据OpenId获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string GetUserInfoByOpenId(string openId)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + GetAccesstoken() + "&openid=" + openId + "&lang=zh_CN";
            try
            {
                CookieContainer container = new CookieContainer();
                string htmlContent = SendMessageToWeChatServer(url, "get", null, ref container);
                return htmlContent;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion
        /// <summary>
        /// 获取访问Token
        /// </summary>
        /// <returns></returns>
        public static string GetAccesstoken()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + LoginInfo.AppId + "&secret=" + LoginInfo.AppSecrect;//请求登录的URL
            string token = string.Empty;
            try
            {
                CookieContainer container = new CookieContainer();//接收缓存
                string tokenStr = SendMessageToWeChatServer(url, "post", null, ref container);
                token = JsonHelper.ReadJsonString(tokenStr.ToString(), "access_token");
            }
            catch (Exception ex)
            {
            }
            return token;

        }

        /// <summary>
        /// 向weChat服务发送消息
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="reqMethod">请求方式</param>
        /// <param name="reqParams">请求参数</param>
        /// <returns></returns>
        private static string SendMessageToWeChatServer(string url, string reqMethod, string reqParams, ref CookieContainer cookieContain)
        {
            string resultStr = string.Empty;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                webRequest.CookieContainer = cookieContain;                                      //保存cookie 
                webRequest.Method = reqMethod.ToUpper();                                          //请求方式
                webRequest.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded
                if (!string.IsNullOrEmpty(reqParams))
                {
                    webRequest.ContentLength = Encoding.UTF8.GetBytes(reqParams).Length;
                }
                switch (reqMethod.ToUpper())
                {
                    case "POST":
                        webRequest.Referer = "https://mp.weixin.qq.com/";//request的referer地址，网络上的版本因为这句没写所以会出现invalid referrer
                        Stream newStream = webRequest.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。
                        if (!string.IsNullOrEmpty(reqParams))
                        {
                            byte[] byteArray = Encoding.UTF8.GetBytes(reqParams); // 转化
                            newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                        }
                        newStream.Close();
                        break;
                    case "GET":
                        webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
                        break;
                }
                HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader stream = new StreamReader(httpResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                resultStr = stream.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return resultStr;
        }
    }
}
