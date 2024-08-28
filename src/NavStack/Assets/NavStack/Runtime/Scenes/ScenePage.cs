using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
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

        public bool SetActiveOnNavigated { get; set; } = true;
        public bool LoadAsync { get; set; } = true;
        
        public bool AllowSceneActivation
        {
            get
            {
                return allowSceneActivation;
            }
            set
            {
                allowSceneActivation = value;
                if (asyncOperation != null) asyncOperation.allowSceneActivation = value;
            }
        }

        public Scene Scene => scene;
        public float Progress => asyncOperation == null ? 0f : asyncOperation.progress;

        Scene scene;
        bool allowSceneActivation;
        AsyncOperation asyncOperation;

        public async UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default)
        {
            Assert.IsNull(asyncOperation);

            asyncOperation = SceneManager.UnloadSceneAsync(scene.buildIndex);
            asyncOperation.allowSceneActivation = allowSceneActivation;
            await asyncOperation.ToUniTask(cancellationToken: cancellationToken);

            asyncOperation = null;
        }

        public async UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default)
        {
            Assert.IsNull(asyncOperation);

            if (LoadAsync)
            {
                asyncOperation = SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
                asyncOperation.allowSceneActivation = allowSceneActivation;
                await asyncOperation.ToUniTask(cancellationToken: cancellationToken);
            }
            else
            {
                SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Additive);
            }

            if (SetActiveOnNavigated)
            {
                SceneManager.SetActiveScene(scene);
            }

            asyncOperation = null;
        }
    }
}