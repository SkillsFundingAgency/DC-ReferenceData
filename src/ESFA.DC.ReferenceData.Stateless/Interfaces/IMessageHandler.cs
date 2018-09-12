using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.Stateless.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task<bool> Handle(T message, CancellationToken cancellationToken);
    }
}
