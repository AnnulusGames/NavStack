using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Scenes
{
    public static class SceneNavigationExtensions
    {
        public static UniTask PushSceneAsync(this INavigationStack navigationStack, int sceneBuildIndex, bool setActiveScene = true, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(new ScenePage(sceneBuildIndex)
            {
                SetActiveOnNavigated = setActiveScene,
            }, cancellationToken);
        }

        public static UniTask PushSceneAsync(this INavigationStack navigationStack, string sceneName, bool setActiveScene = true, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(new ScenePage(sceneName)
            {
                SetActiveOnNavigated = setActiveScene,
            }, cancellationToken);
        }

        public static UniTask AddSceneAsync(this INavigationSheet navigationSheet, int sceneBuildIndex, bool setActiveScene = true, CancellationToken cancellationToken = default)
        {
            return navigationSheet.AddAsync(new ScenePage(sceneBuildIndex)
            {
                SetActiveOnNavigated = setActiveScene
            }, cancellationToken);
        }

        public static UniTask AddSceneAsync(this INavigationSheet navigationSheet, string sceneName, bool setActiveScene = true, CancellationToken cancellationToken = default)
        {
            return navigationSheet.AddAsync(new ScenePage(sceneName)
            {
                SetActiveOnNavigated = setActiveScene
            }, cancellationToken);
        }
    }
}