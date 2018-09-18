using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
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

        public FcsFeedService(
            ISyndicationFeedService syndicationFeedService,
            IFcsSyndicationFeedParserService fcsSyndicationFeedParserService,
            IContractMappingService contractMappingService)
        {
            _syndicationFeedService = syndicationFeedService;
            _fcsSyndicationFeedParserService = fcsSyndicationFeedParserService;
            _contractMappingService = contractMappingService;
        }

        public async Task<IEnumerable<Contractor>> GetNewContractorsFromFeedAsync(string uri, IEnumerable<Guid> existingSyndicationItemIds, CancellationToken cancellationToken)
        {
            string previousPageUri = uri;
            var existingSyndicationItemIdsHashSet = new HashSet<Guid>(existingSyndicationItemIds.Distinct());

            var contractorCache = new Dictionary<string, Contractor>();

            IEnumerable<Guid> newCurrentPageSyndicationItemIds;

            do
            {
                var feed = await _retryPolicy.ExecuteAsync(async () => await _syndicationFeedService.LoadSyndicationFeedFromUriAsync(previousPageUri, cancellationToken));

                var contractors = feed
                    .Items
                    .Reverse()
                    .Select(_fcsSyndicationFeedParserService.RetrieveContractFromSyndicationItem)
                    .Where(m => !existingSyndicationItemIdsHashSet.Contains(m.syndicationItemId))
                    .Select(m => _contractMappingService.Map(m.syndicationItemId, m.contract))
                    .ToList();

                newCurrentPageSyndicationItemIds = contractors.Select(c => c.SyndicationItemId);

                foreach (var contractor in contractors)
                {
                    var contractNumber = contractor.Contracts[0].ContractNumber;

                    if (!contractorCache.ContainsKey(contractNumber))
                    {
                        contractorCache.Add(contractNumber, contractor);
                    }
                }

                previousPageUri = _fcsSyndicationFeedParserService.PreviousArchiveLink(feed);
            }
            while (ContinueToNextPage(previousPageUri, newCurrentPageSyndicationItemIds));

            return contractorCache.Values;
        }

        public bool ContinueToNextPage(string nextPageUri, IEnumerable<Guid> newCurrentPageSyndicationItemIds)
        {
            return nextPageUri != null && newCurrentPageSyndicationItemIds.Any();
        }
    }
}
