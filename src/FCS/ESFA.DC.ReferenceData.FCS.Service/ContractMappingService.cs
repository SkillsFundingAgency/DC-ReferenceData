using System;
using System.Collections.Generic;
using ESFA.DC.ReferenceData.FCS.Model;
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

        public ICollection<contractType> FlattenContracts(contractType contract)
        {
            return Flatten(contract, c => c.contracts);
        }

        private ICollection<T> Flatten<T>(T obj, Func<T, IEnumerable<T>> selector)
        {
            var collection = new List<T>();

            Func<T, ICollection<T>, ICollection<T>> recursiveFunc = null;

            recursiveFunc = (con, objectCollection) =>
            {
                objectCollection.Add(con);

                var selectedEnumerable = selector(obj);

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
