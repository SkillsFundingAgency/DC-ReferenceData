using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class ContractAllocation
    {
        public ContractAllocation()
        {
            ContractDeliverable = new HashSet<ContractDeliverable>();
        }

        public int Id { get; set; }
        public string ContractAllocationNumber { get; set; }
        public string FundingStreamCode { get; set; }
        public string Period { get; set; }
        public string PeriodTypeCode { get; set; }
        public string FundingStreamPeriodCode { get; set; }
        public decimal? LearningRatePremiumFactor { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UoPcode { get; set; }
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public int? DeliveryUkprn { get; set; }
        public string DeliveryOrganisation { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? StopNewStartsFromDate { get; set; }
        public int? ContractId { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ICollection<ContractDeliverable> ContractDeliverable { get; set; }
    }
}
