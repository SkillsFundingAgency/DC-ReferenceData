using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using ESFA.DC.ReferenceData.EPA.Service.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class EpaPersistenceService : IEpaPersistenceService
    {
        private readonly IEpaContext _epaContext;

        public EpaPersistenceService(IEpaContext epaContext)
        {
            _epaContext = epaContext;
        }

        public async Task PersistEpaOrganisationsAsync(IEnumerable<Organisation> organisations, CancellationToken cancellationToken)
        {
            using (var transaction = _epaContext.BeginTransaction())
            {
                try
                {
                    _epaContext.Organisations.RemoveRange(_epaContext.Organisations);

                    _epaContext.Organisations.AddRange(organisations);

                    await _epaContext.SaveChangesAsync(cancellationToken);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
