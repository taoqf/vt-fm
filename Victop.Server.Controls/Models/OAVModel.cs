﻿using System;
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
        public OAVModel()
        {

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
        public object AtrributeValue { get; set; }
    }
}
