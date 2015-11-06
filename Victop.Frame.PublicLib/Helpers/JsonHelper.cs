using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// Json转换类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            if (obj == null) return "";
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            string jsonstr = JsonConvert.SerializeObject(obj, timeConverter);
            return jsonstr;
        }
        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="jsonStr">JSON字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(string jsonStr)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(jsonStr);
                return obj;
            }
            catch
            {
                return default(T);
            }

        }
        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="jsonStr">JSON字符串</param>
        /// <param name="depthLength">反序列化深度</param>
        /// <returns></returns>
        public static T ToObject<T>(string jsonStr, int depthLength)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { MaxDepth = depthLength };
                T obj = JsonConvert.DeserializeObject<T>(jsonStr, settings);
                return obj;
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <param name="jsonStr">JSON字符串</param>
        /// <returns></returns>
        public static dynamic DeserializeObject(string jsonStr)
        {
            try
            {
                dynamic obj = JsonConvert.DeserializeObject(jsonStr);
                return obj;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// 查找特定的值
        /// </summary>
        /// <param name="jsonStr">JSON字符串</param>
        /// <param name="key">指定的Key</param>
        /// <returns></returns>
        public static string ReadJsonString(string jsonStr, string key)
        {
            try
            {
                JObject jsonObj = JObject.Parse(jsonStr);
                return jsonObj[key].ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 指定Key的字符串反序列化
        /// </summary>
        /// <typeparam name="T">反序列化的类型</typeparam>
        /// <param name="jsonStr">JSON字符串</param>
        /// <param name="key">指定的Key</param>
        /// <returns></returns>
        public static T ReadJsonObject<T>(string jsonStr, string key)
        {
            try
            {
                JObject jsonObj = JObject.Parse(jsonStr);
                return jsonObj[key].ToObject<T>();
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 读取字符串反序列化
        /// </summary>
        /// <typeparam name="T">反序列化的类型</typeparam>
        /// <param name="jsonStr">JSON字符串</param>
        /// <returns></returns>
        public static T ReadJsonObject<T>(string jsonStr)
        {
            try
            {
                JObject jsonObj = JObject.Parse(jsonStr);
                return jsonObj.ToObject<T>();
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// XML对象转换为Json字符串
        /// </summary>
        /// <param name="doc">XML文档</param>
        /// <returns></returns>
        public static string XmlToJson(XmlDocument doc)
        {
            try
            {
                if (doc != null)
                    return JsonConvert.SerializeXmlNode(doc);
            }
            catch
            {

            } return "";
        }
        /// <summary>
        /// Json字符串转换为XML对象
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static XmlDocument JsonToXml(string jsonStr)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonStr))
                    return JsonConvert.DeserializeXmlNode(jsonStr);
            }
            catch
            {

            } return null;
        }
    }
}
