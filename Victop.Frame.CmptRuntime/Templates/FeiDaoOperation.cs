using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;

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
                if (conditionModelDic.ContainsKey(pBlockName))
                {
                    pBlockModel.SearchData(conditionModelDic[pBlockName]);
                    conditionModelDic.Remove(pBlockName);
                }
                else
                {
                    ViewsConditionModel viewConModel = new ViewsConditionModel();
                    pBlockModel.SearchData(viewConModel);
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
        public void ParamsCurrentRowGet(string pblockName,string paramName, OAVModel oav)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(pblockName);
            if (pBlock.PreBlockSelectedRow != null && pBlock.ViewBlockDataTable.Columns.Contains(paramName))
            {
                oav.AtrributeValue = pBlock.PreBlockSelectedRow[paramName];
            }
        }
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="content">输出内容</param>
        public void SysFeiDaoLog(string content)
        {
            LoggerHelper.Info(content);
        }
    }
}
