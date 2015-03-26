using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
   public class MenuModel : ModelBase,ICloneable
    {
       
        private string menuName;
        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string MenuName
        {
            get { return menuName; }
            set
            {
                menuName = value;
                RaisePropertyChanged("MenuName");
            }
        }

        private string menuCode;
        [JsonIgnore]
        public string MenuCode
        {
            get
            {
                return menuCode;
            }
            set
            {
                menuCode = value;
                RaisePropertyChanged("MenuCode");
            }
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        [JsonProperty(PropertyName = "actionName")]
        public string ResourceName
        {
            get;
            set;
        }

        private string configsystemid;
        /// <summary>
        /// configsystemid
        /// </summary>
        [JsonProperty(PropertyName = "configsystemid")]
        public string ConfigSystemId
        {
            get { return configsystemid; }
            set
            {
                configsystemid = value;
                RaisePropertyChanged("ConfigSystemId");
            }
        }

        private string bzSystemId;
        /// <summary>
        /// SystemId
        /// </summary>
        [JsonProperty(PropertyName = "systemId")]
        public string BzSystemId
        {
            get { return bzSystemId; }
            set
            {
                bzSystemId = value;
                RaisePropertyChanged("BzSystemId");
            }
        }
        private string uid;
       [JsonProperty(PropertyName = "uid")]
        public string Uid
        {
            get { return uid; }
            set
            {
                uid = value;
                RaisePropertyChanged("Uid");
            }
        }

        private ObservableCollection<MenuModel> systemMenuList;
        /// <summary>
        /// 子菜单集合
        /// </summary>
        [JsonProperty(PropertyName = "children")]
        public ObservableCollection<MenuModel> SystemMenuList
        {
            get
            {
                if (systemMenuList == null)
                    systemMenuList = new ObservableCollection<MenuModel>();
                return systemMenuList;
            }
            set
            {
                if (systemMenuList != value)
                {
                    systemMenuList = value;
                    RaisePropertyChanged("SystemMenuList");
                }
            }
        }

        #region 2014-08-28 新增（读取Json格式菜单）
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
        /// <summary>
        /// 图标路径
        /// </summary>
        private string iconUrl = "\ue61f";
        public string IconUrl
        {
            get { return iconUrl; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    iconUrl = value;
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
/// <summary>
/// 浅克隆类属性
/// </summary>
/// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

       public MenuModel Copy()
       {
           return Clone() as MenuModel;
       }
    }
}
