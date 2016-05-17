using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
        /// <param name="nodeName">节点名称</param>
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
        public static bool SaveAttributeOfNodeByName(string nodeName, Dictionary<string, string> attrDic)
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
                                if (!attrItem.Value.Equals(attrDic[attrItem.Name]))
                                {
                                    attrItem.Value = attrDic[attrItem.Name];
                                    SaveFlag = true;
                                }
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
        /// 获取本地httpServer基础url
        /// <param name="initHttpServer">是否为初始化httpServer</param>
        /// </summary>
        /// <returns>"http://localhost:端口/"</returns>
        public static string GetLocalHttpServerBaseUrl(bool initHttpServer = false)
        {
            string urlPort = GetAttributeOfNodeByName("System", "StartPoint");
            int serverPort = Convert.ToInt32(urlPort);
            if (initHttpServer)
            {
                PortCheck(ref serverPort);
                Dictionary<string, string> attrDic = new Dictionary<string, string>();
                attrDic.Add("StartPoint", serverPort.ToString());
                SaveAttributeOfNodeByName("System", attrDic);
            }
            return string.Format("http://localhost:{0}/", serverPort);
        }
        private static bool PortCheck(ref int defaultPort)
        {
            var bakPort = defaultPort;
            bool flag = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();//获取所有的监听连接
            IPEndPoint point = ipEndPoints.ToList().Find(it => it.Port.Equals(bakPort));
            if (point != null)
            {
                bakPort++;
                flag = PortCheck(ref bakPort);
            }
            defaultPort = bakPort;
            return flag;
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
