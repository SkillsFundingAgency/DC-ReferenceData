using System;
using System.Collections.Generic;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.DC;
using ESFA.DC.ReferenceData.FCS.Model.FCS;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class ContractMappingService : IContractMappingService
    {
        public Contractor Map(contract contract)
        {
            var contractor = MapContractor(contract.contractor);

            var contracts = FlattenContracts(contract);
            
            

            return contractor;
        }

        public Contractor MapContractor(contractor contractor)
        {
            return new Contractor()
            {
                OrganisationIdentifier = contractor.organisationIdentifier,
                Ukprn = contractor.ukprn,
                LegalName = contractor.legalName
            };
        }

        public Contract MapContract(contract contract)
        {
            return new Contract()
            {
                ContractNumber = contract.contractNumber,
                ContractVersionNumber = contract.contractVersionNumber,
                StartDate = contract.startDateSpecified ? contract.startDate : null,
                EndDate = contract.endDateSpecified ? contract.endDate : null,
            };
        }

        public ContractAllocation MapContractAllocation(contractAllocationsContractAllocation contractAllocation)
        {
            return new ContractAllocation()
            {
                ContractAllocationNumber = contractAllocation.contractAllocationNumber,
                FundingStreamCode = contractAllocation.fundingStream.fundingStreamCode,
                FundingStreamPeriodCode = contractAllocation.fundingStreamPeriodCode,
                Period = contractAllocation.period.period1,
                PeriodTypeCode = contractAllocation.period.periodType.periodTypeCode.ToString(),
                UoPCode = contractAllocation.uopCode,
                StartDate = contractAllocation.startDateSpecified ? contractAllocation.startDate : default(DateTime?),
                EndDate = contractAllocation.endDateSpecified ? contractAllocation.endDate : null,
            };
        }

        public ContractDeliverable MapContractDeliverable(contractDeliverablesTypeContractDeliverable contractDeliverable)
        {
            return new ContractDeliverable()
            {
                DeliverableCode = contractDeliverable.deliverable.deliverableCode,
                Description = contractDeliverable.deliverableDescription,
                PlannedValue = contractDeliverable.plannedValueSpecified ? contractDeliverable.plannedValue : default(decimal?),
                PlannedVolume = contractDeliverable.plannedVolumeSpecified ? contractDeliverable.plannedVolume : default(int?),
                UnitCost = contractDeliverable.unitCostSpecified ? contractDeliverable.unitCost : default(decimal?)
            };
        }

        public ICollection<contractType> FlattenContracts(contractType contract)
        {
            return Flatten(contract, c => c.contracts);
        }

        public ICollection<contractAllocationsContractAllocation> FlattenContractAllocations(contractAllocationsContractAllocation contractAllocation)
        {
            return Flatten(contractAllocation, a => a.contractAllocations);
        }

        public ICollection<contractDeliverablesTypeContractDeliverable> FlattenContractDeliverables(contractDeliverablesTypeContractDeliverable contractDeliverable)
        {
            return Flatten(contractDeliverable, d => d.contractDeliverables);
        }

        private ICollection<T> Flatten<T>(T obj, Func<T, IEnumerable<T>> selector)
        {
            var collection = new List<T>();

            Func<T, ICollection<T>, ICollection<T>> recursiveFunc = null;

            recursiveFunc = (con, objectCollection) =>
            {
                objectCollection.Add(con);

                var selectedEnumerable = selector.Invoke(con);

                if (selectedEnumerable != null)
                {
                    foreach (var o in selectedEnumerable)
                    {
                        recursiveFunc(o, objectCollection);
                    }
                }

                return objectCollection;
            };

            recursiveFunc.Invoke(obj, collection);

            return collection;
        }
    }
}
