using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ReferenceData.EPA.Service.Tests
{
    public class OrganisationMapperTests
    {
        [Fact]
        public void Map()
        {
            var organisationId = "OrganisationId";
            var organisationName = "OrganisationName";
            var organisationUkprn = "OrganisationUkprn";
            var standardCode = "StandardCode";
            var effectiveFrom = new DateTime(2017, 1, 1);
            var effectiveTo = new DateTime(2018, 1, 1);

            var organisation = new Organisation()
            {
                Id = organisationId,
                Name = organisationName,
                Ukprn = organisationUkprn,
                Standards = new List<Standard>()
                {
                    new Standard()
                    {
                        StandardCode = standardCode,
                        Periods = new List<Period>()
                        {
                            new Period()
                            {
                                EffectiveFrom = effectiveFrom,
                                EffectiveTo = effectiveTo,
                            }
                        }
                    }
                }
            };

            var mappedOrganisation = NewMapper().MapOrganisation(organisation);

            mappedOrganisation.Id.Should().Be(organisationId);
            mappedOrganisation.Name.Should().Be(organisationName);

        }

        [Fact]
        public void Map_NullOrganisation()
        {
            NewMapper().MapOrganisation(null).Should().BeNull();
        }

        [Fact]
        public void Map_NullStandards()
        {
            var organisation = new Organisation()
            {
                Standards = null
            };

            var mappedOrganisation = NewMapper().MapOrganisation(organisation);

            mappedOrganisation.Standards.Should().BeNull();
        }

        [Fact]
        public void Map_NullPeriods()
        {
            var organisation = new Organisation()
            {
                Standards = new List<Standard>()
                {
                    new Standard()
                    {
                        Periods = null
                    }
                }
            };

            var mappedOrganisation = NewMapper().MapOrganisation(organisation);

            mappedOrganisation.Standards.First().Periods.Should().BeNull();
        }

        [Fact]
        public void MapOrganisations()
        {
            var organisations = new List<Organisation>()
            {
                new Organisation(),
                new Organisation(),
                new Organisation(),
            };

            var mappedOrganisations = NewMapper().MapOrganisations(organisations).ToList();

            mappedOrganisations.Should().HaveCount(3);
            mappedOrganisations.Should().AllBeOfType<Model.Organisation>();
        }

        [Fact]
        public void MapOrganisations_Null()
        {
            NewMapper().MapOrganisations(null).Should().BeNull();
        }

        private OrganisationMapper NewMapper()
        {
            return new OrganisationMapper();
        }
    }
}
