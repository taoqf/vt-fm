using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls;
using Victop.Wpf.Controls;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 单据模板类
    /// </summary>
    public class BillTemplateControl : TemplateControl
    {
        BillDefinModel definModel = new BillDefinModel();
        /// <summary>
        /// 凭证
        /// </summary>
        public BillDefinModel DefinModel
        {
            get { return definModel; }
            set { definModel = value; }
        }

        #region 发送获取编码消息
        /// <summary>
        /// 发送获取编码消息
        /// </summary>
        public string SendGetCodeMessage()
        {
            string codeNo = string.Empty;
            try
            {
                string relStr = JsonHelper.ReadJsonString(DefinModel.EncodedService.ToString(), "dataArray");
                if (!string.IsNullOrEmpty(relStr))
                {
                    List<EncodedServiceModel> codeList = JsonHelper.ToObject<List<EncodedServiceModel>>(relStr);
                    if (codeList != null)
                    {
                        string MessageType = "MongoDataChannelService.findDocCode";
                        DataMessageOperation messageOp = new DataMessageOperation();
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", "11");
                        foreach (EncodedServiceModel model in codeList)
                        {
                            switch (model.ParamType)
                            {
                                case "field":
                                    break;
                                case "syspan":
                                    contentDic.Add("setinfo", model.ParamName);
                                    break;
                                case "const":
                                    contentDic.Add("pname", model.ParamName);
                                    break;
                            }
                        }
                        Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                        if (returnDic != null || returnDic.ContainsKey("ReplyContent"))
                        {
                            codeNo = JsonHelper.ReadJsonString(returnDic["ReplyContent"].ToString(), "result");
                        }
                        else
                        {
                            codeNo = string.Empty;
                        }
                    }
                    else
                    {
                        codeNo = string.Empty;
                    }
                }
                else
                {
                    codeNo = string.Empty;
                }
                return codeNo;
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
