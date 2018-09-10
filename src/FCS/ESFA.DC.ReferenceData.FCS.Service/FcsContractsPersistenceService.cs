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

        public async Task<IEnumerable<ContractKey>> GetExistingMasterContractKeys(CancellationToken cancellationToken)
        {
            var masterContracts = await _fcsContext.MasterContracts.Select(mc => new { mc.ContractNumber, mc.ContractVersionNumber }).ToListAsync(cancellationToken);
                
            return masterContracts.Select(mc => new ContractKey(mc.ContractNumber, mc.ContractVersionNumber));
        }

        public async Task PersistContracts(IEnumerable<MasterContract> masterContracts, IEnumerable<ContractKey> existingMasterContractKeys, CancellationToken cancellationToken)
        {
            var existingMasterContractKeysList = existingMasterContractKeys.ToList();

            var masterContractKeys = masterContracts.Select(c => new ContractKey(c.ContractNumber, c.ContractVersionNumber)).ToList();

            // get Maximum Version Number for Contracts
            var maxVersionContracts = GetMaxVersionMasterContractKeys(masterContractKeys).ToList();

            // remove existing Contracts
            var newMasterContracts = GetNewMasterContractKeys(maxVersionContracts, existingMasterContractKeysList).ToList();
            
            // identify older versions to remove
            var defunctExistingMasterContractKeys = GetDefunctExistingMasterContractKeys(newMasterContracts, existingMasterContractKeysList).ToList();

            using (var transaction = _fcsContext.Database.BeginTransaction())
            {
                try
                {
                    // remove older versions
                    var defunctContracts = _fcsContext
                        .MasterContracts
                        .AsEnumerable()
                        .Where(mc =>
                            defunctExistingMasterContractKeys.Any(e =>
                                e.ContractNumber == mc.ContractNumber &&
                                e.ContractVersionNumber == mc.ContractVersionNumber)).ToList();

                    _fcsContext.MasterContracts.RemoveRange(defunctContracts);

                    // add newer versions
                    var newContracts = masterContracts
                        .Where(mc =>
                            newMasterContracts.Any(n =>
                                n.ContractNumber == mc.ContractNumber &&
                                n.ContractVersionNumber == mc.ContractVersionNumber)).ToList();

                    _fcsContext.MasterContracts.AddRange(newContracts);

                    await _fcsContext.SaveChangesAsync();

                    // commit
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<ContractKey> GetMaxVersionMasterContractKeys(IEnumerable<ContractKey> masterContractKeys)
        {
            return masterContractKeys
                .GroupBy(k => k.ContractNumber)
                .Select(k => k.OrderByDescending(c => c.ContractVersionNumber).FirstOrDefault());
        }

        public IEnumerable<ContractKey> GetNewMasterContractKeys(IEnumerable<ContractKey> masterContractKeys,IEnumerable<ContractKey> existingMasterContractKeys)
        {
            return masterContractKeys
                .Where(k => !existingMasterContractKeys.Any(e => e.ContractNumber == k.ContractNumber && e.ContractVersionNumber >= k.ContractVersionNumber));
        }

        public IEnumerable<ContractKey> GetDefunctExistingMasterContractKeys(IEnumerable<ContractKey> masterContractKeys, IEnumerable<ContractKey> existingMasterContractKeys)
        {
            return existingMasterContractKeys.Where(e => masterContractKeys.Any(k => k.ContractNumber == e.ContractNumber && k.ContractVersionNumber > e.ContractVersionNumber));
        }
    }
}
