using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using Victop.Server.Controls.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.DataMessageManager;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 展示层区块
    /// </summary>
    public class PresentationBlockModel : PropertyModelBase
    {
        /// <summary>
        /// 区块名称
        /// </summary>
        private string blockName;
        /// <summary>
        /// 区块名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string BlockName
        {
            get
            {
                return blockName;
            }
            set
            {
                blockName = value;
                RaisePropertyChanged(() => BlockName);
            }
        }
        /// <summary>
        /// 区块类型
        /// </summary>
        private int blockType;
        /// <summary>
        /// 区块类型
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public int BlockType
        {
            get { return blockType; }
            set { blockType = value; }
        }
        /// <summary>
        /// 上级Block名称
        /// </summary>
        private string superiors;
        /// <summary>
        /// 上级Block名称
        /// </summary>
        [JsonProperty(PropertyName = "superiors")]
        public string Superiors
        {
            get { return superiors; }
            set { superiors = value; }
        }
        /// <summary>
        /// 关键字
        /// </summary>
        private string keywords = string.Empty;
        /// <summary>
        /// 关键字(多个关键字时用"|"分隔)
        /// </summary>
        [JsonProperty(PropertyName = "keyword")]
        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }
        /// <summary>
        /// 方法
        /// </summary>
        private string method;
        /// <summary>
        /// 方法
        /// </summary>
        [JsonProperty(PropertyName = "method")]
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        /// <summary>
        /// View层名称
        /// </summary>
        private string viewName;
        /// <summary>
        /// View层名称
        /// </summary>
        [JsonProperty(PropertyName = "view")]
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }
        /// <summary>
        /// 是否渲染
        /// </summary>
        private int autoRender;
        /// <summary>
        /// 是否渲染
        /// </summary>
        [JsonProperty(PropertyName = "autorender")]
        public int AutoRender
        {
            get { return autoRender; }
            set { autoRender = value; }
        }
        /// <summary>
        /// 渲染类型
        /// </summary>
        private int renderType;
        /// <summary>
        /// 渲染类型
        /// </summary>
        [JsonProperty(PropertyName = "rendertype")]
        public int RenderType
        {
            get { return renderType; }
            set { renderType = value; }
        }
        /// <summary>
        /// 绑定View中的Block名称
        /// </summary>
        private string bindingBlock;
        /// <summary>
        /// 绑定View中的Block名称
        /// </summary>
        [JsonProperty(PropertyName = "binding")]
        public string BindingBlock
        {
            get { return bindingBlock; }
            set { bindingBlock = value; }
        }
        /// <summary>
        /// 上级视图Block
        /// </summary>
        [JsonIgnore]
        private PresentationBlockModel parentPreBlockModel;
        /// <summary>
        /// 上级视图Block
        /// </summary>
        [JsonIgnore]
        public PresentationBlockModel ParentPreBlockModel
        {
            get { return parentPreBlockModel; }
            set { parentPreBlockModel = value; }
        }
        /// <summary>
        /// 呈现层对应的视图层Block
        /// </summary>
        [JsonIgnore]
        public ViewsBlockModel ViewBlock
        {
            get;
            set;
        }
        /// <summary>
        /// 展示层Block的数据表
        /// </summary>
        [JsonIgnore]
        private DataTable viewBlockDataTable;
        /// <summary>
        /// 展示层Block的数据表
        /// </summary>
        [JsonIgnore]
        public DataTable ViewBlockDataTable
        {
            get
            {
                if (viewBlockDataTable == null)
                    viewBlockDataTable = new DataTable();
                return viewBlockDataTable;
            }
            set
            {
                if (viewBlockDataTable != value)
                {
                    viewBlockDataTable = value;
                    RaisePropertyChanged(() => ViewBlockDataTable);
                }
            }
        }
        /// <summary>
        /// 展示层Block当前选择行
        /// </summary>
        [JsonIgnore]
        private DataRow preBlockSelectedRow;
        /// <summary>
        /// 展示层Block当前选择行
        /// </summary>
        [JsonIgnore]
        public DataRow PreBlockSelectedRow
        {
            get
            {
                return preBlockSelectedRow;
            }
            set
            {
                if (preBlockSelectedRow != value)
                {
                    preBlockSelectedRow = value;
                    RaisePropertyChanged(() => PreBlockSelectedRow);
                }
            }
        }
        /// <summary>
        /// 设置检索条件
        /// </summary>
        /// <param name="conditionModel"></param>
        public void SetSearchCondition(ViewsBlockConditionModel conditionModel)
        {
            if (conditionModel != null && conditionModel.TableCondition != null)
            {
                ViewBlock.Conditions.TableCondition = conditionModel.TableCondition;
            }
            if (conditionModel != null && conditionModel.TableSort != null)
            {
                ViewBlock.Conditions.TableSort = conditionModel.TableSort;
            }
            if (conditionModel != null && conditionModel.PageIndex != null)
            {
                ViewBlock.Conditions.PageIndex = conditionModel.PageIndex;
            }
            if (conditionModel != null && conditionModel.PageSize != null)
            {
                ViewBlock.Conditions.PageSize = conditionModel.PageSize;
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetData()
        {
            ViewBlock.ViewModel.GetBlockData(BindingBlock);
            if (string.IsNullOrEmpty(keywords) && (superiors.Equals("root") || (ParentPreBlockModel != null && string.IsNullOrEmpty(ParentPreBlockModel.Keywords))))
            {
                ViewBlockDataTable = ViewBlock.BlockDataSet.Tables["dataArray"];
            }
            else
            {
                string[] keyStrList = Keywords.Split('|');
                DataTable dt = ViewBlock.BlockDataSet.Tables["dataArray"].Clone();
                dt.Clear();
                if (superiors.Equals("root"))
                {
                    foreach (DataRow item in ViewBlock.BlockDataSet.Tables["dataArray"].Rows)
                    {
                        string sqlStr = string.Empty;
                        if (!string.IsNullOrEmpty(Keywords))
                        {
                            sqlStr = OrganizeDtSql(keyStrList, item);
                        }
                        DataRow[] drs = dt.Select(sqlStr);
                        if (drs.Length == 0)
                        {
                            dt.ImportRow(item);
                        }

                    }
                    ViewBlockDataTable = dt;
                }
                else
                {
                    if (this.ParentPreBlockModel != null)
                    {
                        string sqlStr = string.Empty;
                        if (!string.IsNullOrEmpty(ParentPreBlockModel.Keywords))
                        {
                            sqlStr = OrganizeDtSql(ParentPreBlockModel.Keywords.Split('|'), ParentPreBlockModel.PreBlockSelectedRow);
                        }
                        if (string.IsNullOrEmpty(Keywords))
                        {
                            DataRow[] drs = ViewBlock.BlockDataSet.Tables["dataArray"].Select(sqlStr);
                            foreach (DataRow drsitem in drs)
                            {
                                dt.ImportRow(drsitem);
                            }
                        }
                        else
                        {
                            foreach (DataRow item in ViewBlock.BlockDataSet.Tables["dataArray"].Rows)
                            {
                                sqlStr += " and " + OrganizeDtSql(Keywords.Split('|'), item);
                                DataRow[] drs = dt.Select(sqlStr);
                                if (drs.Length == 0)
                                {
                                    dt.ImportRow(item);
                                }
                            }
                        }
                    }
                    ViewBlockDataTable = dt;
                }
            }
            if (ViewBlockDataTable != null && ViewBlockDataTable.Rows.Count > 0)
            {
                PreBlockSelectedRow = ViewBlockDataTable.Rows[0];
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        public void SearchData()
        {
            ViewBlock.ViewModel.SearchData();
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="spaceId">SpaceId</param>
        public void SearchData(string spaceId)
        {
            ViewBlock.ViewModel.SearchData(spaceId);
        }
        /// <summary>
        /// 获取完整数据集
        /// </summary>
        /// <returns></returns>
        public DataSet GetFullData()
        {
            return ViewBlock.BlockDataSet;
        }

        /// <summary>
        /// 组织Dt的sql语句
        /// </summary>
        /// <param name="keyStrList"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string OrganizeDtSql(string[] keyStrList, DataRow item)
        {
            string sqlStr = string.Empty;
            for (int i = 0; i < keyStrList.Length; i++)
            {
                if (string.IsNullOrEmpty(keyStrList[i]))
                    continue;
                sqlStr += keyStrList[i] + "=";
                sqlStr += "'" + item[keyStrList[i]] + "'";
                if (i != keyStrList.Length - 1)
                {
                    sqlStr += " and ";
                }
            }
            return sqlStr;
        }
        /// <summary>
        /// 设置当前选择行
        /// </summary>
        /// <param name="dr"></param>
        public void SetCurrentRow(DataRow dr)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                ViewBlock.SetCurrentRow(dr);
            }
            else
            {
                DataRow[] drs = ViewBlock.BlockDataSet.Tables["dataArray"].Select(string.Format("_id='{0}'", dr["_id"].ToString()));
                ViewBlock.SetCurrentRow(drs[0]);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        ///<param name="saveToServer">是否保存到服务端</param>
        public bool SaveData(bool saveToServer = true)
        {
            if (!string.IsNullOrEmpty(Keywords) || (ParentPreBlockModel != null && !string.IsNullOrEmpty(ParentPreBlockModel.keywords)))
            {
                foreach (DataRow item in ViewBlockDataTable.Rows)
                {
                    switch (item.RowState)
                    {
                        case DataRowState.Added:
                            ViewBlock.BlockDataSet.Tables["dataArray"].ImportRow(item);
                            break;
                        case DataRowState.Deleted:
                            DataRow[] delDrs = ViewBlock.BlockDataSet.Tables["dataArray"].Select(string.Format("_id='{0}'", item["_id"]));
                            foreach (DataRow drsitem in delDrs)
                            {
                                drsitem.Delete();
                            }
                            break;
                        case DataRowState.Modified:
                            DataRow[] modDrs = ViewBlock.BlockDataSet.Tables["dataArray"].Select(string.Format("_id='{0}'", item["_id"]));
                            foreach (DataRow drsitem in modDrs)
                            {
                                foreach (DataColumn colitem in drsitem.Table.Columns)
                                {
                                    drsitem[colitem.ColumnName] = item[colitem.ColumnName];
                                }
                            }
                            break;
                        case DataRowState.Detached:
                        case DataRowState.Unchanged:
                        default:
                            break;
                    }
                }
                ViewBlockDataTable.AcceptChanges();
            }
            return ViewBlock.ViewModel.SaveData(saveToServer);
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="cascading">是否级联</param>
        /// <returns></returns>
        public bool ResetData(bool cascading)
        {
            DataMessageOperation dataOp = new DataMessageOperation();
            return dataOp.ResetData(ViewBlock.ViewId, JsonHelper.ToJson(ViewBlock.BlockDataPath), cascading);
        }
    }
}
