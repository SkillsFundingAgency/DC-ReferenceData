using ESFA.DC.ReferenceData.ULN.Model.ULN;
using System.IO;

namespace ESFA.DC.ReferenceData.ULN.Service.Interface
{
    public interface IULNFileDeserializer
    {
        ULNFile Deserialize(Stream stream);
    }
}
