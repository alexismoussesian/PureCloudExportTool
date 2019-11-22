using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCloudExportTool_Main
{
    public class Main
    {
        /// <summary>
        /// Plugins folder
        /// </summary>
        private const string PluginsFolder = "Plugins";
        private const int StatisticsInterval = 30;
        private const int MaxIntervalDurationDays = 7;

        /// <summary>
        /// Contains the list of plugins that were successfully loaded from the Plugins folder
        /// </summary>
        private static ICollection<IPlugin> _loadedPlugins;
        private static List<string[]> _pluginsCmdArgsHelp;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly PureCloud PureCloudObj = new PureCloud();

        /// <summary>
        /// List of stats to retrieve (i.e. Conversations, Queues, Users)
        /// </summary>
        private static List<string> _stats = new List<string>();

        private static readonly DateTime To = DateTime.UtcNow;
        private static DateTime ToForAggregates => new DateTime(To.Year, To.Month, To.Day, To.Hour, To.Minute >= 30 ? 30 : 0, 0);
        private static readonly string[] DateTimeFormats = { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" }; // - formats from documentation: /startdate=2015-12-31 or /startdate="2015-12-31 14:00:00"

    }
}
