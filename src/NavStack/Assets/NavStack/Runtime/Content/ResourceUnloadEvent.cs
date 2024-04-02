using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Content
{
    internal sealed class ResourceUnloadEvent : IPageLifecycleEvent
    {
        public ResourceUnloadEvent(IPage page, UnityEngine.Object original, UnityEngine.Object instance, IResourceProvider resourceProvider)
        {
            this.page = page;
            this.original = original;
            this.instance = instance;
            this.resourceProvider = resourceProvider;
        }

        readonly IPage page;
        readonly UnityEngine.Object original;
        readonly UnityEngine.Object instance;
        readonly IResourceProvider resourceProvider;

        async UniTask IPageLifecycleEvent.OnCleanup(CancellationToken cancellationToken)
        {
            if (instance != null) UnityEngine.Object.Destroy(instance);
            await resourceProvider.UnloadAsync(original, cancellationToken);
            page.LifecycleEvents.Remove(this);
        }
    }
}