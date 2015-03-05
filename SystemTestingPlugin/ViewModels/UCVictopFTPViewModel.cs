using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using SystemTestingPlugin.Models;
using Victop.Server.Controls.Models;
using Victop.Frame.PublicLib.Common;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace SystemTestingPlugin.ViewModels
{
    public class UCVictopFTPViewModel : ModelBase
    {
        #region 字段
        /// <summary>
        /// 站点信息
        /// </summary>
        private FTPSiteInfoModel siteInfoModel;
        /// <summary>
        /// 本地文件列表
        /// </summary>
        private ObservableCollection<FTPFileInfoModel> localFileList;
        /// <summary>
        /// 远程文件列表
        /// </summary>
        private ObservableCollection<FTPFileInfoModel> remoteFileList;
        /// <summary>
        /// 选定的远程文件
        /// </summary>
        private FTPFileInfoModel selectedRmoteFile;
        /// <summary>
        /// 选定的本地文件
        /// </summary>
        private FTPFileInfoModel selectedLocalFile;
        /// <summary>
        /// 错误信息
        /// </summary>
        private string errorMsg;
        #endregion
        #region 属性
        /// <summary>
        /// 站点信息
        /// </summary>
        public FTPSiteInfoModel SiteInfoModel
        {
            get
            {
                if (siteInfoModel == null)
                    siteInfoModel = new FTPSiteInfoModel();
                return siteInfoModel;
            }
            set
            {
                if (siteInfoModel != value)
                {
                    siteInfoModel = value;
                    RaisePropertyChanged("SiteInfoModel");
                }
            }
        }
        /// <summary>
        /// 本地文件列表
        /// </summary>
        public ObservableCollection<FTPFileInfoModel> LocalFileList
        {
            get
            {
                if (localFileList == null)
                    localFileList = new ObservableCollection<FTPFileInfoModel>();
                return localFileList;
            }
            set
            {
                if (localFileList != value)
                {
                    localFileList = value;
                    RaisePropertyChanged("LocalFileList");
                }
            }
        }
        /// <summary>
        /// 远程文件列表
        /// </summary>
        public ObservableCollection<FTPFileInfoModel> RemoteFileList
        {
            get
            {
                if (remoteFileList == null)
                    remoteFileList = new ObservableCollection<FTPFileInfoModel>();
                return remoteFileList;
            }
            set
            {
                if (remoteFileList != value)
                {
                    remoteFileList = value;
                    RaisePropertyChanged("RemoteFileList");
                }
            }
        }
        /// <summary>
        /// 选定远程文件
        /// </summary>
        public FTPFileInfoModel SelectedRmoteFile
        {
            get
            {
                return selectedRmoteFile;
            }
            set
            {
                if (selectedRmoteFile != value)
                {
                    selectedRmoteFile = value;
                    RaisePropertyChanged("SelectedRmoteFile");
                }
            }
        }
        /// <summary>
        /// 选定的本地文件
        /// </summary>
        public FTPFileInfoModel SelectedLocalFile
        {
            get
            {
                return selectedLocalFile;
            }
            set
            {
                if (selectedLocalFile != value)
                {
                    selectedLocalFile = value;
                    RaisePropertyChanged("SelectedLocalFile");
                }
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return errorMsg;
            }
            set
            {
                if (errorMsg != value)
                {
                    errorMsg = value;
                    RaisePropertyChanged("ErrorMsg");
                }
            }
        }
        #endregion
        #region 命令
        /// <summary>
        /// 页面加载
        /// </summary>
        public ICommand mainViewLoadedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SiteInfoModel.LocalPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
                    SiteInfoModel.RemotePath = "/";
                    GetLocalFileInfo();
                });
            }
        }
        /// <summary>
        ///连接
        /// </summary>
        public ICommand btnConnectionClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ConnectFTP();
                });
            }
        }
        /// <summary>
        /// 浏览
        /// </summary>
        public ICommand btnLocalViewClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    GetLocalFileInfo();
                });
            }
        }
        /// <summary>
        /// 本地站点双击事件
        /// </summary>
        public ICommand lviewLocalMouseDoubleClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedLocalFile != null)
                    {
                        if (SelectedLocalFile.IsDirectory)
                        {
                            if (!SelectedLocalFile.FileName.Equals(".."))
                            {
                                SiteInfoModel.LocalPath = Path.Combine(SiteInfoModel.LocalPath.EndsWith(":") ? SiteInfoModel.LocalPath + "\\" : SiteInfoModel.LocalPath, SelectedLocalFile.FileName);
                            }
                            else
                            {
                                SiteInfoModel.LocalPath = SiteInfoModel.LocalPath.Contains("\\") ? SiteInfoModel.LocalPath.Substring(0, SiteInfoModel.LocalPath.LastIndexOf("\\")) : AppDomain.CurrentDomain.BaseDirectory;
                            }
                            GetLocalFileInfo();
                        }
                        else
                        {
                            FTPManagerCommon ftpMgr = new FTPManagerCommon(SiteInfoModel.HostUrl, SiteInfoModel.UserName, SiteInfoModel.UserPwd);
                            bool result = ftpMgr.Upload(Path.Combine(SiteInfoModel.LocalPath.EndsWith(":") ? SiteInfoModel.LocalPath + "\\" : SiteInfoModel.LocalPath, SelectedLocalFile.FileName), SiteInfoModel.RemotePath.EndsWith("/") ? SiteInfoModel.RemotePath : SiteInfoModel.RemotePath + "/");
                            if (result)
                            {
                                ConnectFTP();
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 远端站点双击事件
        /// </summary>
        public ICommand lviewRemoteMouseDoubleClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedRmoteFile != null)
                    {
                        if (SelectedRmoteFile.IsDirectory)
                        {
                            if (!SelectedRmoteFile.FileName.Equals(".."))
                            {
                                if (SiteInfoModel.RemotePath.EndsWith("/"))
                                {
                                    SiteInfoModel.RemotePath = string.Format("{0}{1}", SiteInfoModel.RemotePath, SelectedRmoteFile.FileName);
                                }
                                else
                                {
                                    SiteInfoModel.RemotePath = string.Format("{0}/{1}", SiteInfoModel.RemotePath, SelectedRmoteFile.FileName);
                                }
                            }
                            else
                            {
                                SiteInfoModel.RemotePath = SiteInfoModel.RemotePath.Contains("/") ? SiteInfoModel.RemotePath.Substring(0, SiteInfoModel.RemotePath.LastIndexOf("/")) : "/";
                            }
                            ConnectFTP();
                        }
                        else
                        {
                            FTPManagerCommon ftpMgr = new FTPManagerCommon(SiteInfoModel.HostUrl, SiteInfoModel.UserName, SiteInfoModel.UserPwd);
                            bool result = ftpMgr.Download(SiteInfoModel.LocalPath, SiteInfoModel.RemotePath.EndsWith("/") ? SiteInfoModel.RemotePath : SiteInfoModel.RemotePath + "/", SelectedRmoteFile.FileName, out errorMsg);
                            if (result)
                            {
                                GetLocalFileInfo();
                            }
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 远程下载
        /// </summary>
        public ICommand mItemRemoteDownLoadClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FTPManagerCommon ftpMgr = new FTPManagerCommon(SiteInfoModel.HostUrl, SiteInfoModel.UserName, SiteInfoModel.UserPwd);
                    bool result = ftpMgr.Download(SiteInfoModel.LocalPath, SiteInfoModel.RemotePath.EndsWith("/") ? SiteInfoModel.RemotePath : SiteInfoModel.RemotePath + "/", SelectedRmoteFile.FileName, out errorMsg);
                    if (result)
                    {
                        GetLocalFileInfo();
                    }
                }, () =>
                {
                    if (SelectedRmoteFile != null && !SelectedRmoteFile.IsDirectory)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
        /// <summary>
        /// 远程删除
        /// </summary>
        public ICommand mItemRemoteDelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FTPManagerCommon ftpMgr = new FTPManagerCommon(SiteInfoModel.HostUrl, SiteInfoModel.UserName, SiteInfoModel.UserPwd);
                    ftpMgr.DeleteFileName(SiteInfoModel.RemotePath.EndsWith("/") ? SiteInfoModel.RemotePath : SiteInfoModel.RemotePath + "/", SelectedRmoteFile.FileName);
                    ConnectFTP();
                }, () =>
                {
                    if (SelectedRmoteFile != null && !SelectedRmoteFile.IsDirectory)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
        /// <summary>
        /// 远程重命名
        /// </summary>
        public ICommand mItemRemoteReNameClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FTPManagerCommon ftpMgr = new FTPManagerCommon(SiteInfoModel.HostUrl, SiteInfoModel.UserName, SiteInfoModel.UserPwd);
                    ftpMgr.Rename(SiteInfoModel.RemotePath.EndsWith("/") ? SiteInfoModel.RemotePath : SiteInfoModel.RemotePath + "/", SelectedRmoteFile.FileName, SelectedRmoteFile.FileName);
                    ConnectFTP();
                }, () =>
                {
                    if (SelectedRmoteFile != null && !SelectedRmoteFile.IsDirectory)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
        /// <summary>
        /// 查看
        /// </summary>
        public ICommand btnRemoteViewClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ConnectFTP();
                });
            }
        }

        #endregion
        #region 私有方法
        /// <summary>
        /// 获取本地文件列表
        /// </summary>
        private void GetLocalFileInfo()
        {
            if (!string.IsNullOrEmpty(SiteInfoModel.LocalPath))
            {
                LocalFileList.Clear();
                DirectoryInfo localInfo = new DirectoryInfo(SiteInfoModel.LocalPath.EndsWith(":") ? SiteInfoModel.LocalPath + "\\" : SiteInfoModel.LocalPath);
                foreach (DirectoryInfo dirItem in localInfo.GetDirectories())
                {
                    FTPFileInfoModel dirInfo = new FTPFileInfoModel();
                    dirInfo.IsDirectory = true;
                    dirInfo.FileName = dirItem.Name;
                    dirInfo.LastModifyTime = dirItem.LastWriteTime;
                    LocalFileList.Add(dirInfo);
                }
                foreach (FileInfo fileItem in localInfo.GetFiles())
                {
                    FTPFileInfoModel fileInfo = new FTPFileInfoModel();
                    fileInfo.FileName = fileItem.Name;
                    fileInfo.LastModifyTime = fileItem.LastWriteTime;
                    fileInfo.FileSize = fileItem.Length.ToString();
                    fileInfo.FileType = fileItem.Extension;
                    LocalFileList.Add(fileInfo);
                }
                LocalFileList.Insert(0, new FTPFileInfoModel() { FileName = "..", IsDirectory = true });
            }
        }
        /// <summary>
        /// 链接FTP
        /// </summary>
        private void ConnectFTP()
        {
            FTPManagerCommon ftpMgr = new FTPManagerCommon(siteInfoModel.HostUrl, siteInfoModel.UserName, siteInfoModel.UserPwd);
            string[] fileList = ftpMgr.GetFilesDetailList(SiteInfoModel.RemotePath);
            if (fileList != null && fileList.Count() > 0)
            {
                FTPFileInfoModel fileInfo = new FTPFileInfoModel() { FileName = "..", IsDirectory = true };
                RemoteFileList = GetList(fileList);
                RemoteFileList.Insert(0, fileInfo);
            }
        }
        /// <summary>
        /// 按照一定的规则进行字符串截取
        /// </summary>
        /// <param name="s">截取的字符串</param>
        /// <param name="c">查找的字符</param>
        /// <param name="startIndex">查找的位置</param>
        private string CutSubstringFromStringWithTrim(ref string InfoStr, char charKey, int startIndex)
        {
            int posIndex = InfoStr.IndexOf(charKey, startIndex);
            string retString = InfoStr.Substring(0, posIndex);
            InfoStr = (InfoStr.Substring(posIndex)).Trim();
            return retString;
        }
        /// <summary>
        /// 判断文件列表的方式Window方式还是Unix方式
        /// </summary>
        /// <param name="recordList">文件信息列表</param>
        private FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                 && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8
                 && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }
        /// <summary>
        /// 获得文件和目录列表
        /// </summary>
        /// <param name="datastring">FTP返回的列表字符信息</param>
        private ObservableCollection<FTPFileInfoModel> GetList(string[] datastring)
        {
            ObservableCollection<FTPFileInfoModel> myListArray = new ObservableCollection<FTPFileInfoModel>();
            FileListStyle _directoryListStyle = GuessFileListStyle(datastring);
            foreach (string s in datastring)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FTPFileInfoModel fileInfo = new FTPFileInfoModel();
                    fileInfo.FileName = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            fileInfo = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            fileInfo = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (!(fileInfo.FileName == "." || fileInfo.FileName == ".."))
                    {
                        myListArray.Add(fileInfo);
                    }
                }
            }
            return myListArray;
        }
        /// <summary>
        /// 从Unix格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FTPFileInfoModel ParseFileStructFromUnixStyleRecord(string Record)
        {
            FTPFileInfoModel fileInfo = new FTPFileInfoModel();
            string processstr = Record.Trim();
            fileInfo.Flags = processstr.Substring(0, 10);
            fileInfo.IsDirectory = (fileInfo.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            CutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
            fileInfo.FileOwner = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            fileInfo.FileGroup = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            string fileSize = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            fileInfo.FileSize = fileInfo.IsDirectory ? string.Empty : fileSize;
            fileInfo.FileType = fileInfo.IsDirectory ? "文件夹" : "文件";
            string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            if (yearOrTime.IndexOf(":") >= 0)  //time
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }
            string fileTime = CutSubstringFromStringWithTrim(ref processstr, ' ', 8);
            fileInfo.LastModifyTime = DateTime.Parse(fileTime);
            fileInfo.FileName = processstr;   //最后就是名称
            return fileInfo;
        }
        /// <summary>
        /// 从Windows格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FTPFileInfoModel ParseFileStructFromWindowsStyleRecord(string Record)
        {
            FTPFileInfoModel fileInfo = new FTPFileInfoModel();
            string processstr = Record.Trim();
            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            fileInfo.LastModifyTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                fileInfo.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                fileInfo.IsDirectory = false;
            }
            fileInfo.FileName = processstr;
            return fileInfo;
        }
        #endregion
    }
    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }
}
