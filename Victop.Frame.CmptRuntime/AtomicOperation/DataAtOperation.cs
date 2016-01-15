using System;
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
        public void SavePBlockData(string pBlockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pBlockName);
            pBlock.SaveData(true);
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
                pBlock.PreBlockSelectedRow = datagrid.SelectedItem != null ? ((DataRowView)datagrid.SelectedItem).Row : null;
            }
        }

        /// <summary>
        /// 移动选择行
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="oavdirection">方向oav</param>
        /// <param name="oavfield">排序字段oav</param>
        public void VicDataGridSelectRowMove(string pblockName, OAVModel oavdirection, OAVModel oavfield)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock != null && pBlock.PreBlockSelectedRow != null && oavfield.AtrributeValue != null && !string.IsNullOrEmpty(oavfield.AtrributeValue.ToString()))
            {
                string field = oavfield.AtrributeValue.ToString();
                DataRow[] drTem = pBlock.ViewBlockDataTable.Select("_id='" + pBlock.PreBlockSelectedRow["_id"].ToString() + "'");
                if (drTem.Length > 0)
                {
                    if (oavdirection.AtrributeValue.Equals("up"))
                    {
                        int order = 0;
                        if (Int32.TryParse(drTem[0][field].ToString(), out order))
                        {
                            if (order > 1)
                            {
                                DataRow[] drSub = pBlock.ViewBlockDataTable.Select("" + field + "='" + (order - 1).ToString() + "'");
                                if (drSub.Length > 0)
                                {
                                    drSub[0][field] = order;
                                    drTem[0][field] = order - 1;
                                }
                            }
                        }
                    }
                    if (oavdirection.AtrributeValue.Equals("down"))
                    {
                        int order = 0;
                        if (Int32.TryParse(drTem[0][field].ToString(), out order))
                        {
                            if (order < pBlock.ViewBlockDataTable.Rows.Count)
                            {
                                DataRow[] drAdd = pBlock.ViewBlockDataTable.Select("" + field + "='" + (order + 1).ToString() + "'");
                                if (drAdd.Length > 0)
                                {
                                    drAdd[0][field] = order + 1;
                                    drTem[0][field] = order;
                                }
                            }
                        }
                    }
                    pBlock.ViewBlockDataTable.DefaultView.Sort = "" + field + " asc";
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

    }
}
