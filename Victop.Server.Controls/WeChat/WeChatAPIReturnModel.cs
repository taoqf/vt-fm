using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Server.Controls.WeChat
{
    public class WeChatAPIReturnModel
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        [JsonProperty(PropertyName = "errcode")]
        public string ErrorCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [JsonProperty(PropertyName = "errmsg")]
        public string ErrorMsg { get; set; }
    }
}
