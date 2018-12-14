using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.ULN.Model
{
    public partial class Import
    {
        public Import()
        {
            UniqueLearnerNumbers = new HashSet<UniqueLearnerNumber>();
        }

        public int Id { get; set; }
        public string Filename { get; set; }
        public int UlnsInFileCount { get; set; }
        public int NewUlnsInFileCount { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public virtual ICollection<UniqueLearnerNumber> UniqueLearnerNumbers { get; set; }
    }
}
