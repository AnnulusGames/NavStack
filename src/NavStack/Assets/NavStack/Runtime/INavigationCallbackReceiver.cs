namespace NavStack
{
    public interface INavigationCallbackReceiver
    {
        void OnBeforeInitialize(IPage page) { }
        void OnAfterInitialize(IPage page) { }
        void OnBeforeCleanup(IPage page) { }
        void OnAfterCleanup(IPage page) { }
        void OnBeforeAppear(IPage page) { }
        void OnAfterAppear(IPage page) { }
        void OnBeforeDisappear(IPage page) { }
        void OnAfterDisappear(IPage page) { }
    }
}