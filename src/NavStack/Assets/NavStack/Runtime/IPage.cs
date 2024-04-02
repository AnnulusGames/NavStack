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
        UniTask OnAppear(NavigationOptions options, CancellationToken cancellationToken = default);
        UniTask OnDisappear(NavigationOptions options, CancellationToken cancellationToken = default);
    }
}