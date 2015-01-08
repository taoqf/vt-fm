using SystemTestingPlugin.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using Victop.Frame.DataChannel;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;
using System.Diagnostics;

namespace SystemTestingPlugin.ViewModels
{
    public class UCAreaWindowViewModel : ModelBase
    {
        #region 字段定义
        /// <summary>
        /// 数据信息实体实例
        /// </summary>
        private DataSearchInfoModel dataInfoModel;
        /// <summary>
        /// 编码服务实体实例
        /// </summary>
        private CodeServiceInfoModel codeInfoModel;
        /// <summary>
        /// 用户信息实体实例
        /// </summary>
        private UserMenuInfoModel userInfoModel;
        /// <summary>
        /// 其他信息实体实例
        /// </summary>
        private OtherInfoModel otherInfoModel;
        #endregion
        #region 属性定义
        /// <summary>
        /// 数据信息实体实例
        /// </summary>
        public DataSearchInfoModel DataInfoModel
        {
            get
            {
                if (dataInfoModel == null)
                    dataInfoModel = new DataSearchInfoModel();
                return dataInfoModel;
            }
            set
            {
                if (dataInfoModel != value)
                {
                    dataInfoModel = value;
                    RaisePropertyChanged("DataInfoModel");
                }
            }
        }
        /// <summary>
        /// 编码服务实体实例
        /// </summary>
        public CodeServiceInfoModel CodeInfoModel
        {
            get
            {
                if (codeInfoModel == null)
                    codeInfoModel = new CodeServiceInfoModel();
                return codeInfoModel;
            }
            set
            {
                if (codeInfoModel != value)
                {
                    codeInfoModel = value;
                    RaisePropertyChanged("CodeInfoModel");
                }
            }
        }
        /// <summary>
        /// 用户信息实体实例
        /// </summary>
        public UserMenuInfoModel UserInfoModel
        {
            get
            {
                if (userInfoModel == null)
                    userInfoModel = new UserMenuInfoModel();
                return userInfoModel;
            }
            set
            {
                if (userInfoModel != value)
                {
                    userInfoModel = value;
                    RaisePropertyChanged("UserInfoModel");
                }
            }
        }
        /// <summary>
        /// 其他消息实体实例
        /// </summary>
        public OtherInfoModel OtherInfoModel
        {
            get
            {
                if (otherInfoModel == null)
                    otherInfoModel = new OtherInfoModel();
                return otherInfoModel;
            }
            set
            {
                if (otherInfoModel != value)
                {
                    otherInfoModel = value;
                    RaisePropertyChanged("OtherInfoModel");
                }
            }
        }
        #endregion
        #region 命令
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand btnSearchClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        string MessageType = "MongoDataChannelService.findBusiData";
                        MessageOperation messageOp = new MessageOperation();
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", DataInfoModel.SystemId);
                        contentDic.Add("configsystemid", DataInfoModel.ConfigsystemId);
                        contentDic.Add("modelid", DataInfoModel.ModelId);
                        if (!string.IsNullOrEmpty(DataInfoModel.SpaceId))
                        {
                            contentDic.Add("spaceId", DataInfoModel.SpaceId);
                        }
                        if (!string.IsNullOrEmpty(DataInfoModel.ConditionStr))
                        {
                            List<Dictionary<string, object>> conList = JsonHelper.ToObject<List<Dictionary<string, object>>>(DataInfoModel.ConditionStr);
                            if (conList != null)
                            {
                                contentDic.Add("conditions", conList);
                            }
                        }
                        Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                        if (returnDic != null)
                        {
                            DataOperation dataOp = new DataOperation();
                            DataInfoModel.ChannelId = returnDic["DataChannelId"].ToString();
                            if (string.IsNullOrEmpty(DataInfoModel.DataPath))
                            {
                                List<object> pathList = new List<object>();
                                pathList.Add(DataInfoModel.TableName);
                                DataInfoModel.DataPath = JsonHelper.ToJson(pathList);
                            }
                            DataSet mastDs = new DataSet();
                            Stopwatch watch = new Stopwatch();
                            watch.Start();
                            mastDs = dataOp.GetData(DataInfoModel.ChannelId, DataInfoModel.DataPath);
                            watch.Stop();
                            VicMessageBoxNormal.Show(watch.ElapsedMilliseconds.ToString());
                            DataInfoModel.ResultDataTable = mastDs.Tables["dataArray"];
                        }
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, () =>
                {
                    bool result = true;
                    if (string.IsNullOrEmpty(DataInfoModel.ModelId))
                    {
                        DataInfoModel.VertifyMsg = "请输入ModelId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(DataInfoModel.SystemId))
                    {
                        DataInfoModel.VertifyMsg = "请输入SystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(DataInfoModel.ConfigsystemId))
                    {
                        DataInfoModel.VertifyMsg = "请输入ConfigsystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(DataInfoModel.TableName))
                    {
                        DataInfoModel.VertifyMsg = "请输入TableName";
                        return false;
                    }
                    DataInfoModel.VertifyMsg = string.Empty;
                    return result;
                });
            }
        }
        /// <summary>
        /// 查看Json数据
        /// </summary>
        public ICommand btnViewJsonDataClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        DataOperation dataOp = new DataOperation();
                        DataInfoModel.JsonData = dataOp.GetJSONData(DataInfoModel.ChannelId);
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, () =>
                {
                    return !string.IsNullOrEmpty(DataInfoModel.ChannelId);
                });
            }
        }
        /// <summary>
        /// 执行编码服务
        /// </summary>
        public ICommand btnExcuteCodeServiceClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        MessageOperation messageOp = new MessageOperation();
                        string MessageType = "MongoDataChannelService.findDocCode";
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", CodeInfoModel.SystemId);
                        contentDic.Add("configsystemid", CodeInfoModel.ConfigsystemId);
                        contentDic.Add("pname", CodeInfoModel.PName);
                        contentDic.Add("setinfo", CodeInfoModel.SetInfo);
                        Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                        if (returnDic != null)
                        {
                            CodeInfoModel.ResultData = returnDic["ReplyContent"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, () =>
                {
                    bool result = true;
                    if (string.IsNullOrEmpty(CodeInfoModel.SystemId))
                    {
                        CodeInfoModel.VertifyMsg = "请输入SystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(CodeInfoModel.ConfigsystemId))
                    {
                        CodeInfoModel.VertifyMsg = "请输入ConfigsystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(CodeInfoModel.PName))
                    {
                        CodeInfoModel.VertifyMsg = "请输入PName";
                        return false;
                    }
                    if (string.IsNullOrEmpty(CodeInfoModel.SetInfo))
                    {
                        CodeInfoModel.VertifyMsg = "请输入SetInfo";
                        return false;
                    }
                    CodeInfoModel.VertifyMsg = string.Empty;
                    return result;
                });
            }
        }
        /// <summary>
        /// 用户菜单信息
        /// </summary>
        public ICommand tBoxUserSearchClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        MessageOperation messageOp = new MessageOperation();
                        string MessageType = "MongoDataChannelService.afterLogin";
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("systemid", UserInfoModel.SystemId);
                        contentDic.Add("configsystemid", UserInfoModel.ConfigsystemId);
                        contentDic.Add("client_type_val", UserInfoModel.ClientType);
                        contentDic.Add("userCode", UserInfoModel.UserCode);
                        Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                        if (returnDic != null)
                        {
                            UserInfoModel.ResultData = returnDic["ReplyContent"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                }, () =>
                {
                    bool result = true;
                    if (string.IsNullOrEmpty(UserInfoModel.SystemId))
                    {
                        UserInfoModel.VertifyMsg = "请输入SystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(UserInfoModel.ConfigsystemId))
                    {
                        UserInfoModel.VertifyMsg = "请输入ConfigsystemId";
                        return false;
                    }
                    if (string.IsNullOrEmpty(UserInfoModel.ClientType))
                    {
                        UserInfoModel.VertifyMsg = "请输入ClientType";
                        return false;
                    }
                    if (string.IsNullOrEmpty(UserInfoModel.UserCode))
                    {
                        UserInfoModel.VertifyMsg = "请输入UserCode";
                        return false;
                    }
                    UserInfoModel.VertifyMsg = string.Empty;
                    return result;
                });
            }
        }
        /// <summary>
        /// 其他信息
        /// </summary>
        public ICommand btnOtherExcuteClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        MessageOperation messageOp = new MessageOperation();
                        Dictionary<string, object> contentDic = JsonHelper.ToObject<Dictionary<string, object>>(OtherInfoModel.OtherConditionData);
                        Dictionary<string, object> returnDic = messageOp.SendMessage(OtherInfoModel.MessageType, contentDic, "JSON");
                        if (returnDic != null)
                        {
                            OtherInfoModel.OtherResultData = returnDic["ReplyContent"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        VicMessageBoxNormal.Show(ex.Message);
                    }
                });
            }
        }

        #endregion
    }
}
