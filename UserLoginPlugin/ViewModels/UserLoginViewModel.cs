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
using Victop.Wpf.Controls;

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
        /// <summary>
        /// 窗体卸载
        /// </summary>
        public ICommand winMainUnloadedCommand
        {
            get
            {
                return new RelayCommand(() => {
                    MessageOperation messageOp = new MessageOperation();
                    string messageType = "PluginService.PluginStop";
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("ObjectId", LoginWindow.Uid);
                    messageOp.SendMessage(messageType,contentDic);
                });
            }
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
                    MessageBoxResult result = VicMessageBoxNormal.Show("确定要退出么？", "", MessageBoxButton.YesNo);
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
                return new RelayCommand(() =>
                {
                    UserLogin();
                }, () => { return CheckUserLogin(); });
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
            string messageType = "GalleryService.GetGalleryInfo";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("usercode", "test7");
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                GalleryList = JsonHelper.ReadJsonObject<Dictionary<string, string>>(returnDic["ReplyContent"].ToString());
            }
        }
        #endregion

        #region 设置通道信息
        /// <summary>设置通道信息 </summary>
        private void SetCurrentGallery()
        {
            string messageType = "GalleryService.SetGalleryInfo";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("GalleryKey", SelectedGallery);
            MessageOperation messageOp = new MessageOperation();
            messageOp.SendMessage(messageType, contentDic);
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
        private void UserLogin()
        {
            try
            {
                LoginWindow.Cursor = Cursors.Wait;
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("usercode", LoginInfoModel.UserName);
                contentDic.Add("userpw", LoginInfoModel.UserPwd);
                string MessageType = "LoginService.userLoginNew";
                MessageOperation messageOp = new MessageOperation();
                Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic);
                if (returnDic != null)
                {
                    if (!returnDic["ReplyMode"].ToString().Equals("0"))
                    {
                        LoginWindow.DialogResult = true;
                        LoginWindow.Close();
                    }
                    else
                    {
                        MessageBox.Show((returnDic["ReplyAlertMessage"] == null || string.IsNullOrEmpty(returnDic["ReplyAlertMessage"].ToString())) ? returnDic["ReplyContent"].ToString() : returnDic["ReplyAlertMessage"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("登录失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoginWindow.Cursor = Cursors.Arrow;
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
