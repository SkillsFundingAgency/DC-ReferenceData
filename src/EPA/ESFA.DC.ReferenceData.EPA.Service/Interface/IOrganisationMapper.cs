using System.Collections.Generic;
using ESFA.DC.ReferenceData.EPA.Model.EPA;

namespace ESFA.DC.ReferenceData.EPA.Service.Interface
{
    public interface IOrganisationMapper
    {
        IEnumerable<Model.Organisation> MapOrganisations(IEnumerable<Organisation> organisation);

       Model.Organisation MapOrganisation(Organisation organisation);
    }
}
