using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// User32消息辅助类
    /// </summary>
    public class User32MessageHelper
    {
        /// <summary>
        /// 下载完成
        /// </summary>
        public const int WM_DOWNLOAD_COMPLETED = 0x00AA;

        /// <summary>
        /// 查询Window
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 设置窗体的显示与隐藏
        /// </summary>
        /// <param name="hWnd">目标窗体句柄</param>
        /// <param name="nCmdShow">0：隐藏,1:显示</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="wnd">目标窗体句柄</param>
        /// <param name="msg">消息标识</param>
        /// <param name="wP">自定义数值</param>
        /// <param name="lP">结构体</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="wnd">目标窗体句柄</param>
        /// <param name="msg">消息标识</param>
        /// <param name="wP">自定义数值</param>
        /// <param name="lP">结构体</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);
        /// <summary>
        /// 隐藏控制台
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <param name="ConsoleTitle">控制台标题(可为空,为空则取默认值)</param>  
        public static void HideConsole(string processName, string ConsoleTitle = "")
        {
            ConsoleTitle = String.IsNullOrEmpty(ConsoleTitle) ? Console.Title : ConsoleTitle;
            IntPtr hWnd = FindWindow(processName, ConsoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }
        }
        /// <summary>  
        /// 显示控制台  
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <param name="ConsoleTitle">控制台标题(可为空,为空则去默认值)</param>  
        public static void ShowConsole(string processName, string ConsoleTitle = "")
        {
            ConsoleTitle = String.IsNullOrEmpty(ConsoleTitle) ? Console.Title : ConsoleTitle;
            IntPtr hWnd = FindWindow(processName, ConsoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 1);
            }
        }

    }
}
