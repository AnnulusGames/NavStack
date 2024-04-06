using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack;
using NavStack.UI;
using LitMotion;
using LitMotion.Extensions;
using R3;

public class SamplePage1 : Page
{
    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        this.OnAppearAsObservable().Subscribe(x =>
        {
            Debug.Log("OnAppear");
        })
        .AddTo(this);

        this.OnDisappearAsObservable().Subscribe(x =>
        {
            Debug.Log("OnDisappear");
        })
        .AddTo(this);
    }

    protected override UniTask OnInitializeCore(CancellationToken cancellationToken = default)
    {
        return base.OnInitializeCore(cancellationToken);
    }

    protected override async UniTask OnAppearCore(NavigationContext context, CancellationToken cancellationToken = default)
    {
        if (!context.Options.Animated)
        {
            canvasGroup.alpha = 1f;
            return;
        }

        await LMotion.Create(0f, 1f, 0.25f)
            .WithEase(Ease.InQuad)
            .BindToCanvasGroupAlpha(canvasGroup)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }

    protected override async UniTask OnDisappearCore(NavigationContext context, CancellationToken cancellationToken = default)
    {
        if (!context.Options.Animated)
        {
            canvasGroup.alpha = 0f;
            return;
        }

        await LMotion.Create(1f, 0f, 0.25f)
            .WithEase(Ease.OutQuad)
            .BindToCanvasGroupAlpha(canvasGroup)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }
}
