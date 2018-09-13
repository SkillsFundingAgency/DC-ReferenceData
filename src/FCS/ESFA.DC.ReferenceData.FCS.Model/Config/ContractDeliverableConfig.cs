using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class ContractDeliverableConfig : EntityTypeConfiguration<ContractDeliverable>
    {
        public ContractDeliverableConfig()
        {
            this.ToTable("ContractDeliverable");

            this.HasKey(cd => cd.Id);

            this.HasRequired(cd => cd.ContractAllocation).WithMany(ca => ca.ContractDeliverables).HasForeignKey(cd => cd.ContractAllocationId).WillCascadeOnDelete();
        }
    }
}
