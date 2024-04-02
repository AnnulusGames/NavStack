using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface INavigationSheet : INavigation
    {
        UniTask AddAsync(IPage page, CancellationToken cancellationToken = default);
        UniTask RemoveAsync(IPage page, CancellationToken cancellationToken = default);
        UniTask RemoveAllAsync(CancellationToken cancellationToken = default);
        UniTask ShowAsync(int index, NavigationOptions options, CancellationToken cancellationToken = default);
        UniTask HideAsync(NavigationOptions options, CancellationToken cancellationToken = default);
    }
}