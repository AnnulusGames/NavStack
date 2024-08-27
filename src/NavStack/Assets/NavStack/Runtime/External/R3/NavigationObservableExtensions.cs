#if NAVSTACK_R3_SUPPORT
using System.Threading;
using R3;

namespace NavStack
{
    public static class NavigationObservableExtensions
    {
        public static Observable<IPage> OnPageAttachedAsObservable(this INavigation navigation, CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<IPage>(
                h => navigation.OnPageAttached += h,
                h => navigation.OnPageAttached -= h,
                cancellationToken
            );
        }

        public static Observable<IPage> OnPageDetachedAsObservable(this INavigation navigation, CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<IPage>(
                h => navigation.OnPageDetached += h,
                h => navigation.OnPageDetached -= h,
                cancellationToken
            );
        }

        public static Observable<(IPage Previous, IPage Current)> OnNavigatingAsObservable(this INavigation navigation, CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<(IPage, IPage)>(
                h => navigation.OnNavigating += h,
                h => navigation.OnNavigating -= h,
                cancellationToken
            );
        }

        public static Observable<(IPage Previous, IPage Current)> OnNavigatedAsObservable(this INavigation navigation, CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<(IPage, IPage)>(
                h => navigation.OnNavigated += h,
                h => navigation.OnNavigated -= h,
                cancellationToken
            );
        }
    }
}
#endif