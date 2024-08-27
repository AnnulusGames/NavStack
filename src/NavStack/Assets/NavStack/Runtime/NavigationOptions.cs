namespace NavStack
{
    public record NavigationOptions
    {
        public bool Animated { get; init; } = true;
        public NavigationAwaitOperation AwaitOperation { get; init; } = NavigationAwaitOperation.Error; 
    }
}