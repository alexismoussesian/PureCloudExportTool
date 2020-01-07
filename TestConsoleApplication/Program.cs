using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureCloudExportTool_Main;
using PureCloudExportTool;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //var service = PureCloudExportTool_Main.Main.Begin(args);
            //var service = new PureCloudExportTool_Service();
            var service = new Main();

            Console.WriteLine("End Processing");
            Console.WriteLine("Press enter to exit...");

            Console.ReadLine();

        }
    }
}
