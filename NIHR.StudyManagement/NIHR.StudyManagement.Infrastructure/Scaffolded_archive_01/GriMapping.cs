using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class GriMapping
    {
        public int Id { get; set; }
        public int GriResearchStudyId { get; set; }
        public string ProjectId { get; set; } = null!;
        public int SourceSystemId { get; set; }

        public virtual GriResearchStudy GriResearchStudy { get; set; } = null!;
        public virtual SourceSystem SourceSystem { get; set; } = null!;
    }
}
