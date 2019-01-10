using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using Microsoft.EntityFrameworkCore;

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
            var syndicationItemIds = await _fcsContext
                .Contractor
                .Select(c => c.SyndicationItemId)
                .ToListAsync(cancellationToken);

            return syndicationItemIds
                .Where(c => c.HasValue)
                .Select(c => c.Value)
                .Distinct();
        }

        public async Task PersistContracts(IEnumerable<Contractor> contractors, CancellationToken cancellationToken)
        {
            using (var transaction = _fcsContext.Database.BeginTransaction())
            {
                try
                {
                    contractors = contractors.ToList();

                    var contractNumbers = contractors.SelectMany(c => c.Contract).Select(c => c.ContractNumber).ToList();

                    var defunctContractors = _fcsContext
                        .Contractor
                        .Where(o => o.Contract.Any(c => contractNumbers.Contains(c.ContractNumber)))
                        .ToList();

                    _logger.LogVerbose($"FCS Contracts - Persisting {contractors.Count()} Contractors - Removing {defunctContractors.Count()} Contractors");

                    _fcsContext.Contractor.RemoveRange(defunctContractors);

                    _fcsContext.Contractor.AddRange(contractors);

                    await _fcsContext.SaveChangesAsync(cancellationToken);

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
