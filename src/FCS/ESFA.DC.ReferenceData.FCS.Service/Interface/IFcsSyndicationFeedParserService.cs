
using System.ServiceModel.Syndication;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.FCS;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsSyndicationFeedParserService
    {
        string PreviousArchiveLink(SyndicationFeed syndicationFeed);

        string CurrentArchiveLink(SyndicationFeed syndicationFeed);

        string NextArchiveLink(SyndicationFeed syndicationFeed);

        contract RetrieveContractFromSyndicationItem(SyndicationItem syndicationItem);

        AtomItemSummary RetrieveAtomItemSummaryFromSyndicationItem(SyndicationItem syndicationItem);
    }
}
