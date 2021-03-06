﻿using System.Collections.Generic;
using pcet.plugin.csv.Mapping;

namespace pcet.plugin.csv.Model
{
    public class Participant : Reflectionable
    {
        public string participantId { get; set; }
        public string participantName { get; set; }
        public string userId { get; set; }
        public string purpose { get; set; }
        public string externalContactId { get; set; }
        public string externalOrganizationId { get; set; }
        public IList<Session> sessions { get; set; }
    }
}
