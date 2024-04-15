using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface IPageLifecycleEvent
    {
        UniTask OnAttached(CancellationToken cancellationToken = default);
        UniTask OnDetached(CancellationToken cancellationToken = default);
    }
}