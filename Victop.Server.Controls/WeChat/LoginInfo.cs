using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Victop.Server.Controls.WeChat
{
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

        /// <summary>
        /// AppId
        /// </summary>
        public static string AppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string AppSecrect { get; set; }
    }
}
