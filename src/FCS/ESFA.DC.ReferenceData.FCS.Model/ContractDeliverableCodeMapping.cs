using ESFA.DC.ReferenceData.FCS.Model.Abstract;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class ContractDeliverableCodeMapping : AbstractEntity
    {
        public string FundingStreamPeriodCode { get; set; }

        public string ExternalDeliverableCode { get; set; }

        public string FCSDeliverableCode { get; set; }

        public string DeliverableName { get; set; }

        public bool Claimable { get; set; }
    }
}
