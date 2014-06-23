using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Victop.Frame.Connection;
using Victop.Frame.CoreLibrary.Models;

namespace ConnectionUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RequestMessage message = new RequestMessage();
            MessageBodyManager bodyManager = new MessageBodyManager();
            ReplyMessage replyMessage = bodyManager.SendMessage(message);
        }
    }
}
