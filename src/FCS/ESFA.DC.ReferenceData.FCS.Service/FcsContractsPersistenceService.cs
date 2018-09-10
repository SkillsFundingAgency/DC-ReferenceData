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

        public async Task<IEnumerable<ContractKey>> GetExistingContractKeys(CancellationToken cancellationToken)
        {
            var contracts = await _fcsContext.Contracts.Select(mc => new { mc.ContractNumber, mc.ContractVersionNumber }).ToListAsync(cancellationToken);
                
            return contracts.Select(mc => new ContractKey(mc.ContractNumber, mc.ContractVersionNumber));
        }

        public async Task PersistContracts(IEnumerable<Contractor> contractors, IEnumerable<ContractKey> existingContractKeys, CancellationToken cancellationToken)
        {
            var existingContractKeysList = existingContractKeys.ToList();

            var contractKeys = contractors.SelectMany(c => c.Contracts).Select(c => new ContractKey(c.ContractNumber, c.ContractVersionNumber)).ToList();

            // get Maximum Version Number for Contracts
            var maxVersionContracts = GetMaxVersionContractKeys(contractKeys).ToList();

            // remove existing Contracts
            var newMasterContracts = GetNewContractKeys(maxVersionContracts, existingContractKeysList).ToList();
            
            // identify older versions to remove
            var defunctExistingMasterContractKeys = GetDefunctExistingContractKeys(newMasterContracts, existingContractKeysList).ToList();

            using (var transaction = _fcsContext.Database.BeginTransaction())
            {
                try
                {
                    // remove older versions
                    var defunctContractors = _fcsContext
                        .Contracts
                        .AsEnumerable()
                        .Where(mc =>
                            defunctExistingMasterContractKeys.Any(e =>
                                e.ContractNumber == mc.ContractNumber &&
                                e.ContractVersionNumber == mc.ContractVersionNumber))
                        .Select(c => c.Contractor).ToList();

                    _fcsContext.Contractors.RemoveRange(defunctContractors);

                    // add newer versions
                    var newContractors = contractors
                        .Where(ctr =>
                            ctr.Contracts.Any(
                                c => newMasterContracts.Any(n =>
                                    n.ContractNumber == c.ContractNumber &&
                                n.ContractVersionNumber == c.ContractVersionNumber)))
                        .ToList();

                    _fcsContext.Contractors.AddRange(newContractors);

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

        public IEnumerable<ContractKey> GetMaxVersionContractKeys(IEnumerable<ContractKey> contractKeys)
        {
            return contractKeys
                .GroupBy(k => k.ContractNumber)
                .Select(k => k.OrderByDescending(c => c.ContractVersionNumber).FirstOrDefault());
        }

        public IEnumerable<ContractKey> GetNewContractKeys(IEnumerable<ContractKey> contractKeys, IEnumerable<ContractKey> existingContractKeys)
        {
            return contractKeys
                .Where(k => !existingContractKeys.Any(e => e.ContractNumber == k.ContractNumber && e.ContractVersionNumber >= k.ContractVersionNumber));
        }

        public IEnumerable<ContractKey> GetDefunctExistingContractKeys(IEnumerable<ContractKey> contractKeys, IEnumerable<ContractKey> existingContractKeys)
        {
            return existingContractKeys.Where(e => contractKeys.Any(k => k.ContractNumber == e.ContractNumber && k.ContractVersionNumber > e.ContractVersionNumber));
        }
    }
}
