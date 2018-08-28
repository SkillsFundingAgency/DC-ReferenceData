using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsFeedService
    {
        Task<string> FindFirstPageFromEntryPoint(string uri);
    }
}
