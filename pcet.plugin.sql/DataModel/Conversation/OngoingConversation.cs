using System;
using System.ComponentModel.DataAnnotations;

namespace pcet.plugin.sql.DataModel.Conversation
{
    public class OngoingConversation
    {
        [Key]
        public string conversationId { get; set; }
        public DateTime? putIntoTheBagUtc { get; set; } = DateTime.UtcNow;
    }
}
