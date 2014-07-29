using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Victop.Frame.PublicLib.Managers
{
    /// <summary>
    /// 配置文件读取类
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 配置文件XML文档
        /// </summary>
        private static XmlDocument xmlDoc = null;

        /// <summary>
        /// 获取节点下的属性集合
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetNodeAttributes(string nodeName)
        {
            if (xmlDoc == null)
            {
                LoadPartnerConfig();
            }
            Dictionary<string, string> attributeList = new Dictionary<string, string>();
            XmlElement rootElement = xmlDoc.DocumentElement;
            if (rootElement != null && rootElement.ChildNodes.Count > 0)
            {
                foreach (XmlNode item in rootElement.ChildNodes)
                {
                    if (item.Name.Equals(nodeName))
                    {
                        foreach (XmlAttribute attrItem in item.Attributes)
                        {
                            attributeList.Add(attrItem.Name, attrItem.Value);
                        }
                        break;
                    }
                }
            }
            return attributeList;
        }
        /// <summary>
        /// 根据节点名和属性名获取属性的值
        /// </summary>
        /// <param name="NodeName">节点名称</param>
        /// <param name="attributeName">属性名称</param>
        /// <returns></returns>
        public static string GetAttributeOfNodeByName(string nodeName, string attributeName)
        {
            if (xmlDoc == null)
            {
                LoadPartnerConfig();
            }
            string attrValue = string.Empty;
            XmlElement rootElement = xmlDoc.DocumentElement;
            if (rootElement != null && rootElement.ChildNodes.Count > 0)
            {
                foreach (XmlNode item in rootElement.ChildNodes)
                {
                    if (item.Name.Equals(nodeName))
                    {
                        foreach (XmlAttribute attrItem in item.Attributes)
                        {
                            if (attrItem.Name.Equals(attributeName))
                            {
                                attrValue = attrItem.Value;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            return attrValue;
        }
        /// <summary>
        /// 保存节点名下的属性值
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="attrDic">属性键值对</param>
        /// <returns>保存是否成功</returns>
        public static bool SaveAttributeOfNodeByName(string nodeName, Dictionary<string,string> attrDic)
        {
            if (xmlDoc == null)
            {
                LoadPartnerConfig();
            }
            bool SaveFlag = false;
            XmlElement rootElement = xmlDoc.DocumentElement;
            if (rootElement != null && rootElement.ChildNodes.Count > 0)
            {
                foreach (XmlNode item in rootElement.ChildNodes)
                {
                    if (item.Name.Equals(nodeName))
                    {
                        foreach (XmlAttribute attrItem in item.Attributes)
                        {
                            if (attrDic.ContainsKey(attrItem.Name))
                            {
                                attrItem.Value = attrDic[attrItem.Name];
                                SaveFlag = true;
                            }
                        }
                        break;
                    }
                }
            }
            if (SaveFlag)
            {
                SavePartnerConfig();
            }
            return SaveFlag;
        }
        /// <summary>
        ///  加载配置文件
        /// </summary>
        private static void LoadPartnerConfig()
        {
            string configFile = ConfigurationManager.AppSettings.Get("partnerconfig");
            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + configFile;
            if (File.Exists(fileFullPath))
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(fileFullPath);
            }
        }
        private static void SavePartnerConfig()
        {
            string configFile = ConfigurationManager.AppSettings.Get("partnerconfig");
            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + configFile;
            if (!File.Exists(fileFullPath))
            {
                File.Create(fileFullPath);
                
            }
            xmlDoc.Save(fileFullPath);
        }
    }
}
