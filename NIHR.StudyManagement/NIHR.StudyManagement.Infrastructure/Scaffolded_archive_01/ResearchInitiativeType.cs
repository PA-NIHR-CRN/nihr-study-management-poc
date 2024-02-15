using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class ResearchInitiativeType
    {
        public ResearchInitiativeType()
        {
            ResearchInitiatives = new HashSet<ResearchInitiative>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ResearchInitiative> ResearchInitiatives { get; set; }
    }
}
