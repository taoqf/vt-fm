using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.Models
{
    /// <summary>
    /// 用户基础信息
    /// </summary>
    public class UserBaseInfoModel
    {
        /// <summary>
        /// 用户表主键
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        [JsonProperty(PropertyName = "usercode")]
        public string UserCode { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [JsonProperty(PropertyName = "user_name")]
        public string UserName { get; set; }
        /// <summary>
        /// 用户拼写
        /// </summary>
        [JsonProperty(PropertyName = "name_spell")]
        public string NameSpell { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [JsonProperty(PropertyName = "is_disabled")]
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 创建时间(时间戳)
        /// </summary>
        [JsonProperty(PropertyName = "create_time")]
        public long CreateTime { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        [JsonProperty(PropertyName = "productid")]
        public string ProductId { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        [JsonProperty(PropertyName = "avatar_path")]
        public string AvatarPath { get; set; }
        /// <summary>
        /// 头像名称
        /// </summary>
        [JsonProperty(PropertyName = "avatar_name")]
        public string AvatarName { get; set; }
        /// <summary>
        /// 头像文件类型
        /// </summary>
        [JsonProperty(PropertyName = "avatar_type")]
        public string AvatarType { get; set; }
        /// <summary>
        /// 邮箱信息
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        [JsonProperty(PropertyName = "last_login_time")]
        public long LastLoginTime { get; set; }
        /// <summary>
        /// 手机是否验证
        /// </summary>
        [JsonProperty(PropertyName = "phone_verified")]
        public bool PhoneVerified { get; set; }
        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        [JsonProperty(PropertyName = "email_verified")]
        public bool EmailVerified { get; set; }
        /// <summary>
        /// 角色信息集合
        /// </summary>
        [JsonProperty(PropertyName = "pub_user_connect")]
        public List<UserRoleInfoModel> RoleList { get; set; }
    }
}
