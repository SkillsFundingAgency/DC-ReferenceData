using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using ESFA.DC.ReferenceData.EPA.Service.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class EpaFeedService : IEpaFeedService
    {
        private readonly IEpaRestClient _epaRestClient;
        private readonly ILogger _logger;

        public EpaFeedService(IEpaRestClient epaRestClient, ILogger logger)
        {
            _epaRestClient = epaRestClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Organisation>> GetOrganisationsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInfo("EPA Reference Data - Starting Get Organisations");

            var organisations = await _epaRestClient.GetOrganisationsAsync(cancellationToken);
            
            var standardsTasks = organisations.Select(o => (organisation: o, standardsTask: _epaRestClient.GetStandardsForOrganisationAsync(o.Id, cancellationToken))).ToList();

            Task.WaitAll(standardsTasks.Select(t => t.standardsTask).ToArray(), cancellationToken);

            var retrievedOrganisations = standardsTasks.Select(t =>
            {
                t.organisation.Standards = t.standardsTask.Result.ToList();
                return t.organisation;
            }).ToList();

            _logger.LogInfo($"EPA Reference Data - Finishing Get Organisations, Count : {retrievedOrganisations.Count}");

            return retrievedOrganisations;
        }
    }
}
