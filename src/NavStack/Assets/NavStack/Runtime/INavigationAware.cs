using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface INavigationAware
    {
        UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default);
        UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default);
    }
}