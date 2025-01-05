using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Scenes
{
    public static class SceneNavigationStackExtensions
    {
        public static UniTask PushSceneAsync(this INavigationStack navigationStack, int sceneBuildIndex, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(new ScenePage(sceneBuildIndex, false), new NavigationContext(), cancellationToken);
        }

        public static UniTask PushSceneAsync(this INavigationStack navigationStack, string sceneName, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(new ScenePage(sceneName, false), new NavigationContext(), cancellationToken);
        }

        public static UniTask PushSceneAsync(this INavigationStack navigationStack, int sceneBuildIndex, NavigationContext context, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(new ScenePage(sceneBuildIndex, false), context, cancellationToken);
        }

        public static UniTask PushSceneAsync(this INavigationStack navigationStack, string sceneName, NavigationContext context, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(new ScenePage(sceneName, false), context, cancellationToken);
        }
    }
}