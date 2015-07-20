﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.ComLink.ICE
{
    using Ice;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using Victop.Frame.ComLink.ICE.Apps;
    using Victop.Frame.ComLink.ICE.Maps;
    using Victop.Frame.ComLink.ICE.Servants;
    using Victop.Frame.ComLink.ICE.Util;
    using Victop.Frame.CoreLibrary.AbsClasses;
    using Victop.Frame.CoreLibrary.Interfaces;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.CoreLibrary.VicException;
    using Victop.Frame.CoreLibrary.Common;
    using Victop.Frame.PublicLib.Helpers;
    using slice;

    /// <summary>
    /// ICE 通信管理器
    /// </summary>
    /// <remarks>ICE 通信管理器</remarks>
    public class ICEManager : IComlink
    {
        /// <summary>
        /// ICE配置文件
        /// </summary>
        private string iceConfigFile = "ice.cfg";
        private int messageSizeMax = 1024; // ICE传递消息的最大尺寸
        private string proxyID;
        /// <summary>
        /// 基类，事件触发调用该类对应的方法
        /// </summary>
        public Base BaseInfo = null;

        private RouterMap routerMaps;
        /// <summary>
        /// 路由适配器管理器
        /// </summary>
        public RouterMap RouterMaps
        {
            get
            {
                if (routerMaps == null)
                    routerMaps = new RouterMap();
                return routerMaps;
            }
            set
            {
                routerMaps = value;
            }
        }

        private ProxyMap serverProxyMaps;
        /// <summary>
        /// 服务器代理管理器
        /// </summary>
        public ProxyMap ServerProxyMaps
        {
            get
            {
                if (serverProxyMaps == null)
                    serverProxyMaps = new ProxyMap();
                return serverProxyMaps;
            }
            set
            {
                serverProxyMaps = value;
            }
        }
        private QueueMap queueMaps;
        /// <summary>
        /// 队列管理器
        /// </summary>
        public QueueMap QueueMaps
        {
            get
            {
                if (queueMaps == null)
                    queueMaps = new QueueMap();
                return queueMaps;
            }
            set
            {
                queueMaps = value;
            }
        }

        private CallbackMap callbackMaps;
        /// <summary>
        /// 存储回调的代理
        /// </summary>
        public CallbackMap CallbackMaps
        {
            get
            {
                if (callbackMaps == null)
                    callbackMaps = new CallbackMap();
                return callbackMaps;
            }
            set
            {
                callbackMaps = value;
            }
        }
        private ChannelMap channelMaps;
        /// <summary>
        /// 通道管理器(发送者与建立者的配对关系缓存)
        /// </summary>
        public ChannelMap ChannelMaps
        {
            get
            {
                if (channelMaps == null)
                    channelMaps = new ChannelMap();
                return channelMaps;
            }
            set
            {
                channelMaps = value;
            }
        }
        private IceApplication iceApp;
        /// <summary>
        /// ICE通信器实例
        /// </summary>
        public IceApplication ICEApp
        {
            get
            {
                if (iceApp == null)
                    iceApp = new IceApplication();
                return iceApp;
            }
            set
            {
                iceApp = value;
            }
        }
        /// <summary>
        /// ICE通信器主线程
        /// </summary>
        private Thread comLinkThread;
        /// <summary>
        /// ICE回调适配器
        /// </summary>
        private ObjectAdapter callBackAdapter;
        /// <summary>
        /// 通信器
        /// </summary>
        private Communicator commtor
        {
            get { return ICEApp.Communicator; }
        }
        /// <summary>
        /// ICE状态
        /// </summary>
        private int commStatus
        {
            get { return ICEApp.ICEStatus; }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public ReplyMessage SendMessage(RequestMessage messageInfo)
        {
            if (string.IsNullOrWhiteSpace(messageInfo.CurrentRecepitId) && string.IsNullOrWhiteSpace(messageInfo.TargetAddress))
            {
                throw new NoReceiptException(messageInfo);
            }
            ReplyMessage replyMessage = null;
            bool isAsync = false;
            int reSendTime = BaseInfo.ResendTimes;
            try
            {
                replyMessage = SendMessageToServer(messageInfo, isAsync);
            }
            catch (ConnectFailedException connFailed)
            {
                if (reSendTime > 0)
                {
                    // 如果是连接失败异常，重试发送
                    for (int i = 1; i <= reSendTime; i++)
                    {
                        try
                        {
                            replyMessage = SendMessageToServer(messageInfo, isAsync);
                            // 没有出现异常表示发送成功，跳出循环重试，返回应答消息
                            break;
                        }
                        catch (ConnectFailedException connFailedEx)
                        {
                            if (i >= reSendTime)
                            {
                                throw connFailedEx;
                            }
                        }
                    }
                }
                else
                {
                    throw connFailed;
                }
            }
            catch (ConnectionLostException connLost)
            {
                throw connLost.InnerException;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return replyMessage;
        }
        /// <summary>
        /// 发送消息，解析传入的消息是否存在路由信息，如果存在调用携带路由的消息发送，不存在则正常的发送消息.
        /// </summary>
        /// <param name="Msg">要传送的消息</param>
        /// <param name="isAsync">是否启动异步分派机制</param>
        /// <returns>返回的应答消息类</returns>
        private ReplyMessage SendMessageToServer(RequestMessage message, bool isAsync)
        {
            Reply reply;
            string receiptID = message.CurrentRecepitId;
            string routerAddress = message.RouterAddress;
            // 检查传入的消息是否携带路由信息，选择不用的发送模式
            if (!String.IsNullOrWhiteSpace(receiptID) || String.IsNullOrWhiteSpace(routerAddress))
            {
                reply = NormalSendMessage(message, isAsync);
            }
            else
            {
                reply = RouterSendMessage(message, isAsync);
            }
            return ResponseUtil.ToReplyMessage(reply);
        }

        /// <summary>
        /// 普通的消息发送，无路由，参数指定了发送方式为异步还是同步
        /// </summary>
        /// <param name="message">要发送的消息类</param>
        /// <param name="isAsync"> 是否使用异步分派的方式发送</param>
        /// <returns>返回的应答消息类</returns>
        private Reply NormalSendMessage(RequestMessage message, bool isAsync)
        {
            string messageID = message.MessageId;
            string serverAddress = message.TargetAddress;
            string receiptID = message.CurrentRecepitId;
            string senderID = message.CurrentSenderId;
            string messageType = message.MessageType;
            MessageEndpointPrx callbackProxy = null;
            MessageEndpointPrx serverProxy = null;
            bool isCallback = false;
            if (!String.IsNullOrWhiteSpace(receiptID))
            {
                callbackProxy = CallbackMaps.FindCallBack(receiptID);
            }
            if (null != callbackProxy)
            {
                // 回调方式发送
                isCallback = true;
                serverProxy = callbackProxy;
            }
            else
            {
                // 根据地址发送
                isCallback = false;
                serverProxy = CreateServerProxy(serverAddress, null);
                // 检查是否已经发送过回调代理
                if (!ChannelMaps.ContainChannelInfo(senderID, serverAddress, null))
                {
                    callbackProxy = CreateCallbackProxy(senderID);
                    //bool flag = serverProxy.setCallback(callbackProxy, senderID);
                    ChannelMaps.AddChannelInfo(senderID, serverAddress, null);
                }
            }
            message.ClientCallBackProxy = callbackProxy.ToString();
            // 发送
            return DefaultSendMessage(serverProxy, message, isAsync, isCallback);
        }
        /// <summary>
        /// 创建一个服务器代理,检查缓存中是否已经有服务器代理,有则直接取出,没有则新建一个服务器代理.
        /// </summary>
        /// <param name="serverAddress">接收地址</param>
        /// <param name="routerAddress">路由地址</param>
        /// <returns>接收方代理</returns>
        private MessageEndpointPrx CreateServerProxy(string serverAddress, string routerAddress)
        {
            MessageEndpointPrx serverProxy = null; ;
            proxyID = serverAddress + "@" + routerAddress;
            serverProxy = ServerProxyMaps.FindServerProxy(proxyID);
            if (serverProxy != null)
            {
                // 检查取出的代理是否有效，有效则直接将代理返回应用程序使用
                try
                {
                    RouterPrx routerProxy = (RouterPrx)serverProxy.ice_getRouter();
                    if (routerProxy != null)
                    {
                        routerProxy.addProxies(new ObjectPrx[] { serverProxy });
                    }
                    return serverProxy;
                }
                catch (System.Exception ex1)
                {

                }
            }
            // 没有维护有代理，或者是代理已经失效，重新建立一个新的代理
            if (String.IsNullOrWhiteSpace(routerAddress))
            {
                // 不使用路由的远端代理
                ObjectPrx serverPrx = commtor.stringToProxy(serverAddress);
                serverProxy = MessageEndpointPrxHelper.checkedCast(serverPrx);
            }
            else
            {
                // 使用了路由的远端代理
                RouterPrx router = null;
                try
                {

                    RouterEntry RouterEntry = RouterMaps.FindRouterEntry(routerAddress);
                    router = RouterEntry.RouterProxy;
                    // router.ice_ping();
                }
                catch (System.Exception ex2)
                {
                    // 本地维护的路由代理已经失效，重新获取一个路由代理
                    try
                    {
                        router = CreateRouter(routerAddress).RouterProxy;
                    }
                    catch (System.Exception ex3)
                    {
                        throw ex3;
                    }
                }
                // 创建远端服务器代理
                ObjectPrx serverPrx = commtor.stringToProxy(serverAddress).ice_router(router);
                serverProxy = MessageEndpointPrxHelper.uncheckedCast(serverPrx);
            }
            ServerProxyMaps.AddServerProxy(proxyID, serverProxy);
            return serverProxy;
        }

        /// <summary>
        /// 创建路由代理
        /// </summary>
        /// <param name="routerAddress">路由地址</param>
        /// <returns>路由单元</returns>
        private RouterEntry CreateRouter(string routerAddress)
        {
            RouterEntry routerEntry = new RouterEntry();
            // 建立与Glacier路由的代理
            ObjectPrx routerProxy = commtor.stringToProxy(routerAddress);
            Glacier2.RouterPrx routerPrx = Glacier2.RouterPrxHelper.checkedCast(routerProxy);
            // 建立与Glacier路由的会话
            routerPrx.createSession("CsharpReceiver", "");
            string category = routerPrx.getCategoryForClient();
            // 建立携带Glacier路由信息的路由适配器
            ObjectAdapter routerAdapter = commtor.createObjectAdapterWithRouter(category, routerPrx);
            // 激活路由适配器，用来加载通过该Glacier的回调Servant
            routerAdapter.activate();
            // 保存该Glacier路由的信息，并启动心跳线程保持通道畅通
            routerEntry.RouterAddress = routerAddress;
            routerEntry.RouterProxy = routerPrx;
            routerEntry.RouterAdapter = routerAdapter;
            routerEntry.StartRefresh();
            // 存入RouterMap进行维护
            RouterMaps.AddRouterEntry(routerAddress, routerEntry);
            return routerEntry;
        }
        /// <summary>
        /// 创建回调代理
        /// </summary>
        /// <param name="senderID">发送者的名称</param>
        /// <returns>回调代理</returns>
        private MessageEndpointPrx CreateCallbackProxy(string senderID)
        {
            Identity id = new Identity(senderID, "CallbackProxy");
            if (null == callBackAdapter)
            {
                // 创建回调适配器
                callBackAdapter = commtor.createObjectAdapterWithEndpoints("CallbackAdapter", "tcp");
                callBackAdapter.activate();
            }
            Ice.Object servant = callBackAdapter.find(id);
            if (null == servant)
            {
                // 创建回调实例
                servant = new MessageEndpointI(senderID, this);
                ObjectPrx proxy = callBackAdapter.add(servant, id);
                return MessageEndpointPrxHelper.uncheckedCast(proxy);
            }
            else
            {
                ObjectPrx proxy = callBackAdapter.createProxy(id);
                return MessageEndpointPrxHelper.uncheckedCast(proxy);
            }
        }

        /// <summary>
        /// 有路由的消息发送，使用路由代理，参数指定了发送方式为异步还是同步
        /// </summary>
        /// <param name="message">要发送的消息结构体</param>
        /// <param name="isAsync">是否分派的方式发送使用异步</param>
        /// <returns>返回的应答消息类</returns>
        private Reply RouterSendMessage(RequestMessage message, bool isAsync)
        {
            string messageID = message.MessageId;
            string routerAddress = message.RouterAddress;
            string serverAddress = message.TargetAddress;
            string receiptID = message.CurrentRecepitId;
            string senderID = message.CurrentSenderId;
            string messageType = message.MessageType;
            bool isCallback = false;
            MessageEndpointPrx serverProxy = CreateServerProxy(serverAddress, routerAddress);
            if (!ChannelMaps.ContainChannelInfo(senderID, serverAddress, routerAddress))
            {
                MessageEndpointPrx callbackProxy = CreateCallbackProxyWithRouter(routerAddress, senderID);
                //bool flag = serverProxy.setCallback(callbackProxy, senderID);
                //if (flag)
                //{
                //    ChannelMaps.AddChannelInfo(senderID, serverAddress, routerAddress);
                //}
                message.ClientCallBackProxy = callbackProxy.ToString();
            }
            // 发送
            return DefaultSendMessage(serverProxy, message, isAsync, isCallback);
        }

        /// <summary>
        /// 创建带路由的回调代理
        /// </summary>
        /// <param name="routerUrl">路由地址</param>
        /// <param name="senderID">发送者的名称</param>
        /// <returns>回调代理</returns>
        private MessageEndpointPrx CreateCallbackProxyWithRouter(string routerUrl, string senderID)
        {
            RouterEntry routerEntry = RouterMaps.FindRouterEntry(routerUrl); // 查询是否已经有对应该路由的信息
            if (null == routerEntry)
            {
                try
                {
                    // 创建一个对应该路由的路由代理，创建路由适配器，并激活
                    routerEntry = CreateRouter(routerUrl);
                }
                catch (System.Exception e)
                {
                    throw e;
                }
            }
            ObjectAdapter routerAdapter = routerEntry.RouterAdapter;
            Identity id = new Identity(senderID, routerAdapter.getName());
            Ice.Object servant = routerAdapter.find(id);
            if (null == servant)
            {
                // 创建回调实例
                servant = new MessageEndpointI(senderID, this);
                ObjectPrx proxy = routerAdapter.add(servant, id);
                return MessageEndpointPrxHelper.uncheckedCast(proxy);
            }
            else
            {
                ObjectPrx proxy = routerAdapter.createProxy(id);
                return MessageEndpointPrxHelper.uncheckedCast(proxy);
            }
        }

        /// <summary>
        /// 消息发送，检查消息长度，如果长度超过单次发送限制的90%，则分割消息，根据参数使用异步分派或直接发送
        /// </summary>
        /// <param name="serverProxy">服务器代理</param>
        /// <param name="message">消息</param>
        /// <param name="isAsync">异步分派</param>
        /// <param name="isCallback">回调标识</param>
        /// <returns></returns>
        private Reply DefaultSendMessage(MessageEndpointPrx serverProxy, RequestMessage message, bool isAsync, bool isCallback)
        {
            isCallback = true;
            // 格式化消息，发送
            Message msg = MessageUtil.SOAmsg2ICEmsg(message);
            try
            {
                int FragmentSize = (int)(messageSizeMax * 1024 * 0.9);
                if (Encoding.UTF8.GetBytes(message.MessageContent).Length > FragmentSize)
                {
                    // 消息超长，分段发送
                    return FragmentSendMessage(message.MessageContent, msg, FragmentSize, serverProxy, isCallback);
                }
                else
                {
                    if (isAsync)
                    {
                        serverProxy.begin_sendMessage(null, isCallback, msg, 1, 1, message.MessageContent);
                        return null;
                    }
                    else
                    {
                        Reply reply = serverProxy.sendMessage(null, isCallback, msg, 1, 1, message.MessageContent);
                        return reply;
                    }
                }
            }
            catch (ConnectFailedException ex)
            {
                string serverAddress = message.TargetAddress;
                string routerAddress = message.RouterAddress;
                string senderID = message.CurrentSenderId;
                // 清理缓存再发一次
                string ProxyID = serverAddress + "@" + routerAddress;
                ServerProxyMaps.DelServerProxy(ProxyID);
                ChannelMaps.RemoveChannelInfo(senderID);
                throw ex;
            }
        }
        /// <summary>
        /// 分段发送处理逻辑
        /// </summary>
        /// <param name="msgJson">格式化好的消息Json字符串</param>
        /// <param name="fragmentSize">片段最大长度</param>
        /// <param name="serverPrx">远端接收者代理</param>
        /// <param name="isCallback">是否回调方式发送</param>
        /// <returns>外部应用程序可使用的结构体</returns>
        private Reply FragmentSendMessage(string content, Message msg, int fragmentSize, MessageEndpointPrx serverPrx, bool isCallback)
        {
            content += "]";// 添加消息保护盖字符"]"，保证最后在空格不被过滤
            Reply reply = null; // 代理回复的应答消息
            int fragmentNum; // 片段数
            int index; // 循环发送次数
            int msgPos; // 截取消息片段的起始坐标
            string sendMsg; // 截取之后的片段，发送的数据
            //string Hash = Ice.Util.generateUUID(); // 产生一个标记用来标记消息片段
            string hash = System.Guid.NewGuid().ToString(); // 产生一个标记用来标记消息片段
            int messageLength = content.Length; // 获取消息总长度
            if (messageLength % fragmentSize > 0)
            {
                fragmentNum = messageLength / fragmentSize + 1;
            }
            else
            {
                fragmentNum = messageLength / fragmentSize;
            }
            for (index = 1; index <= fragmentNum; index++)
            {
                msgPos = (index - 1) * fragmentSize; // 计算片段起始坐标
                if (msgPos + fragmentSize > messageLength)
                {
                    // 最后一截片段
                    sendMsg = content.Substring(msgPos);
                }
                else
                {
                    // 截取消息片段
                    sendMsg = content.Substring(msgPos, fragmentSize);
                }
                reply = serverPrx.sendMessage(hash, isCallback, msg, index, fragmentNum, sendMsg);
            }

            return reply;
        }
        /// <summary>
        /// 通信器初始化
        /// </summary>
        public void Init(Base baseI)
        {
            this.BaseInfo = baseI;
            ComLinkRun();
        }
        /// <summary>
        /// 通信器运行
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private int ComLinkRun()
        {
            if (GetStatus() == 1)
            {
                return commStatus;
            }
            try
            {
                //Ice客户端 通讯器实例
                comLinkThread = new Thread(new ParameterizedThreadStart(CommunicatorThreadProc));
                comLinkThread.Start(new string[] { "" });
                int timeOut = 10000; // 设置等待时间10秒
                int timeSleep = 500; // 等待启动循环频率
                int time = 0;
                do
                {
                    if (commStatus > 0)
                    {
                        break;
                    }
                    if (time > timeOut)
                    {
                        throw new System.TimeoutException("Start the IcaApplication time out, wait " + timeOut + "s.");
                    }
                    time += timeSleep;
                    Thread.Sleep(timeSleep);
                } while (true);
            }
            catch (System.Exception ex)
            {
                return 0;
            }
            return commStatus;
        }
        /// <summary>
        /// ICE通讯器实例
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private void CommunicatorThreadProc(object args)
        {
            FileInfo file = new FileInfo(iceConfigFile);
            if (!file.Exists)
            {
                StreamWriter writer = new StreamWriter(file.OpenWrite());
                byte[] ice = Victop.Frame.ComLink.Properties.Resources.ice;
                string cfg = Encoding.UTF8.GetString(ice);
                writer.Write(cfg);
                writer.Flush();
                writer.Close();
            }
            iceApp.main((string[])args, iceConfigFile);
        }

        /// <summary>
        /// 获取通信器状态
        /// </summary>
        public int GetStatus()
        {
            return commStatus;
        }

        /// <summary>
        /// 重置通信器
        /// </summary>
        public void Reset()
        {
            throw new System.NotImplementedException(); //TODO:方法实现
        }

        /// <summary>
        /// 停止通信器
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Stop()
        {
            try
            {
                if (!commtor.isShutdown())
                {

                    iceApp.Stop();
                    Thread.Sleep(500);
                    if (comLinkThread != null)
                    {
                        comLinkThread.Abort();
                        comLinkThread = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                ServerProxyMaps.DelServerProxy(proxyID);
                callBackAdapter = null;
            }
        }

        /// <summary>
        /// 检查是否存在回调代理
        /// </summary>
        public bool CheckCallBackEndPoint(string target)
        {
            return CallbackMaps.CallbackMaps.ContainsKey(target);
        }
        /// <summary>
        /// 把A端点的回调代理，转交给B端点(p2p)
        /// </summary>
        /// <param name="sender">请求者</param>
        /// <param name="target">目标</param>
        /// <returns>结果</returns>
        public bool TransferEndPoint(string sender, string target)
        {
            bool flag = false;
            MessageEndpointPrx targetProxy = null;
            MessageEndpointPrx senderProxy = null;
            targetProxy = CallbackMaps.FindCallBack(target);
            if (null != targetProxy)
            {
                senderProxy = CallbackMaps.FindCallBack(sender);
                //flag = senderProxy.setCallback(targetProxy, target);
                flag = true;
            }
            return flag;
        }
        public string StartLocalServer(string ip, string port)
        {
            throw new NotImplementedException();
        }
    }
}

