#if NAVSTACK_R3_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace NavStack
{
    public static class PageObservableExtensions
    {
        public static Observable<NavigationOptions> OnAppearAsObservable(this IPage page)
        {
            var e = new OnAppearObservable();
            page.LifecycleEvents.Add(e);
            return e.AsObservable();
        }

        public static Observable<NavigationOptions> OnDisappearAsObservable(this IPage page)
        {
            var e = new OnDisappearObservable();
            page.LifecycleEvents.Add(e);
            return e.AsObservable();
        }

        public static Observable<Unit> OnCleanupAsObservable(this IPage page)
        {
            var e = new OnCleanupObservable();
            page.LifecycleEvents.Add(e);
            return e.AsObservable();
        }

        sealed class OnAppearObservable : IPageLifecycleEvent
        {
            readonly Subject<NavigationOptions> subject = new();
            public Observable<NavigationOptions> AsObservable() => subject;

            UniTask IPageLifecycleEvent.OnAppear(NavigationOptions options, CancellationToken cancellationToken)
            {
                subject.OnNext(options);
                return UniTask.CompletedTask;
            }

            UniTask IPageLifecycleEvent.OnCleanup(CancellationToken cancellationToken)
            {
                subject.Dispose();
                return UniTask.CompletedTask;
            }
        }

        sealed class OnDisappearObservable : IPageLifecycleEvent
        {
            readonly Subject<NavigationOptions> subject = new();
            public Observable<NavigationOptions> AsObservable() => subject;

            UniTask IPageLifecycleEvent.OnDisappear(NavigationOptions options, CancellationToken cancellationToken)
            {
                subject.OnNext(options);
                return UniTask.CompletedTask;
            }

            UniTask IPageLifecycleEvent.OnCleanup(CancellationToken cancellationToken)
            {
                subject.Dispose();
                return UniTask.CompletedTask;
            }
        }

        sealed class OnCleanupObservable : IPageLifecycleEvent
        {
            readonly Subject<Unit> subject = new();
            public Observable<Unit> AsObservable() => subject;

            UniTask IPageLifecycleEvent.OnCleanup(CancellationToken cancellationToken)
            {
                subject.OnNext(Unit.Default);
                subject.Dispose();
                return UniTask.CompletedTask;
            }
        }
    }
}
#endif