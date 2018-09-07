using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service.Model;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsContractsPersistenceService
    {
        Task<IEnumerable<MasterContractKey>> GetExistingMasterContractKeys(CancellationToken cancellationToken);

        Task PersistContracts(IEnumerable<MasterContract> masterContracts, IEnumerable<MasterContractKey> existingMasterContractKeys, CancellationToken cancellationToken);
    }
}
