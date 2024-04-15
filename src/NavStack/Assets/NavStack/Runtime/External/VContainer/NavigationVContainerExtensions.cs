#if NAVSTACK_VCONTAINER_SUPPORT
using VContainer;

namespace NavStack
{
    public static class NavigationVContainerExtensions
    {
        public static void RegisterNavigation<T>(this IContainerBuilder builder, T navigation)
            where T : INavigation
        {
            builder.RegisterInstance(navigation);
            builder.RegisterBuildCallback(resolver =>
            {
                navigation.OnPageAttached += page =>
                {
                    resolver.Inject(page);
                };
            });
        }
    }
}
#endif