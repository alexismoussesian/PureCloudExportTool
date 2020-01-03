using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pcet.plugin.sql.DataModel.ParticipantAttr
{
    public class ParticipantAttr
    {
        [Key]
        [Column(Order = 1)]
        public string conversationId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string participantId { get; set; }
        [Key]
        [Column(Order = 3)]
        public string attrName { get; set; }
        public string attrValue { get; set; }
    }
}
