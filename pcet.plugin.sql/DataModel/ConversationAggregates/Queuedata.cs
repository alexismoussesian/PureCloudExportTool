using System.Collections.Generic;

namespace pcet.plugin.sql.DataModel.ConversationAggregates
{
    class Queuedata
    {
        public Group group { get; set; }
        public IList<Datum> data { get; set; }
    }
}
