using System;

namespace EndlessRunner.Events
{
    /// <summary>
    /// Zero-allocation, strongly-typed generic pub-sub event bus.
    /// Allows decoupled communication between gameplay systems.
    /// </summary>
    public static class EventBus<T> where T : struct
    {
        private static Action<T> _onEventRaised;

        /// <summary>
        /// Subscribes a listener method to the event stream.
        /// </summary>
        public static void Subscribe(Action<T> listener)
        {
            _onEventRaised += listener;
        }

        /// <summary>
        /// Unsubscribes a listener method from the event stream.
        /// </summary>
        public static void Unsubscribe(Action<T> listener)
        {
            _onEventRaised -= listener;
        }

        /// <summary>
        /// Raises an event struct to all subscribed listeners with zero GC allocation.
        /// </summary>
        public static void Raise(T eventData)
        {
            _onEventRaised?.Invoke(eventData);
        }
    }
}