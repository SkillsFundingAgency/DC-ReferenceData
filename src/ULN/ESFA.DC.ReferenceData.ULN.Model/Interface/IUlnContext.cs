using System;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ReferenceData.ULN.Model.Interface
{
    public interface IUlnContext : IDisposable
    {
        DbSet<Import> Imports { get; set; }
        DbSet<UniqueLearnerNumber> UniqueLearnerNumbers { get; set; }
    }
}
