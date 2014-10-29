using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Command;
using ThemeManagerPlugin.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using System.Reflection;
using System.Windows;

namespace ThemeManagerPlugin.ViewModels
{
    public class UCThemesWindowViewModel : ModelBase
    {
        #region 字段&属性
        Storyboard stdEnd;
        private Window portalWindow;
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
        private int _skinNum;
        public int SkinNum
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
                return new RelayCommand <object>((x) =>
                {
                   
                    portalWindow = (Window)x;
                    stdEnd = (Storyboard)portalWindow.Resources["end"];
                    stdEnd.Completed += (c, d) =>
                    {
                       portalWindow.Close();
                    };
                 
                    GetThemeSkinNum();
                    GetDefaultThemeSkin();
                });
            }
        }

        #region 窗体关闭命令
        public ICommand btnCloseClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //MessageBoxResult result = VicMessageBoxNormal.Show("确定要退出么？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    //if (result == MessageBoxResult.Yes)
                    //{
                        stdEnd.Begin();
                    //}
                });
            }
        }
        #endregion

     

        /// <summary>
        /// 获取主题文件夹中默认皮肤路径
        /// </summary>
        private void GetDefaultThemeSkin()
        {
            /*读取配置文件中的默认皮肤路径*/
            string skinDefaultName = ConfigurationManager.AppSettings.Get("skinurl");
            if (this.SystemThemeList.Count > 0)
            {
                foreach (ThemeModel model in SystemThemeList)
                {
                    if (model.SkinPath == skinDefaultName)
                    {
                        SelectedListBoxItem = model;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取主题文件夹中皮肤个数和皮肤列表
        /// </summary>
        private void GetThemeSkinNum()
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "theme", "*Skin.dll");
            SkinNum = files.Count();
            for (int j = 0; j < files.Count(); j++)
            {
                string skinNamespace = Path.GetFileNameWithoutExtension(files[j]);//得到皮肤命名空间
                ThemeModel model = new ThemeModel();
                this.ReflectorInfo(files[j], skinNamespace + ".Skin", model);
                model.SkinPath = @"theme\" + skinNamespace + ".dll";
                SystemThemeList.Add(model);
            }

            if (this.SystemThemeList.Count > 0)
            {
                List<ThemeModel> list = this.SystemThemeList.ToList();
                list.Sort();
                this.SystemThemeList = new ObservableCollection<ThemeModel>(list);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 主题皮肤改变发送消息
        /// </summary>
        /// <param name="model"></param>
        private void ChangeFrameWorkTheme()
        {
            string messageType = "ServerCenterService.ChangeThemeByDll";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
            ServiceParams.Add("SourceName", this.SelectedListBoxItem.ThemeName);
            ServiceParams.Add("SkinPath", this.SelectedListBoxItem.SkinPath);
            contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
            MessageOperation messageOp = new MessageOperation();
            messageOp.SendMessage(messageType, contentDic);

        }

        /// <summary> 
        /// 利用反射获取程序集中类
        /// </summary> 
        private void ReflectorInfo(string dllPath, string skinNamespace, ThemeModel model)
        {
            Assembly ass = Assembly.LoadFrom(dllPath); // 加载程序集 
            Type type = ass.GetType(skinNamespace); // 获取该程序集所包含的类型 
            object obj = Activator.CreateInstance(type);
            model.SkinOrder = (int)type.GetField("SkinOrder").GetValue(obj);
            model.SkinName = type.GetField("SkinName").GetValue(obj).ToString();
            model.ThemeName = type.GetField("ThemeName").GetValue(obj).ToString();
        }

        /// <summary>
        /// 修改默认皮肤
        /// </summary>
        private void UpdateDefaultSkin()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["skinurl"].Value = this.SelectedListBoxItem.SkinPath;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion

        #region 在主题列表选中主题皮肤触发命令
        public ICommand listBoxSystemThemeListSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedListBoxItem != null)
                    {
                        try
                        {
                            ChangeFrameWorkTheme();
                            this.UpdateDefaultSkin();
                        }
                        catch (Exception ex)
                        {
                            VicMessageBoxNormal.Show("Change error: " + ex.Message);
                        }
                    }
                });
            }
        }
        #endregion
    }
}
