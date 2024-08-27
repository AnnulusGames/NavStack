using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack;
using NavStack.UI;
using NavStack.Content;
using R3;

public class Sandbox : MonoBehaviour
{
    [SerializeField] NavigationStack navigation;

    void Start()
    {
        navigation.OnNavigatingAsObservable()
            .Subscribe(x =>
            {
                Debug.Log("OnNavigating");
                Debug.Log(x.Previous == null ? "null" : ((SamplePage1)x.Previous).GetCurrentText());
                Debug.Log(x.Current == null ? "null" : ((SamplePage1)x.Current).GetCurrentText());
            })
            .AddTo(this);

        navigation.OnNavigatedAsObservable()
            .Subscribe(x =>
            {
                Debug.Log("OnNavigated");
                Debug.Log(x.Previous == null ? "null" : ((SamplePage1)x.Previous).GetCurrentText());
                Debug.Log(x.Current == null ? "null" : ((SamplePage1)x.Current).GetCurrentText());
            })
            .AddTo(this);
    }

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
