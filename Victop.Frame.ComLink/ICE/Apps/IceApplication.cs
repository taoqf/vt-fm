﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.ComLink.ICE.Apps
{
    using Ice;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

	/// <summary>
	/// ICE应用程序
	/// </summary>
	/// <remarks>ICE应用程序</remarks>
    public class IceApplication : Ice.Application
	{
        private int iceStatus;
		/// <summary>
		/// ICE状态
		/// </summary>
		public int ICEStatus
		{
            get
            {
                return iceStatus;
            }
            set
            {
                iceStatus = value;
            }
		}
        /// <summary>
        /// 通信器
        /// </summary>
        public Communicator Communicator
        {
            get { return communicator(); }
        }
		/// <summary>
		/// 停止
		/// </summary>
		public void Stop()
		{
            Communicator.shutdown();
            iceStatus = 0;
		}

		/// <summary>
		/// 启动
		/// </summary>
        public override int run(string[] args)
        {
            try
            {
                iceStatus = 1;
                Communicator.waitForShutdown();
            }
            catch (System.Exception)
            {

                iceStatus = 0;
            }
            return iceStatus;
        }
    }
}

