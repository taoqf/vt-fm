using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace ThemeManagerPlugin.Models
{
    public class WallPaperModel : PropertyModelBase
    {
        /// <summary>
        /// 壁纸缩略图
        /// </summary>
        private string wallPreview;
        public string WallPreview
        {
            get { return wallPreview; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    wallPreview = value;
                    RaisePropertyChanged("WallPreview");
                }
            }
        }
        /// <summary>
        /// 壁纸下载图
        /// </summary>
        private string wallDisplay;
        public string WallDisplay
        {
            get { return wallDisplay; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    wallDisplay = value;
                    RaisePropertyChanged("WallDisplay");
                }
            }
        }
        /// <summary>
        /// 壁纸名称
        /// </summary>
        private string wallPaperName;
        public string WllPaperName
        {
            get { return wallPaperName; }
            set
            {
                if (wallPaperName != value)
                {
                    wallPaperName = value;
                    RaisePropertyChanged("WllPaperName");
                }
            }
        }
        /// <summary>
        /// 图片类型
        /// </summary>
        private string imgtype;
        public string WllPaperType
        {
            get { return imgtype; }
            set
            {
                if (imgtype != value)
                {
                    imgtype = value;
                    RaisePropertyChanged("WllPaperType");
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
        /// 壁纸文件存储路径
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
        /// <summary>
        /// 分类编号
        /// </summary>
        private string category_no;
        public string Category_No
        {
            get { return category_no; }
            set
            {
                if (category_no != value)
                {
                    category_no = value;
                    RaisePropertyChanged("Category_No");
                }
            }
        }
    }

}
