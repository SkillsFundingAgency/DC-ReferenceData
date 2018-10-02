using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using ESFA.DC.ReferenceData.EPA.Service.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ReferenceData.EPA.Service.Tests
{
    public class EpaFeedServiceTests
    {
        [Fact]
        public async Task GetOrganisationsAsync_SingleOrganisation()
        {
            var organisationId = "OrganisationId";

            IEnumerable<Organisation> organisations = new List<Organisation>()
            {
                new Organisation()
                {
                    Id = organisationId
                }
            };

            IEnumerable<Standard> standards = new List<Standard>
            {
                new Standard(),
                new Standard(),
            };

            var cancellationToken = CancellationToken.None;

            var epaRestClientMock = new Mock<IEpaRestClient>();

            epaRestClientMock.Setup(c => c.GetOrganisationsAsync(cancellationToken)).Returns(Task.FromResult(organisations));
            epaRestClientMock.Setup(c => c.GetStandardsForOrganisationAsync(organisationId, cancellationToken)).Returns(Task.FromResult(standards));
            
            var loggerMock = new Mock<ILogger>();

            loggerMock.Setup(l => l.LogInfo("EPA Reference Data - Starting Get Organisations", It.IsAny<object[]>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Verifiable();
            loggerMock.Setup(l => l.LogInfo("EPA Reference Data - Finishing Get Organisations, Count : 1", It.IsAny<object[]>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));

            var organisationsResult = await NewService(epaRestClientMock.Object, loggerMock.Object).GetOrganisationsAsync(cancellationToken);

            organisationsResult.Should().HaveCount(1);

            var organisation = organisationsResult.First();
            organisation.Standards.Should().HaveCount(2);
            organisation.Standards.Should().BeEquivalentTo(standards);

            loggerMock.VerifyAll();
        }

        [Fact]
        public async Task GetOrganisationsAsync_MultipleOrganisations()
        {
            var organisationIdOne = "OrganisationIdOne";
            var organisationIdTwo = "OrganisationIdTwo";

            IEnumerable<Organisation> organisations = new List<Organisation>()
            {
                new Organisation()
                {
                    Id = organisationIdOne
                },
                new Organisation()
                {
                    Id = organisationIdTwo
                }
            };

            IEnumerable<Standard> standardsOne = new List<Standard>
            {
                new Standard(),
                new Standard(),
            };

            IEnumerable<Standard> standardsTwo = new List<Standard>
            {
                new Standard(),
                new Standard(),
                new Standard(),
            };

            var cancellationToken = CancellationToken.None;

            var epaRestClientMock = new Mock<IEpaRestClient>();

            epaRestClientMock.Setup(c => c.GetOrganisationsAsync(cancellationToken)).Returns(Task.FromResult(organisations));
            epaRestClientMock.Setup(c => c.GetStandardsForOrganisationAsync(organisationIdOne, cancellationToken)).Returns(Task.FromResult(standardsOne));
            epaRestClientMock.Setup(c => c.GetStandardsForOrganisationAsync(organisationIdTwo, cancellationToken)).Returns(Task.FromResult(standardsTwo));

            var loggerMock = new Mock<ILogger>();

            loggerMock.Setup(l => l.LogInfo("EPA Reference Data - Starting Get Organisations", It.IsAny<object[]>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Verifiable();
            loggerMock.Setup(l => l.LogInfo("EPA Reference Data - Finishing Get Organisations, Count : 2", It.IsAny<object[]>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));

            var organisationsResult = (await NewService(epaRestClientMock.Object, loggerMock.Object).GetOrganisationsAsync(cancellationToken)).ToList();

            organisationsResult.Should().HaveCount(2);

            var organisationOne = organisationsResult[0];
            organisationOne.Standards.Should().HaveCount(2);
            organisationOne.Standards.Should().BeEquivalentTo(standardsOne);

            var organisationTwo = organisationsResult[1];
            organisationTwo.Standards.Should().HaveCount(3);
            organisationTwo.Standards.Should().BeEquivalentTo(standardsTwo);

            loggerMock.VerifyAll();
        }

        private EpaFeedService NewService(IEpaRestClient epaRestClient = null, ILogger logger = null)
        {
            return new EpaFeedService(epaRestClient, logger);
        }
    }
}
