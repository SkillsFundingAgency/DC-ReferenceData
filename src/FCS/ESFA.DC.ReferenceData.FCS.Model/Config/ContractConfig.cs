using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class ContractConfig : EntityTypeConfiguration<Contract>
    {
        public ContractConfig()
        {
            this.ToTable("Contract");

            this.HasKey(c => c.Id);

            this.HasMany(c => c.ContractAllocations).WithRequired(ca => ca.Contract).HasForeignKey(ca => ca.ContractId);

            this.HasRequired(c => c.Contractor).WithMany(c => c.Contracts).HasForeignKey(c => c.ContractorId).WillCascadeOnDelete();
        }
    }
}
