using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    public class AreaMenu : PropertyModelBase
    {
        /// <summary>
        ///区域名称
        /// </summary>
        private string areaName = "我的收藏夹1";
        public string AreaName
        {
            get { return areaName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    areaName = value;
                    RaisePropertyChanged("AreaName");
                }
            }
        }
        /// <summary>
        ///区域唯一ID
        /// </summary>
        public string AreaID
        {
            get; 
            set;
        }
        /// <summary>
        ///区域插件展示方式（big、normal、small）
        /// </summary>
        private string menuForm = "normal";
        public string MenuForm
        {
            get { return menuForm; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    menuForm = value;
                    RaisePropertyChanged("MenuForm");
                }
            }
        }
        /// <summary>
        ///区域距左侧宽度
        /// </summary>
        private double leftSpan = 300;
        public double LeftSpan
        {
            get { return leftSpan; }
            set
            {
                leftSpan = value;
                RaisePropertyChanged("LeftSpan");

            }
        }
        /// <summary>
        ///区域距顶部高度
        /// </summary>
        private double topSpan = 300;
        public double TopSpan
        {
            get { return topSpan; }
            set
            {
                topSpan = value;
                RaisePropertyChanged("TopSpan");

            }
        }
        /// <summary>
        ///区域宽度
        /// </summary>
        private double areaWidth = 300;
        public double AreaWidth
        {
            get { return areaWidth; }
            set
            {
                areaWidth = value;
                RaisePropertyChanged("AreaWidth");

            }
        }
        /// <summary>
        ///区域高度
        /// </summary>
        private double areaHeight = 300;
        public double AreaHeight
        {
            get { return areaHeight; }
            set
            {
                areaHeight = value;
                RaisePropertyChanged("AreaHeight");

            }
        }

        /// <summary>
        /// 插件列表
        /// </summary>
        private ObservableCollection<MenuModel> pluginList;
        public ObservableCollection<MenuModel> PluginList
        {
            get
            {
                if (pluginList == null)
                    pluginList = new ObservableCollection<MenuModel>();
                return pluginList;
            }
            set
            {
                if (pluginList != value)
                {
                    pluginList = value;
                    RaisePropertyChanged("PluginList");
                }
            }
        }
    }
}
