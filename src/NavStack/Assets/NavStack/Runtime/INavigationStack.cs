using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface INavigationStack : INavigation
    {
        UniTask PushAsync(IPage page, NavigationOptions options, CancellationToken cancellationToken = default);
        UniTask PushAsync(Func<UniTask<IPage>> factory, NavigationOptions options, CancellationToken cancellationToken = default);
        UniTask PopAsync(NavigationOptions options, CancellationToken cancellationToken = default);
    }
}