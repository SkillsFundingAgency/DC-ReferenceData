using System.Diagnostics.Contracts;
using System.ServiceModel.Syndication;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsSyndicationFeedParserService
    {
        string PreviousArchiveLink(SyndicationFeed syndicationFeed);

        string CurrentArchiveLink(SyndicationFeed syndicationFeed);

        contract RetrieveContractFromSyndicationItem(SyndicationItem syndicationItem);
    }
}
