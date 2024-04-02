#if NAVSTACK_ADDRESSABLES_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace NavStack.Content
{
    internal sealed class AddressablesResourceProvider : IResourceProvider
    {
        public async UniTask<T> LoadAsync<T>(string key, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object
        {
            var result = await Addressables.LoadAssetAsync<T>(key).ToUniTask(cancellationToken: cancellationToken);
            return result;
        }

        public UniTask UnloadAsync<T>(T obj, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object
        {
            cancellationToken.ThrowIfCancellationRequested();
            Addressables.Release(obj);
            return UniTask.CompletedTask;
        }
    }
}
#endif