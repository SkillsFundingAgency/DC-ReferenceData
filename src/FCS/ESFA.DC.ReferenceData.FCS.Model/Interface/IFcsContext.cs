using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.FCS.Model.Interface
{
    public interface IFcsContext
    {
        DbSet<Contractor> Contractors { get; set; }

        DbSet<Contract> Contracts { get; set; }

        DbSet<ContractAllocation> ContractAllocations { get; set; }

        DbSet<ContractDeliverable> ContractDeliverables { get; set; }

        DbSet<ContractDeliverableCodeMapping> ContractDeliverableCodeMappings { get; set; }

        DbSet<EsfEligibilityRule> EsfEligibilityRules { get; set; }

        DbSet<EsfEligibilityRuleEmploymentStatus> EsfEligibilityRuleEmploymentStatuses { get; set; }

        DbSet<EsfEligibilityRuleLocalAuthority> EsEligibilityRulefLocalAuthorities { get; set; }

        DbSet<EsfEligibilityRuleLocalEnterprisePartnership> EsfEligibilityRuleLocalEnterprisePartnerships { get; set; }

        DbSet<EsfEligibilityRuleSectorSubjectAreaLevel> EsfEligibilityRuleSectorSubjectAreaLevel { get; set; }

        Database Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
