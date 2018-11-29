using System.Data.Entity;
using ESFA.DC.ReferenceData.FCS.Model.Config;
using ESFA.DC.ReferenceData.FCS.Model.Interface;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class FcsContext : DbContext, IFcsContext
    {
        public FcsContext()
            : base("name=FCS")
        {
        }

        public FcsContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Contractor> Contractors { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }

        public virtual DbSet<ContractAllocation> ContractAllocations { get; set; }

        public virtual DbSet<ContractDeliverable> ContractDeliverables { get; set; }

        public virtual DbSet<ContractDeliverableCodeMapping> ContractDeliverableCodeMappings { get; set; }

        public virtual DbSet<EsfEligibilityRule> EsfEligibilityRules { get; set; }

        public virtual DbSet<EsfEligibilityRuleEmploymentStatus> EsfEligibilityRuleEmploymentStatuses { get; set; }

        public virtual DbSet<EsfEligibilityRuleLocalAuthority> EsEligibilityRulefLocalAuthorities { get; set; }

        public virtual DbSet<EsfEligibilityRuleLocalEnterprisePartnership> EsfEligibilityRuleLocalEnterprisePartnerships { get; set; }

        public virtual DbSet<EsfEligibilityRuleSectorSubjectAreaLevel> EsfEligibilityRuleSectorSubjectAreaLevel { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContractorConfig());
            modelBuilder.Configurations.Add(new ContractConfig());
            modelBuilder.Configurations.Add(new ContractAllocationConfig());
            modelBuilder.Configurations.Add(new ContractDeliverableConfig());
            modelBuilder.Configurations.Add(new ContractDeliverableCodeMappingConfig());
            modelBuilder.Configurations.Add(new EsfEligibilityRuleConfig());
            modelBuilder.Configurations.Add(new EsfEligibilityRuleEmploymentStatusConfig());
            modelBuilder.Configurations.Add(new EsfEligibilityRuleLocalAuthorityConfig());
            modelBuilder.Configurations.Add(new EsfEligibilityRuleLocalEnterprisePartnershipConfig());
            modelBuilder.Configurations.Add(new EsfEligibilityRuleSectorSubjectAreaLevelConfig());
        }
    }
}
