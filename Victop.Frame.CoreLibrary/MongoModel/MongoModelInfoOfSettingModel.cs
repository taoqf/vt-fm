using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 设置实体
    /// </summary>
    public class MongoModelInfoOfSettingModel
    {
        /// <summary>
        /// 字段设置
        /// </summary>
        private List<MongoModelInfoOfSettingFieldSettingModel> fieldSetting;
        /// <summary>
        /// 字段设置
        /// </summary>
        public List<MongoModelInfoOfSettingFieldSettingModel> FieldSetting
        {
            get
            {
                if (fieldSetting == null)
                    fieldSetting = new List<MongoModelInfoOfSettingFieldSettingModel>();
                return fieldSetting;
            }
            set { fieldSetting = value; }
        }
    }
}
