﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 组件定义
    /// </summary>
    public class CompntDefinModel:PropertyModelBase
    {
        /// <summary>
        /// 组件视图层
        /// </summary>
        private ObservableCollection<DefinViewsModel> compntViews;
        /// <summary>
        /// 组件视图
        /// </summary>
        [JsonProperty(PropertyName = "views")]
        public ObservableCollection<DefinViewsModel> CompntViews
        {
            get
            {
                if (compntViews == null)
                    compntViews = new ObservableCollection<DefinViewsModel>();
                return compntViews;
            }
            set
            {
                if (compntViews != value)
                {
                    compntViews = value;
                    RaisePropertyChanged(()=>CompntViews);
                }
            }
        }
        /// <summary>
        /// 组件展示层
        /// </summary>
        private DefinPresentationModel compntPresentation;
        /// <summary>
        /// 组件展示层
        /// </summary>
        [JsonProperty(PropertyName = "presentation")]
        public DefinPresentationModel CompntPresentation
        {
            get
            {
                if (compntPresentation == null)
                    compntPresentation = new DefinPresentationModel();
                return compntPresentation;
            }
            set
            {
                if (compntPresentation != value)
                {
                    compntPresentation = value;
                    RaisePropertyChanged(()=> CompntPresentation);
                }
            }
        }
    }
}
