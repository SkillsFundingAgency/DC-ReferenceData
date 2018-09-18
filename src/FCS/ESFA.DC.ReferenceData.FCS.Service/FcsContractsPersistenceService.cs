using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsContractsPersistenceService : IFcsContractsPersistenceService
    {
        private readonly IFcsContext _fcsContext;
        private readonly ILogger _logger;

        public FcsContractsPersistenceService(IFcsContext fcsContext, ILogger logger)
        {
            _fcsContext = fcsContext;
            _logger = logger;
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
                    contractors = contractors.ToList();

                    var contractNumbers = contractors.SelectMany(c => c.Contracts).Select(c => c.ContractNumber).ToList();

                    var defunctContractors = _fcsContext
                        .Contractors
                        .Where(o => o.Contracts.Any(c => contractNumbers.Contains(c.ContractNumber)))
                        .ToList();

                    _logger.LogVerbose($"FCS Contracts - Persisting {contractors.Count()} Contractors - Removing {defunctContractors.Count()} Contractors");

                    _fcsContext.Contractors.RemoveRange(defunctContractors);

                    _fcsContext.Contractors.AddRange(contractors);

                    await _fcsContext.SaveChangesAsync();

                    transaction.Commit();

                    _logger.LogVerbose($"FCS Contracts - Persisted {contractors.Count()} Contractors - Removed {defunctContractors.Count()} Contractors");
                }
                catch (Exception exception)
                {
                    _logger.LogError("FCS Contracts Persist Failed", exception);

                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
