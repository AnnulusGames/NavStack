using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface IPage
    {
        IList<IPageLifecycleEvent> LifecycleEvents { get; }
        UniTask OnInitialize(CancellationToken cancellationToken = default);
        UniTask OnCleanup(CancellationToken cancellationToken = default);
        UniTask OnAppear(NavigationContext context, CancellationToken cancellationToken = default);
        UniTask OnDisappear(NavigationContext context, CancellationToken cancellationToken = default);
    }
}