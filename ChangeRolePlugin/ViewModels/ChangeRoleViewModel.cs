using ChangeRolePlugin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Controls;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.DataMessageManager;
using System.Windows;
using Victop.Wpf.Controls;
using System.Configuration;
using Victop.Frame.PublicLib.Managers;
namespace ChangeRolePlugin.ViewModels
{
   public class ChangeRoleViewModel:ModelBase
   {
       #region 字段
       private UserControl mainWindow;
       private ObservableCollection<ChangeRoleModel> roleInfoList;
       DataMessageOperation messageOp = new DataMessageOperation();
       private Window roleWindow;
       #endregion

       #region 属性
       /// <summary>
       /// 角色信息集合
       /// </summary>
       public ObservableCollection<ChangeRoleModel> RoleInfoList
       {
           get
           {
               if (roleInfoList == null)
                   roleInfoList = new ObservableCollection<ChangeRoleModel>();
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
       private ChangeRoleModel selectedRoleInfo;
       public ChangeRoleModel SelectedRoleInfo
       {
           get
           {
               if(selectedRoleInfo==null)
               {
               selectedRoleInfo=new ChangeRoleModel();
               }
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

       private string userName;
       public string UserName
       {
           get
           {
               return userName;
           }
           set
           {
               if (userName != value)
               {
                   userName = value;
                   RaisePropertyChanged("UserName");
               }
           }
       }
       private string userPwd;
       public string UserPwd
       {
           get
           {
               return userPwd;
           }
           set
           {
               if (userPwd != value)
               {
                   userPwd = value;
                   RaisePropertyChanged("UserPwd");
               }
           }
       }
       private string clientId;
       public string ClientId
       {
           get
           {
               return clientId;
           }
           set
           {
               if (clientId != value)
               {
                   clientId = value;
                   RaisePropertyChanged("ClientId");
               }
           }
       }
       #endregion

       #region 命令
       /// <summary>
       /// 初始化命令
       /// </summary>
       public ICommand mainLoadedCommand
       {
           get {
               return new RelayCommand<object>((x) =>
               {
                   mainWindow = (UserControl)x;
                   FrameworkElement ct = (FrameworkElement)mainWindow.Parent;
                   while (true)
                   {
                       if (ct is Window)
                       {
                           roleWindow = (Window)ct;
                           break;
                       }
                       ct = (FrameworkElement)ct.Parent;
                   }
                   string name = ConfigManager.GetAttributeOfNodeByName("UserInfo", "User");
                   UserName = ConfigManager.GetAttributeOfNodeByName("UserInfo", "User");
                   UserPwd = ConfigManager.GetAttributeOfNodeByName("UserInfo", "Pwd");
                   ClientId = ConfigManager.GetAttributeOfNodeByName("UserInfo", "ClientId");
                   Dictionary<string, object> result = messageOp.SendSyncMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                   RoleInfoList = JsonHelper.ToObject<ObservableCollection<ChangeRoleModel>>(JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserRole"));
                   int i = 0;
               });
           }
       }
       #endregion
       /// <summary>
       /// 确认
       /// </summary>
       public ICommand btnConfirmClickCommand
       {
           get {
               return new RelayCommand(() =>
               {
                   Dictionary<string, object> menuDic = GetCurrentRoleMenu();
                   if (string.IsNullOrEmpty(SelectedRoleInfo.Role_No))
                   {
                       VicMessageBoxNormal.Show("请选择角色");
                       return;
                   }
                   if (menuDic != null && !menuDic["ReplyMode"].ToString().Equals("0"))
                   {
                       DataMessageOperation dataOp = new DataMessageOperation();
                       string messageType = "LoginService.setUserInfo";
                       Dictionary<string, object> setUserContentDic = new Dictionary<string, object>();
                       setUserContentDic.Add("UserCode", UserName);
                       setUserContentDic.Add("UserPwd", UserPwd);
                       setUserContentDic.Add("ClientId",ClientId);
                       setUserContentDic.Add("UserRole", SelectedRoleInfo.Role_No);
                       dataOp.SendSyncMessage(messageType, setUserContentDic);
                       
                   }
                   else
                   {
                       VicMessageBoxNormal.Show(menuDic["ReplyAlertMessage"].ToString());
                   }
                   roleWindow.DialogResult = true;
                   roleWindow.Close();
               });
           }
       }
       /// <summary>
       /// 取消
       /// </summary>
       public ICommand btnCancelClickCommand
       {
           get {
               return new RelayCommand(() =>
               {
                   roleWindow.Close();
               });
           }
       }
       #region 私方法
       /// <summary>
       /// 获取当前角色菜单
       /// </summary>
       /// <param name="roleNo"></param>
       /// <returns></returns>
       private Dictionary<string, object> GetCurrentRoleMenu(string roleNo = null)
       {
           DataMessageOperation dataOp = new DataMessageOperation();
           string messageType = "MongoDataChannelService.getuserauthInfo";
           Dictionary<string, object> menuDic = new Dictionary<string, object>();
           menuDic.Add("systemid", ConfigurationManager.AppSettings["clientsystem"]);
           menuDic.Add("productid", ClientId);
           menuDic.Add("usercode", UserName);
           menuDic.Add("role_no", string.IsNullOrEmpty(roleNo) ? SelectedRoleInfo.Role_No : roleNo);
           menuDic.Add("client_type", "3");
           Dictionary<string, object> resultDic = dataOp.SendSyncMessage(messageType, menuDic);
           return resultDic;
       }
       #endregion
   }
}
