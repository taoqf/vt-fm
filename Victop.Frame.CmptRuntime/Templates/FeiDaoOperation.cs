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

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 飞道原子操作映射类
    /// </summary>
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
        /// <param name="oav">事实</param>
        public void SetConditionSearchIn(string pBlockName, string paramField, OAVModel oav)
        {
            dataOperation.SetConditionSearchIn(pBlockName, paramField, oav);
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
        #endregion

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
        /// 设置选中数据通过dataGrid控件
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="dgrid">dataGird控件</param>
        public void SetPBlockCurrentRowByDataGrid(string pBlockName, FrameworkElement dgrid)
        {
            dataOperation.SetPBlockCurrentRowByDataGrid(pBlockName, dgrid);
        }
        /// <summary>
        /// 设置选中数据通过ListBox控件
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="lbox">ListBox控件</param>
        public void SetPBlockCurrentRowByListBox(string pBlockName, FrameworkElement lbox)
        {
            dataOperation.SetPBlockCurrentRowByListBox(pBlockName, lbox);
        }
        /// <summary>
        /// 获取ListBox选中项内容
        /// </summary>
        /// <param name="lboxName">ListBox控件</param>
        /// <param name="oav">接受oav</param>
        public void GetListBoxSelectItem(string lboxName, OAVModel oav)
        {
            uIElementOperation.GetListBoxSelectItem(lboxName, oav);
        }
        /// <summary>
        /// 提交BlockData的数据
        /// </summary>
        /// <param name="pBlockName">区块名称</param>
        /// <param name="isSaveServer">是否提交服务器</param>
        public void SavePBlockData(string pBlockName, bool isSaveServer = true)
        {
            dataOperation.SavePBlockData(pBlockName, isSaveServer);
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
        public void PBlockAddRow(string pBlockName, OAVModel oav=null)
        {
            dataOperation.PBlockAddRow(pBlockName, oav);
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="blockName">区块名称</param>
        /// <param name="position">插入位置</param>
        /// <param name="oav">接收oav</param>
        public void blockInsertRow(string blockName, int position, OAVModel oav=null)
        {
            dataOperation.BlockInsertRow(blockName, position,oav);
        }
        /// <summary>
        /// 获取选中行的列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowGet(string pblockName, string paramName, OAVModel oav)
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
        public void ParamsCurrentRowUpGet(string pblockName, string paramName, OAVModel oav)
        {
            dataOperation.ParamsCurrentRowUpGet(pblockName, paramName, oav);
        }
        /// <summary>
        /// 获取选中行的下一行列值
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowDownGet(string pblockName, string paramName, OAVModel oav)
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
        /// <param name="oav">接受oav</param>
        public void GetMaxNumber(string pblockName, string fieldName, OAVModel oav)
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
        /// <param name="oav">接收oav</param>
        public void ParamsCurrentRowSet(string pblockName, string paramName, OAVModel oav)
        {
           dataOperation.ParamsCurrentRowSet(pblockName, paramName, oav);
        }
        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="pblockName">区块名称</param>
        public void VicDataGridSelectRowDelete( string pblockName)
        {
            dataOperation.VicDataGridSelectRowDelete( pblockName);
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
            dataOperation.SetPBlockData(pblockNameOne, pblockNameTwo, pblockNameThree, fildone, fildtwo, fildthree, fildfour);
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
        /// <param name="oav">接受oav</param>
        public void GetMaxNumberFromOneLetter(string pblockName, string fieldName, string firstLetter, OAVModel oav)
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
        /// <param name="oav"></param>
        public void InsertFact(OAVModel oav)
        {
            systemOperation.InsertFact(oav);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav"></param>
        public void RemoveFact(OAVModel oav)
        {
            systemOperation.RemoveFact(oav);
        }
        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav"></param>
        public void UpdateFact(OAVModel oav)
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
            systemOperation.ExcuteComponentTrigger(compntName,compntTrigger, paramInfo);
        }
        /// <summary>
        /// 获取页面参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsPageGet(string paramName, OAVModel oav)
        {
            systemOperation.ParamsPageGet(paramName, oav);
        }
        /// <summary>
        /// 获取组件参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsCompntGet(string paramName, OAVModel oav)
        {
            systemOperation.ParamsCompntGet(paramName, oav);
        }
        /// <summary>
        /// 获取Dictionary中参数值
        /// </summary>
        /// <param name="oavDic">存储dic类型的oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsGetByDictionary(OAVModel oavDic, string paramName, OAVModel oav)
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
        public void ParamsInterCompntAdd(OAVModel oavCom, OAVModel oavPage)
        {
            systemOperation.ParamsInterCompntAdd(oavCom, oavPage);
        }
        /// <summary>
        /// 组件取参数
        /// </summary>
        /// <param name="se">状态信息</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsInterCompntParse(StateTransitionModel se, string paramName, OAVModel oav)
        {
           systemOperation.ParamsInterCompntParse( se, paramName, oav);
        }
        /// <summary>
        /// 弹框展示组件操作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void UCCompntShowDialog(string compntName, int height = 600, int width = 600)
        {
            systemOperation.UCCompntShowDialog( compntName, height = 600, width = 600);
        }
        /// <summary>
        /// 弹框展示组件操作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void UCCompntShow(string compntName, int height = 600, int width = 600)
        {
            systemOperation.UCCompntShow(compntName, height = 600, width = 600);
        }
        /// <summary>
        /// 设置组件参数
        /// </summary>
        /// <param name="compntName">组件名称</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        public void SetCompntParamDic(string compntName, string paramName, object paramValue)
        {
            systemOperation.SetCompntParamDic(compntName,paramName,paramValue);
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
        public void ShowMessageResult(object messageInfo, object caption, OAVModel oav)
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
        public void SetParamValue(object paramValue, OAVModel oav)
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
          systemOperation.SetCompontVisility( paramValue, visibility);
        }
        /// <summary>
        /// 设置警戒条件
        /// </summary>
        /// <param name="se">状体转移实体</param>
        /// <param name="oav">oav警戒值</param>
        /// <param name="oavmsg">oav消息内容</param>
        public void SetActionGuard(StateTransitionModel se, OAVModel oav, OAVModel oavmsg)
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
        /// 将文件写入指定文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="content">规则内容</param>
        public void WriteTextToFile(string fileName, string content)
        {
            systemOperation.WriteTextToFile(fileName, content);
        }
        /// <summary>
        /// 发送获取编码消息
        /// </summary>
        /// <param name="SystemId">系统ID</param>
        /// <param name="iPName">规则名称(例如:"BH005")</param>
        /// <param name="iCodeRule">编码规则</param>
        /// <returns>单号</returns>
        public  string SendGetCodeMessage(string SystemId, string iPName, string iCodeRule)
        {
            return systemOperation.SendGetCodeMessage(SystemId, iPName, iCodeRule);
        }

        #region 类型转换
        /// <summary>
        /// 转换字符串类型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public string ConvertToString(object paramValue)
        {
            return systemOperation.ConvertToString(paramValue);
        }
        /// <summary>
        /// 转换整型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public int ConvertToInt(object paramValue)
        {
            return systemOperation.ConvertToInt(paramValue);
        }
        /// <summary>
        /// 转换长整形
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public long ConvertToLong(object paramValue)
        {
            return systemOperation.ConvertToLong(paramValue);
        }
        /// <summary>
        /// 转换浮点型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public decimal ConvertToDecimal(object paramValue)
        {
            return systemOperation.ConvertToDecimal(paramValue);
        }
        /// <summary>
        /// 转换bool型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public bool ConvertToBool(object paramValue)
        {
            return systemOperation.ConvertToBool(paramValue);
        }
        #endregion

        #endregion

        #region UI操作
        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="btnName">按钮名称</param>
        /// <param name="btnContent">按钮内容</param>
        public void SetButtonText(string btnName, string btnContent)
        {
            uIElementOperation.SetButtonText(btnName, btnContent);
        }
        /// <summary>
        /// 设置文本标签内容
        /// </summary>
        /// <param name="lblName">标签名称</param>
        /// <param name="lblContent">标签内容</param>
        public void SetLabelText(string lblName, object lblContent)
        {
            uIElementOperation.SetLabelText(lblName, lblContent);
        }
        /// <summary>
        /// 获取文本标签内容
        /// </summary>
        /// <param name="lblName">标签名称</param>
        /// <param name="oav">接受oav</param>
        public void GetLabelText(string lblName, OAVModel oav)
        {
            uIElementOperation.GetLabelText(lblName, oav);
        }
        /// <summary>
        /// 获取界面元素名称
        /// </summary>
        /// <param name="element">界面元素</param>
        /// <param name="oav">接受oav</param>
        public void GetElementName(FrameworkElement element, OAVModel oav)
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
        #region VicTreeView原子操作
        /// <summary>
        /// 得到tree选中的值
        /// </summary>
        /// <param name="treeName">控件名</param>
        /// <param name="paramName">字段名</param>
        /// <param name="oav">接收oav</param>
        public void VicTreeViewGetParam(string treeName, string paramName, OAVModel oav)
        {
            uIElementOperation.VicTreeViewGetParam(treeName, paramName, oav);
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
            uIElementOperation.VicListViewGetParam(listName, paramName, oav);
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
            uIElementOperation.VicTextBoxNormalGetParamByVicText(txtName, oav);
        }
        /// <summary>
        /// VicTextBoxNormal赋值VicText
        /// </summary>
        /// <param name="txtName">控件名</param>
        /// <param name="oav">接收oav</param>
        public void VicTextBoxNormalSetVicText(string txtName, OAVModel oav)
        {
            uIElementOperation.VicTextBoxNormalSetVicText(txtName, oav);
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
            uIElementOperation.VicTextBoxGetParamByVicText(txtName, oav);
        }
        /// <summary>
        /// VicTextBox赋值VicText
        /// </summary>
        /// <param name="txtName">控件名</param>
        /// <param name="oav">接收oav</param>
        public void VicTextBoxSetVicText(string txtName, OAVModel oav)
        {
            uIElementOperation.VicTextBoxSetVicText(txtName, oav);
        }
        #endregion

        #region VicDataGrid原子操作
        /// <summary>
        /// 是否有选中项
        /// </summary>
        /// <param name="dgridName">控件名</param>
        /// <param name="oav">接收oav</param>
        /// <param name="oavmsg">消息oav</param>
        public void VicDataGridIsSelectItem(string dgridName, OAVModel oav, OAVModel oavmsg=null)
        {
            uIElementOperation.VicDataGridIsSelectItem(dgridName, oav, oavmsg);
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
        /// 设置dgrid选中行
        /// </summary>
        /// <param name="dgridName">控件名</param>
        /// <param name="selectedIndex">选中行号</param>
        public void VicDataGridSetSelectedIndex(string dgridName, int selectedIndex)
        {
            uIElementOperation.VicDataGridSetSelectedIndex(dgridName, selectedIndex);
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
            uIElementOperation.UnitPageRuleLoad(unitPageName, pblockName, currentPage = 1, pageSize = 20);
        }
        #endregion

        #endregion
     
        
   


        
    }
}
