﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pcet.plugin.sql.DataModel.Conversation
{
    public class AuthorizationDivision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RowId { get; set; }
        public string DivisionIds { get; set; }
    }
}
