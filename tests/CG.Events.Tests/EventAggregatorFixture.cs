using CG.Events.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CG.Events
{
    /// <summary>
    /// This class is a test fixture for the <see cref="EventAggregator"/>
    /// class.
    /// </summary>
    [TestClass]
    public class EventAggregatorFixture
    {
        // *******************************************************************
        // Types.
        // *******************************************************************

        #region Types

        /// <summary>
        /// This class is used for internal testing purposes.
        /// </summary>
        class TestEvent : EventBase { }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method ensures the <see cref="EventAggregator.GetEvent{TEvent}"/>
        /// method returns a valid event.
        /// </summary>
        [TestMethod]
        public void GetEventReturnsEvent()
        {
            // Arrange ...
            var eventAggregator = new EventAggregator();

            // Act ...
            var result = eventAggregator.GetEvent<TestEvent>();

            // Assert ...
            Assert.IsInstanceOfType(result, typeof(TestEvent));
        }

        #endregion
    }
}
