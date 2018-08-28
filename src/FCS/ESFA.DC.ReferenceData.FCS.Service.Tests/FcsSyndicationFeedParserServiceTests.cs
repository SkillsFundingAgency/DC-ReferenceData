using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
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

        private FcsSyndicationFeedParserService NewService()
        {
            return new FcsSyndicationFeedParserService();
        }
    }
}
