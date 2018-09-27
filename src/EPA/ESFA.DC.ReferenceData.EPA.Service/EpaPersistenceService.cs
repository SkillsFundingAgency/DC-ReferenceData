using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using ESFA.DC.ReferenceData.EPA.Service.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class EpaPersistenceService : IEpaPersistenceService
    {
        private readonly IEpaContext _epaContext;
        private readonly ILogger _logger;

        public EpaPersistenceService(IEpaContext epaContext, ILogger logger)
        {
            _epaContext = epaContext;
            _logger = logger;
        }

        public async Task PersistEpaOrganisationsAsync(IEnumerable<Organisation> organisations, CancellationToken cancellationToken)
        {
            organisations = organisations.ToList();

            _logger.LogInfo($"EPA Reference Data - Persisting {organisations.Count()} Organisations");

            using (var transaction = _epaContext.BeginTransaction())
            {
                try
                {
                    _epaContext.Organisations.RemoveRange(_epaContext.Organisations);

                    _epaContext.Organisations.AddRange(organisations);

                    await _epaContext.SaveChangesAsync(cancellationToken);

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError("EPA Reference Data - Persistence Failure", ex);
                    throw;
                }
            }

            _logger.LogInfo($"EPA Reference Data - Persisted {organisations.Count()} Organisations");
        }
    }
}
