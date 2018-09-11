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

        public async Task<IEnumerable<Guid>> GetExistingSyndicationItemIds(CancellationToken cancellationToken)
        {
            return await _fcsContext.Contractors.Select(c => c.SyndicationItemId).Distinct().ToListAsync(cancellationToken);
        }

        public async Task PersistContracts(IEnumerable<Contractor> contractors, CancellationToken cancellationToken)
        {
            using (var transaction = _fcsContext.Database.BeginTransaction())
            {
                try
                {
                    var contractNumbers = contractors.SelectMany(c => c.Contracts).Select(c => c.ContractNumber).ToList();

                    // remove older versions
                    var defunctContractors = _fcsContext
                        .Contracts
                        .AsEnumerable()
                        .Where(c => contractNumbers.Contains(c.ContractNumber))
                        .Select(c => c.Contractor).ToList();

                    _fcsContext.Contractors.RemoveRange(defunctContractors);

                    _fcsContext.Contractors.AddRange(contractors);

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
    }
}
