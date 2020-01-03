using System.Collections.Generic;

namespace pcet.plugin.sql.DataModel.ConversationAggregates
{
    class Datum
    {
        public string interval { get; set; }
        public IList<Metric> metrics { get; set; }
    }
}
