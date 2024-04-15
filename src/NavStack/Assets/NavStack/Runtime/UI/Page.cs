#if NAVSTACK_UGUI_SUPPORT
using UnityEngine;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Page")]
    [RequireComponent(typeof(RectTransform))]
    public class Page : MonoBehaviour, IPage
    {
        
    }
}
#endif