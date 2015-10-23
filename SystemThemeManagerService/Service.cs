using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Xml;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.PublicLib.Managers;
using Victop.Server.Controls;

namespace SystemThemeManagerService
{
    /// <summary>
    /// 系统主题管理服务
    /// </summary>
    public class Service : IService
    {
        #region 属性
        /// <summary>
        /// 是否自动运行
        /// </summary>
        public int AutoInit
        {
            get { return 0; }
        }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get { return "SystemThemeManagerService"; }
        }
        private List<string> serviceReceiptMessageType;
        /// <summary>
        /// 接收消息类型
        /// </summary>
        public List<string> ServiceReceiptMessageType
        {
            get
            {
                if (serviceReceiptMessageType == null)
                {
                    serviceReceiptMessageType = new List<string>();
                    serviceReceiptMessageType.Add("ServerCenterService.ChangeTheme");
                    serviceReceiptMessageType.Add("ServerCenterService.ChangeLanguage");
                    serviceReceiptMessageType.Add("ServerCenterService.ChangeThemeByDll");
                }
                return serviceReceiptMessageType;
            }
        }
        /// <summary>
        /// 服务描述
        /// </summary>
        public string ServiceDescription
        {
            get { return "皮肤资源服务"; }
        }
        private string serviceParams;
        /// <summary>
        /// 服务参数
        /// </summary>
        public string ServiceParams
        {
            get
            {
                return serviceParams;
            }
            set
            {
                serviceParams = value;
            }
        }

        private string currentMessageType;
        /// <summary>
        /// 当前消息类型
        /// </summary>
        public string CurrentMessageType
        {
            get
            {
                return currentMessageType;
            }
            set
            {
                currentMessageType = value;
            }
        }
        #endregion
        /// <summary>
        /// 服务运行
        /// </summary>
        /// <returns></returns>
        public bool ServiceRun()
        {
            bool result = false;
            if (!string.IsNullOrEmpty(currentMessageType))
            {
                try
                {
                    string SourceName = JsonHelper.ReadJsonString(serviceParams, "SourceName");
                    switch (CurrentMessageType)
                    {
                        case "ServerCenterService.ChangeTheme"://换肤
                            ChangeFrameWorkTheme(SourceName);
                            result = true;
                            break;
                        case "ServerCenterService.ChangeThemeByDll"://换肤
                            string skinPath = JsonHelper.ReadJsonString(serviceParams, "SkinPath");
                            ChangeFrameWorkTheme(SourceName, skinPath);
                            result = true;
                            break;
                        case "ServerCenterService.ChangeLanguage"://换语言
                            ChangeFrameWorkLanguage(SourceName);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            return result;
        }
        #region 具体服务实现
        /// <summary>
        /// 切换主题
        /// </summary>
        /// <param name="ThemePath"></param>
        private void ChangeFrameWorkTheme(string ThemePath = null)
        {
            ResourceDictionary resource = new ResourceDictionary();
            string path = string.IsNullOrEmpty(ThemePath) ? ConfigManager.GetAttributeOfNodeByName("UserInfo", "UserSkin") : ThemePath;
            path = string.Format("theme\\{0}.dll", path);
            resource.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + path, UriKind.RelativeOrAbsolute);
            //将资源字典合并到当前资源中
            if (Application.Current.Resources.MergedDictionaries.Count > 0 && Application.Current.Resources.MergedDictionaries[0] != null)
            {
                Application.Current.Resources.MergedDictionaries[0] = resource;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(resource);
            }
            if (!string.IsNullOrEmpty(ThemePath))
            {
                Dictionary<string, string> themeDic = new Dictionary<string, string>();
                themeDic.Add("UserSkin", "Victop.Themes.MetroSkin");
                ConfigManager.SaveAttributeOfNodeByName("UserInfo", themeDic);
            }
        }
        /// <summary>
        /// 切换主题
        /// </summary>
        /// <param name="ThemePath"></param>
        private void ChangeFrameWorkTheme(string ThemeName, string SkinPath)
        {
            Assembly assembly = Assembly.LoadFrom(string.Format("theme\\{0}.dll", SkinPath));
            ResourceDictionary myResourceDictionary = Application.LoadComponent(new Uri(ThemeName, UriKind.Relative)) as ResourceDictionary;
            if (Application.Current.Resources.MergedDictionaries.Count > 0 && Application.Current.Resources.MergedDictionaries[0] != null)
            {
                Application.Current.Resources.MergedDictionaries[0] = myResourceDictionary;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
            }
            if (!string.IsNullOrEmpty(SkinPath))
            {
                Dictionary<string, string> themeDic = new Dictionary<string, string>();
                themeDic.Add("UserSkin", SkinPath);
                ConfigManager.SaveAttributeOfNodeByName("UserInfo", themeDic);
            }
        }
        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="LunguagePath"></param>
        private void ChangeFrameWorkLanguage(string LunguagePath = null)
        {
            ResourceDictionary resource = new ResourceDictionary();
            string path = string.IsNullOrEmpty(LunguagePath) ? ConfigurationManager.AppSettings.Get("language") : LunguagePath;
            //GetXmlValue(AppDomain.CurrentDomain.BaseDirectory + path, "");
            //resource.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + path, UriKind.RelativeOrAbsolute);
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + path);
            XmlNodeList nodeList = doc.GetElementsByTagName("resource");
            foreach (XmlNode node in nodeList)
            {
                if (resource.Contains(node.Attributes["id"].Value))
                    continue;
                resource.Add(node.Attributes["id"].Value, node.InnerText);
            }

            //将资源字典合并到当前资源中  
            if (Application.Current.Resources.MergedDictionaries.Count > 1 && Application.Current.Resources.MergedDictionaries[1] != null)
            {
                Application.Current.Resources.MergedDictionaries[1] = resource;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(resource);
            }
            LoadAllPluginsLanguageResource(path);
            if (!string.IsNullOrEmpty(LunguagePath))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["language"].Value = LunguagePath;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        /// <summary>加载所有插件的语言资源</summary>
        private void LoadAllPluginsLanguageResource(string language)
        {
            //截取xxx/zh-cn.xxxx格式中的zh-cn
            string[] str = language.Split('/');
            language = str[str.Length - 1];
            str = language.Split('.');
            language = str[0];
            /// <summary>加载Plugin文件夹下所有插件的语言资源</summary>
            string FolderPath = AppDomain.CurrentDomain.BaseDirectory + @"Plugin\";
            DirectoryInfo folderInfo = new DirectoryInfo(FolderPath);
            FileInfo[] files = folderInfo.GetFiles("*.language");
            if (files == null) return;
            if (files.Count() == 0) return;
            foreach (FileInfo file in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file.FullName);
                XmlNodeList nodeList = doc.GetElementsByTagName("resourcecollection");
                foreach (XmlNode xmlNode in nodeList)
                {
                    string languageNode = xmlNode.Attributes["language"].Value;
                    if (!string.Equals(language, languageNode)) continue;
                    XmlNodeList list = xmlNode.ChildNodes;
                    foreach (XmlNode node in list)
                    {
                        string id = node.Attributes["id"].Value.ToString();
                        string value = node.InnerText;
                        if (Application.Current.Resources.MergedDictionaries[1].Contains(id))
                        {
                            Application.Current.Resources.MergedDictionaries[1][id] = value;
                        }
                        else
                        {
                            Application.Current.Resources.MergedDictionaries[1].Add(id, value);
                        }
                    }
                }
            }
        }
        #endregion


        public string ReplyContent
        {
            get { return string.Empty; }
        }
    }
}
