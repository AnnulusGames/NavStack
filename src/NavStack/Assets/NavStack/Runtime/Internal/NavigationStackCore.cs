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

        bool isRunning;

        public async UniTask PopAsync(INavigation navigation, NavigationContext context, CancellationToken cancellationToken = default)
        {
            var copiedContext = context.CreateCopy();
            copiedContext.Options = context.Options ?? navigation.DefaultOptions;

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
                if (page is INavigationStackEvent navigationStackEvent)
                {
                    await navigationStackEvent.OnPop(copiedContext, cancellationToken);
                }
                pageStack.TryPeek(out activePage);

                var task1 = UniTask.CompletedTask;
                if (page is INavigationAware navigationAware1)
                {
                    task1 = navigationAware1.OnNavigatedTo(copiedContext, cancellationToken);
                }

                var task2 = UniTask.CompletedTask;
                if (activePage is INavigationAware navigationAware2)
                {
                    task2 = navigationAware2.OnNavigatedFrom(copiedContext, cancellationToken);
                }
                
                await UniTask.WhenAll(task1, task2);

                OnPageDetached?.Invoke(page);
                if (page is IPageLifecycle pageLifecycle)
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
            var copiedContext = context.CreateCopy();
            copiedContext.Options = context.Options ?? navigation.DefaultOptions;
            
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
                if (page is IPageLifecycle pageLifecycle)
                {
                    await pageLifecycle.OnAttached(cancellationToken);
                }

                pageStack.Push(page);
                if (page is INavigationStackEvent navigationStackEvent)
                {
                    await navigationStackEvent.OnPush(copiedContext, cancellationToken);
                }
                activePage = page;

                var task1 = UniTask.CompletedTask;
                if (activePage is INavigationAware navigationAware1)
                {
                    task1 = navigationAware1.OnNavigatedTo(copiedContext, cancellationToken);
                }

                var task2 = UniTask.CompletedTask;
                if (page is INavigationAware navigationAware2)
                {
                    task2 = navigationAware2.OnNavigatedFrom(copiedContext, cancellationToken);
                }

                await UniTask.WhenAll(task1, task2);

            }
            finally
            {
                isRunning = false;
            }
        }
    }
}