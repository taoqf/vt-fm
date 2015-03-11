using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    public class AreaMenu:ModelBase
    {
        /// <summary>
        ///区域名称
        /// </summary>
        private string areaName = "oftenFun1";
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
        private int leftSpan =300;
        public int LeftSpan
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
        private int topSpan = 300;
        public int TopSpan
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
        private int areaWidth=300;
        public int AreaWidth
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
        private int areaHeight=300;
        public int AreaHeight
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
        public List<Dictionary<string, object>> PluginList
        {
            get;
            set;
        }
    }
}
