using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using gnu.CORBA.Poa;
using Microsoft.Win32;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using System.Reflection;
using System.Resources;

namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// 系统原子操作
    /// </summary>
    public class SystemAtOperation
    {
        private TemplateControl MainView;
        private static Dictionary<string, object> paramCompntDic = new Dictionary<string, object>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public SystemAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
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
        /// 转移触发事件
        /// </summary>
        /// <param name="triggerName">触发事件名</param>
        /// <param name="triggerSource">触发源</param>
        public void TranslationState(string triggerName, object triggerSource)
        {
            MainView.FeiDaoFSM.Do(triggerName, triggerSource, true);
        }
        /// <summary>
        /// 插入事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        public void InsertFact(OAVModel oav)
        {
            MainView.FeiDaoFSM.InsertFact(oav);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        public void RemoveFact(OAVModel oav)
        {
            MainView.FeiDaoFSM.RemoveFact(oav);
        }
        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        public void UpdateFact(OAVModel oav)
        {
            if (oav.AtrributeValue != null)
                MainView.FeiDaoFSM.UpdateFact(oav);
        }
        /// <summary>
        /// 系统输出
        /// </summary>
        /// <param name="consoleText">输出内容</param>
        public void SysConsole(string consoleText)
        {
            Console.WriteLine(consoleText);
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
        /// 设置警戒条件
        /// </summary>
        /// <param name="se">状体转移实体</param>
        /// <param name="oav">oav警戒值</param>
        /// <param name="oavmsg">oav消息内容</param>
        public void SetActionGuard(StateTransitionModel se, OAVModel oav, OAVModel oavmsg)
        {
            if (oav.AtrributeValue != null && (bool)oav.AtrributeValue)
                se.ActionGuard = true;
            else
            {
                string msg = oavmsg.AtrributeValue != null ? oavmsg.AtrributeValue.ToString() : "条件不满足！";
                VicMessageBoxNormal.Show(msg);
                se.ActionGuard = false;
            }
        }
        /// <summary>
        /// 执行页面动作
        /// </summary>
        /// <param name="pageTrigger">动作名称</param>
        /// <param name="paramInfo">事件触发元素</param>
        public void ExcutePageTrigger(string pageTrigger, object paramInfo)
        {
            if (MainView.ParentControl != null)
            {
                MainView.ParentControl.FeiDaoFSM.Do(pageTrigger, paramInfo);
            }
        }
        /// <summary>
        /// 执行组件动作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="compntTrigger">动作名称</param>
        /// <param name="paramInfo">事件触发元素</param>
        public void ExcuteComponentTrigger(string compntName, string compntTrigger, object paramInfo)
        {
            TemplateControl tc = MainView.FindName(compntName) as TemplateControl;
            if (tc != null)
                tc.FeiDaoFSM.Do(compntTrigger, paramInfo);
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
        /// 获取组件参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsCompntGet(string paramName, OAVModel oav)
        {
            if (MainView.ParamDict.ContainsKey(paramName))
            {
                oav.AtrributeValue = MainView.ParamDict[paramName];
            }
        }
        /// <summary>
        /// 获取Dictionary中参数值
        /// </summary>
        /// <param name="oavDic">存储dic类型的oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsGetByDictionary(OAVModel oavDic, string paramName, OAVModel oav)
        {
            if (oavDic.AtrributeValue != null)
            {
                Dictionary<string, object> dic = JsonHelper.ToObject<Dictionary<string, object>>(oavDic.AtrributeValue.ToString());
                if (dic != null && dic.ContainsKey(paramName))
                {
                    oav.AtrributeValue = dic[paramName];
                }
            }
        }
        /// <summary>
        /// 新增页面参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        public void ParamsPageAdd(string paramName, object paramValue)
        {
            if (MainView.ParentControl.ParamDict.ContainsKey(paramName))
            {
                MainView.ParentControl.ParamDict[paramName] = paramValue;
            }
            else
            {
                MainView.ParentControl.ParamDict.Add(paramName, paramValue);
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
            if (paramCompntDic.ContainsKey(compntName))
            {
                ucCom.ParamDict = paramCompntDic[compntName] as Dictionary<string, object>;
                paramCompntDic.Remove(compntName);
            }
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
        /// 设置组件参数
        /// </summary>
        /// <param name="compntName">组件名称</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        public void SetCompntParamDic(string compntName, string paramName, object paramValue)
        {
            if (!string.IsNullOrEmpty(compntName) && !string.IsNullOrEmpty(paramName))
            {
                if (paramCompntDic.ContainsKey(compntName))
                {
                    Dictionary<string, object> paramDic = paramCompntDic[compntName] as Dictionary<string, object>;
                    if (paramDic.ContainsKey(paramName))
                    {
                        paramDic[paramName] = paramValue;
                    }
                    else
                    {
                        paramDic.Add(paramName, paramValue);
                    }
                }
                else
                {
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add(paramName, paramValue);
                    paramCompntDic.Add(compntName, paramDic);
                }
            }
        }
        /// <summary>
        /// 弹框展示组件操作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        public void UCCompntShow(string compntName, int height = 600, int width = 600)
        {
            TemplateControl ucCom = MainView.GetComponentInstanceByName(compntName);
            if (ucCom == null)
            {
                Console.WriteLine("原子操作：UCCompntShowDialog未找到组件" + compntName);
                LoggerHelper.Info("原子操作：UCCompntShowDialog未找到组件" + compntName);
                return;
            }
            ucCom.ParentControl = MainView;
            if (paramCompntDic.ContainsKey(compntName))
            {
                ucCom.ParamDict = paramCompntDic[compntName] as Dictionary<string, object>;
                paramCompntDic.Remove(compntName);
            }
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
        /// 使用window弹出内容
        /// </summary>
        /// <param name="content">弹出内容</param>
        public void ShowVicWindowContent(object content)
        {
            VicTextBoxNormal textBox = new VicTextBoxNormal();
            textBox.VicText = content.ToString();
            textBox.Height = 580;
            textBox.Width = 780;
            VicWindowNormal win = new VicWindowNormal();
            win.Owner = XamlTreeHelper.GetParentObject<Window>(MainView);
            win.ShowInTaskbar = false;
            win.SetResourceReference(VicWindowNormal.StyleProperty, "WindowMessageSkin");
            win.Height = 600;
            win.Width = 800;
            win.Title = "预览";
            win.Content = textBox;
            win.Show();
        }
        /// <summary>
        /// 弹框关闭操作
        /// </summary>
        public void UCCompntClose()
        {
            Window win = XamlTreeHelper.GetParentObject<Window>(MainView);
            if (win != null)
            {
                win.Close();
            }
        }
        /// <summary>
        /// 弹出提示信息
        /// </summary>
        /// <param name="messageInfo">消息内容</param>
        public void ShowMessage(object messageInfo)
        {
            VicMessageBoxNormal.Show(messageInfo == null ? "空值" : messageInfo.ToString());
        }
        /// <summary>
        /// 弹出提示询问
        /// </summary>
        /// <param name="messageInfo">提示信息</param>
        /// <param name="caption">标题</param>
        /// <param name="oav">接收oav</param>
        public void ShowMessageResult(object messageInfo, object caption, OAVModel oav)
        {
            MessageBoxResult msgboxresult = VicMessageBoxNormal.Show(messageInfo == null ? "空值" : messageInfo.ToString(), caption == null ? "空值" : caption.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (MessageBoxResult.Yes == msgboxresult)
            {
                oav.AtrributeValue = "1";
            }
            else if (MessageBoxResult.No == msgboxresult)
            {
                oav.AtrributeValue = "0";
            }
            else
            {
                oav.AtrributeValue = "";
            }
        }
        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <param name="oav">接收oav</param>
        public void SetParamValue(object paramValue, OAVModel oav)
        {
            oav.AtrributeValue = paramValue;
        }
        /// <summary>
        /// 设置页面显示元素
        /// </summary>
        /// <param name="paramValue">元素名称</param>
        /// <param name="visibility">是否显示</param>
        public void SetCompontVisility(string paramValue, bool visibility)
        {
            FrameworkElement tc = MainView.FindName(paramValue) as FrameworkElement;
            if (tc != null)
            {
                if (visibility)
                {
                    tc.Visibility = Visibility.Visible;
                }
                else
                {
                    tc.Visibility = Visibility.Collapsed;
                }
            }
        }
        /// <summary>
        /// 弹框展示当前组件中的一部分
        /// </summary>
        /// <param name="layoutName">布局名称</param>
        public void UcCurrentCompntContentShow(string layoutName)
        {
            VicPopup controlConten = MainView.FindName(layoutName) as VicPopup;
            if (controlConten == null)
            {
                Console.WriteLine("原子操作：UcCurrentCompntContentShow未找到布局" + layoutName);
                LoggerHelper.Info("原子操作：UcCurrentCompntContentShow未找到布局" + layoutName);
                return;
            }
            controlConten.IsOpen = true;
            controlConten.PlacementTarget = MainView.ParentControl;
            controlConten.Placement = PlacementMode.Center;
        }
        /// <summary>
        /// 弹框关闭操作
        /// </summary>
        /// <param name="layoutName">布局名称</param>
        public void UcCurrentCompntContentClose(string layoutName)
        {
            VicPopup controlConten = MainView.FindName(layoutName) as VicPopup;
            if (controlConten != null)
            {
                controlConten.IsOpen = false;
            }
        }
        /// <summary>
        /// 将规则文件另存
        /// </summary>
        /// <param name="content">规则内容</param>
        public void WriteTextToFile(object content)
        {
            List<Dictionary<string, object>> dicList = JsonHelper.ToObject<List<Dictionary<string, object>>>(content.ToString());
            if (dicList.Count > 0 && dicList[0].ContainsKey("rules_string"))
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "Files (*.drl)|*.drl|All Files (*.*)|*.*";
                saveFile.FileName = "Rules";
                if (saveFile.ShowDialog() == true)
                {
                    FileStream fs = new FileStream(saveFile.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(dicList[0]["rules_string"]);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
        }

        #region 获取资源
        /// <summary>
        /// 返回指定名称资源的值
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>返回资源值</returns>
        public string GetStringByResourceName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Type type = MainView.GetType();
                Assembly assembly = Assembly.GetAssembly(type);
                type = assembly.GetTypes().FirstOrDefault(it => it.Name.Equals("Resources"));
                if (type != null)
                {
                    ResourceManager rm = new ResourceManager(type.FullName, assembly);
                    return rm.GetString(name);
                }
            }
            return string.Empty;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 转换字符串类型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public string ConvertToString(object paramValue)
        {
            return paramValue == null ? "" : paramValue.ToString();
        }
        /// <summary>
        /// 转换整型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public int ConvertToInt(object paramValue)
        {
            int i = 0;
            if (paramValue != null)
                int.TryParse(paramValue.ToString(), out i);
            return i;
        }
        /// <summary>
        /// 转换长整形
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public long ConvertToLong(object paramValue)
        {
            long i = 0;
            if (paramValue != null)
                long.TryParse(paramValue.ToString(), out i);
            return i;
        }
        /// <summary>
        /// 转换浮点型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public decimal ConvertToDecimal(object paramValue)
        {
            decimal i = 0;
            if (paramValue != null)
                decimal.TryParse(paramValue.ToString(), out i);
            return i;
        }
        /// <summary>
        /// 转换bool型
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public bool ConvertToBool(object paramValue)
        {
            bool i = false;
            if (paramValue != null)
                bool.TryParse(paramValue.ToString(), out i);
            return i;
        }
        #endregion

        #region 字符串处理
        /// <summary>
        /// 指定的字符串是否出现在字符串实例中
        /// </summary>
        /// <param name="strValue">字符串实例</param>
        /// <param name="value">指定的字符串</param>
        /// <returns>是否包含</returns>
        public bool StrContains(object strValue, string value)
        {
            if (strValue == null || value == null)
                return false;
            return strValue.ToString().Contains(value);
        }
        #endregion

        #region 发送获取编码消息
        /// <summary>
        /// 发送获取编码消息
        /// </summary>
        /// <param name="SystemId">系统ID</param>
        /// <param name="iPName">规则名称(例如:"BH005")</param>
        /// <param name="iCodeRule">编码规则</param>
        /// <returns>单号</returns>
        public string SendGetCodeMessage(string SystemId, string iPName, string iCodeRule)
        {
            try
            {
                if (string.IsNullOrEmpty(iPName) || string.IsNullOrEmpty(SystemId))
                {
                    return string.Empty;
                }
                string MessageType = "MongoDataChannelService.findDocCode";
                DataMessageOperation messageOp = new DataMessageOperation();
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("systemid", SystemId);
                contentDic.Add("pname", iPName);
                contentDic.Add("setinfo", iCodeRule);
                Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                if (returnDic != null || returnDic.ContainsKey("ReplyContent"))
                {
                    return JsonHelper.ReadJsonString(returnDic["ReplyContent"].ToString(), "result");
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("发送获取编码消息异常（string SendGetCodeMessage）：{0}", ex.Message);
                return string.Empty;
            }
        }
        #endregion
    }
}
