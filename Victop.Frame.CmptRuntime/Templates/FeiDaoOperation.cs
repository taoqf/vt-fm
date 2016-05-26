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
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Victop.Frame.DataMessageManager;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 飞道原子操作映射类
    /// </summary>
    [ComVisible(true)]
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
        /// <summary>
        /// UI元素原子操作
        /// </summary>
        private UIElementAtOperation uIElementOperation;
        /// <summary>
        /// 凭证原子操作
        /// </summary>
        private VoucherAtOperation vercherAtOperation;
        /// <summary>
        /// webvisio原子操作
        /// </summary>
        private WebVisioAtOperation webvisioAtOperation;
        #endregion

        /// <summary>
        /// 构造函数，页面/组件实体
        /// </summary>
        /// <param name="mainView">页面/组件实体</param>
        public FeiDaoOperation(TemplateControl mainView)
        {
            MainView = mainView;
            customServiceOperation = new CustomServiceAtOperation(MainView);
            dataOperation = new DataAtOperation(MainView);
            systemOperation = new SystemAtOperation(MainView);
            uIElementOperation = new UIElementAtOperation(MainView);
            vercherAtOperation = new VoucherAtOperation(mainView);
            webvisioAtOperation = new WebVisioAtOperation(mainView);
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

        /// <summary>
        /// 发送服务消息
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void SendHttpRequest(string requestUrl, object oav)
        {
            customServiceOperation.SendHttpRequest(requestUrl, oav);
        }

        #endregion

        #region 数据操作
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
        /// 设置区块查询条件大于小于
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="operatorStr">操作符</param>
        public void SetConditionSearchMoreOrLess(string pBlockName, string paramField, object paramValue,
                                                 string operatorStr)
        {
            dataOperation.SetConditionSearchMoreOrLess(pBlockName, paramField, paramValue, operatorStr);
        }

        /// <summary>
        /// 设置区块查询条件In子查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="listIn">In子查询集合</param>
        public void SetConditionSearchIn(string pBlockName, string paramField, object listIn)
        {
            dataOperation.SetConditionSearchIn(pBlockName, paramField, listIn);
        }
        /// <summary>
        /// 设置区块查询条件NotIn子查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="listNotIn">NotIn子查询集合</param>
        public void SetConditionSearchNotIn(string pBlockName, string paramField, object listNotIn)
        {
            dataOperation.SetConditionSearchNotIn(pBlockName, paramField, listNotIn);
        }
        /// <summary>
        /// 设置区块查询条件"不等于"查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="NotEqual">不等于的值</param>
        public void SetConditionSearchNotEqual(string pBlockName, string paramField, object NotEqual)
        {
            dataOperation.SetConditionSearchNotEqual(pBlockName, paramField, NotEqual);
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
        /// <summary>
        /// 设置区块是否取空数据
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="isEmptyData">true取空数据</param>
        public void SetConditionIsEmptyData(string blockName, bool isEmptyData)
        {
            dataOperation.SetConditionIsEmptyData(blockName, isEmptyData);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public void SearchData(string pBlockName)
        {
            dataOperation.SearchData(pBlockName);
        }
        /// <summary>
        /// 获取BlockData的数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public void GetPBlockData(string pBlockName)
        {
            dataOperation.GetPBlockData(pBlockName);
        }
        /// <summary>
        /// 获取BlockData行数
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oav">接受oav</param>
        public void GetPBlockDataRowCount(string pBlockName, object oav)
        {
            dataOperation.GetPBlockDataRowCount(pBlockName, oav);
        }

        /// <summary>
        /// 获取block数据指定条件的列值
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="condition">条件</param>
        /// <param name="columnName">列名</param>
        /// <param name="oav">接收OAV</param>
        public void GetPBlockDataRowColumnValue(string pBlockName, string condition, string columnName, object oav)
        {
            dataOperation.GetPBlockDataRowColumnValue(pBlockName, condition, columnName, oav);
        }

        /// <summary>
        /// 设置元素选中行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="element">元素</param>
        public void SetPBlockCurrentRowByElement(string pBlockName, object element)
        {
            dataOperation.SetPBlockCurrentRowByElement(pBlockName, element);
        }
        /// <summary>
        /// 设置元素选中行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="name">元素名称</param>
        public void SetPBlockCurrentRowByName(string pBlockName, string name)
        {
            dataOperation.SetPBlockCurrentRowByName(pBlockName, name);
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
            dataOperation.SavePBlockData(pBlockName, oav, isSaveServer);
        }
        /// <summary>
        /// 设置block选中行数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        public void SetPBlockCurrentRow(string pBlockName)
        {
            dataOperation.SetPBlockCurrentRow(pBlockName);
        }
        /// <summary>
        /// 根据指定行设置block选中行数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oavdr">指定行</param>
        public void SetPBlockCurrentRowByDataRow(string pBlockName, object oavdr)
        {
            dataOperation.SetPBlockCurrentRowByDataRow(pBlockName, oavdr);
        }
        /// <summary>
        /// 把行集合中指定列转换成逗号拼接的字符串
        /// </summary>
        /// <param name="oavdrs">datarow集合</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oavstr">指定行</param>
        /// <param name="listIndex">datarow集合索引</param>
        public void GetStrFromListDataRow(object oavdrs, string fieldName, object oavstr, int listIndex = -1)
        {
            dataOperation.GetStrFromListDataRow(oavdrs, fieldName, oavstr, listIndex);
        }
        /// <summary>
        /// 把行集合中指定列转换成字符串集合
        /// </summary>
        /// <param name="oavdrs">datarow集合</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oavstr">指定行</param>
        public void GetListFromListDataRow(object oavdrs, string fieldName, object oavstr)
        {
            dataOperation.GetListFromListDataRow(oavdrs, fieldName, oavstr);
        }
        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oav">接收OAV</param>
        public void PBlockAddRow(string pBlockName, object oav)
        {
            dataOperation.PBlockAddRow(pBlockName, oav);
        }
        /// <summary>
        /// 根据OAV和旧值更新block中的数据
        /// </summary>
        /// <param name="block转换的oav">oav</param>
        /// <param name="需要更新的旧值">oldValue</param>
        /// <param name="新值">newValue</param>
        public void UpdateBlockByOAV(object oav, object oldValue, object newValue)
        {
            dataOperation.UpdateBlockByOAV(oav, oldValue, newValue);
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="position">插入位置</param>
        /// <param name="oav">接收oav</param>
        public void BlockInsertRow(string blockName, int position, object oav)
        {
            dataOperation.BlockInsertRow(blockName, position, oav);
        }
        /// <summary>
        /// 获取选中行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowGet(string pblockName, string paramName, object oav)
        {
            dataOperation.ParamsCurrentRowGet(pblockName, paramName, oav);
        }
        /// <summary>
        /// 获取最后一行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsLastRowGet(string pblockName, string paramName, object oav)
        {
            dataOperation.ParamsLastRowGet(pblockName, paramName, oav);
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
            dataOperation.ParamsRowSet(pblockName, rowid, paramName, paramValue);
        }
        /// <summary>
        /// 获取选中行的上一行列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowUpGet(string pblockName, string paramName, object oav)
        {
            dataOperation.ParamsCurrentRowUpGet(pblockName, paramName, oav);
        }
        /// <summary>
        /// 获取选中行的下一行列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowDownGet(string pblockName, string paramName, object oav)
        {
            dataOperation.ParamsCurrentRowDownGet(pblockName, paramName, oav);
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
            dataOperation.BlockDataExChange(pblockName, valueCurrentRow, valueUpOrDown, fieldName);
        }
        /// <summary>
        /// 获取最大序号加1
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oav">接收oav</param>
        public void GetMaxNumber(string pblockName, string fieldName, object oav)
        {
            dataOperation.GetMaxNumber(pblockName, fieldName, oav);
        }
        /// <summary>
        /// 获取最小序号减1
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oav">接收oav</param>
        public void GetMinNumber(string pblockName, string fieldName, object oav)
        {
            dataOperation.GetMinNumber(pblockName, fieldName, oav);
        }
        /// <summary>
        /// 重新排序
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">排序字段名称</param>
        public void SetBlockAgainOrder(string pblockName, string fieldName)
        {
            dataOperation.SetBlockAgainOrder(pblockName, fieldName);
        }
        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">排序字段名称</param>
        /// <param name="sort">排序方式1正序-1倒序</param>
        public void SetBlockOrder(string pblockName, string fieldName, int sort = 1)
        {
            dataOperation.SetBlockOrder(pblockName, fieldName, sort);
        }
        /// <summary>
        /// 赋值选中行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="value">值</param>
        public void ParamsCurrentRowSet(string pblockName, string paramName, object value)
        {
            dataOperation.ParamsCurrentRowSet(pblockName, paramName, value);
        }
        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void VicDataGridSelectRowDelete(string pblockName)
        {
            dataOperation.VicDataGridSelectRowDelete(pblockName);
        }
        /// <summary>
        /// 删除最后一行
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void VicDataGridtLastRowDelete(string pblockName)
        {
            dataOperation.VicDataGridtLastRowDelete(pblockName);
        }
        /// <summary>
        /// 区块数据转换OAV
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        public void BlockToOAV(string blockName, string fieldName)
        {
            dataOperation.BlockToOAV(blockName, fieldName);
        }
        /// <summary>
        /// 批量修改区块选中的数据集合中的某列值
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fileValue">字段值</param>
        public void BlockSelectRowsUpdate(string blockName, string filedName, string filedValue)
        {
            dataOperation.BlockSelectRowsUpdate(blockName, filedName, filedValue);
        }
        /// <summary>
        /// 区块选中的数据集合转换OAV
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        public void BlockSelectRowsToOAV(string blockName, string fieldName)
        {
            dataOperation.BlockSelectRowsToOAV(blockName, fieldName);
        }
        /// <summary>
        /// 清除区块生成的OAV
        /// </summary>
        /// <param name="blockName">区块名称</param>
        public void ClearOAVByBlock(string blockName)
        {
            dataOperation.ClearOAVByBlock(blockName);
        }
        /// <summary>
        /// 清除区块数据
        /// </summary>
        /// <param name="blockName">区块名称</param>
        public void ClearBlockData(string blockName)
        {
            dataOperation.ClearBlockData(blockName);
        }
        /// <summary>
        /// 设置选中集合
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        public void SetSelectRows(string pblockName, string fieldName, object fieldValue)
        {
            dataOperation.SetSelectRows(pblockName, fieldName, fieldValue);
        }
        /// <summary>
        /// 设置全不选
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void SetSelectRowsNull(string pblockName)
        {
            dataOperation.SetSelectRowsNull(pblockName);
        }
        /// <summary>
        /// 设置全选
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void SetSelectRowsAll(string pblockName)
        {
            dataOperation.SetSelectRowsAll(pblockName);
        }
        /// <summary>
        /// 获取最大序号加1(字段中首字母为字母)
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="firstLetter">首字母</param>
        /// <param name="oav">接收oav</param>
        public void GetMaxNumberFromOneLetter(string pblockName, string fieldName, string firstLetter, object oav)
        {
            dataOperation.GetMaxNumberFromOneLetter(pblockName, fieldName, firstLetter, oav);
        }
        /// <summary>
        ///复制控件内容到剪贴板
        /// </summary>
        /// <param name="elementName">控件名称</param>
        ///</summary>
        public void CopyControlContent(string elementName)
        {
            dataOperation.CopyControlContent(elementName);
        }
        /// <summary>
        ///将json转换成DT
        /// </summary>
        /// <param name="elementName">控件名称</param>
        ///</summary>
        public void JsonToDataTable(object str, string elementName)
        {
            dataOperation.JsonToDataTable(str, elementName);
        }
        /// <summary>
        /// 数据存在性验证
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oav">返回结果</param>
        public void DataExistenceVerification(string pblockName, string fieldName, object oav)
        {
            dataOperation.DataExistenceVerification(pblockName, fieldName, oav);
        }
        /// <summary>
        /// 格式正确性验证 
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="length">字段长度限制</param>
        /// <param name="oav">返回结果</param>
        public void FormatCorrectnessVerification(string pblockName, string fieldName, int length, object oav)
        {
            dataOperation.FormatCorrectnessVerification(pblockName, fieldName, length, oav);
        }
        /// <summary>
        /// 获取选中列的确定字段值集合（VicCheckFlag）
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oav">返回集合结果</param>
        public void GetDataGridColumnValueList(string pblockName, object oav, List<object> fieldName)
        {
            dataOperation.GetDataGridColumnValueList(pblockName, oav, fieldName);
        }
        /// <summary>
        /// 获取列表选中行数（VicCheckFlag）
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="oav">返回结果</param>
        /// <param name="type">类型</param>
        public void GetDataGridSelectRowsCount(string pblockName, object oav, string type = "")
        {
            dataOperation.GetDataGridSelectRowsCount(pblockName, oav, type);
        }
        /// <summary>
        /// 对确定行字段赋值
        /// </summary>
        /// <param name="pblockName"区块名称>区块名称</param>
        /// <param name="rowId">行id</param>
        /// <param name="oav">集合</param>
        /// <param name="fieldName">字段</param>
        public void SetDataGridColumnValueList(string pblockName, string rowId, object oav, List<object> fieldName)
        {
            dataOperation.SetDataGridColumnValueList(pblockName, rowId, oav, fieldName);
        }


        /// <summary>
        /// dgridName中某列字段值在界面显示时，状态转换成用户想要显示的结果
        /// </summary>
        /// <param name="dgridName">列表名称</param>
        /// <param name="fieldName">字段</param>
        /// <param name="relation">键值对关系的类型</param>
        public void SetDgridColumnValueStateChange(string dgridName, string fieldName, Dictionary<string, object> relation)
        {
            dataOperation.SetDgridColumnValueStateChange(dgridName, fieldName, relation);
        }


        #endregion

        #region 系统操作
        /// <summary>
        /// 系统输出
        /// </summary>
        /// <param name="consoleText">文本值</param>
        public void SysConsole(string consoleText)
        {
            systemOperation.SysConsole(consoleText);
        }
        /// <summary>
        /// 设置分组，对应set_focus
        /// </summary>
        /// <param name="groupName">分组信息</param>
        //public void SetFocus(string groupName)
        //{
        //    systemOperation.SetFocus(groupName);
        //}
        /// <summary>
        /// 插入事实，对应oav_insert
        /// </summary>
        /// <param name="o">o</param>
        /// <param name="a">a</param>
        /// <param name="v">v</param>
        //public void InsertFact(string o, string a, object v = null)
        //{
        //    systemOperation.InsertFact(o, a, v);
        //}
        /// <summary>
        /// 移除事实，对应oav_remove
        /// </summary>
        /// <param name="oav"></param>
        //public void RemoveFact(object oav)
        //{
        //    systemOperation.RemoveFact(oav);
        //}
        /// <summary>
        /// 修改事实，对应oav_modify
        /// </summary>
        /// <param name="oav">oav事实实例</param>
        /// <param name="v">v值</param>
        //public void UpdateFact(object oav, object v)
        //{
        //    systemOperation.UpdateFact(oav, v);
        //}
        /// <summary>
        /// 修改事实，对应oav_modify
        /// </summary>
        /// <param name="oav">oav事实</param>
        //public void UpdateFact(object oav)
        //{
        //    systemOperation.UpdateFact(oav);
        //}
        /// <summary>
        /// 提交事实，对应oav_validate
        /// </summary>
        /// <param name="oav">oav事实实例</param>
        //public void CommitFact(object oav)
        //{
        //    systemOperation.CommitFact(oav);
        //}
        /// <summary>
        /// 转移触发事件
        /// </summary>
        /// <param name="triggerName">触发事件名</param>
        /// <param name="triggerSource">触发源</param>
        public void TranslationState(string triggerName, object triggerSource)
        {
            systemOperation.TranslationState(triggerName, triggerSource);
        }
        /// <summary>
        /// 执行页面动作
        /// </summary>
        /// <param name="pageTrigger">动作名称</param>
        /// <param name="paramInfo">事件触发元素</param>
        public void ExcutePageTrigger(string pageTrigger, object paramInfo)
        {
            systemOperation.ExcutePageTrigger(pageTrigger, paramInfo);
        }
        /// <summary>
        /// 执行组件动作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="compntTrigger">动作名称</param>
        /// <param name="paramInfo">事件触发元素</param>
        public void ExcuteComponentTrigger(string compntName, string compntTrigger, object paramInfo)
        {
            systemOperation.ExcuteComponentTrigger(compntName, compntTrigger, paramInfo);
        }
        /// <summary>
        /// 获取页面参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsPageGet(string paramName, object oav)
        {
            systemOperation.ParamsPageGet(paramName, oav);
        }
        /// <summary>
        /// 获取组件参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsCompntGet(string paramName, object oav)
        {
            systemOperation.ParamsCompntGet(paramName, oav);
        }
        /// <summary>
        /// 获取List集合
        /// </summary>
        /// <returns>List集合</returns>
        public void GetList(object oav)
        {
            systemOperation.GetList(oav);
        }
        /// <summary>
        /// 获取集合中的值
        /// </summary>
        /// <param name="oav">List集合</param>
        /// <param name="index">索引</param>
        /// <param name="oavValue">返回结果</param>
        public void GetListValueByIndex(object oav, int index, object oavValue)
        {
            systemOperation.GetListValueByIndex(oav, index, oavValue);
        }
        /// <summary>
        /// 将对象加入List集合结尾处
        /// </summary>
        /// <param name="value">集合中的项</param>
        /// <param name="paramList">List集合</param>
        public void ListAdd(object value, object paramList)
        {
            systemOperation.ListAdd(value, paramList);
        }
        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="count">长度</param>
        public void GetListCount(object list, object count)
        {
            systemOperation.GetListCount(list, count);
        }
        /// <summary>
        /// 移除指定位置元素
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="setcount">移除位置</param>
        public void RemoveListSetCount(object list, int setcount)
        {
            systemOperation.RemoveListSetCount(list, setcount);
        }
        /// <summary>
        /// 获取集合指定位置元素
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="setcount">获取位置</param>
        /// <param name="getcontent">内容</param>
        public void GetListSetCountContent(object list, int setcount, object getcontent)
        {
            systemOperation.GetListSetCountContent(list, setcount, getcontent);
        }
        /// <summary>
        /// 获取ComboBox的DT
        /// </summary>
        /// <param name="oav">接收oav</param>
        /// <returns>DT</returns>
        public void GetComboBoxDt(object oav)
        {
            systemOperation.GetComboBoxDt(oav);

        }
        /// <summary>
        ///给ComboBox的Dt赋值
        /// </summary>
        /// <param name="keyValue">selectValue值</param>
        /// <param name="displayValue">displaymember值</param>
        /// <param name="paramDt">oav</param>
        public void SetComboBoxDtRow(string keyValue, string displayValue, object paramDt)
        {
            systemOperation.SetComboBoxDtRow(keyValue, displayValue, paramDt);

        }
        /// <summary>
        /// 将DT表绑到ComboBox数据源
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="paramDt">oav</param>
        public void SetComboItemsSource(string elementName, object paramDt)
        {
            systemOperation.SetComboItemsSource(elementName, paramDt);
        }
        /// <summary>
        /// 获取Dictionary中参数值
        /// </summary>
        /// <param name="oavDic">存储dic类型的oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsGetByDictionary(object oavDic, string paramName, object oav)
        {
            systemOperation.ParamsGetByDictionary(oavDic, paramName, oav);
        }
        /// <summary>
        /// 获取第一个数组中Dictionary中参数值
        /// </summary>
        /// <param name="oavDic">存储List<Dictionary>类型的oav</param>
        /// <param name="paramName">key参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsGetByListDictionary(object oavDic, string paramName, object oav)
        {
            systemOperation.ParamsGetByListDictionary(oavDic, paramName, oav);
        }
        /// <summary>
        /// 新增页面参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        public void ParamsPageAdd(string paramName, object paramValue)
        {
            systemOperation.ParamsPageAdd(paramName, paramValue);
        }
        /// <summary>
        /// 组件参数封装
        /// </summary>
        /// <param name="oavCom">组件参数</param>
        /// <param name="oavPage">页面接收参数</param>
        public void ParamsInterCompntAdd(object oavCom, object oavPage)
        {
            systemOperation.ParamsInterCompntAdd(oavCom, oavPage);
        }
        /// <summary>
        /// 组件取参数，一般情况下组件回填页面传回来的参数时用到
        /// </summary>
        /// <param name="oavParams">参数oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsInterCompntParse(object oavParams, string paramName, object oav)
        {
            systemOperation.ParamsInterCompntParse(oavParams, paramName, oav);
        }
        /// <summary>
        /// 组件取参数，一般情况下中组件从部件中取回参数时用到
        /// </summary>
        /// <param name="oavParams">参数oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void GetParamsInterCompntParse(object oavParams, string paramName, object oav)
        {
            systemOperation.GetParamsInterCompntParse(oavParams, paramName, oav);
        }
        /// <summary>
        /// 弹框展示组件操作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void UCCompntShowDialog(string compntName, int height = 600, int width = 600)
        {
            systemOperation.UCCompntShowDialog(compntName, height, width);
        }
        /// <summary>
        /// 弹框展示组件操作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void UCCompntShow(string compntName, int height = 600, int width = 600)
        {
            systemOperation.UCCompntShow(compntName, height, width);
        }
        /// <summary>
        /// 使用window弹出内容
        /// </summary>
        /// <param name="content">弹出内容</param>
        public void ShowVicWindowContent(object content)
        {
            systemOperation.ShowVicWindowContent(content);
        }
        /// <summary>
        /// 设置组件参数
        /// </summary>
        /// <param name="compntName">组件名称</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        public void SetCompntParamDic(string compntName, string paramName, object paramValue)
        {
            systemOperation.SetCompntParamDic(compntName, paramName, paramValue);
        }
        /// <summary>
        /// 弹框关闭操作
        /// </summary>
        public void UCCompntClose()
        {
            systemOperation.UCCompntClose();
        }
        /// <summary>
        /// 弹出提示信息
        /// </summary>
        /// <param name="messageInfo">消息内容</param>
        public void ShowMessage(object messageInfo)
        {
            systemOperation.ShowMessage(messageInfo);
        }
        /// <summary>
        /// 弹出提示询问
        /// </summary>
        /// <param name="messageInfo">提示信息</param>
        /// <param name="caption">标题</param>
        /// <param name="oav">接收oav</param>
        public void ShowMessageResult(object messageInfo, object caption, object oav)
        {
            systemOperation.ShowMessageResult(messageInfo, caption, oav);
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
        /// <param name="paramValue">参数值</param>
        /// <param name="oav">接收oav</param>
        public void SetParamValue(object paramValue, object oav)
        {
            systemOperation.SetParamValue(paramValue, oav);
        }
        /// <summary>
        /// 设置页面显示元素
        /// </summary>
        /// <param name="paramValue">元素名称</param>
        /// <param name="visibility">是否显示</param>
        public void SetCompontVisility(string paramValue, bool visibility)
        {
            systemOperation.SetCompontVisility(paramValue, visibility);
        }
        /// <summary>
        /// 设置警戒条件
        /// </summary>
        /// <param name="se">状体转移实体</param>
        /// <param name="oav">oav警戒值</param>
        /// <param name="oavmsg">oav消息内容</param>
        public void SetActionGuard(object se, object oav, object oavmsg)
        {
            systemOperation.SetActionGuard(se, oav, oavmsg);
        }
        /// <summary>
        /// 弹框展示当前组件中的一部分
        /// </summary>
        /// <param name="layoutName">布局名称</param>
        public void UcCurrentCompntContentShow(string layoutName)
        {
            systemOperation.UcCurrentCompntContentShow(layoutName);
        }
        /// <summary>
        /// 弹框关闭操作
        /// </summary>
        /// <param name="layoutName">布局名称</param>
        public void UcCurrentCompntContentClose(string layoutName)
        {
            systemOperation.UcCurrentCompntContentClose(layoutName);
        }
        /// <summary>
        /// 将规则文件另存
        /// </summary>
        /// <param name="content">规则内容</param>
        public void WriteTextToFile(object content)
        {
            systemOperation.WriteTextToFile(content);
        }
        /// <summary>
        /// 保存规则文件
        /// </summary>
        /// <param name="content">规则内容</param>
        /// <param name="oav">接受oav</param>
        public void SaveWriteTextToFile(object content, object oav)
        {
            systemOperation.SaveWriteTextToFile(content, oav);
        }
        /// <summary>
        /// 上传文件或者替换文件
        /// </summary>
        /// <param name="localFilePath">本地文件地址</param>
        /// <param name="filePath">新的文件路径或者老的文件路径</param>
        /// <param name="oav">接受oav</param>
        /// <param name="productId">产品ID,默认“feidao”</param>
        public void UpLoadFile(string localFilePath, object filePath, object oav, string productId = "feidao")
        {
            systemOperation.UpLoadFile(localFilePath, filePath, oav, productId);
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="fileType">文件类型(image,autio,video,file)</param>
        /// <param name="oav">接受oav文件路径</param>
        public void OpenFile(string fileType, object oav)
        {
            systemOperation.OpenFile(fileType, oav);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">下载地址</param>
        /// <param name="localFilePath">本地文件地址</param>
        /// <param name="oav">接受oav全路径</param>
        /// <param name="productId">产品路径</param>
        public void DownLoadFile(string localFilePath, string filePath, object oav, string productId = "feidao")
        {
            systemOperation.DownLoadFile(localFilePath, filePath, oav, productId);
        }
        /// <summary>
        /// 获取一个新的guid
        /// </summary>
        /// <param name="oav">接受oav</param>
        public void GetNewGuid(object oav)
        {
            systemOperation.GetNewGuid(oav);
        }
        /// <summary>
        /// 发送获取编码消息
        /// </summary>
        /// <param name="SystemId">系统ID</param>
        /// <param name="iPName">规则名称(例如:"BH005")</param>
        /// <param name="iCodeRule">编码规则</param>
        /// <param name="oav">接受oav</param>
        public void SendGetCodeMessage(string SystemId, string iPName, string iCodeRule, object oav)
        {
            systemOperation.SendGetCodeMessage(SystemId, iPName, iCodeRule, oav);
        }

        /// <summary>
        /// 发送编码服务
        /// </summary>
        /// <param name="paramDic">服务参数{"productid": "feidao","spaceid": "feidao",	"systemid": "11","no": "bm00001","templateno": "BH101"}
        /// <param name="oav">返回参数Dictionary<string, object>,key:"code","msg"</param>
        public void SendCodeServiceMessage(object paramDic, object oav)
        {
            systemOperation.SendCodeServiceMessage(paramDic, oav);
        }

        /// <summary> 
        /// 返回文件url地址 
        /// </summary> 
        /// <param name="filePath">文件编号</param> 
        /// <param name="oav">接受oav</param> 
        /// <param name="productId">产品id默认feidao</param> 
        public void GetFileUrlByFilePath(string filePath, object oav, string productId = "feidao")
        {
            systemOperation.GetFileUrlByFilePath(filePath, oav, productId);
        }
        /// <summary>
        /// 返回指定名称资源的值
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="oav">接受oav</param>
        public void GetStringByResourceName(string name, object oav)
        {
            systemOperation.GetStringByResourceName(name, oav);
        }

        #region 字符串处理
        /// <summary>
        /// 指定的字符串是否出现在字符串实例中
        /// </summary>
        /// <param name="strValue">字符串实例</param>
        /// <param name="value">指定的字符串</param>
        /// <param name="oav">接受oav</param>
        public void StrContains(object strValue, string value, object oav)
        {
            systemOperation.StrContains(strValue, value, oav);
        }
        /// <summary>
        /// 从当前 System.String 对象移除数组中指定的一组字符的所有尾部匹配项
        /// </summary>
        /// <param name="strValue">字符串实例</param>
        /// <param name="value">一组字符组成的字符串</param>
        /// <param name="oav">接受oav</param>
        public void StrTrimEnd(object strValue, string value, object oav)
        {
            systemOperation.StrTrimEnd(strValue, value, oav);
        }
        /// <summary>
        /// 从当前 System.String 对象移除数组中指定的一组字符的所有头部匹配项
        /// </summary>
        /// <param name="strValue">字符串实例</param>
        /// <param name="value">一组字符组成的字符串</param>
        /// <param name="oav">接受oav</param>
        public void StrTrimStart(object strValue, string value, object oav)
        {
            systemOperation.StrTrimStart(strValue, value, oav);
        }
        /// <summary>
        /// 比较字符串一致性
        /// </summary>
        /// <param name="firstValue">字符串实例</param>
        /// <param name="secondValue">指定的字符串</param>
        /// <param name="oav">接受oav</param>
        public void CmpStrIsEqual(string firstValue, string secondValue, object oav)
        {
            systemOperation.CmpStrIsEqual(firstValue, secondValue, oav);
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="str">字符串实例</param>
        /// <param name="oav">接收oav</param>
        public void GetStrLength(object str, object oav)
        {
            systemOperation.GetStrLength(str, oav);
        }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="str">字符串实例</param>
        /// <param name="join">指定的字符串连接符</param>
        /// <param name="oav">接收oav</param>
        /// <param name="sort">排序方式true正序false倒序</param>
        public void AppendStr(object str, string join, object oav, bool sort = true)
        {
            systemOperation.AppendStr(str, join, oav, sort);
        }



        /// <summary>
        /// 日期类型转为自定义格式字符串
        /// </summary>
        /// <param name="oav">接收oav</param>
        /// <param name="datetime">时间</param>
        /// <param name="format">转换格式，如（yyyy-MM-dd、yyyy-MM-dd HH:mm:ss、yyyy/MM/dd HH:mm等）</param>        
        public void ConvertDateTimeToString(object oav, object datetime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            systemOperation.ConvertDateTimeToString(oav, datetime, format);
        }
        #endregion

        #region 加减乘除
        /// <summary>
        /// 数相加
        /// </summary>
        /// <param name="v1">数1</param>
        /// <param name="v2">数2</param>
        /// <param name="oav">接收oav</param>
        public void NumAdd(object v1, object v2, object oav)
        {
            systemOperation.NumAdd(v1, v2, oav);
        }
        /// <summary>
        /// 数相减
        /// </summary>
        /// <param name="v1">数1</param>
        /// <param name="v2">数2</param>
        /// <param name="oav">接收oav</param>
        public void NumMinux(object v1, object v2, object oav)
        {
            systemOperation.NumMinux(v1, v2, oav);
        }
        /// <summary>
        /// 数相乘
        /// </summary>
        /// <param name="v1">数1</param>
        /// <param name="v2">数2</param>
        /// <param name="oav">接收oav</param>
        public void NumMultiply(object v1, object v2, object oav)
        {
            systemOperation.NumMultiply(v1, v2, oav);
        }
        /// <summary>
        /// 数相除
        /// </summary>
        /// <param name="v1">数1</param>
        /// <param name="v2">数2</param>
        /// <param name="oav">接收oav</param>
        public void NumDivide(object v1, object v2, object oav)
        {
            systemOperation.NumDivide(v1, v2, oav);
        }
        #endregion

        /// <summary>
        /// 四舍五入取整
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="oav">接收oav</param>
        public void RoundNumbers(object value, object oav)
        {
            systemOperation.RoundNumbers(value, oav);
        }

        #region 时间
        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <param name="oav">接受oav</param>
        /// <param name="day">指定要添加的天数默认0</param>
        public void GetDateTime(object oav, int day = 0)
        {
            systemOperation.GetDateTime(oav, day);
        }
        #endregion

        /// <summary>
        /// 比较数字大小
        /// </summary>
        /// <param name="firstNum">数字实例</param>
        /// <param name="secondNum">指定的数字</param>
        /// <param name="type">比较类型</param>
        /// <param name="oav">接受oav(>:0，<:1，=:2,条件不合法：-1)</param>
        public void CompareNum(object firstNum, object secondNum, object oav)
        {
            systemOperation.CompareNum(firstNum, secondNum, oav);
        }

        #region 获取系统变量
        /// <summary>
        /// 获取系统变量值
        /// </summary>
        /// <param name="sysVariableName">变量名称（用户编号：usercode 用户姓名：username 客户端编号：clientno 产品ID：productid 服务器时间：timestemp 命名空间:spaceid）</param>
        /// <param name="oav">接受oav</param>
        public void GetSysVariableValue(string sysVariableName, object oav)
        {
            systemOperation.GetSysVariableValue(sysVariableName, oav);
        }
        #endregion

        /// <summary>
        /// 获取Dictionary集合
        /// </summary>
        /// <returns>Dictionary集合</returns>
        public void GetDictionary(object oav)
        {
            systemOperation.GetDictionary(oav);
        }

        /// <summary>
        /// 将对象加入Dictionary集合结尾处
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="paramDic">集合</param>
        public void DictionaryAdd(string key, object value, object paramDic)
        {
            systemOperation.DictionaryAdd(key, value, paramDic);
        }

        /// <summary>
        /// 获取键值中指定key的value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="paramDic">集合</param>
        /// <param name="value">返回值</param>
        public void GetDictionaryKeyValue(string key, object paramDic, object value)
        {
            systemOperation.GetDictionaryKeyValue(key, paramDic, value);
        }
        #endregion

        #region UI操作
        /// <summary>
        /// 设置元素内容
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="content">内容</param>
        public void SetElementContent(string elementName, object content)
        {
            uIElementOperation.SetElementContent(elementName, content);
        }
        /// <summary>
        /// 设置元素是否选中
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="isChecked">是否选中</param>
        public void SetElementChecked(string elementName, bool isChecked)
        {
            uIElementOperation.SetElementChecked(elementName, isChecked);
        }
        /// <summary>
        /// 获取元素内容
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="oav">接收oav</param>
        public void GetElementContent(string elementName, object oav)
        {
            uIElementOperation.GetElementContent(elementName, oav);
        }
        /// <summary>
        /// 设置元素选中项索引
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="selectedIndex">索引</param>
        public void SetElementSelectIndex(string elementName, int selectedIndex)
        {
            uIElementOperation.SetElementSelectIndex(elementName, selectedIndex);
        }
        /// <summary>
        /// 获取界面元素名称
        /// </summary>
        /// <param name="element">界面元素</param>
        /// <param name="oav">接收oav</param>
        public void GetElementName(object element, object oav)
        {
            uIElementOperation.GetElementName(element, oav);
        }
        /// <summary>
        /// 设置按钮是否被选择
        /// </summary>
        /// <param name="paramValue">按钮的名称(Name)</param>
        /// <param name="isSelect">是否被选择</param>
        public void SetButtonSelect(string paramValue, bool isSelect)
        {
            uIElementOperation.SetButtonSelect(paramValue, isSelect);
        }
        /// <summary>
        /// 设置元素是否启用
        /// </summary>
        /// <param name="elementName">界面元素名称</param>
        /// <param name="state">状态（0启用，1不可用）</param>
        public void SetElementIsEnabled(string elementName, int state = 0)
        {
            uIElementOperation.SetElementIsEnabled(elementName, state);
        }
        /// <summary>
        /// 设置元素是否可见
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="visibility">是否显示</param>
        ///  <param name="type">占空间还是不占空间隐藏：0,不占空间;1;不占空间</param>
        public void SetElementVisility(string elementName, bool visibility, int type = 0)
        {
            uIElementOperation.SetElementVisility(elementName, visibility, type);
        }
        /// <summary>
        /// 根据DataGrid名称获取当前选择行中某一字段的值
        /// </summary>
        /// <param name="gridName">grid名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">返回OAV值</param>
        public void ParamsCurrentRowGetFromGridName(string gridName, string paramName, object oav)
        {
            uIElementOperation.ParamsCurrentRowGetFromGridName(gridName, paramName, oav);
        }
        /// <summary>
        /// 获取元素是否选中
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="oav">接收oav</param>
        public void GetElementIsChecked(string elementName, object oav)
        {
            uIElementOperation.GetElementIsChecked(elementName, oav);
        }
        /// <summary>
        /// 是否有选中项
        /// </summary>
        /// <param name="itemsControlName">控件名</param>
        /// <param name="oav">接收oav</param>
        /// <param name="oavmsg">消息oav</param>
        public void VicDataGridIsSelectItem(string itemsControlName, object oav, object oavmsg = null)
        {
            uIElementOperation.VicDataGridIsSelectItem(itemsControlName, oav, oavmsg);
        }
        /// <summary>
        /// 更新列
        /// </summary>
        /// <param name="dgridName">控件名</param>
        public void VicDataGridUpdateColumn(string dgridName)
        {
            uIElementOperation.VicDataGridUpdateColumn(dgridName);
        }
        /// <summary>
        /// VicDataGrid是否只读
        /// </summary>
        /// <param name="dgridName">控件名</param>
        /// <param name="state">状态（0启用，1不可用）</param>
        public void VicDataGridIsReadOnly(string dgridName, int state = 0)
        {
            uIElementOperation.VicDataGridIsReadOnly(dgridName, state);
        }
        /// <summary>
        /// 清除dgrid数据集合
        /// </summary>
        /// <param name="dgridName">控件名</param>
        public void VicDataGridClearData(string dgridName)
        {
            uIElementOperation.VicDataGridClearData(dgridName);
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
            uIElementOperation.UnitPageRuleLoad(unitPageName, pblockName, currentPage, pageSize);
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
            uIElementOperation.UnitWebBrowserLoad(unitWebBrowserName, content, pblockName, columnName, ctrl, executeJsFunc, type);
        }
        /// <summary>
        /// 源编辑器
        /// </summary>
        /// <param name="unitAE">源编辑器部件名字</param>
        /// <param name="text">内容</param>
        public void UnitAvalonEditLoad(string unitAE, string text)
        {
            uIElementOperation.UnitAvalonEditLoad(unitAE, text);
        }
        /// <summary>
        /// 画廊部件初始化
        /// </summary>
        /// <param name="unitName">部件名称</param>
        /// <param name="pblockName">区块名称</param>
        /// <param name="imagecolumName">图片列</param>
        /// <param name="titlecolumName">标题列</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        public void UnitGalleryRuleLoad(string unitName, string pblockName, string imagecolumName, string titlecolumName, double width = 80, double height = 100)
        {
            uIElementOperation.UnitGalleryRuleLoad(unitName, pblockName, imagecolumName, titlecolumName, width, height);
        }
        /// <summary>
        /// UnitListBoxFontIconRule部件刷新
        /// </summary>
        /// <param name="unitName">部件名称</param>
        public void UnitListBoxFontIconRuleRefresh(string unitName)
        {
            uIElementOperation.UnitListBoxFontIconRuleRefresh(unitName);
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="paramDic"></param>
        public void TemplateControlLoad(string name, object paramDic)
        {
            uIElementOperation.TemplateControlLoad(name, paramDic);
        }
        #endregion

        #region 错误处理方法
        /// <summary>
        /// 错误处理(JS专用)
        /// </summary>
        /// <param name="args"></param>
        public void SendErrMsg(string args)
        {
            SysFeiDaoLog("js businessmachine error:" + MainView.Name + args);
        }
        /// <summary>
        /// 确认初始化(JS专用)
        /// </summary>
        public void SendInit()
        {
            MainView.BuiltBrowserInit();
        }
        /// <summary>
        /// 确认加载
        /// </summary>
        public void SendLoad()
        {
            MainView.MainViewLoaded();
        }
        #endregion

        #region webviso原子操作
        /// <summary>
        /// 更新节点数据
        /// </summary>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="dataNo">数据编号</param>
        /// <param name="Data">数据</param>
        public void UpdateNodeData(string drawingName, string canvasId, string nodeId, int dataNo, object Data)
        {
            webvisioAtOperation.UpdateNodeData(drawingName, canvasId, nodeId, dataNo, Data);
        }
        /// <summary>
        /// 更新节点文本
        /// </summary>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="nodeText">数据</param>
        /// <param name="textIndex">当模具是状态图时，0是标题，1是文本，其余模具可以使用默认值0</param>
        public void UpdateNodeText(string drawingName, string canvasId, string nodeId, string nodeText, int textIndex = 0)
        {
            webvisioAtOperation.UpdateNodeText(drawingName, canvasId, nodeId, nodeText);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeIds">节点id,多个id用逗号隔开</param>
        public void DeleteNode(string drawingName, string canvasId, string nodeIds)
        {
            webvisioAtOperation.DeleteNode(drawingName, canvasId, nodeIds);
        }
        /// <summary>
        /// 连接元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="targetNodeId">目标节点id</param>
        /// <param name="lineId">连接线id</param>
        public void ConnectElements(string drawingName, string canvasId, string targetNodeId, string lineId)
        {
            webvisioAtOperation.ConnectElements(drawingName, canvasId, targetNodeId, lineId);
        }
        /// <summary>
        /// 克隆元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void CloneElement(string drawingName, string canvasId, string nodeId)
        {
            webvisioAtOperation.CloneElement(drawingName, canvasId, nodeId);
        }
        /// <summary>
        /// 选中元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void SelectElement(string drawingName, string canvasId, string nodeId)
        {
            webvisioAtOperation.SelectElement(drawingName, canvasId, nodeId);
        }
        /// <summary>
        ///  断开target线连接
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="lineId">连接线id</param>
        public void BreakTargetConnection(string drawingName, string canvasId, string lineId)
        {
            webvisioAtOperation.BreakTargetConnection(drawingName, canvasId, lineId);
        }
        /// <summary>
        /// 删除画布
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        public void DeletePage(string drawingName, string canvasId)
        {
            webvisioAtOperation.DeletePage(drawingName, canvasId);
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="content"></param>
        public void WebVisioImportData(string drawingName, object content)
        {
            webvisioAtOperation.WebVisioImportData(drawingName, content);
        }
        /// <summary>
        /// 导入模板
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="content"></param>
        public void WebVisioImportTemplateData(string drawingName, object content)
        {
            webvisioAtOperation.WebVisioImportTemplateData(drawingName, content);
        }
        /// <summary>
        /// 更新节点颜色
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="color">颜色值(#FFFFFF/red)</param>
        public void UpdateShapeColor(string drawingName, string canvasId, string nodeId, string color)
        {
            webvisioAtOperation.UpdateShapeColor(drawingName, canvasId, nodeId, color);
        }
        /// <summary>
        /// 改变元素类型
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="targetType">目标类型</param> 
        public void ChangeElement(string drawingName, string canvasId, string nodeId, string targetType)
        {
            webvisioAtOperation.ChangeElement(drawingName, canvasId, nodeId, targetType);
        }
        /// <summary>
        /// 设置图形缩放比例
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="scaleWidth">宽度缩放倍数</param>
        /// <param name="scaleHeight">高度缩放倍数</param>
        public void SetShapeScale(string drawingName, string canvasId, string nodeId, double scaleWidth, double scaleHeight)
        {
            webvisioAtOperation.SetShapeScale(drawingName, canvasId, nodeId, scaleWidth, scaleHeight);
        }
        /// <summary>
        /// 生成图形
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="positionX">X轴坐标</param>
        /// <param name="positionY">Y轴坐标</param>
        public void CreateShapeElement(string drawingName, string canvasId, string businessType, double positionX, double positionY)
        {
            webvisioAtOperation.CreateShapeElement(drawingName, canvasId, businessType, positionX, positionY);
        }
        /// <summary>
        /// 生成连接线
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="sourceId">源端图形id</param>
        /// <param name="targetId">目标图形id</param>
        public void CreateLineElement(string drawingName, string canvasId, string businessType, string sourceId, string targetId)
        {
            webvisioAtOperation.CreateLineElement(drawingName, canvasId, businessType, sourceId, targetId);
        }
        /// <summary>
        /// 获取画布中的所有元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <returns>元素集合</returns>
        public void GetAllElement(string drawingName, string canvasId)
        {
            webvisioAtOperation.GetAllElement(drawingName, canvasId);
        }
        /// <summary>
        /// 获取图形上离开线集合
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void GetOutLinesByShape(string drawingName, string canvasId, string nodeId)
        {
            webvisioAtOperation.GetOutLinesByShape(drawingName, canvasId, nodeId);
        }
        /// <summary>
        /// 获取图形进入线集合
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void GetInLinesByShape(string drawingName, string canvasId, string nodeId)
        {
            webvisioAtOperation.GetInLinesByShape(drawingName, canvasId, nodeId);
        }
        /// <summary>
        /// 获取线源端的图形
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <returns>源端图形</returns>
        public void GetSourceShapeByLine(string drawingName, string canvasId, string nodeId)
        {
            webvisioAtOperation.GetSourceShapeByLine(drawingName, canvasId, nodeId);
        }
        /// <summary>
        /// 获取线源端的图形
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <returns>源端图形</returns>
        public void GetTargetShapeByLine(string drawingName, string canvasId, string nodeId)
        {
            webvisioAtOperation.GetTargetShapeByLine(drawingName, canvasId, nodeId);
        }
        /// <summary>
        /// 获取导出图形内容
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="oav">接受oav</param>
        public void GetExportData(string drawingName, object oav)
        {
            webvisioAtOperation.GetExportData(drawingName, oav);
        }
        /// <summary>
        /// 消息转OAV
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="keyName">消息体键值</param>
        public void MessageToOAV(string drawingName, string keyName)
        {
            webvisioAtOperation.MessageToOAV(drawingName, keyName);
        }
        #endregion

        #region 专用原子操作
        /// <summary>
        /// 规则机台模板then专用原子操作
        /// </summary>
        /// <param name="pblockrhs">规则右实例block</param>
        /// <param name="pblockparams">规则右实例参数block</param>
        /// <param name="action_no">原子操作实例编号</param>
        /// <param name="rule_no">规则编号</param>
        /// <param name="oav">接受oav</param>
        public void AddThenTemplate(string pblockrhs, string pblockparams, string action_no, string rule_no, object oav)
        {
            dataOperation.AddThenTemplate(pblockrhs, pblockparams, action_no, rule_no, oav);
        }
        /// <summary>
        /// 是否允许组合或拆分
        /// </summary>
        /// <param name="pBlockName">区块名称page_dom_struct对应的P块</param>
        /// <param name="page_no">页面编号</param>
        /// <param name="drList">选中的行集合</param>
        /// <param name="type">组合或拆分0组合1拆分</param>
        /// <param name="oav">接受oav(true,false)</param>
        public void IsAllowComOrSplit(string pBlockName, string page_no, List<DataRow> drList, int type, object oav)
        {
            dataOperation.IsAllowComOrSplit(pBlockName, page_no, drList, type, oav);
        }
        #endregion

        #region 动态构建的Listbox部件初始化或者刷新方法
        /// <summary>
        /// 动态构建的Listbox部件初始化或者刷新方法
        /// </summary>
        /// <param name="unitName">部件名称</param>
        /// <param name="pblockName">p块名称</param>
        public void UnitListBoxDynamicRuleRefresh(string unitName, string pblockName)
        {
            uIElementOperation.UnitListBoxDynamicRuleRefresh(unitName, pblockName);
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
            uIElementOperation.UnitListBoxDynamicRuleGetChosedRows(unitName, oav);
        }
        #endregion

        #region webVisio部件专用原子操作
        /// <summary>
        /// webVisio部件渲染数据
        /// </summary>
        /// <param name="drawingName">部件名称</param>
        /// <param name="message">消息内容</param>
        public void WebVisionDoRender(string drawingName, string message)
        {
            webvisioAtOperation.WebVisionDoRender(drawingName, message);
        }
        /// <summary>
        /// 获取webVisio中Tag中的值
        /// </summary>
        /// <param name="drawingName">部件名称</param>
        /// <param name="key">关键字</param>
        /// <param name="oav">返回oav</param>
        public void WebVisionGetParams(string drawingName, string key, object oav)
        {
            webvisioAtOperation.WebVisionGetParams(drawingName, key, oav);
        }
        /// <summary>
        /// 设置元素是否可见(占空间)
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="visibility">是否显示</param>
        public void SetElementVisilityOrHidden(string elementName, bool visibility)
        {
            uIElementOperation.SetElementVisilityOrHidden(elementName, visibility);
        }
        /// <summary>
        /// 通过字段值设置block当前选中行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="key">关键字字段名，值在表中唯一</param>
        /// <param name="param">字段值</param>
        public void SetPBlockCurrentRowByKey(string pBlockName, string key, string param)
        {
            dataOperation.SetPBlockCurrentRowByKey(pBlockName, key, param);
        }
        /// <summary>
        /// 判断在pblock表中某列中的某个值是否存在
        /// </summary>
        /// <param name="pBlockName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oav"></param>
        public void SetPBlockDtIsHavaExist(string pBlockName, string key, string value, object oav)
        {
            dataOperation.SetPBlockDtIsHavaExist(pBlockName, key, value, oav);
        }
        /// <summary>
        /// 抽取制品数据【pvd，状态图】
        /// </summary>
        /// <param name="pageNo">页面编号</param>
        /// <param name="compntGroupNo">组件组合编号</param>
        /// <param name="diagramNo">图号</param>
        /// <param name="type">获取数据类型</param>
        /// <param name="oav">返回消息oav</param>
        public void ExtractProductData(object pageNo, object compntGroupNo, object diagramNo, string type, object oav)
        {
            systemOperation.ExtractProductData(pageNo, compntGroupNo, diagramNo, type, oav);
        }
        /// <summary>
        /// 生成绘图数据
        /// </summary>
        /// <param name="diagramNo">图号</param>
        /// <param name="oav">返回消息oav</param>
        public void ExtractDrawingData(object diagramNo, object oav)
        {
            systemOperation.ExtractDrawingData(diagramNo, oav);
        }
        #endregion

        #region 原型图形部件
        /// <summary>
        /// 原型图形部件初始化
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="width">宽度</param>
        public void UnitUCDesignerRuleLoad(string unitName, object pageType, object width)
        {
            uIElementOperation.UnitUCDesignerRuleLoad(unitName, pageType, width);
        }
        /// <summary>
        /// 原型图形部件加载文件
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="filePath">文件服务地址</param>
        /// <param name="oav">是否加载成功</param>
        public void UnitUCDesignerRuleLoadFile(string unitName, object filePath, object oav)
        {
            uIElementOperation.UnitUCDesignerRuleLoadFile(unitName, filePath, oav);
        }
        /// <summary>
        /// 原型图形部件保存上传文件
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="filePath">文件服务地址</param>
        /// <param name="oav">是否上传成功</param>
        public void UnitUCDesignerRuleSaveFile(string unitName, object filePath, object oav)
        {
            uIElementOperation.UnitUCDesignerRuleSaveFile(unitName, filePath, oav);
        }

        /// <summary>
        /// 原型图形部件选择选中P块状态改变
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="pDomName">Dom树P块名称</param>
        /// <param name="pPName">P表P块名称</param>
        /// <param name="formatId">版式id</param>
        public void UnitUCDesignerRuleSelectPState(string unitName, string pDomName, string pPName, object formatId)
        {
            uIElementOperation.UnitUCDesignerRuleSelectPState(unitName, pDomName, pPName, formatId);
        }
        /// <summary>
        /// 原型图形部件保存页面布局（返回布局文件串）
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="oav">接收oav</param>
        public void UnitUCDesignerRuleSavePageLayout(string unitName, object oav)
        {
            uIElementOperation.UnitUCDesignerRuleSavePageLayout(unitName, oav);
        }
        /// <summary>
        /// 原型图形部件导入页面布局
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="elementStr">布局文件字符串</param>
        public void UnitUCDesignerRuleDropPageLayout(string unitName, object elementStr)
        {
            uIElementOperation.UnitUCDesignerRuleDropPageLayout(unitName, elementStr);
        }
        /// <summary>
        /// 原型图形部件导入版式
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="elementStr">版式文件字符串</param>
        /// <param name="pointx">鼠标位置</param>
        /// <param name="pointy">鼠标位置</param>
        /// <param name="itemBase">版式所属组件图形</param>
        public void UnitUCDesignerRuleDropFormat(string unitName, object elementStr, object pointx, object pointy, object itemBase)
        {
            uIElementOperation.UnitUCDesignerRuleDropFormat(unitName, elementStr, pointx, pointy, itemBase);
        }
        /// <summary>
        /// 原型图形部件保存版式（返回版式文件串）
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="oav">接收oav</param>
        public void UnitUCDesignerRuleSaveFormat(string unitName, object oav)
        {
            uIElementOperation.UnitUCDesignerRuleSaveFormat(unitName, oav);
        }
        /// <summary>
        /// 原型图形部件拖拽控件
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="itemTitle">控件模具名</param>
        /// <param name="pointx">鼠标位置</param>
        /// <param name="pointy">鼠标位置</param>
        /// <param name="itemBase">版式所属组件图形</param>
        /// <param name="id">新增控件id</param>
        public void UnitUCDesignerRuleDropItem(string unitName, object itemTitle, object pointx, object pointy, object itemBase, object id)
        {
            uIElementOperation.UnitUCDesignerRuleDropItem(unitName, itemTitle, pointx, pointy, itemBase, id);
        }
        /// <summary>
        /// 原型图形控件Dom树加载后处理
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="pDomName">Dom树P块名称</param>
        /// <param name="pDomTypeName">Dom类型P块名称</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="pageNo">页面编号</param>
        /// <param name="nodeRoot">Dom树根节点</param>
        public void UnitUCDesignerRuleDomLoad(string unitName, string pDomName, string pDomTypeName, object pageType, object pageNo, object nodeRoot)
        {
            uIElementOperation.UnitUCDesignerRuleDomLoad(unitName, pDomName, pDomTypeName, pageType, pageNo, nodeRoot);
        }
        /// <summary>
        /// 原型图形部件得到页面布局父节点
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="oav">接收oav</param>
        public void UnitUCDesignerRuleGetPageLayoutSuperiors(string unitName, object pageType, object oav)
        {
            uIElementOperation.UnitUCDesignerRuleGetPageLayoutSuperiors(unitName, pageType, oav);
        }
        /// <summary>
        /// 原型图形控件选择组件图形刷新
        /// </summary>
        /// <param name="unitName">部件</param>
        /// <param name="id">组件id</param>
        public void UnitUCDesignerRuleSelectCom(string unitName, object id = null)
        {
            uIElementOperation.UnitUCDesignerRuleSelectCom(unitName, id);
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
        public void ComponentVisioPropertyLoad(string unitName, string pblockNameProperty, string pageType,
                                               string nodeNo, string nodeTypeNo, string nodeTypeName, string pblockNo = "",
                                               string pblockName = "", string formatNo = "")
        {
            uIElementOperation.ComponentVisioPropertyLoad(unitName, pblockNameProperty, pageType,
                                                nodeNo, nodeTypeNo, nodeTypeName, pblockNo,
                                                pblockName, formatNo)
            ;
        }

        #region 操作分析封装的原子操作

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
            uIElementOperation.ComponentOperationAnalysisRender(componetName, pblockName, maintable, secondtable, secendtwotable);
        }
        /// <summary>
        /// 操作分析组件添加主数据
        /// </summary>
        /// <param name="componetName">组件名称</param>
        public void ComponentOperationAnalysisAdd(string componetName)
        {
            uIElementOperation.ComponentOperationAnalysisAdd(componetName);
        }
        /// <summary>
        /// 获取组件中的值
        /// </summary>
        /// <param name="componetName">组件名称</param>
        /// <param name="key">key值</param>
        /// <param name="oav">返回参数</param>
        public void GetComponentParamDictData(string componetName, string key, object oav)
        {
            uIElementOperation.GetComponentParamDictData(componetName, key, oav);
        }
        /// <summary>
        /// 获取combox选择值
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="oav"></param>
        public void GetComBoxSelectValue(string elementName, object oav)
        {
            uIElementOperation.GetComBoxSelectValue(elementName, oav);
        }
        #endregion

        /// <summary>
        /// 判断当前模型是否有修改
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oav">返回结果（true:有修改尚未保存服务器；false:无需要提交的修改）</param>
        public void GetChangedData(string pBlockName, object oav)
        {
            dataOperation.GetChangedData(pBlockName, oav);
        }
        #endregion
    }
}
