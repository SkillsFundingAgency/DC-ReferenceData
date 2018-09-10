using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class ContractorConfig : EntityTypeConfiguration<Contractor>
    {
        public ContractorConfig()
        {
            this.ToTable("Contractor");

            this.HasKey(c => c.Id);

            this.HasMany(c => c.Contracts).WithRequired(c => c.Contractor).HasForeignKey(c => c.ContractorId);
        }
    }
}
