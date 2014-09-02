using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.DataChannel.Enums;

namespace Victop.Frame.DataChannel
{
    internal class DataTool
    {

        /// <summary>
        /// 将xml对象内容字符串转换为DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(ChangeDateTimeFormate(xmlData));
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);

                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        /// <summary>
        /// 正规表达式,匹配并转换带T的时间格式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string ChangeDateTimeFormate(string source)
        {
            string rule = "\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{5}";
            if (Regex.IsMatch(source, rule))
            {
                StringBuilder stringBuilder = new StringBuilder();
                string[] str = Regex.Split(source, rule);
                stringBuilder.Append(str[0]);
                MatchCollection results = Regex.Matches(source, rule);
                for (int i = 0; i < results.Count; i++)
                {
                    stringBuilder.Append(results[i].Value.Replace("T", " ").Substring(0, 19));
                    stringBuilder.Append(str[i + 1]);
                }
                return stringBuilder.ToString();
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        ///  将DataSet转换为xml
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <param name="xmlFile"></param>
        public static string ConvertDataSetToXML(DataSet xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;

            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //用WriteXml方法写入文件.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                UnicodeEncoding utf = new UnicodeEncoding();
                return utf.GetString(arr).Trim();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }
        /// <summary>
        /// 将XML转为DataTable
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ConvertXMLToDataTable(XmlNode xmlData, string tableName)
        {

            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataTable table = ConvertXMLToDataTableSchema(xmlData, tableName);
                stream = new StringReader(ChangeDateTimeFormate(xmlData.SelectSingleNode("ROWDATA").OuterXml));
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                table.ReadXml(reader);

                return table;
            }
            catch (System.Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
        private static DataTable ConvertXMLToDataTableSchema(XmlNode xml, string tablename)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataTable table = new DataTable();
                table.TableName = tablename;
                stream = new StringReader(GetXmlSchema(tablename, xml.SelectSingleNode("METADATA").SelectSingleNode("FIELDS").SelectNodes("FIELD")));
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                table.ReadXmlSchema(reader);
                return table;
            }
            catch (System.Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
        private static StringBuilder stringBuilder = new StringBuilder();
        private static List<string> tmpList = new List<string>();
        private static string GetXmlSchema(string tableName, XmlNodeList list)
        {
            stringBuilder.Clear();
            tmpList.Clear();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-16\"?>");
            stringBuilder.Append("<xs:schema xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"\" id=\"NewDataSet\">");
            stringBuilder.Append("<xs:element msdata:UseCurrentLocale=\"true\" msdata:MainDataTable=\"").Append(tableName).Append("\" msdata:IsDataSet=\"true\" name=\"NewDataSet\">");
            stringBuilder.Append("<xs:complexType>");
            stringBuilder.Append("<xs:choice maxOccurs=\"unbounded\" minOccurs=\"0\">");
            stringBuilder.Append("<xs:element name=\"").Append(tableName).Append("\">");
            stringBuilder.Append("<xs:complexType>");
            foreach (XmlNode node in list)
            {
                if (!tmpList.Contains(((XmlElement)node).GetAttribute("attrname")))
                {
                    //stringBuilder.Append(" <xs:attribute name=\"").Append(((XmlElement)node).GetAttribute("attrname")).Append( "msdata:Caption=测试").Append("\" type=\"xs:string\"/>");
                    stringBuilder.AppendFormat("<xs:attribute name=\"{0}\" msdata:Caption=\"{1}\" type=\"xs:string\"/>", ((XmlElement)node).GetAttribute("attrname"), "测试");
                    tmpList.Add(((XmlElement)node).GetAttribute("attrname"));
                }
            }
            stringBuilder.Append("</xs:complexType>");
            stringBuilder.Append("</xs:element>");
            stringBuilder.Append("</xs:choice>");
            stringBuilder.Append("</xs:complexType>");
            stringBuilder.Append("</xs:element>");
            stringBuilder.Append("</xs:schema>");
            return stringBuilder.ToString();
        }
        /// <summary>
        /// xmldocument 对象转成string 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string XmlDocumentToString(XmlDocument doc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = System.Xml.Formatting.Indented;
            doc.Save(writer);

            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return xmlString;
        }


        #region JSON与DataTable的转换
        /// <summary>
        /// JSON Array转换成DataTable
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string jsonStr)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                ArrayList arrayList = JsonHelper.ToObject<ArrayList>(jsonStr);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        /// <summary>
        /// DataTable转换为Json Array
        /// </summary>
        /// <param name="jsonDt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable jsonDt)
        {
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in jsonDt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in jsonDt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
            return JsonHelper.ToJson(arrayList);
        }
        #endregion

        /// <summary>
        /// 依据路径获取数据
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public static string GetDataByPath(string viewId, string dataPath)
        {
            DataOperation dataOp = new DataOperation();
            string JsonData = dataOp.GetJSONData(viewId);
            JsonData = JsonHelper.ReadJsonString(JsonData, "docDataStore");
            try
            {
                List<object> pathList = JsonHelper.ToObject<List<object>>(dataPath);
                if (pathList != null)
                {
                    for (int i = 0; i < pathList.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            JsonData = JsonHelper.ReadJsonString(JsonData, pathList[i].ToString());
                            if (i == pathList.Count - 1)
                            {
                                return JsonData;
                            }
                            JsonData = JsonHelper.ReadJsonString(JsonData, "dataArray");
                        }
                        else if (i % 2 == 1)
                        {
                            Dictionary<string, string> pathDic = JsonHelper.ToObject<Dictionary<string, string>>(pathList[i].ToString());
                            if (pathDic != null)
                            {
                                List<Dictionary<string, object>> arrayList = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonData);
                                if (arrayList != null)
                                {
                                    for (int j = 0; j < arrayList.Count; j++)
                                    {
                                        if (arrayList[j].ContainsKey(pathDic["key"]) && arrayList[j][pathDic["key"]].ToString().Equals(pathDic["value"]))
                                        {
                                            JsonData = JsonHelper.ToJson(arrayList[j]);
                                            if (i == pathList.Count - 1)
                                            {
                                                return JsonData;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                JsonData = string.Empty;
            }
            catch (Exception ex)
            {

                throw;
            }
            return JsonData;
        }

        /// <summary>
        /// 依据路径保存数据
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <param name="saveData"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        public static bool SaveCurdDataByPath(string viewId, List<object> dataPath, Dictionary<string,object> saveData, OpreateStateEnum rowState)
        {
            try
            {
                if (!SaveDataByPath(viewId, dataPath, saveData, rowState))
                {
                    return false;
                }
                Dictionary<string, object> curdDic = new Dictionary<string, object>();
                curdDic.Add("flag", rowState);
                curdDic.Add("path", dataPath);
                curdDic.Add("rowdata", saveData);
                DataOperation dataOp = new DataOperation();
                switch (rowState)
                {
                    case OpreateStateEnum.Added:
                        string addCurdJson = dataOp.GetCurdJSONData(viewId);
                        List<object> addCurdList;
                        if (string.IsNullOrEmpty(addCurdJson))
                        {
                            addCurdList = new List<object>();
                            addCurdList.Add(curdDic);
                        }
                        else
                        {
                            addCurdList = JsonHelper.ToObject<List<object>>(addCurdJson);
                            addCurdList.Add(curdDic);
                        }
                        dataOp.SaveCurdJSONData(viewId, JsonHelper.ToJson(addCurdList));

                        break;
                    case OpreateStateEnum.Modified:
                        string modCurdJson = dataOp.GetCurdJSONData(viewId);
                        List<object> modCurdList;
                        if (string.IsNullOrEmpty(modCurdJson))
                        {
                            modCurdList = new List<object>();
                            modCurdList.Add(curdDic);
                        }
                        else
                        {
                            modCurdList = JsonHelper.ToObject<List<object>>(modCurdJson);
                        }
                        bool modFlag = true;
                        //TODO:修改是增加对修改内总键值的判断，若cud池中已经存在键，则将新的值存入其中，若没有，则将新的键值加入对应的rowdata中
                        foreach (var item in modCurdList)
                        {
                            string tt = JsonHelper.ToJson(item);
                            Dictionary<string, object> modDic = JsonHelper.ToObject<Dictionary<string, object>>(tt);
                            if (modDic["path"].ToString().Equals(dataPath))
                            {
                                modDic["rowdata"] = saveData;
                                modFlag = false;
                                break;
                            }
                        }
                        if (modFlag)
                        {
                            modCurdList.Add(curdDic);
                        }
                        dataOp.SaveCurdJSONData(viewId, JsonHelper.ToJson(modCurdList));
                        break;
                    case OpreateStateEnum.Deleted:
                        string delCurdJson = dataOp.GetCurdJSONData(viewId);
                        List<object> delCurdList;
                        if (delCurdJson == null)
                        {
                            delCurdList = new List<object>();
                            delCurdList.Add(curdDic);
                        }
                        else
                        {
                            delCurdList = JsonHelper.ToObject<List<object>>(delCurdJson);
                        }
                        foreach (var item in delCurdList)
                        {
                            Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(item.ToString());
                            if (itemDic["flag"].ToString().Equals("4") && itemDic["path"].ToString().Equals(dataPath))
                            {
                                string delKey = JsonHelper.ReadJsonString(itemDic["rowdata"].ToString(), "_id");
                                if (saveData["_id"].ToString().Equals(delKey))
                                {
                                    delCurdList.Remove(item);
                                }
                                else
                                {
                                    delCurdList.Add(curdDic);
                                }
                                break;
                            }
                            else if (itemDic["path"].ToString().Equals(dataPath))
                            {
                                string delKey = JsonHelper.ReadJsonString(itemDic["rowdata"].ToString(), "_id");
                                if (saveData["_id"].ToString().Equals(delKey))
                                {
                                    delCurdList.Remove(item);
                                    delCurdList.Add(curdDic);
                                    break;
                                }
                            }
                        }
                        dataOp.SaveCurdJSONData(viewId, JsonHelper.ToJson(delCurdList));
                        break;
                    case OpreateStateEnum.None:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }
        /// <summary>
        /// 将修改内容保存到原始数据中
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="dataPath"></param>
        /// <param name="saveData"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        public static bool SaveDataByPath(string viewId, List<object> dataPath, Dictionary<string,object> saveData, OpreateStateEnum rowState)
        {
            try
            {
                DataOperation dataOp = new DataOperation();
                string jsonData = string.Empty;
                Dictionary<string, object> fullDataDic = JsonHelper.ToObject<Dictionary<string, object>>(dataOp.GetJSONData(viewId));
                List<Dictionary<string, object>> DataList = new List<Dictionary<string, object>>();
                List<object> pathList = dataPath;
                if (pathList != null)
                {
                    Dictionary<string, object> storeValueDic = JsonHelper.ToObject<Dictionary<string, object>>(fullDataDic["docDataStore"].ToString());
                    DataList.Add(storeValueDic);
                    for (int i = 0; i < pathList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(jsonData))
                        {
                            jsonData = fullDataDic["docDataStore"].ToString();
                        }

                        if (i % 2 == 0)
                        {
                            if (i == pathList.Count - 1)
                            {
                                jsonData = JsonHelper.ReadJsonString(jsonData, pathList[i].ToString());
                                Dictionary<string, object> arrayDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonData);
                                List<object> arrayList = JsonHelper.ToObject<List<object>>(arrayDic["dataArray"].ToString());
                                switch (rowState)
                                {
                                    case OpreateStateEnum.Added:
                                        //TODO:根据主键值判定是否存在重复记录
                                        arrayList.Add(saveData);
                                        break;
                                    case OpreateStateEnum.Modified:
                                        break;
                                    case OpreateStateEnum.Deleted:
                                        foreach (var item in arrayList)
                                        {
                                            Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(item.ToString());
                                            if (itemDic.ContainsKey("_id") && itemDic["_id"].ToString().Equals(saveData["_id"].ToString()))
                                            {
                                                arrayList.Remove(item);
                                                break;
                                            }
                                        }
                                        break;
                                    case OpreateStateEnum.None:
                                        break;
                                    default:
                                        break;
                                }
                                arrayDic["dataArray"] = arrayList;
                                DataList[DataList.Count - 1][pathList[i].ToString()] = arrayDic;
                            }
                            else
                            {
                                jsonData = JsonHelper.ReadJsonString(jsonData, pathList[i].ToString());
                            }
                        }
                        else if (i % 2 == 1)
                        {
                            Dictionary<string, object> tableDic = JsonHelper.ToObject<Dictionary<string, object>>(jsonData);
                            DataList.Add(tableDic);
                            jsonData = tableDic["dataArray"].ToString();
                            if (i == pathList.Count - 1)
                            {
                                List<object> arrayList = JsonHelper.ToObject<List<object>>(jsonData);
                                for (int j = 0; j < arrayList.Count; j++)
                                {
                                    Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(arrayList[j].ToString());
                                    switch (rowState)
                                    {
                                        case OpreateStateEnum.Added:
                                            break;
                                        case OpreateStateEnum.Modified:
                                            if (itemDic["_id"].ToString().Equals(JsonHelper.ReadJsonString(pathList[i].ToString(), "value")))
                                            {
                                                Dictionary<string, object> saveDic = saveData;
                                                foreach (string savekey in saveDic.Keys)
                                                {
                                                    if (itemDic.ContainsKey(savekey))
                                                    {
                                                        itemDic[savekey] = saveDic[savekey];
                                                    }
                                                    else
                                                    {
                                                        itemDic.Add(savekey, saveDic[savekey]);
                                                    }
                                                }
                                            }
                                            break;
                                        case OpreateStateEnum.Deleted:
                                            break;
                                        case OpreateStateEnum.None:
                                            break;
                                        default:
                                            break;
                                    }
                                    arrayList[j] = itemDic;
                                }
                                tableDic["dataArray"] = arrayList;
                            }
                            else
                            {
                                Dictionary<string, object> tmpDic = new Dictionary<string, object>();
                                tmpDic.Add("dataArray", jsonData);
                                DataList.Add(tmpDic);
                                List<object> arrayList = JsonHelper.ToObject<List<object>>(jsonData);
                                foreach (var item in arrayList)
                                {
                                    Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(item.ToString());
                                    if (itemDic["_id"].ToString().Equals(JsonHelper.ReadJsonString(pathList[i].ToString(), "value")))
                                    {
                                        jsonData = item.ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //保存原始数据
                    for (int i = DataList.Count - 1; i > 0; i--)
                    {
                        if (i % 2 == 0)
                        {
                            List<object> tempList = JsonHelper.ToObject<List<object>>(DataList[i - 1]["dataArray"].ToString());
                            for (int j = 0; j < tempList.Count; j++)
                            {
                                Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(tempList[j].ToString());
                                if (itemDic["_id"].ToString().Equals(JsonHelper.ReadJsonString(pathList[i - 1].ToString(), "value")))
                                {
                                    tempList[j] = DataList[i];
                                }
                            }
                            DataList[i - 1]["dataArray"] = tempList;
                        }
                        else
                        {
                            Dictionary<string, object> pathDic = JsonHelper.ToObject<Dictionary<string, object>>(pathList[i].ToString());
                            if (i == 1)
                            {
                                Dictionary<string, object> fullDic = new Dictionary<string, object>();
                                fullDic.Add(pathList[i - 1].ToString(), DataList[i]);
                                DataList[i - 1] = fullDic;
                            }
                            else
                            {
                                List<object> tempList = JsonHelper.ToObject<List<object>>(DataList[i - 1]["dataArray"].ToString());
                                for (int j = 0; j < tempList.Count; j++)
                                {
                                    Dictionary<string, object> itemDic = JsonHelper.ToObject<Dictionary<string, object>>(tempList[j].ToString());
                                    if (itemDic.ContainsKey(pathList[i - 1].ToString()) && itemDic[pathList[i - 1].ToString()] != null)
                                    {

                                        Dictionary<string, object> currentPathDic = JsonHelper.ToObject<Dictionary<string, object>>(itemDic[pathList[i - 1].ToString()].ToString());
                                        if (itemDic["_id"].ToString().Equals(JsonHelper.ReadJsonString(pathList[i - 2].ToString(), "value")))
                                        {
                                            currentPathDic["dataArray"] = DataList[i];
                                            itemDic[pathList[i - 1].ToString()] = currentPathDic;
                                            DataList[i - 1] = itemDic;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Dictionary<string, object> tempDic = new Dictionary<string, object>();
                    tempDic.Add("docDataStore", JsonHelper.ToJson(DataList[0]));
                    jsonData = JsonHelper.ToJson(tempDic);
                }
                return dataOp.SaveJSONData(viewId, jsonData);
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("保存原数据异常：{0}", ex.Message);
                return false;
            }
        }
    }
}
