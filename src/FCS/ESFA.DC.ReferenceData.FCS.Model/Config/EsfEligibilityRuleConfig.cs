using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class EsfEligibilityRuleConfig : EntityTypeConfiguration<EsfEligibilityRule>
    {
        public EsfEligibilityRuleConfig()
        {
            this.ToTable("EsfEligibilityRule");

            this.HasKey(pk => new { pk.TenderSpecReference, pk.LotReference });

            this.HasMany(c => c.EsfEligibilityRuleEmploymentStatuses).WithRequired(c => c.EsfEligibilityRule).HasForeignKey(c => new { c.TenderSpecReference, c.LotReference }).WillCascadeOnDelete();

            this.HasMany(c => c.EsfEligibilityRuleLocalAuthorities).WithRequired(c => c.EsfEligibilityRule).HasForeignKey(c => new { c.TenderSpecReference, c.LotReference }).WillCascadeOnDelete();

            this.HasMany(c => c.EsfEligibilityRuleLocalEnterprisePartnerships).WithRequired(c => c.EsfEligibilityRule).HasForeignKey(c => new { c.TenderSpecReference, c.LotReference }).WillCascadeOnDelete();

            this.HasMany(c => c.EsfEligibilityRuleSectorSubjectAreaLevels).WithRequired(c => c.EsfEligibilityRule).HasForeignKey(c => new { c.TenderSpecReference, c.LotReference }).WillCascadeOnDelete();
        }
    }
}
