using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded02
{
    public partial class ResearchInitiativeType : DbEntity
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
