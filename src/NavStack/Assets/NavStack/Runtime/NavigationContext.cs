namespace NavStack
{
    public record NavigationContext
    {
        public NavigationOptions Options { get; init; }
        public NavigationParameters Parameters { get; init; } = new();
    }
}