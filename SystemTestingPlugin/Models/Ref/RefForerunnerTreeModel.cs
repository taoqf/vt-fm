using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    /// <summary>
    /// 引用前导树
    /// </summary>
    public class RefForerunnerTreeModel:PropertyModelBase
    {
        /// <summary>
        /// 树Id
        /// </summary>
        private string treeId;
        /// <summary>
        /// 树Id
        /// </summary>
        public string TreeId
        {
            get
            {
                return treeId;
            }
            set
            {
                if (treeId != value)
                {
                    treeId = value;
                    RaisePropertyChanged("TreeId");
                }
            }
        }
        /// <summary>
        /// 树父节点
        /// </summary>
        private string treeParentId;
        /// <summary>
        /// 树父节点
        /// </summary>
        public string TreeParentId
        {
            get
            {
                return treeParentId;
            }
            set
            {
                if (treeParentId != value)
                {
                    treeParentId = value;
                    RaisePropertyChanged("TreeParentId");
                }
            }
        }
        /// <summary>
        /// 树展示
        /// </summary>
        private string treeDisplay;
        /// <summary>
        /// 树展示
        /// </summary>
        public string TreeDisplay
        {
            get
            {
                return treeDisplay;
            }
            set
            {
                if (treeDisplay != value)
                {
                    treeDisplay = value;
                    RaisePropertyChanged("TreeDisplay");
                }
            }
        }
        /// <summary>
        /// 树节点值
        /// </summary>
        private DataRow treeValue;
        /// <summary>
        /// 树节点值
        /// </summary>
        public DataRow TreeValue
        {
            get
            {
                return treeValue;
            }
            set
            {
                if (treeValue != value)
                {
                    treeValue = value;
                    RaisePropertyChanged("TreeValue");
                }
            }
        }
        /// <summary>
        /// 前导树信息
        /// </summary>
        private ObservableCollection<RefForerunnerTreeModel> forerunnerTreeList;
        /// <summary>
        /// 前导树信息
        /// </summary>
        public ObservableCollection<RefForerunnerTreeModel> ForerunnerTreeList
        {
            get
            {
                if (forerunnerTreeList == null)
                    forerunnerTreeList = new ObservableCollection<RefForerunnerTreeModel>();
                return forerunnerTreeList;
            }
            set
            {
                if (forerunnerTreeList != value)
                {
                    forerunnerTreeList = value;
                    RaisePropertyChanged("ForerunnerTreeList");
                }
            }
        }
    }
}
