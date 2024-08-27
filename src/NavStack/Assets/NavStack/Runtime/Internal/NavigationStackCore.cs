using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Internal
{
    public sealed class NavigationStackCore
    {
        readonly ConcurrentStack<IPage> pageStack = new();
        public IReadOnlyCollection<IPage> Pages => pageStack;

        public IPage ActivePage => activePage;
        IPage activePage;

        public event Action<IPage> OnPageAttached;
        public event Action<IPage> OnPageDetached;
        public event Action<(IPage Previous, IPage Current)> OnNavigating;
        public event Action<(IPage Previous, IPage Current)> OnNavigated;

        bool isRunning;

        public async UniTask PopAsync(INavigation navigation, NavigationContext context, CancellationToken cancellationToken = default)
        {
            var copiedContext = context with
            {
                Options = context.Options ?? navigation.DefaultOptions
            };

            if (isRunning)
            {
                switch (copiedContext.Options.AwaitOperation)
                {
                    case NavigationAwaitOperation.Error:
                        throw new InvalidOperationException("Navigation is currently in transition.");
                    case NavigationAwaitOperation.WaitForCompletion:
                        await UniTask.WaitWhile(() => isRunning, cancellationToken: cancellationToken);
                        break;
                    case NavigationAwaitOperation.Drop:
                        return;
                }
            }

            isRunning = true;

            try
            {
                if (pageStack.Count == 0) throw new InvalidOperationException("Empty stack");

                pageStack.TryPop(out var page);
                if (page is IPageStackEvent stackEvent)
                {
                    await stackEvent.OnPop(copiedContext, cancellationToken);
                }
                pageStack.TryPeek(out activePage);

                var task1 = page.OnNavigatedFrom(copiedContext, cancellationToken);
                var task2 = activePage == null ? UniTask.CompletedTask : activePage.OnNavigatedTo(copiedContext, cancellationToken);

                OnNavigating?.Invoke((page, activePage));

                await UniTask.WhenAll(task1, task2);

                OnNavigated?.Invoke((page, activePage));
                OnPageDetached?.Invoke(page);

                if (page is IPageLifecycleEvent pageLifecycle)
                {
                    await pageLifecycle.OnDetached(cancellationToken);
                }
            }
            finally
            {
                isRunning = false;
            }
        }

        public async UniTask PushAsync(INavigation navigation, Func<UniTask<IPage>> pageFactory, NavigationContext context, CancellationToken cancellationToken = default)
        {
            var copiedContext = context with
            {
                Options = context.Options ?? navigation.DefaultOptions
            };

            if (isRunning)
            {
                switch (copiedContext.Options.AwaitOperation)
                {
                    case NavigationAwaitOperation.Error:
                        throw new InvalidOperationException("Navigation is currently in transition.");
                    case NavigationAwaitOperation.WaitForCompletion:
                        await UniTask.WaitWhile(() => isRunning, cancellationToken: cancellationToken);
                        break;
                    case NavigationAwaitOperation.Drop:
                        return;
                }
            }

            isRunning = true;

            try
            {
                var page = await pageFactory();

                OnPageAttached?.Invoke(page);
                if (page is IPageLifecycleEvent pageLifecycle)
                {
                    await pageLifecycle.OnAttached(cancellationToken);
                }

                pageStack.Push(page);
                if (page is IPageStackEvent navigationStackEvent)
                {
                    await navigationStackEvent.OnPush(copiedContext, cancellationToken);
                }

                var prevPage = activePage;
                activePage = page;

                var task1 = prevPage == null ? UniTask.CompletedTask : prevPage.OnNavigatedFrom(copiedContext, cancellationToken);
                var task2 = activePage.OnNavigatedTo(copiedContext, cancellationToken);

                OnNavigating?.Invoke((prevPage, activePage));

                await UniTask.WhenAll(task1, task2);

                OnNavigated?.Invoke((prevPage, activePage));
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}