﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
                        case "Image":
                            Image img = element as Image;
                            img.Source = new BitmapImage(new Uri(content.ToString()));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 设置元素是否选中
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="isChecked">是否选中</param>
        public void SetElementChecked(string elementName, bool isChecked)
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
                            checkbox.IsChecked = isChecked;
                            break;
                        case "VicRadioButtonNormal":
                            VicRadioButtonNormal radiobutton = element as VicRadioButtonNormal;
                            radiobutton.IsChecked = isChecked;
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
                            else
                            {
                                o.v = "";
                            }
                            break;
                        case "VicComboBoxNormal":
                            VicComboBoxNormal cmbox = element as VicComboBoxNormal;
                            if (cmbox.SelectedItem != null)
                            {
                                ComboBoxItem item = (ComboBoxItem)cmbox.SelectedItem;
                                o.v = item.Content;
                            }
                            else
                            {
                                o.v = "";
                            }
                            break;
                        default:
                            break;
                    }
                    if (o.v == null)
                        o.v = "";
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
                        case "VicComboBoxNormal":
                            VicComboBoxNormal combobx = element as VicComboBoxNormal;
                            combobx.SelectedIndex = selectedIndex;
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
        public void GetElementName(object element, object oav)
        {
            FrameworkElement fElement = element as FrameworkElement;
            if (fElement != null && oav != null)
            {
                dynamic o = oav;
                o.v = fElement.Name;
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
        /// 设置元素是否可见
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="visibility">是否显示</param>
        ///  <param name="type">占空间还是不占空间隐藏：0,不占空间;1,不占空间</param>
        public void SetElementVisility(string elementName, bool visibility, int type = 0)
        {
            FrameworkElement tc = MainView.FindName(elementName) as FrameworkElement;
            if (tc != null)
            {
                if (visibility)
                {
                    tc.Visibility = Visibility.Visible;
                }
                else
                {
                    if (type == 0)
                    {
                        tc.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        tc.Visibility = Visibility.Hidden;
                    }
                }
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
                            if (oavmsg != null & oavmsg != DBNull.Value && !string.IsNullOrEmpty(oavmsg.ToString()))
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
                            if (oavmsg != null & oavmsg != DBNull.Value && !string.IsNullOrEmpty(oavmsg.ToString()))
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
                            if (oavmsg != null & oavmsg != DBNull.Value && !string.IsNullOrEmpty(oavmsg.ToString()))
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
                            if (oavmsg != null & oavmsg != DBNull.Value && !string.IsNullOrEmpty(oavmsg.ToString()))
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
        /// 源编辑器
        /// </summary>
        /// <param name="unitAE">源编辑器部件名字</param>
        /// <param name="text">内容</param>
        public void UnitAvalonEditLoad(string unitAE, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                TemplateControl tc = MainView.FindName(unitAE) as TemplateControl;
                if (tc == null)
                    return;
                Dictionary<string, object> dicParam = new Dictionary<string, object>();
                dicParam.Add("text", text);
                tc.Excute(dicParam);
            }
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
        /// <param name="type">展示类型</param>
        public void UnitWebBrowserLoad(string unitWebBrowserName, object content, string pblockName = "", string columnName = "", bool ctrl = true, string executeJsFunc = "ExcuteWPF", string type = "editor")
        {
            TemplateControl unitWebBrowser = MainView.FindName(unitWebBrowserName) as TemplateControl;
            if (unitWebBrowser == null)
                return;

            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            dicParam.Add("ParamContent", content);
            dicParam.Add("ParamCtrl", ctrl);
            switch (type)
            {
                case "editor":
                    dicParam.Add("URL", ConfigurationManager.AppSettings["htmleditor"] + "?productid=feidao");
                    break;
                case "json":
                    dicParam.Add("URL", AppDomain.CurrentDomain.BaseDirectory + "form/jsonview/jsonview.html");
                    break;
                default:
                    dicParam.Add("URL", AppDomain.CurrentDomain.BaseDirectory + "form/jsonview/jsonview.html");
                    break;
            }
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
        /// <summary>
        /// 画廊部件初始化
        /// </summary>
        /// <param name="unitName">部件名称</param>
        /// <param name="pblockName">区块名称</param>
        /// <param name="imagecolumName">图片列</param>
        /// <param name="titlecolumName">标题列</param>
        public void UnitGalleryRuleLoad(string unitName, string pblockName, string imagecolumName, string titlecolumName)
        {
            TemplateControl unitGallery = MainView.FindName(unitName) as TemplateControl;
            if (unitGallery == null)
                return;
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            DataTable dtData = pBlock.ViewBlockDataTable;
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            dicParam.Add("MessageType", "init");
            Dictionary<string, object> dicContentParam = new Dictionary<string, object>();
            dicContentParam.Add("data", dtData);
            dicContentParam.Add("image", imagecolumName);
            dicContentParam.Add("title", titlecolumName);
            dicParam.Add("MessageContent", dicContentParam);
            unitGallery.Excute(dicParam);
        }

        /// <summary>
        /// UnitListBoxFontIconRule部件刷新
        /// </summary>
        /// <param name="unitName">部件名称</param>
        public void UnitListBoxFontIconRuleRefresh(string unitName)
        {
            TemplateControl unitPage = MainView.FindName(unitName) as TemplateControl;
            if (unitPage == null)
                return;
            unitPage.Excute(new Dictionary<string, object> { { "MessageType", "refresh" } });
        }
        /// <summary>
        /// 设置元素是否可见(占空间)
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="visibility">是否显示</param>
        public void SetElementVisilityOrHidden(string elementName, bool visibility)
        {
            FrameworkElement tc = MainView.FindName(elementName) as FrameworkElement;
            if (tc != null)
            {
                if (visibility)
                {
                    tc.Visibility = Visibility.Visible;
                    tc.Height = 700;
                }
                else
                {
                    tc.Visibility = Visibility.Hidden;
                    tc.Height = 0;
                }
            }
        }
        /// <summary>
        /// 操作分析组件渲染数据
        /// </summary>
        /// <param name="componetName">组件名称</param>
        /// <param name="pblockName">pBlock名称</param>
        /// <param name="maintable">主表</param>
        /// <param name="secondtable">二层第一张表</param>
        /// <param name="secendtwotable">二层第二张表</param>
        public void ComponentOperationAnalysisRender(string componetName, string pblockName, string maintable, string secondtable, string secendtwotable)
        {
            TemplateControl tc = MainView.FindName(componetName) as TemplateControl;
            if (tc != null)
            {
                PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
                string chanleId = pBlock.ViewBlock.ViewModel.ViewId;
                Dictionary<string, object> dicParam = new Dictionary<string, object>();
                dicParam.Add("MessageType", "Rendering");
                Dictionary<string, object> dicContentParam = new Dictionary<string, object>();
                dicContentParam.Add("channelId", chanleId);
                dicContentParam.Add("mainDtName", maintable);
                dicContentParam.Add("operationFlowDtName", secondtable);
                dicContentParam.Add("pageFlowDtName", secendtwotable);
                dicParam.Add("MessageContent", dicContentParam);
                tc.Excute(dicParam);
            }
        }

        /// <summary>
        /// 操作分析组件添加主数据
        /// </summary>
        /// <param name="componetName">组件名称</param>
        public void ComponentOperationAnalysisAdd(string componetName)
        {
            TemplateControl tc = MainView.FindName(componetName) as TemplateControl;
            if (tc != null)
            {
                Dictionary<string, object> dicParam = new Dictionary<string, object>();
                dicParam.Add("MessageType", "Add");
                Dictionary<string, object> dicContentParam = new Dictionary<string, object>();
                dicParam.Add("MessageContent", dicContentParam);
                tc.Excute(dicParam);
            }
        }

        /// <summary>
        /// 获取组件中的值
        /// </summary>
        /// <param name="componetName">组件名称</param>
        /// <param name="key">key值</param>
        /// <param name="oav">返回参数</param>
        public void GetComponentParamDictData(string componetName, string key, object oav)
        {
            if (!string.IsNullOrEmpty(componetName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(componetName);
                if (tc != null && tc.ParamDict != null && tc.ParamDict.ContainsKey(key))
                {
                    dynamic o = oav;
                    o.v = tc.ParamDict[key];
                }
            }
        }
        /// <summary>
        /// 获取combox选择值
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="oav"></param>
        public void GetComBoxSelectValue(string elementName, object oav)
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
                        case "VicComboBoxNormal":
                            VicComboBoxNormal cmbox = element as VicComboBoxNormal;
                            if (cmbox.SelectedItem != null)
                            {
                                DataRow dataRow = ((DataRowView)cmbox.SelectedItem).Row;
                                if (dataRow != null)
                                {
                                    o.v = dataRow["value"];
                                }
                                else
                                {
                                    o.v = "";
                                }
                            }
                            else
                            {
                                o.v = "";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region 动态构建的Listbox部件初始化或者刷新方法
        /// <summary>
        /// 动态构建的Listbox部件初始化或者刷新方法
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="pblockName">p块名称</param>
        public void UnitListBoxDynamicRuleRefresh(string unitName, string pblockName)
        {
            TemplateControl unitListbox = MainView.FindName(unitName) as TemplateControl;
            if (unitListbox == null) { return; }
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            DataTable dtData = pBlock.ViewBlockDataTable;
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            dicMessage.Add("MessageType", "refresh");
            Dictionary<string, object> dicContent = new Dictionary<string, object>();
            dicContent.Add("DtData", dtData);
            dicMessage.Add("MessageContent", dicContent);
            unitListbox.Excute(dicMessage);
        }
        #endregion

        #region 动态构建的Listbox部件获取复选框选中的数据集合
        /// <summary>
        /// 动态构建的Listbox部件获取复选框选中的数据集合
        /// <param name="unitName">部件名称</param>
        /// <param name="oav">接收OAV</param>
        /// </summary>
        public void UnitListBoxDynamicRuleGetChosedRows(string unitName, object oav)
        {
            TemplateControl unitListbox = MainView.FindName(unitName) as TemplateControl;
            if (unitListbox == null) { return; }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            dicMessage.Add("MessageType", "getrows");
            unitListbox.Excute(dicMessage);
            if (unitListbox.ParamDict != null && unitListbox.ParamDict.ContainsKey("ChosedRowsList"))
            {
                dynamic o = oav;
                List<DataRow> list = unitListbox.ParamDict["ChosedRowsList"] as List<DataRow>;
                o.v = list;
            }
        }
        #endregion 

        #region 动态构建的ComponentVisioProperty初始化方法

        /// <summary>
        ///  动态构建的ComponentVisioProperty初始化方法
        /// </summary>
        /// <param name="unitName">组件名</param>
        /// <param name="pblockNameProperty">节点属性P名称</param>
        /// <param name="pageType">客户端类型</param>
        /// <param name="nodeNo">节点编号</param>
        /// <param name="nodeTypeNo">节点类型编号</param>
        /// <param name="nodeTypeName">节点类型名称</param>
        /// <param name="pblockNo">节点所属P编号</param>
        /// <param name="pblockName">节点所属P名称</param>
        /// <param name="formatNo">节点所属版式编号</param>
        public void ComponentVisioPropertyLoad(string unitName, string pblockNameProperty, string pageType, string nodeNo, string nodeTypeNo,string nodeTypeName, string pblockNo , string pblockName , string formatNo)
        {
            TemplateControl componentVisioProperty = MainView.FindName(unitName) as TemplateControl;
            if (componentVisioProperty == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "init");
            //参数
            Dictionary<string, object> dicContent = new Dictionary<string, object>();
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockNameProperty);
            dicContent.Add("dtNodeProperty", pBlock.ViewBlockDataTable);
            dicContent.Add("pageType", pageType);
            dicContent.Add("nodeNo", nodeNo);
            dicContent.Add("nodeTypeNo", nodeTypeNo);
            dicContent.Add("nodeTypeName", nodeTypeName);
            dicContent.Add("pblockNo", pblockNo);
            dicContent.Add("pblockName", pblockName);
            dicContent.Add("formatNo", formatNo);
            dicMessage.Add("MessageContent", dicContent);

            componentVisioProperty.Excute(dicMessage);
        }

        #endregion

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="paramDic"></param>
        public void TemplateControlLoad(string name,object paramDic)
        {
            Dictionary<string, object> dicParam = (Dictionary<string, object>)paramDic;
            TemplateControl tc = MainView.FindName(name) as TemplateControl;
            if (tc == null)
                return;
            tc.Excute(dicParam);
        }

        #region 原型图形部件
        /// <summary>
        /// 原型图形部件初始化
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="width">宽度</param>
        public void UnitUCDesignerRuleLoad(string unitName, object pageType, object width)
        {
            TemplateControl template = MainView.FindName(unitName) as TemplateControl;
            if (template == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "refresh");
            //参数
            Dictionary<string, object> dicContent = new Dictionary<string, object>();
            dicContent.Add("pageType", pageType);
            dicContent.Add("width", width);
            dicMessage.Add("MessageContent", dicContent);

            template.Excute(dicMessage);
        }
        /// <summary>
        /// 原型图形部件加载文件
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="fileName">文件物理地址</param>
        public void UnitUCDesignerRuleLoadFile(string unitName, object fileName)
        {
            TemplateControl template = MainView.FindName(unitName) as TemplateControl;
            if (template == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "loadFile");
            //参数
            Dictionary<string, object> dicContent = new Dictionary<string, object>();
            dicContent.Add("fileName", fileName);
            dicMessage.Add("MessageContent", dicContent);

            template.Excute(dicMessage);
        }
        /// <summary>
        /// 原型图形部件保存页面布局（返回布局文件串）
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="oav">接收oav</param>
        public void UnitUCDesignerRuleSavePageLayout(string unitName, object oav)
        {
            TemplateControl template = MainView.FindName(unitName) as TemplateControl;
            if (template == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "savePageLayout");
            template.Excute(dicMessage);
            if (template.ParamDict != null && template.ParamDict.ContainsKey("result"))
            {
                dynamic o = oav;
                o.v = template.ParamDict["result"];
            }
        }
        /// <summary>
        /// 原型图形部件导入页面布局
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="elementStr">布局文件字符串</param>
        public void UnitUCDesignerRuleDropPageLayout(string unitName, object elementStr)
        {
            TemplateControl template = MainView.FindName(unitName) as TemplateControl;
            if (template == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "dropPageLayout");
            //参数
            Dictionary<string, object> dicContent = new Dictionary<string, object>();
            dicContent.Add("elementStr", elementStr);
            dicMessage.Add("MessageContent", dicContent);

            template.Excute(dicMessage);
        }
        /// <summary>
        /// 原型图形部件导入版式
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="elementStr">版式文件字符串</param>
        /// <param name="point">鼠标位置</param>
        /// <param name="itemBase">版式所属组件图形</param>
        public void UnitUCDesignerRuleDropFormat(string unitName, object elementStr, object point, object itemBase)
        {
            TemplateControl template = MainView.FindName(unitName) as TemplateControl;
            if (template == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "dropFormat");
            //参数
            Dictionary<string, object> dicContent = new Dictionary<string, object>();
            dicContent.Add("elementStr", elementStr);
            dicContent.Add("point", point);
            dicContent.Add("itemBase", itemBase);
            dicMessage.Add("MessageContent", dicContent);

            template.Excute(dicMessage);
        }
        /// <summary>
        /// 原型图形部件保存版式（返回版式文件串）
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="oav">接收oav</param>
        public void UnitUCDesignerRuleSaveFormat(string unitName, object oav)
        {
            TemplateControl template = MainView.FindName(unitName) as TemplateControl;
            if (template == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "saveFormat");
            template.Excute(dicMessage);
            if (template.ParamDict != null && template.ParamDict.ContainsKey("result"))
            {
                dynamic o = oav;
                o.v = template.ParamDict["result"];
            }
        }
        /// <summary>
        /// 原型图形部件拖拽控件
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="itemTitle">控件模具名</param>
        /// <param name="point">鼠标位置</param>
        /// <param name="itemBase">版式所属组件图形</param>
        public void UnitUCDesignerRuleDropItem(string unitName, object itemTitle, object point, object itemBase)
        {
            TemplateControl template = MainView.FindName(unitName) as TemplateControl;
            if (template == null)
            {
                return;
            }
            Dictionary<string, object> dicMessage = new Dictionary<string, object>();
            //类型
            dicMessage.Add("MessageType", "dropItem");
            //参数
            Dictionary<string, object> dicContent = new Dictionary<string, object>();
            dicContent.Add("itemTitle", itemTitle);
            dicContent.Add("point", point);
            dicContent.Add("itemBase", itemBase);
            dicMessage.Add("MessageContent", dicContent);

            template.Excute(dicMessage);
        }
        #endregion


    }
}