using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Victop.Frame.PublicLib.Helpers
{
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
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "fitdata\\" + fitDataPath + ".json";
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
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
                    return File.ReadAllText(filePath);
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
    }
}
