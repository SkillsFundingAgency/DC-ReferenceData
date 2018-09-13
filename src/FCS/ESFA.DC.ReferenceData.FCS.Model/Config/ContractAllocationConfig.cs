using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class ContractAllocationConfig : EntityTypeConfiguration<ContractAllocation>
    {
        public ContractAllocationConfig()
        {
            this.ToTable("ContractAllocation");

            this.HasKey(ca => ca.Id);

            this.HasMany(ca => ca.ContractDeliverables).WithRequired(cd => cd.ContractAllocation).HasForeignKey(cd => cd.ContractAllocationId).WillCascadeOnDelete();

            this.HasRequired(ca => ca.Contract).WithMany(c => c.ContractAllocations).HasForeignKey(c => c.ContractId).WillCascadeOnDelete();
        }
    }
}
