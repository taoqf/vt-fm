using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Victop.Frame.CoreLibrary.Interfaces;

namespace Victop.Frame.CoreLibrary
{
    public class FeiDaoDBFactory
    {
        /// <summary>
        /// 创建默认通信器
        /// </summary>
        /// <returns></returns>
		public static IFeiDaoDataOperation CreateDefaultFeiDaoDB()
        {
            Assembly feidaoDBAssembly = Assembly.Load("Victop.Frame.DataChannel");
            IFeiDaoDataOperation feidaoDbObject = (IFeiDaoDataOperation)feidaoDBAssembly.CreateInstance("Victop.Frame.DataChannel.LocalDBManager");
            return feidaoDbObject;
        }
    }
}
