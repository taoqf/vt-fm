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

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 飞道原子操作映射类
    /// </summary>
    public class FeiDaoOperation
    {
        #region 查询条件操作
        /// <summary>
        /// 存储查询条件
        /// </summary>
        private Dictionary<string, ViewsConditionModel> conditionModelDic = new Dictionary<string, ViewsConditionModel>();
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
        /// 查询
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

        private TemplateControl MainView;
        /// <summary>
        /// 构造函数，页面/组件实体
        /// </summary>
        /// <param name="mainView"></param>
        public FeiDaoOperation(TemplateControl mainView)
        {
            MainView = mainView;
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
            Console.WriteLine(consoleText);
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
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            pBlock.SaveData(true);
        }
        /// <summary>
        /// 设置block选中行数据
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="oav"></param>
        public void SetPBlockCurrentRow(string blockName,OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            if (pBlock != null && pBlock.ViewBlockDataTable.Rows.Count > 0)
            {
                pBlock.PreBlockSelectedRow = pBlock.ViewBlockDataTable.Rows[0];
                oav.AtrributeValue = pBlock.PreBlockSelectedRow;
            }
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
            MainView.FeiDaoFSM.SetFocus(groupName);
        }
        /// <summary>
        /// 插入事实
        /// </summary>
        /// <param name="oav"></param>
        public void InsertFact(OAVModel oav)
        {
            MainView.FeiDaoFSM.InsertFact(oav);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav"></param>
        public void RemoveFact(OAVModel oav)
        {
            MainView.FeiDaoFSM.RemoveFact(oav);
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
        public void ParamsCurrentRowSet(OAVModel oavSelectedRow, string paramName, OAVModel oav)
        {
            DataRow drSelected = oavSelectedRow.AtrributeValue as DataRow;
            if (drSelected != null && drSelected.Table.Columns.Contains(paramName))
            {
                drSelected[paramName] = oav.AtrributeValue;
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
                win.Close();
        }
        /// <summary>
        /// 弹出提示信息
        /// </summary>
        /// <param name="messageInfo"></param>
        public void ShowMessage(string messageInfo)
        {
            VicMessageBoxNormal.Show(messageInfo);
        }
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="content">输出内容</param>
        public void SysFeiDaoLog(string content)
        {
            LoggerHelper.Info(content);
        }
        /// <summary>
        /// 值是否为null
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public bool IsNUll(object paramValue)
        {
            return paramValue == null ? true : false;
        }

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
                txtBox.VicText = oav.AtrributeValue==null?"":oav.AtrributeValue.ToString();
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
        #endregion

        #region 状态实体操作
        /// <summary>
        /// 设置警戒条件
        /// </summary>
        /// <param name="se"></param>
        /// <param name="oav"></param>
        /// <param name="oavmsg"></param>
        public void SetActionGuard(StateTransitionModel se, OAVModel oav,OAVModel oavmsg)
        {
            if (oav.AtrributeValue != null && (bool)oav.AtrributeValue)
                se.ActionGuard = true;
            else
            {
                VicMessageBoxNormal.Show(oavmsg.AtrributeValue.ToString());
                se.ActionGuard = false;
            }
        }

        #endregion
    }
}
