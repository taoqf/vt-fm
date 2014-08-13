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
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.DataChannel.Enums;
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
        private Hashtable CreateDataSetByReplyMessage(ReplyMessage replyMessageInfo, RequestMessage messageInfo)
        {
            Hashtable hashData = new Hashtable();
            ChannelData channelData = new ChannelData();
            channelData.MessageInfo = messageInfo;
            Dictionary<string, string> contDic = JsonHelper.ToObject<Dictionary<string, string>>(replyMessageInfo.ReplyContent);
            if (messageInfo.MessageType.Equals("DataChannelService.loadDataByModelAsync"))
            {
                channelData.DataInfo = CreateDataSetByJSON(replyMessageInfo.ReplyContent);
            }
            else if (messageInfo.MessageType.Equals("DataChannelService.getFormReferenceSpecial"))
            {
                channelData.DataInfo = CreateDataSet(replyMessageInfo.ReplyContent, SOADataTypeEnum.DATAREFERENCE);
            }
            else
            {
                if (contDic == null)
                {
                    channelData.DataInfo = CreateDataSet(replyMessageInfo.ReplyContent, SOADataTypeEnum.MASTERDATA);
                }
                else
                {
                    channelData.DataInfo = CreateDataSet(contDic["Result"], SOADataTypeEnum.MODELDATA);
                }
            }
            hashData.Add("Data", channelData);
            return hashData;

        }



        /// <summary>
        /// 根据Xml格式文本创建DataSet
        /// </summary>
        /// <param name="dataXml"></param>
        /// <returns></returns>
        private DataSet CreateDataSet(string dataXml, SOADataTypeEnum typeEnum)
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
                if (doc.DocumentElement.SelectSingleNode("treejson") != null)
                {
                    XmlNodeList jsonNodeList = doc.DocumentElement.SelectSingleNode("treejson").SelectNodes("DATASET");
                    foreach (XmlNode item in jsonNodeList)
                    {
                        DataTable dt = new DataTable(item.Attributes["datasetid"].Value);
                        dt.Columns.Add("treejson");
                        DataRow dr = dt.NewRow();
                        dr["treejson"] = item.InnerText;
                        dt.Rows.Add(dr);
                        ReplyData.Tables.Add(dt);
                    }
                }
                XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("DATASET");
                BuildModeDataReferenceTable(ReplyData, xmlNodeList);
                switch (typeEnum)
                {
                    case SOADataTypeEnum.MODELDATA:
                        ModelDataOrganize(ReplyData, xmlNodeList);
                        break;
                    case SOADataTypeEnum.MASTERDATA:
                        BuildMasterFieldTable(ReplyData, xmlNodeList, "masterfield");
                        MasterDataOrganize(ReplyData, xmlNodeList, "masterfield");
                        break;
                    case SOADataTypeEnum.BUSINESSRULE:
                        break;
                    case SOADataTypeEnum.DATAREFERENCE:
                        BuildMasterFieldTable(ReplyData, xmlNodeList, "returnfield");
                        MasterDataOrganize(ReplyData, xmlNodeList, "returnfield");
                        break;
                    default:
                        break;
                }


            }
            catch (Exception)
            {

                throw;
            }
            ReplyData.AcceptChanges();
            return ReplyData;
        }
        /// <summary>
        /// 模型数据组织
        /// </summary>
        /// <param name="ReplyData"></param>
        /// <param name="xmlNodeList"></param>
        private void ModelDataOrganize(DataSet ReplyData, XmlNodeList xmlNodeList)
        {
            foreach (XmlNode node in xmlNodeList)
            {
                DataTable dt = new DataTable();
                string datasetId = node.Attributes["datasetid"].Value;
                if (ReplyData.Tables.Contains(datasetId))
                    continue;
                XmlNodeList nodexmlNodeList = node.ChildNodes;
                Dictionary<string, string> columnDes = new Dictionary<string, string>();
                foreach (XmlNode childNode in nodexmlNodeList)
                {
                    if (childNode.Name.Equals("fieldinfo"))
                    {
                        columnDes = GetColumnCaption(childNode);
                    }
                    if (childNode.Name.Equals("DATAPACKET"))
                    {
                        if (ReplyData.Tables.Contains("gmDatareference"))
                        {
                            dt = CreateDataSchema(childNode, columnDes, ReplyData.Tables["gmDatareference"]);
                        }
                        else
                        {
                            dt = CreateDataSchema(childNode, columnDes);
                        }
                        dt = SetDataTableRow(dt, childNode);
                    }
                }
                dt.TableName = datasetId;
                ReplyData.Tables.Add(dt);
            }
        }
        /// <summary>
        /// 主档数据组织
        /// </summary>
        /// <param name="ReplyData"></param>
        /// <param name="xmlNodeList"></param>
        private void MasterDataOrganize(DataSet ReplyData, XmlNodeList xmlNodeList, string fieldTableName)
        {
            foreach (XmlNode node in xmlNodeList)
            {
                DataTable dt = new DataTable();
                string datasetId = node.Attributes["datasetid"].Value;
                if (ReplyData.Tables.Contains(datasetId))
                    continue;
                XmlNodeList nodexmlNodeList = node.ChildNodes;
                Dictionary<string, string> columnDic = new Dictionary<string, string>();
                if (ReplyData.Tables.Contains(fieldTableName))
                {
                    foreach (DataRow dr in ReplyData.Tables[fieldTableName].Rows)
                    {
                        Dictionary<string, object> rowDic = new Dictionary<string, object>();
                        foreach (DataColumn dc in ReplyData.Tables[fieldTableName].Columns)
                        {
                            rowDic.Add(dc.ColumnName, dr[dc.ColumnName]);
                        }
                        columnDic.Add(dr["columnid"].ToString().ToLower(), JsonHelper.ToJson(rowDic));
                    }
                }
                foreach (XmlNode childNode in nodexmlNodeList)
                {
                    if (childNode.Name.Equals("DATAPACKET"))
                    {
                        if (ReplyData.Tables.Contains("gmDatareference"))
                        {
                            dt = CreateDataSchema(childNode, columnDic, ReplyData.Tables["gmDatareference"]);
                        }
                        else
                        {
                            dt = CreateDataSchema(childNode, columnDic);
                        }
                        dt = SetDataTableRow(dt, childNode);
                    }
                }
                dt.TableName = datasetId;
                ReplyData.Tables.Add(dt);
            }
        }
        /// <summary>
        /// 构建模型数据引用Table
        /// </summary>
        /// <param name="ReplyData"></param>
        /// <param name="xmlNodeList"></param>
        private void BuildModeDataReferenceTable(DataSet ReplyData, XmlNodeList xmlNodeList)
        {
            foreach (XmlNode item in xmlNodeList)
            {
                if (item.Attributes["datasetid"].Value.Equals("gmDatareference"))
                {
                    DataTable gmdt = new DataTable();
                    foreach (XmlNode childNode in item.ChildNodes)
                    {
                        gmdt = CreateDataSchema(childNode, null);
                        gmdt = SetDataTableRow(gmdt, childNode);
                    }
                    gmdt.TableName = "gmDatareference";
                    ReplyData.Tables.Add(gmdt);
                }
            }
        }

        /// <summary>
        /// 构建主档数据字段表
        /// </summary>
        /// <param name="ReplyData"></param>
        /// <param name="xmlNodeList"></param>
        private void BuildMasterFieldTable(DataSet ReplyData, XmlNodeList xmlNodeList, string fieldTableName)
        {
            foreach (XmlNode item in xmlNodeList)
            {
                if (item.Attributes["datasetid"].Value.Equals(fieldTableName))
                {
                    DataTable masterFieldDt = new DataTable();
                    foreach (XmlNode childNode in item.ChildNodes)
                    {
                        masterFieldDt = CreateDataSchema(childNode, null);
                        masterFieldDt = SetDataTableRow(masterFieldDt, childNode);
                    }
                    masterFieldDt.TableName = fieldTableName;
                    ReplyData.Tables.Add(masterFieldDt);
                }
            }
        }

        /// <summary>
        /// 读取模型取数字段描述
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetColumnCaption(XmlNode xmlNode)
        {
            Dictionary<string, string> columnDic = new Dictionary<string, string>();
            XmlNodeList nodeList = xmlNode.SelectSingleNode("DATAPACKET").SelectSingleNode("ROWDATA").SelectNodes("ROW");
            foreach (XmlNode item in nodeList)
            {
                Dictionary<string, object> RowDic = new Dictionary<string, object>();
                for (int i = 0; i < item.Attributes.Count; i++)
                {
                    RowDic.Add(item.Attributes[i].Name, item.Attributes[i].Value);
                }
                if (item.Attributes["columnid"] == null || string.IsNullOrEmpty(item.Attributes["columnid"].Value))
                {

                    columnDic.Add(item.Attributes["columnid2"].Value, JsonHelper.ToJson(RowDic));
                }
                else
                {
                    columnDic.Add(item.Attributes["columnid"].Value, JsonHelper.ToJson(RowDic));
                }
            }
            return columnDic;
        }

        /// <summary>
        /// 根据JSON格式文本创建DataSet
        /// </summary>
        /// <param name="dataJSON"></param>
        /// <returns></returns>
        private DataSet CreateDataSetByJSON(string dataJSON)
        {
            DataSet ds = new DataSet();
            string resultStr = JsonHelper.ReadJsonString(dataJSON, "Result");
            string metaStr = JsonHelper.ReadJsonString(resultStr, "meta");
            string dataStr = JsonHelper.ReadJsonString(resultStr, "data");
            List<object> metaList = JsonHelper.ToObject<List<object>>(metaStr);
            List<object> dataList = JsonHelper.ToObject<List<object>>(dataStr);
            Dictionary<string, object> metaDic = JsonHelper.ToObject<Dictionary<string, object>>(metaList[0].ToString());
            Dictionary<string, object> dataDic = JsonHelper.ToObject<Dictionary<string, object>>(dataList[0].ToString());
            string datasetsStr = metaDic["datasets"].ToString();
            string datarowsStr = dataDic["datarows"].ToString();
            List<object> datatableList = JsonHelper.ToObject<List<object>>(datasetsStr);
            List<object> datarowsList = JsonHelper.ToObject<List<object>>(datarowsStr);
            for (int i = 0; i < datatableList.Count; i++)
            {
                Dictionary<string, object> tableInfoDic = JsonHelper.ToObject<Dictionary<string, object>>(datatableList[i].ToString());
                string keyField = tableInfoDic["keys"].ToString();
                DataTable dt = new DataTable(tableInfoDic["datasetid"].ToString());
                string fieldStr = tableInfoDic["fields"].ToString();
                List<object> fieldList = JsonHelper.ToObject<List<object>>(fieldStr);
                for (int j = 0; j < fieldList.Count; j++)
                {
                    Dictionary<string, object> fieldInfo = JsonHelper.ToObject<Dictionary<string, object>>(fieldList[j].ToString());
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = fieldInfo["fieldid"].ToString();
                    dc.Caption = fieldInfo["fieldcaption"].ToString();
                    switch (fieldInfo["datatype"].ToString())
                    {
                        case "日期":
                            dc.DataType = typeof(DateTime);
                            break;
                        case "GUID":
                            dc.DataType = typeof(Guid);
                            break;
                        case "整型":
                            dc.DataType = typeof(Int32);
                            break;
                        case "字符":
                        default:
                            dc.DataType = typeof(String);
                            break;
                    }
                    dt.Columns.Add(dc);
                    if (fieldInfo["fieldid"].ToString().Equals(keyField))
                    {
                        dt.PrimaryKey = new DataColumn[] { dc };
                    }
                }
                for (int j = 0; j < datarowsList.Count; j++)
                {
                    Dictionary<string, object> datarowInfo = JsonHelper.ToObject<Dictionary<string, object>>(datarowsList[j].ToString());
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dataCol in dt.Columns)
                    {
                        if (datarowInfo[dataCol.ColumnName] == null)
                            continue;
                        if (dataCol.DataType == typeof(DateTime))
                        {
                            dr[dataCol] = datarowInfo[dataCol.ColumnName].ToString().Replace("T", " ").Substring(0, 19);
                        }
                        else
                        {
                            dr[dataCol] = datarowInfo[dataCol.ColumnName];
                        }
                    }
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
            }
            ds.AcceptChanges();
            return ds;
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
        private DataTable CreateDataSchema(XmlNode xmlNode, Dictionary<string, string> fieldDc, DataTable gmDt = null)
        {
            DataTable dt = new DataTable();
            List<DataColumn> keyDcList = new List<DataColumn>();
            XmlNodeList fieldList = xmlNode.SelectSingleNode("METADATA").SelectSingleNode("FIELDS").SelectNodes("FIELD");
            foreach (XmlNode item in fieldList)
            {
                #region 临时处理SOA返回表结构出现重复列的情况
                if (dt.Columns.Contains(item.Attributes["attrname"].Value))
                    continue;
                #endregion
                DataColumn dc = SetDataColumnInfo(item);
                if (gmDt != null && gmDt.Rows.Count > 0)
                {
                    DataRow[] drs = gmDt.Select("columnid='" + dc.ColumnName + "'");
                    if (drs.Count() > 0)
                    {
                        dc.ExtendedProperties.Add("DataReference", dc.ColumnName);
                    }
                }
                if (fieldDc != null && fieldDc.Count > 0)
                {
                    string fieldKey = item.Attributes["attrname"].Value;
                    if (fieldDc.ContainsKey(fieldKey))
                    {
                        Dictionary<string, object> rowDic = JsonHelper.ToObject<Dictionary<string, object>>(fieldDc[fieldKey]);
                        dc.Caption = rowDic["columncaption"].ToString();
                        if (rowDic.ContainsKey("ismust") && rowDic["ismust"] != null)
                        {
                            dc.AllowDBNull = !rowDic["ismust"].ToString().Equals("0");
                        }
                        else
                        {
                            dc.AllowDBNull = true;
                        }
                        if (rowDic.ContainsKey("isreadonly") && rowDic["isreadonly"] != null)
                        {
                            dc.ReadOnly = rowDic["isreadonly"].ToString().Equals("0");
                        }
                        if (rowDic.ContainsKey("iskey") && rowDic["iskey"] != null && rowDic["iskey"].ToString().Equals("1"))
                        {
                            dc.ReadOnly = true;
                            keyDcList.Add(dc);
                        }
                    }
                }
                dt.Columns.Add(dc);
            }
            //if (keyDcList.Count > 0)
            //{
            //    DataColumn[] keyDCs = new DataColumn[keyDcList.Count];
            //    for (int i = 0; i < keyDcList.Count; i++)
            //    {
            //        keyDCs[i] = keyDcList[i];
            //    }
            //    dt.PrimaryKey = keyDCs;
            //}
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
                case "i4":
                    dc.DataType = typeof(Int32);
                    break;
                case "i8":
                    dc.DataType = typeof(Int64);
                    break;
                case "dateTime":
                    dc.DataType = typeof(DateTime);
                    dc.DateTimeMode = DataSetDateTime.Utc;
                    break;
                case "string":
                default:
                    dc.DataType = typeof(string);
                    dc.MaxLength = Convert.ToInt32(xmlNode.Attributes["WIDTH"].Value);
                    break;
            }
            return dc;
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
        public virtual string GetXmlData(string channelId, bool masterFlag = true)
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
                bool ValidTableFlag = false;
                if (masterFlag)
                {
                    if (mastDt.TableName.Equals("masterdata"))
                    {
                        ValidTableFlag = true;
                    }
                }
                else
                {
                    if (Regex.IsMatch(mastDt.TableName, @"^[1-9]\d*$"))
                    {
                        ValidTableFlag = true;
                    }
                }

                if (!ValidTableFlag)
                    continue;
                stringBuilder.Append("<DATASET id=\"").Append(mastDt.TableName).Append("\">");
                stringBuilder.Append("<DATAPACKET Version=\"").Append("2.0").Append("\">");
                stringBuilder.Append("<METADATA>");
                stringBuilder.Append("<FIELDS>");
                foreach (DataColumn mastDc in mastDt.Columns)
                {
                    string fieldType = "string";
                    long fieldLength = 50;
                    switch (mastDc.DataType.Name)
                    {
                        case "Int32":
                            fieldType = "i4";
                            fieldLength = 11;
                            break;
                        case "Int64":
                            fieldType = "i8";
                            fieldLength = 20;
                            break;
                        case "DateTime":
                            fieldType = "dateTime";
                            fieldLength = 23;
                            break;
                        case "String":
                            fieldLength = mastDc.MaxLength;
                            break;
                        default:
                            fieldType = "string";
                            break;
                    }
                    stringBuilder.AppendFormat("<FIELD attrname='{0}' fieldtype='{1}' WIDTH='{2}' /> ", mastDc.ColumnName, fieldType, fieldLength);
                }
                stringBuilder.Append("</FIELDS>");
                stringBuilder.Append("</METADATA>");
                stringBuilder.Append("<ROWDATA>");
                foreach (DataRow mastDr in mastDt.Rows)
                {
                    if (mastDr.RowState == DataRowState.Added)
                    {
                        GetDataRowXml(stringBuilder, mastDt, mastDr, "4");
                    }
                    else if (mastDr.RowState == DataRowState.Modified)
                    {
                        GetDataRowXml(stringBuilder, mastDt, mastDr, "8");
                        GetDataRowXml(stringBuilder, mastDt, mastDr, "1", true);
                    }
                    else if (mastDr.RowState == DataRowState.Deleted)
                    {
                        GetDataRowXml(stringBuilder, mastDt, mastDr, "2", true);
                    }
                }
                stringBuilder.Append("</ROWDATA>");
                stringBuilder.Append("</DATAPACKET>");
                stringBuilder.Append("</DATASET>");
            }
            stringBuilder.Append("</DATA>");
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 获取JSON格式数据
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public virtual List<object> GetJSONData(string channelId, bool masterFlag = true)
        {
            DataChannelManager dataManager = new DataChannelManager();
            Hashtable hashData = dataManager.GetData(channelId);
            ChannelData channelData = hashData["Data"] as ChannelData;
            DataSet requestDs = channelData.DataInfo;
            List<object> updateRowList = new List<object>();
            foreach (DataTable mastDt in requestDs.Tables)
            {
                #region 判定信息
                //bool ValidTableFlag = false;
                //if (masterFlag)
                //{
                //    if (mastDt.TableName.Equals("masterdata"))
                //    {
                //        ValidTableFlag = true;
                //    }
                //}
                //else
                //{
                //    if (Regex.IsMatch(mastDt.TableName, @"^[1-9]\d*$"))
                //    {
                //        ValidTableFlag = true;
                //    }
                //}

                //if (!ValidTableFlag)
                //    continue;
                #endregion
                foreach (DataRow dr in mastDt.Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        Dictionary<string, object> oldDic = new Dictionary<string, object>();
                        oldDic.Add("rowstate", 1);
                        foreach (DataColumn dc in mastDt.Columns)
                        {

                            oldDic.Add(dc.ColumnName, dr[dc.ColumnName, DataRowVersion.Original]);
                        }
                        oldDic.Add("statedescribe", null);
                        updateRowList.Add(oldDic);
                        Dictionary<string, object> newDic = new Dictionary<string, object>();
                        newDic.Add("rowstate", 8);
                        foreach (DataColumn dc in mastDt.Columns)
                        {

                            newDic.Add(dc.ColumnName, dr[dc.ColumnName]);
                        }
                        newDic.Add("statedescribe", null);
                        updateRowList.Add(newDic);
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        Dictionary<string, object> newDic = new Dictionary<string, object>();
                        newDic.Add("rowstate", 2);
                        foreach (DataColumn dc in mastDt.Columns)
                        {
                            newDic.Add(dc.ColumnName, dr[dc.ColumnName, DataRowVersion.Original]);
                        }
                        newDic.Add("statedescribe", null);
                        updateRowList.Add(newDic);
                    }
                    if (dr.RowState == DataRowState.Added)
                    {
                        Dictionary<string, object> newDic = new Dictionary<string, object>();
                        newDic.Add("rowstate", 4);
                        foreach (DataColumn dc in mastDt.Columns)
                        {
                            newDic.Add(dc.ColumnName, dr[dc.ColumnName]);
                        }
                        newDic.Add("statedescribe", null);
                        updateRowList.Add(newDic);
                    }
                }
            }
            return updateRowList;
        }


        private static void GetDataRowXml(StringBuilder stringBuilder, DataTable mastDt, DataRow mastDr, string rowState, bool isDelete = false)
        {
            try
            {
                stringBuilder.Append("<ROW ");
                stringBuilder.AppendFormat("RowState='{0}'", rowState);
                foreach (DataColumn mastDc in mastDt.Columns)
                {
                    if (mastDc.DataType == typeof(DateTime))
                    {
                        string dateTimeStr = string.Empty;
                        if (isDelete)
                        {
                            dateTimeStr = string.IsNullOrEmpty(mastDr[mastDc.ColumnName, DataRowVersion.Original].ToString()) ? "" : ((DateTime)mastDr[mastDc.ColumnName, DataRowVersion.Original]).ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        else
                        {
                            dateTimeStr = string.IsNullOrEmpty(mastDr[mastDc.ColumnName].ToString()) ? "" : ((DateTime)mastDr[mastDc.ColumnName]).ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        stringBuilder.AppendFormat(" {0}='{1}' ", mastDc.ColumnName, dateTimeStr);
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
            Hashtable replyHashtable = CreateDataSetByReplyMessage(replyMessageInfo, messageInfo);
            DataChannelManager dataManager = new DataChannelManager();
            string DataChannelId = string.Empty;
            if (!dataManager.CheckDataExist(messageInfo, out DataChannelId))
            {
                DataChannelId = replyMessageInfo.MessageId;
            }
            if (dataManager.AddData(DataChannelId, replyHashtable))
            {
                replyMessageInfo.DataChannelId = DataChannelId;
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

