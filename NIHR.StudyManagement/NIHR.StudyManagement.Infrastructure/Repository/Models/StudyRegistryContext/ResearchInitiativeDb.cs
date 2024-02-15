namespace NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext
{
    public partial class ResearchInitiativeDb : DbEntity
    {
        public ResearchInitiativeDb()
        {
            LocalSystemLinkedIdentifiers = new HashSet<SourceSystemLinkedIdentifierDb>();
            ResearchInitiativeTeams = new HashSet<ResearchInitiativeTeamMemberDb>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string Identifier { get; set; } = null!;

        public string ProtocolId { get; set; } = null!;

        public virtual ICollection<SourceSystemLinkedIdentifierDb> LocalSystemLinkedIdentifiers { get; set; }
        public virtual ICollection<ResearchInitiativeTeamMemberDb> ResearchInitiativeTeams { get; set; }
    }

    public abstract class DbEntity
    {
        public DateTime Created { get; set; }

        public DbEntity()
        {
            Created = DateTime.Now;
        }
    }
}
