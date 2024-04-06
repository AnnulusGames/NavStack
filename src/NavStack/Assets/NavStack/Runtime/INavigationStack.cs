using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface INavigationStack : INavigation
    {
        UniTask PushAsync(IPage page, NavigationContext context, CancellationToken cancellationToken = default);
        UniTask PushAsync(Func<UniTask<IPage>> factory, NavigationContext context, CancellationToken cancellationToken = default);
        UniTask PopAsync(NavigationContext context, CancellationToken cancellationToken = default);
    }
}