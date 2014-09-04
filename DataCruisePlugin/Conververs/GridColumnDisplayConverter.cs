using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DataCruisePlugin.Models;
using Victop.Frame.DataChannel;
using System.Data;
using Victop.Frame.SyncOperation;
using System.Collections.ObjectModel;

namespace DataCruisePlugin.Conververs
{
    public class GridColumnDisplayConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return value;
            }
            try
            {
                Dictionary<string, object> paramDic = parameter as Dictionary<string, object>;
                string colName = paramDic["columnname"].ToString();
                EntityDefinitionModel entityModel = paramDic["tag"] as EntityDefinitionModel;
                ObservableCollection<EntityDefinitionModel> allModels = paramDic["fullentity"] as ObservableCollection<EntityDefinitionModel>;
                List<RefEntityModel> refList = entityModel.DataRef as List<RefEntityModel>;
                RefEntityModel refModel = refList.FirstOrDefault(it => it.SelfField == colName);
                EntityDefinitionModel refEntityModel = allModels.FirstOrDefault(it => it.Id == refModel.TableId);
                if (string.IsNullOrEmpty(refEntityModel.HostTable))
                {
                    if (string.IsNullOrEmpty(refEntityModel.ViewId))
                        refEntityModel.ViewId = SendFindDataMessage(refEntityModel.TableName);
                }
                DataOperation dataOp = new DataOperation();
                DataTable dt = dataOp.GetData(refEntityModel.ViewId, refEntityModel.DataPath, CreateStructDataTable(refEntityModel));
                DataRow[] drs = dt.Select(string.Format("{0}='{1}'", refModel.SourceField, value));
                if (drs != null && drs.Count() > 0)
                    return drs[0][refModel.SourceText];
                else
                    return value;
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        private DataTable CreateStructDataTable(EntityDefinitionModel entityModel)
        {
            List<EntityFieldModel> entityFields = entityModel.Fields as List<EntityFieldModel>;
            DataTable structDt = new DataTable(entityModel.TableName);
            foreach (EntityFieldModel item in entityFields)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = item.Field;
                if (item.Field.Equals("_id"))
                {
                    dc.ReadOnly = true;
                    dc.DefaultValue = Guid.NewGuid();
                }
                dc.Caption = item.FieldTitle;
                switch (item.FieldType)
                {
                    case "date":
                        dc.DataType = typeof(DateTime);
                        break;
                    case "int":
                        dc.DataType = typeof(Int32);
                        break;
                    case "long":
                        dc.DataType = typeof(Int64);
                        break;
                    case "bool":
                        dc.DataType = typeof(Boolean);
                        break;
                    case "string":
                    default:
                        dc.DataType = typeof(String);
                        break;
                }
                structDt.Columns.Add(dc);
            }
            return structDt;
        }

        /// <summary>
        /// 发送检索数据消息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string SendFindDataMessage(string tableName, List<object> conditons = null)
        {
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "800");
            contentDic.Add("configsystemid", "101");
            contentDic.Add("spaceid", "tbs");
            contentDic.Add("tablename", tableName);
            if (conditons != null)
            {
                contentDic.Add("tablecondition", conditons);
            }
            string messageType = "MongoDataChannelService.findTableData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                return returnDic["DataChannelId"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
