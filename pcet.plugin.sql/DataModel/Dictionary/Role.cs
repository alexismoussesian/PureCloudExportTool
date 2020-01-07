using System.ComponentModel.DataAnnotations;

namespace pcet.plugin.sql.DataModel.Dictionary
{
    public class Role
    {
        [Key]
        public string id { get; set; }
        public string name { get; set; }
    }
}
