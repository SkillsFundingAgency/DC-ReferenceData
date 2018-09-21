using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model;

namespace ESFA.DC.ReferenceData.EPA.Service.Interface
{
    public interface IEpaPersistenceService
    {
        Task PersistEpaOrganisationsAsync(IEnumerable<Organisation> organisations, CancellationToken cancellationToken);
    }
}
