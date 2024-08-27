using System.Collections.Generic;

namespace NavStack
{
    public record NavigationContext
    {
        public NavigationOptions Options { get; init; }
        public IDictionary<object, object> Parameters { get; init; } = new Dictionary<object, object>();
    }
}