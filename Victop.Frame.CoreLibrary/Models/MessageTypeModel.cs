using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.CoreLibrary.Models
{
    /// <summary>
    /// 消息类型实体
    /// </summary>
    public class MessageTypeModel
    {
        /// <summary>
        /// 检索消息类型
        /// </summary>
        private List<string> searchMessageType;
        /// <summary>
        /// 保存消息类型
        /// </summary>
        public List<string> SearchMessageType
        {
            get
            {
                if (searchMessageType == null)
                    searchMessageType = new List<string>();
                return searchMessageType;
            }
            set { searchMessageType = value; }
        }
        /// <summary>
        /// 保存消息类型
        /// </summary>
        private List<string> saveMessageType;
        /// <summary>
        /// 保存消息类型
        /// </summary>
        public List<string> SaveMessageType
        {
            get
            {
                if (saveMessageType == null)
                    saveMessageType = new List<string>();
                return saveMessageType;
            }
            set { saveMessageType = value; }
        }
        /// <summary>
        /// 无操作消息类型
        /// </summary>
        private List<string> noneMessageType;
        /// <summary>
        /// 无操作消息类型
        /// </summary>
        public List<string> NoneMessageType
        {
            get
            {
                if (noneMessageType == null)
                    noneMessageType = new List<string>();
                return noneMessageType;
            }
            set { noneMessageType = value; }
        }
    }
}
