using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class EsfEligibilityRuleSectorSubjectAreaLevelConfig : EntityTypeConfiguration<EsfEligibilityRuleSectorSubjectAreaLevel>
    {
        public EsfEligibilityRuleSectorSubjectAreaLevelConfig()
        {
            this.ToTable("EsfEligibilityRuleSectorSubjectAreaLevel");

            this.HasKey(pk => new { pk.Id });

            this.HasRequired(cd => cd.EsfEligibilityRule).WithMany(ca => ca.EsfEligibilityRuleSectorSubjectAreaLevel).HasForeignKey(cd => new { cd.TenderSpecReference, cd.LotReference }).WillCascadeOnDelete();
        }
    }
}
