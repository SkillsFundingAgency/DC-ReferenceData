using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.EPA.Model.Config
{
    public class PeriodConfig : EntityTypeConfiguration<Period>
    {
        public PeriodConfig()
        {
            this.ToTable("Period");

            this.HasKey(p => new { p.OrganisationId, p.StandardCode, p.EffectiveFrom });

            this.HasRequired(p => p.Standard).WithMany(s => s.Periods).HasForeignKey(p => new { p.OrganisationId, p.StandardCode });
        }
    }
}
