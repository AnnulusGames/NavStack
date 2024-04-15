using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using NavStack;
using LitMotion;
using LitMotion.Extensions;

public class SamplePage1 : MonoBehaviour, IPage, IPageStackEvent
{
    [SerializeField] Text text;
    
    public UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default) => UniTask.CompletedTask;
    public UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default) => UniTask.CompletedTask;

    public async UniTask OnPush(NavigationContext context, CancellationToken cancellationToken = default)
    {
        text.text = context.Parameters["id"] as string;

        if (!context.Options.Animated)
        {
            transform.localScale = Vector3.one;
            return;
        }

        await LMotion.Create(Vector3.zero, Vector3.one, 0.25f)
            .WithEase(Ease.InQuad)
            .BindToLocalScale(transform)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }

    public async UniTask OnPop(NavigationContext context, CancellationToken cancellationToken = default)
    {
        if (!context.Options.Animated)
        {
            transform.localScale = Vector3.zero;
            return;
        }

        await LMotion.Create(Vector3.one, Vector3.zero, 0.25f)
            .WithEase(Ease.OutQuad)
            .BindToLocalScale(transform)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }
}
