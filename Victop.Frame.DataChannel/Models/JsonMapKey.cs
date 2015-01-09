using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Victop.Frame.DataChannel.Models
{
    internal class JsonMapKey
    {
        internal string ViewId { get; set; }
        internal string DataPath { get; set; }
        /// <summary>
        /// 引用映射表
        /// </summary>
        private DataTable refMapTable;
        /// <summary>
        /// 引用映射表
        /// </summary>
        internal DataTable RefMapTable
        {
            get
            {
                if (refMapTable == null)
                {
                    refMapTable = new DataTable("refTable");
                    DataColumn idDc = new DataColumn("_id", typeof(Guid));
                    idDc.AllowDBNull = false;
                    refMapTable.Columns.Add(idDc);
                    DataColumn triggerTableDc = new DataColumn("triggertable", typeof(string));
                    refMapTable.Columns.Add(triggerTableDc);
                    DataColumn rowKeyDc = new DataColumn("rowkey", typeof(string));
                    refMapTable.Columns.Add(rowKeyDc);
                    DataColumn triggerfieldDc = new DataColumn("triggerfield", typeof(string));
                    refMapTable.Columns.Add(triggerfieldDc);
                    DataColumn valueDc = new DataColumn("value", typeof(object));
                    refMapTable.Columns.Add(valueDc);
                    DataColumn dependidDc = new DataColumn("dependid", typeof(Guid));
                    refMapTable.Columns.Add(dependidDc);
                }
                return refMapTable;
            }
            set
            {
                refMapTable = value;
            }
        }
    }
}
