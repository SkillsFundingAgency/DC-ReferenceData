using System;
using System.ServiceModel.Syndication;
using ESFA.DC.ReferenceData.FCS.Model.FCS;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsSyndicationFeedParserService
    {
        string PreviousArchiveLink(SyndicationFeed syndicationFeed);

        string CurrentArchiveLink(SyndicationFeed syndicationFeed);

        string NextArchiveLink(SyndicationFeed syndicationFeed);

        (Guid syndicationItemId, contract contract) RetrieveContractFromSyndicationItem(SyndicationItem syndicationItem);
    }
}
