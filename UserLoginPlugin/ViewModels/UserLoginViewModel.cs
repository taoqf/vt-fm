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
using Victop.Frame.PublicLib.Helpers;
using System.Threading;
using System.Data;
using Victop.Wpf.Controls;
using Victop.Frame.DataMessageManager;
using System.Windows.Threading;
using System.Diagnostics;
using System.Configuration;

namespace UserLoginPlugin.ViewModels
{
    public class UserLoginViewModel : ModelBase
    {
        #region 字段

       
        private Window LoginWindow;
        //把UserControl明转为Window
        UserLoginWindow newWindow;
        private PasswordBox pwBox;
        private string selectedGallery;
        private ObservableCollection<UserRoleInfoModel> roleInfoList;
        private UserRoleInfoModel selectedRoleInfo;
        private bool showRoleList;
        /// <summary>
        /// 是否显示加载
        /// </summary>
        private bool isRingShow = false;
        /// <summary>
        /// 主窗口是否可用
        /// </summary>
        private bool mainViewEnable = true;
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
        /// <summary>
        /// 角色信息集合
        /// </summary>
        public ObservableCollection<UserRoleInfoModel> RoleInfoList
        {
            get
            {
                if (roleInfoList == null)
                    roleInfoList = new ObservableCollection<UserRoleInfoModel>();
                return roleInfoList;
            }
            set
            {
                if (roleInfoList != value)
                {
                    roleInfoList = value;
                    RaisePropertyChanged("RoleInfoList");
                }
            }
        }
        /// <summary>
        /// 选定的角色信息
        /// </summary>
        public UserRoleInfoModel SelectedRoleInfo
        {
            get
            {
                return selectedRoleInfo;
            }
            set
            {
                if (selectedRoleInfo != value)
                {
                    selectedRoleInfo = value;
                    RaisePropertyChanged("SelectedRoleInfo");
                }
            }
        }
        public bool IsRingShow
        {
            get
            {
                return isRingShow;
            }
            set
            {
                if (isRingShow != value)
                {
                    isRingShow = value;
                    RaisePropertyChanged("IsRingShow");
                }
            }
        }
        /// <summary>
        /// 主窗口是否可用
        /// </summary>
        public bool MainViewEnable
        {
            get
            {
                return mainViewEnable;
            }
            set
            {
                if (mainViewEnable != value)
                {
                    mainViewEnable = value;
                    RaisePropertyChanged("MainViewEnable");
                }
            }
        }
        public bool ShowRoleList
        {
            get { return showRoleList; }
            set
            {
                if (showRoleList != value)
                {
                    showRoleList = value;
                    RaisePropertyChanged("ShowRoleList");
                }
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
                    UserControl ucLogin = (UserControl)x;
                   
                    FrameworkElement ct = (FrameworkElement)ucLogin.Parent;
                    while (true)
                    {
                        if (ct is Window)
                        {
                            LoginWindow = (Window)ct;
                            break;
                        }
                        ct = (FrameworkElement)ct.Parent;
                    }
                    UserLoginInit();
                });
            }
        }
        /// <summary>
        /// 窗体卸载
        /// </summary>
        public ICommand gridMainUnloadedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataMessageOperation messageOp = new DataMessageOperation();
                    string messageType = "PluginService.PluginStop";
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("ObjectId", LoginWindow.Uid);
                    messageOp.SendAsyncMessage(messageType, contentDic);
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
                    VisualStateManager.GoToState(LoginWindow, "SecondPage", true);
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
                    LoginWindow.Close();
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
                            if (LoginInfoModel.UserPwd.Equals("111111"))
                            {

                                VicMessageBoxNormal.Show("密码过于简单,将转向修改密码界面！");
                                Process proc = new System.Diagnostics.Process();
                                proc.StartInfo.FileName = string.Format("{0}?userCode={1}&ClientId={2}&ProductId={3}", ConfigurationManager.AppSettings["updatepwdhttp"], LoginInfoModel.UserName, string.Format("{0}::{1}", LoginInfoModel.ClientId,LoginInfoModel.ProductId), LoginInfoModel.ProductId);
                                proc.Start();
                                return;
                            }
                            else
                            {
                                IsRingShow = true;
                                MainViewEnable = false;
                                UserLogin();
                            }
                      
                    }, () => { return CheckUserLogin(); });
            }
        }
        #endregion

        #region 取消命令
        /// <summary>取消命令 </summary>
        public ICommand btnCancelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LoginWindow.Close();
                });
            }
        }
        #endregion
        /// <summary>
        /// 角色列表窗体加载命令
        /// </summary>
        public ICommand roleListWinLoadedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }



        /// <summary>
        /// 角色列表选定命令
        /// </summary>
        public ICommand btnConfirmClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    #region 更新通道信息
                    DataMessageOperation dataOp = new DataMessageOperation();
                    string messageType = "LoginService.setUserInfo";
                    Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
                    setUserContentDic.Add("UserCode", LoginInfoModel.UserName);
                    setUserContentDic.Add("UserPwd", LoginInfoModel.UserPwd);
                    setUserContentDic.Add("ClientId", LoginInfoModel.ClientId);
                    setUserContentDic.Add("ProductId", LoginInfoModel.ProductId);
                    setUserContentDic.Add("ClientNo", LoginInfoModel.ClientNo);
                    setUserContentDic.Add("UserRole", SelectedRoleInfo.Role_No);
                    dataOp.SendAsyncMessage(messageType, setUserContentDic);
                    #region 为关闭窗体添加动画
                    newWindow = LoginWindow as UserLoginWindow;
                    if (newWindow != null)
                    {
                        newWindow.FrontGD.tm.Tick += tm_Tick;
                        newWindow.FrontGD.tm.Start();
                    }
                    #endregion
                    #endregion
                }, () =>
                {
                    if (SelectedRoleInfo == null)
                    {
                        return false;
                    }
                    return true;
                });
            }
        }



        #endregion

        #region 自定义方法

        #region 关闭窗口添加命令
        private void tm_Tick(object sender, EventArgs e)
        {
            if (newWindow != null)
            {
                newWindow.FrontGD.tm.Stop();
            }
            ShowRoleList = false;
            LoginWindow.DialogResult = true;
        }
        #endregion

        #region 加载登录窗体
        private void UserLoginInit()
        {
            string versionString = ConfigManager.GetAttributeOfNodeByName("System", "Mode");
            LoginInfoModel.UserName = ConfigManager.GetAttributeOfNodeByName("UserInfo", "User");
            LoginInfoModel.UserPwd = ConfigManager.GetAttributeOfNodeByName("UserInfo", "Pwd");
            LoginInfoModel.ClientId = ConfigManager.GetAttributeOfNodeByName("UserInfo", "ClientId");
            LoginInfoModel.ProductId = ConfigManager.GetAttributeOfNodeByName("UserInfo", "ProductId");
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
            VisualStateManager.GoToState(LoginWindow, "FirstPage", false);
        }
        #endregion

        #region 获取通道信息
        /// <summary>获取通道信息 </summary>
        private void GetGalleryInfo()
        {
            string messageType = "GalleryService.GetGalleryInfo";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("usercode", "test7");
            DataMessageOperation messageOp = new DataMessageOperation();
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(messageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                GalleryList = JsonHelper.ReadJsonObject<Dictionary<string, string>>(returnDic["ReplyContent"].ToString());
                if (GalleryList != null && GalleryList.Count > 0)
                {
                    SelectedGallery = GalleryList.Keys.First();
                }
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
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendAsyncMessage(messageType, contentDic);
        }
        #endregion

        #region 登录操作
        void AfterLogin(object returnMsg)
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            Dictionary<string, object> returnDic = JsonHelper.ToObject<Dictionary<string, object>>(returnMsg.ToString());
            if (returnDic != null)
            {
                if (!returnDic["ReplyMode"].ToString().Equals("0"))
                {
                    IsRingShow = false;
                    //SaveLoginUserInfo();
                    Dictionary<string, object> result = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                    RoleInfoList = JsonHelper.ToObject<ObservableCollection<UserRoleInfoModel>>(JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserRole"));
                    if (RoleInfoList == null || RoleInfoList.Count == 0)
                    {
                        DataMessageOperation dataOp = new DataMessageOperation();
                        string messageType = "LoginService.setUserInfo";
                        Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
                        setUserContentDic.Add("UserCode", LoginInfoModel.UserName);
                        setUserContentDic.Add("UserPwd", LoginInfoModel.UserPwd);
                        setUserContentDic.Add("ClientId", LoginInfoModel.ClientId);
                        setUserContentDic.Add("ProductId", LoginInfoModel.ProductId);
                        setUserContentDic.Add("ClientNo", LoginInfoModel.ClientNo);
                        dataOp.SendAsyncMessage(messageType, setUserContentDic);
                        Application.Current.Dispatcher.Invoke((Action)delegate { this.LoginWindow.DialogResult = true; });
                        
                        }
                        else
                        {
                            if (RoleInfoList.Count == 1)
                            {
                                DataMessageOperation dataOp = new DataMessageOperation();
                                string messageType = "LoginService.setUserInfo";
                                Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
                                setUserContentDic.Add("UserCode", LoginInfoModel.UserName);
                                setUserContentDic.Add("UserPwd", LoginInfoModel.UserPwd);
                                setUserContentDic.Add("ClientId", LoginInfoModel.ClientId);
                                setUserContentDic.Add("ProductId", LoginInfoModel.ProductId);
                                setUserContentDic.Add("ClientNo", LoginInfoModel.ClientNo);
                                setUserContentDic.Add("UserRole", RoleInfoList[0].Role_No);
                                dataOp.SendAsyncMessage(messageType, setUserContentDic);
                            Application.Current.Dispatcher.Invoke((Action)delegate { this.LoginWindow.DialogResult = true; });

                            }
                            else
                            {
                                ShowRoleList = true;
                                if (this.LoginWindow != null)
                                {
                                    this.LoginWindow.Height = 1; this.LoginWindow.Width = 1;
                                }
                            }
                        }

                    }
                    else
                    {
                        IsRingShow = false;
                        MainViewEnable = true;
                        MessageBox.Show((returnDic["ReplyAlertMessage"] == null || string.IsNullOrEmpty(returnDic["ReplyAlertMessage"].ToString())) ? returnDic["ReplyContent"].ToString() : returnDic["ReplyAlertMessage"].ToString());
                    }
                }

            else
            {
                IsRingShow = false;
                MainViewEnable = true;
                MessageBox.Show("登录失败");
            }
        }
        private void UserLogin()
        {
            try
            {
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("usercode", LoginInfoModel.UserName);
                contentDic.Add("userpw", LoginInfoModel.UserPwd);
                contentDic.Add("spaceId", string.Format("{0}::{1}", LoginInfoModel.ClientId, string.IsNullOrEmpty(LoginInfoModel.ProductId) ? LoginInfoModel.ClientId : LoginInfoModel.ProductId));
                string MessageType = "LoginService.userLogin";
                DataMessageOperation messageOp = new DataMessageOperation();
                //Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                messageOp.SendAsyncMessage(MessageType, contentDic, new WaitCallback(AfterLogin));


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

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
        /// <summary>
        /// 保存登录用户信息
        /// </summary>
        private void SaveLoginUserInfo()
        {
            Dictionary<string, string> userDic = new Dictionary<string, string>();
            userDic.Add("User", LoginInfoModel.UserName);
            userDic.Add("Pwd", LoginInfoModel.UserPwd);
            userDic.Add("ProductId", LoginInfoModel.ProductId);
            ConfigManager.SaveAttributeOfNodeByName("UserInfo", userDic);
            DataMessageOperation messageOp = new DataMessageOperation();
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "12");
            contentDic.Add("configsystemid", "11");
            contentDic.Add("modelid", "feidao-model-pub_product-0001");
            List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "pub_product");
            List<Dictionary<string, object>> tableConList = new List<Dictionary<string, object>>();
            Dictionary<string, object> tableConDic = new Dictionary<string, object>();
            tableConDic.Add("productid", LoginInfoModel.ProductId);
            tableConList.Add(tableConDic);
            conDic.Add("tablecondition", tableConList);
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                string channelId = returnDic["DataChannelId"].ToString();
                DataSet mastDs = messageOp.GetData(channelId, "[\"pub_product\"]");
                if (mastDs != null && mastDs.Tables.Contains("dataArray") && mastDs.Tables["dataArray"].Rows.Count > 0)
                {
                    DataRow[] drs = mastDs.Tables["dataArray"].Select(string.Format("productid='{0}'", LoginInfoModel.ProductId));
                    if (drs != null && drs.Count() > 0)
                    {
                        LoginInfoModel.ClientNo = drs[0]["client_no"].ToString();
                    }
                }
            }

        }
        #endregion

        #endregion
    }
}
