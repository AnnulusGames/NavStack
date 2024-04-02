using System;
using UnityEngine;

namespace NavStack
{
    [Serializable]
    public sealed class NavigationOptions
    {
        public NavigationOptions() { }
        public NavigationOptions(NavigationOptions source)
        {
            animated = source.animated;
            awaitOperation = source.awaitOperation;
        }

        [SerializeField] bool animated = true;
        [SerializeField] NavigationAwaitOperation awaitOperation = NavigationAwaitOperation.Error;

        public bool Animated
        {
            get => animated;
            set => animated = value;
        }

        public NavigationAwaitOperation AwaitOperation
        {
            get => awaitOperation;
            set => awaitOperation = value;
        }
    }
}