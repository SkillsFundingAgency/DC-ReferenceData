using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.FCS;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IContractMappingService
    {

        MasterContract Map(contract contract);
    }
}
