using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using Victop.Frame.CmptRuntime.AtomicOperation;
using System.Windows.Data;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 飞道原子操作映射类
    /// </summary>
    public class FeiDaoOperation
    {
        private TemplateControl MainView;

        #region 分类原子操作实例
        /// <summary>
        /// 自定义服务
        /// </summary>
        private CustomServiceAtOperation customServiceOperation;
        /// <summary>
        /// 数据操作
        /// </summary>
        private DataAtOperation dataOperation;
        /// <summary>
        /// 系统原子操作
        /// </summary>
        private SystemAtOperation systemOperation;
        #endregion
        /// <summary>
        /// 构造函数，页面/组件实体
        /// </summary>
        /// <param name="mainView"></param>
        public FeiDaoOperation(TemplateControl mainView)
        {
            MainView = mainView;
            customServiceOperation = new CustomServiceAtOperation(MainView);
            dataOperation = new DataAtOperation(MainView);
            systemOperation = new SystemAtOperation(MainView);
        }
        #region 自定义服务操作
        /// <summary>
        /// 服务参数设置
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        public void ServiceParamSet(string serviceName, string paramName, object paramValue)
        {
            customServiceOperation.ServiceParamSet(serviceName, paramName, paramValue);
        }
        /// <summary>
        /// 发送服务消息
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void SendServiceMessage(string serviceName)
        {
            customServiceOperation.SendServiceMessage(serviceName);
        }
        #endregion

        #region 查询条件操作
        /// <summary>
        /// 设置区块查询条件日期区间
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        public void SetConditionSearchDate(string pBlockName, string paramField, string startDate, string endDate)
        {
            dataOperation.SetConditionSearchDate(pBlockName, paramField, startDate, endDate);
        }
        /// <summary>
        /// 设置区块查询条件模糊匹配
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="paramValue">参数值</param>
        public void SetConditionSearchLike(string pBlockName, string paramField, object paramValue)
        {
            dataOperation.SetConditionSearchLike(pBlockName, paramField, paramValue);
        }
        /// <summary>
        /// 设置区块查询条件子查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="oav">事实</param>
        public void SetConditionSearchIn(string pBlockName, string paramField, OAVModel oav)
        {
            dataOperation.SetConditionSearchIn(pBlockName, paramField, oav);
        }
        /// <summary>
        /// 设置区块查询条件
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="paramValue">参数值</param>
        public void SetConditionSearch(string pBlockName, string paramField, object paramValue)
        {
            dataOperation.SetConditionSearch(pBlockName, paramField, paramValue);
        }

        /// <summary>
        /// 设置区块排序
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="sort">排序方式1正序-1倒序</param>
        public void SetConditionSort(string pBlockName, string paramField, int sort)
        {
            dataOperation.SetConditionSort(pBlockName, paramField, sort);
        }
        /// <summary>
        /// 设置区块分页
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="index">页码</param>
        /// <param name="size">条数</param>
        public void SetConditionPaging(string pBlockName, int index, int size)
        {
            dataOperation.SetConditionPaging(pBlockName, index, size);
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public void SearchData(string pBlockName)
        {
            dataOperation.SearchData(pBlockName);
        }
        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="btnName"></param>
        /// <param name="btnContent"></param>
        public void SetButtonText(string btnName, string btnContent)
        {
            Button btn = MainView.FindName(btnName) as Button;
            btn.Content = btnContent;
        }
        /// <summary>
        /// 设置选中数据通过datagrid
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="dgrid"></param>
        public void SetPBlockCurrentRowByDataGrid(string blockName, object dgrid)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            VicDataGrid datagrid = dgrid as VicDataGrid;
            if (pBlock != null && pBlock.ViewBlockDataTable.Rows.Count > 0 && datagrid != null && datagrid.SelectedItem != null)
            {
                pBlock.PreBlockSelectedRow = ((DataRowView)datagrid.SelectedItem).Row;
            }
        }
        /// <summary>
        /// 系统输出
        /// </summary>
        /// <param name="consoleText"></param>
        public void SysConsole(string consoleText)
        {
            systemOperation.SysConsole(consoleText);
        }
        /// <summary>
        /// 获取BlockData的数据
        /// </summary>
        /// <param name="blockName"></param>
        public void GetPBlockData(string blockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            pBlock.GetData();
        }
        /// <summary>
        /// 提交BlockData的数据
        /// </summary>
        /// <param name="blockName"></param>
        public void SavePBlockData(string blockName)
        {
            dataOperation.SavePBlockData(blockName);
        }
        /// <summary>
        /// 设置block选中行数据
        /// </summary>
        /// <param name="blockName"></param>
        public void SetPBlockCurrentRow(string blockName)
        {
            dataOperation.SetPBlockCurrentRow(blockName);
        }
        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="blockName"></param>
        public void PBlockAddRow(string blockName)
        {
            dataOperation.PBlockAddRow(blockName);
        }
        /// <summary>
        /// 新增行参数
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="fieldName"></param>
        /// <param name="paramValue"></param>
        public void PBlockAddRowParam(string blockName, string fieldName, object paramValue)
        {
            dataOperation.PBlockAddRowParam(blockName,fieldName,paramValue);
        }
        /// <summary>
        /// 执行页面动作
        /// </summary>
        /// <param name="pageTrigger"></param>
        /// <param name="paramInfo"></param>
        public void ExcutePageTrigger(string pageTrigger, object paramInfo)
        {
            MainView.ParentControl.FeiDaoFSM.Do(pageTrigger, paramInfo);
        }
        /// <summary>
        /// 执行组件动作
        /// </summary>
        /// <param name="compntName"></param>
        /// <param name="compntTrigger"></param>
        /// <param name="paramInfo"></param>
        public void ExcuteComponentTrigger(string compntName, string compntTrigger, object paramInfo)
        {
            TemplateControl tc = MainView.FindName(compntName) as TemplateControl;
            tc.FeiDaoFSM.Do(compntTrigger, paramInfo);
        }
        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="groupName">分组信息</param>
        public void SetFocus(string groupName)
        {
            systemOperation.SetFocus(groupName);
        }
        /// <summary>
        /// 插入事实
        /// </summary>
        /// <param name="oav"></param>
        public void InsertFact(OAVModel oav)
        {
            systemOperation.InsertFact(oav);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav"></param>
        public void RemoveFact(OAVModel oav)
        {
            systemOperation.RemoveFact(oav);
        }
        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav"></param>
        public void UpdateFact(OAVModel oav)
        {
            if (oav.AtrributeValue != null)
                systemOperation.UpdateFact(oav);
        }
        /// <summary>
        /// 获取页面参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsPageGet(string paramName, OAVModel oav)
        {
            if (MainView.ParentControl.ParamDict.ContainsKey(paramName))
            {
                oav.AtrributeValue = MainView.ParentControl.ParamDict[paramName];
            }
        }
        /// <summary>
        /// 获取参数根据dictionary
        /// </summary>
        /// <param name="paramDic">dic</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsGetByDictionary(object paramDic, string paramName, OAVModel oav)
        {
            Dictionary<string, object> dic = paramDic as Dictionary<string, object>;
            if (dic != null && dic.ContainsKey(paramName))
            {
                oav.AtrributeValue = dic[paramName];
            }
        }
        /// <summary>
        /// 获取选中行的列值
        /// </summary>
        /// <param name="pblockName">P名</param>
        /// <param name="paramName">列名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsCurrentRowGet(string pblockName, string paramName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock.PreBlockSelectedRow != null && pBlock.ViewBlockDataTable.Columns.Contains(paramName))
            {
                oav.AtrributeValue = pBlock.PreBlockSelectedRow[paramName];
            }
        }
        /// <summary>
        /// 赋值选中行的列值
        /// </summary>
        /// <param name="pblockName">P名</param>
        /// <param name="paramName">列名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsCurrentRowSet(string pblockName, string paramName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock.PreBlockSelectedRow != null && pBlock.ViewBlockDataTable.Columns.Contains(paramName))
            {
                DataRow[] drSelected = pBlock.ViewBlockDataTable.Select("_id='" + pBlock.PreBlockSelectedRow["_id"].ToString() + "'");
                if (drSelected.Length > 0)
                {
                    drSelected[0][paramName] = oav.AtrributeValue;
                    pBlock.PreBlockSelectedRow = drSelected[0];
                }
            }
        }
        /// <summary>
        /// 组件参数封装
        /// </summary>
        /// <param name="oavCom">组件参数</param>
        /// <param name="oavPage">页面接收参数</param>
        public void ParamsInterCompntAdd(OAVModel oavCom, OAVModel oavPage)
        {
            FrameworkElement fElement = oavPage.AtrributeValue as FrameworkElement;
            if (fElement == null)
            {
                fElement = new FrameworkElement();
            }
            Dictionary<string, object> dicParams = fElement.Tag as Dictionary<string, object>;
            if (dicParams == null)
            {
                dicParams = new Dictionary<string, object>();
            }
            if (!dicParams.ContainsKey(oavCom.AtrributeName))
            {
                dicParams.Add(oavCom.AtrributeName, oavCom.AtrributeValue);
            }
            else
            {
                dicParams[oavCom.AtrributeName] = oavCom.AtrributeValue;
            }
            fElement.Tag = dicParams;
            oavPage.AtrributeValue = fElement;
        }
        /// <summary>
        /// 组件取参数
        /// </summary>
        /// <param name="se">状态信息</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsInterCompntParse(StateTransitionModel se, string paramName, OAVModel oav)
        {
            if (se.ActionSourceElement != null)
            {
                Dictionary<string, object> dicParams = se.ActionSourceElement.Tag as Dictionary<string, object>;
                if (dicParams != null && dicParams.ContainsKey(paramName))
                {
                    oav.AtrributeValue = dicParams[paramName];
                }
            }
        }
        /// <summary>
        /// 弹框展示组件操作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void UCCompntShowDialog(string compntName, int height = 600, int width = 600)
        {
            TemplateControl ucCom = MainView.ParentControl.GetComponentInstanceByName(compntName);
            if (ucCom == null)
            {
                Console.WriteLine("原子操作：UCCompntShowDialog未找到组件" + compntName);
                LoggerHelper.Info("原子操作：UCCompntShowDialog未找到组件" + compntName);
                return;
            }
            ucCom.ParentControl = MainView.ParentControl;

            VicWindowNormal win = new VicWindowNormal();
            win.Owner = XamlTreeHelper.GetParentObject<Window>(MainView);
            win.ShowInTaskbar = false;
            win.SetResourceReference(VicWindowNormal.StyleProperty, "WindowMessageSkin");
            win.Height = height;
            win.Width = width;
            win.Title = ucCom.Tag.ToString();
            win.Content = ucCom;
            win.ShowDialog();
        }
        /// <summary>
        /// 弹框展示组件操作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void UCCompntShow(string compntName, int height = 600, int width = 600)
        {
            TemplateControl ucCom = MainView.ParentControl.GetComponentInstanceByName(compntName);
            if (ucCom == null)
            {
                Console.WriteLine("原子操作：UCCompntShowDialog未找到组件" + compntName);
                LoggerHelper.Info("原子操作：UCCompntShowDialog未找到组件" + compntName);
                return;
            }
            ucCom.ParentControl = MainView.ParentControl;

            VicWindowNormal win = new VicWindowNormal();
            win.Owner = XamlTreeHelper.GetParentObject<Window>(MainView);
            win.ShowInTaskbar = false;
            win.SetResourceReference(VicWindowNormal.StyleProperty, "WindowMessageSkin");
            win.Height = height;
            win.Width = width;
            win.Title = ucCom.Tag.ToString();
            win.Content = ucCom;
            win.Show();
        }
        /// <summary>
        /// 弹框关闭操作
        /// </summary>
        public void UCCompntClose()
        {
            Window win = XamlTreeHelper.GetParentObject<Window>(MainView);
            if (win != null)
            {
                win.Close();
            }
        }
        /// <summary>
        /// 弹出提示信息
        /// </summary>
        /// <param name="messageInfo"></param>
        public void ShowMessage(object messageInfo)
        {
            VicMessageBoxNormal.Show(messageInfo == null ? "空值" : messageInfo.ToString());
        }
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="content">输出内容</param>
        public void SysFeiDaoLog(string content)
        {
            systemOperation.SysFeiDaoLog(content);
        }
        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="paramValue"></param>
        /// <param name="oav"></param>
        public void SetParamValue(object paramValue, OAVModel oav)
        {
            oav.AtrributeValue = paramValue;
        }

        #region 类型转换
        /// <summary>
        /// 转换字符串类型
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public string ConvertToString(object paramValue)
        {
            return paramValue == null ? "" : paramValue.ToString();
        }
        /// <summary>
        /// 转换整型
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public int ConvertToInt(object paramValue)
        {
            int i = 0;
            if (paramValue != null)
                int.TryParse(paramValue.ToString(), out i);
            return i;
        }
        /// <summary>
        /// 转换长整形
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public long ConvertToLong(object paramValue)
        {
            long i = 0;
            if (paramValue != null)
                long.TryParse(paramValue.ToString(), out i);
            return i;
        }
        /// <summary>
        /// 转换浮点型
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public decimal ConvertToDecimal(object paramValue)
        {
            decimal i = 0;
            if (paramValue != null)
                decimal.TryParse(paramValue.ToString(), out i);
            return i;
        }
        /// <summary>
        /// 转换bool型
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public bool ConvertToBool(object paramValue)
        {
            bool i = false;
            if (paramValue != null)
                bool.TryParse(paramValue.ToString(), out i);
            return i;
        } 
        #endregion
        

        #region VicTreeView原子操作
        /// <summary>
        /// 得到tree选中的值
        /// </summary>
        /// <param name="treeName">控件名</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav"></param>
        public void VicTreeViewGetParam(string treeName, string paramName, OAVModel oav)
        {
            VicTreeView treeView = MainView.FindName(treeName) as VicTreeView;
            if (treeView != null && treeView.SelectedItem != null)
            {
                DataRow drSelect = ((DataRowView)treeView.SelectedItem).Row;
                if (drSelect.Table.Columns.Contains(paramName))
                {
                    oav.AtrributeValue = drSelect[paramName];
                }
            }
        }
        #endregion

        #region VicListView原子操作
        /// <summary>
        /// 得到list选中的值
        /// </summary>
        /// <param name="listName">控件名</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav"></param>
        public void VicListViewGetParam(string listName, string paramName, OAVModel oav)
        {
            VicListViewNormal listView = MainView.FindName(listName) as VicListViewNormal;
            if (listView != null && listView.SelectedItem != null)
            {
                DataRow drSelect = ((DataRowView)listView.SelectedItem).Row;
                if (drSelect.Table.Columns.Contains(paramName))
                {
                    oav.AtrributeValue = drSelect[paramName];
                }
            }
        }
        #endregion

        #region VicTextBoxNormal原子操作
        /// <summary>
        /// 得到VicTextBoxNormal VicText值
        /// </summary>
        /// <param name="txtName">控件名</param>
        /// <param name="oav"></param>
        public void VicTextBoxNormalGetParamByVicText(string txtName, OAVModel oav)
        {
            VicTextBoxNormal txtBox = MainView.FindName(txtName) as VicTextBoxNormal;
            if (txtBox != null)
            {
                oav.AtrributeValue = txtBox.VicText;
            }
        }
        /// <summary>
        /// VicTextBoxNormal赋值VicText
        /// </summary>
        /// <param name="txtName"></param>
        /// <param name="oav"></param>
        public void VicTextBoxNormalSetVicText(string txtName, OAVModel oav)
        {
            VicTextBoxNormal txtBox = MainView.FindName(txtName) as VicTextBoxNormal;
            if (txtBox != null)
            {
                txtBox.VicText = oav.AtrributeValue == null ? "" : oav.AtrributeValue.ToString();
            }
        }
        #endregion

        #region VicTextBox 原子操作
        /// <summary>
        /// 得到VicTextBox VicText值
        /// </summary>
        /// <param name="txtName">控件名</param>
        /// <param name="oav"></param>
        public void VicTextBoxGetParamByVicText(string txtName, OAVModel oav)
        {
            VicTextBox txtBox = MainView.FindName(txtName) as VicTextBox;
            if (txtBox != null)
            {
                oav.AtrributeValue = txtBox.VicText;
            }
        }
        /// <summary>
        /// VicTextBox赋值VicText
        /// </summary>
        /// <param name="txtName"></param>
        /// <param name="oav"></param>
        public void VicTextBoxSetVicText(string txtName, OAVModel oav)
        {
            VicTextBox txtBox = MainView.FindName(txtName) as VicTextBox;
            if (txtBox != null)
            {
                txtBox.VicText = oav.AtrributeValue == null ? "" : oav.AtrributeValue.ToString();
            }
        }
        #endregion

        #region VicDataGrid原子操作
        /// <summary>
        /// 是否有选中项
        /// </summary>
        /// <param name="dgridName"></param>
        /// <param name="oav"></param>
        /// <param name="oavmsg">消息</param>
        public void VicDataGridIsSelectItem(string dgridName, OAVModel oav, OAVModel oavmsg)
        {
            VicDataGrid dgrid = MainView.FindName(dgridName) as VicDataGrid;
            if (dgrid != null && dgrid.SelectedItem != null)
            {
                oav.AtrributeValue = true;
            }
            else
            {
                oav.AtrributeValue = false;
                oavmsg.AtrributeValue = "当前选择项为空";
            }
        }
        /// <summary>
        /// 更新列
        /// </summary>
        /// <param name="dgridName"></param>
        public void VicDataGridUpdateColumn(string dgridName)
        {
            VicDataGrid dgrid = MainView.FindName(dgridName) as VicDataGrid;
            if (dgrid != null)
            {
                DataTable dataSource=dgrid.ItemsSource as DataTable;
                if (dataSource == null)
                    return;
                for (int i = 0; i < dgrid.Columns.Count; i++)
                {
                    DataGridColumn column = dgrid.Columns[i];
                    if (column is VicDataGridComboBoxColumn)
                    {
                        VicDataGridComboBoxColumn comboxColOlder = column as VicDataGridComboBoxColumn;
                        Binding binding = comboxColOlder.SelectedValueBinding as Binding;
                        string field = binding != null ? binding.Path.Path : "";
                        if (string.IsNullOrEmpty(field))
                            continue;

                        VicDataGridComboBoxColumn comboxCol = new VicDataGridComboBoxColumn();
                        if (dataSource.Columns[field].ExtendedProperties.ContainsKey("ComboBox"))
                        {
                            comboxCol.ItemsSource = (dataSource.Columns[field].ExtendedProperties["ComboBox"] as DataTable).DefaultView;
                        }
                        comboxCol.SelectedValuePath = "val";
                        comboxCol.DisplayMemberPath = "txt";
                        comboxCol.SelectedValueBinding = new Binding(field) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };

                        comboxCol.Header = comboxColOlder.Header;

                        comboxCol.Visibility = comboxColOlder.Visibility;
                        comboxCol.IsReadOnly = comboxColOlder.IsReadOnly;
                        comboxCol.Width = comboxColOlder.Width;
                        //comboxCol.ComboBoxClosed += comboxCol_ComboBoxClosed;
                        dgrid.Columns.Insert(i, comboxCol);
                        dgrid.Columns.Remove(column);
                    }
                }
            }
        }
        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="dgridName"></param>
        public void VicDataGridSelectRowDelete(string dgridName)
        {
            VicDataGrid dgrid = MainView.FindName(dgridName) as VicDataGrid;
            if (dgrid != null && dgrid.SelectedItem != null)
            {
                DataTable dt = dgrid.ItemsSource as DataTable;
                if (dt != null)
                {
                    DataRow dr = ((DataRowView)dgrid.SelectedItem).Row;
                    if (dr.RowState == DataRowState.Added)
                    {
                        dt.Rows.Remove(dr);
                    }
                    else
                    {
                        dr.Delete();
                    }
                }
            }
            else
            {
                VicMessageBoxNormal.Show("请选择选中行！");
            }
        }
        #endregion

        #region UnitPageRule原子操作
        /// <summary>
        /// UnitPageRule分页加载
        /// </summary>
        /// <param name="unitPageName"></param>
        /// <param name="pblockName"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public void UnitPageRuleLoad(string unitPageName, string pblockName, int currentPage = 1, int pageSize = 20)
        {
            TemplateControl unitPage = MainView.FindName(unitPageName) as TemplateControl;
            if (unitPage == null)
                return;
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            dicParam.Add("CurrentPage", currentPage);
            dicParam.Add("PageSize", pageSize);
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            DataSet ds = pBlock.ViewBlockDataTable.DataSet;
            if (ds != null)
            {
                dicParam.Add("TotalNum", int.Parse(ds.Tables["summary"].Rows[0]["totalRow"].ToString()));
                dicParam.Add("TotalPage", int.Parse(ds.Tables["summary"].Rows[0]["totalPage"].ToString()));
            }
            unitPage.Excute(dicParam);
        }
        #endregion

        #region 状态实体操作
        /// <summary>
        /// 设置警戒条件
        /// </summary>
        /// <param name="se"></param>
        /// <param name="oav"></param>
        /// <param name="oavmsg"></param>
        public void SetActionGuard(StateTransitionModel se, OAVModel oav, OAVModel oavmsg)
        {
            systemOperation.SetActionGuard(se, oav, oavmsg);
        }

        #endregion
        
    }
}
