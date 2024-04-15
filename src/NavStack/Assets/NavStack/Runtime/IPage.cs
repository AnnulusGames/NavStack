using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface IPage
    {
        UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default);
        UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default);
    }
}