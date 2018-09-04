using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.FCS;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsFeedService : IFcsFeedService
    {
        private readonly ISyndicationFeedService _syndicationFeedService;
        private readonly IFcsSyndicationFeedParserService _fcsSyndicationFeedParserService;

        public FcsFeedService(ISyndicationFeedService syndicationFeedService,
            IFcsSyndicationFeedParserService fcsSyndicationFeedParserService)
        {
            _syndicationFeedService = syndicationFeedService;
            _fcsSyndicationFeedParserService = fcsSyndicationFeedParserService;
        }

        public async Task<string> FindFirstPageFromEntryPointAsync(string uri, CancellationToken cancellationToken)
        {
            var previousArchive = uri;
            SyndicationFeed currentSyndicationFeed;

            do
            {
                currentSyndicationFeed = await _syndicationFeedService.LoadSyndicationFeedFromUriAsync(previousArchive, cancellationToken);

                previousArchive = _fcsSyndicationFeedParserService.PreviousArchiveLink(currentSyndicationFeed);
            } while (previousArchive != null);

            return _fcsSyndicationFeedParserService.CurrentArchiveLink(currentSyndicationFeed);
        }

        public async Task<IEnumerable<contract>> LoadContractsFromFeedToEndAsync(string uri, CancellationToken cancellationToken)
        {
            string nextPage = uri;
            IDictionary<string, contract> contracts = new Dictionary<string, contract>();

            do
            {
                var feed = await _syndicationFeedService.LoadSyndicationFeedFromUriAsync(nextPage, cancellationToken);

                foreach (var contract in feed.Items.Select(_fcsSyndicationFeedParserService.RetrieveContractFromSyndicationItem))
                {
                    // naive assumption that latest contract always appears later in feed.
                    contracts[contract.contractNumber] = contract;
                }

                nextPage = _fcsSyndicationFeedParserService.NextArchiveLink(feed);
            } while (nextPage != null);

            return contracts.Values;
        }
    }
}
