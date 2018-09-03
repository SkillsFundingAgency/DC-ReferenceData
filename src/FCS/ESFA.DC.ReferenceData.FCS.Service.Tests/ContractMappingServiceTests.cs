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

        [Fact]
        public void FlattenContracts_TwoLevels()
        {
            var contract = new contract()
            {
                contracts = new[]
                {
                    new contract(),
                    new contract(),
                    new contract()
                    {
                        contracts = new[]
                        {
                            new contract(), 
                        }
                    },
                }
            };

            var flattenedContracts = NewService().FlattenContracts(contract);

            flattenedContracts.Should().HaveCount(5);
        }

        [Fact]
        public void MapContract()
        {
            var contractNumber = "contractNumber";
            var contractVersionNumber = 1;
            var startDate = new DateTime(2017, 1, 1);
            var endDate = new DateTime(2018, 1, 1);

            var fcsContract = new contract()
            {
                contractNumber = contractNumber,
                contractVersionNumber = contractVersionNumber,
                startDateSpecified = true,
                startDate = startDate,
                endDateSpecified = true,
                endDate = endDate,
            };

            var contract = NewService().MapContract(fcsContract);

            contract.ContractNumber.Should().Be(contractNumber);
            contract.ContractVersionNumber.Should().Be(contractVersionNumber);
            contract.StartDate.Should().Be(startDate);
            contract.EndDate.Should().Be(endDate);
        }


        [Fact]
        public void MapContract_NullDates()
        {
            var contractNumber = "contractNumber";
            var contractVersionNumber = 1;

            var fcsContract = new contract()
            {
                contractNumber = contractNumber,
                contractVersionNumber = contractVersionNumber,
                startDateSpecified = false,
                endDateSpecified = false,
            };

            var contract = NewService().MapContract(fcsContract);

            contract.ContractNumber.Should().Be(contractNumber);
            contract.ContractVersionNumber.Should().Be(contractVersionNumber);
            contract.StartDate.Should().BeNull();
            contract.EndDate.Should().BeNull();
        }

        private ContractMappingService NewService()
        {
            return new ContractMappingService();
        }
    }
}
