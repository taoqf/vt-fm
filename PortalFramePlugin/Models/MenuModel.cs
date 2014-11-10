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
    public class MenuModel:ModelBase
    {
        #region 暂未使用
        private string id;
        /// <summary>
        /// 菜单标识
        /// </summary>
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        private string endPoint;
        /// <summary>
        /// 终节点信息
        /// </summary>
        public string EndPoint
        {
            get { return endPoint; }
            set
            {
                if (endPoint != value)
                {
                    endPoint = value;
                    RaisePropertyChanged("EndPoint");
                }
            }
        }

        private string menuId;
        /// <summary>
        /// 菜单id
        /// </summary>
        public string MenuId
        {
            get { return menuId; }
            set
            {
                menuId = value;
                RaisePropertyChanged("MenuId");
            }
        }


        private string parentMenu;
        /// <summary>
        /// 父级菜单
        /// </summary>
        public string ParentMenu
        {
            get { return parentMenu; }
            set
            {
                parentMenu = value;
                RaisePropertyChanged("ParentMenu");
            }
        }

        /// <summary>
        /// 是否激活
        /// </summary>
        public int Actived
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        {
            get;
            set;
        }

        /// <summary>
        /// 自动打开标识
        /// </summary>
        public int AutoOpenFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Stamp
        {
            get;
            set;
        }

        /// <summary>
        /// 显示方式
        /// </summary>
        public int DisplayType
        {
            get;
            set;
        }

        /// <summary>
        /// 打开方式
        /// </summary>
        public int OpenType
        {
            get;
            set;
        }

        public string Compatible
        {
            get;
            set;
        }

        /// <summary>
        /// 终结点参数
        /// </summary>
        public string EndPointParam
        {
            get;
            set;
        }

        /// <summary>
        /// 主目录Id
        /// </summary>
        public string HomeId
        {
            get;
            set;
        }

        /// <summary>
        /// 队序
        /// </summary>
        public int Sequence
        {
            get;
            set;
        }
        private string formName;
        /// <summary>
        /// Form名称
        /// </summary>
        public string FormName
        {
            get
            {
                return formName;
            }
            set
            {
                if (formName != value)
                {
                    formName = value;
                    RaisePropertyChanged("FormName");
                }
            }
        }

        /// <summary>
        /// Form描述
        /// </summary>
        public string FormMemo
        {
            get;
            set;
        }

        /// <summary>
        /// 数据FormId
        /// </summary>
        public string DataFormId
        {
            get;
            set;
        }

        /// <summary>
        /// 前置文档属性
        /// </summary>
        public string PredocStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 文档状态
        /// </summary>
        public string DocStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 默认打印模版
        /// </summary>
        public string DefaultPrintTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 资源树
        /// </summary>
        public string ResourceTree
        {
            get;
            set;
        }

        /// <summary>
        /// 最大打印数量
        /// </summary>
        public string MaxPrintCount
        {
            get;
            set;
        }

        /// <summary>
        /// 保存解决方案
        /// </summary>
        public string SaveProject
        {
            get;
            set;
        }
        #endregion

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

        private string _iconUrl;

        /// <summary>
        /// 图标路径
        /// </summary>
        [JsonProperty(PropertyName = "iconUrl")]
        public string IconUrl
        {
            get { return _iconUrl; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false &&
                    value.Contains("-"))
                {
                    _iconUrl = value;
                    RaisePropertyChanged("IconUrl");
                    string[] strs = _iconUrl.Split("-".ToCharArray());
                    if (strs.Length > 0)
                    {
                        this.CheckedIconUrl = strs[0] + "-2.png";
                    }
                }
            }
        }

        private string _checkedIconUrl;

        /// <summary>
        /// 选中图标路径
        /// </summary>
        [JsonProperty(PropertyName = "checkedIconUrl")]
        public string CheckedIconUrl
        {
            get { return _checkedIconUrl; }
            set
            {
                _checkedIconUrl = value;
                RaisePropertyChanged("CheckedIconUrl");
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
        public List<Dictionary<string,object>> FitDataPath
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
