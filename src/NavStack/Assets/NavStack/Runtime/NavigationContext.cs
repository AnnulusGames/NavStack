namespace NavStack
{
    public record NavigationContext
    {
        public NavigationAwaitOperation AwaitOperation { get; init; } = NavigationAwaitOperation.Sequential;
        public NavigationParameters Parameters { get; init; } = new();
    }
}