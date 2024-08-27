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
            return navigationSheet.ShowAsync(index, new NavigationContext(), cancellationToken);
        }

        public static UniTask AddNewObjectAsync<T>(this INavigationSheet navigationSheet, T prefab, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object, IPage
        {
            var instance = UnityEngine.Object.Instantiate(prefab);
            if (instance is Component component)
            {
                component.GetCancellationTokenOnDestroy().RegisterWithoutCaptureExecutionContext(instance =>
                {
                    if (instance != null) UnityEngine.Object.Destroy((UnityEngine.Object)instance);
                }, instance);
            }

            return navigationSheet.AddAsync(instance, cancellationToken);
        }

        public static async UniTask AddNewObjectAsync(this INavigationSheet navigationSheet, string key, IResourceProvider resourceProvider = null, bool loadAsync = true, CancellationToken cancellationToken = default)
        {
            resourceProvider ??= ResourceProvider.DefaultResourceProvider;

            UnityEngine.Object resource;

            if (loadAsync)
            {
                resource = await resourceProvider.LoadAsync<UnityEngine.Object>(key, cancellationToken);
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                resource = resourceProvider.Load<UnityEngine.Object>(key);
            }

            var instance = UnityEngine.Object.Instantiate(resource);
            if (!TryGetComponent<IPage>(instance, out var page)) throw new Exception(); // TODO:

            void OnPageDetached(IPage pageDetached)
            {
                if (pageDetached != page) return;
                if (instance != null) UnityEngine.Object.Destroy(instance);
                resourceProvider.UnloadAsync(resource).Forget();
                navigationSheet.OnPageDetached -= OnPageDetached;
            }

            navigationSheet.OnPageDetached += OnPageDetached;

            await navigationSheet.AddAsync(page, cancellationToken);
        }

        public static UniTask HideAsync(this INavigationSheet navigationSheet, CancellationToken cancellationToken = default)
        {
            return navigationSheet.HideAsync(new NavigationContext(), cancellationToken);
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