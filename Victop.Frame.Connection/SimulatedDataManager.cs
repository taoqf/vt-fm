using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Victop.Frame.CoreLibrary.Enums;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.Connection
{
    /// <summary>
    /// 模拟数据管理器
    /// </summary>
    public class SimulatedDataManager
    {
        /// <summary>
        /// 提交请求
        /// </summary>
        /// <param name="messageInfo">请求消息类型</param>
        /// <returns></returns>
        public ReplyMessage SubmitRequest(RequestMessage messageInfo)
        {
            ReplyMessage replyMessage = new ReplyMessage();
            replyMessage.MessageId = messageInfo.MessageId;
            string modelId = JsonHelper.ReadJsonString(messageInfo.MessageContent, "modelId");
            if (!string.IsNullOrEmpty(modelId))
            {
                Dictionary<string, object> returnContentDic = new Dictionary<string, object>();
                int ReplyCode;
                string dataStr = ReadLocalData(modelId, out ReplyCode);
                returnContentDic.Add("Result", dataStr);
                returnContentDic.Add("code", ReplyCode);
                returnContentDic.Add("messageType", messageInfo.MessageType);
                replyMessage.ReplyContent = JsonHelper.ToJson(returnContentDic);
                replyMessage.ReplyMode = (ReplyModeEnum)ReplyCode;
                if (ReplyCode.Equals(0))
                {
                    replyMessage.ReplyAlertMessage = dataStr;
                }
            }
            else
            {
                Dictionary<string, object> returnContentDic = new Dictionary<string, object>();
                returnContentDic.Add("Result", "数据标识无效");
                returnContentDic.Add("code", 0);
                returnContentDic.Add("messageType", messageInfo.MessageType);
                replyMessage.ReplyContent = JsonHelper.ToJson(returnContentDic);
                replyMessage.ReplyMode = (ReplyModeEnum)0;
                replyMessage.ReplyAlertMessage = "数据标识无效";
            }
            return replyMessage;
        }
        /// <summary>
        /// 读取本地数据
        /// </summary>
        /// <param name="modelId">模型标识</param>
        /// <returns></returns>
        private string ReadLocalData(string modelId,out int opCode)
        {
            try
            {
                string returnStr = string.Empty;
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "data";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileFullName = Path.Combine(filePath, modelId + ".xml");
                if (File.Exists(fileFullName))
                {
                    using (StreamReader fileReader = new StreamReader(fileFullName))
                    {
                        returnStr = fileReader.ReadToEnd();
                    }
                    opCode = 1;
                }
                else
                {
                    returnStr = "数据标识对应的文件不存在";
                    opCode = 0;
                }
                return returnStr;
            }
            catch (Exception ex)
            {
                opCode = 0;
                return ex.Message;
            }
        }
    }
}
