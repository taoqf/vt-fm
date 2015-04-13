using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroUserLoginPlugin.Models;
using MetroUserLoginPlugin.Views;
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
using System.Text.RegularExpressions;
using MetroUserLoginPlugin.Enums;

namespace MetroUserLoginPlugin.ViewModels
{
    public class UserLoginViewModel : ModelBase
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
                    try
                    {
                        VisualStateManager.GoToState(LoginWindow, "SecondPage", true);
                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show(ex.ToString());
                    }
                  
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
                    UserLogin();
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
        #endregion

        #region 自定义方法

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
        private void UserLogin()
        {
            try
            {
                LoginWindow.Cursor = Cursors.Wait;
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("usercode", LoginInfoModel.UserName);
                contentDic.Add("userpw", LoginInfoModel.UserPwd);
                contentDic.Add("logintypenew", GetUserLoginForm(LoginInfoModel.UserName).ToString());
                contentDic.Add("spaceId", string.Format("{0}::{1}", LoginInfoModel.ClientId, string.IsNullOrEmpty(LoginInfoModel.ProductId) ? LoginInfoModel.ClientId : LoginInfoModel.ProductId));
                string MessageType = "LoginService.userLoginNew";
                DataMessageOperation messageOp = new DataMessageOperation();
                Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                if (returnDic != null)
                {
                    if (!returnDic["ReplyMode"].ToString().Equals("0"))
                    {
                        LoginWindow.DialogResult = true;
                        SaveLoginUserInfo();
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
        /// <summary>
        /// 获取登录方式
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private LoginFormEnum GetUserLoginForm(string userName)
        {
            if (Regex.IsMatch(userName, @"^[1]+[3,4,5,7,8]+\d{9}"))
            {
                return LoginFormEnum.phone;
            }
            if (Regex.IsMatch(userName, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                return LoginFormEnum.email;
            }
            return LoginFormEnum.usercode;
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
            #region 获取ClientNo
            if (LoginInfoModel.ClientId.Equals("feidao"))
            {
                string MessageType = "MongoDataChannelService.findBusiData";
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("systemid", "18");
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
                Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic, "JSON");
                if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
                {
                    string channelId = returnDic["DataChannelId"].ToString();
                    DataSet mastDs = messageOp.GetData(channelId, "[\"pub_product\"]");
                    if (mastDs != null && mastDs.Tables.Contains("dataArray") && mastDs.Tables["dataArray"].Rows.Count > 0)
                    {
                        LoginInfoModel.ClientNo = mastDs.Tables["dataArray"].Rows[0]["client_no"].ToString();
                    }
                } 
            }
            #endregion
            #region 更新通道信息
            string messageType = "LoginService.setUserInfo";
            Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
            setUserContentDic.Add("UserCode", LoginInfoModel.UserName);
            setUserContentDic.Add("UserPwd", LoginInfoModel.UserPwd);
            setUserContentDic.Add("ClientId", LoginInfoModel.ClientId);
            setUserContentDic.Add("ProductId", LoginInfoModel.ProductId);
            setUserContentDic.Add("ClientNo", LoginInfoModel.ClientNo);
            messageOp.SendAsyncMessage(messageType, setUserContentDic);
            #endregion

        }
        #endregion

        #endregion


        //20150407设计新样式
        /// <summary>
        /// 添加应用弹窗是否显示
        /// </summary>
        private bool popupIsShow;
        public bool PopupIsShow
        {
            get
            {
                return popupIsShow;
            }
            set
            {
                if (popupIsShow != value)
                {
                    popupIsShow = value;
                    RaisePropertyChanged("PopupIsShow");
                }
            }
        }

        /// <summary>
        /// 单击“设定”命令
        ///  </summary>
        public ICommand btnAdvancedSetsClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        PopupIsShow = true;
                    });
            }
        }
        
            /// <summary>
        /// 单击“设定”命令
        ///  </summary>
        public ICommand popupBtnCloseClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        PopupIsShow = false;
                    });
            }
        }


        #region  添加电子邮件和电话号码登陆方式
        #region 添加下拉框中的子项
       //初始化邮箱格式
        string[] semial = { "@qq.com", "@139.com", "@163.com", "@126.com", "@sohu.com", "@sina.com", "@gmial.com", "@21cn.com", "@hotmial.com", "@yeah.net", "@ifeidao.com" };

        private bool addlist(string input_txt) //input_txt即为输入框内容
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input_txt, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        #endregion 
        
        #region 判断是不是电话号码
       private bool IsMobilePhone(string input_txt)  //你要校验的电话号码字符串
        {
            if (input_txt.Length == 0)
            {
                return false;
            }
            else if (input_txt.Length != 11)
            {
                return false;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(input_txt, @"^[1]+[3,4,5,7,8]+\d{9}");
        }
        #endregion
        #endregion
    }
}
