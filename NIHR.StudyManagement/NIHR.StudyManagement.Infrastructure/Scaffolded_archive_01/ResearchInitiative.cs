using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class ResearchInitiative
    {
        public ResearchInitiative()
        {
            GriResearchStudies = new HashSet<GriResearchStudy>();
        }

        public int Id { get; set; }
        public int? ResearchInitiativeTypeId { get; set; }

        public virtual ResearchInitiativeType? ResearchInitiativeType { get; set; }
        public virtual ICollection<GriResearchStudy> GriResearchStudies { get; set; }
    }
}
