﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// UI元素原子操作
    /// </summary>
    public class UIElementAtOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public UIElementAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }

        #region 私有字段
       /// <summary>
       /// 页面/组件基类
       /// </summary>
        private TemplateControl MainView;
        #endregion

        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="btnName">按钮名称</param>
        /// <param name="btnContent">按钮内容</param>
        public void SetButtonText(string btnName, string btnContent)
        {
            Button btn = MainView.FindName(btnName) as Button;
            if (btn != null)
                btn.Content = btnContent;
        }
        /// <summary>
        /// 获取界面元素名称
        /// </summary>
        /// <param name="element">界面元素</param>
        /// <param name="oav">接受oav</param>
        public void GetElementName(FrameworkElement element, OAVModel oav)
        {
            if (element != null && oav != null)
            {
                oav.AtrributeValue = element.Name;
            }
        }
        /// <summary>
        /// 设置按钮是否被选择
        /// </summary>
        /// <param name="paramValue">按钮的名称(Name)</param>
        /// <param name="isSelect">是否被选择</param>
        public void SetButtonSelect(string paramValue, bool isSelect)
        {
            VicButtonNormal tcButton = MainView.FindName(paramValue) as VicButtonNormal;
            if (tcButton != null)
            {
                if (isSelect)
                {
                    tcButton.Background = Brushes.DarkCyan;
                }
                else
                {
                    tcButton.Background = Brushes.Transparent;
                }
            }
        }
        #region VicTreeView原子操作
        /// <summary>
        /// 得到tree选中的值
        /// </summary>
        /// <param name="treeName">控件名</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
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
        /// <param name="oav">接收oav</param>
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
        /// <param name="oav">接收oav</param>
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
        /// <param name="txtName">控件名</param>
        /// <param name="oav">接收oav</param>
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
        /// <param name="oav">接收oav</param>
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
        /// <param name="txtName">控件名</param>
        /// <param name="oav">接收oav</param>
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
        /// <param name="dgridName">控件名</param>
        /// <param name="oav">接收oav</param>
        /// <param name="oavmsg">消息oav</param>
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
        /// <param name="dgridName">控件名</param>
        public void VicDataGridUpdateColumn(string dgridName)
        {
            VicDataGrid dgrid = MainView.FindName(dgridName) as VicDataGrid;
            if (dgrid != null)
            {
                DataTable dataSource = dgrid.ItemsSource as DataTable;
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
        #endregion

        #region UnitPageRule原子操作
        /// <summary>
        /// UnitPageRule分页加载
        /// </summary>
        /// <param name="unitPageName">分页控件名</param>
        /// <param name="pblockName">区块名称</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">数据条数</param>
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
    }
}
