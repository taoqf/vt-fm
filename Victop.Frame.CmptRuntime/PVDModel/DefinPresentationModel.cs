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
    /// 组件定义呈现层实体
    /// </summary>
    public class DefinPresentationModel : PropertyModelBase
    {
        /// <summary>
        /// 呈现层Block
        /// </summary>
        private ObservableCollection<PresentationBlockModel> presentationBlocks;
        /// <summary>
        /// 呈现层Block
        /// </summary>
        [JsonProperty(PropertyName = "blocks")]
        public ObservableCollection<PresentationBlockModel> PresentationBlocks
        {
            get
            {
                if (presentationBlocks == null)
                    presentationBlocks = new ObservableCollection<PresentationBlockModel>();
                return presentationBlocks;
            }
            set
            {
                presentationBlocks = value;
                RaisePropertyChanged(() => PresentationBlocks);
            }
        }
    }
}
