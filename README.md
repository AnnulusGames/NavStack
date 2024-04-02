# NavStack
 Asynchronous screen transition/navigation for Unity.

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

In NavStack, a single screen is divided into units called "Pages." The only requirement for being a Page is implementing the `IPage` interface. You can represent a Page with any object, such as a GameObject, VisualElement, or Scene. (An implementation for uGUI is provided by default.)

Page transitions and lifecycle management are handled by "Navigation." NavStack provides two types of Navigation: `INavigationStack`, which can stack Page transitions, and `INaviagtionSheet`, which only switches between active Pages without maintaining a history of transitions.

## Page Lifecycle

The basic interface for a Page, `IPage`, has the following definition. Each event associated with a Page is called by Navigation, allowing you to customize the screen transition process by implementing these methods.

```cs
public interface IPage
{
    IList<IPageLifecycleEvent> LifecycleEvents { get; }

    UniTask OnInitialize(CancellationToken cancellationToken = default);
    UniTask OnAppear(NavigationOptions options, CancellationToken cancellationToken = default);
    UniTask OnDisappear(NavigationOptions options, CancellationToken cancellationToken = default);
    UniTask OnCleanup(CancellationToken cancellationToken = default);
}
```

| Event | Description |
| - | - |
| OnInitialize | Called once when the Page is initialized. |
| OnAppear | Called each time the Page becomes visible. |
| OnDisappear | Called each time the Page becomes hidden. |
| OnCleanup | Called once when the Page is cleaned up. |

Additionally, you can add callbacks before and after each lifecycle event by implementing objects that implement `IPageLifecycleEvent`.

```cs
public interface IPageLifecycleEvent
{
    UniTask OnInitialize(CancellationToken cancellationToken = default);
    UniTask OnCleanup(CancellationToken cancellationToken = default);
    UniTask OnAppear(NavigationOptions options, CancellationToken cancellationToken = default);
    UniTask OnDisappear(NavigationOptions options, CancellationToken cancellationToken = default);
}
```

## NavigationStack

NavigationStack supports stacked screen transitions. When you push a Page onto the stack, it becomes the active Page, and the last Page pushed becomes the active Page. To push a Page, use `PushAsync()`, and to pop a Page, use `PopAsync()`.

```cs
INavigationStack navigationStack;
IPage page;

await navigationStack.PushAsync(page);
await navigationStack.PopAsync();
```

In NavigationStack, the Page lifecycle is handled as follows:

1. `OnInitialize` is called when pushing a Page.
2. During transitions, `OnDisappear` is called for the previous Page and `OnAppear` for the new Page.
3. `OnCleanup` is called when popping a Page.

## NavigationSheet

NavigationSheet supports tab-like switching between active Pages. Unlike NavigationStack, it does not maintain a history of Page transitions.

To use NavigationSheet, you need to add Pages with `AddAsync()` beforehand.

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

In NavigationSheet, the Page lifecycle is handled as follows:

1. `OnInitialize` is called when adding a Page.
2. During transitions, `OnDisappear` is called for the previous Page and `OnAppear` for the new Page.
3. `OnCleanup` is called when removing a Page.

## NavigationOptions

When performing Page transitions using Navigation, you can pass `NavigationOptions`. If omitted, the options specified in the Navigation's `DefaultOptions` are applied.

| Property | Description |
| - | - |
| Animated | Whether to play transition animations (default value is true) |
| AwaitOpearion | Behavior when a transition operation is called again while a Page transition is in progress (default value is NavigationAwaitOperation.Error) |

## Navigation Callbacks

By implementing `INavigationCallbackReceiver`, you can add callbacks before and after each Page lifecycle event by adding objects to Navigation.

```cs
public interface INavigationCallbackReceiver
{
    void OnBeforeInitialize(IPage page) { }
    void OnAfterInitialize(IPage page) { }
    void OnBeforeCleanup(IPage page) { }
    void OnAfterCleanup(IPage page) { }
    void OnBeforeAppear(IPage page) { }
    void OnAfterAppear(IPage page) { }
    void OnBeforeDisappear(IPage page) { }
    void OnAfterDisappear(IPage page) { }
}
```

```cs
INavigationCallbackReceiver receiver;
navigation.CallbackReveivers.Add(receiver);
```

## Workflow for uGUI

NavStack provides components for handling Pages and Navigation with uGUI by default.

When using NavStack with uGUI, add the `Navigation Stack` / `Navigation Sheet` component to any object placed under the Canvas.

<img src="https://github.com/AnnulusGames/NavStack/blob/main/docs/images/img-navigationstack-inspector.png" width="500">

Next, create Pages to display UI. Define a component that inherits from the `Page` class.

```cs
public class SamplePage1 : Page
{
    [SerializeField] CanvasGroup canvasGroup;

    // Override methods to customize events
    protected override async UniTask OnAppearCore(NavigationOptions options, CancellationToken cancellationToken = default)
    {
        if (!options.Animated)
        {
            canvasGroup.alpha = 1f;
            return;
        }

        // In this example, LitMotion is used to implement tween animations
        await LMotion.Create(0f, 1f, 0.25f)
            .WithEase(Ease.InQuad)
            .BindToCanvasGroupAlpha(canvasGroup)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }

    protected override async UniTask OnDisappearCore(NavigationOptions options, CancellationToken cancellationToken = default)
    {
        if (!options.Animated)
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

It's convenient to manage Prefabs of created Pages. Prefab-ized Pages can be added to Navigation using `PushNewObjectAsync()` or `AddNewObjectAsync()`, allowing for the generation/destruction of objects based on the Page lifecycle.

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