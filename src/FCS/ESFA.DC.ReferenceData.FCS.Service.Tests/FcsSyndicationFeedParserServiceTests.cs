using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ReferenceData.FCS.Service.Tests
{
    public class FcsSyndicationFeedParserServiceTests
    {
        [Fact]
        public void PreviousArchiveLink_Found()
        {
            var uriString = "http://abc.def/";

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(new Uri(uriString), "prev-archive", null, null, 0) }
            };

            NewService().PreviousArchiveLink(syndicationFeed).Should().Be(uriString);
        }

        [Fact]
        public void PreviousArchiveLink_Null_NotFound()
        {
            var uriString = "http://abc.def/";

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(new Uri(uriString), "next-archive", null, null, 0) }
            };

            NewService().PreviousArchiveLink(syndicationFeed).Should().BeNull();
        }

        [Fact]
        public void PreviousArchiveLink_Null_NullUri()
        {

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(null, "prev-archive", null, null, 0) }
            };

            NewService().PreviousArchiveLink(syndicationFeed).Should().BeNull();
        }

        [Fact]
        public void CurrentArchiveLink_Found()
        {
            var uriString = "http://abc.def/";

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(new Uri(uriString), "current", null, null, 0) }
            };

            NewService().CurrentArchiveLink(syndicationFeed).Should().Be(uriString);
        }

        [Fact]
        public void CurrentArchiveLink_Null_NotFound()
        {
            var uriString = "http://abc.def/";

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(new Uri(uriString), "next-archive", null, null, 0) }
            };

            NewService().CurrentArchiveLink(syndicationFeed).Should().BeNull();
        }

        [Fact]
        public void CurrentArchiveLink_Null_NullUri()
        {

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(null, "current", null, null, 0) }
            };

            NewService().CurrentArchiveLink(syndicationFeed).Should().BeNull();
        }


        [Fact]
        public void NextArchiveLink_Found()
        {
            var uriString = "http://abc.def/";

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(new Uri(uriString), "next-archive", null, null, 0) }
            };

            NewService().NextArchiveLink(syndicationFeed).Should().Be(uriString);
        }

        [Fact]
        public void NextArchiveLink_Null_NotFound()
        {
            var uriString = "http://abc.def/";

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(new Uri(uriString), "current", null, null, 0) }
            };

            NewService().NextArchiveLink(syndicationFeed).Should().BeNull();
        }

        [Fact]
        public void NextArchiveLink_Null_NullUri()
        {

            var syndicationFeed = new SyndicationFeed()
            {
                Links = { new SyndicationLink(null, "next-archive", null, null, 0) }
            };

            NewService().NextArchiveLink(syndicationFeed).Should().BeNull();
        }

        [Fact]
        public void AtomItemSummaryFromSyndicationItem()
        {
            string summaryText = @"{UKPRN: 10001951, contractNumber: ""MAIN-3005"", version: 1}";

            //Receiving this from the Feed, not correct Json
            //string summaryText = @"{UKPRN: 10001951, contractNumber: MAIN-3005, version: 1}";

            var syndicationItem = new SyndicationItem
            {
                Summary = new TextSyndicationContent(summaryText)
            };

            var atomItemSummary = NewService(jsonSerializationService: new JsonSerializationService()).RetrieveAtomItemSummaryFromSyndicationItem(syndicationItem);

            atomItemSummary.UKPRN.Should().Be(10001951);
            atomItemSummary.contractNumber.Should().Be("MAIN-3005");
            atomItemSummary.version.Should().Be(1);
        }

        private FcsSyndicationFeedParserService NewService(IXmlSerializationService xmlSerializationService = null, IJsonSerializationService jsonSerializationService = null)
        {
            return new FcsSyndicationFeedParserService(xmlSerializationService, jsonSerializationService);
        }
    }
}
