using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ESFA.DC.ReferenceData.FCS.Model.Interface
{
    public interface IFcsContext
    {
        DbSet<Contract> Contract { get; set; }
        DbSet<ContractAllocation> ContractAllocation { get; set; }
        DbSet<ContractDeliverable> ContractDeliverable { get; set; }
        DbSet<ContractDeliverableCodeMapping> ContractDeliverableCodeMapping { get; set; }
        DbSet<Contractor> Contractor { get; set; }
        DbSet<EsfEligibilityRule> EsfEligibilityRule { get; set; }
        DbSet<EsfEligibilityRuleEmploymentStatus> EsfEligibilityRuleEmploymentStatus { get; set; }
        DbSet<EsfEligibilityRuleLocalAuthority> EsfEligibilityRuleLocalAuthority { get; set; }
        DbSet<EsfEligibilityRuleLocalEnterprisePartnership> EsfEligibilityRuleLocalEnterprisePartnership { get; set; }
        DbSet<EsfEligibilityRuleSectorSubjectAreaLevel> EsfEligibilityRuleSectorSubjectAreaLevel { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
