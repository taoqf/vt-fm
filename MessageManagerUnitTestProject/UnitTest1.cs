using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Victop.Frame.MessageManager;
using System.Collections.Generic;
using Victop.Frame.PublicLib.Helpers;

namespace MessageManagerUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginRun");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("PluginName", "PortalFramePlugin");
            contentDic.Add("PluginPath", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(Test));
        }
        private void Test(object newTest)
        {
            Console.WriteLine(newTest.ToString());
        }


        [TestMethod]
        public void TestMethod2()
        {
            string str = "{\"MessageType\":\"11\",\"MessageContent\":\"发送消息\"}";
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), str, new WaitCallback(Test2));
        }
        private void Test2(object newTest)
        {
            Console.WriteLine(newTest.ToString());
        }
    }
}
