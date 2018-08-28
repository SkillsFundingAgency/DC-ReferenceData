﻿using System.ServiceModel.Syndication;
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
        public void FindFirstPageFromEntryPoint()
        {
            var syndicationFeedOne = new SyndicationFeed();
            var syndicationFeedTwo = new SyndicationFeed();

            var uriOne = "uriOne";
            var uriTwo = "uriTwo";

            var syndicationFeedServiceMock = new Mock<ISyndicationFeedService>();
            var fcsSyndicationFeedParserServiceMock = new Mock<IFcsSyndicationFeedParserService>();

            syndicationFeedServiceMock.Setup(s => s.LoadFromUriAsync(uriOne)).Returns(Task.FromResult(syndicationFeedOne));
            fcsSyndicationFeedParserServiceMock.Setup(s => s.PreviousArchiveLink(syndicationFeedOne)).Returns(uriTwo);

            syndicationFeedServiceMock.Setup(s => s.LoadFromUriAsync(uriTwo)).Returns(Task.FromResult(syndicationFeedTwo));
            fcsSyndicationFeedParserServiceMock.Setup(s => s.PreviousArchiveLink(syndicationFeedTwo)).Returns(null as string);

            fcsSyndicationFeedParserServiceMock.Setup(s => s.CurrentArchiveLink(syndicationFeedTwo)).Returns(uriTwo);

            NewService(syndicationFeedServiceMock.Object, fcsSyndicationFeedParserServiceMock.Object).FindFirstPageFromEntryPoint(uriOne).Result.Should().Be(uriTwo);
        }

        private FcsFeedService NewService(ISyndicationFeedService syndicationFeedService = null, IFcsSyndicationFeedParserService fcsSyndicationFeedParserService = null)
        {
            return new FcsFeedService(syndicationFeedService, fcsSyndicationFeedParserService);
        }
    }
}