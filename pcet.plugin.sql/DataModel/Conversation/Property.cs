using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pcet.plugin.sql.DataModel.Conversation
{
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RowId { get; set; }
        public string propertyType { get; set; }
        public string property { get; set; }
        public string value { get; set; }
    }
}
