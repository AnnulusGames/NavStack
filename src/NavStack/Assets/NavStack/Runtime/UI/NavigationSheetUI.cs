#if NAVSTACK_UGUI_SUPPORT
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack.Internal;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Navigation Sheet UI")]
    [RequireComponent(typeof(RectTransform))]
    public class NavigationSheetUI : MonoBehaviour, INavigationSheet
    {
        [SerializeField] RectTransform parentTransform;

        readonly NavigationSheetCore core = new();

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

        public event Action<(IPage Previous, IPage Current)> OnNavigated
        {
            add => core.OnNavigated += value;
            remove => core.OnNavigated -= value;
        }

        public event Action<(IPage Previous, IPage Current)> OnNavigating
        {
            add => core.OnNavigating += value;
            remove => core.OnNavigating -= value;
        }

        public IReadOnlyCollection<IPage> Pages => core.Pages;
        public IPage ActivePage => core.ActivePage;

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

        public UniTask AddAsync(IPage page, CancellationToken cancellationToken = default)
        {
            return core.AddAsync(page, cancellationToken);
        }

        public UniTask RemoveAsync(IPage page, CancellationToken cancellationToken = default)
        {
            return core.RemoveAsync(page, cancellationToken);
        }

        public UniTask RemoveAllAsync(CancellationToken cancellationToken = default)
        {
            return core.RemoveAllAsync(cancellationToken);
        }

        public UniTask ShowAsync(int index, NavigationContext context, CancellationToken cancellationToken = default)
        {
            return core.ShowAsync(index, context, cancellationToken);
        }

        public UniTask HideAsync(NavigationContext context, CancellationToken cancellationToken = default)
        {
            return core.HideAsync(context, cancellationToken);
        }
    }
}
#endif