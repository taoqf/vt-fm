using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ThemeManagerPlugin.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace ThemeManagerPlugin.ViewModels
{
    public class UCThemesWindowViewModel : ModelBase
    {
        #region 字段&属性
        string ThemePath;//当前主题路径
        /// <summary>皮肤列表 </summary>
        private ObservableCollection<ThemeModel> _systemThemeList;
        public ObservableCollection<ThemeModel> SystemThemeList
        {
            get
            {
                if (_systemThemeList == null)
                    _systemThemeList = new ObservableCollection<ThemeModel>();
                return _systemThemeList;
            }
            set
            {
                if (_systemThemeList != value)
                {
                    _systemThemeList = value;
                    RaisePropertyChanged("SystemThemeList");
                }
            }
        }
        /// <summary>当前选中主题 </summary>
        private ThemeModel _selectedItem;
        public ThemeModel SelectedListBoxItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged("SelectedListBoxItem");
                }
            }
        }

        /// <summary>N款皮肤 </summary>
        private int  _skinNum;
        public int  SkinNum
        {
            get { return _skinNum; }
            set
            {
                if (_skinNum != value)
                {
                    _skinNum = value;
                    RaisePropertyChanged("SkinNum");
                }
            }
        }
       
        #endregion
        #region 窗体加载命令
        /// <summary>窗体加载命令 </summary>
        public ICommand gridMainLoadedCommand
        {
            get
            {
                return new RelayCommand(( ) =>
                {

                    GetThemeSkinNum();/*获取主题文件夹中皮肤个数和皮肤列表*/
                    GetDefaultThemeSkinPath();/*获取主题文件夹中默认皮肤路径*/
                });
            }
        }
        private void GetDefaultThemeSkinPath() /*获取主题文件夹中默认皮肤路径*/
        {
            ThemePath = ConfigurationManager.AppSettings.Get("skinurl"); /*读取配置文件中的路径值theme/SkinDefault/Resources.xaml*/
            foreach (ThemeModel model in SystemThemeList)
            {
                if (model.ThemeName == ThemePath)
                {
                    SelectedListBoxItem = model;
                }
            }
        }
      
        private void GetThemeSkinNum()/*获取主题文件夹中皮肤个数和皮肤列表*/
        {
            string[] directories = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "theme");
            SkinNum = directories.Count();
            for (int i = 0; i < directories.Count(); i++)
            {
                string[] ThemePathpart = directories[i].Split("\\".ToCharArray());
                string Themename = ThemePathpart[ThemePathpart.Length -1].ToString();//得到皮肤名字
                ThemeModel skin = new ThemeModel();
                skin.SkinName = Themename;
                skin.ThemeName = "theme/" + Themename + "/Resources.xaml";
                SystemThemeList.Add(skin);
            }
        }
        #endregion
        #region
        // 在主题列表选中主题皮肤触发命令 
        public ICommand listBoxSystemThemeListSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedListBoxItem != null)
                    {
                        ChangeFrameWorkTheme(SelectedListBoxItem);
                    }
                });
            }
        }
        // 主题皮肤改变发送消息
        private void ChangeFrameWorkTheme(ThemeModel model)
        {
            string messageType = "ServerCenterService.ChangeTheme";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
            ServiceParams.Add("SourceName", model.ThemeName);
            contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
            MessageOperation messageOp = new MessageOperation();
            messageOp.SendMessage(messageType, contentDic);

        }
        #endregion
    }
}
