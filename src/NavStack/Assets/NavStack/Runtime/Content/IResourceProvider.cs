using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack.Content
{
    public interface IResourceProvider
    {
        UniTask<T> LoadAsync<T>(string key, CancellationToken cancellationToken = default) where T : UnityEngine.Object;
        UniTask UnloadAsync<T>(T obj, CancellationToken cancellationToken = default) where T : UnityEngine.Object;
    }
}