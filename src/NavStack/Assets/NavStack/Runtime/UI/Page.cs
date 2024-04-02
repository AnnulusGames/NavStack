#if NAVSTACK_UGUI_SUPPORT
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Page")]
    [RequireComponent(typeof(RectTransform))]
    public class Page : MonoBehaviour, IPage
    {
        readonly List<IPageLifecycleEvent> events = new();
        public IList<IPageLifecycleEvent> LifecycleEvents => events;

        public async UniTask OnInitialize(CancellationToken cancellationToken = default)
        {
            var array = ArrayPool<IPageLifecycleEvent>.Shared.Rent(events.Count);
            try
            {
                events.CopyTo(array);
                for (int i = 0; i < events.Count; i++)
                {
                    await array[i].OnInitialize(cancellationToken);
                }
            }
            finally
            {
                ArrayPool<IPageLifecycleEvent>.Shared.Return(array);
            }

            await OnInitializeCore(cancellationToken);
        }

        public async UniTask OnCleanup(CancellationToken cancellationToken = default)
        {
            var array = ArrayPool<IPageLifecycleEvent>.Shared.Rent(events.Count);
            try
            {
                events.CopyTo(array);
                for (int i = 0; i < events.Count; i++)
                {
                    await array[i].OnCleanup(cancellationToken);
                }
            }
            finally
            {
                ArrayPool<IPageLifecycleEvent>.Shared.Return(array);
            }

            await OnCleanupCore(cancellationToken);
        }

        public async UniTask OnDisappear(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            var array = ArrayPool<IPageLifecycleEvent>.Shared.Rent(events.Count);
            try
            {
                events.CopyTo(array);
                for (int i = 0; i < events.Count; i++)
                {
                    await array[i].OnDisappear(options, cancellationToken);
                }
            }
            finally
            {
                ArrayPool<IPageLifecycleEvent>.Shared.Return(array);
            }

            await OnDisappearCore(options, cancellationToken);
        }

        public async UniTask OnAppear(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            var array = ArrayPool<IPageLifecycleEvent>.Shared.Rent(events.Count);
            try
            {
                events.CopyTo(array);
                for (int i = 0; i < events.Count; i++)
                {
                    await array[i].OnAppear(options, cancellationToken);
                }
            }
            finally
            {
                ArrayPool<IPageLifecycleEvent>.Shared.Return(array);
            }

            await OnAppearCore(options, cancellationToken);
        }

        protected virtual UniTask OnInitializeCore(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnCleanupCore(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnAppearCore(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnDisappearCore(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
    }
}
#endif