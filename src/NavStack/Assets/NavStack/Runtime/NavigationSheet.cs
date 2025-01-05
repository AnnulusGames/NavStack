using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NavStack.Internal;

namespace NavStack
{
    public class NavigationSheet : INavigationSheet
    {
        readonly NavigationSheetCore core = new();

        public IPage ActivePage => core.ActivePage;
        public IReadOnlyCollection<IPage> Pages => core.Pages;

        public event Action<IPage> OnPageAttached
        {
            add => core.OnPageAttached += value;
            remove => core.OnPageAttached -= value;
        }

        public event Action<IPage> OnPageDetached
        {
            add => core.OnPageDetached += value;
            remove => core.OnPageDetached -= value;
        }

        public event Action<(IPage Previous, IPage Current)> OnNavigating
        {
            add => core.OnNavigating += value;
            remove => core.OnNavigating -= value;
        }

        public event Action<(IPage Previous, IPage Current)> OnNavigated
        {
            add => core.OnNavigated += value;
            remove => core.OnNavigated -= value;
        }

        public UniTask AddAsync(IPage page, CancellationToken cancellationToken = default)
        {
            return core.AddAsync(page, cancellationToken);
        }

        public UniTask HideAsync(NavigationContext context, CancellationToken cancellationToken = default)
        {
            return core.HideAsync(context, cancellationToken);
        }

        public UniTask RemoveAllAsync(CancellationToken cancellationToken = default)
        {
            return core.RemoveAllAsync(cancellationToken);
        }

        public UniTask RemoveAsync(IPage page, CancellationToken cancellationToken = default)
        {
            return RemoveAsync(page, cancellationToken);
        }

        public UniTask ShowAsync(int index, NavigationContext context, CancellationToken cancellationToken = default)
        {
            return ShowAsync(index, context, cancellationToken);
        }
    }
}