using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    internal enum PageCallbackType
    {
        OnBeforeInitialize,
        OnAfterInitialize,
        OnBeforeAppear,
        OnAfterAppear,
        OnBeforeDisappear,
        OnAfterDisappear,
        OnBeforeCleanup,
        OnAfterCleanup,
    }

    internal static class NavigationHelper
    {
        public static async UniTask InvokeOnAppear(IPage page, List<INavigationCallbackReceiver> callbackReceivers, NavigationContext context, CancellationToken cancellationToken)
        {
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnBeforeAppear);
            await page.OnAppear(context, cancellationToken);
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnAfterAppear);
        }

        public static async UniTask InvokeOnDisappear(IPage page, List<INavigationCallbackReceiver> callbackReceivers, NavigationContext context, CancellationToken cancellationToken)
        {
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnBeforeDisappear);
            await page.OnDisappear(context, cancellationToken);
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnAfterDisappear);
        }

        public static async UniTask InvokeOnInitialize(IPage page, List<INavigationCallbackReceiver> callbackReceivers, CancellationToken cancellationToken)
        {
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnBeforeInitialize);
            await page.OnInitialize(cancellationToken);
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnAfterInitialize);
        }

        public static async UniTask InvokeOnCleanup(IPage page, List<INavigationCallbackReceiver> callbackReceivers, CancellationToken cancellationToken)
        {
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnBeforeCleanup);
            await page.OnCleanup(cancellationToken);
            InvokeCallback(page, callbackReceivers, PageCallbackType.OnAfterCleanup);
        }

        public static void InvokeCallback(IPage page, List<INavigationCallbackReceiver> receivers, PageCallbackType callbackType)
        {
            var array = ArrayPool<INavigationCallbackReceiver>.Shared.Rent(receivers.Count);
            try
            {
                receivers.CopyTo(array);
                var span = array.AsSpan(0, receivers.Count);
                for (int i = 0; i < span.Length; i++)
                {
                    switch (callbackType)
                    {
                        case PageCallbackType.OnBeforeInitialize:
                            span[i].OnBeforeInitialize(page);
                            break;
                        case PageCallbackType.OnAfterInitialize:
                            span[i].OnAfterInitialize(page);
                            break;
                        case PageCallbackType.OnBeforeAppear:
                            span[i].OnBeforeAppear(page);
                            break;
                        case PageCallbackType.OnAfterAppear:
                            span[i].OnAfterAppear(page);
                            break;
                        case PageCallbackType.OnBeforeDisappear:
                            span[i].OnBeforeDisappear(page);
                            break;
                        case PageCallbackType.OnAfterDisappear:
                            span[i].OnAfterDisappear(page);
                            break;
                        case PageCallbackType.OnBeforeCleanup:
                            span[i].OnBeforeCleanup(page);
                            break;
                        case PageCallbackType.OnAfterCleanup:
                            span[i].OnAfterCleanup(page);
                            break;
                    }
                }
            }
            finally
            {
                ArrayPool<INavigationCallbackReceiver>.Shared.Return(array, true);
            }
        }
    }
}