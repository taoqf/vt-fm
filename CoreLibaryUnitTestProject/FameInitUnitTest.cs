using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;

namespace CoreLibaryUnitTestProject
{
    [TestClass]
    public class FameInitUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            FrameInit init = new FrameInit();
            init.FrameRun();
        }
    }
}
