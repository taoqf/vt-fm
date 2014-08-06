﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.ServerManagerCenter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.PublicLib.Helpers;
    using Victop.Server.Controls;
    using Victop.Frame.CoreLibrary;
    using System.Windows.Controls;
    using System.Threading;
    using Victop.Frame.CoreLibrary.Enums;
	/// <summary>
	/// 服务启动器
	/// </summary>
	/// <remarks>服务启动器</remarks>
	public class ServerInitiator
	{
        private static Dictionary<string, ServerInfoEntity> serverInfoList;
		/// <summary>
		/// 启动服务信息集合
		/// </summary>
        public static Dictionary<string, ServerInfoEntity> ServerInfoList
		{
            get
            {
                if (serverInfoList == null)
                    serverInfoList = new Dictionary<string, ServerInfoEntity>();
                return serverInfoList;
            }
		}
        private static ServerInitiator instance = null;
        /// <summary>
        /// 服务实例
        /// </summary>
        /// <returns></returns>
        public static ServerInitiator GetInstance()
        {
            if (instance == null)
                instance = new ServerInitiator();
            return instance;
        }
        /// <summary>
        /// 服务执行
        /// </summary>
        /// <param name="messageInfo">执行参数</param>
        public string Run(RequestMessage messageInfo)
        {
            string ReplyContent = string.Empty;
            GC.Collect();
            switch (messageInfo.MessageType)
            {
                case "PluginService.PluginRun":
                    ReplyContent = PluginRun(messageInfo);
                    break;
                case "PluginService.PluginStop":
                    ReplyContent = PluginStop(messageInfo);
                    break;
                case "GalleryService.SetGalleryInfo":
                    ReplyContent = SetCurrentGallery(messageInfo);
                    break;
                case "GalleryService.GetGalleryInfo":
                    ReplyContent = GetGalleryInfo(messageInfo);
                    break;
                default:
                    RegisterServerInfo serverInfo = JsonHelper.ToObject<RegisterServerInfo>(JsonHelper.ReadJsonString(messageInfo.MessageContent, "ServerInfo"));
                    ReplyContent = ServerRun(serverInfo,messageInfo);
                    break;
            }
            return ReplyContent;
        }
        private string GetGalleryInfo(RequestMessage messageInfo)
        {
            Dictionary<string, CloudGalleryInfo> galleryList = new Dictionary<string, CloudGalleryInfo>();
            galleryList = new GalleryManager().GetAllGalleryInfo();
            Dictionary<string, object> returnDic = new Dictionary<string, object>();
            Dictionary<string, object> returnList = new Dictionary<string, object>();
            foreach (CloudGalleryInfo item in galleryList.Values)
            {
                returnList.Add(item.CloudGalleryId.ToString(), item.CloudGalleryId.ToString());
            }
            returnDic.Add("ReplyMode", "1");
            returnDic.Add("ReplyContent", returnList);
            return JsonHelper.ToJson(returnDic);
        }
        private string SetCurrentGallery(RequestMessage messageInfo)
        {
            Dictionary<string, string> returnDic = new Dictionary<string, string>();
            Dictionary<string, string> contDic = JsonHelper.ToObject<Dictionary<string, string>>(messageInfo.MessageContent);
            if (contDic.ContainsKey("GalleryKey"))
            {
                GalleryManager galleryManager = new GalleryManager();
                if (Enum.GetNames(typeof(GalleryEnum)).Contains(contDic["GalleryKey"]))
                {
                    string GalleryKey = contDic["GalleryKey"];
                    if (GalleryKey == GalleryEnum.ENTERPRISE.ToString())
                    {
                        galleryManager.SetCurrentGalleryId(GalleryEnum.ENTERPRISE);
                    }
                    if (GalleryKey == GalleryEnum.LOCALSOA.ToString())
                    {
                        galleryManager.SetCurrentGalleryId(GalleryEnum.LOCALSOA);
                    }
                    if (GalleryKey == GalleryEnum.VICTOP.ToString())
                    {
                        galleryManager.SetCurrentGalleryId(GalleryEnum.VICTOP);
                    }
                    returnDic.Add("ReplyMode", "1");
                    returnDic.Add("ReplyContent", "更改当前通道成功");
                    return JsonHelper.ToJson(returnDic);
        }
                else
                {
                    returnDic = new Dictionary<string, string>();
                    returnDic.Add("ReplyMode", "0");
                    returnDic.Add("ReplyContent", "不存需要调整通道信息");
                    return JsonHelper.ToJson(returnDic);
                }
            }
            returnDic = new Dictionary<string, string>();
            returnDic.Add("ReplyMode", "0");
            returnDic.Add("ReplyContent", "请传入通道信息");
            return JsonHelper.ToJson(returnDic);
        }
		/// <summary>
		/// 服务启动
		/// </summary>
        public string ServerRun(RegisterServerInfo serverInfo, RequestMessage messageInfo)
		{
            //通过注册服务信息，得到服务的路径，并通过反射方式将服务实例化
            //执行运行方法
            Dictionary<string, string> returnDic = new Dictionary<string, string>();
            string replyContent = string.Empty;
            Assembly serviceAssembly = ServerFactory.GetServerAssemblyByName(serverInfo.ServerName, serverInfo.ServerPath);
            Type[] types = serviceAssembly.GetTypes();
            foreach (Type t in types)
            {
                if (IsValidServer(t)) //判断是否有继承IService
                {
                    IService service = (IService)serviceAssembly.CreateInstance(t.FullName);
                    service.ServiceParams = JsonHelper.ReadJsonString(messageInfo.MessageContent, "ServiceParams");
                    service.CurrentMessageType = messageInfo.MessageType;
                    if (service.ServiceRun())
                    {
                        if (string.IsNullOrEmpty(service.ReplyContent))
                        {
                            returnDic.Add("ReplyMode", "1");
                            returnDic.Add("ReplyContent", "服务启动成功");
                            replyContent = JsonHelper.ToJson(returnDic);
                        }
                        else
                        {
                            replyContent = service.ReplyContent;
                        }
                    }
                    else
                    {
                        returnDic.Add("ReplyMode", "0");
                        returnDic.Add("ReplyContent", "服务启动失败");
                        replyContent = JsonHelper.ToJson(returnDic);
                    }
                }
            }
            return replyContent;
		}

		/// <summary>
		/// 插件启动
		/// </summary>
		private string PluginRun(RequestMessage messageInfo)
		{
            try
            {
                string pluginName = JsonHelper.ReadJsonString(messageInfo.MessageContent, "PluginName");
                string pluginPath = JsonHelper.ReadJsonString(messageInfo.MessageContent, "PluginPath");
                string pluginParam = JsonHelper.ReadJsonString(messageInfo.MessageContent, "PluginParam");
                Dictionary<string, object> paramDict = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(pluginParam))
                {
                    paramDict = JsonHelper.ToObject<Dictionary<string, object>>(pluginParam);
                }
                Assembly pluginAssembly = ServerFactory.GetServerAssemblyByName(pluginName, pluginPath);
                Type[] types = pluginAssembly.GetTypes();
                bool validPlugin = false;
                foreach (Type t in types)
                {
                    if (IsValidPlugin(t)) //判断是否有继承IPlugin
                    {
                        IPlugin plugin = (IPlugin)pluginAssembly.CreateInstance(t.FullName);
                        plugin.ParamDict = paramDict;
                        ActivePluginInfo activePluginInfo = new ActivePluginInfo();
                        activePluginInfo.ShowType = plugin.ShowType;
                        activePluginInfo.ObjectId = messageInfo.MessageId;
                        activePluginInfo.CloudGalleryId = GalleryManager.GetCurrentGalleryId();
                        activePluginInfo.AppId = pluginName;
                        activePluginInfo.PluginInstance = plugin;
                        ActivePluginManager activePluginManager = new ActivePluginManager();
                        activePluginManager.AddPlugin(activePluginInfo);
                        validPlugin = true;
                    }
                }
                Dictionary<string, string> returnDic = null;
                if (validPlugin)
                {
                    returnDic = new Dictionary<string, string>();
                    returnDic.Add("ReplyMode", "1");
                    returnDic.Add("ReplyContent", "启动成功");
                }
                else
                {
                    returnDic = new Dictionary<string, string>();
                    returnDic.Add("ReplyMode", "0");
                    returnDic.Add("ReplyContent", "无效插件");
                }
                return JsonHelper.ToJson(returnDic);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> returnDic = new Dictionary<string, string>();
                returnDic.Add("ReplyMode", "0");
                returnDic.Add("ReplyContent", ex.Message);
                return JsonHelper.ToJson(returnDic);
            }

		}

		/// <summary>
		/// 发送服务消息
		/// </summary>
		public virtual ReplyMessage SendServerMessage(RequestMessage messageInfo)
		{
			throw new System.NotImplementedException(); //TODO:方法实现
		}

		/// <summary>
		/// 插件关闭
		/// </summary>
		private string PluginStop(RequestMessage messageInfo)
		{
            try
            {
                string replyContent = string.Empty;
                string pluginObjectId = JsonHelper.ReadJsonString(messageInfo.MessageContent, "ObjectId");
                if (!string.IsNullOrEmpty(pluginObjectId))
                {
                    ActivePluginManager pluginManager = new ActivePluginManager();
                    pluginManager.RemovePlugin(new ActivePluginInfo() { ObjectId = pluginObjectId });
                    replyContent = "插件移除成功";
                }
                return replyContent;
            }
            catch (Exception ex)
            {
                throw;
            }
		}

		/// <summary>
		/// 服务运行完成(回调方法)
		/// </summary>
		private void ServerComplate(string replyMessage)
		{
			throw new System.NotImplementedException(); //TODO:方法实现
		}
        /// <summary>
        /// 更新活动插件列表
        /// </summary>
        /// <param name="messageInfo"></param>
		private void UpdateActivePluginList(RequestMessage messageInfo)
		{
			throw new System.NotImplementedException(); //TODO:方法实现
		}
        /// <summary>
        /// 是否为有效的插件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsValidPlugin(Type type)
        {
            bool ret = false;
            Type[] interfaces = type.GetInterfaces();
            foreach (Type theInterface in interfaces)
            {
                if (theInterface.FullName == "Victop.Server.Controls.IPlugin")
                {
                    ret = true;
                    break;
                }
            }
            return ret;
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
	}
}

