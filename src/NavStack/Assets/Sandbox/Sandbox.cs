using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack;
using NavStack.UI;
using NavStack.Content;

public class Sandbox : MonoBehaviour
{
    [SerializeField] NavigationStack navigation;
    [SerializeField] Page prefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            navigation.PushNewObjectAsync("SamplePage1", ResourceProvider.Addressables, destroyCancellationToken).Forget();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            navigation.PopAsync(destroyCancellationToken).Forget();
        }
    }
}
