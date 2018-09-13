using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ReferenceData.FCS.Service.Tests
{
    public class FcsFeedServiceTests
    {
        [Fact]
        public void ContinueToNextPage_True()
        {
            NewService().ContinueToNextPage("not null", new List<Guid>() {Guid.Empty}).Should().BeTrue();
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

        private FcsFeedService NewService(ISyndicationFeedService syndicationFeedService = null, IFcsSyndicationFeedParserService fcsSyndicationFeedParserService = null, IContractMappingService contractMappingService = null)
        {
            return new FcsFeedService(syndicationFeedService, fcsSyndicationFeedParserService, contractMappingService);
        }
    }
}
