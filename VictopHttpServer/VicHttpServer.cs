using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace VictopHttpServer
{
    public partial class VicHttpServer : ServiceBase
    {
        public Thread VicServerThread { get; set; }
        public VicHttpServer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            VicServerThread = new Thread(new ParameterizedThreadStart(RefreshSessionThreadProc));
            VicServerThread.Start(this);
        }
        protected override void OnStop()
        {
            VicServerThread.Abort();
        }

        private static void RefreshSessionThreadProc(System.Object threadArgument)
        {
            try
            {
                using (HttpListener listerner = new HttpListener())
                {
                    listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                    listerner.Prefixes.Add(ConfigurationManager.AppSettings["httpweb"].ToString());
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
                        if (ctx.Request.Url.AbsoluteUri.Contains(ConfigurationManager.AppSettings["httpweb"].ToString()))
                        {
                            Console.WriteLine(ctx.Request.Url.LocalPath);
                            HttpListenerRequest request = ctx.Request;
                            if (request.AcceptTypes != null)
                            {
                                foreach (string item in request.AcceptTypes)
                                {
                                    Console.WriteLine(item);
                                }
                            }
                            else
                            {
                                Console.WriteLine("AcceptTypes 是空");
                            }
                            string fileExtendName = ctx.Request.Url.LocalPath.Substring(ctx.Request.Url.LocalPath.LastIndexOf("."));
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
                
            }
        }
    }
}
