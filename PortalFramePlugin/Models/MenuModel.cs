using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace PortalFramePlugin.Models
{
    public class MenuModel:ModelBase
    {
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

        private string bzSystemId;
        /// <summary>
        /// SystemId
        /// </summary>
        public string BzSystemId
        {
            get { return bzSystemId; }
            set
            {
                bzSystemId = value;
                RaisePropertyChanged("BzSystemId");
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
        private string menuName;
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName
        {
            get { return menuName; }
            set 
            {
                menuName = value;
                RaisePropertyChanged("MenuName");
            }
        }
        private string formId;
        /// <summary>
        /// FormId
        /// </summary>
        public string FormId
        {
            get { return formId; }
            set
            {
                formId = value;
                RaisePropertyChanged("FormId");
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
        /// 资源名称
        /// </summary>
        public string ResourceName
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
    }
}
