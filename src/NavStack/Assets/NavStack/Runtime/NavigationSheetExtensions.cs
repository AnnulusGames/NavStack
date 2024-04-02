using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack.Content;

namespace NavStack
{
    public static class NavigationSheetExtensions
    {
        public static UniTask ShowAsync(this INavigationSheet navigationSheet, int index, CancellationToken cancellationToken = default)
        {
            return navigationSheet.ShowAsync(index, navigationSheet.DefaultOptions, cancellationToken);
        }

        public static UniTask RegisterNewObjectAsync<T>(this INavigationSheet navigationSheet, T prefab, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object, IPage
        {
            var instance = UnityEngine.Object.Instantiate(prefab);
            if (instance is Component component)
            {
                instance.LifecycleEvents.Add(new DestroyObjectEvent(component.gameObject));
            }

            return navigationSheet.RegisterAsync(instance, cancellationToken);
        }

        public static UniTask RegisterNewObjectAsync(this INavigationSheet navigationSheet, string key, CancellationToken cancellationToken = default)
        {
            return RegisterNewObjectAsync(navigationSheet, key, ResourceProvider.DefaultResourceProvider, cancellationToken);
        }

        public static UniTask RegisterNewObjectAsync(this INavigationSheet navigationSheet, string key, NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return RegisterNewObjectAsync(navigationSheet, key, ResourceProvider.DefaultResourceProvider, options, cancellationToken);
        }

        public static UniTask RegisterNewObjectAsync(this INavigationSheet navigationSheet, string key, IResourceProvider resourceProvider, CancellationToken cancellationToken = default)
        {
            return RegisterNewObjectAsync(navigationSheet, key, resourceProvider, navigationSheet.DefaultOptions, cancellationToken);
        }

        public static async UniTask RegisterNewObjectAsync(this INavigationSheet navigationSheet, string key, IResourceProvider resourceProvider, NavigationOptions options, CancellationToken cancellationToken = default)
        {
            var resource = await resourceProvider.LoadAsync<UnityEngine.Object>(key, cancellationToken);

            var instance = UnityEngine.Object.Instantiate(resource);
            if (!TryGetComponent<IPage>(instance, out var page)) throw new Exception(); // TODO:

            page.LifecycleEvents.Add(new ResourceUnloadEvent(page, resource, instance, resourceProvider));

            await navigationSheet.RegisterAsync(page, cancellationToken);
        }

        public static UniTask HideAsync(this INavigationSheet navigation, CancellationToken cancellationToken = default)
        {
            return navigation.HideAsync(navigation.DefaultOptions, cancellationToken);
        }

        static bool TryGetComponent<T>(UnityEngine.Object obj, out T result)
        {
            if (obj is GameObject gameObject)
            {
                result = gameObject.GetComponent<T>();
                return true;
            }

            if (obj is Component component)
            {
                result = component.GetComponent<T>();
                return true;
            }

            result = default;
            return false;
        }
    }
}