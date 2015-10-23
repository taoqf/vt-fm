using slice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.CoreLibrary.Models;



namespace Victop.Frame.ComLink.ICE.Util
{
    public class MessageUtil
    {
        public static RequestMessage ICEmsg2SOAmsg(Message msg)
        {
            RequestMessage message = new RequestMessage();
            message.FromId = msg.fromID;
            message.MessageControl = msg.messageControl;
            message.MessageId = msg.replyToID;
            message.MessageType = msg.messageType;
            message.TargetAddress = msg.receiptAddress;
            message.CurrentRecepitId = msg.receiptSessionID;
            message.ReplyToId = string.IsNullOrEmpty(msg.replyToID) ? msg.messageID : msg.replyToID;
            message.RouterAddress = msg.routerAddress;
            message.CurrentSenderId = msg.senderSessionID;
            message.SessionId = msg.sessionId;
            message.ToId = msg.toID;
            message.MessageContent = msg.messageContent;
            message.SpaceId = msg.spaceId;
            message.ClientCallBackProxy = msg.clientCallBackProxy;
            return message;
        }
        public static Message SOAmsg2ICEmsg(RequestMessage message)
        {
            Message msg = new Message();
            msg.fromID = message.FromId;
            msg.messageControl = message.MessageControl;
            msg.messageID = message.MessageId;
            msg.messageType = message.MessageType;
            msg.receiptAddress = message.TargetAddress;
            msg.receiptSessionID = message.CurrentRecepitId;
            msg.replyToID = message.ReplyToId;
            msg.routerAddress = message.RouterAddress;
            msg.senderSessionID = message.CurrentSenderId;
            msg.sessionId = message.SessionId;
            msg.toID = message.ToId;
            msg.spaceId = message.SpaceId;
            msg.clientCallBackProxy = message.ClientCallBackProxy;
            return msg;
        }
    }
}
