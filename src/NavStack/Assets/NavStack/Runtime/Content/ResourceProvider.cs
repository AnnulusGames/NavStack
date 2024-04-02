namespace NavStack.Content
{
    public static class ResourceProvider
    {
        static ResourceProvider()
        {
            DefaultResourceProvider = Resources;
        }

        public static IResourceProvider DefaultResourceProvider { get; set; }

        public static readonly IResourceProvider Resources = new ResourcesResourceProvider();
#if NAVSTACK_ADDRESSABLES_SUPPORT
        public static readonly IResourceProvider Addressables = new AddressablesResourceProvider();
#endif
    }
}