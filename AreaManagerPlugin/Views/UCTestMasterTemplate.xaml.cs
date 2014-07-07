using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Victop.Frame.Templates.DataGridTemplates;
using Victop.Frame.Templates.ReferenceTemplate;
using Victop.Wpf.Controls;

namespace AreaManagerPlugin.Views
{
    /// <summary>
    /// UCTestMasterTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class UCTestMasterTemplate : UserControl
    {
        #region 变量
        private string systemId = "905";
        private string masterName = "供应商管理";//部门管理表 供应商管理
        #endregion 

        #region 无参构造函数
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public UCTestMasterTemplate()
        {
            InitializeComponent();
            dgrid.UserControlInit(systemId, masterName, " 1=1 ");
            dgrid.AddClick += dgrid_AddClick;   //添加新行时赋默认值         
        }
        #endregion         

        #region 添加方法
        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSearchToolBarT_btnAddClick(object sender, EventArgs e)
        {
            dgrid.Add();
        }
        /// <summary>
        /// 添加新行时赋默认值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dr"></param>
        void dgrid_AddClick(object sender, DataRow dr)
        {
            dr["vndmanageguid"] = Guid.NewGuid();
            dr["vndcode"] = Guid.NewGuid();
            dr["id"] = Guid.NewGuid();
            dr["actived"] = "1";                    //是否生效（默认值：是）
        }
        #endregion 

        #region 删除方法
        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSearchToolBarT_btnDeleteClick(object sender, EventArgs e)
        {
            dgrid.Delete();
        }
        #endregion 

        #region 取消方法
        /// <summary>
        /// 取消方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSearchToolBarT_btnCancelClick(object sender, EventArgs e)
        {
            dgrid.Cancle();
            searchToolBar.CancelFlag = dgrid.CannelFlag;
        }
        #endregion 

        #region 保存方法
        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSearchToolBarT_btnSaveClick(object sender, EventArgs e)
        {
            dgrid.Save();
        }
        #endregion 

        private void searchToolBar_btnSearchClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                dgrid.SqlFilter = " vndname like '%" + txtName.Text.Trim() + "%'";
            }
            dgrid.Search();
        }
    }
}
