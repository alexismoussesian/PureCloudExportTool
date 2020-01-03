using System.Collections.Generic;

namespace pcet.plugin.sql.DataModel.UserAggregates
{
    class Userdata
    {
        public Group group { get; set; }
        public IList<Datum> data { get; set; }
    }
}
