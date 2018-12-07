using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.Interfaces;
using ESFA.DC.ReferenceData.Interfaces.Constants;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class ULNReferenceDataTask : IReferenceDataTask
    {
        public string TaskName => TaskNameConstants.UlnReferenceDataTaskName;

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
