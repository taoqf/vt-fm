using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Victop.Frame.Adapter;
using Victop.Frame.CoreLibrary.Models;
namespace AdapterUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            VEntry<int> vlist = new VEntry<int>(5, DateTime.Now);
            ObjectPool<string, VEntry<int>> ssg = new ObjectPool<string, VEntry<int>>();
            ssg.Add("dd", vlist);
            ssg.Add("dd", vlist);
            ssg.Add("dd", vlist);
            ssg.Add("dd", vlist);
            //测试Git修改
            //MessagePoolManager messagePoolMana = new MessagePoolManager();
            //messagePoolManager.GetMessageData("ddd");
            //bool falg =  messagePoolManager.CreateMessagePool(null);
            //MessageManager mm = new MessageManager();
            //mm.CreateMessage(Victop.Frame.CoreLibrary.Enums.MessageTargetEnum.MAIN);

            TaskPoolManager tm = new TaskPoolManager();
            tm.CreateMessagePool("dsdf");
            MessagePoolManager mpm = new MessagePoolManager();

            string messageid = Guid.NewGuid().ToString();
            string ReplyToId = messageid;
            RequestMessage message = new RequestMessage();
            message.MessageId = messageid;
            message.ReplyToId = ReplyToId;
            message.MessageType = "sadsadf";
            mpm.SaveMessageData(message);

            mpm.GetMessageData(messageid);

            
            Console.WriteLine(ssg.PoolMap.Count);
        }
    }
}
