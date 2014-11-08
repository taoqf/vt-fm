using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
namespace MachinePlatformPlugin.ViewModels
{
   public class IsueFeedbackViewModel:ModelBase
    {
        #region 字段
        /// <summary>
        /// 用户控件
        /// </summary>
        private VicWindowNormal winIsueFeedback;
        private RichTextBox tboxIssue;
        private RichTextBox tboxSuggest;
        public static string valueInput = "";
        public static string valueOutput;
        #endregion

        #region 属性
        #endregion

        #region 命令

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public ICommand IssueFeedbackViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    try
                    {
                        winIsueFeedback = (VicWindowNormal)x;
                        tboxIssue = (RichTextBox)winIsueFeedback.FindName("tboxDescribe");

                        tboxSuggest = (RichTextBox)winIsueFeedback.FindName("tboxDesSuggest");
                        SetContentValue(valueInput);
                    }
                    catch { }
                });
            }
        }
        #endregion

        #region 确认
        /// <summary>
        /// 确认按钮
        /// </summary>
        public ICommand btnAffirmClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Affirm();
                });
            }
        }

        #endregion

        #region 取消
        /// <summary>
        /// 取消按钮
        /// </summary>
        public ICommand btnCancelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Cancel();
                });
            }
        }

        #endregion

        #endregion

        #region 方法

        #region 确认
        /// <summary>
        /// 确认方法
        /// </summary>
        public void Affirm()
        {
            TextRange txtRange = new TextRange(tboxSuggest.Document.ContentStart, tboxSuggest.Document.ContentEnd);
            valueOutput = txtRange.Text;
            //string valueOutput = System.Windows.Markup.XamlWriter.Save(tboxSuggest.Document);
            if (string.IsNullOrEmpty(valueOutput))
            {
                VicMessageBoxNormal.Show("输入问题为空，请输入反馈问题！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string[] input = valueInput.Split('\n');
            List<string> LInput = new List<string>(input);
            LInput.Insert(2, valueOutput);
            valueInput = string.Empty;
            foreach (string str in LInput)
            {
                valueInput += str+"\n";
            }
            //valueInput += "\n" + valueOutput;
            SetContentValue(valueInput);
            tboxSuggest.Document.Blocks.Clear();
            //List<object> list = new List<object>();
            //System.IO.StringReader sr = new System.IO.StringReader(valueOutput);
            //System.Xml.XmlReader xr = System.Xml.XmlReader.Create(sr);
            //FlowDocument doc = System.Windows.Markup.XamlReader.Load(xr) as FlowDocument;
            //foreach (var m in doc.Blocks)
            //{
            //    list.Add(m);
            //}
            //tboxIssue.Document.Blocks.AddRange(list);
            //tboxSuggest.Document.Blocks.Clear();

            //TextRange txtRange = new TextRange(tboxIssue.Document.ContentStart, tboxIssue.Document.ContentEnd);
            //valueInput = txtRange.Text;
        }
        #endregion

        #region 取消
        /// <summary>
        /// 取消方法
        /// </summary>
        public void Cancel()
        {
            MessageBoxResult result = VicMessageBoxNormal.Show("确定要退出么？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                winIsueFeedback.Close();
            }
        }
        #endregion

        #region 获取问题反馈内容
        /// <summary>
        /// 获取问题反馈内容
        /// </summary>
        /// <param name="value"></param>
        public void SetContentValue(string value)
        {
            tboxIssue.Document = new FlowDocument(new Paragraph(new Run(value)));
            //FlowDocument doc = new FlowDocument();
            //Paragraph p = new Paragraph();  // Paragraph 类似于 html 的 P 标签
            //Run r = new Run(value);      // Run 是一个 Inline 的标签
            //p.Inlines.Add(r);
            //doc.Blocks.Add(p);

            //tboxIssue.Document = doc;
        }
        #endregion

        #endregion
    }
}
