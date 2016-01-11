using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Server.Controls.Models
{
    /// <summary>
    /// 对象属性事实实体类
    /// </summary>
    public class OAVModel
    {
        /// <summary>
        /// 对象事实构造函数
        /// </summary>
        /// <param name="objectName">对象名称</param>
        /// <param name="atrributeName">属性名称</param>
        public OAVModel(string objectName, string atrributeName)
        {
            this.objectName = objectName;
            this.atrributeName = atrributeName;
        }
        /// <summary>
        /// 对象事实构造函数
        /// </summary>
        /// <param name="objectName">对象名称</param>
        /// <param name="atrributeName">属性名称</param>
        /// <param name="atrributeValue">属性值</param>
        public OAVModel(string objectName, string atrributeName, object atrributeValue)
        {
            this.objectName = objectName;
            this.atrributeName = atrributeName;
            this.AtrributeValue = atrributeValue;
        }
        private string objectName;
        /// <summary>
        /// 对象名称
        /// </summary>
        public string ObjectName
        {
            get
            {
                return objectName;
            }
        }
        private string atrributeName;
        /// <summary>
        /// 属性名
        /// </summary>
        public string AtrributeName
        {
            get
            {
                return atrributeName;
            }
        }
        /// <summary>
        /// 属性值
        /// </summary>
        public object AtrributeValue
        {
            get; set;
        }
        /// <summary>
        /// 属性值整型
        /// </summary>
        public int AtrributeValueInt
        {
            get
            {
                int i = 0;
                if (AtrributeValue != null)
                {
                    int.TryParse(AtrributeValue.ToString(), out i);
                }
                return i;
            }
        }
        /// <summary>
        /// 属性值长整型
        /// </summary>
        public long AtrributeValueLong
        {
            get
            {
                long value = 0;
                if (AtrributeValue != null)
                {
                    long.TryParse(AtrributeValue.ToString(), out value);
                }
                return value;
            }
        }
        /// <summary>
        /// 属性值浮点型
        /// </summary>
        public decimal AtrributeValueDecimal
        {
            get
            {
                decimal value = 0;
                if (AtrributeValue != null)
                {
                    decimal.TryParse(AtrributeValue.ToString(), out value);
                }
                return value;
            }
        }
        /// <summary>
        /// 属性值布尔型
        /// </summary>
        public bool AtrributeValueBoolean
        {
            get
            {
                bool value = false;
                if (AtrributeValue != null)
                {
                    bool.TryParse(AtrributeValue.ToString(), out value);
                }
                return value;
            }
        }
    }
}
