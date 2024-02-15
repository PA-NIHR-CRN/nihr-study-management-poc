using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext
{
    public partial class ResearchInitiativeTeamMemberDb : DbEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int ResearchInitiativeId { get; set; }
        public int RoleId { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public virtual PersonDb Person { get; set; } = null!;
        public virtual ResearchInitiativeDb ResearchInitiative { get; set; } = null!;
        public virtual RoleDb Role { get; set; } = null!;
    }
}
