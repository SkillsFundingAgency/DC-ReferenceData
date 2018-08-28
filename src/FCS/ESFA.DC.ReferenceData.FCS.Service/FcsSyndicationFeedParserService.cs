using System.Linq;
using System.ServiceModel.Syndication;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsSyndicationFeedParserService : IFcsSyndicationFeedParserService
    {
        private const string PreviousArchive = "prev-archive";
        private const string CurrentArchive = "current";
        
        public string CurrentArchiveLink(SyndicationFeed syndicationFeed)
        {
            return RetrieveLinkForRelationshipType(syndicationFeed, CurrentArchive);
        }

        public string PreviousArchiveLink(SyndicationFeed syndicationFeed)
        {
            return RetrieveLinkForRelationshipType(syndicationFeed, PreviousArchive);
        }

        private string RetrieveLinkForRelationshipType(SyndicationFeed syndicationFeed, string relationshipType)
        {
            return syndicationFeed.Links.FirstOrDefault(l => l.RelationshipType == relationshipType)?.Uri?.ToString();
        }
    }
}
