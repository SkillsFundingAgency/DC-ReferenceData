using System.ServiceModel.Syndication;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsSyndicationFeedParserService
    {
        string PreviousArchiveLink(SyndicationFeed syndicationFeed);

        string CurrentArchiveLink(SyndicationFeed syndicationFeed);
    }
}
