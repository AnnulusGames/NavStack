using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    internal sealed class NavigationSheetCore
    {
        public IPage ActivePage => activePage;
        IPage activePage;

        public IReadOnlyCollection<IPage> Pages => pages;
        readonly List<IPage> pages = new();

        public IList<INavigationCallbackReceiver> CallbackReceivers => callbackReceivers;
        readonly List<INavigationCallbackReceiver> callbackReceivers = new();

        bool isRunning;

        public async UniTask RegisterAsync(IPage page, CancellationToken cancellationToken = default)
        {
            await NavigationHelper.InvokeOnInitialize(page, callbackReceivers, cancellationToken);
            pages.Add(page);
        }

        public async UniTask UnregisterAsync(IPage page, CancellationToken cancellationToken = default)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var remove = pages.Remove(page);
            if (!remove) throw new InvalidOperationException(); // TODO: add message

            await NavigationHelper.InvokeOnCleanup(page, callbackReceivers, cancellationToken);
        }

        public UniTask UnregisterAllAsync(CancellationToken cancellationToken = default)
        {
            var array = new UniTask[pages.Count];
            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                array[i] = NavigationHelper.InvokeOnCleanup(page, callbackReceivers, cancellationToken);
            }

            activePage = null;
            pages.Clear();

            return UniTask.WhenAll(array);
        }

        public async UniTask ShowAsync(int index, NavigationOptions options, CancellationToken cancellationToken = default)
        {
            if (isRunning)
            {
                switch (options.AwaitOperation)
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

                var task1 = activePage == null
                    ? UniTask.CompletedTask
                    : NavigationHelper.InvokeOnDisappear(activePage, callbackReceivers, options, cancellationToken);
                var task2 = NavigationHelper.InvokeOnAppear(page, callbackReceivers, options, cancellationToken);

                await UniTask.WhenAll(task1, task2);

                activePage = page;
            }
            finally
            {
                isRunning = false;
            }
        }

        public async UniTask HideAsync(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            if (activePage == null)
            {
                throw new InvalidOperationException(); // TODO: add message
            }

            if (isRunning)
            {
                switch (options.AwaitOperation)
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
                await NavigationHelper.InvokeOnDisappear(activePage, callbackReceivers, options, cancellationToken);
                activePage = null;
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}