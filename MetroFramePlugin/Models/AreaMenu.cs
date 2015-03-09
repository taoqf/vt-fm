using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    public class AreaMenu:ModelBase
    {
        [JsonProperty(PropertyName = "areaName")]
        private string areaName = "我的常用功能";
        public string AreaName
        {
            get { return areaName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    areaName = value;
                    RaisePropertyChanged("AreaName");
                }
            }
        }
        [JsonProperty(PropertyName = "oftenFun")]
        private string areaTitle = "oftenFun";
        public string AreaTitle
        {
            get { return areaTitle; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    areaTitle = value;
                    RaisePropertyChanged("AreaTitle");
                }
            }
        }
        /// <summary>
        ///区域插件展示方式（大、中、小）
        /// </summary>
        [JsonProperty(PropertyName = "menuForm")]
        private string menuForm = "normal";
        public string MenuForm
        {
            get { return menuForm; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    menuForm = value;
                    RaisePropertyChanged("MenuForm");
                }
            }
        }
        /// <summary>
        ///区域宽度
        /// </summary>
        [JsonProperty(PropertyName = "areaWidth")]
        private string areaWidth;
        public string AreaWidth
        {
            get { return areaWidth; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    areaWidth = value;
                    RaisePropertyChanged("AreaWidth");
                }
            }
        }
        /// <summary>
        ///区域高度
        /// </summary>
        [JsonProperty(PropertyName = "areaHeight")]
        private string areaHeight;
        public string AreaHeight
        {
            get { return areaHeight; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    areaHeight = value;
                    RaisePropertyChanged("AreaHeight");
                }
            }
        }
        /// <summary>
        /// 插件列表
        /// </summary>
        [JsonProperty(PropertyName = "pluginList")]
        public List<Dictionary<string, object>> PluginList
        {
            get;
            set;
        }
    }
}
