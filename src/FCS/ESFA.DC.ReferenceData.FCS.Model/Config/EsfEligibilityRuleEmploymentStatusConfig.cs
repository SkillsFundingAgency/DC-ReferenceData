using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class EsfEligibilityRuleEmploymentStatusConfig : EntityTypeConfiguration<EsfEligibilityRuleEmploymentStatus>
    {
        public EsfEligibilityRuleEmploymentStatusConfig()
        {
            this.ToTable("EsfEligibilityRuleEmploymentStatus");

            this.HasKey(pk => new { pk.TenderSpecReference, pk.LotReference, pk.Code });

            this.HasRequired(cd => cd.EsfEligibilityRule).WithMany(ca => ca.EsfEligibilityRuleEmploymentStatuses).HasForeignKey(cd => new { cd.TenderSpecReference, cd.LotReference }).WillCascadeOnDelete();
        }
    }
}
