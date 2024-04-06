using System.Collections.Generic;

namespace NavStack
{
    public class NavigationContext
    {
        public NavigationOptions Options { get; set; }
        public IDictionary<object, object> Parameters { get; set; } = new Dictionary<object, object>();

        public virtual NavigationContext CreateCopy()
        {
            var context = new NavigationContext()
            {
                Options = new(Options),
                Parameters = new Dictionary<object, object>()
            };

            foreach (var kv in Parameters)
            {
                context.Parameters.Add(kv);
            }

            return context;
        }
    }
}