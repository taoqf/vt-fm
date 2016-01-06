using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;
using Victop.Frame.PublicLib.Helpers;
using Victop.Wpf.Controls;

namespace VictopUploadHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 字段
        WebClient client = new WebClient();
        private string uploadUrl = string.Empty;
        private string uploadMethod = string.Empty;
        private string uploadFileName = string.Empty;
        private string resultStr = string.Empty;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Console.Write(resultStr);
        }

        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            vicpBar.Value = e.ProgressPercentage * 2;
            tBoxSendSize.Text = ConvertSize(e.BytesSent);
            tBoxTotalSize.Text = ConvertSize(e.TotalBytesToSend);

        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            resultStr = System.Text.Encoding.Default.GetString(e.Result);
            this.Close();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string[] argsStr = Environment.GetCommandLineArgs();
            if (argsStr.Length == 4)
            {
                uploadUrl = argsStr[1];
                uploadMethod = argsStr[2];
                uploadFileName = argsStr[3];
                tboxFileName.Text = uploadFileName;
                client.UploadFileCompleted += Client_UploadFileCompleted;
                client.UploadProgressChanged += Client_UploadProgressChanged;
                client.UploadFileAsync(new Uri(uploadUrl), uploadMethod, uploadFileName);
            }
        }

        /// <summary> 
        /// 转换字节大小 
        /// </summary> 
        /// <param name="byteSize">输入字节数</param> 
        /// <returns>返回值</returns> 
        private static string ConvertSize(long byteSize)
        {
            string str = "";
            float tempf = (float)byteSize;
            if (tempf / 1024 > 1)
            {
                if (((tempf / 1024) / 1024) / 1024 > 1)
                {
                    str = (((tempf / 1024) / 1024) / 1024).ToString("##0.00", CultureInfo.InvariantCulture) + "G";
                }
                else if ((tempf / 1024) / 1024 > 1)
                {
                    str = ((tempf / 1024) / 1024).ToString("##0.00", CultureInfo.InvariantCulture) + "MB";
                }
                else
                {
                    str = (tempf / 1024).ToString("##0.00", CultureInfo.InvariantCulture) + "KB";
                }
            }
            else
            {
                str = tempf.ToString(CultureInfo.InvariantCulture) + "B";
            }
            return str;
        }

        private void btnCannel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.OK == VicMessageBoxNormal.Show("是否取消上传", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information))
            {
                //client.CancelAsync();
                Dictionary<string, object> resultDic = new Dictionary<string, object>();
                resultDic.Add("code", "-1");
                resultDic.Add("message", "已取消上传");
                resultStr = JsonHelper.ToJson(resultDic);
                this.Close();
            }
        }
    }
}
