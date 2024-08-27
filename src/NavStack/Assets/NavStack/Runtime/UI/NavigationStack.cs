#if NAVSTACK_UGUI_SUPPORT
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack.Internal;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Navigation Stack")]
    [RequireComponent(typeof(RectTransform))]
    public class NavigationStack : MonoBehaviour, INavigationStack
    {
        [SerializeField] RectTransform parentTransform;
        [SerializeField] SerializableNavigationOptions defaultOptions;

        readonly NavigationStackCore core = new();

        public event Action<IPage> OnPageAttached
        {
            add => core.OnPageAttached += value;
            remove => core.OnPageAttached -= value;
        }
        public event Action<IPage> OnPageDetached
        {
            add => core.OnPageDetached += value;
            remove => core.OnPageDetached -= value;
        }

        public IPage ActivePage => core.ActivePage;
        public IReadOnlyCollection<IPage> Pages => core.Pages;
        public NavigationOptions DefaultOptions
        {
            get => defaultOptions.ToNavigationOptions();
            set => defaultOptions = new(value);
        }

        protected virtual void Awake()
        {
            OnPageAttached += page =>
            {
                if (page is Component component)
                {
                    if (parentTransform != null)
                    {
                        component.transform.SetParent(parentTransform, false);
                    }
                }
            };
        }

        public UniTask PopAsync(NavigationContext context, CancellationToken cancellationToken = default)
        {
            return core.PopAsync(this, context, cancellationToken);
        }

        public UniTask PushAsync(IPage page, NavigationContext context, CancellationToken cancellationToken = default)
        {
            return core.PushAsync(this, () => new(page), context, cancellationToken);
        }

        public UniTask PushAsync(Func<UniTask<IPage>> factory, NavigationContext context, CancellationToken cancellationToken = default)
        {
            return core.PushAsync(this, factory, context, cancellationToken);
        }
    }
}
#endif