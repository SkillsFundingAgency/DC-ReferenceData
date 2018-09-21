using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using ESFA.DC.ReferenceData.EPA.Service.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ReferenceData.EPA.Service.Tests
{
    public class EpaReferenceDataTaskTests
    {
        [Fact]
        public void TaskName()
        {
            NewTask().TaskName.Should().Be("Epa");
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var servicePointConfigurationServiceMock = new Mock<IServicePointConfigurationService>();
            var epaFeedServiceMock = new Mock<IEpaFeedService>();
            var organisationMapperMock = new Mock<IOrganisationMapper>();
            var epaPersistenceServiceMock = new Mock<IEpaPersistenceService>();

            var cancellationToken = CancellationToken.None;

            IEnumerable<Organisation> organisations = new List<Organisation>();
            IEnumerable<Model.Organisation> modelOrganisations = new List<Model.Organisation>();

            servicePointConfigurationServiceMock.Setup(s => s.ConfigureServicePoint()).Verifiable();
            epaFeedServiceMock.Setup(s => s.GetOrganisationsAsync(cancellationToken)).Returns(Task.FromResult(organisations)).Verifiable();
            organisationMapperMock.Setup(m => m.MapOrganisations(organisations)).Returns(modelOrganisations).Verifiable();
            epaPersistenceServiceMock.Setup(s => s.PersistEpaOrganisationsAsync(modelOrganisations, cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            await NewTask(servicePointConfigurationServiceMock.Object, epaFeedServiceMock.Object, organisationMapperMock.Object, epaPersistenceServiceMock.Object).ExecuteAsync(cancellationToken);

            servicePointConfigurationServiceMock.VerifyAll();
            epaFeedServiceMock.VerifyAll();
            organisationMapperMock.VerifyAll();
            epaPersistenceServiceMock.VerifyAll();
        }

        private EpaReferenceDataTask NewTask(IServicePointConfigurationService servicePointConfigurationService = null, IEpaFeedService epaFeedService = null, IOrganisationMapper organisationMapper = null, IEpaPersistenceService epaPersistenceService = null)
        {
            return new EpaReferenceDataTask(servicePointConfigurationService, epaFeedService, organisationMapper, epaPersistenceService);
        }
    }
}
