using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model.EPA;

namespace ESFA.DC.ReferenceData.EPA.Service.Interface
{
    public interface IEpaRestClient
    {
        Task<IEnumerable<Organisation>> GetOrganisationsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<Standard>> GetStandardsForOrganisationAsync(string organisationId, CancellationToken cancellationToken);
    }
}
