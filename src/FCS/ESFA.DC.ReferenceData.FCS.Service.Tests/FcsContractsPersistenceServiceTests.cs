using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ReferenceData.FCS.Service.Tests
{
    public class FcsContractsPersistenceServiceTests
    {
        [Fact]
        public void GetMaxVersionMasterContractKeys()
        {
            var masterContractKeys = new List<ContractKey>()
            {
                new ContractKey("1", 1),
                new ContractKey("1", 2),
                new ContractKey("1", 3),
                new ContractKey("2", 1),
                new ContractKey("2", 1),
                new ContractKey("3", 1),
            };

            var expectedContractKeys = new List<ContractKey>()
            {
                new ContractKey("1", 3),
                new ContractKey("2", 1),
                new ContractKey("3", 1),
            };

            var maxVersionContractkeys = NewService().GetMaxVersionMasterContractKeys(masterContractKeys).ToList();

            maxVersionContractkeys.Should().HaveCount(3);
            maxVersionContractkeys.Should().Contain(expectedContractKeys);
        }

        [Fact]
        public void GetNewMasterContractKeys()
        {
            var masterContractKeys = new List<ContractKey>()
            {
                new ContractKey("1", 2),
                new ContractKey("2", 1),
                new ContractKey("3", 2),
                new ContractKey("4", 1)
            };

            var existingMasterContractKeys = new List<ContractKey>()
            {
                new ContractKey("1", 2),
                new ContractKey("2", 1),
                new ContractKey("3", 1),
                new ContractKey("4", 2),
            };

            var newMasterContractKeys = NewService().GetNewMasterContractKeys(masterContractKeys, existingMasterContractKeys).ToList();

            newMasterContractKeys.Should().HaveCount(1);
            newMasterContractKeys.Should().Contain(new ContractKey("3", 2));
        }

        [Fact]
        public void GetDefunctExistingMasterContractKeys()
        {
            var masterContractKeys = new List<ContractKey>()
            {
                new ContractKey("1", 2),
                new ContractKey("2", 1),
                new ContractKey("3", 2),
            };

            var existingMasterContractKeys = new List<ContractKey>()
            {
                new ContractKey("1", 1),
                new ContractKey("2", 1),
                new ContractKey("3", 1),
            };

            var expectedDefunctMasterContractKeys = new List<ContractKey>()
            {
                new ContractKey("1", 1),
                new ContractKey("3", 1),
            };

            var defunctMasterContractKeys = NewService().GetDefunctExistingMasterContractKeys(masterContractKeys, existingMasterContractKeys).ToList();

            defunctMasterContractKeys.Should().HaveCount(2);
            defunctMasterContractKeys.Should().Contain(expectedDefunctMasterContractKeys);
        }

        private FcsContractsPersistenceService NewService(IFcsContext fcsContext = null)
        {
            return new FcsContractsPersistenceService(fcsContext);
        }
    }
}
