using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsContractsPersistenceService
    {
        Task<IEnumerable<Guid>> GetExistingSyndicationItemIds(CancellationToken cancellationToken);

        Task PersistContracts(IEnumerable<Contractor> contractors, CancellationToken cancellationToken);
    }
}
