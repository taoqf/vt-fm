using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.PublicLib.Helpers
{
    public static class RegexHelper
    {
        /// <summary>
        /// 以匹配字符串开始
        /// </summary>
        /// <param name="regexStr"></param>
        /// <returns></returns>
        public static string StartWith(string regexStr)
        {
            regexStr = string.Format("^{0}.*$", regexStr);
            return regexStr;
        }
        /// <summary>
        /// 以匹配字符串结尾
        /// </summary>
        /// <param name="regexStr"></param>
        /// <returns></returns>
        public static string EndWith(string regexStr)
        {
            regexStr = string.Format("^.*{0}$", regexStr);
            return regexStr;
        }
        /// <summary>
        /// 包含字符串
        /// </summary>
        /// <param name="regexStr"></param>
        /// <returns></returns>
        public static string Contains(string regexStr)
        {
            regexStr = string.Format("^.*{0}.*$", regexStr);
            return regexStr;
        }
    }
}
