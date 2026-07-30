using NUnit.Framework;
using EndlessRunner.Events;

namespace EndlessRunner.Tests.EditMode
{
    public class EventBusTests
    {
        private bool _eventReceived;

        [Test]
        public void EventBus_RaisesEvent_ToSubscribedListeners()
        {
            // Arrange
            _eventReceived = false;
            EventBus<PlayerJumpedEvent>.Subscribe(OnPlayerJumped);

            // Act
            EventBus<PlayerJumpedEvent>.Raise(new PlayerJumpedEvent());

            // Assert
            Assert.IsTrue(_eventReceived);

            // Cleanup
            EventBus<PlayerJumpedEvent>.Unsubscribe(OnPlayerJumped);
        }

        private void OnPlayerJumped(PlayerJumpedEvent e)
        {
            _eventReceived = true;
        }
    }
}