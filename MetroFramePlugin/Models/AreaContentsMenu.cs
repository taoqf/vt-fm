using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    public class AreaContentsMenu : ModelBase
    {
        #region （读取Json格式菜单）
        /// <summary>
        /// 插件类型（0：组件||1：插件）
        /// </summary>
        [JsonProperty(PropertyName = "actionType")]
        public string ActionType
        {
            get;
            set;
        }
        /// <summary>
        /// 展示方式（0：窗口||1：UserControl）
        /// </summary>
        [JsonProperty(PropertyName = "showType")]
        public string ShowType
        {
            get;
            set;
        }


        private string menuName;
        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty(PropertyName = "menuName")]
        public string MenuName
        {
            get { return menuName; }
            set
            {
                menuName = value;
                RaisePropertyChanged("MenuName");
            }
        }
        /// <summary>
        /// 图标路径
        /// </summary>
        [JsonProperty(PropertyName = "iconUrl")]
        private string _iconUrl = "\ue6a4";
        public string IconUrl
        {
            get { return _iconUrl; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    _iconUrl = value;
                    RaisePropertyChanged("IconUrl");
                }
            }
        }
      
        /// <summary>
        /// 插件描述
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// spaceId
        /// </summary>
        [JsonProperty(PropertyName = "spaceId")]
        public string SpaceId
        {
            get;
            set;
        }
        /// <summary>
        /// menuno
        /// </summary>
        [JsonProperty(PropertyName = "menuno")]
        public string MenuNo
        {
            get;
            set;
        }
        /// <summary>
        /// 装配数据路径
        /// </summary>
        [JsonProperty(PropertyName = "fitDataPath")]
        public List<Dictionary<string, object>> FitDataPath
        {
            get;
            set;
        }
        /// <summary>
        /// CAD名称
        /// </summary>
        [JsonProperty(PropertyName = "actionCADName")]
        public string ActionCADName
        {
            get;
            set;
        }
        #endregion
    }
}
