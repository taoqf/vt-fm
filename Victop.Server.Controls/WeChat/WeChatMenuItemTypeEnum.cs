using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Server.Controls.WeChat
{
    public enum WeChatMenuItemTypeEnum
    {
        /// <summary>
        /// 点击推事件
        /// </summary>
        CLICK,
        /// <summary>
        /// 跳转URL
        /// </summary>
        VIEW,
        /// <summary>
        /// 扫码推事件
        /// </summary>
        SCANCODE_PUSH,
        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        SCANCODE_WAITMSG,
        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        PIC_SYSPHOTO,
        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        PIC_PHOTO_OR_ALBUM,
        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        PIC_WEIXIN,
        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        LOCATION_SELECT
    }
}
