﻿using System.Data.Entity;
using ESFA.DC.ReferenceData.FCS.Model.Config;
using ESFA.DC.ReferenceData.FCS.Model.Interface;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class FcsContext : DbContext, IFcsContext
    {
        public FcsContext()
            : base("name=FCS")
        {
        }

        public FcsContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContractorConfig());
            modelBuilder.Configurations.Add(new ContractConfig());
            modelBuilder.Configurations.Add(new ContractAllocationConfig());
            modelBuilder.Configurations.Add(new ContractDeliverableConfig());
        }

        public virtual DbSet<Contractor> Contractors { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }

        public virtual DbSet<ContractAllocation> ContractAllocations { get; set; }

        public virtual DbSet<ContractDeliverable> ContractDeliverables { get; set; }
    }
}
