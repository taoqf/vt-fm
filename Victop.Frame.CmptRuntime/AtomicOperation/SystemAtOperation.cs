﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            MainView.FeiDaoMachine.SetFocus(groupName);
        }
        /// <summary>
        /// 转移触发事件
        /// </summary>
        /// <param name="triggerName">触发事件名</param>
        /// <param name="triggerSource">触发源</param>
        public void TranslationState(string triggerName, object triggerSource)
        {
            MainView.FeiDaoMachine.Do(triggerName, triggerSource);
        }
        /// <summary>
        /// 插入事实
        /// </summary>
        /// <param name="o">o</param>
        /// <param name="a">a</param>
        /// <param name="v">v</param>
        public object InsertFact(string o, string a, object v = null)
        {
            return MainView.FeiDaoMachine.InsertFact(o, a, v);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav"></param>
        public void RemoveFact(object oav)
        {
            MainView.FeiDaoMachine.RemoveFact(oav);
        }
        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        public void UpdateFact(object oav)
        {
            if (oav != null)
                MainView.FeiDaoMachine.UpdateFact(oav);
        }

        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav">oav事实实例</param>
        /// <param name="v">v值</param>
        public void UpdateFact(object oav, object v)
        {
            MainView.FeiDaoMachine.UpdateFact(oav,v);
        }

        /// <summary>
        /// 提交事实
        /// </summary>
        /// <param name="oav">oav事实实例</param>
        public void CommitFact(object oav)
        {
            MainView.FeiDaoMachine.CommitFact(oav);
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
        public void SetActionGuard(object se, object oav, object oavmsg)
        {
            dynamic o1 = oav;
            dynamic o2 = oavmsg;
            dynamic o3 = se;
            if (o1.v != null && (bool)o1.v)
                o3.v = false;
            else
            {
                string msg = o2.v != null ? o2.v.ToString() : "条件不满足！";
                VicMessageBoxNormal.Show(msg);
                o3.v = true;
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
                MainView.ParentControl.FeiDaoMachine.Do(pageTrigger, paramInfo);
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
            TemplateControl tc = MainView.GetCompntInstance(compntName);
            if (tc != null)
            {
                tc.FeiDaoMachine.Do(compntTrigger, paramInfo);
            }
        }
        /// <summary>
        /// 获取页面参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsPageGet(string paramName, object oav)
        {
            if (MainView.ParentControl.ParamDict.ContainsKey(paramName))
            {
                dynamic o = oav;
                o.v = MainView.ParentControl.ParamDict[paramName];
            }
        }
        /// <summary>
        /// 获取组件参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsCompntGet(string paramName, object oav)
        {
            if (MainView.ParamDict.ContainsKey(paramName))
            {
                dynamic o = oav;
                o.v = MainView.ParamDict[paramName];
            }
        }
        /// <summary>
        /// 获取List集合
        /// </summary>
        /// <returns>List集合</returns>
        public void GetList(object oav)
        {
            dynamic o = oav;
            o.v = new List<object>();
        }
        /// <summary>
        /// 将对象加入List集合结尾处
        /// </summary>
        /// <param name="value">集合中的项</param>
        /// <param name="paramList">List集合</param>
        public void ListAdd(object value, object paramList)
        {
            List<object> list = (List<object>)paramList;
            if (list != null)
            {
                list.Add(value);
            }
        }
        /// <summary>
        /// 获取ComboBox的DT
        /// </summary>
        /// <param name="oav">接收oav</param>
        /// <returns>DT</returns>
        public void GetComboBoxDt( object oav)
        {
            DataTable dt=new DataTable();
            dt.Columns.Add(new DataColumn("key", typeof(string)));
            dt.Columns.Add(new DataColumn("value", typeof(string)));
            dynamic o = oav;
            o.v = dt;
          
        }
        
        /// <summary>
        ///给ComboBox的Dt赋值
        /// </summary>
        /// <param name="keyValue">selectValue值</param>
        /// <param name="displayValue">displaymember值</param>
        /// <param name="paramDt">oav</param>
        public void SetComboBoxDtRow(string keyValue, string displayValue, object paramDt)
        {
           
            DataTable dt = (DataTable)paramDt;
            if (dt != null)
            {
                dt.Rows.Add(keyValue, displayValue);
            }
         
        }
        /// <summary>
        /// 将DT表绑到ComboBox数据源
        /// </summary>
        /// <param name="elementName">元素名称</param>
        /// <param name="paramDt">oav</param>
        public void SetComboItemsSource(string elementName, object paramDt)
        {
            DataTable dt = (DataTable)paramDt;
            if (!string.IsNullOrEmpty(elementName))
            {
                VicComboBoxNormal combo = MainView.FindName(elementName) as VicComboBoxNormal;
                if (combo != null && paramDt!=null)
                {
                    combo.ItemsSource = dt.DefaultView;
                    combo.SelectedValuePath = "key";
                    combo.DisplayMemberPath = "value";
                }
            }

        }
        /// <summary>
        /// 获取Dictionary中参数值
        /// </summary>
        /// <param name="oavDic">存储dic类型的oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsGetByDictionary(object oavDic, string paramName, object oav)
        {
            dynamic o1 = oavDic;
            dynamic o2 = oav;
            if (o1.v != null)
            {
                Dictionary<string, object> dic = JsonHelper.ToObject<Dictionary<string, object>>(o1.v.ToString());
                if (dic != null && dic.ContainsKey(paramName))
                {
                    o2.v = dic[paramName];
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
        public void ParamsInterCompntAdd(object oavCom, object oavPage)
        {
            dynamic o1 = oavCom;
            dynamic o2 = oavPage;
            FrameworkElement fElement = o2.v as FrameworkElement;
            if (fElement == null)
            {
                fElement = new FrameworkElement();
            }
            Dictionary<string, object> dicParams = fElement.Tag as Dictionary<string, object>;
            if (dicParams == null)
            {
                dicParams = new Dictionary<string, object>();
            }
            if (!dicParams.ContainsKey(o1.a))
            {
                dicParams.Add(o1.a, o1.v);
            }
            else
            {
                dicParams[o1.a] = o1.v;
            }
            fElement.Tag = dicParams;
            o2.v = fElement;
        }
        /// <summary>
        /// 组件取参数
        /// </summary>
        /// <param name="oavParams">参数oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsInterCompntParse(object oavParams, string paramName, object oav)
        {
            dynamic o1 = oavParams;
            dynamic o2 = oav;
            FrameworkElement fElement = o1.v as FrameworkElement;
            if (fElement != null)
            {
                Dictionary<string, object> dicParams = fElement.Tag as Dictionary<string, object>;
                if (dicParams != null && dicParams.ContainsKey(paramName))
                {
                    o2.v = dicParams[paramName];
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
            win.Title = ucCom.Tag == null ? "" : ucCom.Tag.ToString();
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
            win.Title = ucCom.Tag == null ? "" : ucCom.Tag.ToString();
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
        public void ShowMessageResult(object messageInfo, object caption, object oav)
        {
            dynamic o = oav;
            MessageBoxResult msgboxresult = VicMessageBoxNormal.Show(messageInfo == null ? "空值" : messageInfo.ToString(), caption == null ? "空值" : caption.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (MessageBoxResult.Yes == msgboxresult)
            {
                o.v = "1";
            }
            else if (MessageBoxResult.No == msgboxresult)
            {
                o.v = "0";
            }
            else
            {
                o.v = "";
            }
        }
        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <param name="oav">接收oav</param>
        public void SetParamValue(object paramValue, object oav)
        {
            dynamic o = oav;
            o.v = paramValue;
        }
        /// <summary>
        /// 设置页面显示元素（已经停用，建议使用SetElementVisility原子操作）
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
            if (content == null)
            {
                return;
            }
            List<Dictionary<string, object>> dicList = JsonHelper.ToObject<List<Dictionary<string, object>>>(content.ToString());
            if (dicList != null)
            {
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
        }
        /// <summary>
        /// 保存规则文件
        /// </summary>
        /// <param name="content">规则内容</param>
        /// <param name="oav">接受oav</param>
        public void SaveWriteTextToFile(object content, object oav)
        {
            dynamic o = oav;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\data\\Rule.drl";
            FileStream fs = new FileStream(filepath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            if (content == null)
            {
                o.v = filepath;
            }
            List<Dictionary<string, object>> dicList = JsonHelper.ToObject<List<Dictionary<string, object>>>(content.ToString());
            if (dicList == null)
            {
                o.v = filepath;
            }
            else if (dicList.Count > 0 && dicList[0].ContainsKey("rules_string"))
            {
                sw.Write(dicList[0]["rules_string"]);
            }
            sw.Flush();
            sw.Close();
            fs.Close();
            o.v = filepath;
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
            dynamic o = oav;
            try
            {
                Dictionary<string, object> messageContent = new Dictionary<string, object>();
                Dictionary<string, string> address = new Dictionary<string, string>();
                address.Add("UploadFromPath", localFilePath);
                address.Add("DelFileId", Convert.ToString(filePath));
                address.Add("ProductId", productId);
                messageContent.Add("ServiceParams", JsonHelper.ToJson(address));
                Dictionary<string, object> returnDic = new DataMessageOperation().SendSyncMessage("ServerCenterService.UploadDocument", messageContent);
                if (returnDic != null && returnDic["ReplyMode"].ToString() != "0")
                {
                    Dictionary<string, object> replyContent = JsonHelper.ToObject<Dictionary<string, object>>(returnDic["ReplyContent"].ToString());
                    filePath = replyContent["fileId"].ToString();
                    o.v = filePath;
                }
                else
                {
                    o.v = "";
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("上传文件异常(string UpLoadFile)：{0}", ex.Message);
                o.v = "";
            }
        }
        /// <summary>
        /// 获取一个新的guid
        /// </summary>
        /// <param name="oav">接受oav</param>
        public void GetNewGuid(object oav)
        {
            dynamic o = oav;
            o.v = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 返回指定名称资源的值
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="oav">接受oav</param>
        public void GetStringByResourceName(string name, object oav)
        {
            dynamic o = oav;
            if (!string.IsNullOrEmpty(name))
            {
                Type type = MainView.GetType();
                Assembly assembly = Assembly.GetAssembly(type);
                type = assembly.GetTypes().FirstOrDefault(it => it.Name.Equals("Resources"));
                if (type != null)
                {
                    ResourceManager rm = new ResourceManager(type.FullName, assembly);
                    o.v = rm.GetString(name);
                }
            }
            o.v = string.Empty;
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
            dynamic o = oav;
            if (strValue == null || value == null)
                o.v = false;
            else
                o.v = strValue.ToString().Contains(value);
        }
        /// <summary>
        /// 从当前 System.String 对象移除数组中指定的一组字符的所有尾部匹配项
        /// </summary>
        /// <param name="strValue">字符串实例</param>
        /// <param name="value">一组字符组成的字符串</param>
        /// <param name="oav">接受oav</param>
        public void StrTrimEnd(object strValue, string value, object oav)
        {
            dynamic o = oav;
            if (strValue == null || value == null)
                o.v = "";
            else
            {
                char[] chararray = value.ToCharArray();
                o.v = strValue.ToString().TrimEnd(chararray);
            }
           
        }
        /// <summary>
        /// 从当前 System.String 对象移除数组中指定的一组字符的所有头部匹配项
        /// </summary>
        /// <param name="strValue">字符串实例</param>
        /// <param name="value">一组字符组成的字符串</param>
        /// <param name="oav">接受oav</param>
        public void StrTrimStart(object strValue, string value, object oav)
        {
            dynamic o = oav;
            if (strValue == null || value == null)
                o.v = "";
            else
            {
                char[] chararray = value.ToCharArray();
                o.v = strValue.ToString().TrimStart(chararray);
            }
           
        }
        /// <summary>
        /// 比较字符串一致性
        /// </summary>
        /// <param name="firstValue">字符串实例</param>
        /// <param name="secondValue">指定的字符串</param>
        /// <param name="oav">接受oav</param>
        public void CmpStrIsEqual(string firstValue, string secondValue, object oav)
        {
            dynamic o = oav;
            if (firstValue == null || secondValue == null)
                o.v = false;
           else o.v = firstValue.Equals(secondValue);
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="str">字符串实例</param>
        /// <param name="oav">接收oav</param>
        public void GetStrLength(object str, object oav)
        {
            dynamic o = oav;
            if (str == null)
                o.v = 0;
            else o.v = str.ToString().Length;
        }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="str">字符串实例</param>
        /// <param name="join">指定的字符串连接符</param>
        /// <param name="oav">接收oav</param>
        /// <param name="sort">排序方式true正序false倒序</param>
        public void AppendStr(object str,string join, object oav,bool sort)
        {
            dynamic o = oav;
            if (sort)
                o.v += str + join;
            else
            {
                if (o.v==null)
                o.v = join + str;
                else
                {
                    o.v = join + str + o.v;
                }
            }
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
            dynamic o = oav;
            if (v1 == null || v2 == null)
            {
                o.v = "";
                return;
            }
            double i=0;
            double j=0;
            double.TryParse(v1.ToString(), out i);
            double.TryParse(v2.ToString(), out j);
            o.v = i + j;
        }
        /// <summary>
        /// 数相减
        /// </summary>
        /// <param name="v1">数1</param>
        /// <param name="v2">数2</param>
        /// <param name="oav">接收oav</param>
        public void NumMinux(object v1, object v2, object oav)
        {
            dynamic o = oav;
            if (v1 == null || v2 == null)
            {
                o.v = "";
                return;
            }
            double i = 0;
            double j = 0;
            double.TryParse(v1.ToString(), out i);
            double.TryParse(v2.ToString(), out j);
            o.v = i - j;
        }
        /// <summary>
        /// 数相乘
        /// </summary>
        /// <param name="v1">数1</param>
        /// <param name="v2">数2</param>
        /// <param name="oav">接收oav</param>
        public void NumMultiply(object v1, object v2, object oav)
        {
            dynamic o = oav;
            if (v1 == null || v2 == null)
            {
                o.v = "";
                return;
            }
            double i = 0;
            double j = 0;
            double.TryParse(v1.ToString(), out i);
            double.TryParse(v2.ToString(), out j);
            o.v = i * j;
        }
        /// <summary>
        /// 数相除
        /// </summary>
        /// <param name="v1">数1</param>
        /// <param name="v2">数2</param>
        /// <param name="oav">接收oav</param>
        public void NumDivide(object v1, object v2, object oav)
        {
            dynamic o = oav;
            if (v1 == null || v2 == null)
            {
                o.v = "";
                return;
            }
            double i = 0;
            double j = 0;
            double.TryParse(v1.ToString(), out i);
            double.TryParse(v2.ToString(), out j);
            o.v = i / j;
        }

        #endregion

        /// <summary>
        /// 四舍五入取整
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="oav">接收oav</param>
        public void RoundNumbers(object value, object oav)
        {
            dynamic o = oav;
            if (value == null)
            {
                o.v = "";
                return;
            }
            double i = 0;
            double.TryParse(value.ToString(), out i);
            o.v = Math.Round(i, MidpointRounding.AwayFromZero);
        }

        #region 时间
        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <param name="oav">接受oav</param>
        /// <param name="day">指定要添加的天数</param>
        public void GetDateTime(object oav, int day = 0)
        {
            dynamic o = oav;
            DataMessageOperation messageOp = new DataMessageOperation();
            Dictionary<string, object> resultDic = messageOp.SendSyncMessage("MongoDataChannelService.fetchSystime", new Dictionary<string, object>());
            string result = JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "simpleDate");
            DateTime dt = Convert.ToDateTime(result);
            dt = TimeZone.CurrentTimeZone.ToLocalTime(dt);
            DateTime dtnew = dt.AddDays(day);
            result = dtnew.ToString();
            o.v = result;
        }
        #endregion

        /// <summary>
        /// 日期类型转为自定义格式字符串
        /// </summary>
        /// <param name="oav">接收oav</param>
        /// <param name="datetime">时间</param>
        /// <param name="format">转换格式，如（yyyy-MM-dd、yyyy-MM-dd HH:mm:ss、yyyy/MM/dd HH:mm等）</param>
        public void ConvertDateTimeToString(object oav, object datetime, string format = "yyyy-MM-dd HH:mm:ss") 
        {
            dynamic o = oav;
            if (datetime == null) 
            {
                o.v = "";
                return;
            }
            DateTime dt = Convert.ToDateTime(datetime);
            o.v = dt.ToString(format);
        }

        #region 获取系统变量
        /// <summary>
        /// 获取系统变量值
        /// </summary>
        /// <param name="sysVariableName">变量名称（用户编号：usercode 用户姓名：username 客户端编号：clientno 产品ID：productid 服务器时间：timestemp 命名空间:spaceid）</param>
        /// <param name="oav">接受oav</param>
        public void GetSysVariableValue(string sysVariableName, object oav)
        {
            dynamic o = oav;
            DataMessageOperation messageOp = new DataMessageOperation();
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            try
            {
                switch (sysVariableName)
                {
                    case "usercode":
                        resultDic = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                        o.v = JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "UserCode");
                        break;
                    case "username":
                        resultDic = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                        o.v = JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "UserName");
                        break;
                    case "clientno":
                        resultDic = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                        o.v = JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "ClientNo");
                        break;
                    case "productid":
                        resultDic = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                        o.v = JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "ProductId");
                        break;
                    case "spaceid":
                        resultDic = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                        o.v = JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "SpaceId");
                        break;
                    case "systime":
                        break;
                    case "timestemp":
                        resultDic = messageOp.SendSyncMessage("MongoDataChannelService.fetchSystime", new Dictionary<string, object>());
                        string result = JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "simpleDate");
                        DateTime dt = Convert.ToDateTime(result);
                        dt = TimeZone.CurrentTimeZone.ToLocalTime(dt);
                        o.v = dt.ToString();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("获取系统变量值异常（string GetSysVariableValue）：{0}", ex.Message);
            }
        }
        #endregion

        #region 发送获取编码消息
        /// <summary>
        /// 发送获取编码消息
        /// </summary>
        /// <param name="SystemId">系统ID</param>
        /// <param name="iPName">规则名称(例如:"BH005")</param>
        /// <param name="iCodeRule">编码规则</param>
        /// <param name="oav">接受oav</param>
        public void SendGetCodeMessage(string SystemId, string iPName, string iCodeRule, object oav)
        {
            dynamic o = oav;
            try
            {
                if (string.IsNullOrEmpty(iPName) || string.IsNullOrEmpty(SystemId))
                {
                    o.v = string.Empty;
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
                    o.v = JsonHelper.ReadJsonString(returnDic["ReplyContent"].ToString(), "result");
                }
                else
                {
                    o.v = string.Empty;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("发送获取编码消息异常（string SendGetCodeMessage）：{0}", ex.Message);
                o.v = string.Empty;
            }
        }
        #endregion
        #region When 返回原子操作
        /// <summary>
        /// 返回字符串类型
        /// </summary>
        /// <param name="o">需转换值</param>
        /// <returns></returns>
        public string GetStringByObject(object o)
        {
            if (o != null)
            {
                return o.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}
