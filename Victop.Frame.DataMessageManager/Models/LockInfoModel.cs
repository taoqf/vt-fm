using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.DataMessageManager.Enums;

namespace Victop.Frame.DataMessageManager.Models
{
    /// <summary>
    /// 锁定信息实体
    /// </summary>
    public class LockInfoModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public long LockTimeOut { get; set; }
        public string SystemId { get; set; }
        public string ConfigSystemId { get; set; }
        public string SpaceId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 锁定操作
        /// </summary>
        public LockStatusEnum OpenFlag { get; set; }
    }
}
