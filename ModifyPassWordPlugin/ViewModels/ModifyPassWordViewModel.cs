using GalaSoft.MvvmLight.Command;
using ModifyPassWordPlugin.Models;
using ModifyPassWordPlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace ModifyPassWordPlugin.ViewModels
{
    public class ModifyPassWordViewModel : ModelBase
    {
        #region 字段
        private UserControl mainView;
        private ModifyPassWordModel pwdModel;
        private PasswordBox pwdOldUser;
        private PasswordBox pwdNewUser;
        private PasswordBox pwdAffirmUser;
        private VicTextBlockNormal txtOld;
        private VicTextBlockNormal txtNew;
        private VicTextBlockNormal txtAffirm;

        #endregion

        #region 属性
        public ModifyPassWordModel PwdModel
        {
            get {
                if (pwdModel == null)
                {
                    pwdModel = new ModifyPassWordModel();
                }
                return pwdModel;
            }
            set { 
                if(pwdModel!=value)
                {
                    PwdModel = value;
                    RaisePropertyChanged("PwdModel");
                }
            }
        }
        #endregion

        #region 命令
        public ICommand MainLoadedCommand
        {
            get {
                return new RelayCommand<object>((x) =>
                {
                    mainView = (UserControl)x;
                    pwdOldUser = (PasswordBox)mainView.FindName("pwdOldUser");
                    pwdNewUser = (PasswordBox)mainView.FindName("pwdNewUser");
                    pwdAffirmUser = (PasswordBox)mainView.FindName("pwdAffirmUser");
                    txtOld = (VicTextBlockNormal)mainView.FindName("txtOld");
                    txtNew = (VicTextBlockNormal)mainView.FindName("txtNew");
                    txtAffirm = (VicTextBlockNormal)mainView.FindName("txtAffirm");
                    pwdOldUser.LostFocus += pwdOldUser_LostFocus;
                    pwdNewUser.LostFocus += pwdNewUser_LostFocus;
                    pwdAffirmUser.LostFocus += pwdAffirmUser_LostFocus;
                   // PwdModel.UserCode = GetUserCode();
                   PwdModel.UserCode = ModifyPassWordWindow.ParamDict["usercode"].ToString();
                });
            }
        }
        /// <summary>
        /// 提交
        /// </summary>
        public ICommand btnSubmitClickCommand
        {
            get {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (PwdModel.UserCode == "" || PwdModel.UserCode.Equals(String.Empty))
                        {
                            VicMessageBoxNormal.Show("请先登录后修改密码");
                            return; 
                        }
                        pwdModel.AffirmIsEnabled = true;
                         string messageType = "MongoDataChannelService.getPwdUpdatePwd";
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", "12");
                    contentDic.Add("id", PwdModel.UserCode);
                    contentDic.Add("password", PwdModel.NewUserPwd);
                    contentDic.Add("captcha", PwdModel.OldUserPwd);
                    contentDic.Add("edit_type", "normal");
                    DataMessageOperation messageOp = new DataMessageOperation();
                    Dictionary<string, object> returnDic = messageOp.SendSyncMessage(messageType, contentDic);
                    if (returnDic != null && returnDic.ContainsKey("ReplyContent"))
                    {
                        Dictionary<string, object> dic = JsonHelper.ToObject<Dictionary<string, object>>(returnDic["ReplyContent"].ToString());
                        if (dic != null)
                        {
                            if (dic["result"].ToString().Equals("False"))
                            {
                                pwdOldUser.BorderBrush = Brushes.Red;
                                PwdModel.OldUserPrompt = dic["msg"].ToString();
                                txtOld.Foreground = Brushes.Red;
                                PwdModel.AffirmIsEnabled = false;
                            }
                            else
                            {
                                PwdModel.AffirmIsEnabled = true;
                                VicMessageBoxNormal.Show(dic["msg"].ToString());
                            }
                        }
                        else
                        {
                            VicMessageBoxNormal.Show("ReplyContent为空");
                            return;
                        }

                    }
                   
                    }
                    catch (Exception ex)
                    {

                        VicMessageBoxNormal.Show(ex.ToString());
                    }
                    }, () => { return CheckUserLogin();
                });
            }
        }
        private void getPwdCallBack(object obj)
        {
            Dictionary<string, object> dict = obj as Dictionary<string, object>;
            int i=0;
        }
        #endregion

        #region 私方法
        /// <summary>检查输入信息</summary>
        private bool CheckUserLogin()
        {
            bool CheckStatus = true;
            if (string.IsNullOrEmpty(PwdModel.OldUserPwd))
            {
                CheckStatus = false;
                return CheckStatus;
            }
            
            if (string.IsNullOrEmpty(PwdModel.NewUserPwd))
            {
                CheckStatus = false;
                return CheckStatus;
            }
            if (string.IsNullOrEmpty(PwdModel.AffirmUserPwd))
            {
                CheckStatus = false;
                return CheckStatus;
            }
            return CheckStatus;
        }

        public static string GetUserCode()
        {
            DataMessageOperation messageOp = new DataMessageOperation();
            Dictionary<string, object> result = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
            return JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserCode");
        }
        void pwdAffirmUser_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordBox pwd = sender as PasswordBox;
            if (pwd.Password.Length == 0 || pwd.Password.Equals(string.Empty))
            {
                pwd.BorderBrush = Brushes.Red;
                PwdModel.AffirmUserPrompt = "不能为空";
                txtAffirm.Foreground = Brushes.Red;
                PwdModel.AffirmIsEnabled = false;
                return;
            }
            else
            {
                pwd.BorderBrush = Brushes.WhiteSmoke;
                PwdModel.AffirmUserPrompt = string.Empty;
                txtAffirm.Foreground = Brushes.WhiteSmoke;
                PwdModel.AffirmIsEnabled = true;
            }
            if (pwd.Password.Length < 6 || pwd.Password.Length > 16)
            {
                pwd.BorderBrush = Brushes.Red;
                PwdModel.AffirmUserPrompt = "6-16个字符,区分大小写111111";
                txtAffirm.Foreground = Brushes.Red;
                PwdModel.AffirmIsEnabled = false;
                return;
            }
            else
            {
                pwd.BorderBrush = Brushes.WhiteSmoke;
                PwdModel.AffirmUserPrompt = string.Empty;
                txtAffirm.Foreground = Brushes.WhiteSmoke;
                PwdModel.AffirmIsEnabled = true;
            }
            if (!pwdModel.NewUserPwd.Equals(pwdModel.AffirmUserPwd))
            {
                PwdModel.AffirmUserPrompt = "两次输入密码不一致，清重新输入";
                txtAffirm.Foreground = Brushes.Red;
                PwdModel.NewUserPwd = string.Empty;
                PwdModel.AffirmUserPwd = string.Empty;
                PwdModel.AffirmIsEnabled = false;
                return;
            }
            else
            {
                pwd.BorderBrush = Brushes.WhiteSmoke;
                PwdModel.AffirmUserPrompt = string.Empty;
                txtAffirm.Foreground = Brushes.WhiteSmoke;
                PwdModel.AffirmIsEnabled = true;
            }
        }

        void pwdNewUser_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordBox pwd = sender as PasswordBox;
            if (pwd.Password.Length == 0 || pwd.Password.Equals(string.Empty))
            {
                pwd.BorderBrush = Brushes.Red;
                PwdModel.NewUserPrompt = "不能为空";
                txtNew.Foreground = Brushes.Red;
                PwdModel.AffirmIsEnabled = false;
                return;
            }
            else
            {
                pwd.BorderBrush = Brushes.WhiteSmoke;
                PwdModel.NewUserPrompt = string.Empty;
                txtNew.Foreground = Brushes.WhiteSmoke;
                PwdModel.AffirmIsEnabled = true;
            }
            if (pwd.Password.Length < 6 || pwd.Password.Length > 16)
            {
                pwd.BorderBrush = Brushes.Red;
                PwdModel.NewUserPrompt = "6-16个字符,可有大小写英文、数字或符号组成";
                txtNew.Foreground = Brushes.Red;
                PwdModel.AffirmIsEnabled = false;
                return;
            }
            else
            {
                pwd.BorderBrush = Brushes.WhiteSmoke;
                PwdModel.NewUserPrompt = string.Empty;
                txtNew.Foreground = Brushes.WhiteSmoke;
                PwdModel.AffirmIsEnabled = true;
            }
        }

        void pwdOldUser_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordBox pwd = sender as PasswordBox;
            if (pwd.Password.Length == 0 || pwd.Password.Equals(string.Empty))
            {
                pwd.BorderBrush = Brushes.Red;
                PwdModel.OldUserPrompt = "不能为空";
                txtOld.Foreground = Brushes.Red;
                PwdModel.AffirmIsEnabled = false;
                return;
            }
            else
            {
                pwd.BorderBrush = Brushes.WhiteSmoke;
                PwdModel.OldUserPrompt = string.Empty;
                txtOld.Foreground = Brushes.WhiteSmoke;
                PwdModel.AffirmIsEnabled = true;
            }
            if (pwd.Password.Length < 6 || pwd.Password.Length > 16)
            {
                pwd.BorderBrush = Brushes.Red;
                PwdModel.OldUserPrompt = "6-16个字符,可有大小写英文、数字或符号组成";
                txtOld.Foreground = Brushes.Red;
                PwdModel.AffirmIsEnabled = false;
                return;
            }
            else
            {
                pwd.BorderBrush = Brushes.WhiteSmoke;
                PwdModel.OldUserPrompt = string.Empty;
                txtOld.Foreground = Brushes.WhiteSmoke;
                PwdModel.AffirmIsEnabled = true;
            }

        }
        #endregion
    }
}
