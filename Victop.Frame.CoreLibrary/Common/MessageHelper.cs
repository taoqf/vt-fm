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
                    message.ToId = JsonHelper.ReadJsonString(msgHead, "toID");
                    message.ReplyToId = JsonHelper.ReadJsonString(msgHead, "replyToID");
                    message.MessageType = JsonHelper.ReadJsonString(msgHead, "messageType");
                    message.MessageControl = JsonHelper.ReadJsonString(msgHead, "messageControl");
                    message.SessionId = JsonHelper.ReadJsonString(msgHead, "sessionId");
                    message.SpaceId = JsonHelper.ReadJsonString(msgHead, "spaceId");
                    message.ClientCallBackProxy = JsonHelper.ReadJsonString(msgHead, "clientCallBackProxy");
                }
            }
            catch (Exception)
            { }
            return message;
        }
       
    }
}
