using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Victop.Frame.ServerManagerCenter;
using Victop.Frame.PublicLib.Helpers;
using System.Collections.Generic;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.CoreLibrary;

namespace ServerManagerCenterUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("PluginName", "PortalFramePlugin");
            contentDic.Add("PluginPath", "");
            ServerRouter router = new ServerRouter();
            RequestMessage messageInfo = new RequestMessage() { MessageType = "PluginService.PluginRun", MessageContent = JsonHelper.ToJson(contentDic) };
            ReplyMessage message = router.ServerRamify(messageInfo, RamifyEnum.RUN);
            ActivePluginManager pluginManager = new ActivePluginManager();
            Console.WriteLine(pluginManager.GetActivePlugins().Keys.Count.ToString());
        }
    }
}
