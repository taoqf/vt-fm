using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.PublicLib.Common
{
    /// <summary>
    /// FTP管理通用类
    /// </summary>
    public class FTPManagerCommon
    {
        string ftpServerIP;
        string ftpUserID;
        string ftpPassword;
        FtpWebRequest reqFTP;//FTP请求
        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <param name="path"></param>
        private void Connect(String path)
        {
            // 根据uri创建FtpWebRequest对象
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
        }
        /// <summary>
        /// FTP管理通用类构造函数
        /// </summary>
        /// <param name="ftpServerIP">FTP服务地址</param>
        /// <param name="ftpUserID">用户名</param>
        /// <param name="ftpPassword">密码</param>
        public FTPManagerCommon(string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
        }

        //三个重载函数从ftp服务器上获得文件列表
        private string[] GetFileList(string path, string WRMethods)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            try
            {
                Connect(path);
                reqFTP.Method = WRMethods;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("获取文件列表错误:{0}", ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }
        public string[] GetFileList(string path)
        {
            return GetFileList("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectory);
        }
        public string[] GetFileList()
        {
            return GetFileList("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectory);
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="localpath">本地路径</param>
        /// <param name="ftppath">ftp路径</param>
        /// <returns>true:上传成功;false:上传失败</returns>
        public bool Upload(string localpath, string ftppath)
        {
            bool result = false;
            try
            {
                FtpCheckDirectoryExist(ftppath);
                FileInfo fileInf = new FileInfo(localpath);
                string url = "ftp://" + ftpServerIP + "/" + (string.IsNullOrEmpty(ftppath) ? "/" : ftppath) + fileInf.Name;
                Connect(url);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                try
                {
                    Stream strm = reqFTP.GetRequestStream();
                    contentLen = fs.Read(buff, 0, buffLength);
                    while (contentLen != 0)
                    {
                        strm.Write(buff, 0, contentLen);
                        contentLen = fs.Read(buff, 0, buffLength);
                        result = true;
                    }
                    strm.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    LoggerHelper.ErrorFormat("上传错误:{0}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("上传错误:{0}", ex.Message);
            }
            return result;
        }

        private void FtpCheckDirectoryExist(string destFilePath)
        {
            string fullDir = FtpParseDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "/";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                if (!string.IsNullOrEmpty(dir))
                {
                    try
                    {
                        curDir += dir + "/";
                        MakeDir(curDir);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private string FtpParseDirectory(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }

        /// <summary>
        ///下载文件
        /// </summary>
        /// <param name="localpath">本地路径</param>
        /// <param name="ftpPath">ftp路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="errorinfo">错误信息</param>
        /// <returns></returns>
        public bool Download(string localpath, string ftpPath, string fileName, out string errorinfo)
        {
            try
            {
                String onlyFileName = Path.GetFileName(fileName);
                string newFileName = localpath + "\\" + onlyFileName;
                if (File.Exists(newFileName))
                {
                    errorinfo = string.Format("本地文件{0}已存在,无法下载", newFileName);
                    return false;
                }
                string url = "ftp://" + ftpServerIP + "/" + (string.IsNullOrEmpty(ftpPath) ? "/" : ftpPath) + fileName;
                Connect(url);//连接 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);

                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                errorinfo = "";
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = string.Format("因{0},无法下载", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="localFullName">本地完整路径</param>
        /// <param name="ftpFullName">服务器段完整路径</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool DownLoad(string localFullName, string ftpFullName, out string errorInfo)
        {
            try
            {
                string url = "ftp://" + ftpServerIP + "/" + ftpFullName;
                Connect(url);//连接 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);

                FileStream outputStream = new FileStream(localFullName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                errorInfo = "";
                return true;
            }
            catch (Exception ex)
            {
                errorInfo = string.Format("因{0},无法下载", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        public void DeleteFileName(string remotePath, string fileName)
        {
            try
            {
                string url = "ftp://" + ftpServerIP + (string.IsNullOrEmpty(remotePath) ? "/" : remotePath) + fileName;
                Connect(url);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("删除文件错误:{0}", ex.Message);
            }
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dirName">目录名称</param>
        public bool MakeDir(string dirName)
        {
            Connect(string.Format("ftp://{0}{1}", ftpServerIP, dirName));
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse reponse = (FtpWebResponse)reqFTP.GetResponse();
                reponse.Close();
            }
            catch (Exception ex)
            {
                reqFTP.Abort();
                return false;
            }
            reqFTP.Abort();
            return true;
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirName">目录名称</param>
        public void DelDir(string remotePath, string dirName)
        {
            try
            {
                string url = "ftp://" + ftpServerIP + "/" + (string.IsNullOrEmpty(remotePath) ? "/" : remotePath) + dirName;
                Connect(url);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("删除目录错误:{0}", ex.Message);
            }
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public long GetFileSize(string fileName)
        {
            long fileSize = 0;
            try
            {
                FileInfo fileInf = new FileInfo(fileName);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//连接 
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("获取文件大小错误:{0}", ex.Message);
            }
            return fileSize;
        }
        public DateTime GetFileTime(string fileName)
        {
            DateTime fileTime = DateTime.Now;
            try
            {
                FileInfo fileInf = new FileInfo(fileName);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//连接 
                reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                fileTime = response.LastModified;
                response.Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("获取文件大小错误:{0}", ex.Message);
            }
            return fileTime;
        }
        /// <summary>
        /// 修改文件名称
        /// </summary>
        /// <param name="currentFilename">当前名称</param>
        /// <param name="newFilename">新文件名</param>
        public void Rename(string remotePath, string currentFilename, string newFilename)
        {
            try
            {
                string url = "ftp://" + ftpServerIP + "/" + (string.IsNullOrEmpty(remotePath) ? "/" : remotePath) + currentFilename;
                Connect(url);
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("修改文件名称错误:{0}", ex.Message);
            }
        }
        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            return GetFileList("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectoryDetails);
        }
        /// <summary>
        /// 获取文件明细
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public string[] GetFilesDetailList(string path)
        {
            return GetFileList("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectoryDetails);
        }
    }
}
