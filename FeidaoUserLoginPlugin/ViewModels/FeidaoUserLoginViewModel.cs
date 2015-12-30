using FeidaoUserLoginPlugin.Models;
using FeidaoUserLoginPlugin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Victop.Frame.DataMessageManager;
using Victop.Frame.DataMessageManager.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.PublicLib.Managers;
using Victop.Server.Controls.Models;
using Victop.Server.Controls.MVVM;
using Victop.Wpf.Controls;

namespace FeidaoUserLoginPlugin.ViewModels
{
    public class FeidaoUserLoginViewModel : ModelBase
    {
        private Window LoginWindow;
        //把UserControl明转为Window
        FeidaoUserLoginWindow newWindow;
        private PasswordBox pwBox;
        private string selectedGallery;
        private ObservableCollection<FeidaoUserRoleInfoModel> roleInfoList;
        private FeidaoUserRoleInfoModel selectedRoleInfo;
        private bool showRoleList;
        private Visibility visRole = Visibility.Collapsed;
        private Visibility visLogin = Visibility.Visible;
        private bool visSystemSet = true;
        private bool visMini = true;
        private bool visClose = true;
        /// <summary>
        /// 是否显示加载
        /// </summary>
        private bool isRingShow = false;
        /// <summary>
        /// 主窗口是否可用
        /// </summary>
        private bool mainViewEnable = true;
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
                    RaisePropertyChanged(() => ErrMsg);
                }
            }
        }
        public bool VisMini
        {
            get { return visMini; }
            set
            {
                if (visMini != value)
                {
                    visMini = value;
                }
                RaisePropertyChanged(() => VisMini);
            }
        }
        public bool VisClose
        {
            get { return visClose; }
            set
            {
                if (visClose != value)
                {
                    visClose = value;
                }
                RaisePropertyChanged(() => VisClose);
            }
        }
        public bool VisSystemSet
        {
            get { return visSystemSet; }
            set
            {
                if (visSystemSet != value)
                {
                    visSystemSet = value;
                }
                RaisePropertyChanged(() => VisSystemSet);
            }
        }
        public Visibility VisLogin
        {
            get { return visLogin; }
            set
            {
                if (visLogin != value)
                {
                    visLogin = value;
                }
                RaisePropertyChanged(() => VisLogin);
            }
        }
        public Visibility VisRole
        {
            get { return visRole; }
            set
            {
                if (visRole != value)
                {
                    visRole = value;
                }
                RaisePropertyChanged(() => VisRole);
            }
        }
        private FeidaoUserLoginModel _LoginInfoModel;
        /// <summary>登录用户信息</summary>
        public FeidaoUserLoginModel LoginInfoModel
        {
            get
            {
                if (_LoginInfoModel == null)
                    _LoginInfoModel = new FeidaoUserLoginModel();
                return _LoginInfoModel;
            }
            set
            {
                if (_LoginInfoModel != value)
                {
                    _LoginInfoModel = value;
                    RaisePropertyChanged(() => LoginInfoModel);
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
                    RaisePropertyChanged(() => GalleryList);
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
                RaisePropertyChanged(() => SelectedGallery);
            }
        }
        /// <summary>
        /// 角色信息集合
        /// </summary>
        public ObservableCollection<FeidaoUserRoleInfoModel> RoleInfoList
        {
            get
            {
                if (roleInfoList == null)
                    roleInfoList = new ObservableCollection<FeidaoUserRoleInfoModel>();
                return roleInfoList;
            }
            set
            {
                if (roleInfoList != value)
                {
                    roleInfoList = value;
                    RaisePropertyChanged(() => RoleInfoList);
                }
            }
        }
        /// <summary>
        /// 选定的角色信息
        /// </summary>
        public FeidaoUserRoleInfoModel SelectedRoleInfo
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
                    RaisePropertyChanged(() => SelectedRoleInfo);
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
                    RaisePropertyChanged(() => IsRingShow);
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
                    RaisePropertyChanged(() => MainViewEnable);
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
                    RaisePropertyChanged(() => ShowRoleList);
                }
            }
        }



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
        /// <summary>
        /// 修改密码
        /// </summary>
        public ICommand btnModifyPassClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataMessageOperation dataOp = new DataMessageOperation();
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add("userCode", LoginInfoModel.UserName);
                    PluginModel pluginModel = dataOp.StartPlugin(new ExcutePluginParamModel() { PluginName = "ModifyPassWordPlugin", VisiblePlugin = false }, paramDic);
                    if (pluginModel.ErrorMsg == null || pluginModel.ErrorMsg == "")
                    {
                        Window win = pluginModel.PluginInterface.StartWindow;
                        win.ShowDialog();
                    }

                });
            }
        }
        public ICommand btnLoginClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (LoginInfoModel.UserPwd.Equals("111111"))
                    {
                        if (VicMessageBoxNormal.Show("密码为初始化密码，请登录后修改密码", "标题", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                        {
                            IsRingShow = true;
                            MainViewEnable = false;
                            UserLogin();
                            DataMessageOperation dataOp = new DataMessageOperation();
                            Dictionary<string, object> paramDic = new Dictionary<string, object>();
                            paramDic.Add("usercode", LoginInfoModel.UserName);
                            PluginModel pluginModel = dataOp.StartPlugin(new ExcutePluginParamModel() { PluginName = "ModifyPassWordPlugin", VisiblePlugin = false }, paramDic);
                            if (pluginModel.ErrorMsg == null || pluginModel.ErrorMsg == "")
                            {
                                Window win = pluginModel.PluginInterface.StartWindow;
                                win.ShowDialog();
                            }
                        }
                        else
                        {
                            IsRingShow = true;
                            MainViewEnable = false;
                            UserLogin();
                        }

                    }
                    else
                    {
                        IsRingShow = true;
                        MainViewEnable = false;
                        UserLogin();
                    }
                }, () =>
                {
                    return CheckUserLogin();
                });
            }
        }
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
                    Dictionary<string, object> menuDic = GetCurrentRoleMenu();
                    if (menuDic != null && !menuDic["ReplyMode"].ToString().Equals("0"))
                    {
                        DataMessageOperation dataOp = new DataMessageOperation();
                        string messageType = "LoginService.setUserInfo";
                        Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
                        setUserContentDic.Add("UserCode", LoginInfoModel.UserName);
                        setUserContentDic.Add("UserPwd", LoginInfoModel.UserPwd);
                        setUserContentDic.Add("ClientId", LoginInfoModel.ClientId);
                        setUserContentDic.Add("UserRole", SelectedRoleInfo.Role_No);
                        dataOp.SendSyncMessage(messageType, setUserContentDic);
                        LoginWindow.DialogResult = true;
                        ShowRoleList = false;
                        visRole = Visibility.Collapsed;
                        MainViewEnable = true;
                    }
                    else
                    {
                        ShowRoleList = false;
                        visRole = Visibility.Collapsed;
                        MainViewEnable = true;
                        VicMessageBoxNormal.Show(menuDic["ReplyAlertMessage"].ToString());
                    }
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

        private Dictionary<string, object> GetCurrentRoleMenu(string roleNo = null)
        {
            DataMessageOperation dataOp = new DataMessageOperation();
            string messageType = "MongoDataChannelService.getuserauthInfo";
            Dictionary<string, object> menuDic = new Dictionary<string, object>();
            menuDic.Add("systemid", ConfigurationManager.AppSettings["clientsystem"]);
            menuDic.Add("productid", LoginInfoModel.ClientId);
            menuDic.Add("usercode", LoginInfoModel.UserName);
            menuDic.Add("role_no", string.IsNullOrEmpty(roleNo) ? SelectedRoleInfo.Role_No : roleNo);
            menuDic.Add("client_type", "3");
            Dictionary<string, object> resultDic = dataOp.SendSyncMessage(messageType, menuDic);
            return resultDic;
        }
        private void UserLoginInit()
        {
            string versionString = ConfigManager.GetAttributeOfNodeByName("System", "Mode");
            LoginInfoModel.UserName = ConfigManager.GetAttributeOfNodeByName("UserInfo", "User");
            LoginInfoModel.UserPwd = ConfigManager.GetAttributeOfNodeByName("UserInfo", "Pwd");
            LoginInfoModel.ClientId = ConfigManager.GetAttributeOfNodeByName("UserInfo", "ClientId");
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
        /// <summary>设置通道信息 </summary>
        private void SetCurrentGallery()
        {
            string messageType = "GalleryService.SetGalleryInfo";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("GalleryKey", SelectedGallery);
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendAsyncMessage(messageType, contentDic);
        }
        void AfterLogin(object returnMsg)
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            Dictionary<string, object> returnDic = JsonHelper.ToObject<Dictionary<string, object>>(returnMsg.ToString());
            if (returnDic != null)
            {
                if (returnDic["ReplyMode"].ToString().Equals("1"))
                {
                    IsRingShow = false;
                    SaveLoginUserInfo();
                    Dictionary<string, object> result = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                    RoleInfoList = JsonHelper.ToObject<ObservableCollection<FeidaoUserRoleInfoModel>>(JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserRole"));
                    if (RoleInfoList == null || RoleInfoList.Count == 0)
                    {
                        Dictionary<string, object> resultDic = GetCurrentRoleMenu(Guid.NewGuid().ToString());
                        DataMessageOperation dataOp = new DataMessageOperation();
                        string messageType = "LoginService.setUserInfo";
                        Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
                        setUserContentDic.Add("UserCode", LoginInfoModel.UserName);
                        setUserContentDic.Add("UserPwd", LoginInfoModel.UserPwd);
                        setUserContentDic.Add("ClientId", LoginInfoModel.ClientId);
                        dataOp.SendSyncMessage(messageType, setUserContentDic);
                        Application.Current.Dispatcher.Invoke((Action)delegate { this.LoginWindow.DialogResult = true; });

                    }
                    else
                    {
                        if (RoleInfoList.Count == 1)
                        {
                            Dictionary<string, object> resultDic = GetCurrentRoleMenu(RoleInfoList[0].Role_No);
                            if (resultDic != null && resultDic["ReplyMode"].ToString().Equals("1"))
                            {
                                DataMessageOperation dataOp = new DataMessageOperation();
                                string messageType = "LoginService.setUserInfo";
                                Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
                                setUserContentDic.Add("UserCode", LoginInfoModel.UserName);
                                setUserContentDic.Add("UserPwd", LoginInfoModel.UserPwd);
                                setUserContentDic.Add("ClientId", LoginInfoModel.ClientId);
                                setUserContentDic.Add("UserRole", RoleInfoList[0].Role_No);
                                dataOp.SendSyncMessage(messageType, setUserContentDic);
                                Application.Current.Dispatcher.Invoke((Action)delegate { this.LoginWindow.DialogResult = true; });

                            }
                            else
                            {
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    VicMessageBoxNormal.Show(resultDic["ReplyAlertMessage"].ToString());
                                    this.LoginWindow.DialogResult = true;
                                });
                            }

                        }
                        else
                        {
                            VisLogin = Visibility.Collapsed;
                            ShowRoleList = true;
                            MainViewEnable = true;
                            VisRole = Visibility.Visible;
                            VisSystemSet = false;
                            VisMini = false;
                            VisClose = false;
                            if (this.LoginWindow != null)
                            {
                                Application.Current.Dispatcher.Invoke((Action)delegate { this.LoginWindow.Height = 626; this.LoginWindow.Width = 386; });
                            }
                        }
                    }

                }
                else
                {
                    IsRingShow = false;
                    MainViewEnable = true;
                    MessageBox.Show(returnDic["ReplyAlertMessage"] == null || string.IsNullOrEmpty(returnDic["ReplyAlertMessage"].ToString()) ? returnDic["ReplyContent"].ToString() : returnDic["ReplyAlertMessage"].ToString());
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
                DataMessageOperation messageOp = new DataMessageOperation();
                Dictionary<string, object> loginDic = new Dictionary<string, object>();
                var dic = messageOp.SendSyncMessage("MongoDataChannelService.loginout", loginDic);
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("usercode", LoginInfoModel.UserName);
                contentDic.Add("userpw", LoginInfoModel.UserPwd);
                string MessageType = "LoginService.userLogin";
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
            ConfigManager.SaveAttributeOfNodeByName("UserInfo", userDic);
            LoginInfoModel.ClientId = ConfigManager.GetAttributeOfNodeByName("UserInfo", "ClientId");
        }
    }
}
