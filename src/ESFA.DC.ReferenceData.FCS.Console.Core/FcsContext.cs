using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class FcsContext : DbContext
    {
        public FcsContext()
        {
        }

        public FcsContext(DbContextOptions<FcsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<ContractAllocation> ContractAllocation { get; set; }
        public virtual DbSet<ContractDeliverable> ContractDeliverable { get; set; }
        public virtual DbSet<ContractDeliverableCodeMapping> ContractDeliverableCodeMapping { get; set; }
        public virtual DbSet<Contractor> Contractor { get; set; }
        public virtual DbSet<EsfEligibilityRule> EsfEligibilityRule { get; set; }
        public virtual DbSet<EsfEligibilityRuleEmploymentStatus> EsfEligibilityRuleEmploymentStatus { get; set; }
        public virtual DbSet<EsfEligibilityRuleLocalAuthority> EsfEligibilityRuleLocalAuthority { get; set; }
        public virtual DbSet<EsfEligibilityRuleLocalEnterprisePartnership> EsfEligibilityRuleLocalEnterprisePartnership { get; set; }
        public virtual DbSet<EsfEligibilityRuleSectorSubjectAreaLevel> EsfEligibilityRuleSectorSubjectAreaLevel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=FCS;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.Property(e => e.ContractNumber).HasMaxLength(20);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.ContractorId)
                    .HasConstraintName("FK_Contract_ToContractor");
            });

            modelBuilder.Entity<ContractAllocation>(entity =>
            {
                entity.Property(e => e.ContractAllocationNumber).HasMaxLength(20);

                entity.Property(e => e.DeliveryOrganisation).HasMaxLength(100);

                entity.Property(e => e.DeliveryUkprn).HasColumnName("DeliveryUKPRN");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FundingStreamCode).HasMaxLength(10);

                entity.Property(e => e.FundingStreamPeriodCode).HasMaxLength(20);

                entity.Property(e => e.LearningRatePremiumFactor).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.LotReference).HasMaxLength(100);

                entity.Property(e => e.Period).HasMaxLength(4);

                entity.Property(e => e.PeriodTypeCode).HasMaxLength(20);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StopNewStartsFromDate).HasColumnType("datetime");

                entity.Property(e => e.TenderSpecReference).HasMaxLength(100);

                entity.Property(e => e.TerminationDate).HasColumnType("datetime");

                entity.Property(e => e.UoPcode)
                    .HasColumnName("UoPCode")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractAllocation)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ContractAllocation_ToContract");
            });

            modelBuilder.Entity<ContractDeliverable>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.PlannedValue).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.UnitCode).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.UnitCost).HasColumnType("decimal(13, 2)");

                entity.HasOne(d => d.ContractAllocation)
                    .WithMany(p => p.ContractDeliverable)
                    .HasForeignKey(d => d.ContractAllocationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ContractDeliverable_ToContractAllocation");
            });

            modelBuilder.Entity<ContractDeliverableCodeMapping>(entity =>
            {
                entity.Property(e => e.DeliverableName).HasMaxLength(255);

                entity.Property(e => e.ExternalDeliverableCode).HasMaxLength(255);

                entity.Property(e => e.FcsdeliverableCode)
                    .HasColumnName("FCSDeliverableCode")
                    .HasMaxLength(255);

                entity.Property(e => e.FundingStreamPeriodCode).HasMaxLength(255);
            });

            modelBuilder.Entity<Contractor>(entity =>
            {
                entity.Property(e => e.LegalName).HasMaxLength(100);

                entity.Property(e => e.OrganisationIdentifier).HasMaxLength(100);

                entity.Property(e => e.Ukprn).HasColumnName("UKPRN");
            });

            modelBuilder.Entity<EsfEligibilityRule>(entity =>
            {
                entity.HasKey(e => new { e.TenderSpecReference, e.LotReference });

                entity.Property(e => e.TenderSpecReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LotReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MaxPriorAttainment)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.MinPriorAttainment)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EsfEligibilityRuleEmploymentStatus>(entity =>
            {
                entity.HasKey(e => new { e.TenderSpecReference, e.LotReference, e.Code });

                entity.Property(e => e.TenderSpecReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LotReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.EsfEligibilityRule)
                    .WithMany(p => p.EsfEligibilityRuleEmploymentStatus)
                    .HasForeignKey(d => new { d.TenderSpecReference, d.LotReference })
                    .HasConstraintName("FK_EsfEligibilityRuleEmploymentStatusToRule");
            });

            modelBuilder.Entity<EsfEligibilityRuleLocalAuthority>(entity =>
            {
                entity.HasKey(e => new { e.TenderSpecReference, e.LotReference, e.Code });

                entity.Property(e => e.TenderSpecReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LotReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.EsfEligibilityRule)
                    .WithMany(p => p.EsfEligibilityRuleLocalAuthority)
                    .HasForeignKey(d => new { d.TenderSpecReference, d.LotReference })
                    .HasConstraintName("FK_EsfEligibilityRuleLocalAuthorityToRule");
            });

            modelBuilder.Entity<EsfEligibilityRuleLocalEnterprisePartnership>(entity =>
            {
                entity.HasKey(e => new { e.TenderSpecReference, e.LotReference, e.Code });

                entity.Property(e => e.TenderSpecReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LotReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.EsfEligibilityRule)
                    .WithMany(p => p.EsfEligibilityRuleLocalEnterprisePartnership)
                    .HasForeignKey(d => new { d.TenderSpecReference, d.LotReference })
                    .HasConstraintName("FK_EsfEligibilityRuleLocalEnterprisePartnershipToRule");
            });

            modelBuilder.Entity<EsfEligibilityRuleSectorSubjectAreaLevel>(entity =>
            {
                entity.Property(e => e.LotReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MaxLevelCode)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MinLevelCode)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SectorSubjectAreaCode).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.TenderSpecReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.EsfEligibilityRule)
                    .WithMany(p => p.EsfEligibilityRuleSectorSubjectAreaLevel)
                    .HasForeignKey(d => new { d.TenderSpecReference, d.LotReference })
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EsfEligibilityRuleSectorSubjectAreaLevelToRule");
            });
        }
    }
}
