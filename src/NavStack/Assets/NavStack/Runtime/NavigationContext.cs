namespace NavStack
{
    public record NavigationContext
    {
        public NavigationAwaitOperation AwaitOperation { get; init; } = NavigationAwaitOperation.Error;
        public NavigationParameters Parameters { get; init; } = new();
    }
}