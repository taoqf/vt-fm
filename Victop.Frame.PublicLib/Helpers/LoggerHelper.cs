using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using log4net;
using log4net.Core;
using log4net.Config;
using log4net.Appender;
using log4net.Layout;
using log4net.Filter;
using Victop.Frame.PublicLib.Managers;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// 日志辅助类
    /// </summary>

    public class LoggerHelper
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static LoggerHelper()
        {
            ConsoleAppender console = CreateConsoleAppender();
            BasicConfigurator.Configure(console);
            console.ActivateOptions();
            bool autoClean = ConfigManager.GetAttributeOfNodeByName("Log", "Clean").Equals("0") ? true : false;
            if (autoClean)
            {
                CleanLogFile();
            }
            string nowData = DateTime.Now.ToString("yyyyMMddHHmmss");
            string debugFile = "debug\\" + nowData + ".log";
            string infoFile = "log\\" + nowData + ".log";

            RollingFileAppender debug = CreateFileAppender("debug", debugFile);
            RollingFileAppender info = CreateFileAppender("log", infoFile);

            LevelRangeFilter infoFilter = new LevelRangeFilter();
            infoFilter.LevelMin = Level.Info;
            info.AddFilter(infoFilter);
            BasicConfigurator.Configure(info);
            info.ActivateOptions();
            bool debugFlag = ConfigManager.GetAttributeOfNodeByName("Log","Debug").Equals("0") ? true : false;
            if (debugFlag)
            {
                BasicConfigurator.Configure(debug);
                debug.ActivateOptions();
            }
            string path = string.Format("{0}Logger.xml", AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(path))
            {
                XmlConfigurator.Configure(new FileInfo(path));
            }
        }

        /// <summary>
        /// 创建控制台日志配置
        /// </summary>
        /// <returns>配置</returns>
        private static ConsoleAppender CreateConsoleAppender()
        {
            ConsoleAppender appender = new ConsoleAppender();
            appender.Name = "console";
            PatternLayout layout = new PatternLayout("[V+]%-d{HH:mm:ss.fff} %-5p [%t] %c{1}(%L) | %m%n");
            appender.Layout = layout;
            return appender;
        }

        /// <summary>
        /// 创建文件日志配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="file">日志文件路径</param>
        /// <returns>配置</returns>
        private static RollingFileAppender CreateFileAppender(string name, string file)
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = name;
            appender.File = file;
            appender.AppendToFile = true;
            appender.StaticLogFileName = false;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.MaximumFileSize = "5MB";
            appender.MaxSizeRollBackups = 10;
            PatternLayout layout = new PatternLayout("[V+] %-d{MM-dd HH:mm:ss.fff} %-5p [%t] %c{1}(%L) | %m%n");
            appender.Layout = layout;
            return appender;
        }

        /// <summary>
        /// 清理超过一定时间的日志文件
        /// </summary>
        private static void CleanLogFile()
        {
            DateTime date = DateTime.Now;
            string unit = ConfigManager.GetAttributeOfNodeByName("Log", "Unit");
            int num = Convert.ToInt32(ConfigManager.GetAttributeOfNodeByName("Log", "Num"));
            switch (unit.ToLower().Substring(0, 3))
            {
                case "yea":
                    date = date.AddYears(0 - num);
                    break;
                case "mon":
                    date = date.AddMonths(0 - num);
                    break;
                case "day":
                    date = date.AddDays(0 - num);
                    break;
                case "hou":
                    date = date.AddHours(0 - num);
                    break;
                case "min":
                    date = date.AddMinutes(0 - num);
                    break;
                case "sec":
                    date = date.AddSeconds(0 - num);
                    break;
                default:
                    break;
            }
            DirectoryInfo debug = new DirectoryInfo("debug");
            if (debug.Exists)
            {
                FileInfo[] files = debug.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.CreationTime < date)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception)
                        { }
                    }
                }
            }
            DirectoryInfo info = new DirectoryInfo("log");
            if (info.Exists)
            {
                FileInfo[] files = info.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.CreationTime < date)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception)
                        { }
                    }
                }
            }
        }
        /// <summary>
        /// 调试输出
        /// </summary>
        /// <param name="message">输出对象</param>
        public static void Debug(object message)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
        }
        /// <summary>
        /// 调试输出
        /// </summary>
        /// <param name="message">输出对象</param>
        /// <param name="ex">异常信息</param>
        public static void Debug(object message, Exception ex)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message, ex);
            }
        }
        /// <summary>
        /// 调试输出
        /// </summary>
        /// <param name="format">输出格式</param>
        /// <param name="args">输出参数</param>
        public static void DebugFormat(string format, params object[] args)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat(format, args);
            }
        }
        /// <summary>
        /// 错误输出
        /// </summary>
        /// <param name="message">输出对象</param>
        public static void Error(object message)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
        }
        /// <summary>
        /// 错误输出
        /// </summary>
        /// <param name="message">输出对象</param>
        /// <param name="ex">异常信息</param>
        public static void Error(object message, Exception ex)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsErrorEnabled)
            {
                logger.Error(message, ex);
            }
        }
        /// <summary>
        /// 错误输出
        /// </summary>
        /// <param name="format">输出格式</param>
        /// <param name="args">输出参数</param>
        public static void ErrorFormat(string format, params object[] args)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsErrorEnabled)
            {
                logger.ErrorFormat(format, args);
            }
        }
        /// <summary>
        /// 毁灭输出
        /// </summary>
        /// <param name="message">输出对象</param>
        public static void Fatal(object message)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }
        }
        /// <summary>
        /// 毁灭输出
        /// </summary>
        /// <param name="message">输出对象</param>
        /// <param name="ex">异常信息</param>
        public static void Fatal(object message, Exception ex)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message, ex);
            }
        }
        /// <summary>
        /// 毁灭输出
        /// </summary>
        /// <param name="format">输出格式</param>
        /// <param name="args">输出参数</param>
        public static void FatalFormat(string format, params object[] args)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsFatalEnabled)
            {
                logger.FatalFormat(format, args);
            }
        }
        /// <summary>
        /// 信息输出
        /// </summary>
        /// <param name="message">输出对象</param>
        public static void Info(object message)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }
        /// <summary>
        /// 信息输出
        /// </summary>
        /// <param name="message">输出对象</param>
        /// <param name="ex">错误信息</param>
        public static void Info(object message, Exception ex)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsInfoEnabled)
            {
                logger.Info(message, ex);
            }
        }
        /// <summary>
        /// 信息输出
        /// </summary>
        /// <param name="format">输出格式</param>
        /// <param name="args">输出参数</param>
        public static void InfoFormat(string format, params object[] args)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsInfoEnabled)
            {
                logger.InfoFormat(format, args);
            }
        }
        /// <summary>
        /// 警告输出
        /// </summary>
        /// <param name="message">输出对象</param>
        public static void Warn(object message)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
        }
        /// <summary>
        /// 警告输出
        /// </summary>
        /// <param name="message">输出对象</param>
        /// <param name="ex">异常信息</param>
        public static void Warn(object message, Exception ex)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message, ex);
            }
        }
        /// <summary>
        /// 警告输出
        /// </summary>
        /// <param name="format">输出格式</param>
        /// <param name="args">输出参数</param>
        public static void WarnFormat(string format, params object[] args)
        {
            ILog logger = LogManager.GetLogger((new StackTrace(false)).GetFrame(1).GetMethod().DeclaringType);
            if (logger.IsWarnEnabled)
            {
                logger.WarnFormat(format, args);
            }
        }
    }
}
