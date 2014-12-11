﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.CoreLibrary.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// 登录用户信息
	/// </summary>
	/// <remarks>登录用户信息</remarks>
	public class LoginUserInfo
	{
        /// <summary>
        /// 用户Id
        /// </summary>
        private string userId;
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private string userName;
		/// <summary>
		/// 用户名
		/// </summary>
		public virtual string UserName
		{
            get { return userName; }
            set { userName = value; }
		}
        private string userPwd;
		/// <summary>
		/// 用户密码
		/// </summary>
		public virtual string UserPwd
		{
            get { return userPwd; }
            set { userPwd = value; }
		}
        /// <summary>
        /// 用户完整信息
        /// </summary>
        private List<Dictionary<string, object>> userFullInfo;
        /// <summary>
        /// 用户完整信息
        /// </summary>
        public List<Dictionary<string, object>> UserFullInfo
        {
            get { return userFullInfo; }
            set { userFullInfo = value; }
        }
        /// <summary>
        /// 用户头像
        /// </summary>
        private string userImg;
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImg
        {
            get { return userImg; }
            set { userImg = value; }
        }
        private string linkServerAddress;
		/// <summary>
		/// 连接器服务地址
		/// </summary>
		public virtual string LinkServerAddress
		{
            get { return linkServerAddress; }
            set { linkServerAddress = value; }
		}
        private string linkRouterAddress;
		/// <summary>
		/// 连接器路由地址
		/// </summary>
		public virtual string LinkRouterAddress
		{
            get { return linkRouterAddress; }
            set { linkRouterAddress = value; }
		}
        private string sessionId;
		/// <summary>
		/// 连接器通信标识
		/// </summary>
		public virtual string SessionId
		{
            get { return sessionId; }
            set { sessionId = value; }
		}
        private string userCode;
        /// <summary>
        /// 用户code
        /// </summary>
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }
        private string channelId;
        /// <summary>
        /// 通道标识
        /// </summary>
        public string ChannelId
        {
            get
            {
                return channelId;
            }
            set
            {
                channelId = value;
            }
        }

	}
}

