using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class ResearchStudyTeamMember
    {
        public int Id { get; set; }
        public int GriMappingId { get; set; }
        public int ResearcherId { get; set; }
        public int PersonRoleId { get; set; }

        public virtual GriResearchStudy GriMapping { get; set; } = null!;
        public virtual PersonRole PersonRole { get; set; } = null!;
        public virtual Researcher Researcher { get; set; } = null!;
    }
}
