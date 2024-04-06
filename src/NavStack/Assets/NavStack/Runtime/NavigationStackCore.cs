using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    internal sealed class NavigationStackCore
    {
        readonly ConcurrentStack<IPage> pageStack = new();
        public IReadOnlyCollection<IPage> Pages => pageStack;

        public IPage ActivePage => activePage;
        IPage activePage;

        readonly List<INavigationCallbackReceiver> callbackReceivers = new();
        public IList<INavigationCallbackReceiver> CallbackReceivers => callbackReceivers;

        bool isRunning;

        public async UniTask PopAsync(NavigationContext context, CancellationToken cancellationToken = default)
        {
            if (isRunning)
            {
                switch (context.Options.AwaitOperation)
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
                pageStack.TryPeek(out activePage);

                var task1 = NavigationHelper.InvokeOnDisappear(page, callbackReceivers, context, cancellationToken);
                var task2 = activePage == null
                    ? UniTask.CompletedTask
                    : NavigationHelper.InvokeOnAppear(activePage, callbackReceivers, context, cancellationToken);

                await UniTask.WhenAll(task1, task2);

                await NavigationHelper.InvokeOnCleanup(page, callbackReceivers, cancellationToken);
            }
            finally
            {
                isRunning = false;
            }
        }

        public async UniTask PushAsync(Func<UniTask<IPage>> pageFactory, NavigationContext context, CancellationToken cancellationToken = default)
        {
            if (isRunning)
            {
                switch (context.Options.AwaitOperation)
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
                await NavigationHelper.InvokeOnInitialize(page, callbackReceivers, cancellationToken);

                var task1 = activePage == null
                    ? UniTask.CompletedTask
                    : NavigationHelper.InvokeOnDisappear(activePage, callbackReceivers, context, cancellationToken);
                var task2 = NavigationHelper.InvokeOnAppear(page, callbackReceivers, context, cancellationToken);

                await UniTask.WhenAll(task1, task2);

                pageStack.Push(page);
                activePage = page;
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}