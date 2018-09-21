using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using ESFA.DC.ReferenceData.EPA.Service.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class OrganisationMapper : IOrganisationMapper
    {
        public Model.Organisation MapOrganisation(Organisation organisation)
        {
            if (organisation == null)
            {
                return null;
            }

            return new Model.Organisation()
            {
                Id = organisation.Id,
                Name = organisation.Name,
                Ukprn = organisation.Ukprn,
                Standards = organisation.Standards?.Select(
                    standard =>
                        new Model.Standard()
                        {
                            StandardCode = standard.StandardCode,
                            Periods = standard.Periods?.Select(
                                period => new Model.Period()
                                {
                                    EffectiveFrom = period.EffectiveFrom,
                                    EffectiveTo = period.EffectiveTo,
                                }).ToList()
                        }).ToList()
            };
        }

        public IEnumerable<Model.Organisation> MapOrganisations(IEnumerable<Organisation> organisations)
        {
            return organisations?.Select(MapOrganisation);
        }
    }
}
