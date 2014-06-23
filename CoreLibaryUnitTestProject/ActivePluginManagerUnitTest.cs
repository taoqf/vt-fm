using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.CoreLibrary.Enums;

namespace CoreLibaryUnitTestProject
{
    [TestClass]
    public class ActivePluginManagerUnitTest
    {
        [TestMethod]
        public void AddPluginTestMethod()
        {
            ActivePluginManager pluginMgr = new ActivePluginManager();
            ActivePluginInfo pluginInfo = new ActivePluginInfo();
            pluginInfo.AppId = "test";
            pluginInfo.BusinessKey = "DF001200";
            pluginInfo.CloudGalleryId= GalleryEnum.VICTOP;
            pluginInfo.ObjectId = "test001";
            pluginInfo.ShowType = 0;
            pluginMgr.AddPlugin(pluginInfo);
           
            //foreach (ActivePluginInfo item in new ActivePluginManager().GetActivePlugins().Keys)
            //{
            //    if (item.BusinessKey == "DF001200")
            //    {
            //        Console.WriteLine("找到了");
            //        break;
            //    }
            //}
            Console.WriteLine(GalleryEnum.LOCALSOA.ToString());
        }
    }
}
