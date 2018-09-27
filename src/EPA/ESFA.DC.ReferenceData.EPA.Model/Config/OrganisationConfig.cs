using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.EPA.Model.Config
{
    public class OrganisationConfig : EntityTypeConfiguration<Organisation>
    {
        public OrganisationConfig()
        {
            this.ToTable("Organisation");

            this.HasKey(o => o.Id);

            this.HasMany(o => o.Standards).WithRequired(s => s.Organisation).HasForeignKey(s => s.OrganisationId).WillCascadeOnDelete();
        }
    }
}
