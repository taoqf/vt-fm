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
        /// <param name="regexStr">匹配字符</param>
        /// <returns></returns>
        public static string StartWith(string regexStr)
        {
            regexStr = string.Format("^{0}.*$", regexStr);
            return regexStr;
        }
        /// <summary>
        /// 以匹配字符串结尾
        /// </summary>
        /// <param name="regexStr">匹配字符</param>
        /// <returns></returns>
        public static string EndWith(string regexStr)
        {
            regexStr = string.Format("^.*{0}$", regexStr);
            return regexStr;
        }
        /// <summary>
        /// 包含字符串
        /// </summary>
        /// <param name="regexStr">匹配字符</param>
        /// <returns></returns>
        public static string Contains(string regexStr)
        {
            regexStr = string.Format("^.*{0}.*$", regexStr);
            return regexStr;
        }
        /// <summary>
        /// 整形范围(包含边界值)
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        public static Dictionary<string, object> longRange(long minValue, long maxValue)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$gte", minValue);
            resultDic.Add("$lte", maxValue);
            return resultDic;
        }
        /// <summary>
        /// 整形大于
        /// </summary>
        /// <param name="relativeValue">比较的值</param>
        /// /// <param name="edgeFlag">是否包含边界值</param>
        /// <returns></returns>
        public static Dictionary<string, object> longGreatThan(long relativeValue, bool edgeFlag = true)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            if (edgeFlag)
            {
                resultDic.Add("$gte", relativeValue);
            }
            else
            {
                resultDic.Add("$gt", relativeValue);
            }
            return resultDic;
        }
        /// <summary>
        /// 整形小于
        /// </summary>
        /// <param name="relativeValue">比较的值</param>
        /// <param name="edgeFlag">是否包含边界值</param>
        /// <returns></returns>
        public static Dictionary<string, object> longLessThan(long relativeValue, bool edgeFlag = true)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            if (edgeFlag)
            {
                resultDic.Add("$lte", relativeValue);
            }
            else
            {
                resultDic.Add("$lt", relativeValue);
            }
            return resultDic;
        }

        /// <summary>
        /// 时间范围(包含边界值)
        /// </summary>
        /// <param name="startTimeStr">开始时间</param>
        /// <param name="endTimeStr">结束时间</param>
        /// <returns></returns>
        public static Dictionary<string, object> TimeRange(string startTimeStr, string endTimeStr)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            DateTime zeroTime = new DateTime(1970, 1, 1);
            DateTime startTime = Convert.ToDateTime(startTimeStr);
            DateTime endTime = Convert.ToDateTime(endTimeStr);
            resultDic.Add("$gte", (long)(startTime.ToUniversalTime() - zeroTime.ToUniversalTime()).TotalMilliseconds);
            resultDic.Add("$lte", (long)(endTime.ToUniversalTime() - zeroTime.ToUniversalTime()).TotalMilliseconds);
            return resultDic;
        }
        /// <summary>
        /// 检索值In
        /// </summary>
        /// <param name="valueList">值集合</param>
        /// <returns></returns>
        public static Dictionary<string, object> ValuesIn(List<object> valueList)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$in", valueList);
            return resultDic;
        }
        /// <summary>
        /// 检索值Not In
        /// </summary>
        /// <param name="valueList">值集合</param>
        /// <returns></returns>
        public static Dictionary<string, object> ValuesNotIn(List<object> valueList)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$nin", valueList);
            return resultDic;
        }
        /// <summary>
        /// 检索值Or
        /// </summary>
        /// <param name="valueList">值集合</param>
        /// <returns></returns>
        public static Dictionary<string, object> ValuesOr(List<object> valueList)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$or", valueList);
            return resultDic;
        }

        /// <summary>
        /// 不等于
        /// </summary>
        /// <param name="relativeValue">比较的值</param>
        /// <returns></returns>
        public static Dictionary<string, object> ValueNoEqual(object relativeValue)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            resultDic.Add("$ne", relativeValue);
            return resultDic;
        }
        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long DateTimeToTimestamp(DateTime dateTime)
        {
            DateTime zeroTime = new DateTime(1970, 1, 1);
            return (long)(dateTime.ToUniversalTime() - zeroTime.ToUniversalTime()).TotalMilliseconds;
        }
        /// <summary>
        /// 时间戳转为本地时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(long timestamp)
        {
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
            return dt.AddMilliseconds(timestamp);
        }

    }
}
