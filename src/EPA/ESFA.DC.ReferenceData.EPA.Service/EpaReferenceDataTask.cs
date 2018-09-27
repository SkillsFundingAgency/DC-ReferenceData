using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.EPA.Service.Interface;
using ESFA.DC.ReferenceData.Interfaces;
using ESFA.DC.ReferenceData.Interfaces.Constants;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class EpaReferenceDataTask : IReferenceDataTask
    {
        private readonly IServicePointConfigurationService _servicePointConfigurationService;
        private readonly IEpaFeedService _epaFeedService;
        private readonly IOrganisationMapper _organisationMapper;
        private readonly IEpaPersistenceService _epaPersistenceService;
        private readonly ILogger _logger;

        public EpaReferenceDataTask(IServicePointConfigurationService servicePointConfigurationService, IEpaFeedService epaFeedService, IOrganisationMapper organisationMapper, IEpaPersistenceService epaPersistenceService, ILogger logger)
        {
            _servicePointConfigurationService = servicePointConfigurationService;
            _epaFeedService = epaFeedService;
            _organisationMapper = organisationMapper;
            _epaPersistenceService = epaPersistenceService;
            _logger = logger;
        }

        public string TaskName => TaskNameConstants.EpaReferenceDataTaskName;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                _servicePointConfigurationService.ConfigureServicePoint();

                var feedOrganisations = await _epaFeedService.GetOrganisationsAsync(cancellationToken);

                var mappedOrganisations = _organisationMapper.MapOrganisations(feedOrganisations);

                await _epaPersistenceService.PersistEpaOrganisationsAsync(mappedOrganisations, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("EPA Reference Data Task Failed", ex);
                throw;
            }
        }
    }
}
