using System;
using System.Collections.Generic;

namespace NavStack
{
    public interface INavigation
    {
        IPage ActivePage { get; }
        IReadOnlyCollection<IPage> Pages { get; }
        NavigationOptions DefaultOptions { get; set; }
        event Action<IPage> OnPageAttached;
        event Action<IPage> OnPageDetached;
        event Action<(IPage Previous, IPage Current)> OnNavigating;
        event Action<(IPage Previous, IPage Current)> OnNavigated;
    }
}