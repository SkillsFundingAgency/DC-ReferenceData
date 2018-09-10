using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service.Model;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsContractsPersistenceService
    {
        Task<IEnumerable<ContractKey>> GetExistingContractKeys(CancellationToken cancellationToken);

        Task PersistContracts(IEnumerable<Contractor> contractors, IEnumerable<ContractKey> existingContractKeys, CancellationToken cancellationToken);
    }
}
