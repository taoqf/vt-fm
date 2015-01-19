using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using UserLoginPlugin.Models;
using Victop.Frame.PublicLib.Managers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using System.Diagnostics;
using Victop.Frame.DataMessageManager;

namespace UserLoginPlugin.ViewModels
{
    public class SystemConfigViewModel : ModelBase
    {
        #region 字段
        private SystemConfigModel sysConfigModel;
        private Window mainWindow;
        private List<string> cleanUnitList;
        #endregion

        #region 属性
        /// <summary>系统配置模型</summary>
        public SystemConfigModel SysConfigModel
        {
            get
            {
                if (sysConfigModel == null)
                    sysConfigModel = new SystemConfigModel();
                return sysConfigModel;
            }
            set
            {
                if (sysConfigModel != value)
                {
                    sysConfigModel = value;
                    RaisePropertyChanged("SysConfigModel");
                }
            }
        }
        /// <summary>清理单位集合</summary>
        public List<string> CleanUnitList
        {
            get
            {
                if (cleanUnitList == null)
                {
                    cleanUnitList = new List<string>();
                    cleanUnitList.Add("day");
                    cleanUnitList.Add("week");
                    cleanUnitList.Add("month");
                    cleanUnitList.Add("quarter");
                    cleanUnitList.Add("year");
                }
                return cleanUnitList;
            }
            set
            {
                if (cleanUnitList != value)
                {
                    cleanUnitList = value;
                    RaisePropertyChanged("CleanUnitList");
                }
            }
        }
        #endregion

        #region Command命令

        #region 窗体加载命令
        public ICommand gridSysConfigLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    UserControl ucSystemConfig = (UserControl)x;
                    FrameworkElement ct = (FrameworkElement)ucSystemConfig.Parent;
                    while (true)
                    {
                        if (ct is Window)
                        {
                            mainWindow = (Window)ct;
                            break;
                        }
                        ct = (FrameworkElement)ct.Parent;
                    }
                    InitSystemConfigInfo();
                });
            }
        }
        #endregion

        #region 确定命令
        /// <summary>确定命令 </summary>
        public ICommand btnConfirmClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SaveSystemConfigInfo();
                    VisualStateManager.GoToState(mainWindow, "FirstPage", true);
                });
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
                    VisualStateManager.GoToState(mainWindow, "FirstPage", true);
                });
            }
        }
        #endregion

        #endregion

        #region 私有方法

        #region 初始化系统配置信息
        /// <summary>初始化系统配置信息</summary>
        private void InitSystemConfigInfo()
        {
            //系统和日志
            Dictionary<string, string> sysDic = ConfigManager.GetNodeAttributes("System");
            SysConfigModel.AppName = sysDic["AppName"];
            SysConfigModel.Mode = sysDic["Mode"];
            SysConfigModel.ComLink = sysDic["ComLink"];
            SysConfigModel.AutoSearch = sysDic["AutoSearch"];
            SysConfigModel.StartPoint = sysDic["StartPoint"];
            SysConfigModel.EndPoint = sysDic["EndPoint"];
            SysConfigModel.BroadCastTime = sysDic["BroadCastTime"];


            Dictionary<string, string> logDic = ConfigManager.GetNodeAttributes("Log");
            SysConfigModel.Debug = logDic["Debug"];
            SysConfigModel.Clean = logDic["Clean"];
            SysConfigModel.Unit = logDic["Unit"];
            SysConfigModel.Num = logDic["Num"];
            //接入平台
            Dictionary<string, string> masterDic = ConfigManager.GetNodeAttributes("Master");
            string masterServer = masterDic["Server"];
            SysConfigModel.CloudServerIP =string.IsNullOrEmpty(masterServer)?string.Empty: masterServer.Split(':')[0];
            SysConfigModel.CloudServerPort = string.IsNullOrEmpty(masterServer) ? string.Empty : masterServer.Split(':')[1];

            string masterRouter = masterDic["Router"];
            SysConfigModel.CloudRouterIP = string.IsNullOrEmpty(masterRouter) ? string.Empty : masterRouter.Split(':')[0];
            SysConfigModel.CloudRouterPort = string.IsNullOrEmpty(masterRouter) ? string.Empty : masterRouter.Split(':')[1];

            string serverHost = ConfigManager.GetAttributeOfNodeByName("Server", "Host");
            SysConfigModel.CloudHostIP = string.IsNullOrEmpty(serverHost) ? string.Empty : serverHost.Split(':')[0];
            SysConfigModel.CloudHostPort = string.IsNullOrEmpty(serverHost) ? string.Empty : serverHost.Split(':')[1];

            Dictionary<string, string> clientDic = ConfigManager.GetNodeAttributes("Client");
            string clientServer = clientDic["Server"];
            SysConfigModel.EnterpriseServerIP = string.IsNullOrEmpty(clientServer) ? string.Empty : clientServer.Split(':')[0];
            SysConfigModel.EnterpriseServerPort = string.IsNullOrEmpty(clientServer) ? string.Empty : clientServer.Split(':')[1];

            string clientRouter = clientDic["Router"];
            SysConfigModel.EnterpriseRouterIP = string.IsNullOrEmpty(clientRouter) ? string.Empty : clientRouter.Split(':')[0];
            SysConfigModel.EnterpriseRouterPort = string.IsNullOrEmpty(clientRouter) ? string.Empty : clientRouter.Split(':')[1];

            SysConfigModel.EnterpriseLan = clientDic["Lan"];
            SysConfigModel.EnterpriseIsNeedRouter = clientDic["IsNeedRouter"];

            string p2pServer = ConfigManager.GetAttributeOfNodeByName("P2PClient", "Server");
            SysConfigModel.P2PServerIP = string.IsNullOrEmpty(p2pServer) ? string.Empty : p2pServer.Split(':')[0];
            SysConfigModel.P2PServerPort = string.IsNullOrEmpty(p2pServer) ? string.Empty : p2pServer.Split(':')[1];
            //用户信息
            Dictionary<string, string> userDic = ConfigManager.GetNodeAttributes("UserInfo");
            SysConfigModel.UserName = userDic["User"];
            SysConfigModel.PassWord = userDic["Pwd"];
            SysConfigModel.ClientId = userDic["ClientId"];
        }
        #endregion

        #region 保存系统配置信息
        /// <summary>保存系统配置信息</summary>
        private void SaveSystemConfigInfo()
        {
            try
            {
                #region 系统和日志
                Dictionary<string, string> systemDic = new Dictionary<string, string>();
                systemDic.Add("AppName", SysConfigModel.AppName);
                systemDic.Add("Mode", SysConfigModel.Mode);
                systemDic.Add("ComLink", SysConfigModel.ComLink);
                systemDic.Add("AutoSearch", SysConfigModel.AutoSearch);
                systemDic.Add("StartPoint", SysConfigModel.StartPoint);
                systemDic.Add("EndPoint", SysConfigModel.EndPoint);
                systemDic.Add("BroadCastTime", SysConfigModel.BroadCastTime);
                ConfigManager.SaveAttributeOfNodeByName("System", systemDic);
                Dictionary<string, string> logDic = new Dictionary<string, string>();
                logDic.Add("Debug", SysConfigModel.Debug);
                logDic.Add("Clean", SysConfigModel.Clean);
                logDic.Add("Unit", SysConfigModel.Unit);
                logDic.Add("Num", SysConfigModel.Num);
                ConfigManager.SaveAttributeOfNodeByName("Log", logDic);
                #endregion

                #region 接入平台
                Dictionary<string, string> masterDic = new Dictionary<string, string>();
                masterDic.Add("Server", GetIPAddressPort(SysConfigModel.CloudServerIP, SysConfigModel.CloudServerPort));
                masterDic.Add("Router", GetIPAddressPort(SysConfigModel.CloudRouterIP, SysConfigModel.CloudRouterPort));
                ConfigManager.SaveAttributeOfNodeByName("Master", masterDic);

                Dictionary<string, string> serverDic = new Dictionary<string, string>();
                serverDic.Add("Host", GetIPAddressPort(SysConfigModel.CloudHostIP, SysConfigModel.CloudHostPort));
                ConfigManager.SaveAttributeOfNodeByName("Server", serverDic);

                Dictionary<string, string> clientDic = new Dictionary<string, string>();
                clientDic.Add("Server", GetIPAddressPort(SysConfigModel.EnterpriseServerIP, SysConfigModel.EnterpriseServerPort));
                clientDic.Add("Router", GetIPAddressPort(SysConfigModel.EnterpriseRouterIP, SysConfigModel.EnterpriseRouterPort));
                clientDic.Add("Lan", SysConfigModel.EnterpriseLan);
                clientDic.Add("IsNeedRouter", SysConfigModel.EnterpriseIsNeedRouter);
                ConfigManager.SaveAttributeOfNodeByName("Client", clientDic);

                Dictionary<string, string> p2pClientDic = new Dictionary<string, string>();
                p2pClientDic.Add("Server", GetIPAddressPort(SysConfigModel.P2PServerIP, SysConfigModel.P2PServerPort));
                ConfigManager.SaveAttributeOfNodeByName("P2PClient", p2pClientDic);
                #endregion

                #region 用户信息
                Dictionary<string, string> userInfoDic = new Dictionary<string, string>();
                userInfoDic.Add("User", SysConfigModel.UserName);
                userInfoDic.Add("Pwd", SysConfigModel.PassWord);
                userInfoDic.Add("ClientId", SysConfigModel.ClientId);
                ConfigManager.SaveAttributeOfNodeByName("UserInfo", userInfoDic);
                #endregion
                #region 更新通道信息
                SetUserInfo();
                #endregion
            }
            catch (Exception ex)
            {
                VicMessageBoxNormal.Show(ex.ToString());
            }
        }
        private void SetUserInfo()
        {
            string messageType = "LoginService.setUserInfo";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("UserCode", SysConfigModel.UserName);
            contentDic.Add("UserPwd", SysConfigModel.PassWord);
            contentDic.Add("ClientId", SysConfigModel.ClientId);
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendAsyncMessage(messageType, contentDic);
        }
        /// <summary>
        /// 获取ip地址和端口号
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <returns>ip地址和端口号</returns>
        private string GetIPAddressPort(string ip, string port)
        {
            if (string.IsNullOrEmpty(ip)) return string.Empty;
            if (string.IsNullOrEmpty(port)) return ip;
            return string.Format("{0}:{1}",ip,port);
        }
        #endregion

        #endregion

    }
}

