#if NAVSTACK_UGUI_SUPPORT
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Navigation Stack")]
    [RequireComponent(typeof(RectTransform))]
    public class NavigationStack : MonoBehaviour, INavigationStack
    {
        sealed class CallbackReceiver : INavigationCallbackReceiver
        {
            public NavigationStack NavigationStack { get; set; }

            public void OnAfterDisappear(IPage page)
            {
                if (!NavigationStack.togglePageActive) return;
                if (page is Component component)
                {
                    component.gameObject.SetActive(false);
                }
            }

            public void OnBeforeAppear(IPage page)
            {
                if (!NavigationStack.togglePageActive) return;
                if (page is Component component)
                {
                    component.gameObject.SetActive(true);
                }
            }

            public void OnAfterInitialize(IPage page)
            {
                if (page is Component component)
                {
                    if (NavigationStack.parentTransform != null)
                    {
                        component.transform.SetParent(NavigationStack.parentTransform, false);
                    }
                }
            }
        }

        [SerializeField] RectTransform parentTransform;
        [SerializeField] bool togglePageActive = true;
        [SerializeField] NavigationOptions defaultOptions;

        readonly NavigationStackCore core = new();

        public IPage ActivePage => core.ActivePage;
        public IReadOnlyCollection<IPage> Pages => core.Pages;
        public NavigationOptions DefaultOptions
        {
            get => defaultOptions;
            set => defaultOptions = value;
        }
        public IList<INavigationCallbackReceiver> CallbackReceivers => core.CallbackReceivers;

        protected virtual void Awake()
        {
            CallbackReceivers.Add(new CallbackReceiver() { NavigationStack = this });
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