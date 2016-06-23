using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Newtonsoft.Json;
using Victop.Server.Controls.Models;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;

namespace MetroFramePlugin.Models
{
    public class MenuModel : PropertyModelBase
    {
        [JsonProperty(PropertyName = "Id")]
        private string id;
        /// <summary>
        /// 标识
        /// </summary>
        [JsonIgnore]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty(PropertyName = "ParentId")]
        private string parentId;
        [JsonIgnore]
        public string ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }
        /// <summary>
        /// 关联插件名称
        /// </summary>
        [JsonProperty(PropertyName = "Package_url")]
        private string packageUrl;
        [JsonIgnore]
        public string PackageUrl
        {
            get
            {
                return packageUrl;
            }
            set
            {
                if (packageUrl != value)
                {
                    packageUrl = value;
                    RaisePropertyChanged(() => PackageUrl);
                }
            }
        }
        /// <summary>
        /// SystemId
        /// </summary>
        [JsonProperty(PropertyName = "Systemid")]
        private string systemId;
        [JsonIgnore]
        public string SystemId
        {
            get
            {
                return systemId;
            }
            set
            {
                if (systemId != value)
                {
                    systemId = value;
                    RaisePropertyChanged(() => SystemId);
                }
            }
        }

        /// <summary>
        /// 功能号
        /// </summary>
        [JsonProperty(PropertyName = "FormId")]
        private string formId;
        [JsonIgnore]
        public string FormId
        {
            get
            {
                return formId;
            }
            set
            {
                if (formId != value)
                {
                    formId = value;
                    RaisePropertyChanged(() => FormId);
                }
            }
        }
        /// <summary>
        /// 菜单编号
        /// </summary>
        [JsonProperty(PropertyName = "Menu_no")]
        private string menuNo;
        [JsonIgnore]
        public string MenuNo
        {
            get
            {
                return menuNo;
            }
            set
            {
                if (menuNo != value)
                {
                    menuNo = value;
                    RaisePropertyChanged(() => MenuNo);
                }
            }
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty(PropertyName = "Menu_name")]
        private string menuName;
        [JsonIgnore]
        public string MenuName
        {
            get
            {
                return menuName;
            }
            set
            {
                if (menuName != value)
                {
                    menuName = value;
                    RaisePropertyChanged(() => MenuName);
                }
            }
        }
        /// <summary>
        /// 插件标识
        /// </summary>
        [JsonProperty(PropertyName = "Uid")]
        private string uid;
        [JsonIgnore]
        public string Uid
        {
            get
            {
                return uid;
            }
            set
            {
                if (uid != value)
                {
                    uid = value;
                    RaisePropertyChanged(() => Uid);
                }
            }
        }
        /// <summary>
        /// 子菜单集合
        /// </summary>
        [JsonProperty(PropertyName = "Children")]
        private ObservableCollection<MenuModel> systemMenuList;
        [JsonIgnore]
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
                    RaisePropertyChanged(() => SystemMenuList);
                }
            }
        }


        /// <summary>
        /// 展示方式
        /// </summary>
        [JsonProperty(PropertyName = "Show_type")]
        private string showType;
        [JsonIgnore]
        public string ShowType
        {
            get
            {
                return showType;
            }
            set
            {
                if (showType != value)
                {
                    showType = value;
                    RaisePropertyChanged(() => ShowType);
                }
            }
        }
        /// <summary>
        /// 图标路径
        /// </summary>
        [JsonProperty(PropertyName = "Icon")]
        private string icon = "\ue495";
        [JsonIgnore]
        public string Icon
        {
            get { return icon; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    icon = value;
                    RaisePropertyChanged(() => Icon);
                }
            }
        }

        /// <summary>
        /// 插件背景色
        /// </summary>
        [JsonProperty(PropertyName = "PluginBG")]
        private string pluginBG = "#009600";
        [JsonIgnore]
        public string PluginBG
        {
            get
            {

                return pluginBG;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    pluginBG = value;
                    RaisePropertyChanged(() => PluginBG);
                }
            }
        }

        /// <summary>
        /// 插件描述
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        private string description;
        [JsonIgnore]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChanged(() => Description);
                }
            }
        }
        /// <summary>
        /// 权限码
        /// </summary>
        [JsonProperty(PropertyName = "Authoritycode")]
        private long authorityCode;
        [JsonIgnore]
        public long AuthorityCode
        {
            get
            {
                return authorityCode;
            }
            set
            {
                if (authorityCode != value)
                {
                    authorityCode = value;
                    RaisePropertyChanged(() => AuthorityCode);
                }
            }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        [JsonProperty(PropertyName = "Priority")]
        private int priority;
        [JsonIgnore]
        public int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                if (priority != value)
                {
                    priority = value;
                    RaisePropertyChanged(() => Priority);
                }
            }
        }
        /// <summary>
        /// 是否单实例
        /// </summary>
        [JsonProperty(PropertyName = "Is_single")]
        private bool isSingle;
        [JsonIgnore]
        public bool IsSingle
        {
            get
            {
                return isSingle;
            }
            set
            {
                if (isSingle != value)
                {
                    isSingle = value;
                    RaisePropertyChanged(() => IsSingle);
                }
            }
        }
        /// <summary>
        /// 是否在线运行
        /// </summary>
        [JsonProperty(PropertyName = "Is_offline")]
        private bool isOffline;
        [JsonIgnore]
        public bool IsOffline
        {
            get
            {
                return isOffline;
            }
            set
            {
                if (isOffline != value)
                {
                    isOffline = value;
                    RaisePropertyChanged(() => IsOffline);
                }
            }
        }
        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
        [JsonProperty(PropertyName = "Is_guest")]
        private bool isGuest;
        [JsonIgnore]
        public bool IsGuest
        {
            get
            {
                return isGuest;
            }
            set
            {
                if (isGuest != value)
                {
                    isGuest = value;
                    RaisePropertyChanged(() => IsGuest);
                }
            }
        }

        [JsonProperty(PropertyName = "RoleAuthList")]
        private List<MenuRoleAuth> roleAuthList;
        [JsonIgnore]
        public List<MenuRoleAuth> RoleAuthList
        {
            get
            {
                if (roleAuthList == null)
                    roleAuthList = new List<MenuRoleAuth>();
                return roleAuthList;
            }
            set
            {
                if (roleAuthList != value)
                {
                    roleAuthList = value;
                    RaisePropertyChanged(() => RoleAuthList);
                }
            }
        }


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
