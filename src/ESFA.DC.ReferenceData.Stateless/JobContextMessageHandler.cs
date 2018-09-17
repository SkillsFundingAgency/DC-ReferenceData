using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using ESFA.DC.ReferenceData.Stateless.Interfaces;

namespace ESFA.DC.ReferenceData.Stateless
{
    public class JobContextMessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly IFcsServiceConfiguration _fcsServiceConfiguration;
        private readonly IFcsFeedService _fcsFeedService;
        private readonly IFcsContractsPersistenceService _fcsContractsPersistenceService;

        public JobContextMessageHandler(IFcsServiceConfiguration fcsServiceConfiguration, IFcsFeedService fcsFeedService, IFcsContractsPersistenceService fcsContractsPersistenceService)
        {
            _fcsServiceConfiguration = fcsServiceConfiguration;
            _fcsFeedService = fcsFeedService;
            _fcsContractsPersistenceService = fcsContractsPersistenceService;
        }

        public async Task<bool> HandleAsync(JobContextMessage message, CancellationToken cancellationToken)
        {
            var existingSyndicationItemIds = await _fcsContractsPersistenceService.GetExistingSyndicationItemIds(cancellationToken);

            var fcsContracts = await _fcsFeedService.GetNewContractorsFromFeedAsync(_fcsServiceConfiguration.FeedUri + "/api/contracts/notifications/approval-onwards", existingSyndicationItemIds, cancellationToken);

            await _fcsContractsPersistenceService.PersistContracts(fcsContracts, CancellationToken.None);

            return true;
        }
    }
}
