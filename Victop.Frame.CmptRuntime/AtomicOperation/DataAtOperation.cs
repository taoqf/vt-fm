﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
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
        /// 新增行参数
        /// </summary>
        private Dictionary<string, Dictionary<string, object>> rowAddParamDic = new Dictionary<string, Dictionary<string, object>>();
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public DataAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }
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
        /// <param name="oav">事实</param>
        public void SetConditionSearchIn(string pBlockName, string paramField, OAVModel oav)
        {
            if (!string.IsNullOrEmpty(pBlockName) && !string.IsNullOrEmpty(paramField) && oav != null)
            {
                List<object> list = oav.AtrributeValue as List<object>;
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
        #endregion
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
        /// 提交BlockData的数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="isSaveServer">是否提交服务器</param>
        public void SavePBlockData(string pBlockName,bool isSaveServer=true)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            pBlock.SaveData(isSaveServer);
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
        public void PBlockAddRow(string pBlockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                DataRow dr = pBlock.ViewBlockDataTable.NewRow();
                dr["_id"] = Guid.NewGuid().ToString();
                if (rowAddParamDic.ContainsKey(pBlockName))
                {
                    Dictionary<string, object> dicParam = rowAddParamDic[pBlockName];
                    foreach (string key in dicParam.Keys)
                    {
                        if (dr.Table.Columns.Contains(key))
                        {
                            dr[key] = dicParam[key];
                        }
                    }
                    rowAddParamDic.Remove(pBlockName);
                }

                pBlock.ViewBlockDataTable.Rows.Add(dr);
            }
        }
        /// <summary>
        /// 新增行参数
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="paramValue">字段值</param>
        public void PBlockAddRowParam(string pBlockName, string fieldName, object paramValue)
        {
            if (!rowAddParamDic.ContainsKey(pBlockName))
                rowAddParamDic.Add(pBlockName, new Dictionary<string, object>());

            Dictionary<string, object> dicParam = rowAddParamDic[pBlockName];
            if (dicParam.ContainsKey(fieldName))
            {
                dicParam[fieldName] = paramValue;
            }
            else
            {
                dicParam.Add(fieldName, paramValue);
            }
        }
        /// <summary>
        /// 设置选中数据通过dataGrid控件
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="dgrid">dataGird控件</param>
        public void SetPBlockCurrentRowByDataGrid(string pBlockName, FrameworkElement dgrid)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            VicDataGrid datagrid = dgrid as VicDataGrid;
            if (pBlock != null && pBlock.ViewBlockDataTable.Rows.Count > 0 && datagrid != null)
            {
                if (datagrid.SelectedItem != null)
                {
                    pBlock.PreBlockSelectedRow = ((DataRowView)datagrid.SelectedItem).Row;
                    pBlock.SetCurrentRow(pBlock.PreBlockSelectedRow);
                }
                else
                {
                    pBlock.PreBlockSelectedRow = null;
                }
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
        /// 数据行交换
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="oavCurrent">当行选择行</param>
        /// <param name="oavUpOrDown">当前行的上一行或下一行</param>
        /// <param name="fieldName">排序字段</param>
        public void BlockDataExChange(string pblockName, OAVModel oavCurrent, OAVModel oavUpOrDown, string fieldName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null && oavCurrent.AtrributeValue != null & oavUpOrDown.AtrributeValue != null)
            {
                int indexCurrent = 1;
                int indexUpOrDown = 1;
                if (Int32.TryParse(oavCurrent.AtrributeValue.ToString(), out indexCurrent) && Int32.TryParse(oavUpOrDown.AtrributeValue.ToString(), out indexUpOrDown))
                {
                    DataRow[] drCurrent = pBlock.ViewBlockDataTable.Select("" + fieldName + "='" + oavCurrent.AtrributeValue.ToString() + "'");
                    DataRow[] drUpOrDown = pBlock.ViewBlockDataTable.Select("" + fieldName + "='" + oavUpOrDown.AtrributeValue.ToString() + "'");
                    if (drCurrent.Length > 0 && drUpOrDown.Length > 0)
                    {
                        drCurrent[0][fieldName] = indexUpOrDown;
                        drUpOrDown[0][fieldName] = indexCurrent;
                    }
                }
            }
        }
        /// <summary>
        /// 获取最大序号加1
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oav">接受oav</param>
        public void GetMaxNumber(string pblockName, string fieldName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                if (pBlock.ViewBlockDataTable.Columns.Contains(fieldName))
                {
                    int item_number = 1;
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
        /// <param name="fieldname">排序字段名称</param>
        public void SetBlockOrder(string pblockName, string fieldname)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.ViewBlockDataTable != null)
            {
                pBlock.ViewBlockDataTable.DefaultView.Sort = "order asc";
            }
        }
        /// <summary>
        /// 赋值选中行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
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
        /// 设置PBlock数据
        /// </summary>
        /// <param name="pblockNameOne">需要赋值的区块名称</param>
        /// <param name="pblockNameTwo">作为模板赋值的区块名称</param>
        /// <param name="pblockNameThree">作为赋值的区块名称</param>
        /// <param name="fildone">左表</param>
        /// <param name="fildtwo">右表</param>
        /// <param name="fildthree">左表</param>
        /// <param name="fildfour">右表</param>
        public void SetPBlockData(string pblockNameOne, string pblockNameTwo, string pblockNameThree, string fildone, string fildtwo, string fildthree, string fildfour)
        {
            if (!string.IsNullOrWhiteSpace(pblockNameOne) && !string.IsNullOrWhiteSpace(pblockNameTwo) && !string.IsNullOrWhiteSpace(pblockNameThree))
            {
                PresentationBlockModel pBlockOne = MainView.GetPresentationBlockModel(pblockNameOne);
                PresentationBlockModel pBlockTwo = MainView.GetPresentationBlockModel(pblockNameTwo);
                PresentationBlockModel pBlockThree = MainView.GetPresentationBlockModel(pblockNameThree);
                if (pBlockOne != null && pBlockTwo != null && pBlockThree != null)
                {
                    pBlockOne.ViewBlockDataTable = pBlockTwo.ViewBlockDataTable.Copy();
                    DataRow dr = pBlockOne.ViewBlockDataTable.NewRow();
                    dr["_id"] = Guid.NewGuid().ToString();
                    dr[fildone] = pBlockThree.ViewBlockDataTable.Rows[0][fildtwo];
                    dr[fildthree] = pBlockThree.ViewBlockDataTable.Rows[0][fildfour];
                    pBlockOne.ViewBlockDataTable.Rows.Add(dr);
                }
            }
        }

    }
}
