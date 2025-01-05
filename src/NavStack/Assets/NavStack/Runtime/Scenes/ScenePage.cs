using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace NavStack.Scenes
{
    public class ScenePage : IPage, IPageLifecycleEvent
    {
        enum SceneLoadType
        {
            BuildIndex,
            SceneName
        }

        readonly SceneLoadType loadType;
        readonly int buildIndex;
        readonly string sceneName;

        public bool LoadOnAttached { get; init; } = false;

        public ScenePage(int buildIndex)
        {
            loadType = SceneLoadType.BuildIndex;
            this.buildIndex = buildIndex;
        }

        public ScenePage(string sceneName)
        {
            loadType = SceneLoadType.SceneName;
            this.sceneName = sceneName;
        }

        public UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default)
        {
            if (LoadOnAttached) return UniTask.CompletedTask;
            return UnloadAsync(cancellationToken);
        }

        public UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default)
        {
            if (LoadOnAttached) return UniTask.CompletedTask;
            return LoadAsync(cancellationToken);
        }

        public UniTask OnAttached(CancellationToken cancellationToken = default)
        {
            if (!LoadOnAttached) return UniTask.CompletedTask;
            return LoadAsync(cancellationToken);
        }

        public UniTask OnDetached(CancellationToken cancellationToken = default)
        {
            if (!LoadOnAttached) return UniTask.CompletedTask;
            return UnloadAsync(cancellationToken);
        }

        async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            switch (loadType)
            {
                case SceneLoadType.BuildIndex:
                    await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive).WithCancellation(cancellationToken);
                    break;
                case SceneLoadType.SceneName:
                    await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).WithCancellation(cancellationToken);
                    break;
            }
        }

        async UniTask UnloadAsync(CancellationToken cancellationToken)
        {
            switch (loadType)
            {
                case SceneLoadType.BuildIndex:
                    await SceneManager.UnloadSceneAsync(buildIndex).WithCancellation(cancellationToken);
                    break;
                case SceneLoadType.SceneName:
                    await SceneManager.UnloadSceneAsync(sceneName).WithCancellation(cancellationToken);
                    break;
            }
        }
    }
}