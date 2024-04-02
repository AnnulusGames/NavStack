using System.Collections.Generic;

namespace NavStack
{
    public interface INavigation
    {
        IPage ActivePage { get; }
        IReadOnlyCollection<IPage> Pages { get; }
        NavigationOptions DefaultOptions { get; set; }
        IList<INavigationCallbackReceiver> CallbackReceivers { get; }
    }
}