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

        public async Task<IEnumerable<MasterContract>> GetNewMasterContractsFromFeedAsync(string uri, IEnumerable<MasterContractKey> existingMasterContractKeys, CancellationToken cancellationToken)
        {
            string previousPageUri = uri;
            var existingMasterContractKeysHashSet = new HashSet<MasterContractKey>(existingMasterContractKeys.Distinct());

            var masterContractsCache = new List<MasterContract>();
            
            IEnumerable<MasterContractKey> currentPageMasterContractKeys;

            do
            {
                var feed = await _retryPolicy.ExecuteAsync(async () => await _syndicationFeedService.LoadSyndicationFeedFromUriAsync(previousPageUri, cancellationToken));

                var masterContracts = feed
                    .Items
                    .Select(_fcsSyndicationFeedParserService.RetrieveContractFromSyndicationItem)
                    .Select(_contractMappingService.Map).ToList();

                currentPageMasterContractKeys = masterContracts.Select(mc => new MasterContractKey(mc.ContractNumber, mc.ContractVersionNumber));

                masterContractsCache.AddRange(masterContracts);

                previousPageUri = _fcsSyndicationFeedParserService.PreviousArchiveLink(feed);
            } while (ContinueToNextPage(previousPageUri, existingMasterContractKeysHashSet, currentPageMasterContractKeys));

            return masterContractsCache;
        }

        public bool ContinueToNextPage(string nextPageUri, IEnumerable<MasterContractKey> existingMasterContractKeys, IEnumerable<MasterContractKey> currentPageMasterContractKeys)
        {
            return nextPageUri != null 
                   && !currentPageMasterContractKeys.All(c => existingMasterContractKeys.Any(e  => e.ContractNumber == c.ContractNumber && e.ContractVersionNumber >= c.ContractVersionNumber ));
        }
    }
}
