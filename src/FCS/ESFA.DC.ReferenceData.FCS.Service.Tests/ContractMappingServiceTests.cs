using System;
using System.Linq;
using ESFA.DC.ReferenceData.FCS.Model.FCS;
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
        public void MapMasterContract()
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

            var contract = NewService().MapMasterContract(fcsContract);

            contract.ContractNumber.Should().Be(contractNumber);
            contract.ContractVersionNumber.Should().Be(contractVersionNumber);
            contract.StartDate.Should().Be(startDate);
            contract.EndDate.Should().Be(endDate);
        }

        [Fact]
        public void MapContract_NullAllocations()
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
        public void MapContract_Allocations()
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
                contractAllocations = new[]
                {
                    new contractAllocationsContractAllocation()
                }
            };

            var contract = NewService().MapContract(fcsContract);

            contract.ContractNumber.Should().Be(contractNumber);
            contract.ContractVersionNumber.Should().Be(contractVersionNumber);
            contract.StartDate.Should().Be(startDate);
            contract.EndDate.Should().Be(endDate);
            contract.ContractAllocations.Should().HaveCount(1);
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

        [Fact]
        public void FlattenContractAllocations()
        {
            var contractAllocation = new contractAllocationsContractAllocation()
            {
                contractAllocations = new[]
                {
                    new contractAllocationsContractAllocation(),
                    new contractAllocationsContractAllocation(),
                    new contractAllocationsContractAllocation(),
                }
            };

            NewService().FlattenContractAllocations(contractAllocation).Should().HaveCount(4);
        }

        [Fact]
        public void FlattenContractAllocations_TwoLevels()
        {
            var contractAllocation = new contractAllocationsContractAllocation()
            {
                contractAllocations = new[]
                {
                    new contractAllocationsContractAllocation(),
                    new contractAllocationsContractAllocation(),
                    new contractAllocationsContractAllocation()
                    {
                        contractAllocations = new []
                        {
                            new contractAllocationsContractAllocation(),
                            new contractAllocationsContractAllocation(),
                        }
                    },
                }
            };

            NewService().FlattenContractAllocations(contractAllocation).Should().HaveCount(6);
        }

        [Fact]
        public void MapContractAllocation_NullDeliverables()
        {
            var contractAllocationNumber = "contractAllocationNumber";
            var fundingStreamCode = "fundingStreamCode";
            var fundingStreamPeriodCode = "fundingStreamPeriodCode";
            var period = "period";
            var periodTypeCode = periodTypeCodeType.LEVY;
            var uopCode = "uopCode";
            var startDate = new DateTime(2017, 1, 1);
            var endDate = new DateTime(2018, 1, 1);
                
            var fcsContractAllocation = new contractAllocationsContractAllocation()
            {
                contractAllocationNumber = contractAllocationNumber,
                fundingStream = new fundingStream() { fundingStreamCode = fundingStreamCode },
                fundingStreamPeriodCode = fundingStreamPeriodCode,
                period = new period()
                {
                    period1 = period,
                    periodType = new periodTypeType() { periodTypeCode = periodTypeCode }
                },
                uopCode = uopCode,
                startDateSpecified = true,
                startDate = startDate,
                endDateSpecified = true,
                endDate = endDate,
            };

            var contractAllocation = NewService().MapContractAllocation(fcsContractAllocation);

            contractAllocation.ContractAllocationNumber.Should().Be(contractAllocationNumber);
            contractAllocation.FundingStreamCode.Should().Be(fundingStreamCode);
            contractAllocation.FundingStreamPeriodCode.Should().Be(fundingStreamPeriodCode);
            contractAllocation.Period.Should().Be(period);
            contractAllocation.PeriodTypeCode.Should().Be("LEVY");
            contractAllocation.UoPCode.Should().Be(uopCode);
            contractAllocation.StartDate.Should().Be(startDate);
            contractAllocation.EndDate.Should().Be(endDate);
        }

        [Fact]
        public void MapContractAllocation_Deliverables()
        {
            var contractAllocationNumber = "contractAllocationNumber";
            var fundingStreamCode = "fundingStreamCode";
            var fundingStreamPeriodCode = "fundingStreamPeriodCode";
            var period = "period";
            var periodTypeCode = periodTypeCodeType.LEVY;
            var uopCode = "uopCode";
            var startDate = new DateTime(2017, 1, 1);
            var endDate = new DateTime(2018, 1, 1);

            var fcsContractAllocation = new contractAllocationsContractAllocation()
            {
                contractAllocationNumber = contractAllocationNumber,
                fundingStream = new fundingStream() { fundingStreamCode = fundingStreamCode },
                fundingStreamPeriodCode = fundingStreamPeriodCode,
                period = new period()
                {
                    period1 = period,
                    periodType = new periodTypeType() { periodTypeCode = periodTypeCode }
                },
                uopCode = uopCode,
                startDateSpecified = true,
                startDate = startDate,
                endDateSpecified = true,
                endDate = endDate,
                contractDeliverables = new []
                {
                    new contractDeliverablesTypeContractDeliverable(), 
                }
               
            };

            var contractAllocation = NewService().MapContractAllocation(fcsContractAllocation);

            contractAllocation.ContractAllocationNumber.Should().Be(contractAllocationNumber);
            contractAllocation.FundingStreamCode.Should().Be(fundingStreamCode);
            contractAllocation.FundingStreamPeriodCode.Should().Be(fundingStreamPeriodCode);
            contractAllocation.Period.Should().Be(period);
            contractAllocation.PeriodTypeCode.Should().Be("LEVY");
            contractAllocation.UoPCode.Should().Be(uopCode);
            contractAllocation.StartDate.Should().Be(startDate);
            contractAllocation.EndDate.Should().Be(endDate);
            contractAllocation.ContractDeliverables.Should().HaveCount(1);
        }

        [Fact]
        public void MapContractAllocation_NullDates()
        {
            var contractAllocationNumber = "contractAllocationNumber";
            var fundingStreamCode = "fundingStreamCode";
            var fundingStreamPeriodCode = "fundingStreamPeriodCode";
            var period = "period";
            var periodTypeCode = periodTypeCodeType.LEVY;
            var uopCode = "uopCode";

            var fcsContractAllocation = new contractAllocationsContractAllocation()
            {
                contractAllocationNumber = contractAllocationNumber,
                fundingStream = new fundingStream() { fundingStreamCode = fundingStreamCode },
                fundingStreamPeriodCode = fundingStreamPeriodCode,
                period = new period()
                {
                    period1 = period,
                    periodType = new periodTypeType() { periodTypeCode = periodTypeCode }
                },
                uopCode = uopCode,
                startDateSpecified = false,
                endDateSpecified = false,
            };

            var contractAllocation = NewService().MapContractAllocation(fcsContractAllocation);

            contractAllocation.ContractAllocationNumber.Should().Be(contractAllocationNumber);
            contractAllocation.FundingStreamCode.Should().Be(fundingStreamCode);
            contractAllocation.FundingStreamPeriodCode.Should().Be(fundingStreamPeriodCode);
            contractAllocation.Period.Should().Be(period);
            contractAllocation.PeriodTypeCode.Should().Be("LEVY");
            contractAllocation.UoPCode.Should().Be(uopCode);
            contractAllocation.StartDate.Should().BeNull();
            contractAllocation.EndDate.Should().BeNull();
        }

        [Fact]
        public void MapContractDeliverable()
        {
            var description = "description";
            var deliverableCode = 1;
            var unitCost = 1.2m;
            var plannedVolume = 2;
            var plannedValue = 1.3m;

            var fcsContractDeliverable = new contractDeliverablesTypeContractDeliverable()
            {
                deliverableDescription = description,
                deliverable = new deliverableType() {  deliverableCode = deliverableCode },
                unitCostSpecified = true,
                unitCost = unitCost,
                plannedVolumeSpecified = true,
                plannedVolume = plannedVolume,
                plannedValueSpecified = true,
                plannedValue = plannedValue
            };

            var contractDeliverable = NewService().MapContractDeliverable(fcsContractDeliverable);

            contractDeliverable.Description.Should().Be(description);
            contractDeliverable.DeliverableCode.Should().Be(deliverableCode);
            contractDeliverable.UnitCost.Should().Be(unitCost);
            contractDeliverable.PlannedVolume.Should().Be(plannedVolume);
            contractDeliverable.PlannedValue.Should().Be(plannedValue);
        }


        [Fact]
        public void MapContractDeliverable_Nulls()
        {
            var description = "description";
            var deliverableCode = 1;

            var fcsContractDeliverable = new contractDeliverablesTypeContractDeliverable()
            {
                deliverableDescription = description,
                deliverable = new deliverableType() { deliverableCode = deliverableCode },
                unitCostSpecified = false,
                plannedVolumeSpecified = false,
                plannedValueSpecified = false,
            };

            var contractDeliverable = NewService().MapContractDeliverable(fcsContractDeliverable);

            contractDeliverable.Description.Should().Be(description);
            contractDeliverable.DeliverableCode.Should().Be(deliverableCode);
            contractDeliverable.UnitCost.Should().BeNull();
            contractDeliverable.PlannedVolume.Should().BeNull();
            contractDeliverable.PlannedValue.Should().BeNull();
        }

        [Fact]
        public void MapTree()
        {
            var contractNumberMaster = "Master";
            var contractNumberA = "ContractA";
            var subContractNumberA = "SubContractA";
            var contractNumberB = "ContractB";

            var contractAllocationNumberA1 = "ContractAllocationA1";
            var subContractAllocationNumberA1 = "SubContractAllocationA1";
            var contractAllocationNumberA2 = "ContractAllocationA2";

            var deliverableDescriptionA1 = "DeliverableA1";
            var subDeliverableDescriptionA1 = "SubDeliverableA1";
            var deliverableDescriptionA2 = "DeliverableA2";

            var contract = new contract()
            {
                contractor = new contractor(),
                contractNumber = contractNumberMaster,
                hierarchyType = hierarchyType.MASTERCONTRACT,
                contracts = new []
                {
                    new contract()
                    {
                        contractNumber = contractNumberA,
                        hierarchyType = hierarchyType.CONTRACT,
                        contracts = new[]
                        {
                            new contract()
                            {
                                contractNumber = subContractNumberA,
                                hierarchyType = hierarchyType.CONTRACT,
                            }
                        },
                        contractAllocations = new []
                        {
                            new contractAllocationsContractAllocation()
                            {
                                contractAllocationNumber = contractAllocationNumberA1,
                                contractDeliverables = new []
                                {
                                    new contractDeliverablesTypeContractDeliverable()
                                    {
                                        deliverableDescription = deliverableDescriptionA1,
                                        contractDeliverables = new []
                                        {
                                            new contractDeliverablesTypeContractDeliverable()
                                            {
                                                deliverableDescription = subDeliverableDescriptionA1
                                            }
                                        }
                                    },
                                    new contractDeliverablesTypeContractDeliverable()
                                    {
                                        deliverableDescription = deliverableDescriptionA2
                                    }
                                },
                                contractAllocations = new []
                                {
                                    new contractAllocationsContractAllocation()
                                    {
                                        contractAllocationNumber = subContractAllocationNumberA1
                                    } 
                                }
                            },
                            new contractAllocationsContractAllocation()
                            {
                                contractAllocationNumber = contractAllocationNumberA2
                            }
                        }
                    },
                    new contract()
                    {
                        contractNumber = contractNumberB,
                        hierarchyType = hierarchyType.CONTRACT,
                    }
                }
            };

            var masterContract = NewService().Map(contract);

            masterContract.ContractNumber.Should().Be(contractNumberMaster);

            var contractor = masterContract.Contractor;

            contractor.Contracts.Should().HaveCount(3);
            contractor.Contracts.Select(c => c.ContractNumber).Should().Contain(contractNumberA, contractNumberB, subContractNumberA);

            var contractAllocations = contractor.Contracts.First(c => c.ContractNumber == contractNumberA).ContractAllocations;

            contractAllocations.Should().HaveCount(3);
            contractAllocations.Select(a => a.ContractAllocationNumber).Should().Contain(contractAllocationNumberA1, contractAllocationNumberA2, subContractAllocationNumberA1);

            var contractDeliverables = contractAllocations.First(a => a.ContractAllocationNumber == contractAllocationNumberA1).ContractDeliverables;

            contractDeliverables.Should().HaveCount(3);
            contractDeliverables.Select(d => d.Description).Should().Contain(deliverableDescriptionA1, deliverableDescriptionA2, subDeliverableDescriptionA1);
        }

        [Fact]
        public void Map_NullContractor()
        {
            var contract = new contract();

            Action action = () => NewService().Map(contract);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FlattenContractDeliverables()
        {
            var contractDeliverable = new contractDeliverablesTypeContractDeliverable()
            {
                contractDeliverables = new[]
                {
                    new contractDeliverablesTypeContractDeliverable(),
                    new contractDeliverablesTypeContractDeliverable(),
                    new contractDeliverablesTypeContractDeliverable(),
                }
            };

            NewService().FlattenContractDeliverables(contractDeliverable).Should().HaveCount(4);
        }

        [Fact]
        public void FlattenContractDeliverables_TwoLevels()
        {
            var contractDeliverable = new contractDeliverablesTypeContractDeliverable()
            {
                contractDeliverables = new[]
                {
                    new contractDeliverablesTypeContractDeliverable(),
                    new contractDeliverablesTypeContractDeliverable(),
                    new contractDeliverablesTypeContractDeliverable()
                    {
                        contractDeliverables = new[]
                        {
                            new contractDeliverablesTypeContractDeliverable(),
                            new contractDeliverablesTypeContractDeliverable(),
                            new contractDeliverablesTypeContractDeliverable(),
                        }
                    }
                }
            };

            NewService().FlattenContractDeliverables(contractDeliverable).Should().HaveCount(7);
        }

        private ContractMappingService NewService()
        {
            return new ContractMappingService();
        }
    }
}
