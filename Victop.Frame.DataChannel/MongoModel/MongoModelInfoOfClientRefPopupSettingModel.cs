using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// 数据引用装配设置
    /// </summary>
    public class MongoModelInfoOfClientRefPopupSettingModel
    {
        /// <summary>
        /// 装配列信息
        /// </summary>
        private MongoModelInfoOfClientRefPopupSettingColumnsModel settingColumns;
        [JsonProperty(PropertyName = "columns")]
        public MongoModelInfoOfClientRefPopupSettingColumnsModel SettingColumns
        {
            get
            {
                if (settingColumns == null)
                    settingColumns = new MongoModelInfoOfClientRefPopupSettingColumnsModel();
                return settingColumns;
            }
            set { settingColumns = value; }
        }
        /// <summary>
        /// 是否只能返回一行数据 1:一行数据，0:多行数据
        /// </summary>
        private int settingSingleRow = 1;
        /// <summary>
        /// 是否只能返回一行数据
        /// </summary>
        public int SettingSingleRow
        {
            get { return settingSingleRow; }
            set { settingSingleRow = value; }
        }
    }
}
