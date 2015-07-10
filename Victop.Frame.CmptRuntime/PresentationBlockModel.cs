using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using Victop.Server.Controls.Models;

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
        [JsonProperty(PropertyName = "name")]
        private string blockName;
        /// <summary>
        /// 区块名称
        /// </summary>
        public string BlockName
        {
            get
            {
                return blockName;
            }
            set
            {
                blockName = value;
                RaisePropertyChanged("BlockName");
            }
        }
        /// <summary>
        /// 上级Block名称
        /// </summary>
        [JsonProperty(PropertyName = "superiors")]
        private string superiors;
        /// <summary>
        /// 上级Block名称
        /// </summary>
        public string Superiors
        {
            get { return superiors; }
            set { superiors = value; }
        }
        /// <summary>
        /// 关键字
        /// </summary>
        [JsonProperty(PropertyName = "keyword")]
        private string keywords;
        /// <summary>
        /// 关键字(多个关键字时用"|"分隔)
        /// </summary>
        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }
        /// <summary>
        /// 方法
        /// </summary>
        [JsonProperty(PropertyName = "method")]
        private string method;
        /// <summary>
        /// 方法
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        /// <summary>
        /// View层名称
        /// </summary>
        [JsonProperty(PropertyName = "view")]
        private string viewName;
        /// <summary>
        /// View层名称
        /// </summary>
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }
        /// <summary>
        /// 自动加载
        /// </summary>
        [JsonProperty(PropertyName = "autorender")]
        private bool autoRender;
        /// <summary>
        /// 自动加载
        /// </summary>
        public bool AutoRender
        {
            get { return autoRender; }
            set { autoRender = value; }
        }
        /// <summary>
        /// 绑定View中的Block名称
        /// </summary>
        [JsonProperty(PropertyName = "binding")]
        private string bindingBlock;
        /// <summary>
        /// 绑定View中的Block名称
        /// </summary>
        public string BindingBlock
        {
            get { return bindingBlock; }
            set { bindingBlock = value; }
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
        private DataTable viewBlockDataTable;
        /// <summary>
        /// 展示层Block的数据表
        /// </summary>
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
                    RaisePropertyChanged("ViewBlockDataTable");
                }
            }
        }
        /// <summary>
        /// 展示层Block当前选择行
        /// </summary>
        private DataRow preBlockSelectedRow;
        /// <summary>
        /// 展示层Block当前选择行
        /// </summary>
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
                    RaisePropertyChanged("PreBlockSelectedRow");
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
            if (string.IsNullOrEmpty(keywords))
            {
                ViewBlockDataTable = ViewBlock.BlockDt.Tables["dataArray"];
            }
            else
            {
                List<string> keywordsList = Keywords.Split('|').ToList();
                if (!keywordsList.Contains("_id"))
                {
                    keywordsList.Add("_id");
                }
                if (superiors.Equals("root"))
                {
                    ViewBlockDataTable = ViewBlock.BlockDt.Tables["dataArray"].DefaultView.ToTable("dataArray", true, keywordsList.ToArray());
                }
                else
                {

                }
            }
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
                DataRow[] drs = ViewBlock.BlockDt.Tables["dataArray"].Select(string.Format("_id='{0}'", dr["_id"].ToString()));
                ViewBlock.SetCurrentRow(drs[0]);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>

        public void SaveData()
        {
            ViewBlock.ViewModel.SaveData();
        }
    }
}
