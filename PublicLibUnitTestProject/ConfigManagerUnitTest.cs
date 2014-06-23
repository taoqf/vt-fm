using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Victop.Frame.PublicLib;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using Victop.Frame.PublicLib.Managers;

namespace PublicLibUnitTestProject
{
    [TestClass]
    public class ConfigManagerUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string fileName = "PartnerConfig.xml";
            string nodeName = "System";
            string attributeName = "AppName";
            XmlDocument xmlDoc = new XmlDocument();
            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName;
            if (!File.Exists(fileFullPath))
            {
                Console.WriteLine("FilePath:{0}",fileFullPath);
                return;
            }
            try
            {
                xmlDoc.Load(fileFullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            XmlElement rootElement = xmlDoc.DocumentElement;
            if (rootElement != null && rootElement.ChildNodes.Count>0)
            {
                foreach (XmlNode item in rootElement.ChildNodes)
                {
                    if (item.Name.Equals(nodeName))
                    {
                        Console.WriteLine("NodeName:{0}",item.Name);
                        foreach (XmlAttribute attrItem in item.Attributes)
                        {
                            if (attrItem.Name.Equals(attributeName))
                            {
                                Console.WriteLine("AttributeName:{0},AttributeValue:{1}",attrItem.Name,attrItem.Value);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        [TestMethod]
        public void GetNodeAttributesTestMethod()
        {
            string nodeName = "Master";
            Dictionary<string, string> attrList = ConfigManager.GetNodeAttributes(nodeName);
            foreach (var item in attrList)
            {
                Console.WriteLine("属性名:{0},属性值:{1}",item.Key,item.Value);
            }
        }
    }
}
