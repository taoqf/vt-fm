using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using Victop.Frame.CoreLibrary;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.PublicLib.Managers;
using Victop.Server.Controls;
using Victop.Wpf.Controls;

namespace VictopPartner
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorInfo = string.Empty;
            string alertInfo = string.Empty;
            if (e.Exception.InnerException != null)
            {
                errorInfo = string.Format("异常源:{0},异常位置:{1},异常信息:{2}", e.Exception.InnerException.Source, e.Exception.InnerException.StackTrace, e.Exception.InnerException.Message);
                alertInfo = string.Format("{0}异常位置:{1}", e.Exception.Message, e.Exception.InnerException.StackTrace);
            }
            else
            {
                errorInfo = string.Format("异常源:{0},异常位置:{1},异常信息:{2}", e.Exception.Source, e.Exception.StackTrace, e.Exception.Message);
                alertInfo = string.Format("{0}异常位置:{1}", e.Exception.Message, e.Exception.StackTrace);
            }
            VicMessageBoxNormal.Show(alertInfo, "未知错误", MessageBoxButton.OK, MessageBoxImage.Error);
            LoggerHelper.Error(errorInfo);
            e.Handled = true;

        }
        private Thread VicServerThread { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            #region 自动更新
            if (!ConfigManager.GetAttributeOfNodeByName("Client", "Debug").Equals("1"))
            {
                string[] argsStr = Environment.GetCommandLineArgs();
                if (argsStr.Count() <= 1 || (argsStr.Count() > 1 && !Convert.ToBoolean(argsStr[1].ToString())))
                {
                    Process updatePro = Process.Start(ConfigurationManager.AppSettings["updateconfig"] + ".exe", Process.GetCurrentProcess().Id.ToString());
                    updatePro.WaitForExit();
                }
                #region 单例判断
                Process[] processes = Process.GetProcessesByName("VictopPartner");
                if (processes.Length > 1)
                {
                    string appName = ConfigManager.GetAttributeOfNodeByName("System", "AppName");
                    MessageBox.Show(appName + "已在运行中……", "提示", MessageBoxButton.OK, MessageBoxImage.Stop);
                    Thread.Sleep(1000);
                    Environment.Exit(1);
                }
                #endregion
            }
            #endregion
            #region 启动内置HttpServer
            VicServerThread = new Thread(new ParameterizedThreadStart(RefreshSessionThreadProc));
            VicServerThread.Start(this);
            #endregion
            if (FrameInit.GetInstance().FrameRun())
            {
                try
                {
                    this.GetCurrentSkin();
                    string mainPlugin = ConfigurationManager.AppSettings["portalWindow"];
                    Assembly pluginAssembly = ServerFactory.GetServerAssemblyByName(mainPlugin, "");
                    Type[] types = pluginAssembly.GetTypes();
                    foreach (Type t in types)
                    {
                        if (IsValidPlugin(t))
                        {
                            IPlugin plugin = (IPlugin)pluginAssembly.CreateInstance(t.FullName);
                            plugin.StartWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            plugin.StartWindow.ShowDialog();
                            string devPluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["devpluginpath"]);
                            if (Directory.Exists(devPluginPath))
                            {
                                DeleteFolder(devPluginPath);
                            }
                            FrameInit.GetInstance().FrameUnload();
                            Environment.Exit(0);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    FrameInit.GetInstance().FrameUnload();
                    VicServerThread.Abort();
                    Environment.Exit(0);
                }
            }
        }
        /// <summary> 
        /// 删除文件夹及其内容 
        /// </summary> 
        /// <param name="path"></param> 
        public void DeleteFolder(string path)
        {
            foreach (string d in Directory.GetFileSystemEntries(path))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件 
                }
                else
                {
                    DeleteFolder(d);////递归删除子文件夹 
                }
            }
            Directory.Delete(path);
        }
        /// <summary>
        /// 是否为有效的插件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsValidPlugin(Type type)
        {
            bool ret = false;
            Type[] interfaces = type.GetInterfaces();
            foreach (Type theInterface in interfaces)
            {
                if (theInterface.FullName == "Victop.Server.Controls.IPlugin")
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }
        private void RefreshSessionThreadProc(System.Object threadArgument)
        {
            try
            {
                using (HttpListener listerner = new HttpListener())
                {
                    string httpweb = ConfigManager.GetLocalHttpServerBaseUrl();
                    listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                    listerner.Prefixes.Add(httpweb);
                    listerner.Start();
                    Console.WriteLine("WEB服务启动完成……");
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    while (true)
                    {
                        HttpListenerContext ctx = listerner.GetContext();
                        ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
                        if (ctx.Response.Headers.AllKeys.Contains("Access-Control-Allow-Origin"))
                        {
                            ctx.Response.Headers.Set("Access-Control-Allow-Origin", "*");
                        }
                        else
                        {
                            ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                        }
                        if (ctx.Request.Url.AbsoluteUri.Contains(httpweb))
                        {
                            //Console.WriteLine(ctx.Request.Url.LocalPath);
                            //HttpListenerRequest request = ctx.Request;
                            //if (request.AcceptTypes != null)
                            //{
                            //    foreach (string item in request.AcceptTypes)
                            //    {
                            //        Console.WriteLine(item);
                            //    }
                            //}
                            //else
                            //{
                            //    Console.WriteLine("AcceptTypes 是空");
                            //}
                            int fileExtendIndex = ctx.Request.Url.LocalPath.LastIndexOf(".");
                            string fileExtendName = fileExtendIndex != -1 ? ctx.Request.Url.LocalPath.Substring(ctx.Request.Url.LocalPath.LastIndexOf(".")) : "html";
                            HttpListenerResponse response = ctx.Response;
                            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + ctx.Request.Url.LocalPath))
                            {
                                byte[] responseString = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + ctx.Request.Url.LocalPath);
                                // 设置回应头部内容，长度，编码
                                response.ContentLength64 = responseString.LongLength;
                                response.ContentType = string.Format("{0};charset=UTF-8", FileTypeHelper.GetMimeType(fileExtendName));
                                System.IO.Stream output = response.OutputStream;
                                output.Write(responseString, 0, responseString.Length);
                                // 必须关闭输出流
                                output.Close();
                                ctx.Response.Close();
                            }
                            else
                            {
                                string responseString = "请求的内容不存在";
                                StreamWriter writer = new StreamWriter(ctx.Response.OutputStream, UTF8Encoding.UTF8);
                                writer.WriteLine(responseString);
                                writer.Close();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LoggerHelper.ErrorFormat("httpServer Error:{0}", string.IsNullOrEmpty(ex.Message) ? ex.InnerException.Message : ex.Message);
                //VicServerThread.Start(this);
            }
        }

        #region 换肤
        /// <summary>
        /// 获取当前皮肤
        /// </summary>
        private void GetCurrentSkin()
        {
            string skinName = ConfigManager.GetAttributeOfNodeByName("UserInfo", "UserSkin");
            string skinNamespace = System.IO.Path.GetFileNameWithoutExtension(string.Format("theme\\{0}.dll", skinName));//得到皮肤命名空间
            this.ChangeFrameWorkTheme("/" + skinNamespace + ";component/Styles.xaml", skinName);
        }
        /// <summary>
        /// 主题皮肤改变发送消息
        /// </summary>
        private void ChangeFrameWorkTheme(string ThemeName, string SkinPath)
        {
            string messageType = "ServerCenterService.ChangeThemeByDll";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
            ServiceParams.Add("SourceName", ThemeName);
            ServiceParams.Add("SkinPath", SkinPath);
            contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendMessage(messageType, contentDic);
        }
        #endregion
    }
}
