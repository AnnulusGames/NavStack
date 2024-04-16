using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface IPageStackEvent
    {
        UniTask OnPush(NavigationContext context, CancellationToken cancellationToken = default);
        UniTask OnPop(NavigationContext context, CancellationToken cancellationToken = default);
    }
}