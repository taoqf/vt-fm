using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// 数据原子操作
    /// </summary>
    public class WebVisioAtOperation
    {
        #region 私有定义
        private TemplateControl MainView;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public WebVisioAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }
        /// <summary>
        /// 更新节点数据
        /// </summary>
        /// <param name="drawingName">控件名称</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="dataNo">数据编号</param>
        /// <param name="Data">数据</param>
        public void UpdateNodeData(string drawingName, string nodeId, int dataNo, object Data)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string,object> dic = new Dictionary<string,object>();
                    dic.Add("MessageType", "updateNodeData");
                    dic.Add("nodeId",nodeId);
                    dic.Add("dataNo",dataNo);
                    dic.Add("data",Data);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="drawingName">控件名称</param>
        /// <param name="nodeId">节点id</param>
        public void DeleteNode(string drawingName, string nodeId)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "delete");
                    dic.Add("nodeId", nodeId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 保存画布
        /// </summary>
        /// <param name="drawingName">控件名称</param>
        public void SaveDrawingVisio(string drawingName)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "importData");
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="drawingName">控件名称</param>
        /// <param name="content"></param>
        public void WebVisioImportData(string drawingName, object content)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "importData");
                    dic.Add("content", content);
                    tc.Excute(dic);
                }
            }
        }
    }
}
