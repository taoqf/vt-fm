﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.DataChannel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.PublicLib.Helpers;

	/// <summary>
	/// 数据创建器
	/// </summary>
	/// <remarks>数据创建管理器</remarks>
	public class DataCreateManager
	{
		/// <summary>
		/// 依据应答消息创建DataSet并存储到Hashtable
		/// </summary>
        private Hashtable CreateDataSetByReplyMessage(ReplyMessage replyMessageInfo,RequestMessage messageInfo)
		{
            Hashtable hashData = new Hashtable();
            ChannelData channelData = new ChannelData();
            channelData.MessageInfo = messageInfo;
            Dictionary<string, string> contDic = JsonHelper.ToObject<Dictionary<string, string>>(replyMessageInfo.ReplyContent);
            if (contDic == null)
            {
                channelData.DataInfo = CreateMasterDataSet(replyMessageInfo.ReplyContent);
            }
            else
            {
                channelData.DataInfo = CreateMasterDataSet(contDic["Result"]);
            }
            hashData.Add("Data", channelData);
            return hashData;
            
		}

        private DataSet CreateMasterDataSet(string dataXml)
        {
            DataSet ReplyData = new DataSet();
            if (string.IsNullOrEmpty(dataXml))
            {
                return ReplyData;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dataXml);

                XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("DATASET");
                foreach (XmlNode node in xmlNodeList)
                {
                    DataTable dt = new DataTable();
                    string datasetId = node.Attributes["datasetid"].Value;
                    XmlNodeList nodexmlNodeList = node.ChildNodes;
                    foreach (XmlNode childNode in nodexmlNodeList)
                    {
                        if (childNode.Name.Equals("DATAPACKET"))
                        {
                            dt = CreateDataSchema(childNode);
                            dt = SetDataTableRow(dt, childNode);
                        }
                    }
                    dt.TableName = datasetId;
                    ReplyData.Tables.Add(dt);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            ReplyData.AcceptChanges();
            return ReplyData;
        }
        private DataTable SetDataTableRow(DataTable dt, XmlNode xmlNode)
        {
            dt.Clear();
            XmlNodeList nodeList = xmlNode.SelectSingleNode("ROWDATA").SelectNodes("ROW");
            foreach (XmlNode item in nodeList)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (item.Attributes[dc.ColumnName] == null)
                        continue;
                    if (dc.DataType == typeof(DateTime))
                    {
                        dr[dc] = item.Attributes[dc.ColumnName].Value.Replace("T", " ").Substring(0, 19);
                    }
                    else
                    {
                        dr[dc] = item.Attributes[dc.ColumnName].Value;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 创建Data模式
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private DataTable CreateDataSchema(XmlNode xmlNode)
        {
            DataTable dt = new DataTable();
            XmlNodeList fieldList = xmlNode.SelectSingleNode("METADATA").SelectSingleNode("FIELDS").SelectNodes("FIELD");
            foreach (XmlNode item in fieldList)
            {
                #region 临时处理SOA返回表结构出现重复列的情况
                if (dt.Columns.Contains(item.Attributes["attrname"].Value))
                    continue;
                #endregion
                dt.Columns.Add(SetDataColumnInfo(item));
            }
            return dt;
        }
        /// <summary>
        /// 创建DataTable中的列
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private DataColumn SetDataColumnInfo(XmlNode xmlNode)
        {
            DataColumn dc = new DataColumn();
            dc.ColumnName = xmlNode.Attributes["attrname"].Value;
            string fieldType = xmlNode.Attributes["fieldtype"].Value;
            switch (fieldType)
            {
                case "i8":
                case "i4":
                    dc.DataType = typeof(int);
                    break;
                case "dateTime":
                    dc.DataType = typeof(DateTime);
                    break;
                case "string":
                default:
                    dc.DataType = typeof(string);
                    dc.MaxLength = Convert.ToInt32(xmlNode.Attributes["WIDTH"].Value);
                    break;
            }
            return dc;
        }

        private DataSet CreateDataSetByXml(string dataXml)
        {
            DataSet ReplyData=new DataSet();
            if (string.IsNullOrEmpty(dataXml))
            {
                return ReplyData;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dataXml);
                XmlNodeList xmlNodeList = doc.GetElementsByTagName("DATASET");
                foreach (XmlNode node in xmlNodeList)
                {
                    string datasetId = node.Attributes["datasetid"].Value;
                    XmlNodeList nodexmlNodeList = node.ChildNodes;
                    foreach (XmlNode childNode in nodexmlNodeList)
                    {
                        if (childNode.Name.Equals("DATAPACKET"))
                        {
                            DataTable table = DataTool.ConvertXMLToDataTable(childNode, "ROW");
                            table.TableName = datasetId;
                            ReplyData.Tables.Add(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ReplyData;
        }

		/// <summary>
		/// 根据DataSet完善应答消息体
		/// </summary>
		public virtual RequestMessage CreateMessageByDataSet(string channelId)
		{
			throw new System.NotImplementedException(); //TODO:方法实现
		}

		/// <summary>
		/// 获取xml格式数据
		/// </summary>
		public virtual string GetXmlData(string channelId)
		{
            DataChannelManager dataManager = new DataChannelManager();
            Hashtable hashData = dataManager.GetData(channelId);
            ChannelData channelData = hashData["Data"] as ChannelData;
            DataSet requestDs = channelData.DataInfo;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\"  encoding=\"utf-8\"?>");
            stringBuilder.Append("<DATA>");
            foreach (DataTable mastDt in requestDs.Tables)
            {
                if (!mastDt.TableName.Equals("masterdata"))
                    continue;
                stringBuilder.Append("<DATASET id=\"").Append(mastDt.TableName).Append("\">");
                stringBuilder.Append("<DATAPACKET Version=\"").Append("2.0").Append("\">");
                stringBuilder.Append("<METADATA>");
                //stringBuilder.Append("<FIELDS>");
                //foreach (DataColumn mastDc in mastDt.Columns)
                //{
                //    string fieldType = "string";
                //    switch (mastDc.DataType.Name)
                //    {
                //        case "Int16":
                //            fieldType = "i4";
                //            break;
                //        case "Int32":
                //        case "Int64":
                //            fieldType="i8";
                //            break;
                //        case "DateTime":
                //            fieldType = "dateTime";
                //            break;
                //        case "String":
                //        default:
                //            fieldType = "string";
                //            break;
                //    }
                //    stringBuilder.AppendFormat("<FIELD attrname='{0}' fieldtype='{1}' WIDTH='50' /> ",mastDc.ColumnName,fieldType);
                //}
                //stringBuilder.Append("</FIELDS>");
                stringBuilder.Append("</METADATA>");
                stringBuilder.Append("<ROWDATA>");
                foreach (DataRow mastDr in mastDt.Rows)
                {
                    if (mastDr.RowState == DataRowState.Added)
                    {
                        GetDataRowXml(stringBuilder, mastDt, mastDr,"4");
                    }
                    else if (mastDr.RowState == DataRowState.Modified)
                    {
                        GetDataRowXml(stringBuilder, mastDt, mastDr, "8");
                    }
                    else if (mastDr.RowState == DataRowState.Deleted)
                    {
                        GetDataRowXml(stringBuilder, mastDt, mastDr, "2",true);
                    }
                }
                stringBuilder.Append("</ROWDATA>");
                stringBuilder.Append("</DATAPACKET>");
                stringBuilder.Append("</DATASET>");
            }
            stringBuilder.Append("</DATA>");
            return stringBuilder.ToString();
		}

        private static void GetDataRowXml(StringBuilder stringBuilder, DataTable mastDt, DataRow mastDr,string rowState,bool isDelete=false)
        {
            try
            {
                stringBuilder.Append("<ROW ");
                stringBuilder.AppendFormat("RowState='{0}'", rowState);
                foreach (DataColumn mastDc in mastDt.Columns)
                {
                    if (mastDc.DataType == typeof(DateTime))
                    {
                        if (isDelete)
                        {
                            stringBuilder.AppendFormat(" {0}='{1}' ", mastDc.ColumnName, ((DateTime)mastDr[mastDc.ColumnName, DataRowVersion.Original]).ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                        else
                        {
                            stringBuilder.AppendFormat(" {0}='{1}' ", mastDc.ColumnName, ((DateTime)mastDr[mastDc.ColumnName]).ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }
                    else
                    {
                        if (isDelete)
                        {
                            stringBuilder.AppendFormat(" {0}='{1}' ", mastDc.ColumnName, mastDr[mastDc.ColumnName, DataRowVersion.Original]);
                        }
                        else
                        {
                            stringBuilder.AppendFormat(" {0}='{1}' ", mastDc.ColumnName, mastDr[mastDc.ColumnName]);
                        }
                    }
                }
                stringBuilder.AppendFormat("/>");
            }
            catch (Exception ex)
            {
 
            }
        }

		/// <summary>
		/// 提交通道数据
		/// </summary>
		public virtual bool CommitChannelData(string channelId)
		{
            try
            {
                DataChannelManager dataManager = new DataChannelManager();
                Hashtable hashData = dataManager.GetData(channelId);
                ChannelData channelData = hashData["Data"] as ChannelData;
                DataSet requestDs = channelData.DataInfo;
                if (requestDs != null)
                {
                    requestDs.AcceptChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
		}

		/// <summary>
		/// 发送应答消息(连接器使用)
		/// </summary>
        public virtual ReplyMessage SendReplyMessage(ReplyMessage replyMessageInfo, RequestMessage messageInfo)
		{
            Hashtable replyHashtable = CreateDataSetByReplyMessage(replyMessageInfo,messageInfo);
            DataChannelManager dataManager = new DataChannelManager();
            if (dataManager.AddData(replyMessageInfo.MessageId, replyHashtable))
            {
                replyMessageInfo.DataChannelId = replyMessageInfo.MessageId;
                replyMessageInfo.ReplyContent = string.Empty;
            }
            return replyMessageInfo;
		}

		/// <summary>
		/// 发送消息(到消息管理器)
		/// </summary>
		public virtual ReplyMessage SendMessage(string messageInfo)
		{
			throw new System.NotImplementedException(); //TODO:方法实现
		}

	}
}

