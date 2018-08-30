using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsSyndicationFeedParserService : IFcsSyndicationFeedParserService
    {
        private const string PreviousArchive = "prev-archive";
        private const string CurrentArchive = "current";
        private readonly IXmlSerializationService _xmlserializationService;

        public FcsSyndicationFeedParserService(IXmlSerializationService xmlSerializationService)
        {
            _xmlserializationService = xmlSerializationService;
        }
        
        public string CurrentArchiveLink(SyndicationFeed syndicationFeed)
        {
            return RetrieveLinkForRelationshipType(syndicationFeed, CurrentArchive);
        }

        public string PreviousArchiveLink(SyndicationFeed syndicationFeed)
        {
            return RetrieveLinkForRelationshipType(syndicationFeed, PreviousArchive);
        }

        public contract RetrieveContractFromSyndicationItem(SyndicationItem syndicationItem)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    syndicationItem.GetAtom10Formatter().WriteTo(xmlWriter);
                }
                
                var contract = XDocument.Parse(stringWriter.ToString()).Descendants().First(x => x.Name.LocalName == "contract");

                using (var memoryStream = new MemoryStream())
                {
                    using (var xmlWriter = XmlWriter.Create(memoryStream))
                    { 
                        contract.WriteTo(xmlWriter);
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return _xmlserializationService.Deserialize<contract>(memoryStream);
                }
            }
        }

        private string RetrieveLinkForRelationshipType(SyndicationFeed syndicationFeed, string relationshipType)
        {
            return syndicationFeed.Links.FirstOrDefault(l => l.RelationshipType == relationshipType)?.Uri?.ToString();
        }
    }
}
