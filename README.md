# NavStack
 Asynchronous screen transition/navigation for Unity.

[日本語版READMEはこちら](README_JA.md)

## Overview

NavStack is a library for managing screen transitions in Unity. It provides a foundation interface for asynchronous API-based screen transitions, as well as support for integration with uGUI and content management using Resources/Addressables.

> [!NOTE]
> NavStack is currently released as a preview version. Feedback through issues or pull requests would be appreciated.

## Setup

### Requirements

* Unity 2021.3 or later
* [UniTask](https://github.com/Cysharp/UniTask) 2.0.0 or later

### Installation

1. Open the Package Manager from Window > Package Manager.
2. Click the "+" button > Add package from git URL.
3. Enter the following URL:

```
https://github.com/AnnulusGames/NavStack.git?path=src/NavStack/Assets/NavStack
```

Alternatively, open Packages/manifest.json and add the following to the dependencies block:

```json
{
    "dependencies": {
        "com.annulusgames.navstack": "https://github.com/AnnulusGames/NavStack.git?path=src/NavStack/Assets/NavStack"
    }
}
```

## Basic Concepts

In NavStack, a screen is divided into units called "Pages." The only requirement for a Page is the implementation of the `IPage` interface, allowing any object such as GameObjects, VisualElements, or Scenes to be represented as a Page.

Screen transitions and lifecycle management are handled by "Navigation." NavStack provides two types of Navigation: `INavigationStack`, which can stack Page transitions, and `INaviagtionSheet`, which switches between active Pages without keeping a history of transitions.

## Page Lifecycle

The `IPage` interface defines the basic events for a Page's lifecycle. Each event is called from the Navigation side, allowing customization of screen transition processes.

```cs
public interface IPage
{
    UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default);
    UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default);
}
```

| Event           | Description                                |
| --------------- | ------------------------------------------ |
| OnNavigatedFrom | Called when navigating away from the Page. |
| OnNavigatedTo   | Called when navigating to the Page.        |

Additionally, by implementing `IPageLifecycleEvent`, you can add additional lifecycle events for the Page.

```cs
public interface IPageLifecycleEvent
{
    UniTask OnAttached(CancellationToken cancellationToken = default);
    UniTask OnDetached(CancellationToken cancellationToken = default);
}
```

## NavigationStack

NavigationStack supports stacked screen transitions. Pages pushed onto the stack are stacked, and the last pushed Page becomes the active Page. You can push a Page using `PushAsync()` and pop using `PopAsync()`.

```cs
INavigationStack navigationStack;
IPage page;

await navigationStack.PushAsync(page);
await navigationStack.PopAsync();
```

You can also add NavigationStack-specific events to Pages by implementing `IPageStackEvent`.

```cs
public interface IPageStackEvent
{
    UniTask OnPush(NavigationContext context, CancellationToken cancellationToken = default);
    UniTask OnPop(NavigationContext context, CancellationToken cancellationToken = default);
}
```

## NavigationSheet

NavigationSheet supports switching between active Pages, similar to tabs. Unlike NavigationStack, it does not keep a history of Page transitions.

You need to use `AddAsync()` to add Pages to the NavigationSheet.

```cs
INavigationSheet navigationSheet;
IPage page1;
IPage page2;
IPage page3;

await navigationSheet.AddAsync(page1);
await navigationSheet.AddAsync(page2);
await navigationSheet.AddAsync(page3);
```

To switch the displayed Page, use `ShowAsync()`. To hide a Page, use `HideAsync()`.

```cs
int index = 0;
await navigationSheet.ShowAsync(index);
await navigationSheet.HideAsync();
```

You can remove Pages using `RemoveAsync()` or `RemoveAllAsync()`.

```cs
await navigationSheet.RemoveAsync(page3);
await navigationSheet.RemoveAllAsync();
```

## NavigationContext

You can pass `NavigationContext` during Page transitions to pass data between Pages and specify transition options.

### Passing Data

You can pass data to the destination Page through `NavigationContext`.

```cs
var page = new ExamplePage();

var context = new NavigationContext()
{
    Parameters = { { "id", "123456" } }
};

await navigationStack.PushAsync(page, context, cancellationToken);

class ExamplePage : IPage
{
    public UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default)
    {
        var id = (string)context.Parameters["id"];

        ...
    }

    ...
}
```

### NavigationOptions

You can specify transition options using `NavigationOptions`.

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

| Property       | Description                                                                                                                            |
| -------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| Animated       | Specifies whether to play transition animations (default is true).                                                                     |
| AwaitOperation | Specifies the behavior when a transition operation is called again during Page transition (default is NavigationAwaitOperation.Error). |

## Workflow for uGUI

When using NavStack with uGUI, add the `Navigation Stack` / `Navigation Sheet` component to any object placed under the Canvas.

Next, create Pages for displaying UI. Implement a component that inherits from `IPage`.

```cs
public class SamplePage1 : MonoBehaviour, IPage
{
    [SerializeField] CanvasGroup canvasGroup;

    public async UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default)
    {
        if (!context.Options.Animated)
        {
            canvasGroup.alpha = 1f;
            return;
        }

        // Example implementation using LitMotion for tween animations
        await LMotion.Create(0f, 1f, 0.25f)
            .WithEase(Ease.InQuad)
            .BindToCanvasGroupAlpha(canvasGroup)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }

    public async UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default)
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

Prefab the created Page objects for convenience. By adding Prefabed Pages using `PushNewObjectAsync()` or `AddNewObjectAsync()`, you can manage object generation/destruction based on the Page lifecycle.

```cs
Page prefab;
NavigationStack navigationStack;
NavigationSheet navigationSheet;

await navigationStack.PushNewObjectAsync(prefab);
await navigationSheet.AddNewObjectAsync(prefab);
```

## Content Management

TODO

## R3

TODO

## VContainer

TODO

## License

[MIT License](LICENSE)