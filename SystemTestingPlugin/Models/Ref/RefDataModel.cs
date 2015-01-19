using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SystemTestingPlugin.Models
{
    public class RefDataModel
    {
        /// <summary>
        /// 系统库标识
        /// </summary>
        private string systemId;
        /// <summary>
        /// 系统库标识
        /// </summary>
        public string SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }
        /// <summary>
        /// 配置系统库标识
        /// </summary>
        private string configSystemId;
        /// <summary>
        /// 配置系统库标识
        /// </summary>
        public string ConfigSystemId
        {
            get { return configSystemId; }
            set { configSystemId = value; }
        }
        /// <summary>
        /// 引用数据集
        /// </summary>
        private DataSet refDataSet;
        /// <summary>
        /// 引用数据集
        /// </summary>
        public DataSet RefDataSet
        {
            get { return refDataSet; }
            set { refDataSet = value; }
        }
        /// <summary>
        /// 引用内容
        /// </summary>
        private string refContent;
        /// <summary>
        /// 引用内容
        /// </summary>
        public string RefContent
        {
            get { return refContent; }
            set { refContent = value; }
        }
        private string refFieldCaption;
        /// <summary>
        /// 描述
        /// </summary>
        public string RefFieldCaption
        {
            get { return refFieldCaption; }
            set { refFieldCaption = value; }
        }
        private string viewId;
        /// <summary>
        /// 通道标识
        /// </summary>
        public string ViewId
        {
            get { return viewId; }
            set { viewId = value; }
        }
        private string dataPath;
        /// <summary>
        /// 路径
        /// </summary>
        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }
        private string fieldName;

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        private string rowValue;

        public string RowValue
        {
            get { return rowValue; }
            set { rowValue = value; }
        }
    }
}
