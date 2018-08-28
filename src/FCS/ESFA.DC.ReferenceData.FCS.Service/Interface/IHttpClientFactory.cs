using System.Net.Http;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
