using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class ContractMappingService : IContractMappingService
    {
        public Contractor Map(contract contract)
        {
            return MapContractor(contract.contractor);
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

        public ICollection<contractType> FlattenContracts(contractType contract)
        {
            var contractCollection = new List<contractType>();

            Func<contractType, ICollection<contractType>, ICollection<contractType>> recursiveFunction = null;

            recursiveFunction = (con, contracts) =>
            {

                contracts.Add(con);

                if (contract.contracts != null && contract.contracts.Any())
                {
                    foreach (var c in contract.contracts)
                    {
                        recursiveFunction(c, contracts);
                    }
                }

                return contracts;
            };

            return recursiveFunction.Invoke(contract, contractCollection);
        }
    }
}
