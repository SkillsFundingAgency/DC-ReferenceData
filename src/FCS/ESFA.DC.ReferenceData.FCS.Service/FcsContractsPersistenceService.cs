using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Model;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsContractsPersistenceService : IFcsContractsPersistenceService
    {
        private readonly IFcsContext _fcsContext;

        public FcsContractsPersistenceService(IFcsContext fcsContext)
        {
            _fcsContext = fcsContext;
        }

        public async Task<IEnumerable<MasterContractKey>> GetExistingMasterContractKeys(CancellationToken cancellationToken)
        {
            var masterContracts = await _fcsContext.MasterContracts.Select(mc => new { mc.ContractNumber, mc.ContractVersionNumber }).ToListAsync(cancellationToken);
                
            return masterContracts.Select(mc => new MasterContractKey(mc.ContractNumber, mc.ContractVersionNumber));
        }

        public Task PersistContracts(IEnumerable<MasterContract> masterContracts, IEnumerable<MasterContractKey> masterContractKeys, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            // get Maximum Version Number for Contracts

            // remove ones we already have saved

            // identify older versions to remove

            // remove older versions

            // add newer versions

            // commit
        }
    }
}
