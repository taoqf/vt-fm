using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.Models
{
    /// <summary>
    /// 用户角色信息
    /// </summary>
    public class UserRoleInfoModel
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [JsonProperty(PropertyName = "role_no")]
        public string RoleNo { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [JsonProperty(PropertyName = "role_name")]
        public string RoleName { get; set; }
        /// <summary>
        /// 主键Val
        /// </summary>
        [JsonProperty(PropertyName = "pk_val")]
        public string PkVal { get; set; }
        /// <summary>
        /// 检索字段Val
        /// </summary>
        [JsonProperty(PropertyName = "search_field_val")]
        public string SearchFieldVal { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty(PropertyName = "create_time")]
        public long CreateTime { get; set; }
        [JsonProperty(PropertyName = "role_type_id")]
        public string RoleTypeId { get; set; }
        /// <summary>
        /// 角色类型名称
        /// </summary>
        [JsonProperty(PropertyName = "role_type_name")]
        public string RoleTypeName { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        [JsonProperty(PropertyName = "tablename")]
        public string TableName { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }
        /// <summary>
        /// 字段标题
        /// </summary>
        [JsonProperty(PropertyName = "fieldtitle")]
        public string FieldTitle { get; set; }
        /// <summary>
        /// 检索字段
        /// </summary>
        [JsonProperty(PropertyName = "search_field")]
        public string SearchField { get; set; }
        /// <summary>
        /// 检索标题
        /// </summary>
        [JsonProperty(PropertyName = "search_title")]
        public string SearchTitle { get; set; }
    }
}
