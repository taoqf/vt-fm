using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    public class GsdocstateModel : PropertyModelBase
    {
        /// <summary>
        /// 单据状态码
        /// </summary>
        private string statusCode;
        /// <summary>
        /// 单据状态码
        /// </summary>
        [JsonProperty(PropertyName = "statuscode")]
        public string StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }

        /// <summary>
        /// 单据状态名
        /// </summary>
        private string statusName;
        /// <summary>
        /// 单据状态名
        /// </summary>
        [JsonProperty(PropertyName = "statusname")]
        public string StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
    }
}
