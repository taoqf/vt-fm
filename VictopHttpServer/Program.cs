using System.ServiceProcess;

namespace VictopHttpServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new VicHttpServer()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
