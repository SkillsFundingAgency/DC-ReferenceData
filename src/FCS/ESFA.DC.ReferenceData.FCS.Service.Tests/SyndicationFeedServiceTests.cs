using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Xunit;

namespace ESFA.DC.ReferenceData.FCS.Service.Tests
{
    public class SyndicationFeedServiceTests
    {
        [Fact]
        public async Task LoadSyndicationFeedFromUri()
        {
            var atom = File.ReadAllText(@"Files\Atom.xml");

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(atom)
                }));

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            var syndicationFeed = await NewService(httpClient).LoadSyndicationFeedFromUriAsync("http://any.test.uri", CancellationToken.None);

            syndicationFeed.Items.Should().HaveCount(1);
            syndicationFeed.Id.Should().Be("uuid:12537fce-65bc-4f58-bb2d-183f8c3ad069");
        }

        private SyndicationFeedService NewService(HttpClient httpClient)
        {
            return new SyndicationFeedService(httpClient);
        }
    }
}
