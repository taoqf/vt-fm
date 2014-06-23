using Newtonsoft.Json;
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
        //json序列化
        public static string ToJson(object obj)
        {
            if (obj == null) return "";
            string jsonstr = JsonConvert.SerializeObject(obj);
            return jsonstr;
        }
        //json反序列化
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
        //查找特定的值
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
        //XML对象转换为Json字符串
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
        //Json字符串转换为XML对象
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
