using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Content
{
    public interface IResourceProvider
    {
        T Load<T>(string key) where T : UnityEngine.Object;
        void Unload<T>(T obj) where T : UnityEngine.Object;
        UniTask<T> LoadAsync<T>(string key, CancellationToken cancellationToken = default) where T : UnityEngine.Object;
        UniTask UnloadAsync<T>(T obj, CancellationToken cancellationToken = default) where T : UnityEngine.Object;
    }
}