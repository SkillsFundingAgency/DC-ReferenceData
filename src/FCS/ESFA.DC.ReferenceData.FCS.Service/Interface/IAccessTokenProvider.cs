using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IAccessTokenProvider
    {
        Task<string> ProvideAsync();
    }
}
