using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface IPageLifecycleEvent
    {
        UniTask OnInitialize(CancellationToken cancellationToken = default) => UniTask.CompletedTask;
        UniTask OnCleanup(CancellationToken cancellationToken = default) => UniTask.CompletedTask;
        UniTask OnAppear(NavigationOptions options, CancellationToken cancellationToken = default) => UniTask.CompletedTask;
        UniTask OnDisappear(NavigationOptions options, CancellationToken cancellationToken = default) => UniTask.CompletedTask;
    }
}