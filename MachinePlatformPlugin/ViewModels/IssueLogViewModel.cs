using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Victop.Frame.Components;
using Victop.Server.Controls.Models;
namespace MachinePlatformPlugin.ViewModels
{
   public class IssueLogViewModel:ModelBase
    {

        #region 字段
        /// <summary>
        /// 窗体
        /// </summary>
        private UserControl ucIssueLog;
        /// <summary>
        /// 问题日志DataGrid
        /// </summary>
        private CompntSingleDataGrid comdgrid;
        private RichTextBox tboxIssue;
        public static string value;
        #endregion

        #region 属性
        #endregion

        #region 命令

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public ICommand IssueLogViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    try
                    {
                        ucIssueLog = (UserControl)x;
                        tboxIssue = (RichTextBox)ucIssueLog.FindName("tboxIssueLog");
                        SetContentValue(value);
                        #region 组件装配
                        #endregion
                    }
                    catch { }
                });
            }
        }
        #endregion
        #endregion

        #region 方法

        #region 获取问题反馈内容
        /// <summary>
        /// 获取问题反馈内容
        /// </summary>
        /// <param name="value"></param>
        public void SetContentValue(string value)
        {
            //System.IO.StringReader sr = new System.IO.StringReader(value);
            //System.Xml.XmlReader xr = System.Xml.XmlReader.Create(sr);
            //tboxIssue.Document = (FlowDocument)System.Windows.Markup.XamlReader.Load(xr);
            tboxIssue.Document = new FlowDocument(new Paragraph(new Run(value)));
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();  // Paragraph 类似于 html 的 P 标签
            Run r = new Run(value);      // Run 是一个 Inline 的标签
            p.Inlines.Add(r);
            doc.Blocks.Add(p);
            tboxIssue.Document = doc;
        }
        #endregion

        #endregion
    }
}
