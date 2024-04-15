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
            var context = new NavigationContext();
            context.Parameters["id"] = "user" + Random.Range(0, 100);
            navigation.PushNewObjectAsync("SamplePage1", ResourceProvider.Addressables, context, destroyCancellationToken).Forget();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            navigation.PopAsync(destroyCancellationToken).Forget();
        }
    }
}
