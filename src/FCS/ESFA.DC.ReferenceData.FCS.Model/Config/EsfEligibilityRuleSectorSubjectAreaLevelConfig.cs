using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class EsfEligibilityRuleSectorSubjectAreaLevelConfig : EntityTypeConfiguration<EsfEligibilityRuleSectorSubjectAreaLevel>
    {
        public EsfEligibilityRuleSectorSubjectAreaLevelConfig()
        {
            this.ToTable("EsfEligibilityRuleSectorSubjectAreaLevel");

            this.HasRequired(cd => cd.EsfEligibilityRule).WithMany(ca => ca.EsfEligibilityRuleSectorSubjectAreaLevels).HasForeignKey(cd => cd.EsfEligibilityRule).WillCascadeOnDelete();
        }
    }
}
