using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.FCS;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsSyndicationFeedParserService : IFcsSyndicationFeedParserService
    {
        private const string PreviousArchive = "prev-archive";
        private const string CurrentArchive = "current";
        private const string NextArchive = "next-archive";
        private readonly IXmlSerializationService _xmlserializationService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public FcsSyndicationFeedParserService(IXmlSerializationService xmlSerializationService, IJsonSerializationService jsonSerializationService)
        {
            _xmlserializationService = xmlSerializationService;
            _jsonSerializationService = jsonSerializationService;
        }
        
        public string CurrentArchiveLink(SyndicationFeed syndicationFeed)
        {
            return RetrieveLinkForRelationshipType(syndicationFeed, CurrentArchive);
        }

        public string PreviousArchiveLink(SyndicationFeed syndicationFeed)
        {
            return RetrieveLinkForRelationshipType(syndicationFeed, PreviousArchive);
        }

        public string NextArchiveLink(SyndicationFeed syndicationFeed)
        {
            return RetrieveLinkForRelationshipType(syndicationFeed, NextArchive);
        }
        
        public contract RetrieveContractFromSyndicationItem(SyndicationItem syndicationItem)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    syndicationItem.Content.WriteTo(xmlWriter, "temp", "temp");
                }
                
                var contract = XDocument.Parse(stringWriter.ToString()).Descendants().Where(x => x.Name.LocalName == "contract").ToList();

                using (var memoryStream = new MemoryStream())
                {
                    using (var xmlWriter = XmlWriter.Create(memoryStream))
                    { 
                        contract.First().WriteTo(xmlWriter);
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var deserializedContract = _xmlserializationService.Deserialize<contract>(memoryStream);

                    return deserializedContract;
                }
            }
        }

        private string RetrieveLinkForRelationshipType(SyndicationFeed syndicationFeed, string relationshipType)
        {
            return syndicationFeed.Links.FirstOrDefault(l => l.RelationshipType == relationshipType)?.Uri?.ToString();
        }
    }
}
