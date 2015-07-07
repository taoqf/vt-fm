using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 组件定义呈现层实体
    /// </summary>
    public class DefinPresentationModel
    {
        /// <summary>
        /// 呈现层Block
        /// </summary>
        [JsonProperty(PropertyName = "blocks")]
        private ObservableCollection<PresentationBlockModel> presentationBlocks;
        /// <summary>
        /// 呈现层Block
        /// </summary>
        public ObservableCollection<PresentationBlockModel> PresentationBlocks
        {
            get
            {
                if (presentationBlocks == null)
                    presentationBlocks = new ObservableCollection<PresentationBlockModel>();
                return presentationBlocks;
            }
            set { presentationBlocks = value; }
        }
    }
}
