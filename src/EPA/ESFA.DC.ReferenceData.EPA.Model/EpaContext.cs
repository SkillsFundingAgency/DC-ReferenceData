using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model.Config;
using ESFA.DC.ReferenceData.EPA.Model.Interface;

namespace ESFA.DC.ReferenceData.EPA.Model
{
    public class EpaContext : DbContext, IEpaContext
    {
        public EpaContext()
            : base("name=EPA")
        {
        }

        public EpaContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Organisation> Organisations { get; set; }

        public virtual DbSet<Standard> Standards { get; set; }

        public virtual DbSet<Period> Periods { get; set; }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrganisationConfig());
            modelBuilder.Configurations.Add(new StandardConfig());
            modelBuilder.Configurations.Add(new PeriodConfig());
        }
    }
}
