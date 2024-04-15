using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface IPageLifecycle
    {
        UniTask OnAttached(CancellationToken cancellationToken = default);
        UniTask OnDetached(CancellationToken cancellationToken = default);
    }
}