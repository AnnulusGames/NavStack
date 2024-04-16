# NavStack
 Asynchronous screen transition/navigation for Unity.


[English README is here.](README.md)

## 概要

NavStackはUnityにおける画面遷移を管理するためのライブラリです。非同期APIで構築された画面遷移の基盤インターフェースを提供するほか、uGUIとの連携やResources/Addressablesを用いたコンテンツ管理などをサポートします。

> [!NOTE]
> 現在NavStackはプレビュー版として公開されています。フィードバックをIssueやPull Requestでいただけると幸いです。

## セットアップ

### 要件

* Unity 2021.3 以上
* [UniTask](https://github.com/Cysharp/UniTask) 2.0.0 以上

### インストール

1. Window > Package ManagerからPackage Managerを開く
2. 「+」ボタン > Add package from git URL
3. 以下のURLを入力する

```
https://github.com/AnnulusGames/NavStack.git?path=src/NavStack/Assets/NavStack
```

あるいはPackages/manifest.jsonを開き、dependenciesブロックに以下を追記

```json
{
    "dependencies": {
        "com.annulusgames.navstack": "https://github.com/AnnulusGames/NavStack.git?path=src/NavStack/Assets/NavStack"
    }
}
```

## 基本的な概念

NavStackにおいて、1つの画面は「Page」という単位に分割されます。Pageであるための要件は`IPage`インターフェースの実装のみであり、GameObjectやVisualElement、Sceneなど任意のものをPageとして表現することが可能です。

Pageの遷移やライフサイクルの管理は「Navigation」が行います。NavStackではPageの遷移履歴をスタック可能な`INavigationStack`と、履歴を持たずアクティブなPageの切り替えのみを行う`INaviagtionSheet`の2種類が用意されています。

## Pageのライフサイクル

Pageの基本インターフェースである`IPage`は以下のような定義を持ちます。Pageの持つ各イベントはNavigation側から呼び出され、これを実装することによって画面遷移の処理をカスタマイズできます。

```cs
public interface IPage
{
    UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default);
    UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default);
}
```

| イベント | 説明 |
| - | - |
| OnNavigatedFrom | 他のPageから遷移してきた際に呼ばれる。 |
| OnNavigatedTo | 他のPageへ遷移する際に呼ばれる。 |

これをMonoBehaviourを継承したコンポーネント等で実装することでPageとして使用可能なViewを作成できます。

また`IPageLifecycleEvent`を実装することで、Pageのライフサイクルイベントを追加することも可能です。

```cs
public interface IPageLifecycleEvent
{
    UniTask OnAttached(CancellationToken cancellationToken = default);
    UniTask OnDetached(CancellationToken cancellationToken = default);
}
```

| イベント | 説明 |
| - | - |
| OnAttached | PageがNavigationに追加された際に呼ばれる。 |
| OnDetached | PageがNavigationから削除された際に呼ばれる。 |

## NavigationStack

NavigationStackはスタック形式の画面遷移をサポートするNavigationです。

NavigationStackではPushしたPageはスタックされ、最後にPushしたPageがアクティブなPageとして表示されます。PageをPushするには`PushAsync()`、Popするには`PopAsync()`を使用します。

```cs
INavigationStack navigationStack;
IPage page;

await navigationStack.PushAsync(page);
await navigationStack.PopAsync();
```

また、Page側で`IPageStackEvent`を実装することで、NavigationStack用のイベントを追加することも可能です。

```cs
public interface IPageStackEvent
{
    UniTask OnPush(NavigationContext context, CancellationToken cancellationToken = default);
    UniTask OnPop(NavigationContext context, CancellationToken cancellationToken = default);
}
```

## NavigationSheet

NavigationSheetはタブのような、アクティブなPageの切り替えをサポートするNavigationです。NavigationStackとは異なり、Pageの遷移履歴は保持されません。

NavigationSheetでは事前に`AddAsync()`でPageを追加する必要があります。

```cs
INavigationSheet navigationSheet;
IPage page1;
IPage page2;
IPage page3;

await navigationSheet.AddAsync(page1);
await navigationSheet.AddAsync(page2);
await navigationSheet.AddAsync(page3);
```

表示するPageを切り替える際には`ShowAsync()`を呼び出します。また、Pageを非表示にしたい場合は`HideAsync()`を呼び出します。

```cs
int index = 0;
await navigationSheet.ShowAsync(index);
await navigationSheet.HideAsync();
```

Pageを削除する際には`RemoveAsync()`または`RemoveAllAsync()`が利用できます。

```cs
await navigationSheet.RemoveAsync(page3);
await navigationSheet.RemoveAllAsync();
```

## NavigationContext

Page間のデータの受け渡しやPage遷移のオプションの指定を行いたい場合、遷移時に`NavigationContext`を渡すことが可能です。

### データの受け渡し

`NavigationContext`を通じて遷移先のPageにデータを渡すことが可能です。

```cs
var page = new ExamplePage();

var context = new NavigationContext()
{
    Parameters = { { "id", "123456" } }
};

await navigationStack.PushAsync(page, context, cancellationToken);

class ExamplePage : IPage
{
    public UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default)
    {
        var id = (string)context.Parameters["id"];

        ...
    }

    ...
}
```

### NavigationOptions

`NavigationOptions`を渡すことで遷移のオプションを指定できます。(省略した場合にはNavigationの`DefaultOptions`のオプションが適用されます。)

```cs
var context = new NavigationContext()
{
    Options = new NavigationOptions()
    {
        Animated = true,
        AwaitOperation = NavigationAwaitOperation.Drop,
    }
};

await navigationStack.PushAsync(page, context, cancellationToken);
```

| プロパティ | 説明 |
| - | - |
| Animated | 遷移アニメーションを再生するか (デフォルト値はtrue) |
| AwaitOpearion | Pageの遷移中に再び遷移処理が呼ばれた際の挙動を指定する (デフォルト値はNavigationAwaitOperation.Error) |

## uGUI向けのワークフロー

uGUIでNavStackを使用する際は、Canvas以下に配置した任意のオブジェクトに`Navigation Stack` / `Navigation Sheet` コンポーネントを追加します。

<img src="https://github.com/AnnulusGames/NavStack/blob/main/docs/images/img-navigationstack-inspector.png" width="500">

次に、UIを表示するためのPageを作成します。`IPage`クラスを実装したコンポーネントを定義しましょう。

```cs
public class SamplePage1 : MonoBehaviour, IPage
{
    [SerializeField] CanvasGroup canvasGroup;

    public async UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default)
    {
        if (!context.Options.Animated)
        {
            canvasGroup.alpha = 1f;
            return;
        }

        // この例ではLitMotionを用いたトゥイーンアニメーションを実装している
        await LMotion.Create(0f, 1f, 0.25f)
            .WithEase(Ease.InQuad)
            .BindToCanvasGroupAlpha(canvasGroup)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }

    public async UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default)
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
```

また、作成したPageのオブジェクトはPrefabとして管理すると便利です。Prefab化したPageは`PushNewObjectAsync()`や`AddNewObjectAsync()`を用いてNavigationに追加することで、Pageのライフサイクルに基づいたオブジェクトの生成/破棄を行うことが可能です。

```cs
Page prefab;
NavigationStack navigationStack;
NavigationSheet navigationSheet;

await navigationStack.PushNewObjectAsync(prefab);
await navigationSheet.AddNewObjectAsync(prefab);
```

## コンテンツ管理

TODO

## R3

TODO

## VContainer

TODO

## ライセンス

[MIT License](LICENSE)
