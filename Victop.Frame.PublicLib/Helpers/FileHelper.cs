using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        /// <summary>
        /// 飞道上传助手
        /// </summary>
        /// <param name="uploadUrl">上传url</param>
        /// <param name="uploadMethod">上传方法:GET/POST</param>
        /// <param name="fileFullPath">上传文件完整路径</param>
        public static string VictopUploadHelper(string uploadUrl, string uploadMethod, string fileFullPath)
        {
            if (!string.IsNullOrEmpty(uploadUrl))
            {
                ProcessStartInfo info = new ProcessStartInfo("VictopUploadHelper.exe");
                info.Arguments = string.Format("{0} {1} {2}", uploadUrl, uploadMethod.ToUpper(), fileFullPath);
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                Process pro = Process.Start(info);
                StreamReader sr = pro.StandardOutput;
                string result = sr.ReadToEnd();
                return Regex.Unescape(result);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
