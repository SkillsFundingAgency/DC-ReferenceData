using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Model;
using Polly;
using Polly.Retry;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsFeedService : IFcsFeedService
    {
        private readonly ISyndicationFeedService _syndicationFeedService;
        private readonly IFcsSyndicationFeedParserService _fcsSyndicationFeedParserService;
        private readonly IContractMappingService _contractMappingService;

        private readonly RetryPolicy _retryPolicy = 
            Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, a => TimeSpan.FromSeconds(3));

        public FcsFeedService(ISyndicationFeedService syndicationFeedService,
            IFcsSyndicationFeedParserService fcsSyndicationFeedParserService,
            IContractMappingService contractMappingService)
        {
            _syndicationFeedService = syndicationFeedService;
            _fcsSyndicationFeedParserService = fcsSyndicationFeedParserService;
            _contractMappingService = contractMappingService;
        }

        public async Task<string> FindFirstPageFromEntryPointAsync(string uri, CancellationToken cancellationToken)
        {
            var previousArchiveUri = uri;
            SyndicationFeed currentSyndicationFeed;

            do
            {
                currentSyndicationFeed = await _syndicationFeedService.LoadSyndicationFeedFromUriAsync(previousArchiveUri, cancellationToken);

                previousArchiveUri = _fcsSyndicationFeedParserService.PreviousArchiveLink(currentSyndicationFeed);
            } while (previousArchiveUri != null);

            return _fcsSyndicationFeedParserService.CurrentArchiveLink(currentSyndicationFeed);
        }

        public async Task<IEnumerable<Contractor>> GetNewContractorsFromFeedAsync(string uri, IEnumerable<ContractKey> existingContractKeys, CancellationToken cancellationToken)
        {
            string previousPageUri = uri;
            var existingMasterContractKeysHashSet = new HashSet<ContractKey>(existingContractKeys.Distinct());

            var contractorCache = new List<Contractor>();
            
            IEnumerable<ContractKey> currentPageContractKeys;

            do
            {
                var feed = await _retryPolicy.ExecuteAsync(async () => await _syndicationFeedService.LoadSyndicationFeedFromUriAsync(previousPageUri, cancellationToken));

                var contractors = feed
                    .Items
                    .Select(_fcsSyndicationFeedParserService.RetrieveContractFromSyndicationItem)
                    .Select(_contractMappingService.Map).ToList();

                currentPageContractKeys = contractors.SelectMany(c => c.Contracts).Select(mc => new ContractKey(mc.ContractNumber, mc.ContractVersionNumber));

                contractorCache.AddRange(contractors);

                previousPageUri = _fcsSyndicationFeedParserService.PreviousArchiveLink(feed);
            } while (ContinueToNextPage(previousPageUri, existingMasterContractKeysHashSet, currentPageContractKeys));

            return contractorCache;
        }

        public bool ContinueToNextPage(string nextPageUri, IEnumerable<ContractKey> existingContractKeys, IEnumerable<ContractKey> currentPageContractKeys)
        {
            return nextPageUri != null 
                   && !currentPageContractKeys.All(c => existingContractKeys.Any(e  => e.ContractNumber == c.ContractNumber && e.ContractVersionNumber >= c.ContractVersionNumber ));
        }
    }
}
