using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// 正则表达式辅助类
    /// </summary>
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
        /// <summary>
        /// 整形范围(包含边界值)
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static Dictionary<string, object> longRange(long minValue, long maxValue)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$gte", minValue);
            resultDic.Add("$lte", maxValue);
            return resultDic;
        }
        /// <summary>
        /// 时间范围(包含边界值)
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static Dictionary<string, object> TimeRange(string startTimeStr, string endTimeStr)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            DateTime zeroTime = new DateTime(1970, 1, 1);
            DateTime startTime = Convert.ToDateTime(startTimeStr);
            DateTime endTime = Convert.ToDateTime(endTimeStr);
            resultDic.Add("$gte", (long)(startTime-zeroTime).TotalMilliseconds);
            resultDic.Add("$lte", (long)(endTime - zeroTime).TotalMilliseconds);
            return resultDic;
        }
        /// <summary>
        /// 检索值In
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ValuesIn(List<object> valueList)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$in", valueList);
            return resultDic;
        }
        /// <summary>
        /// 不等于
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ValueNoEqual(object value)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$ne", value);
            return resultDic;
        }

    }
}
