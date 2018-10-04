using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class EsfEligibilityRuleLocalEnterprisePartnershipConfig : EntityTypeConfiguration<EsfEligibilityRuleLocalEnterprisePartnership>
    {
        public EsfEligibilityRuleLocalEnterprisePartnershipConfig()
        {
            this.ToTable("EsfEligibilityRuleLocalEnterprisePartnership");

            this.HasKey(pk => new { pk.TenderSpecReference, pk.LotReference, pk.Code });

            this.HasRequired(cd => cd.EsfEligibilityRule).WithMany(ca => ca.EsfEligibilityRuleLocalEnterprisePartnerships).HasForeignKey(cd => cd.EsfEligibilityRule).WillCascadeOnDelete();
        }
    }
}
