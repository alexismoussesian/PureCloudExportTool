using System;
using System.ServiceModel;
using System.ServiceProcess;

namespace PureCloudExportTool
{
    public class Program : ServiceBase
    {
        public ServiceHost serviceHost = null;

        public Program()
        {
            ServiceName = "PureCloud Export Tool";
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new PureCloudExportTool_Service()
            };
            ServiceBase.Run(ServicesToRun);
        }

        protected override void OnStart(string[] args)
        {
            var service = new PureCloudExportTool_Service();

            Console.WriteLine("End Processing");
            Console.WriteLine("Press enter to exit...");

            Console.ReadLine();

        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }

    }
}
