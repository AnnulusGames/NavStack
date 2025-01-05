using System.Threading;
using Cysharp.Threading.Tasks;
using NavStack;
using NavStack.Scenes;
using UnityEngine;

public class SceneSandbox : MonoBehaviour
{
    static NavigationSheet navigationSheet = new();

    void Start()
    {
        navigationSheet = new();
        SwitchScenes(Application.exitCancellationToken).Forget();
    }

    async UniTask SwitchScenes(CancellationToken cancellationToken)
    {
        await navigationSheet.AddSceneAsync("SceneA", cancellationToken);
        await navigationSheet.AddSceneAsync("SceneB", cancellationToken);

        var index = 0;
        while (true)
        {
            await navigationSheet.ShowAsync(index, cancellationToken);
            await UniTask.WaitForSeconds(1f, cancellationToken: cancellationToken);
            index++;
            if (navigationSheet.Pages.Count == index) index = 0;
        }
    }
}
