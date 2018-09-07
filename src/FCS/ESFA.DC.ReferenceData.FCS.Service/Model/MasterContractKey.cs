namespace ESFA.DC.ReferenceData.FCS.Service.Model
{
    public struct MasterContractKey
    {
        public MasterContractKey(string contractNumber, int contractVersionNumber)
        {
            ContractNumber = contractNumber;
            ContractVersionNumber = contractVersionNumber;
        }

        public string ContractNumber { get; set; }

        public int ContractVersionNumber { get; set; }

    }
}
