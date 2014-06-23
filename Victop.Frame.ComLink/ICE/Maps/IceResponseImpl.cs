using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using slice;
using Victop.Frame.CoreLibrary.Interfaces;
using Victop.Frame.CoreLibrary.Enums;
using Victop.Frame.CoreLibrary.Models;

namespace Victop.Frame.ComLink.ICE.Maps
{
    public class IceResponseImpl:IResponse
    {
        public Reply entry = new Reply();

        private AMD_MessageEndpoint_sendMessage cb;
        private AMD_MessageEndpoint_sendMessageNew _cb;
        private bool flag = false;

        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        public IceResponseImpl(ReplyModeEnum mode)
        {
            this.entry.replyMode = (int)mode;
        }

        public IceResponseImpl(ReplyModeEnum mode, AMD_MessageEndpoint_sendMessage cb)
        {
            this.entry.replyMode = (int)mode;
            this.cb = cb;
        }
        public IceResponseImpl(ReplyModeEnum mode, AMD_MessageEndpoint_sendMessageNew _cb)
        {
            this.entry.replyMode = (int)mode;
            this._cb = _cb;
        }
        /**
        * 发出回应
        * @param replyControl 应答的消息控制
        * @param replyContent 应答的消息内容
        * @param mode 消息处理方式，枚举型：SYNCH=同步、ASYNC=异步、ROUTER=转发、CAST=丢弃
        */
        public void Reply(String replyControl, String replyContent, ReplyModeEnum mode)
        {
            ReplyMessage replyMessage = new ReplyMessage();
            replyMessage.ReplyMode = mode;
            replyMessage.ReplyControl = replyControl;
            replyMessage.ReplyContent = replyContent;
            Reply(replyMessage);
        }


        /**
         * 发出回应
         * @param replyContent 应答的消息内容
         * @param mode 消息处理方式，枚举型：SYNCH=同步、ASYNC=异步、ROUTER=转发、CAST=丢弃
         */
        public void Reply(String replyContent, ReplyModeEnum mode)
        {
            Reply("", replyContent, mode);
        }

        /**
         * 发出回应
         * @param replyMessage
         */
        public void Reply(ReplyMessage replyMessage)
        {
            //entry = ResponseUtil.toReply(replyMessage);//TODO:
            flag = true;
            if (cb != null)
                cb.ice_response(this.entry);
        }
    }
}
