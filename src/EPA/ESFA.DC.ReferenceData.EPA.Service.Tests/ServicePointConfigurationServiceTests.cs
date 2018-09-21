using ESFA.DC.ReferenceData.EPA.Service.Config;
using ESFA.DC.ReferenceData.EPA.Service.Config.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ReferenceData.EPA.Service.Tests
{
    public class ServicePointConfigurationServiceTests
    {
        [Fact]
        public void ConfigureServicePoint()
        {
            var epaServiceConfigurationMock = new Mock<IEpaServiceConfiguration>();

            epaServiceConfigurationMock.SetupGet(c => c.EndpointUri).Returns("http://localhost");

            var servicePoint = NewService(epaServiceConfigurationMock.Object).ConfigureServicePoint();

            servicePoint.ConnectionLimit.Should().Be(250);
            servicePoint.UseNagleAlgorithm.Should().BeFalse();
        }

        private ServicePointConfigurationService NewService(IEpaServiceConfiguration epaServiceConfiguration = null)
        {
            return new ServicePointConfigurationService(epaServiceConfiguration);
        }
    }
}
