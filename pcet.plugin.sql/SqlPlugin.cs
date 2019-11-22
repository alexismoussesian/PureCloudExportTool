using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using log4net;
using pcet.plugin.sql.DataModel.Conversation;
using pcet.plugin.sql.DataModel.ConversationAggregates;
using pcet.plugin.sql.DataModel.Dictionary;
using pcet.plugin.sql.DataModel.Interval;
using pcet.plugin.sql.DataModel.ParticipantAttr;
using pcet.plugin.sql.DataModel.UserAggregates;
using pcet.plugin.sql.DataModel.UserDetails;
using pcet.plugin.sql.DataModel.GroupMember;
using pcet.plugin.sql.DataModel.ConversationMetric;
using pcet.plugin.sql.DataModel.DataTableRow;
using pcet.plugins;

namespace pcet.plugin.sql
{
    public class SqlPlugin : IPlugin
    {
        private const string ConnectionStringArgName = "target-sql";
        private const string AttachParticipantAttrsArgName = "participant-attrs";
        private string ConnectionString => _cmdArgs[ConnectionStringArgName];

        public bool AttachParticipantAttrs
        {
            get
            {
                var result = false;
                if (_cmdArgs.ContainsKey(AttachParticipantAttrsArgName))
                {
                    bool.TryParse(_cmdArgs[AttachParticipantAttrsArgName], out result);
                }
                return result;
            }
        }
        private Dictionary<string, string> _cmdArgs;
        private static readonly ILog Trace = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Initialize(string[] cmdArgs)
        {
            Trace.Info("Initializing");
            _cmdArgs = ParseCmdArgs(cmdArgs);
        }

        public string[] GetCommandLineParameters() => new[] { $"/{ConnectionStringArgName}", $"/{AttachParticipantAttrsArgName}" };

        public string[] GetCommandLineParametersHelp() => new[] { $"/{ConnectionStringArgName}={{SQL Server connection string}}", $"/{AttachParticipantAttrsArgName}={{A type of boolean, optional parameter. It controls whether participant attributes have to be exported.}}" };

        public List<string> SupportedStats() => new List<string>() { "conversations", "queues", "users", "userdetails" };

        public DateTime GetLatestInterval()
        {
            return IntervalManager.GetIntervalUtc(Interval.IntervalTypes.Lastinterval, ConnectionString);
        }

        public DateTime GetLatestIntervalForAggregates()
        {
            return IntervalManager.GetIntervalUtc(Interval.IntervalTypes.LastIntervalForAggregates, ConnectionString);
        }

        public void SetLatestInterval(DateTime dateTime)
        {
            IntervalManager.SetIntervalUtc(Interval.IntervalTypes.Lastinterval, dateTime, ConnectionString);
        }

        public void SetLatestIntervalForAggregates(DateTime dateTime)
        {
            IntervalManager.SetIntervalUtc(Interval.IntervalTypes.LastIntervalForAggregates, dateTime, ConnectionString);
        }

        public void ResetInterval(DateTime dateTime)
        {
            // what to do here?
        }

        public void InitializeDictionaries(Dictionary<string, string> queues, Dictionary<string, string> languages, Dictionary<string, string> skills, Dictionary<string, string> users, Dictionary<string, string> wrapUpCodes, Dictionary<string, string> edgeServers, Dictionary<string, string> campaigns, Dictionary<string, string> contactLists, Dictionary<string, string> presences, Dictionary<string, string> divisions, Dictionary<string, string> dataTables, Dictionary<string, string> groups)
        {
            Trace.Info($"InitializeDictionaries(), queues:{queues?.Count}, languages:{languages?.Count}, skills:{skills?.Count}, users:{users?.Count}, wrap up codes: {wrapUpCodes?.Count}, edge servers: {edgeServers?.Count}, campaigns: {campaigns?.Count}, contactLists: {contactLists?.Count}, presenceDefinitions: {presences?.Count}, divisions:{divisions?.Count}, dataTables:{dataTables?.Count}, groups:{groups?.Count}");
            DictionaryManager.SaveQueues(queues, ConnectionString);
            DictionaryManager.SaveLanguages(languages, ConnectionString);
            DictionaryManager.SaveSkills(skills, ConnectionString);
            DictionaryManager.SaveUsers(users, ConnectionString);
            DictionaryManager.SaveWrapUpCodes(wrapUpCodes, ConnectionString);
            DictionaryManager.SaveEdgeServers(edgeServers, ConnectionString);
            DictionaryManager.SaveCampaigns(campaigns, ConnectionString);
            DictionaryManager.SaveContactLists(contactLists, ConnectionString);
            DictionaryManager.SavePresenceDefinitions(presences, ConnectionString);
            DictionaryManager.SaveDivisions(divisions, ConnectionString);
            DictionaryManager.SaveDataTables(dataTables, ConnectionString);
            DictionaryManager.SaveGroups(groups, ConnectionString);
        }

        public bool PushData(string data)
        {
            // data = System.IO.File.ReadAllText("C:\\temp\\TemProjectFormPcsd\\conv.txt"); // <- loading JSON from file for tests
            Trace.Info($"PushData(), data length: {data.Length}");
            if (string.IsNullOrEmpty(data))
            {
                Trace.Error("PushData() was executed with empty content, exiting... :-(");
                return false;
            }
            // http://stackoverflow.com/questions/1151987/can-i-set-an-unlimited-length-for-maxjsonlength-in-web-config
            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };

            var dataType = ((serializer).Deserialize<Dictionary<string, object>>(data)).Keys.FirstOrDefault();
            Trace.Info($"Detected data type: {dataType}");
            switch (dataType)
            {
                case "conversations":
                    ConversationManager.SaveConversations(ConversationManager.ParseConversations(data), ConnectionString);
                    break;
                case "participantattrs":
                    ParticipantAttrManager.SaveAttrs(ParticipantAttrManager.ParseAttrs(data), ConnectionString);
                    break;
                case "conversationmetrics":
                    ConversationMetricManager.SaveMetrics(ConversationMetricManager.ParseMetrics(data), ConnectionString);
                    break;
                case "userdetailsdata":
                    UserDetailsManager.SaveUserDetails(UserDetailsManager.ParseUserDetails(data), ConnectionString);
                    break;
                case "userdata":
                    UserAggregatesManager.SaveUserAggregates(UserAggregatesManager.ParseUserAggregates(data), ConnectionString);
                    break;
                case "queuedata":
                    ConversationAggregatesManager.SaveConversationAggregates(ConversationAggregatesManager.ParseConversationAggregates(data), ConnectionString);
                    break;
                default:
                    Trace.Error("Unsupported data type. :-(");
                    break;
            }
            return true;
        }


        public bool PushGroupMembers(string groupMember)
        {
            List<GroupMember> ListOfGroupMember = new List<GroupMember>();

            var groupMembers = groupMember.Split('$');

            foreach (var members in groupMembers)
            {
                var items = members.Split('|');
                ListOfGroupMember.Add(new GroupMember { groupId = items[0], id = items[1], name = items[2] });
            }

            GroupMemberManager.SaveGroupMembers(ListOfGroupMember, ConnectionString);

            return true;
        }


        public bool PushDataTableRows(string dataTableRowsComplete)
        {
            List<DataTableRows> ListOfDTRows = new List<DataTableRows>();

            var dataTableRow = dataTableRowsComplete.Split('£');

            foreach (var row in dataTableRow)
            {
                var items = row.Split('@');
                ListOfDTRows.Add(new DataTableRows { dataTableId = items[0], dataTableRows = items[1] });
            }

            DataTableRowManager.SaveGroupMembers(ListOfDTRows, ConnectionString);

            return true;
        }

        public void Dispose()
        {
            Trace.Info("Dispose()");
            // todo: not sure do I have to do anything here
        }

        public List<string> OngoingConversationIdList() { return ConversationManager.GetOngoingConversationIdList(ConnectionString); }

        private Dictionary<string, string> ParseCmdArgs(IEnumerable<string> cmdArgs) { return cmdArgs.ToDictionary(cmdArg => cmdArg.Substring(1, cmdArg.IndexOf("=", StringComparison.Ordinal) - 1), cmdArg => cmdArg.Substring(cmdArg.IndexOf("=", StringComparison.Ordinal) + 1, cmdArg.Length - cmdArg.IndexOf("=", StringComparison.Ordinal) - 1)); }

    }
}
