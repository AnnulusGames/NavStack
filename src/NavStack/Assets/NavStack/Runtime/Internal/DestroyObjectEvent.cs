using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    internal sealed class DestroyObjectEvent : IPageLifecycleEvent
    {
        public DestroyObjectEvent(UnityEngine.Object instance)
        {
            this.instance = instance;
        }

        readonly UnityEngine.Object instance;

        UniTask IPageLifecycleEvent.OnCleanup(CancellationToken cancellationToken)
        {
            if (instance != null) UnityEngine.Object.Destroy(instance);
            return UniTask.CompletedTask;
        }
    }
}