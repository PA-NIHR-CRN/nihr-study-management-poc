namespace NIHR.StudyManagement.Domain.Models
{
    public class RegisterStudyRequestWithContext : RegisterStudyRequest
    {
        public string LocalSystemName {get;set;}

        public string RoleName { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public RegisterStudyRequestWithContext()
        {
            Identifier = "";
            LocalSystemName = "";
            RoleName = "";
            EffectiveFrom = DateTime.Now;
        }
    }

    public class AddStudyToExistingIdentifierRequestWithContext : RegisterStudyRequestWithContext
    {
    }

    public class RegisterStudyRequest
    {
        public string ProjectId { get; set; }

        public PersonWithPrimaryEmail ChiefInvestigator { get; set; }

        public string ShortTitle { get; set; }

        public string Sponsor { get; set; }

        public string? Identifier { get; set; }

        public string ProtocolId { get; set; }

        public RegisterStudyRequest()
        {
            ProjectId = "";
            ChiefInvestigator = new PersonWithPrimaryEmail();
            ShortTitle = "";
            Sponsor = "";
            ProtocolId = "";
        }
    }

    public class Person
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public Person()
        {
            Firstname = "";
            Lastname = "";
        }
    }

    public class PersonWithPrimaryEmail : Person
    {
        public Email Email { get; set; }
    }

    public class LinkedSystemIdentifier
    {
        public string Identifier { get; set; }
        public string SystemName { get; set; }

        public DateTime CreatedAt { get; set; }

        public LinkedSystemIdentifier()
        {
            Identifier = "";
            SystemName = "";
            CreatedAt = DateTime.Now;
        }
    }

    public class TeamMember
    {
        public PersonWithPrimaryEmail Person { get; set; }

        public Role Role { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public TeamMember()
        {
            Person = new PersonWithPrimaryEmail();
            Role = new Role();
            EffectiveFrom = DateTime.Now;
        }
    }

    public class Role
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Role()
        {
            Name = "";
            Description = "";
        }
    }

    public class Email
    {
        public string Address { get; set; }

        public Email()
        {
            Address = "";
        }
    }
}
