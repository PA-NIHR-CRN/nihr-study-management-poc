using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Configuration;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext
{
    public partial class StudyRegistryContext : DbContext
    {
        public StudyRegistryContext()
        {

        }

        public StudyRegistryContext(DbContextOptions<StudyRegistryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SourceSystemDb> SourceSystems { get; set; } = null!;
        public virtual DbSet<SourceSystemLinkedIdentifierDb> SourceSystemLinkedIdentifiers { get; set; } = null!;
        public virtual DbSet<PersonDb> People { get; set; } = null!;
        public virtual DbSet<ResearchInitiativeDb> ResearchInitiatives { get; set; } = null!;
        public virtual DbSet<ResearchInitiativeTeamMemberDb> ResearchInitiativeTeamMembers { get; set; } = null!;
        public virtual DbSet<RoleDb> Roles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SourceSystemDb>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Id");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.ToTable("sourceSystem");
                entity.HasData(
                    new SourceSystemDb { Id = 1, Name = "EDGE", Description = "Edge system" },
                    new SourceSystemDb { Id = 2, Name = "IRAS", Description = "IRAS system" }
                    );
            });

            modelBuilder.Entity<SourceSystemLinkedIdentifierDb>(entity =>
            {
                entity.ToTable("sourceSystemLinkedIdentifier");

                entity.HasIndex(e => e.ResearchInitiativeId, "fk_sourceSystemLinkedIdentifier_researchInitiative");

                entity.HasIndex(e => e.SourceSystemId, "fk_sourceSystemLinkedIdentifier_sourceSystem");

                entity.HasKey(e => e.Id).HasName("id");

                entity.Property(e => e.Created)
                    .HasMaxLength(6)
                    .HasColumnName("created_at");

                entity.Property(e => e.LocalSystemIdentifier)
                    .HasMaxLength(45)
                    .HasColumnName("local_system_identifier");

                entity.Property(e => e.SourceSystemId).HasColumnName("source_system_id");

                entity.Property(e => e.ResearchInitiativeId).HasColumnName("research_initiative_id");

                entity.HasOne(d => d.LocalSystems)
                    .WithMany(p => p.LocalSystemLinkedIdentifiers)
                    .HasForeignKey(d => d.SourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_linked_system");

                entity.HasOne(d => d.ResearchInitiative)
                    .WithMany(p => p.LocalSystemLinkedIdentifiers)
                    .HasForeignKey(d => d.ResearchInitiativeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_linked_research");
            });

            modelBuilder.Entity<PersonDb>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Lastname)
                    .HasMaxLength(45)
                    .HasColumnName("lastname");

                entity.Property(e => e.Created)
                    .HasMaxLength(6)
                    .HasColumnName("created_at");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(45)
                    .HasColumnName("firstname");

                entity.Property(e => e.PrimaryEmail)
                    .HasMaxLength(45)
                    .HasColumnName("primary_email");
            });

            modelBuilder.Entity<ResearchInitiativeDb>(entity =>
            {
                entity.ToTable("researchInitiative");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasMaxLength(6)
                    .HasColumnName("created_at");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(150)
                    .HasColumnName("identifier");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.ProtocolId)
                    .HasMaxLength(45)
                    .HasColumnName("protocol_id");
            });

            modelBuilder.Entity<ResearchInitiativeTeamMemberDb>(entity =>
            {
                entity.ToTable("researchInitiativeTeamMember");

                entity.HasIndex(e => e.PersonId, "fk_team_person_idx");

                entity.HasIndex(e => e.ResearchInitiativeId, "fk_team_research_idx");

                entity.HasIndex(e => e.RoleId, "fk_team_role_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasMaxLength(6)
                    .HasColumnName("created_at");

                entity.Property(e => e.EffectiveFrom)
                    .HasMaxLength(6)
                    .HasColumnName("effective_from");

                entity.Property(e => e.EffectiveTo)
                    .HasMaxLength(6)
                    .HasColumnName("effective_to");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.ResearchInitiativeId).HasColumnName("research_initiative_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.ResearchInitiativeTeams)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_research_person");

                entity.HasOne(d => d.ResearchInitiative)
                    .WithMany(p => p.ResearchInitiativeTeams)
                    .HasForeignKey(d => d.ResearchInitiativeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_research_study");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ResearchInitiativeTeams)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_research_role");
            });

            modelBuilder.Entity<RoleDb>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasMaxLength(6)
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.HasData(
                    new RoleDb { Name = "CHIEF_INVESTIGATOR", Id = 1 },
                    new RoleDb { Name = "RESEARCHER", Id = 2 }
                    );
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
