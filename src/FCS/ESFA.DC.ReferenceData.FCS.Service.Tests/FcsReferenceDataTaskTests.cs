using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ReferenceData.FCS.Service.Tests
{
    public class FcsReferenceDataTaskTests
    {
        [Fact]
        public void TaskName()
        {
            NewTask().TaskName.Should().Be("Fcs");
        }

        private FcsReferenceDataTask NewTask(IFcsServiceConfiguration fcsServiceConfiguration = null, IFcsFeedService fcsFeedService = null, IFcsContractsPersistenceService fcsContractsPersistenceService = null, ILogger logger = null)
        {
            return new FcsReferenceDataTask(fcsServiceConfiguration, fcsFeedService, fcsContractsPersistenceService, logger);
        }
    }
}
