using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace ThemeManagerPlugin.Models
{
    public class OnLineModel : PropertyModelBase
    {
        /// <summary>
        /// 皮肤编号
        /// </summary>
        private string onLineNo;
        public string OnLineNo
        {
            get { return onLineNo; }
            set
            {
                if (onLineNo != value)
                {
                    onLineNo = value;
                    RaisePropertyChanged("OnLineNo");
                }
            }
        }
        /// <summary>
        /// 状态改变
        /// </summary>
        private int stateChange;
        public int StateChange
        {
            get { return stateChange; }
            set
            {
                if (stateChange != value)
                {
                    stateChange = value;
                    RaisePropertyChanged("StateChange");
                }
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        private bool stateType;
        public bool StateType
        {
            get { return stateType; }
            set
            {
                if (stateType != value)
                {
                    stateType = value;
                    RaisePropertyChanged("StateType");
                }
            }
        }
        /// <summary>
        /// 皮肤名称
        /// </summary>
        private string onLineName;
        public string OnLineName
        {
            get { return onLineName; }
            set
            {
                if (onLineName != value)
                {
                    onLineName = value;
                    RaisePropertyChanged("OnLineName");
                }
            }
        }
        /// <summary>
        /// 分类编号
        /// </summary>
        private string typeNo;
        public string TypeNo
        {
            get { return typeNo; }
            set
            {
                if (typeNo != value)
                {
                    typeNo = value;
                    RaisePropertyChanged("typeNo");
                }
            }
        }
        /// <summary>
        /// 皮肤缩略图
        /// </summary>
        private string onLinePreview;
        public string OnLinePreview
        {
            get { return onLinePreview; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    onLinePreview = value;
                    RaisePropertyChanged("OnLinePreview");
                }
            }
        }
      
       
        /// <summary>
        /// 图片类型
        /// </summary>
        private string onLineImgtype;
        public string OnLineImgType
        {
            get { return onLineImgtype; }
            set
            {
                if (onLineImgtype != value)
                {
                    onLineImgtype = value;
                    RaisePropertyChanged("OnLineImgType");
                }
            }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    RaisePropertyChanged("FileName");
                }
            }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        private string filetype;
        public string FileType
        {
            get { return filetype; }
            set
            {
                if (filetype != value)
                {
                    filetype = value;
                    RaisePropertyChanged("FileType");
                }
            }
        }
        /// <summary>
        /// 皮肤文件存储路径
        /// </summary>
        private string filepath;
        public string FilePath
        {
            get { return filepath; }
            set
            {
                if (filepath != value)
                {
                    filepath = value;
                    RaisePropertyChanged("FilePath");
                }
            }
        } 
    }
}
