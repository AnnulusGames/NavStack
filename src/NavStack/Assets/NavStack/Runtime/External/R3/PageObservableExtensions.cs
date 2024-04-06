#if NAVSTACK_R3_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace NavStack
{
    public static class PageObservableExtensions
    {
        public static Observable<NavigationContext> OnAppearAsObservable(this IPage page)
        {
            var e = new OnAppearObservable();
            page.LifecycleEvents.Add(e);
            return e.AsObservable();
        }

        public static Observable<NavigationContext> OnDisappearAsObservable(this IPage page)
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
            readonly Subject<NavigationContext> subject = new();
            public Observable<NavigationContext> AsObservable() => subject;

            UniTask IPageLifecycleEvent.OnAppear(NavigationContext context, CancellationToken cancellationToken)
            {
                subject.OnNext(context);
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
            readonly Subject<NavigationContext> subject = new();
            public Observable<NavigationContext> AsObservable() => subject;

            UniTask IPageLifecycleEvent.OnDisappear(NavigationContext context, CancellationToken cancellationToken)
            {
                subject.OnNext(context);
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