using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext
{
    public partial class SourceSystemLinkedIdentifierDb : DbEntity
    {
        public int Id { get; set; }
        public int ResearchInitiativeId { get; set; }
        public string LocalSystemIdentifier { get; set; } = null!;
        public int SourceSystemId { get; set; }

        public virtual SourceSystemDb LocalSystems { get; set; } = null!;
        public virtual ResearchInitiativeDb ResearchInitiative { get; set; } = null!;
    }
}
