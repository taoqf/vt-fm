using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 飞道浏览器可操作方法
    /// </summary>
    public class FeiDaoBrowserOperation
    {
        #region 私有对象
        DataMessageOperation dataOp = new DataMessageOperation();
        #endregion
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="messageContent">消息体</param>
        /// <returns></returns>
        public string SendMessage(string messageType, string messageContent)
        {
            LoggerHelper.DebugFormat("js send message:MessageType:{0},MessageCotent;{1}", messageType, messageContent);
            Dictionary<string, object> messageDic = new Dictionary<string, object>();
            Dictionary<string, object> contentDic = string.IsNullOrEmpty(messageContent) ? new Dictionary<string, object>() : JsonHelper.ToObject<Dictionary<string, object>>(messageContent);
            Dictionary<string, object> resultDic = dataOp.SendSyncMessage(messageType, contentDic);
            if (resultDic != null)
            {
                string replyCode = resultDic["ReplyMode"].ToString();
                if (replyCode.Equals("1"))
                {
                    messageDic.Add("code", replyCode);
                    messageDic.Add("message", resultDic["ReplyContent"]);
                }
                else
                {
                    messageDic.Add("code", replyCode);
                    messageDic.Add("message", resultDic["ReplyAlertMessage"]);
                }
            }
            else
            {
                messageDic.Add("code", "-1");
                messageDic.Add("message", "发送消息异常");
            }
            LoggerHelper.DebugFormat("wpf return string:{0}", JsonHelper.ToJson(messageDic));
            return JsonHelper.ToJson(messageDic);
        }
    }
}
