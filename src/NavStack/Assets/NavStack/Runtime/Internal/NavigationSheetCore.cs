using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Internal
{
    public sealed class NavigationSheetCore
    {
        public IPage ActivePage => activePage;
        IPage activePage;

        public IReadOnlyCollection<IPage> Pages => pages;
        readonly List<IPage> pages = new();

        public event Action<IPage> OnPageAttached;
        public event Action<IPage> OnPageDetached;

        bool isRunning;

        public async UniTask AddAsync(IPage page, CancellationToken cancellationToken = default)
        {
            OnPageAttached?.Invoke(page);
            if (page is IPageLifecycleEvent lifecycle)
            {
                await lifecycle.OnAttached(cancellationToken);
            }

            pages.Add(page);
        }

        public async UniTask RemoveAsync(IPage page, CancellationToken cancellationToken = default)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var remove = pages.Remove(page);
            if (!remove) throw new InvalidOperationException(); // TODO: add message

            OnPageDetached?.Invoke(page);
            if (page is IPageLifecycleEvent lifecycle)
            {
                await lifecycle.OnDetached(cancellationToken);
            }
        }

        public UniTask RemoveAllAsync(CancellationToken cancellationToken = default)
        {
            var array = new UniTask[pages.Count];
            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                OnPageDetached?.Invoke(page); // TODO: fix callback timing
                array[i] = page is IPageLifecycleEvent lifecycle ? lifecycle.OnDetached(cancellationToken) : UniTask.CompletedTask;
            }

            activePage = null;
            pages.Clear();

            return UniTask.WhenAll(array);
        }

        public async UniTask ShowAsync(INavigation navigation, int index, NavigationContext context, CancellationToken cancellationToken = default)
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
                var page = pages[index];
                if (activePage == page) return;

                var task1 = activePage == null ? UniTask.CompletedTask : activePage.OnNavigatedFrom(copiedContext, cancellationToken);
                var task2 = page.OnNavigatedTo(copiedContext, cancellationToken);

                await UniTask.WhenAll(task1, task2);

                activePage = page;
            }
            finally
            {
                isRunning = false;
            }
        }

        public async UniTask HideAsync(INavigation navigation, NavigationContext context, CancellationToken cancellationToken = default)
        {
            var copiedContext = context.CreateCopy();
            copiedContext.Options = context.Options ?? navigation.DefaultOptions;

            if (activePage == null)
            {
                throw new InvalidOperationException(); // TODO: add message
            }

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
                if (activePage != null)
                {
                    await activePage.OnNavigatedFrom(copiedContext, cancellationToken);
                }

                activePage = null;
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}