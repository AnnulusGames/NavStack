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
                navigation.CallbackReceivers.Add(new InjectCallbackReceiver() { Resolver = resolver });
            });
        }

        sealed class InjectCallbackReceiver : INavigationCallbackReceiver
        {
            public IObjectResolver Resolver { get; set; }

            public void OnBeforeInitialize(IPage page)
            {
                Resolver.Inject(page);
            }
        }
    }
}
#endif