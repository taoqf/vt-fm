using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Server.Controls.WeChat
{
    public class WeChatMenuItemModel
    {
        /// <summary>
        /// 菜单事件类型
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        /// <summary>
        /// 二级菜单
        /// </summary>
        [JsonProperty(PropertyName = "sub_button")]
        public List<WeChatMenuItemModel> SubButtons { get; set; }
    }
}
