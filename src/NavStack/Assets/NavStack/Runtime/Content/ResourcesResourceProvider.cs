using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NavStack.Content
{
    internal sealed class ResourcesResourceProvider : IResourceProvider
    {
        public async UniTask<T> LoadAsync<T>(string key, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object
        {
            var resource = await Resources.LoadAsync(key).ToUniTask(cancellationToken: cancellationToken);
            if (resource is not T result) throw new Exception(); // TODO:

            return result;
        }

        public UniTask UnloadAsync<T>(T obj, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object
        {
            // not supported
            return UniTask.CompletedTask;
        }
    }
}