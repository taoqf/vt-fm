using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Server.Controls.Models;

namespace PortalFramePlugin.Models
{
    public class MenuModel : PropertyModelBase
    {
        private string id;
        /// <summary>
        /// 标识
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string parentId;

        public string ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty(PropertyName = "menu_name")]
        private string menuName;
        /// <summary>
        /// 菜单名称
        /// </summary>
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
                    RaisePropertyChanged("MenuName");
                }
            }
        }
        /// <summary>
        /// 关联插件名称
        /// </summary>
        [JsonProperty(PropertyName = "package_url")]
        private string packageUrl;
        /// <summary>
        /// 关联插件名称
        /// </summary>
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
                    RaisePropertyChanged("PackageUrl");
                }
            }
        }
        /// <summary>
        /// SystemId
        /// </summary>
        [JsonProperty(PropertyName = "systemid")]
        private string systemId;
        /// <summary>
        /// SystemId
        /// </summary>
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
                    RaisePropertyChanged("SystemId");
                }
            }
        }
        /// <summary>
        /// 功能号
        /// </summary>
        [JsonProperty(PropertyName = "formid")]
        private string formId;
        /// <summary>
        /// 功能号
        /// </summary>
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
                    RaisePropertyChanged("FormId");
                }
            }
        }
        /// <summary>
        /// 展示方式
        /// </summary>
        [JsonProperty(PropertyName = "show_type")]
        private string showType;
        /// <summary>
        /// 展示方式
        /// </summary>
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
                    RaisePropertyChanged("ShowType");
                }
            }
        }
        /// <summary>
        /// 图标
        /// </summary>
        private string icon = "\ue7a6";
        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty(PropertyName = "icon")]
        public string Icon
        {
            get
            {
                return icon;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    icon = value;
                    RaisePropertyChanged("Icon");
                }
            }
        }

        /// <summary>
        /// 插件背景色  注：Metro的背景色，留在这儿备用
        /// </summary>
        private string pluginBG = "#009600";
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
                    RaisePropertyChanged("PluginBG");
                }  
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        private string description;
        /// <summary>
        /// 描述
        /// </summary>
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
                    RaisePropertyChanged("Description");
                }
            }
        }
        [JsonProperty(PropertyName = "authoritycode")]
        private long authorityCode;
        /// <summary>
        /// 权限码
        /// </summary>
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
                    RaisePropertyChanged("AuthorityCode");
                }
            }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        [JsonProperty(PropertyName = "priority")]
        private int priority;
        /// <summary>
        /// 优先级
        /// </summary>
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
                    RaisePropertyChanged("Priority");
                }
            }
        }
        /// <summary>
        /// 是否单实例
        /// </summary>
        [JsonProperty(PropertyName = "is_single")]
        private bool isSingle;
        /// <summary>
        /// 是否单实例
        /// </summary>
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
                    RaisePropertyChanged("IsSingle");
                }
            }
        }
        /// <summary>
        /// 是否在线运行
        /// </summary>
        [JsonProperty(PropertyName = "is_offline")]
        private bool isOffline;
        /// <summary>
        /// 是否在线运行
        /// </summary>
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
                    RaisePropertyChanged("IsOffline");
                }
            }
        }
        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
        [JsonProperty(PropertyName = "is_guest")]
        private bool isGuest;
        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
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
                    RaisePropertyChanged("IsGuest");
                }
            }
        }
        /// <summary>
        /// 子集
        /// </summary>
        [JsonProperty(PropertyName = "children")]
        private ObservableCollection<MenuModel> systemMenuList;
        /// <summary>
        /// 子菜单集合
        /// </summary>
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

        private List<MenuRoleAuth> roleAuthList;
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
                    RaisePropertyChanged("RoleAuthList");
                }
            }
        }
    }
    /// <summary>
    /// 菜单角色授权
    /// </summary>
    public class MenuRoleAuth
    {
        private long authCode;

        public long AuthCode
        {
            get { return authCode; }
            set { authCode = value; }
        }
        private string role_No;

        public string Role_No
        {
            get { return role_No; }
            set { role_No = value; }
        }
    }
}
