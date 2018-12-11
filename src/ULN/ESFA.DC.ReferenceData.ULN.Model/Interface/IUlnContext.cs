using System;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ReferenceData.ULN.Model.Interface
{
    public interface IUlnContext : IDisposable
    {
        DbSet<Import> Import { get; set; }
        DbSet<UniqueLearnerNumber> UniqueLearnerNumber { get; set; }

        string ConnectionString { get; }
    }
}
