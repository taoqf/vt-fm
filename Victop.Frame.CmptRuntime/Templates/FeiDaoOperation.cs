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
        /// 设置区块查询条件子查询
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="paramField">参数字段</param>
        /// <param name="listIn">子查询集合</param>
        public void SetConditionSearchIn(string pBlockName, string paramField, object listIn)
        {
            dataOperation.SetConditionSearchIn(pBlockName, paramField, listIn);
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
        /// 新增行
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="oav">接收OAV</param>
        public void PBlockAddRow(string pBlockName, object oav)
        {
            dataOperation.PBlockAddRow(pBlockName, oav);
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
        /// 区块数据转换OAV
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="fieldName">字段名称</param>
        public void BlockToOAV(string blockName, string fieldName)
        {
            dataOperation.BlockToOAV(blockName, fieldName);
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
        /// <param name="o">o</param>
        /// <param name="a">a</param>
        /// <param name="v">v</param>
        public void InsertFact(string o, string a, object v = null)
        {
            systemOperation.InsertFact(o, a, v);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav"></param>
        public void RemoveFact(object oav)
        {
            systemOperation.RemoveFact(oav);
        }
        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        /// <param name="v">v</param>
        public void UpdateFact(object oav)
        {
            systemOperation.UpdateFact(oav);
        }
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
        /// 将对象加入List集合结尾处
        /// </summary>
        /// <param name="value">集合中的项</param>
        /// <param name="paramList">List集合</param>
        public void ListAdd(object value, object paramList)
        {
            systemOperation.ListAdd(value, paramList);
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
        /// 组件取参数
        /// </summary>
        /// <param name="oavParams">参数oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsInterCompntParse(object oavParams, string paramName, object oav)
        {
            systemOperation.ParamsInterCompntParse(oavParams, paramName, oav);
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
        public void UpLoadFile(string localFilePath, string filePath, object oav, string productId = "feidao")
        {
            systemOperation.UpLoadFile(localFilePath, filePath, oav, productId);
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
        #endregion

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
            uIElementOperation.UnitPageRuleLoad(unitPageName, pblockName, currentPage = 1, pageSize = 20);
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
            uIElementOperation.UnitWebBrowserLoad(unitWebBrowserName, content, pblockName, columnName, ctrl, executeJsFunc);
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
    }
}
