#if NAVSTACK_UGUI_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Playable Navigation Page")]
    public class PlayableNavigationPage : Page
    {
        [SerializeField] PlayableDirector playableDirector;

        [Header("Playable Assets")]
        [SerializeField] PlayableAsset onAppearAnimation;
        [SerializeField] PlayableAsset onDisappearAnimation;

        protected override UniTask OnAppearCore(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return PlayAsync(onAppearAnimation, options, cancellationToken);
        }

        protected override UniTask OnDisappearCore(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return PlayAsync(onDisappearAnimation, options, cancellationToken);
        }

        async UniTask PlayAsync(PlayableAsset asset, NavigationOptions options, CancellationToken cancellationToken)
        {
            if (asset == null) return;

            if (!options.Animated)
            {
                playableDirector.Play(asset);
                playableDirector.time = playableDirector.playableAsset.duration;
                return;
            }

            playableDirector.Play(asset);
            await UniTask.WaitUntil(playableDirector.playableGraph.IsDone, cancellationToken: cancellationToken);
        }
    }
}
#endif