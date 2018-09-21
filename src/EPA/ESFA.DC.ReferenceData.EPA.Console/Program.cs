using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using ESFA.DC.ReferenceData.EPA.Service;
using ESFA.DC.ReferenceData.EPA.Service.Config;
using ESFA.DC.ReferenceData.EPA.Service.Config.Interface;
using ESFA.DC.ReferenceData.EPA.Service.Interface;
using ESFA.DC.ReferenceData.Interfaces;
using RestSharp;
using Organisation = ESFA.DC.ReferenceData.EPA.Model.EPA.Organisation;
using Standard = ESFA.DC.ReferenceData.EPA.Model.EPA.Standard;

namespace ESFA.DC.ReferenceData.EPA.Console
{
    class Program
    {
        private static IRestClient _client;
        private static IOrganisationMapper _organisationMapper;
        private static IEpaServiceConfiguration _epaServiceConfiguration;
        private static IServicePointConfigurationService _servicePointConfigurationService;
        private static IEpaRestClient _epaRestClient;
        private static IEpaFeedService _epaFeedService;
        private static IRestClient _restClient;
        private static IReferenceDataTask _referenceDataTask;
        private static IEpaContext _epaContext;
        private static IEpaPersistenceService _epaPersistenceService;

        static void Main(string[] args)
        {
            _epaServiceConfiguration = new EpaServiceConfiguration()
            {
                EpaEndpointUri = "https://findapprenticeshiptraining-api.sfa.bis.gov.uk/",
                EpaConnectionString = "Server=(local);Database=ESFA.DC.ReferenceData.EPA.Database;Trusted_Connection=True;"
            };

            _client = new RestClient(_epaServiceConfiguration.EpaEndpointUri);

            _servicePointConfigurationService = new ServicePointConfigurationService(_epaServiceConfiguration);
            _restClient = new RestClient(_epaServiceConfiguration.EpaEndpointUri);
            _epaRestClient = new EpaRestClient(_restClient);
            _epaFeedService = new EpaFeedService(_epaRestClient);
            _organisationMapper = new OrganisationMapper();
            _epaContext = new EpaContext(_epaServiceConfiguration.EpaConnectionString);
            _epaPersistenceService = new EpaPersistenceService(_epaContext);

            _referenceDataTask = new EpaReferenceDataTask(_servicePointConfigurationService, _epaFeedService, _organisationMapper, _epaPersistenceService);

            _referenceDataTask.ExecuteAsync(CancellationToken.None).Wait();
        }
    }
}
