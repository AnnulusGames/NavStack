#if NAVSTACK_R3_SUPPORT
using System;
using R3;

namespace NavStack
{
    public static class NavigationObservableExtensions
    {
        public static Observable<IPage> OnBeforeInitializeAsObservable(this INavigation navigation)
        {
            var e = new OnBeforeInitializeEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnBeforeInitializeEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnBeforeInitialize(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }

        public static Observable<IPage> OnAfterInitializeAsObservable(this INavigation navigation)
        {
            var e = new OnAfterInitializeEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnAfterInitializeEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnAfterInitialize(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }

        public static Observable<IPage> OnBeforeCleanupAsObservable(this INavigation navigation)
        {
            var e = new OnBeforeCleanupEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnBeforeCleanupEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnBeforeCleanup(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }

        public static Observable<IPage> OnAfterCleanupAsObservable(this INavigation navigation)
        {
            var e = new OnAfterCleanupEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnAfterCleanupEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnBeforeCleanup(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }

        public static Observable<IPage> OnBeforeAppearAsObservable(this INavigation navigation)
        {
            var e = new OnBeforeAppearEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnBeforeAppearEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnBeforeAppear(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }

        public static Observable<IPage> OnAfterAppearAsObservable(this INavigation navigation)
        {
            var e = new OnAfterAppearEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnAfterAppearEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnAfterAppear(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }

        public static Observable<IPage> OnBeforeDisappearAsObservable(this INavigation navigation)
        {
            var e = new OnBeforeDisappearEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnBeforeDisappearEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnBeforeDisappear(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }

        public static Observable<IPage> OnAfterDisappearAsObservable(this INavigation navigation)
        {
            var e = new OnAfterDisappearEvent();
            navigation.CallbackReceivers.Add(e);
            return e.AsObservable();
        }

        sealed class OnAfterDisappearEvent : INavigationCallbackReceiver, IDisposable
        {
            readonly Subject<IPage> subject = new();

            public void Dispose() => subject.Dispose();
            void INavigationCallbackReceiver.OnAfterDisappear(IPage page) => subject.OnNext(page);
            public Observable<IPage> AsObservable() => subject;
        }
    }
}
#endif