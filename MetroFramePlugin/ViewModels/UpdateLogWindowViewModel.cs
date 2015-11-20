using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Victop.Frame.DataMessageManager;
using System.Data;
using MetroFramePlugin.Models;
using System.Collections.ObjectModel;

namespace MetroFramePlugin.ViewModels
{
    public class UpdateLogWindowViewModel : ModelBase
    {
        #region 字段
        /// <summary>
        /// 最新更新记录
        /// </summary>
        private UpdateLogModel firstLogModel;
        /// <summary>
        /// 日志记录集合
        /// </summary>
        private ObservableCollection<UpdateLogModel> logInfoList;
        #endregion
        #region 属性
        /// <summary>
        /// 最新更新记录
        /// </summary>
        public UpdateLogModel FirstLogModel
        {
            get
            {
                if (firstLogModel == null)
                    firstLogModel = new UpdateLogModel();
                return firstLogModel;
            }
            set
            {
                if (firstLogModel != value)
                {
                    firstLogModel = value;
                    RaisePropertyChanged("FirstLogModel");
                }
            }
        }
        /// <summary>
        /// 日志记录集合
        /// </summary>
        public ObservableCollection<UpdateLogModel> LogInfoList
        {
            get
            {
                if (logInfoList == null)
                    logInfoList = new ObservableCollection<UpdateLogModel>();
                return logInfoList;
            }
            set
            {
                if (logInfoList != value)
                {
                    logInfoList = value;
                    RaisePropertyChanged("LogInfoList");
                }
            }
        }
        #endregion
        #region 命令
        public ICommand mainViewLoadedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string MessageType = "MongoDataChannelService.findBusiData";
                    DataMessageOperation messageOp = new DataMessageOperation();
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", "11");
                    contentDic.Add("modelid", "feidao-model-update_log-0001");
                    List<object> conditionList = new List<object>();
                    Dictionary<string, object> conditionDic = new Dictionary<string, object>();
                    conditionDic.Add("name", "update_log");
                    List<object> tableConList = new List<object>();
                    Dictionary<string, object> tableDic = new Dictionary<string, object>();
                    tableDic.Add("client_type", "3");
                    tableDic.Add("productid", "feidao");
                    tableConList.Add(tableDic);
                    conditionDic.Add("tablecondition", tableConList);
                    Dictionary<string, object> sortDic = new Dictionary<string, object>();
                    sortDic.Add("update_date", -1);
                    conditionDic.Add("sort", sortDic);
                    Dictionary<string, object> pageDic = new Dictionary<string, object>();
                    pageDic.Add("size", 10);
                    pageDic.Add("index", 1);
                    conditionDic.Add("paging", pageDic);
                    conditionList.Add(conditionDic);
                    contentDic.Add("conditions", conditionList);
                    Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
                    if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
                    {
                        string viewId = returnDic["DataChannelId"].ToString();
                        DataSet mastDs = new DataSet();
                        mastDs = messageOp.GetData(viewId, "[\"update_log\"]");
                        DataTable logDt = mastDs.Tables["dataArray"];
                        OrginizeLogModel(logDt);
                    }
                });
            }
        }
        #endregion
        #region 私有方法
        private void OrginizeLogModel(DataTable logDt)
        {
            if (logDt != null && logDt.Rows.Count > 0)
            {
                for (int i = 0; i < logDt.Rows.Count; i++)
                {
                    UpdateLogModel logModel = new UpdateLogModel();
                    logModel.LogVersion = "飞道体系" + logDt.Rows[i]["version_code"].ToString() + "(" + logDt.Rows[i]["version"].ToString() + ")";
                    logModel.LogDate = ((DateTime)logDt.Rows[i]["update_date"]).ToString("yyyy年MM月dd日 HH时mm分ss秒");
                    logModel.LogCreater = logDt.Rows[i]["creater"].ToString();
                    logModel.LogContent = logDt.Rows[i]["update_content"].ToString();
                    LogInfoList.Add(logModel);
                    //if (i == 0) 
                    //{
                    //    FirstLogModel.LogVersion = "飞道体系"+logDt.Rows[0]["version_code"].ToString() + "(" + logDt.Rows[0]["version"].ToString()+")更新日志";
                    //    FirstLogModel.LogDate = ((DateTime)logDt.Rows[0]["update_date"]).ToString("yyyy年MM月dd日 HH时mm分ss秒");
                    //    FirstLogModel.LogCreater = logDt.Rows[0]["creater"].ToString();
                    //    FirstLogModel.LogContent = logDt.Rows[0]["update_content"].ToString();
                    //}
                    //else
                    //{
                    //    UpdateLogModel logModel = new UpdateLogModel();
                    //    logModel.LogVersion = "飞道体系" + logDt.Rows[i]["version_code"].ToString() + "(" + logDt.Rows[i]["version"].ToString() + ")更新日志";
                    //    logModel.LogDate = ((DateTime)logDt.Rows[i]["update_date"]).ToString("yyyy年MM月dd日 HH时mm分ss秒");
                    //    logModel.LogCreater = logDt.Rows[i]["creater"].ToString();
                    //    logModel.LogContent = logDt.Rows[i]["update_content"].ToString();
                    //    LogInfoList.Add(logModel);
                    //}
                }
            }
        }
        #endregion
    }
}
