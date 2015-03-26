using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.DataMessageManager.Models;

namespace Victop.Frame.DataMessageManager.StaticClass
{
    internal static class DataLockInfoClass
    {
        private static List<LockInfoModel> lockInfoList;

        internal static List<LockInfoModel> LockInfoList
        {
            get
            {
                if (DataLockInfoClass.lockInfoList == null)
                    DataLockInfoClass.lockInfoList = new List<LockInfoModel>();
                return DataLockInfoClass.lockInfoList;
            }
            set
            {
                DataLockInfoClass.lockInfoList = value;
            }
        }
    }
}
