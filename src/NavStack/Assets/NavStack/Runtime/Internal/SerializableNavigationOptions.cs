using System;
using UnityEngine;

namespace NavStack.Internal
{
    [Serializable]
    public sealed class SerializableNavigationOptions
    {
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

        public SerializableNavigationOptions()
        {
        }

        public SerializableNavigationOptions(NavigationOptions source)
        {
            animated = source.Animated;
            awaitOperation = source.AwaitOperation;
        }

        public NavigationOptions ToNavigationOptions()
        {
            return new NavigationOptions()
            {
                Animated = animated,
                AwaitOperation = awaitOperation,
            };
        }
    }
}