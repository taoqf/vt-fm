using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UserLoginPlugin.Models;
using UserLoginPlugin.Views;
using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Victop.Server.Controls.Models;
using Victop.Frame.PublicLib.Managers;
using Victop.Frame.MessageManager;
using Victop.Frame.CoreLibrary;
using Victop.Frame.PublicLib.Helpers;
using System.Threading;
using System.Data;
using Victop.Frame.SyncOperation;

namespace UserLoginPlugin.ViewModels
{
    public class UserLoginViewModel:ModelBase
    {
        #region 字段
        private Window LoginWindow;
        private PasswordBox pwBox;
        private string selectedGallery;
        #endregion

        #region 属性
        private string _ErrMsg;
        public string ErrMsg
        {
            get
            {
                return _ErrMsg;
            }
            set
            {
                if (_ErrMsg != value)
                {
                    _ErrMsg = value;
                    RaisePropertyChanged("ErrMsg");
                }
            }
        }

        private LoginUserInfoModel _LoginInfoModel;
        /// <summary>登录用户信息</summary>
        public LoginUserInfoModel LoginInfoModel
        {
            get
            {
                if (_LoginInfoModel == null)
                    _LoginInfoModel = new LoginUserInfoModel();
                return _LoginInfoModel;
            }
            set
            {
                if (_LoginInfoModel != value)
                {
                    _LoginInfoModel = value;
                    RaisePropertyChanged("LoginInfoModel");
                }
            }
        }

        private Dictionary<string, string> galleryList;
        /// <summary> 通道集合</summary>
        public Dictionary<string, string> GalleryList
        {
            get
            {
                if (galleryList == null)
                    galleryList = new Dictionary<string, string>();
                return galleryList;
            }
            set
            {
                if (galleryList != value)
                {
                    galleryList = value;
                    RaisePropertyChanged("GalleryList");
                }
            }
        }
        public string SelectedGallery
        {
            get
            {
                return selectedGallery;
            }
            set
            {
                selectedGallery = value;
                RaisePropertyChanged("SelectedGallery");
            }
        }
        #endregion

        #region Command命令
        #region 窗体初始化
        /// <summary>窗体初始化命令</summary>
        public ICommand gridMainLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    LoginWindow = (Window)x;
                    UserLoginInit();
                });
            }
        }
        public ICommand winMainUnloadedCommand
        {
            get
            {
                return new RelayCommand(() => {
                    PluginMessage pluginMessage = new PluginMessage();
                    pluginMessage.SendMessage("", OrganizeCloseData(LoginWindow.Uid), null);
                });
            }
        }
        private string OrganizeCloseData(string ObjectId)
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginStop");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("ObjectId", ObjectId);
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            return JsonHelper.ToJson(messageDic);
        }
        #endregion

        #region 选择通道命令
        /// <summary>选择通道命令 </summary>
        public ICommand CloudSOAComboBoxSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() => 
                {
                    SetCurrentGallery();
                });
            }
        }
        #endregion

        #region 系统设置命令
        public ICommand btnSystemSetClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    OpenSystemConfigWindow();
                });
            }
        }
        #endregion

        #region 窗体最小化命令
        /// <summary>窗体最小化命令 </summary>
        public ICommand btnMiniClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LoginWindow.WindowState = WindowState.Minimized;
                });
            }
        }
        #endregion

        #region 窗体关闭命令
        /// <summary>窗体关闭命令 </summary>
        public ICommand btnCloseClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MessageBoxResult result = MessageBox.Show("确定要退出么？", "", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        LoginWindow.Close();
                    }
                });
            }
        }
        #endregion

        #region 登录命令
        public ICommand btnLoginClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    pwBox = (PasswordBox)x;
                    UserLogin(pwBox.Password, LoginWindow);
                }, (x) => { return CheckUserLogin(); });
            }
        }
        #endregion

        #region 密码框内容改变时命令
        public ICommand pwBoxPasswordChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    //PasswordBox pwBoxCtrl = (PasswordBox)x;
                    //var select = pwBoxCtrl.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic);
                    //select.Invoke(pwBoxCtrl, new object[] { pwBox.Password.Length + 1, pwBox.Password.Length + 1 });
                });
            }
        }
        #endregion

        #endregion

        #region 自定义方法

        #region 加载登录窗体
        private void UserLoginInit()
        {
            string versionString = ConfigManager.GetAttributeOfNodeByName("System", "Mode");
            switch (versionString)
            {
                case null:
                case "":
                    //TODO:暂时不写
                    break;
                case "1":
                    GetGalleryInfo();
                    break;
                case "0":
                    GetGalleryInfo();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 获取通道信息
        /// <summary>获取通道信息 </summary>
        private void GetGalleryInfo()
        {
            new GalleryManager().SetCurrentGalleryId(Victop.Frame.CoreLibrary.Enums.GalleryEnum.ENTERPRISE);
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "GalleryService.GetGalleryInfo");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("usercode", "test7");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(BandingGalleryInfo));
        }
        /// <summary>绑定通道信息 </summary>
        private void BandingGalleryInfo(object message)
        {
            if (!JsonHelper.ReadJsonString(message.ToString(), "ReplyMode").Equals("0"))
            {
                string messageContent = JsonHelper.ReadJsonString(message.ToString(), "ReplyContent");
                GalleryList = JsonHelper.ReadJsonObject<Dictionary<string, string>>(messageContent);
            }
            else
            {
                MessageBox.Show(JsonHelper.ReadJsonString(message.ToString(), "ReplyAlertMessage"));
            }
        }
        #endregion

        #region 设置通道信息
        /// <summary>设置通道信息 </summary>
        private void SetCurrentGallery()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "GalleryService.SetGalleryInfo");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("GalleryKey", SelectedGallery);
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(SetCurrentGallery));
        }
        private void SetCurrentGallery(object message)
        {
            if (!JsonHelper.ReadJsonString(message.ToString(), "ReplyMode").Equals("0"))
            {
               
            }
            else
            {
                MessageBox.Show(JsonHelper.ReadJsonString(message.ToString(), "ReplyAlertMessage"));
            }
        }
        #endregion

        #region 打开系统设置窗体
        /// <summary>
        ///打开系统设置窗体
        /// </summary>
        private void OpenSystemConfigWindow()
        {
            //SystemConfig scWindow = new SystemConfig();
            //scWindow.ShowDialog();
        }
        #endregion

        #region 登录操作
        private void UserLogin(string pass,Window window)
        {
            window.Cursor = Cursors.Wait;
            try
            {
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("usercode", LoginInfoModel.UserName);
                contentDic.Add("userpw", LoginInfoModel.UserPwd);
                //new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(Login));
                string MessageType = "LoginService.userLoginNew";
                MessageOperation messageOp = new MessageOperation();
                Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic);
                if (!returnDic["ReplyMode"].ToString().Equals("0"))
                {
                    LoginWindow.Close();
                }
                else
                {
                    MessageBox.Show(string.IsNullOrEmpty(returnDic["ReplyAlertMessage"].ToString()) ? returnDic["ReplyContent"].ToString() : returnDic["ReplyAlertMessage"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                window.Cursor = Cursors.Arrow;
            }
        }
        /// <summary>检查输入信息</summary>
        private bool CheckUserLogin()
        {
            bool CheckStatus = true;
            if (string.IsNullOrEmpty(LoginInfoModel.UserName))
            {
                CheckStatus = false;
                return CheckStatus;
            }
            if (string.IsNullOrEmpty(selectedGallery))
            {
                CheckStatus = false;
                return CheckStatus;
            }
            return CheckStatus;
        }
        #endregion
      
        #endregion
    }
}
