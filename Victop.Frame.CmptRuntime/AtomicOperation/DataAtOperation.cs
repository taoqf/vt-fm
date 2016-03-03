using gnu.javax.crypto.assembly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// 数据原子操作
    /// </summary>
    public class DataAtOperation
    {
        #region 私有定义
        private TemplateControl MainView;
        /// <summary>
        /// 存储查询条件
        /// </summary>
        private Dictionary<string, ViewsConditionModel> conditionModelDic = new Dictionary<string, ViewsConditionModel>();
        /// <summary>
        /// p生成的oav集合
        /// </summary>
        private Dictionary<string, Dictionary<string, List<OAVModel>>> blockOAVDic = new Dictionary<string, Dictionary<string, List<OAVModel>>>();
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public DataAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }

        /// <summary>
        /// 设置区块查询条件日期区间
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        public void SetConditionSearchDate(string pBlockName, string paramField, string startDate, string endDate)
        {
            if (!string.IsNullOrEmpty(pBlockName) && !string.IsNullOrEmpty(paramField) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime start = Convert.ToDateTime(Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime end = Convert.ToDateTime(Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm:ss"));
                Dictionary<string, object> dataDic = new Dictionary<string, object>();
                dataDic.Add("$gte", (long)(start - (new DateTime(1970, 1, 1))).TotalMilliseconds);
                dataDic.Add("$lte", (long)(end - (new DateTime(1970, 1, 1))).TotalMilliseconds);
                if (conditionModelDic.ContainsKey(pBlockName))
                {
                    Dictionary<string, object> paramDic = conditionModelDic[pBlockName].TableCondition;
                    if (paramDic.ContainsKey(paramField))
                    {
                        paramDic[paramField] = dataDic;
                    }
                    else
                    {
                        paramDic.Add(paramField, dataDic);
                    }
                }
                else
                {
                    ViewsConditionModel viewConModel = new ViewsConditionModel();
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add(paramField, dataDic);
                    viewConModel.TableCondition = paramDic;
                    conditionModelDic.Add(pBlockName, viewConModel);
                }
            }
        }
        /// <summary>
        /// 设置区块查询条件模糊匹配
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="paramValue">参数值</param>
        public void SetConditionSearchLike(string pBlockName, string paramField, object paramValue)
        {
            if (!string.IsNullOrEmpty(pBlockName) && !string.IsNullOrEmpty(paramField))
            {
                if (conditionModelDic.ContainsKey(pBlockName))
                {
                    Dictionary<string, object> paramDic = conditionModelDic[pBlockName].TableCondition;
                    if (paramDic.ContainsKey(paramField))
                    {
                        paramDic[paramField] = RegexHelper.Contains(Convert.ToString(paramValue));
                    }
                    else
                    {
                        paramDic.Add(paramField, RegexHelper.Contains(Convert.ToString(paramValue)));
                    }
                }
                else
                {
                    ViewsConditionModel viewConModel = new ViewsConditionModel();
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add(paramField, RegexHelper.Contains(Convert.ToString(paramValue)));
                    viewConModel.TableCondition = paramDic;
                    conditionModelDic.Add(pBlockName, viewConModel);
                }
            }
        }
        /// <summary>
        /// 设置区块查询条件子查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="listIn">子查询集合</param>
        public void SetConditionSearchIn(string pBlockName, string paramField, object listIn)
        {
            if (!string.IsNullOrEmpty(pBlockName) && !string.IsNullOrEmpty(paramField) && listIn != null)
            {
                List<object> list = listIn as List<object>;
                if (list != null)
                {
                    Dictionary<string, object> inDic = new Dictionary<string, object>();
                    inDic.Add("$in", list);
                    if (conditionModelDic.ContainsKey(pBlockName))
                    {
                        Dictionary<string, object> paramDic = conditionModelDic[pBlockName].TableCondition;
                        if (paramDic.ContainsKey(paramField))
                        {
                            paramDic[paramField] = inDic;
                        }
                        else
                        {
                            paramDic.Add(paramField, inDic);
                        }
                    }
                    else
                    {
                        ViewsConditionModel viewConModel = new ViewsConditionModel();
                        Dictionary<string, object> paramDic = new Dictionary<string, object>();
                        paramDic.Add(paramField, inDic);
                        viewConModel.TableCondition = paramDic;
                        conditionModelDic.Add(pBlockName, viewConModel);
                    }
                }
            }
        }
        /// <summary>
        /// 设置区块查询条件
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="paramValue">参数值</param>
        public void SetConditionSearch(string pBlockName, string paramField, object paramValue)
        {
            if (!string.IsNullOrEmpty(pBlockName) && !string.IsNullOrEmpty(paramField))
            {
                if (conditionModelDic.ContainsKey(pBlockName))
                {
                    Dictionary<string, object> paramDic = conditionModelDic[pBlockName].TableCondition;
                    if (paramDic.ContainsKey(paramField))
                    {
                        paramDic[paramField] = paramValue;
                    }
                    else
                    {
                        paramDic.Add(paramField, paramValue);
                    }
                }
                else
                {
                    ViewsConditionModel viewConModel = new ViewsConditionModel();
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add(paramField, paramValue);
                    viewConModel.TableCondition = paramDic;
                    conditionModelDic.Add(pBlockName, viewConModel);
                }
            }
        }
        /// <summary>
        /// 设置区块是否取空数据
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="isEmptyData">true取空数据</param>
        public void SetConditionIsEmptyData(string blockName, bool isEmptyData)
        {
            if (!string.IsNullOrEmpty(blockName))
            {
                if (conditionModelDic.ContainsKey(blockName))
                {
                    conditionModelDic[blockName].EmptyData = isEmptyData;
                }
                else
                {
                    ViewsConditionModel viewConModel = new ViewsConditionModel();
                    viewConModel.EmptyData = isEmptyData;
                    conditionModelDic.Add(blockName, viewConModel);
                }
            }
        }
        /// <summary>
        /// 设置区块排序
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="sort">排序方式1正序-1倒序</param>
        public void SetConditionSort(string pBlockName, string paramField, int sort)
        {
            if (!string.IsNullOrEmpty(pBlockName) && !string.IsNullOrEmpty(paramField))
            {
                if (conditionModelDic.ContainsKey(pBlockName))
                {
                    Dictionary<string, object> sortDic = conditionModelDic[pBlockName].TableSort;
                    if (sortDic.ContainsKey(paramField))
                    {
                        sortDic[paramField] = sort;
                    }
                    else
                    {
                        sortDic.Add(paramField, sort);
                    }
                }
                else
                {
                    ViewsConditionModel viewConModel = new ViewsConditionModel();
                    Dictionary<string, object> sortDic = new Dictionary<string, object>();
                    sortDic.Add(paramField, sort);
                    viewConModel.TableSort = sortDic;
                    conditionModelDic.Add(pBlockName, viewConModel);
                }
            }
        }
        /// <summary>
        /// 设置区块分页
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="index">页码</param>
        /// <param name="size">条数</param>
        public void SetConditionPaging(string pBlockName, int index, int size)
        {
            if (!string.IsNullOrEmpty(pBlockName))
            {
                if (conditionModelDic.ContainsKey(pBlockName))
                {
                    conditionModelDic[pBlockName].PageIndex = index;
                    conditionModelDic[pBlockName].PageSize = size;
                }
                else
                {
                    ViewsConditionModel viewConModel = new ViewsConditionModel();
                    viewConModel.PageIndex = index;
                    viewConModel.PageSize = size;
                    conditionModelDic.Add(pBlockName, viewConModel);
                }
            }
        }
        /// <summary>
        /// 区块查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public void SearchData(string pBlockName)
        {
            if (!string.IsNullOrEmpty(pBlockName))
            {
                PresentationBlockModel pBlockModel = MainView.GetPresentationBlockModel(pBlockName);
                if (pBlockModel != null)
                {
                    if (conditionModelDic.ContainsKey(pBlockName))
                    {
                        pBlockModel.SearchData(conditionModelDic[pBlockName]);
                        conditionModelDic.Remove(pBlockName);
                    }
                    else
                    {
                        pBlockModel.SearchData();
                    }
                }
            }
        }
        /// <summary>
        /// 获取BlockData的数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public void GetPBlockData(string pBlockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            pBlock.GetData();
        }
        /// <summary>
        /// 获取BlockData行数
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public int GetPBlockDataRowCount(string pBlockName)
        {
            int rowcount = 0;
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                rowcount = pBlock.ViewBlockDataTable.Rows.Count;
            }
            return rowcount;
        }
        /// <summary>
        /// 提交BlockData的数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oav">接受oav返回结果true或false</param>
        /// <param name="isSaveServer">是否提交服务器</param>
        /// <returns>返回保存成功true，失败false</returns>
        public void SavePBlockData(string pBlockName, object oav = null, bool isSaveServer = true)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            bool result = pBlock.SaveData(isSaveServer);
            if(!string.IsNullOrEmpty(oav.ToString()))
            {
                dynamic o = oav;
                o.v = result;
            }
        }
        /// <summary>
        /// 提交BlockData的数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="isSaveServer">是否提交服务器</param>
        /// <returns>返回保存成功true，失败false</returns>
        public bool SavePBlockData(string pBlockName, bool isSaveServer = true)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            return pBlock.SaveData(isSaveServer);
        }
        /// <summary>
        /// 设置block选中行数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public void SetPBlockCurrentRow(string pBlockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            if (pBlock != null && pBlock.ViewBlockDataTable.Rows.Count > 0)
            {
                pBlock.PreBlockSelectedRow = pBlock.ViewBlockDataTable.Rows[0];
            }
        }
        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oav">接收OAV</param>
        public void PBlockAddRow(string pBlockName, object oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);

            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                DataRow dr = pBlock.ViewBlockDataTable.NewRow();
                dr["_id"] = Guid.NewGuid().ToString();
                pBlock.ViewBlockDataTable.Rows.Add(dr);
                if (oav != null)
                {
                    dynamic o = oav;
                    o.v = dr["_id"];
                }
            }
        }
        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oav">接收OAV</param>
        public void PBlockAddRow(string pBlockName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                DataRow dr = pBlock.ViewBlockDataTable.NewRow();
                dr["_id"] = Guid.NewGuid().ToString();
                pBlock.ViewBlockDataTable.Rows.Add(dr);
                if (oav != null)
                    oav.AtrributeValue = dr["_id"];
            }
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="position">插入位置</param>
        /// <param name="oav">接收oav</param>
        public void BlockInsertRow(string blockName, int position, object oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                DataRow dr = pBlock.ViewBlockDataTable.NewRow();
                dr["_id"] = Guid.NewGuid().ToString();
                pBlock.ViewBlockDataTable.Rows.InsertAt(dr, position);
                if (oav != null)
                {
                    dynamic o = oav;
                    o.v = dr["_id"];
                }
            }
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="position">插入位置</param>
        /// <param name="oav">接收oav</param>
        public void BlockInsertRow(string blockName, int position, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                DataRow dr = pBlock.ViewBlockDataTable.NewRow();
                dr["_id"] = Guid.NewGuid().ToString();
                pBlock.ViewBlockDataTable.Rows.InsertAt(dr, position);
                if (oav != null)
                    oav.AtrributeValue = dr["_id"];
            }
        }
        /// <summary>
        /// 设置元素选中行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="element">元素</param>
        public void SetPBlockCurrentRowByElement(string pBlockName, FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(pBlockName) && element != null)
            {
                PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
                if (pBlock != null)
                {
                    string typeName = element.GetType().Name;
                    switch (typeName)
                    {
                        case "VicDataGrid":
                            VicDataGrid datagrid = element as VicDataGrid;
                            if (datagrid.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)datagrid.SelectedItem).Row;
                            }
                            break;
                        case "VicTreeView":
                            VicTreeView treeview = element as VicTreeView;
                            if (treeview.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)treeview.SelectedItem).Row;
                            }
                            break;
                        case "VicListBoxNormal":
                            VicListBoxNormal listbox = element as VicListBoxNormal;
                            if (listbox.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)listbox.SelectedItem).Row;
                            }
                            break;
                        case "VicListViewNormal":
                            VicListViewNormal listview = element as VicListViewNormal;
                            if (listview.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)listview.SelectedItem).Row;
                            }
                            break;
                        default:
                            break;
                    }
                    pBlock.SetCurrentRow(pBlock.PreBlockSelectedRow);
                }
            }
        }
        /// <summary>
        /// 设置元素选中行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="name">元素名称</param>
        public void SetPBlockCurrentRowByName(string pBlockName, string name)
        {
            FrameworkElement element = MainView.FindName(name) as FrameworkElement;
            if (!string.IsNullOrEmpty(pBlockName) && element != null)
            {
                PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
                if (pBlock != null)
                {
                    string typeName = element.GetType().Name;
                    switch (typeName)
                    {
                        case "VicDataGrid":
                            VicDataGrid datagrid = element as VicDataGrid;
                            if (datagrid.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)datagrid.SelectedItem).Row;
                            }
                            break;
                        case "VicTreeView":
                            VicTreeView treeview = element as VicTreeView;
                            if (treeview.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)treeview.SelectedItem).Row;
                            }
                            break;
                        case "VicListBoxNormal":
                            VicListBoxNormal listbox = element as VicListBoxNormal;
                            if (listbox.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)listbox.SelectedItem).Row;
                            }
                            break;
                        case "VicListViewNormal":
                            VicListViewNormal listview = element as VicListViewNormal;
                            if (listview.SelectedItem == null)
                            {
                                pBlock.PreBlockSelectedRow = null;
                            }
                            else
                            {
                                pBlock.PreBlockSelectedRow = ((DataRowView)listview.SelectedItem).Row;
                            }
                            break;
                        default:
                            break;
                    }
                    pBlock.SetCurrentRow(pBlock.PreBlockSelectedRow);
                }
            }
        }
        /// <summary>
        /// 获取选中行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowGet(string pblockName, string paramName, object oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock.PreBlockSelectedRow != null && pBlock.ViewBlockDataTable.Columns.Contains(paramName))
            {
                dynamic o = oav;
                o.v = pBlock.PreBlockSelectedRow[paramName];
            }
        }
        /// <summary>
        /// 获取选中行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowGet(string pblockName, string paramName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock.PreBlockSelectedRow != null && pBlock.ViewBlockDataTable.Columns.Contains(paramName))
            {
                oav.AtrributeValue = pBlock.PreBlockSelectedRow[paramName];
            }
        }
        /// <summary>
        /// 获取选中行的上一行列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowUpGet(string pblockName, string fieldName, object oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null && !string.IsNullOrEmpty(fieldName) && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int index = 1;
                    if (Int32.TryParse(pBlock.PreBlockSelectedRow[fieldName].ToString(), out index))
                    {
                        dynamic o = oav;
                        if (index > 1)
                        {
                            o.v = index - 1;
                        }
                        else if (index == 1)
                        {
                            o.v = index;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取选中行的上一行列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowUpGet(string pblockName, string fieldName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null && !string.IsNullOrEmpty(fieldName) && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int index = 1;
                    if (Int32.TryParse(pBlock.PreBlockSelectedRow[fieldName].ToString(), out index))
                    {
                        if (index > 1)
                        {
                            oav.AtrributeValue = index - 1;
                        }
                        else if (index == 1)
                        {
                            oav.AtrributeValue = index;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取选中行的下一行列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowDownGet(string pblockName, string fieldName, object oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null && !string.IsNullOrEmpty(fieldName) && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int index = 1;
                    if (Int32.TryParse(pBlock.PreBlockSelectedRow[fieldName].ToString(), out index))
                    {
                        dynamic o = oav;
                        if (index < pBlock.ViewBlockDataTable.Rows.Count)
                        {
                            o.v = index + 1;
                        }
                        else if (index == pBlock.ViewBlockDataTable.Rows.Count)
                        {
                            o.v = index;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取选中行的下一行列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowDownGet(string pblockName, string fieldName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null && !string.IsNullOrEmpty(fieldName) && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int index = 1;
                    if (Int32.TryParse(pBlock.PreBlockSelectedRow[fieldName].ToString(), out index))
                    {
                        if (index < pBlock.ViewBlockDataTable.Rows.Count)
                        {
                            oav.AtrributeValue = index + 1;
                        }
                        else if (index == pBlock.ViewBlockDataTable.Rows.Count)
                        {
                            oav.AtrributeValue = index;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 数据行列值交换
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="valueCurrentRow">当行选择行指定列的值</param>
        /// <param name="valueUpOrDown">当前行的上一行或下一行指定列的值</param>
        /// <param name="fieldName">字段名称</param>
        public void BlockDataExChange(string pblockName, object valueCurrentRow, object valueUpOrDown, string fieldName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null && valueCurrentRow != null && valueUpOrDown != null)
            {
                DataRow[] drCurrent = pBlock.ViewBlockDataTable.Select("" + fieldName + "='" + valueCurrentRow + "'");
                DataRow[] drUpOrDown = pBlock.ViewBlockDataTable.Select("" + fieldName + "='" + valueUpOrDown + "'");
                if (drCurrent.Length > 0 && drUpOrDown.Length > 0)
                {
                    drCurrent[0][fieldName] = valueUpOrDown;
                    drUpOrDown[0][fieldName] = valueCurrentRow;
                }
            }
        }
        /// <summary>
        /// 获取最大序号加1
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oav">接收oav</param>
        public void GetMaxNumber(string pblockName, string fieldName, object oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int item_number = 0;
                    dynamic o = oav;
                    if (Int32.TryParse(pBlock.ViewBlockDataTable.Compute("max(" + fieldName + ")", string.Empty).ToString(), out item_number))
                    {
                        o.v = item_number + 1;
                    }
                    else
                    {
                        o.v = 1;
                    }
                }
            }
        }
        /// <summary>
        /// 获取最大序号加1
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oav">接收oav</param>
        public void GetMaxNumber(string pblockName, string fieldName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int item_number = 0;
                    if (Int32.TryParse(pBlock.ViewBlockDataTable.Compute("max(" + fieldName + ")", string.Empty).ToString(), out item_number))
                    {
                        oav.AtrributeValue = item_number + 1;
                    }
                    else
                    {
                        oav.AtrributeValue = 1;
                    }
                }
            }
        }
        /// <summary>
        /// 重新排序
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">排序字段名称</param>
        public void SetBlockAgainOrder(string pblockName, string fieldName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                int RowCount = 0;
                foreach (DataRow Dr in pBlock.ViewBlockDataTable.Rows)
                {
                    if (Dr.RowState != DataRowState.Deleted && Dr.RowState != DataRowState.Detached)
                    {
                        RowCount++;
                        Dr[fieldName] = RowCount;
                    }
                }
            }
        }
        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">排序字段名称</param>
        /// <param name="sort">排序方式1正序-1倒序</param>
        public void SetBlockOrder(string pblockName, string fieldName, int sort = 1)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                if (sort == 1)
                {
                    pBlock.ViewBlockDataTable.DefaultView.Sort = fieldName + " asc";
                }
                if (sort == -1)
                {
                    pBlock.ViewBlockDataTable.DefaultView.Sort = fieldName + " desc";
                }
            }
        }
        /// <summary>
        /// 赋值行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="rowid">行id</param>
        /// <param name="paramName">列名</param>
        /// <param name="paramValue">列值</param>
        public void ParamsRowSet(string pblockName, object rowid, string paramName, object paramValue)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock.ViewBlockDataTable.Columns.Contains(paramName))
            {
                DataRow[] drSelected = pBlock.ViewBlockDataTable.Select("_id='" + rowid.ToString() + "'");
                if (drSelected.Length > 0)
                {
                    drSelected[0][paramName] = paramValue;
                }
            }
        }
        /// <summary>
        /// 赋值选中行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="value">值</param>
        public void ParamsCurrentRowSet(string pblockName, string paramName, object value)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock.PreBlockSelectedRow != null && pBlock.ViewBlockDataTable.Columns.Contains(paramName))
            {
                DataRow[] drSelected = pBlock.ViewBlockDataTable.Select("_id='" + pBlock.PreBlockSelectedRow["_id"].ToString() + "'");
                if (drSelected.Length > 0)
                {
                    drSelected[0][paramName] = value;
                    pBlock.PreBlockSelectedRow = drSelected[0];
                }
            }
        }
        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void VicDataGridSelectRowDelete(string pblockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null)
            {
                DataRow[] drTem = pBlock.ViewBlockDataTable.Select("_id='" + pBlock.PreBlockSelectedRow["_id"].ToString() + "'");
                if (drTem.Length > 0)
                {
                    if (drTem[0].RowState == DataRowState.Added)
                    {
                        pBlock.ViewBlockDataTable.Rows.Remove(drTem[0]);
                    }
                    else
                    {
                        drTem[0].Delete();
                    }
                }
            }
        }
        /// <summary>
        /// 清除区块数据
        /// </summary>
        /// <param name="blockName">区块名称</param>
        public void ClearBlockData(string blockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                for (int i = 0; i < pBlock.ViewBlockDataTable.Rows.Count; i++)
                {
                    if (pBlock.ViewBlockDataTable.Rows[i].RowState == DataRowState.Added)
                    {
                        pBlock.ViewBlockDataTable.Rows.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        pBlock.ViewBlockDataTable.Rows[i].Delete();
                    }
                }
            }
        }
        /// <summary>
        /// 区块选中的数据集合转换OAV
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        public void BlockSelectRowsToOAV(string blockName, string fieldName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null && pBlock.ViewBlockDataTable.Columns.Contains(fieldName) && pBlock.ViewBlockDataTable.Columns.Contains("VicCheckFlag"))
            {
                List<OAVModel> listOAV = new List<OAVModel>();
                foreach (DataRow dr in pBlock.ViewBlockDataTable.Rows)
                {
                    if (Convert.ToBoolean(dr["VicCheckFlag"].ToString()))
                    {
                        string objectName = blockName + ":" + dr["_id"].ToString();
                        foreach (DataColumn col in pBlock.ViewBlockDataTable.Columns)
                        {
                            if (col.ColumnName == fieldName)
                            {
                                OAVModel oavNew = new OAVModel(objectName, col.ColumnName, dr[col.ColumnName]);
                                MainView.FeiDaoFSM.InsertFact(oavNew);
                                listOAV.Add(oavNew);
                            }
                        }
                    }
                }
                //存在oav清除
                if (blockOAVDic.ContainsKey(blockName) && blockOAVDic[blockName].ContainsKey(fieldName))
                {
                    try
                    {
                        foreach (OAVModel oav in blockOAVDic[blockName][fieldName])
                        {
                            MainView.FeiDaoFSM.RemoveFact(oav);
                        }
                        blockOAVDic[blockName].Remove(fieldName);
                    }
                    catch (Exception ex)
                    {
                        blockOAVDic[blockName].Remove(fieldName);
                        LoggerHelper.Error(ex.ToString());
                    }
                }
                if (blockOAVDic.ContainsKey(blockName))
                {
                    blockOAVDic[blockName].Add(fieldName, listOAV);
                }
                else
                {
                    Dictionary<string, List<OAVModel>> dicOAV = new Dictionary<string, List<OAVModel>>();
                    dicOAV.Add(fieldName, listOAV);
                    blockOAVDic.Add(blockName, dicOAV);
                }
            }
        }
        /// <summary>
        /// 区块数据转换OAV
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        public void BlockToOAV(string blockName, string fieldName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                List<OAVModel> listOAV = new List<OAVModel>();
                foreach (DataRow dr in pBlock.ViewBlockDataTable.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted) continue;
                    string objectName = blockName + ":" + dr["_id"].ToString();
                    foreach (DataColumn col in pBlock.ViewBlockDataTable.Columns)
                    {
                        if (col.ColumnName == fieldName)
                        {
                            OAVModel oavNew = new OAVModel(objectName, col.ColumnName, dr[col.ColumnName]);
                            MainView.FeiDaoFSM.InsertFact(oavNew);
                            listOAV.Add(oavNew);
                        }
                    }
                }
                //存在oav清除
                if (blockOAVDic.ContainsKey(blockName) && blockOAVDic[blockName].ContainsKey(fieldName))
                {
                    try
                    {
                        foreach (OAVModel oav in blockOAVDic[blockName][fieldName])
                        {
                            MainView.FeiDaoFSM.RemoveFact(oav);
                        }
                        blockOAVDic[blockName].Remove(fieldName);
                    }
                    catch (Exception ex)
                    {
                        blockOAVDic[blockName].Remove(fieldName);
                        LoggerHelper.Error(ex.ToString());
                    }
                }
                if (blockOAVDic.ContainsKey(blockName))
                {
                    blockOAVDic[blockName].Add(fieldName, listOAV);
                }
                else
                {
                    Dictionary<string, List<OAVModel>> dicOAV = new Dictionary<string, List<OAVModel>>();
                    dicOAV.Add(fieldName, listOAV);
                    blockOAVDic.Add(blockName, dicOAV);
                }
            }
        }
        /// <summary>
        /// 清除区块生成的OAV
        /// </summary>
        /// <param name="blockName">区块名称</param>
        public void ClearOAVByBlock(string blockName)
        {
            //存在oav清除
            if (blockOAVDic.ContainsKey(blockName))
            {
                try
                {
                    foreach (string keyFieldName in blockOAVDic[blockName].Keys)
                    {
                        foreach (OAVModel oav in blockOAVDic[blockName][keyFieldName])
                        {
                            MainView.FeiDaoFSM.RemoveFact(oav);
                        }
                    }
                    blockOAVDic.Remove(blockName);
                }
                catch (Exception ex)
                {
                    blockOAVDic.Remove(blockName);
                    LoggerHelper.Error(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 设置选中集合
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        public void SetSelectRows(string pblockName, string fieldName, object fieldValue)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null && pBlock.ViewBlockDataTable.Columns.Contains("VicCheckFlag") && pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
            {
                DataRow[] drSelect = pBlock.ViewBlockDataTable.Select(fieldName + "='" + fieldValue.ToString() + "'");
                foreach (DataRow dr in drSelect)
                {
                    dr["VicCheckFlag"] = true;
                }
            }
        }
        /// <summary>
        /// 设置全不选
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void SetSelectRowsNull(string pblockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null && pBlock.ViewBlockDataTable.Columns.Contains("VicCheckFlag"))
            {
                foreach (DataRow dr in pBlock.ViewBlockDataTable.Rows)
                {
                    dr["VicCheckFlag"] = false;
                }
            }
        }
        /// <summary>
        /// 设置全选
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void SetSelectRowsAll(string pblockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null && pBlock.ViewBlockDataTable.Columns.Contains("VicCheckFlag"))
            {
                foreach (DataRow dr in pBlock.ViewBlockDataTable.Rows)
                {
                    dr["VicCheckFlag"] = true;
                }
            }
        }
        /// <summary>
        /// 获取最大序号加1(字段中首字母为字母)
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="firstLetter">首字母</param>
        /// <param name="oav">接收oav</param>
        public void GetMaxNumberFromOneLetter(string pblockName, string fieldName, string firstLetter, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int i = 0;
                    foreach (DataRow row in pBlock.ViewBlockDataTable.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted)
                            continue;
                        int param = 0;
                        int.TryParse(row[fieldName].ToString().Substring(1), out param);
                        i = i > param ? i : param;
                    }
                    oav.AtrributeValue = firstLetter + (i + 1).ToString();
                }
            }
        }
    }
}
