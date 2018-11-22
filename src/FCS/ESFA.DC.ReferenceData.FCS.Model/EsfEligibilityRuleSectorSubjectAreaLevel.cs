using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class EsfEligibilityRuleSectorSubjectAreaLevel
    {
        public string TenderSpecReference { get; set; }

        public string LotReference { get; set; }

        public decimal? SectorSubjectAreaCode { get; set; }

        public string MinLevelCode { get; set; }

        public string MaxLevelCode { get; set; }

        public EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
