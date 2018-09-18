using System;
using System.Collections.Generic;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ReferenceData.FCS.Service.Tests
{
    public class FcsFeedServiceTests
    {
        [Fact]
        public void ContinueToNextPage_True()
        {
            NewService().ContinueToNextPage("not null", new List<Guid>() { Guid.Empty }).Should().BeTrue();
        }

        [Fact]
        public void ContinueToNextPage_False_NullNextPage()
        {
            NewService().ContinueToNextPage(null, new List<Guid>() { Guid.Empty }).Should().BeFalse();
        }

        [Fact]
        public void ContinueToNextPage_False_EmptyNewSyndicationItemIds()
        {
            NewService().ContinueToNextPage("not null", new List<Guid>()).Should().BeFalse();
        }

        private FcsFeedService NewService(ISyndicationFeedService syndicationFeedService = null, IFcsSyndicationFeedParserService fcsSyndicationFeedParserService = null, IContractMappingService contractMappingService = null, ILogger logger = null)
        {
            return new FcsFeedService(syndicationFeedService, fcsSyndicationFeedParserService, contractMappingService, logger);
        }
    }
}
