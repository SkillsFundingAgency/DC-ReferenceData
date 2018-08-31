using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ReferenceData.FCS.Service.Tests
{
    public class ContractMappingServiceTests
    {
        [Fact]
        public void MapContractor()
        {
            var organisationIdentifier = "OrganisationIdentifier";
            var ukprn = 123456789;
            var legalName = "LegalName";

            var fcsContractor = new contractor()
            {
                organisationIdentifier = organisationIdentifier,
                ukprn = ukprn,
                legalName = legalName
            };

            var contractor = NewService().MapContractor(fcsContractor);

            contractor.OrganisationIdentifier.Should().Be(organisationIdentifier);
            contractor.Ukprn.Should().Be(ukprn);
            contractor.LegalName.Should().Be(legalName);
        }

        [Fact]
        public void FlattenContracts()
        {
            var contract = new contract()
            {
                contracts = new[] {new contract(), new contract(), new contract(),}
            };

            var flattenedContracts = NewService().FlattenContracts(contract);

            flattenedContracts.Should().HaveCount(4);

        }

        private ContractMappingService NewService()
        {
            return new ContractMappingService();
        }
    }
}
