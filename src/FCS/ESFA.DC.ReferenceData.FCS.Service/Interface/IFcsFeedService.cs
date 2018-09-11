using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service.Model;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsFeedService
    {
        Task<IEnumerable<Contractor>> GetNewContractorsFromFeedAsync(string uri, IEnumerable<Guid> syndicationItemIds, CancellationToken cancellationToken);
    }
}
