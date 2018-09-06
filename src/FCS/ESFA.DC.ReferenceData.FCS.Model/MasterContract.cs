using System;
using System.Collections.Generic;
using ESFA.DC.ReferenceData.FCS.Model.Abstract;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class MasterContract : AbstractEntity
    {
        public string ContractNumber { get; set; }

        public int ContractVersionNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Contractor Contractor { get; set; }
    }
}
