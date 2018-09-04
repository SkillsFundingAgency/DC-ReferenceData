﻿using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.DC;
using ESFA.DC.ReferenceData.FCS.Model.FCS;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IContractMappingService
    {

        Contractor Map(contract contract);
    }
}