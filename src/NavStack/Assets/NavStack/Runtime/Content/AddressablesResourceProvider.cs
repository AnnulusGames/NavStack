#if NAVSTACK_ADDRESSABLES_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace NavStack.Content
{
    internal sealed class AddressablesResourceProvider : IResourceProvider
    {
        public T Load<T>(string key) where T : UnityEngine.Object
        {
            var result = Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
            return result;
        }

        public void Unload<T>(T obj) where T : UnityEngine.Object
        {
            Addressables.Release(obj);
        }

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