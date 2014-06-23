using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.CoreLibrary.Models
{
    public class IPInfo
    {
        private string senderID;
        /// <summary>
        /// 发送者标识
        /// </summary>
        public string SenderID
        {
            get { return senderID; }
            set { senderID = value; }
        }
        private string address;
        /// <summary>
        ///地址
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private int port;
        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
