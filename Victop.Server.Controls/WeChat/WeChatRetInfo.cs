using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Server.Controls.WeChat
{
    public class WeChatRetInfo//保存登录失败微信公众平台网页返回的信息
    {
        public Dictionary<string,object> base_resp { get; set; }
        public string redirect_url { get; set; }
    }
}
