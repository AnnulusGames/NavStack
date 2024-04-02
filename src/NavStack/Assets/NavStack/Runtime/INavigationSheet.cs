using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface INavigationSheet : INavigation
    {
        UniTask RegisterAsync(IPage page, CancellationToken cancellationToken = default);
        UniTask UnregisterAsync(IPage page, CancellationToken cancellationToken = default);
        UniTask UnregisterAllAsync(CancellationToken cancellationToken = default);
        UniTask ShowAsync(int index, NavigationOptions options, CancellationToken cancellationToken = default);
        UniTask HideAsync(NavigationOptions options, CancellationToken cancellationToken = default);
    }
}