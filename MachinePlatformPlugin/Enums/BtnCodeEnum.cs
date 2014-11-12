using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachinePlatformPlugin.Enums
{
    /// <summary>
    /// 按钮编码枚举
    /// </summary>
    public enum BtnCodeEnum
    {
        /// <summary>
        /// 查询
        /// </summary>
        SEARCH = 1,
        /// <summary>
        /// 添加
        /// </summary>
        ADD = 2,
        /// <summary>
        /// 删除
        /// </summary>
        DELETE = 4,
        /// <summary>
        /// 保存
        /// </summary>
        SAVE = 8,
        /// <summary>
        /// 派工
        /// </summary>
        DISPATCH = 16,
        /// <summary>
        /// 开工
        /// </summary>
        WORK=32,
        /// <summary>
        /// 挂起
        /// </summary>
        SUSPEND=64,
        /// <summary>
        /// 解挂
        /// </summary>
        CONTINUE=128,
        /// <summary>
        /// 交工
        /// </summary>
        ENDWORK=256,
        /// <summary>
        /// 驳回
        /// </summary>
        ERJECT=512,
        /// <summary>
        /// 审核
        /// </summary>
        EXAMINE=1024,
        /// <summary>
        /// 问题反馈
        /// </summary>
        FEEDBACK=2048,
        /// <summary>
        /// 上传
        /// </summary>
        UPLOAD=4096,
        /// <summary>
        /// 下载
        /// </summary>
        DOWNLOAD=8192,
        /// <summary>
        /// 生产日志
        /// </summary>
        PROLOG=16384,
        /// <summary>
        /// 问题日志
        /// </summary>
        ISSUELOG=32768
    }
}
