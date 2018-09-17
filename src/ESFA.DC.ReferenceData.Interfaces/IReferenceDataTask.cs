using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.Interfaces
{
    public interface IReferenceDataTask
    {
        string TaskName { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
