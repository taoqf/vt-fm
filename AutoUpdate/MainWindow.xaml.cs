using AutoUpdate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace AutoUpdate
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 字段
        AutoUpdateModel updateModel = new AutoUpdateModel();
        WebClient updateClient = new WebClient();
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += updateWindow_Loaded;
        }

        private void updateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Process[] process = Process.GetProcessesByName("VictopPartner.exe");
            foreach (Process item in process)
            {
                item.Kill();
            }
            InitUpdateInfo();
        }
        /// <summary>
        /// 初始化更新信息
        /// </summary>
        private void InitUpdateInfo()
        {
            updateModel.UpdateUrl = ConfigurationManager.AppSettings["UpdateUrl"];
            updateModel.LocalUpdateTimestamp = Convert.ToInt64(ConfigurationManager.AppSettings["UpDate"]);
            GetTheLastUpdateTime();
            GetUpdateSize();
            if (updateModel.LocalUpdateTimestamp != 0 && updateModel.ServerUpdateTimestamp != 0)
            {
                if (updateModel.ServerUpdateTimestamp > updateModel.LocalUpdateTimestamp)
                {
                    updateClient.DownloadProgressChanged += updateClient_DownloadProgressChanged;
                    updateClient.DownloadFileCompleted += updateClient_DownloadFileCompleted;
                }
            }
            if (updateModel.TotalSize == 0)
            {
                this.Close();
            }
            GetUpdateFileList();
            if (updateModel.FileNames.Count > 0)
            {
                updateModel.UpdatedNum = 0;
                DownLoadFile();
            }
        }

        private void GetUpdateFileList()
        {
            try
            {
                updateModel.FileNames = new List<string>();
                string AutoUpdaterFileName = updateModel.UpdateUrl + "AutoUpdater/AutoUpdater.xml";
                WebClient wc = new WebClient();
                Stream sm = wc.OpenRead(AutoUpdaterFileName);
                XmlTextReader xr = new XmlTextReader(sm);
                while (xr.Read())
                {
                    if (xr.Name == "FileInfo")
                    {
                        string fileInfo = xr.GetAttribute("Name");
                        updateModel.FileNames.Add(fileInfo);
                    }
                }
                xr.Close();
                sm.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetUpdateSize()
        {
            string AutoUpdaterFileName = updateModel.UpdateUrl + "AutoUpdater/AutoUpdater.xml";
            try
            {
                WebClient wc = new WebClient();
                Stream sm = wc.OpenRead(AutoUpdaterFileName);
                XmlTextReader xr = new XmlTextReader(sm);
                while (xr.Read())
                {
                    if (xr.Name == "UpdateFileList")
                    {
                        updateModel.TotalSize = Convert.ToInt64(xr.GetAttribute("Size"), CultureInfo.InvariantCulture);
                        break;
                    }
                }
                xr.Close();
                sm.Close();
            }
            catch (WebException ex)
            {   
            }
        }
        /// <summary>
        /// 文件下载完毕时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void updateClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + updateModel.LoadingFileName))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\" + updateModel.LoadingFileName);
            }
            File.Move(AppDomain.CurrentDomain.BaseDirectory + "\\AutoUpdater\\" + updateModel.LoadingFileName, AppDomain.CurrentDomain.BaseDirectory + "\\" + updateModel.LoadingFileName);
            updateModel.UpdateSize += updateModel.LoadingFileSize;
            if (updateModel.FileNames.Count > updateModel.UpdatedNum)
            {
                try
                {
                    updateModel.UpdatedNum++;
                    DownLoadFile();
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["UpDate"].Value = updateModel.ServerUpdateTimestamp.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                this.Close();
            }
        }

        private void DownLoadFile()
        {
            updateModel.LoadingFileName = updateModel.FileNames[(int)updateModel.UpdatedNum];
            tBlockNow.Text = string.Format("更新进度 {0}/{1}  [ {2} ]", updateModel.UpdatedNum, updateModel.TotalCount, ConvertSize(updateModel.TotalSize));
            proBarTotal.Value = 0;
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\AutoUpdater\\";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            updateClient.DownloadFileAsync(new Uri(updateModel.UpdateUrl + "AutoUpdater/" + updateModel.LoadingFileName), directoryPath + updateModel.LoadingFileName);
        }
        /// <summary>
        /// 下载进行时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void updateClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            tBlockProcess.Text = string.Format("正在下载:{0} [{1}/{2}]", updateModel.LoadingFileName, ConvertSize(e.BytesReceived), ConvertSize(e.TotalBytesToReceive));
            updateModel.LoadingFileSize = e.TotalBytesToReceive;
            float tempF = ((float)(updateModel.UpdateSize + e.BytesReceived) / updateModel.TotalSize);
            proBarNow.Value = Convert.ToInt32(tempF * 100);
            proBarTotal.Value = e.ProgressPercentage;
        }

        /// <summary> 
        /// 判断软件的更新日期 
        /// </summary> 
        /// <param name="Dir">服务器地址</param> 
        /// <returns>返回日期</returns> 
        private void GetTheLastUpdateTime()
        {
            string AutoUpdaterFileName = updateModel.UpdateUrl + "AutoUpdater/AutoUpdater.xml";
            try
            {
                WebClient wc = new WebClient();
                Stream sm = wc.OpenRead(AutoUpdaterFileName);
                XmlTextReader xml = new XmlTextReader(sm);
                while (xml.Read())
                {
                    if (xml.Name == "UpdateTime")
                    {
                        updateModel.ServerUpdateTimestamp = Convert.ToInt64(xml.GetAttribute("Date"));
                        break;
                    }
                }
                xml.Close();
                sm.Close();
            }
            catch (WebException ex)
            {
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
                if ((tempf / 1024) / 1024 > 1)
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
    }
}
