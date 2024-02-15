using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class ResearchInitiativeIdentifier
    {
        public int Int { get; set; }
        public int SourceSystemId { get; set; }
        public string Value { get; set; } = null!;
        public int ResearchInitiativeIdentifierTypeId { get; set; }

        public virtual ResearchInitiativeIdentifierType ResearchInitiativeIdentifierType { get; set; } = null!;
        public virtual SourceSystem SourceSystem { get; set; } = null!;
    }
}
