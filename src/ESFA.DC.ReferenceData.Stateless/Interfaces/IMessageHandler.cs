using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.Stateless.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task<bool> HandleAsync(T message, CancellationToken cancellationToken);
    }
}
