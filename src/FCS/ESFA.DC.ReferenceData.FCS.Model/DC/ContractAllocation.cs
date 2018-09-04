using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model.DC
{
    public class ContractAllocation
    {
        public string ContractAllocationNumber { get; set; }

        public string FundingStreamCode { get; set; }

        public string Period { get; set; }

        public string PeriodTypeCode { get; set; }

        public string FundingStreamPeriodCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string UoPCode { get; set; }

        public List<ContractDeliverable> ContractDeliverables { get; set; }
    }
}
