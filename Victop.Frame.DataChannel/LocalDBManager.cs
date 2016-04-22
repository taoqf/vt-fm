using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Victop.Frame.CoreLibrary.Interfaces;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.DataChannel.Models;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.DataChannel
{
    /// <summary>
    /// 本地数据管理
    /// </summary>
    public class LocalDBManager : IFeiDaoDataOperation
    {
        #region 字段
        static SQLiteConnection localdbConnection;
        private string tableName = "FeiDaoDB";
        #endregion
        /// <summary>
        /// 数据库初始化
        /// </summary>
        /// <returns></returns>
        public bool DataBaseInit()
        {
            try
            {
                #region 创建数据库文件
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlData");
                if (!Directory.Exists(dbPath))
                {
                    Directory.CreateDirectory(dbPath);
                }
                dbPath = Path.Combine(dbPath, "FeiDaoLocalDb.wpf");
                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                }
                #endregion
                #region 创建数据库连接
                localdbConnection = new SQLiteConnection("Data Source=SqlData/FeiDaoLocalDb.wpf;Version=3;");
                localdbConnection.Open();
                #endregion
                #region 初始化数据库表
                if (!IsTableExist(tableName))
                {
                    CreateTable();
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Insert(FeiDaoDBModel newEntity)
        {
            try
            {
                newEntity.ValidDate = RegexHelper.DateTimeToTimestamp(DateTime.Now.AddDays(30));
                FeiDaoDBModel findEntity = FindByCondtion(newEntity.PluginName, newEntity.ParamName);
                string excuteSql = string.Empty;
                if (findEntity != null && !string.IsNullOrEmpty(findEntity.Id))
                {
                    excuteSql = string.Format("update {0} set ParamValue='{1}',ValidDate={2} where Id='{3}'", tableName, newEntity.ParamValue, newEntity.ValidDate, findEntity.Id);
                }
                else
                {
                    if (string.IsNullOrEmpty(newEntity.Id))
                        newEntity.Id = Guid.NewGuid().ToString();
                    excuteSql = string.Format("insert into {0} (Id,PluginName, ParamName,ParamValue,ValidDate) values ('{1}','{2}', '{3}','{4}',{5})", tableName, newEntity.Id, newEntity.PluginName, newEntity.ParamName, newEntity.ParamValue, newEntity.ValidDate);
                }
                SQLiteCommand command = new SQLiteCommand(excuteSql, localdbConnection);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(FeiDaoDBModel updateEntity, string key)
        {
            try
            {
                FeiDaoDBModel findEntity = FindByID(key);
                if (findEntity != null && !string.IsNullOrEmpty(findEntity.Id))
                {
                    string updateSql = string.Format("update {0} set ParamValue='{1}' where Id='{2}'", tableName, updateEntity.ParamValue, findEntity.Id);
                    SQLiteCommand command = new SQLiteCommand(updateSql, localdbConnection);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(string id)
        {
            try
            {
                string delSql = string.Format("delete from {0} where Id='{1}'", tableName, id);
                SQLiteCommand command = new SQLiteCommand(delSql, localdbConnection);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="paramName">参数名称</param>
        /// <returns></returns>
        public bool DeleteByCondition(string pluginName, string paramName)
        {
            try
            {
                string delSql = string.Empty;
                if (string.IsNullOrEmpty(paramName))
                {
                    delSql = string.Format("delete from {0} where pluginName='{1}'", tableName, pluginName);
                }
                else
                {
                    delSql = string.Format("delete from {0} where pluginName='{1}' and paramName='{2}'", tableName, pluginName, paramName);
                }
                SQLiteCommand command = new SQLiteCommand(delSql, localdbConnection);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public FeiDaoDBModel FindByID(string id)
        {
            FeiDaoDBModel InfoModel = new FeiDaoDBModel();
            string findSql = string.Format("select * from {0} where Id='{1}'", tableName, id);
            SQLiteCommand command = new SQLiteCommand(findSql, localdbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                InfoModel.Id = reader["Id"].ToString();
                InfoModel.PluginName = reader["PluginName"].ToString();
                InfoModel.ParamName = reader["ParamName"].ToString();
                InfoModel.ParamValue = reader["ParamValue"].ToString();
                InfoModel.ValidDate = Convert.ToInt64(reader["ValidDate"]);
            }
            return InfoModel;
        }

        public FeiDaoDBModel FindByCondtion(string pluginName, string paramName)
        {
            FeiDaoDBModel InfoModel = new FeiDaoDBModel();
            string findSql = string.Format("select * from {0} where PluginName='{1}' and ParamName='{2}'", tableName, pluginName, paramName);
            SQLiteCommand command = new SQLiteCommand(findSql, localdbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                InfoModel.Id = reader["Id"].ToString();
                InfoModel.PluginName = reader["PluginName"].ToString();
                InfoModel.ParamName = reader["ParamName"].ToString();
                InfoModel.ParamValue = reader["ParamValue"].ToString();
                InfoModel.ValidDate = Convert.ToInt64(reader["ValidDate"]);
            }
            return InfoModel;
        }
        #region 自定义方法
        void CreateTable()
        {
            SQLiteCommand command = new SQLiteCommand(localdbConnection);
            string sql = @"CREATE TABLE FeiDaoDB
(
	Id NVARCHAR(50) NOT NULL PRIMARY KEY, 
    PluginName NVARCHAR(50) NOT NULL, 
    ParamName NVARCHAR(50) NOT NULL, 
    ParamValue NVARCHAR(500) NOT NULL, 
    ValidDate BIGINT NOT NULL
)
";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
        private bool IsTableExist(string tableName)
        {
            using (SQLiteCommand command = new SQLiteCommand(localdbConnection))
            {

                command.CommandText = string.Format("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{0}';", tableName);
                int iaaa = Convert.ToInt32(command.ExecuteScalar());
                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion
    }
}
