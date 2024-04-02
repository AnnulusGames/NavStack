#if NAVSTACK_UGUI_SUPPORT
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Navigation Sheet")]
    [RequireComponent(typeof(RectTransform))]
    public class NavigationSheet : MonoBehaviour, INavigationSheet
    {
        sealed class CallbackReceiver : INavigationCallbackReceiver
        {
            public NavigationSheet NavigationSheet { get; set; }

            public void OnAfterDisappear(IPage page)
            {
                if (!NavigationSheet.togglePageActive) return;
                if (page is Component component)
                {
                    component.gameObject.SetActive(false);
                }
            }

            public void OnBeforeAppear(IPage page)
            {
                if (!NavigationSheet.togglePageActive) return;
                if (page is Component component)
                {
                    component.gameObject.SetActive(true);
                }
            }

            public void OnAfterInitialize(IPage page)
            {
                if (page is Component component)
                {
                    if (NavigationSheet.parentTransform != null)
                    {
                        component.transform.SetParent(NavigationSheet.parentTransform, false);
                    }

                    if (NavigationSheet.togglePageActive)
                    {
                        component.gameObject.SetActive(false);
                    }
                }
            }
        }

        [SerializeField] RectTransform parentTransform;
        [SerializeField] bool togglePageActive = true;
        [SerializeField] NavigationOptions defaultOptions;

        readonly NavigationSheetCore core = new();

        public IReadOnlyCollection<IPage> Pages => core.Pages;
        public IList<INavigationCallbackReceiver> CallbackReceivers => core.CallbackReceivers;
        public IPage ActivePage => core.ActivePage;
        public NavigationOptions DefaultOptions
        {
            get => defaultOptions;
            set => defaultOptions = value;
        }

        protected virtual void Awake()
        {
            CallbackReceivers.Add(new CallbackReceiver() { NavigationSheet = this });
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

        public UniTask ShowAsync(int index, NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return core.ShowAsync(index, options, cancellationToken);
        }

        public UniTask HideAsync(NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return core.HideAsync(options, cancellationToken);
        }
    }
}
#endif