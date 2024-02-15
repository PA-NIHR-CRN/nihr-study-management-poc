using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class SourceSystem
    {
        public SourceSystem()
        {
            GriMappings = new HashSet<GriMapping>();
            GriResearchStudies = new HashSet<GriResearchStudy>();
            ResearchInitiativeIdentifiers = new HashSet<ResearchInitiativeIdentifier>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<GriMapping> GriMappings { get; set; }
        public virtual ICollection<GriResearchStudy> GriResearchStudies { get; set; }
        public virtual ICollection<ResearchInitiativeIdentifier> ResearchInitiativeIdentifiers { get; set; }
    }
}
