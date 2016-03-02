using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using gnu.CORBA.Poa;
using Microsoft.Win32;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using System.Reflection;
using System.Resources;

namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// 凭证原子操作
    /// </summary>
    public class VoucherAtOperation
    {
        private TemplateControl MainView;
        private static Dictionary<string, object> paramCompntDic = new Dictionary<string, object>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public VoucherAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }
    }
}
