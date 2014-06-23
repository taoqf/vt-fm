using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.CoreLibrary.Common
{
    /// <summary>
    /// 格式化消息的工具类
    /// </summary>
    public class MessageHelper
    {
        private static string splitSign = ":@:";		//消息分割符

        public static string SplitSign
        {
            get { return MessageHelper.splitSign; }
            set { MessageHelper.splitSign = value; }
        }
        /// <summary>
        /// ICE接收的消息字符串转为消息类成员
        /// </summary>
        /// <param name="vMsgJson">接收的消息字符串s</param>
        /// <returns>消息类成员</returns>
        public static RequestMessage ReceiveMsgToClass(string vMsgJson)
        {
            RequestMessage message = new RequestMessage();
            try
            {
                long p = 0;
                string msgHead;
                //拆分消息头和消息体
                p = vMsgJson.IndexOf(splitSign);
                if (p > 0)
                {
                    // 截取消息头
                    msgHead = vMsgJson.Substring(0, (int)p);
                    // 截取消息体
                    message.MessageContent = vMsgJson.Substring((int)p + splitSign.Length, vMsgJson.Length - (int)p - splitSign.Length);
                    /*******************************************************
                    * 解析消息头JSON字符串，从中获取类属性并复制给属性对象 *
                    *******************************************************/
                    message.MessageId = JsonHelper.ReadJsonString(msgHead, "messageID");
                    message.RouterAddress = JsonHelper.ReadJsonString(msgHead, "routerAddress");
                    message.TargetAddress = JsonHelper.ReadJsonString(msgHead, "receiptAddress");
                    message.CurrentRecepitId = JsonHelper.ReadJsonString(msgHead, "receiptSessionID");
                    message.CurrentSenderId = JsonHelper.ReadJsonString(msgHead, "senderSessionID");
                    message.FromId = JsonHelper.ReadJsonString(msgHead, "fromID");
                    message.FromRole = JsonHelper.ReadJsonString(msgHead, "fromRole");
                    message.ToId = JsonHelper.ReadJsonString(msgHead, "toID");
                    message.ToRole = JsonHelper.ReadJsonString(msgHead, "toRole");
                    message.ReplyToId = JsonHelper.ReadJsonString(msgHead, "replyToID");
                    message.MessageType = JsonHelper.ReadJsonString(msgHead, "messageType");
                    message.MessageControl = JsonHelper.ReadJsonString(msgHead, "messageControl");
                    message.SessionId = JsonHelper.ReadJsonString(msgHead, "sessionId");
                }
            }
            catch (Exception)
            { }
            return message;
        }
        /// <summary>
        /// 调用DLL的外部程序传入的JSON字符串转为消息类成员.
        /// </summary>
        /// <param name="MsgJson">外部程序传入的JSON字符串</param>
        /// <returns>消息类成员</returns>
        public static RequestMessage SendMsgToClass(string msgJson)
        {
            RequestMessage message = null;
            try
            {
                message = JsonHelper.ToObject<RequestMessage>(msgJson);
            }
            catch (Exception)
            { }
            return message;
        }
        /// <summary>
        /// 消息结构体转为符合ICE传递需求的消息JSON字符串.
        /// </summary>
        /// <returns>ICE传递格式的JSON字符串</returns>
        public static string ToIceString(RequestMessage message)
        {
            // 最终输出的JSON字符串，供ICE通讯器使用
            string rtMsg;
            // 保存消息头信息的JSON字符串
            string msgHead;
            // 保存消息头信息的JSON对象
            Dictionary<string, string> msg = new Dictionary<string, string>();
            msg.Add("messageID", message.MessageId);
            msg.Add("routerAddress", message.RouterAddress);
            msg.Add("receiptAddress", message.TargetAddress);
            msg.Add("receiptSessionID", message.CurrentRecepitId);
            msg.Add("senderSessionID", message.CurrentSenderId);
            msg.Add("fromID", message.FromId);
            msg.Add("fromRole", message.FromRole);
            msg.Add("toID", message.ToId);
            msg.Add("toRole", message.ToRole);
            msg.Add("replyToID", message.ReplyToId);
            msg.Add("messageType", message.MessageType);
            msg.Add("messageControl", message.MessageControl);
            msg.Add("sessionId", message.SessionId);
            msgHead = JsonHelper.ToJson(msg);
            rtMsg = msgHead + MessageHelper.SplitSign + message.MessageContent;
            return rtMsg;
        }

        /// <summary>
        /// 消息结构体转为常规JSON字符串供DLL外部程序使用.
        /// </summary>
        /// <returns>常规JSON字符串</returns>
        public static string ToJson(RequestMessage message)
        {
            // 最终输出的JSON字符串，供调用DLL的外部程序使用
            string rtMsg;
            Dictionary<string, string> msg = new Dictionary<string, string>();
            msg.Add("messageID", message.MessageId);
            msg.Add("routerAddress", message.RouterAddress);
            msg.Add("receiptAddress", message.TargetAddress);
            msg.Add("receiptSessionID", message.CurrentRecepitId);
            msg.Add("senderSessionID", message.CurrentSenderId);
            msg.Add("fromID", message.FromId);
            msg.Add("fromRole", message.FromRole);
            msg.Add("toID", message.ToId);
            msg.Add("toRole", message.ToRole);
            msg.Add("replyToID", message.ReplyToId);
            msg.Add("messageType", message.MessageType);
            msg.Add("messageControl", message.MessageControl);
            msg.Add("sessionId", message.SessionId);
            msg.Add("messageContent", message.MessageContent);
            rtMsg = JsonHelper.ToJson(msg);
            return rtMsg;
        }

        /// <summary>
        /// 从属性messageControl中记录的JSON字符串中读取节点asynchronize的值，
        /// 判断指定接收逻辑做异步处理.
        /// </summary>
        /// <returns>true=异步，false=同步</returns>
        public static bool GetMsgASYNC(RequestMessage message)
        {

            // 初始化JSON解析工具
            try
            {
                return Convert.ToBoolean(JsonHelper.ReadJsonString(message.MessageControl, "asynchronize"));
            }
            catch (System.Exception)
            {
                return false;
            }

        }
    }
}
