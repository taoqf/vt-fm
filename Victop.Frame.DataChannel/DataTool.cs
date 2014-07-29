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


    }
}
