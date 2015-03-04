using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls;

namespace UserManagerService
{
    public class Service : IService
    {
        public int AutoInit
        {
            get { return 0; }
        }

        public string ServiceName
        {
            get { return "UserManagerService"; }
        }

        private List<string> serviceReceiptMessageType;

        public List<string> ServiceReceiptMessageType
        {
            get
            {
                if (serviceReceiptMessageType == null)
                {
                    serviceReceiptMessageType = new List<string>();
                    serviceReceiptMessageType.Add("ServerCenterService.GetUserInfo");
                }
                return serviceReceiptMessageType;

            }
        }
        private string currentMessageType;
        public string CurrentMessageType
        {
            get
            {
                return currentMessageType;
            }
            set
            {
                currentMessageType = value;
            }
        }

        private string serviceParams;
        public string ServiceParams
        {
            get
            {
                return serviceParams;
            }
            set
            {
                serviceParams = value;
            }
        }

        public string ServiceDescription
        {
            get { return "用户信息管理"; }
        }

        public bool ServiceRun()
        {
            Dictionary<string, object> returnDic = new Dictionary<string, object>();
            try
            {
                GalleryManager galleryManager = new GalleryManager();
                CloudGalleryInfo cloudGallyInfo = galleryManager.GetGallery(GalleryManager.GetCurrentGalleryId().ToString());
                LoginUserInfo loginUserInfo = cloudGallyInfo.ClientInfo;
                Dictionary<string, object> userDic = new Dictionary<string, object>();
                userDic.Add("UserName", loginUserInfo.UserName);
                userDic.Add("UserCode", loginUserInfo.UserCode);
                userDic.Add("ClientNo", cloudGallyInfo.ClientNo);
                userDic.Add("ProductId", cloudGallyInfo.ProductId);
                userDic.Add("UserId", loginUserInfo.UserId);
                userDic.Add("UserImg", loginUserInfo.UserImg);
                if (loginUserInfo.UserFullInfo != null && loginUserInfo.UserFullInfo.Count > 0 && loginUserInfo.UserFullInfo[0].ContainsKey("staff_no"))
                {
                    userDic.Add("UserNo", loginUserInfo.UserFullInfo[0]["staff_no"].ToString());
                }
                returnDic.Add("ReplyContent", userDic);
                returnDic.Add("ReplyMode", 1);
                returnDic.Add("ReplyAlertMessage", null);
                return true;
            }
            catch (Exception ex)
            {
                returnDic.Add("ReplyContent", ex.Message);
                returnDic.Add("ReplyMode", 0);
                returnDic.Add("ReplyAlertMessage", ex.Message);
                return false;
            }
            finally
            {
                replyContent = JsonHelper.ToJson(returnDic);
            }
        }

        private string replyContent;

        public string ReplyContent
        {
            get { return replyContent; }
        }
    }
}
