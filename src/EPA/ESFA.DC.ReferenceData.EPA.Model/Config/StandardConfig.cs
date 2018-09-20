using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.EPA.Model.Config
{
    public class StandardConfig : EntityTypeConfiguration<Standard>
    {
        public StandardConfig()
        {
            this.ToTable("Standard");

            this.HasKey(s => new { s.OrganisationId, s.StandardCode });

            this.HasMany(s => s.Periods).WithRequired(p => p.Standard).HasForeignKey(s => new { s.OrganisationId, s.StandardCode }).WillCascadeOnDelete();

            this.HasRequired(s => s.Organisation).WithMany(o => o.Standards).HasForeignKey(s => s.OrganisationId).WillCascadeOnDelete();
        }
    }
}
