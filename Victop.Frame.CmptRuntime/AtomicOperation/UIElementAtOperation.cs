using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// 设置元素内容
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="content">内容</param>
        public void SetElementContent(string elementName, object content)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                FrameworkElement element = MainView.FindName(elementName) as FrameworkElement;
                if (element != null)
                {
                    string typeName = element.GetType().Name;
                    switch (typeName)
                    {
                        case "VicButtonNormal":
                            VicButtonNormal button = element as VicButtonNormal;
                            button.Content = content;
                            break;
                        case "VicLabelNormal":
                            VicLabelNormal label = element as VicLabelNormal;
                            label.Content = content;
                            break;
                        case "VicTextBlockNormal":
                            VicTextBlockNormal textblock = element as VicTextBlockNormal;
                            textblock.Text = Convert.ToString(content);
                            break;
                        case "VicTextBox":
                            VicTextBox textbox = element as VicTextBox;
                            textbox.VicText = Convert.ToString(content);
                            break;
                        case "VicTextBoxNormal":
                            VicTextBoxNormal textboxnormal = element as VicTextBoxNormal;
                            textboxnormal.VicText = Convert.ToString(content);
                            break;
                        case "VicDatePickerNormal":
                            VicDatePickerNormal datepicker = element as VicDatePickerNormal;
                            datepicker.Value = Convert.ToDateTime(content);
                            break;
                        case "VicCheckBoxNormal":
                            VicCheckBoxNormal checkbox = element as VicCheckBoxNormal;
                            checkbox.Content = content;
                            break;
                        case "VicRadioButtonNormal":
                            VicRadioButtonNormal radiobutton = element as VicRadioButtonNormal;
                            radiobutton.Content = content;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取元素内容
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="oav">接收oav</param>
        public void GetElementContent(string elementName, OAVModel oav)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                FrameworkElement element = MainView.FindName(elementName) as FrameworkElement;
                if (element != null)
                {
                    string typeName = element.GetType().Name;
                    switch (typeName)
                    {
                        case "VicButtonNormal":
                            VicButtonNormal button = element as VicButtonNormal;
                            oav.AtrributeValue = button.Content;
                            break;
                        case "VicLabelNormal":
                            VicLabelNormal label = element as VicLabelNormal;
                            oav.AtrributeValue = label.Content;
                            break;
                        case "VicTextBlockNormal":
                            VicTextBlockNormal textblock = element as VicTextBlockNormal;
                            oav.AtrributeValue = textblock.Text;
                            break;
                        case "VicTextBox":
                            VicTextBox textbox = element as VicTextBox;
                            oav.AtrributeValue = textbox.VicText;
                            break;
                        case "VicTextBoxNormal":
                            VicTextBoxNormal textboxnormal = element as VicTextBoxNormal;
                            oav.AtrributeValue = textboxnormal.VicText;
                            break;
                        case "VicDatePickerNormal":
                            VicDatePickerNormal datepicker = element as VicDatePickerNormal;
                            oav.AtrributeValue = datepicker.Value;
                            break;
                        case "VicCheckBoxNormal":
                            VicCheckBoxNormal checkbox = element as VicCheckBoxNormal;
                            oav.AtrributeValue = checkbox.Content;
                            break;
                        case "VicRadioButtonNormal":
                            VicRadioButtonNormal radiobutton = element as VicRadioButtonNormal;
                            oav.AtrributeValue = radiobutton.Content;
                            break;
                        case "VicTreeView":
                            VicTreeView treeview = element as VicTreeView;
                            if (treeview.SelectedItem != null)
                            {
                                TreeViewItem tvi = (TreeViewItem)treeview.SelectedItem;
                                oav.AtrributeValue = tvi.Header;
                            }
                            break;
                        case "VicListBoxNormal":
                            VicListBoxNormal listbox = element as VicListBoxNormal;
                            if (listbox.SelectedItem != null)
                            {
                                ListBoxItem lbi = (ListBoxItem)listbox.SelectedItem;
                                oav.AtrributeValue = lbi.Content;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取元素内容
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="oav">接收oav</param>
        public void GetElementContent(string elementName, object oav)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                dynamic o = oav;
                FrameworkElement element = MainView.FindName(elementName) as FrameworkElement;
                if (element != null)
                {
                    string typeName = element.GetType().Name;
                    switch (typeName)
                    {
                        case "VicButtonNormal":
                            VicButtonNormal button = element as VicButtonNormal;
                            o.v = button.Content;
                            break;
                        case "VicLabelNormal":
                            VicLabelNormal label = element as VicLabelNormal;
                            o.v = label.Content;
                            break;
                        case "VicTextBlockNormal":
                            VicTextBlockNormal textblock = element as VicTextBlockNormal;
                            o.v = textblock.Text;
                            break;
                        case "VicTextBox":
                            VicTextBox textbox = element as VicTextBox;
                            o.v = textbox.VicText;
                            break;
                        case "VicTextBoxNormal":
                            VicTextBoxNormal textboxnormal = element as VicTextBoxNormal;
                            o.v = textboxnormal.VicText;
                            break;
                        case "VicDatePickerNormal":
                            VicDatePickerNormal datepicker = element as VicDatePickerNormal;
                            o.v = datepicker.Value;
                            break;
                        case "VicCheckBoxNormal":
                            VicCheckBoxNormal checkbox = element as VicCheckBoxNormal;
                            o.v = checkbox.Content;
                            break;
                        case "VicRadioButtonNormal":
                            VicRadioButtonNormal radiobutton = element as VicRadioButtonNormal;
                            o.v = radiobutton.Content;
                            break;
                        case "VicTreeView":
                            VicTreeView treeview = element as VicTreeView;
                            if (treeview.SelectedItem != null)
                            {
                                TreeViewItem tvi = (TreeViewItem)treeview.SelectedItem;
                                o.v = tvi.Header;
                            }
                            break;
                        case "VicListBoxNormal":
                            VicListBoxNormal listbox = element as VicListBoxNormal;
                            if (listbox.SelectedItem != null)
                            {
                                ListBoxItem lbi = (ListBoxItem)listbox.SelectedItem;
                                o.v = lbi.Content;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 设置元素选中项索引
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="selectedIndex">索引</param>
        public void SetElementSelectIndex(string elementName, int selectedIndex)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                FrameworkElement element = MainView.FindName(elementName) as FrameworkElement;
                if (element != null)
                {
                    string typeName = element.GetType().Name;
                    switch (typeName)
                    {
                        case "VicDataGrid":
                            VicDataGrid datagrid = element as VicDataGrid;
                            datagrid.SelectedIndex = selectedIndex;
                            break;
                        case "VicListBoxNormal":
                            VicListBoxNormal listbox = element as VicListBoxNormal;
                            listbox.SelectedIndex = selectedIndex;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取界面元素名称
        /// </summary>
        /// <param name="element">界面元素</param>
        /// <param name="oav">接收oav</param>
        public void GetElementName(FrameworkElement element, object oav)
        {
            if (element != null && oav != null)
            {
                dynamic o = oav;
                o.v = element.Name;
            }
        }
        /// <summary>
        /// 获取界面元素名称
        /// </summary>
        /// <param name="element">界面元素</param>
        /// <param name="oav">接收oav</param>
        public void GetElementName(FrameworkElement element, OAVModel oav)
        {
            if (element != null && oav != null)
            {
                oav.AtrributeValue = element.Name;
            }
        }
        /// <summary>
        /// 设置元素是否启用
        /// </summary>
        /// <param name="elementName">界面元素名称</param>
        /// <param name="state">状态（0启用，1不可用）</param>
        public void SetElementIsEnabled(string elementName, int state = 0)
        {
            FrameworkElement element = MainView.FindName(elementName) as FrameworkElement;
            if (element != null)
            {
                if (state == 0)
                    element.IsEnabled = true;
                else
                    element.IsEnabled = false;
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
        /// <summary>
        /// 根据DataGrid名称获取当前选择行中某一字段的值
        /// </summary>
        /// <param name="gridName">grid名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">返回OAV值</param>
        public void ParamsCurrentRowGetFromGridName(string gridName, string paramName, object oav)
        {
            VicDataGrid dataGrid = (VicDataGrid)MainView.FindName(gridName);
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                dynamic o = oav;
                o.v = ((DataRowView)dataGrid.SelectedItem).Row[paramName];
            }
        }
        /// <summary>
        /// 根据DataGrid名称获取当前选择行中某一字段的值
        /// </summary>
        /// <param name="gridName">grid名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">返回OAV值</param>
        public void ParamsCurrentRowGetFromGridName(string gridName, string paramName, OAVModel oav)
        {
            VicDataGrid dataGrid = (VicDataGrid)MainView.FindName(gridName);
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                oav.AtrributeValue = ((DataRowView)dataGrid.SelectedItem).Row[paramName];
            }
        }
        /// <summary>
        /// 获取元素是否选中
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="oav">接收oav</param>
        public void GetElementIsChecked(string elementName, object oav)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                FrameworkElement element = MainView.FindName(elementName) as FrameworkElement;
                if (element != null)
                {
                    string typeName = element.GetType().Name;
                    dynamic o = oav;
                    switch (typeName)
                    {
                        case "VicCheckBoxNormal":
                            VicCheckBoxNormal checkbox = element as VicCheckBoxNormal;
                            o.v = checkbox.IsChecked;
                            break;
                        case "VicRadioButtonNormal":
                            VicRadioButtonNormal radiobutton = element as VicRadioButtonNormal;
                            o.v = radiobutton.IsChecked;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取元素是否选中
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="oav">接收oav</param>
        public void GetElementIsChecked(string elementName, OAVModel oav)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                FrameworkElement element = MainView.FindName(elementName) as FrameworkElement;
                if (element != null)
                {
                    string typeName = element.GetType().Name;
                    switch (typeName)
                    {
                        case "VicCheckBoxNormal":
                            VicCheckBoxNormal checkbox = element as VicCheckBoxNormal;
                            oav.AtrributeValue = checkbox.IsChecked;
                            break;
                        case "VicRadioButtonNormal":
                            VicRadioButtonNormal radiobutton = element as VicRadioButtonNormal;
                            oav.AtrributeValue = radiobutton.IsChecked;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// VicDataGrid是否有选中项
        /// </summary>
        /// <param name="dgridName">控件名</param>
        /// <param name="oav">接收oav</param>
        /// <param name="oavmsg">消息oav</param>
        public void VicDataGridIsSelectItem(string dgridName, object oav, object oavmsg)
        {
            FrameworkElement element = MainView.FindName(dgridName) as FrameworkElement;
            dynamic o1 = oav;
            string msg = "当前选择项为空";
            if (element != null)
            {
                string typeName = element.GetType().Name;
                switch (typeName)
                {
                    case "VicDataGrid":
                        VicDataGrid datagrid = element as VicDataGrid;
                        if (datagrid != null && datagrid.SelectedItem != null)
                        {
                            o1.v = true;
                        }
                        else
                        {
                            o1.v = false;
                            if (oavmsg != null)
                            {
                                dynamic o2 = oavmsg;
                                o2.v = msg;
                            }
                        }
                        break;
                    case "VicTreeView":
                        VicTreeView treeview = element as VicTreeView;
                        if (treeview != null && treeview.SelectedItem != null)
                        {
                            o1.v = true;
                        }
                        else
                        {
                            o1.v = false;
                            if (oavmsg != null)
                            {
                                dynamic o2 = oavmsg;
                                o2.v = msg;
                            }
                        }
                        break;
                    case "VicListBoxNormal":
                        VicListBoxNormal listbox = element as VicListBoxNormal;
                        if (listbox != null && listbox.SelectedItem != null)
                        {
                            o1.v = true;
                        }
                        else
                        {
                            o1.v = false;
                            if (oavmsg != null)
                            {
                                dynamic o2 = oavmsg;
                                o2.v = msg;
                            }
                        }
                        break;
                    case "VicListViewNormal":
                        VicListViewNormal listview = element as VicListViewNormal;
                        if (listview != null && listview.SelectedItem != null)
                        {
                            o1.v = true;
                        }
                        else
                        {
                            o1.v = false;
                            if (oavmsg != null)
                            {
                                dynamic o2 = oavmsg;
                                o2.v = msg;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// VicDataGrid是否有选中项
        /// </summary>
        /// <param name="dgridName">控件名</param>
        /// <param name="oav">接收oav</param>
        /// <param name="oavmsg">消息oav</param>
        public void VicDataGridIsSelectItem(string dgridName, OAVModel oav, OAVModel oavmsg)
        {
            FrameworkElement element = MainView.FindName(dgridName) as FrameworkElement;
            string msg = "当前选择项为空";
            if (element != null)
            {
                string typeName = element.GetType().Name;
                switch (typeName)
                {
                    case "VicDataGrid":
                        VicDataGrid datagrid = element as VicDataGrid;
                        if (datagrid != null && datagrid.SelectedItem != null)
                        {
                            oav.AtrributeValue = true;
                        }
                        else
                        {
                            oav.AtrributeValue = false;
                            if (oavmsg != null)
                                oavmsg.AtrributeValue = msg;
                        }
                        break;
                    case "VicTreeView":
                        VicTreeView treeview = element as VicTreeView;
                        if (treeview != null && treeview.SelectedItem != null)
                        {
                            oav.AtrributeValue = true;
                        }
                        else
                        {
                            oav.AtrributeValue = false;
                            if (oavmsg != null)
                                oavmsg.AtrributeValue = msg;
                        }
                        break;
                    case "VicListBoxNormal":
                        VicListBoxNormal listbox = element as VicListBoxNormal;
                        if (listbox != null && listbox.SelectedItem != null)
                        {
                            oav.AtrributeValue = true;
                        }
                        else
                        {
                            oav.AtrributeValue = false;
                            if (oavmsg != null)
                                oavmsg.AtrributeValue = msg;
                        }
                        break;
                    case "VicListViewNormal":
                        VicListViewNormal listview = element as VicListViewNormal;
                        if (listview != null && listview.SelectedItem != null)
                        {
                            oav.AtrributeValue = true;
                        }
                        else
                        {
                            oav.AtrributeValue = false;
                            if (oavmsg != null)
                                oavmsg.AtrributeValue = msg;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// VicDataGrid更新列
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
        /// <summary>
        /// VicDataGrid是否只读
        /// </summary>
        /// <param name="dgridName">控件名</param>
        /// <param name="state">状态（0启用，1不可用）</param>
        public void VicDataGridIsReadOnly(string dgridName, int state = 0)
        {
            VicDataGrid dgrid = MainView.FindName(dgridName) as VicDataGrid;
            if (dgrid != null)
            {
                if (state == 0)
                {
                    dgrid.IsReadOnly = true;
                }
                else
                {
                    dgrid.IsReadOnly = false;
                }
            }
        }
        /// <summary>
        /// 清除dgrid数据集合
        /// </summary>
        /// <param name="dgridName">控件名</param>
        public void VicDataGridClearData(string dgridName)
        {
            VicDataGrid dgrid = MainView.FindName(dgridName) as VicDataGrid;
            if (dgrid != null)
            {
                dgrid.ItemsSource = null;
            }
        }
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
        /// <summary>
        /// UnitUCWebBrowserRule加载
        /// </summary>
        /// <param name="unitWebBrowserName">浏览器控件名</param>
        /// <param name="content">展示内容</param>
        /// <param name="pblockName">区块名称用于传入当前行可为空字符串</param>
        /// <param name="columnName">列名可为空字符串</param>
        /// <param name="ctrl">控制是否支持提交修改默认true</param>
        /// <param name="executeJsFunc">接收JS方法名默认ExcuteWPF</param>
        public void UnitWebBrowserLoad(string unitWebBrowserName, object content, string pblockName = "", string columnName = "", bool ctrl = true, string executeJsFunc = "ExcuteWPF")
        {
            TemplateControl unitWebBrowser = MainView.FindName(unitWebBrowserName) as TemplateControl;
            if (unitWebBrowser == null)
                return;

            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            dicParam.Add("ParamContent", content);
            dicParam.Add("ParamCtrl", ctrl);
            dicParam.Add("URL", ConfigurationManager.AppSettings["htmleditor"] + "?productid=feidao");
            dicParam.Add("ExecuteJsFunc", executeJsFunc);
            if (!string.IsNullOrEmpty(columnName))
            {
                dicParam.Add("ColumnName", columnName);
            }
            if (!string.IsNullOrEmpty(pblockName))
            {
                PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
                if (pBlock != null && pBlock.PreBlockSelectedRow != null)
                {
                    dicParam.Add("Dr", pBlock.PreBlockSelectedRow);
                }
            }
            unitWebBrowser.Excute(dicParam);
        }
    }
}
