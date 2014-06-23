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
            message.FromId =msg.fromID;
            message.FromRole = msg.fromRole;
            message.MessageControl = msg.messageControl;
            message.MessageId = msg.replyToID;
            message.MessageType =msg.messageType;
            message.TargetAddress =msg.receiptAddress;
            message.CurrentRecepitId =msg.receiptSessionID;
            message.ReplyToId =msg.replyToID;
            message.RouterAddress =msg.routerAddress;
            message.CurrentSenderId =msg.senderSessionID;
            message.SessionId =msg.sessionId;
            message.ToId=msg.toID;
            message.ToRole =msg.toRole;
            message.MessageContent =msg.messageContent;
            return message;
        }
        public static Message SOAmsg2ICEmsg(RequestMessage message)
        {
            Message msg = new Message();
            msg.fromID = message.FromId;
            msg.fromRole = message.FromRole;
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
            msg.toRole = message.ToRole;
            return msg;
        }
    }
}
