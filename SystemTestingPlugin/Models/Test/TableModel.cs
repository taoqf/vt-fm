using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    public class TableModel:ModelBase
    {
        private int tableId;

        public int TableId
        {
            get { return tableId; }
            set
            {
                if (tableId != value)
                {
                    tableId = value;
                    RaisePropertyChanged(()=> TableId);
                }
            }
        }
        private string tableName;

        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                if (tableName != value)
                {
                    tableName = value;
                    RaisePropertyChanged(()=> TableName);
                }
            }
        }
        private DataTable dataInfo;

        public DataTable DataInfo
        {
            get
            {
                if (dataInfo == null)
                    dataInfo = new DataTable();
                return dataInfo;
            }
            set
            {
                if (dataInfo != value)
                {
                    dataInfo = value;
                    RaisePropertyChanged(()=> DataInfo);
                }
            }
        }
    }
}
