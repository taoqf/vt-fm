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
        /// <summary>
        /// 图形生成的oav集合
        /// </summary>
        private Dictionary<string, Dictionary<string, List<dynamic>>> blockOAVDic = new Dictionary<string, Dictionary<string, List<dynamic>>>();
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
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="dataNo">数据编号</param>
        /// <param name="Data">数据</param>
        public void UpdateNodeData(string drawingName, string canvasId, string nodeId, int dataNo, object Data)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "updateNodeData");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("dataNo", dataNo);
                    dic.Add("data", Data);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 更新节点文本
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="nodeText">数据</param>
        public void UpdateNodeText(string drawingName, string canvasId, string nodeId, string nodeText)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "updateNodeText");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("nodeText", nodeText);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeIds">节点id,多个id用逗号隔开</param>
        public void DeleteNode(string drawingName, string canvasId, string nodeIds)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeIds))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "delete");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeIds", nodeIds);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 连接元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="targetNodeId">目标节点id</param>
        /// <param name="lineId">连接线id</param>
        public void ConnectElements(string drawingName, string canvasId, string targetNodeId, string lineId)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(targetNodeId) && !string.IsNullOrEmpty(lineId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "connectElements");
                    dic.Add("canvasId", canvasId);
                    dic.Add("targetNodeId", targetNodeId);
                    dic.Add("lineId", lineId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 克隆元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void CloneElement(string drawingName, string canvasId, string nodeId)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "cloneElement");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 选中元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void SelectElement(string drawingName, string canvasId, string nodeId)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "selectElement");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        ///  断开target线连接
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="lineId">连接线id</param>
        public void BreakTargetConnection(string drawingName, string canvasId, string lineId)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(lineId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "delete");
                    dic.Add("canvasId", canvasId);
                    dic.Add("lineId", lineId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 删除画布
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        public void DeletePage(string drawingName, string canvasId)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(canvasId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "deletePage");
                    dic.Add("canvasId", canvasId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
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
        /// <summary>
        /// 导入模板
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="content"></param>
        public void WebVisioImportTemplateData(string drawingName, object content)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "importTemplateData");
                    dic.Add("content", content);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 更新节点颜色
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="color">颜色值(#FFFFFF/red)</param>
        public void UpdateShapeColor(string drawingName, string canvasId, string nodeId, string color)
        {
            if (!string.IsNullOrEmpty(drawingName) && !string.IsNullOrEmpty(nodeId))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "updateShapeColor");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("color", color);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 改变元素类型
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="drawingName">控件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="targetType">目标类型</param> 
        public void ChangeElement(string drawingName, string canvasId, string nodeId, string targetType)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "changeElement");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("targetType", targetType);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 设置图形缩放比例
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <param name="scaleWidth">宽度缩放倍数</param>
        /// <param name="scaleHeight">高度缩放倍数</param>
        public void SetShapeScale(string drawingName, string canvasId, string nodeId, double scaleWidth, double scaleHeight)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "changeElement");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("scaleWidth", scaleWidth);
                    dic.Add("scaleHeight", scaleHeight);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 生成图形
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="positionX">X轴坐标</param>
        /// <param name="positionY">Y轴坐标</param>
        public void CreateShapeElement(string drawingName, string canvasId, string businessType, double positionX, double positionY)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "changeElement");
                    dic.Add("canvasId", canvasId);
                    dic.Add("businessType", businessType);
                    dic.Add("positionX", positionX);
                    dic.Add("positionY", positionY);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 生成连接线
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="sourceId">源端图形id</param>
        /// <param name="targetId">目标图形id</param>
        public void CreateLineElement(string drawingName, string canvasId, string businessType, string sourceId, string targetId)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "changeElement");
                    dic.Add("canvasId", canvasId);
                    dic.Add("businessType", businessType);
                    dic.Add("sourceId", sourceId);
                    dic.Add("targetId", targetId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 获取画布中的所有元素
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <returns>元素集合</returns>
        public void GetAllElement(string drawingName, string canvasId)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "getAllElement");
                    dic.Add("canvasId", canvasId);
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 获取图形上离开线集合
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void GetOutLinesByShape(string drawingName, string canvasId, string nodeId)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "getLinesByShape");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("lineType", "outLine");
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 获取图形进入线集合
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        public void GetInLinesByShape(string drawingName, string canvasId, string nodeId)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "getLinesByShape");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("lineType", "inLine");
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 获取线源端的图形
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <returns>源端图形</returns>
        public void GetSourceShapeByLine(string drawingName, string canvasId, string nodeId)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "getShapesByLine");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("shapeType", "sourceShape");
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 获取线源端的图形
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="canvasId">画布id</param>
        /// <param name="nodeId">节点id</param>
        /// <returns>源端图形</returns>
        public void GetTargetShapeByLine(string drawingName, string canvasId, string nodeId)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "getShapesByLine");
                    dic.Add("canvasId", canvasId);
                    dic.Add("nodeId", nodeId);
                    dic.Add("shapeType", "targetShape");
                    tc.Excute(dic);
                }
            }
        }
        /// <summary>
        /// 获取导出图形内容
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="oav">接受oav</param>
        public void GetExportData(string drawingName, object oav)
        {
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("MessageType", "getExportData");
                    tc.Excute(dic);
                    dynamic o = oav;
                    o.v = tc.Tag;
                }
            }
        }

        #region 消息转oav
        /// <summary>
        /// 消息转OAV
        /// </summary>
        /// <param name="drawingName">webvisio部件名称</param>
        /// <param name="keyName">消息体键值</param>
        public void MessageToOAV(string drawingName, string keyName)
        {
            string blockName = "webvisio";
            if (!string.IsNullOrEmpty(drawingName))
            {
                TemplateControl tc = (TemplateControl)MainView.FindName(drawingName);
                if (tc != null && tc.Tag != null)
                {
                    List<dynamic> listOAV = new List<dynamic>();
                    Dictionary<string, object> dicParams = tc.Tag as Dictionary<string, object>;
                    if (dicParams != null && dicParams.ContainsKey("MessageType"))
                    {
                        string messageType = dicParams["MessageType"].ToString();
                        object messageContent = dicParams["MessageContent"];
                        if (messageContent != null)
                        {
                            if ((messageType.Equals("beforeDelete") || messageType.Equals("deleted")))
                            {
                                Dictionary<string, object> messageDic = JsonHelper.ToObject<Dictionary<string, object>>(messageContent.ToString());
                                List<Dictionary<string, object>> elements = JsonHelper.ToObject<List<Dictionary<string, object>>>(messageDic["elements"].ToString());
                                MutilShapeToOAV(elements, listOAV, keyName);
                            }
                            else
                            {
                                Dictionary<string, object> messageDic = JsonHelper.ToObject<Dictionary<string, object>>(messageContent.ToString());
                                SingleShapeToOAV(messageDic, listOAV, keyName);
                            }
                        }
                    }
                    else if (dicParams.ContainsKey("shape"))
                    {
                        Dictionary<string, object> sourceShapeDic = JsonHelper.ToObject<Dictionary<string, object>>(dicParams["shape"].ToString());
                        SingleShapeToOAV(sourceShapeDic, listOAV, keyName);
                    }
                    else if (dicParams.ContainsKey("line"))
                    {
                        Dictionary<string, object> sourceShapeDic = JsonHelper.ToObject<Dictionary<string, object>>(dicParams["line"].ToString());
                        SingleShapeToOAV(sourceShapeDic, listOAV, keyName);
                    }
                    else if (dicParams.ContainsKey("sourceShape"))
                    {
                        Dictionary<string, object> sourceShapeDic = JsonHelper.ToObject<Dictionary<string, object>>(dicParams["sourceShape"].ToString());
                        SingleShapeToOAV(sourceShapeDic, listOAV, keyName);
                    }
                    else if (dicParams.ContainsKey("targetShape"))
                    {
                        Dictionary<string, object> sourceShapeDic = JsonHelper.ToObject<Dictionary<string, object>>(dicParams["targetShape"].ToString());
                        SingleShapeToOAV(sourceShapeDic, listOAV, keyName);
                    }
                    else if (dicParams.ContainsKey("elements"))
                    {
                        List<Dictionary<string, object>> elements = JsonHelper.ToObject<List<Dictionary<string, object>>>(dicParams["elements"].ToString());
                        MutilShapeToOAV(elements, listOAV, keyName);
                    }
                    else if (dicParams.ContainsKey("outLines"))
                    {
                        List<Dictionary<string, object>> elements = JsonHelper.ToObject<List<Dictionary<string, object>>>(dicParams["outLines"].ToString());
                        MutilShapeToOAV(elements, listOAV, keyName);
                    }
                    else if (dicParams.ContainsKey("inLines"))
                    {
                        List<Dictionary<string, object>> elements = JsonHelper.ToObject<List<Dictionary<string, object>>>(dicParams["inLines"].ToString());
                        MutilShapeToOAV(elements, listOAV, keyName);
                    }
                    //存在oav清除
                    if (blockOAVDic.ContainsKey(blockName) && blockOAVDic[blockName].ContainsKey(keyName))
                    {
                        try
                        {
                            foreach (dynamic oav in blockOAVDic[blockName][keyName])
                            {
                                MainView.FeiDaoMachine.RemoveFact(oav);
                            }
                            blockOAVDic[blockName].Remove(keyName);
                        }
                        catch (Exception ex)
                        {
                            blockOAVDic[blockName].Remove(keyName);
                            LoggerHelper.Error(ex.ToString());
                        }
                    }
                    if (blockOAVDic.ContainsKey(blockName))
                    {
                        blockOAVDic[blockName].Add(keyName, listOAV);
                    }
                    else
                    {
                        Dictionary<string, List<dynamic>> dicOAV = new Dictionary<string, List<dynamic>>();
                        dicOAV.Add(keyName, listOAV);
                        blockOAVDic.Add(blockName, dicOAV);
                    }
                }
            }
        }
        /// <summary>
        /// 单个元素转oav
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="listOAV"></param>
        /// <param name="keyName"></param>
        private void SingleShapeToOAV(Dictionary<string, object> dic, List<dynamic> listOAV, string keyName)
        {
            if (dic != null && !string.IsNullOrWhiteSpace(keyName))
            {
                string objectName = Guid.NewGuid().ToString();
                if (dic.ContainsKey(keyName))
                {
                    dynamic oav = MainView.FeiDaoMachine.InsertFact(objectName, keyName, dic[keyName]);
                    listOAV.Add(oav);
                }
                else if (dic.ContainsKey("businessData"))
                {
                    Dictionary<string, object> businessDataDic = JsonHelper.ReadJsonObject<Dictionary<string, object>>(dic["businessData"].ToString());
                    if (businessDataDic != null && businessDataDic.ContainsKey(keyName))
                    {
                        dynamic oav = MainView.FeiDaoMachine.InsertFact(objectName, keyName, businessDataDic[keyName]);
                        listOAV.Add(oav);
                    }
                }
            }
        }
        /// <summary>
        /// 多个元素转oav
        /// </summary>
        /// <param name="listDic"></param>
        /// <param name="listOAV"></param>
        /// <param name="keyName"></param>
        private void MutilShapeToOAV(List<Dictionary<string, object>> listDic, List<dynamic> listOAV, string keyName)
        {
            if (listDic != null && !string.IsNullOrWhiteSpace(keyName))
            {
                
                foreach (Dictionary<string, object> dic in listDic)
                {
                    if (dic.ContainsKey("nodeId"))
                    {
                        string objectName = dic["nodeId"].ToString();
                        if (dic.ContainsKey(keyName))
                        {
                            dynamic oav = MainView.FeiDaoMachine.InsertFact(objectName, keyName, dic[keyName]);
                            listOAV.Add(oav);
                        }
                        else
                        {
                            Dictionary<string, object> businessDataDic = JsonHelper.ReadJsonObject<Dictionary<string, object>>(dic["businessData"].ToString());
                            if (businessDataDic != null && businessDataDic.ContainsKey(keyName))
                            {
                                dynamic oav = MainView.FeiDaoMachine.InsertFact(objectName, keyName, businessDataDic[keyName]);
                                listOAV.Add(oav);
                            }
                        }
                    }
                    
                }
            }
        }
        #endregion
    }
}
