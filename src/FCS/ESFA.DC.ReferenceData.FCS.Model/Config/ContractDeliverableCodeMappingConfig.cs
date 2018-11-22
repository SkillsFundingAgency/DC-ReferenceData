using System.Data.Entity.ModelConfiguration;

namespace ESFA.DC.ReferenceData.FCS.Model.Config
{
    internal class ContractDeliverableCodeMappingConfig : EntityTypeConfiguration<ContractDeliverableCodeMapping>
    {
        public ContractDeliverableCodeMappingConfig()
        {
            this.ToTable("ContractDeliverableCodeMapping");

            this.HasKey(cd => cd.Id);
        }
    }
}
