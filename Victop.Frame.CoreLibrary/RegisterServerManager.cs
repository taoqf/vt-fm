﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.CoreLibrary
{
    using Victop.Frame.CoreLibrary.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Victop.Frame.CoreLibrary.Enums;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using Victop.Server.Controls;

    /// <summary>
    /// 注册服务管理
    /// </summary>
    /// <remarks>注册服务管理</remarks>
    public class RegisterServerManager
    {
        /// <summary>
        /// 注册服务
        /// <param name="serverInfo">服务信息</param>
        /// </summary>
        public virtual bool RegisterServer(RegisterServerInfo serverInfo)
        {
            if (serverInfo == null)
                return false;
            if (CoreDataCollection.RegisterServerList.ContainsKey(serverInfo.CloudGalleryId))
            {
                if (CoreDataCollection.RegisterServerList[serverInfo.CloudGalleryId].ContainsKey(serverInfo.ServerName))
                {
                    CoreDataCollection.RegisterServerList[serverInfo.CloudGalleryId][serverInfo.ServerName] = serverInfo;
                }
                else
                {
                    CoreDataCollection.RegisterServerList[serverInfo.CloudGalleryId].Add(serverInfo.ServerName, serverInfo);
                }
            }
            else
            {
                Dictionary<string, RegisterServerInfo> galleryDictionary = new Dictionary<string, RegisterServerInfo>();
                galleryDictionary.Add(serverInfo.ServerName, serverInfo);
                CoreDataCollection.RegisterServerList.Add(serverInfo.CloudGalleryId, galleryDictionary);
            }
            return true;
        }

        /// <summary>
        /// 获取注册服务(根据云id和消息类型)
        /// </summary>
        public virtual RegisterServerInfo GetServer(string cloudGalleryId, string messageType)
        {
            RegisterServerInfo registerServerInfo = null;
            if (CoreDataCollection.RegisterServerList.ContainsKey(cloudGalleryId))
            {
                Dictionary<string, RegisterServerInfo> galleryServerList = CoreDataCollection.RegisterServerList[cloudGalleryId];
                foreach (string item in galleryServerList.Keys)
                {
                    if (galleryServerList[item].ReceiptMessageType.Contains(messageType))
                    {
                        registerServerInfo = galleryServerList[item];
                        break;
                    }
                }
            }
            return registerServerInfo;
        }

        /// <summary>
        /// 注销服务
        /// <param name="serverInfo">服务通道标识及服务名称</param>
        /// </summary>
        public virtual bool UnRegisterServer(RegisterServerInfo serverInfo)
        {
            if (CoreDataCollection.RegisterServerList.ContainsKey(serverInfo.CloudGalleryId))
            {
                if (CoreDataCollection.RegisterServerList[serverInfo.CloudGalleryId].ContainsKey(serverInfo.ServerName))
                {
                    CoreDataCollection.RegisterServerList[serverInfo.CloudGalleryId].Remove(serverInfo.ServerName);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 初始化内置服务
        /// </summary>
        public void InitServerList()
        {
            RegisterServerManager serverManager = new RegisterServerManager();
            foreach (GalleryEnum item in Enum.GetValues(typeof(GalleryEnum)))
            {
                #region 内置登录服务
                RegisterServerInfo serverInfo = new RegisterServerInfo();
                serverInfo.CloudGalleryId = item.ToString();
                serverInfo.ServerName = "UserLogin";
                serverInfo.ReceiptMessageType.Add(ConfigurationManager.AppSettings.Get(serverInfo.ServerName.ToLower()));
                serverInfo.ServerType = ServerTypeEnum.SERVER;
                serverInfo.ServerPath = string.Empty;
                serverInfo.ServerStatus = ResourceEnum.NONE;
                serverManager.RegisterServer(serverInfo);
                #endregion
                #region 内置连接器注册服务
                RegisterServerInfo registerLink = new RegisterServerInfo();
                registerLink.CloudGalleryId = item.ToString();
                registerLink.ServerName = "RegisterLink";
                registerLink.ReceiptMessageType.Add(ConfigurationManager.AppSettings.Get(registerLink.ServerName.ToLower()));
                registerLink.ServerType = ServerTypeEnum.SERVER;
                registerLink.ServerPath = string.Empty;
                registerLink.ServerStatus = ResourceEnum.NONE;
                serverManager.RegisterServer(registerLink);
                #endregion
                #region 内置插件启动服务
                RegisterServerInfo pluginRun = new RegisterServerInfo();
                pluginRun.CloudGalleryId = item.ToString();
                pluginRun.ServerName = "PluginRun";
                pluginRun.ReceiptMessageType.Add(ConfigurationManager.AppSettings.Get(pluginRun.ServerName.ToLower()));
                pluginRun.ServerType = ServerTypeEnum.LOCAL;
                pluginRun.ServerPath = string.Empty;
                pluginRun.ServerStatus = ResourceEnum.NONE;
                serverManager.RegisterServer(pluginRun);
                #endregion
                #region 内置插件关闭服务
                RegisterServerInfo pluginStop = new RegisterServerInfo();
                pluginStop.CloudGalleryId = item.ToString();
                pluginStop.ServerName = "PluginStop";
                pluginStop.ReceiptMessageType.Add(ConfigurationManager.AppSettings.Get(pluginStop.ServerName.ToLower()));
                pluginStop.ServerType = ServerTypeEnum.LOCAL;
                pluginStop.ServerPath = string.Empty;
                pluginStop.ServerStatus = ResourceEnum.NONE;
                serverManager.RegisterServer(pluginStop);
                #endregion
                #region 获取所有通道信息服务
                RegisterServerInfo getGalleryInfo = new RegisterServerInfo();
                getGalleryInfo.CloudGalleryId = item.ToString();
                getGalleryInfo.ServerName = "GetGalleryInfo";
                getGalleryInfo.ReceiptMessageType.Add(ConfigurationManager.AppSettings.Get(getGalleryInfo.ServerName.ToLower()));
                getGalleryInfo.ServerType = ServerTypeEnum.LOCAL;
                getGalleryInfo.ServerPath = string.Empty;
                getGalleryInfo.ServerStatus = ResourceEnum.NONE;
                serverManager.RegisterServer(getGalleryInfo);
                #endregion
                #region 设置通道信息服务
                RegisterServerInfo setGalleryInfo = new RegisterServerInfo();
                setGalleryInfo.CloudGalleryId = item.ToString();
                setGalleryInfo.ServerName = "SetGalleryInfo";
                setGalleryInfo.ReceiptMessageType.Add(ConfigurationManager.AppSettings.Get(setGalleryInfo.ServerName.ToLower()));
                setGalleryInfo.ServerType = ServerTypeEnum.LOCAL;
                setGalleryInfo.ServerPath = string.Empty;
                setGalleryInfo.ServerStatus = ResourceEnum.NONE;
                serverManager.RegisterServer(setGalleryInfo);
                #endregion
                #region 设置登录默认信息
                RegisterServerInfo setUserInfo = new RegisterServerInfo();
                setUserInfo.CloudGalleryId = item.ToString();
                setUserInfo.ServerName = "SetUserInfo";
                setUserInfo.ReceiptMessageType.Add(ConfigurationManager.AppSettings.Get(setUserInfo.ServerName.ToLower()));
                setUserInfo.ServerType = ServerTypeEnum.LOCAL;
                setUserInfo.ServerPath = string.Empty;
                setUserInfo.ServerStatus = ResourceEnum.NONE;
                serverManager.RegisterServer(setUserInfo);
                #endregion
                RegisterLocalServer(serverManager, item.ToString());
            }
        }
        /// <summary>
        /// 注册本地服务
        /// </summary>
        /// <param name="serverManager"></param>
        /// <param name="galleryId"></param>
        private void RegisterLocalServer(RegisterServerManager serverManager,string galleryId)
        {
            string FolderPath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["pluginpath"];
            DirectoryInfo folderInfo = new DirectoryInfo(FolderPath);
            FileInfo[] files = folderInfo.GetFiles("*Service.dll");
            if (files != null && files.Count() > 0)
            {
                foreach (FileInfo file in files)
                {
                    Assembly assembly = ServerFactory.GetServerAssemblyByName(file.Name.Substring(0, file.Name.IndexOf(".")), "");
                    Type[] types = assembly.GetTypes();
                    foreach (Type t in types)
                    {
                        if (IsValidServer(t))
                        {
                            IService service = (IService)assembly.CreateInstance(t.FullName);
                            RegisterServerInfo serviceInfo = new RegisterServerInfo();
                            serviceInfo.CloudGalleryId = galleryId;
                            serviceInfo.ServerName = service.ServiceName;
                            serviceInfo.ReceiptMessageType = service.ServiceReceiptMessageType;
                            serviceInfo.ServerType = ServerTypeEnum.LOCAL;
                            serviceInfo.ServerPath = string.Empty;
                            serviceInfo.ServerStatus = ResourceEnum.NONE;
                            serverManager.RegisterServer(serviceInfo);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 是否为有效的服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsValidServer(Type type)
        {
            bool ret = false;
            Type[] interfaces = type.GetInterfaces();
            foreach (Type theInterface in interfaces)
            {
                if (theInterface.FullName == "Victop.Server.Controls.IService")
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取所有注册服务信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, RegisterServerInfo>> GetAllRegisterServerInfo()
        {
            return CoreDataCollection.RegisterServerList;
        }
    }
}

