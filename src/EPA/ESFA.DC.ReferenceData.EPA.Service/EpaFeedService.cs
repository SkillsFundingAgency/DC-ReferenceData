using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using ESFA.DC.ReferenceData.EPA.Service.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class EpaFeedService : IEpaFeedService
    {
        private readonly IEpaRestClient _epaRestClient;

        public EpaFeedService(IEpaRestClient epaRestClient)
        {
            _epaRestClient = epaRestClient;
        }

        public async Task<IEnumerable<Organisation>> GetOrganisationsAsync(CancellationToken cancellationToken)
        {
            var organisations = await _epaRestClient.GetOrganisationsAsync(cancellationToken);
            
            var standardsTasks = organisations.Select(o => (organisation: o, standardsTask: _epaRestClient.GetStandardsForOrganisationAsync(o.Id, cancellationToken))).ToList();

            Task.WaitAll(standardsTasks.Select(t => t.standardsTask).ToArray(), cancellationToken);

            return standardsTasks.Select(t =>
            {
                t.organisation.Standards = t.standardsTask.Result.ToList();
                return t.organisation;
            });
        }
    }
}
