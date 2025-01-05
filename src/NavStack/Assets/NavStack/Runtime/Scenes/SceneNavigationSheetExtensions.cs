using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Scenes
{
    public static class SceneNavigationSheetExtensions
    {
        public static UniTask AddSceneAsync(this INavigationSheet navigationSheet, int sceneBuildIndex, CancellationToken cancellationToken = default)
        {
            return navigationSheet.AddAsync(new ScenePage(sceneBuildIndex), cancellationToken);
        }

        public static UniTask AddSceneAsync(this INavigationSheet navigationSheet, string sceneName, CancellationToken cancellationToken = default)
        {
            return navigationSheet.AddAsync(new ScenePage(sceneName), cancellationToken);
        }
    }
}