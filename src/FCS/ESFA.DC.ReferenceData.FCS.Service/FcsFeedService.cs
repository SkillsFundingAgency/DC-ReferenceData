﻿using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<string> FindFirstPageFromEntryPoint(string uri)
        {
            var previousArchive = uri;
            SyndicationFeed currentSyndicationFeed;

            do
            {
                currentSyndicationFeed = await _syndicationFeedService.LoadFromUriAsync(previousArchive);

                previousArchive = _fcsSyndicationFeedParserService.PreviousArchiveLink(currentSyndicationFeed);

            } while (previousArchive != null);

            return _fcsSyndicationFeedParserService.CurrentArchiveLink(currentSyndicationFeed);
        }
    }
}