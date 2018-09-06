using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class MasterContractConfig : EntityTypeConfiguration<MasterContract>
    {
        public MasterContractConfig()
        {
            this.ToTable("MasterContract");

            this.HasKey(c => c.Id);

            this.HasRequired(mc => mc.Contractor).WithRequiredPrincipal(c => c.MasterContract).WillCascadeOnDelete();
        }
    }
}
