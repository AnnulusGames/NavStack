using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace NavStack.Scenes
{
    public class ScenePage : IPage
    {
        public ScenePage(int buildIndex)
        {
            scene = SceneManager.GetSceneByBuildIndex(buildIndex);
        }

        public ScenePage(string sceneName)
        {
            scene = SceneManager.GetSceneByName(sceneName);
        }

        public ScenePage(Scene scene)
        {
            this.scene = scene;
        }

        Scene scene;
        public Scene Scene => scene;

        public bool SetActiveOnNavigated { get; init; } = true;

        public async UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(scene.buildIndex);
            await asyncOperation.ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
            await asyncOperation.ToUniTask(cancellationToken: cancellationToken);
            
            if (SetActiveOnNavigated)
            {
                SceneManager.SetActiveScene(scene);
            }
        }
    }
}