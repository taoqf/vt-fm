using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// 文件辅助类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 读取装配数据文本文件
        /// </summary>
        /// <param name="fitDataPath">装配数据文本文件名称</param>
        /// <returns></returns>
        public static string ReadFitData(string fitDataPath)
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "data\\" + fitDataPath + ".json";
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 读取插件配置文件
        /// </summary>
        /// <param name="configPath">插件配置文件</param>
        /// <returns></returns>
        public static string ReadPluginConfigData(string configPath)
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "PluginConfig\\" + configPath + ".json";
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 读取文本文件所有行
        /// </summary>
        /// <param name="filePath">文本文件路径</param>
        /// <returns></returns>
        public static string ReadText(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="stream">流信息</param>
        /// <returns></returns>
        public static string ReadText(Stream stream)
        {
            if (stream == null)
            {
                return string.Empty;
            }
            StreamReader sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
    }
}
