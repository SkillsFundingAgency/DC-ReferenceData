using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ReferenceData.ULN.Model
{
    public partial class UlnContext : IUlnContext
    {
        public string ConnectionString => this.Database.GetDbConnection().ConnectionString;
    }
}
