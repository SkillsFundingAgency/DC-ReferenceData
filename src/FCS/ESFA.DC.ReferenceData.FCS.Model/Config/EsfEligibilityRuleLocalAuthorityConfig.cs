using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class EsfEligibilityRuleLocalAuthorityConfig : EntityTypeConfiguration<EsfEligibilityRuleLocalAuthority>
    {
        public EsfEligibilityRuleLocalAuthorityConfig()
        {
            this.ToTable("EsfEligibilityRuleLocalAuthority");

            this.HasKey(pk => new { pk.TenderSpecReference, pk.LotReference, pk.Code });

            this.HasRequired(cd => cd.EsfEligibilityRule).WithMany(ca => ca.EsfEligibilityRuleLocalAuthorities).HasForeignKey(cd => cd.EsfEligibilityRule).WillCascadeOnDelete();
        }
    }
}
