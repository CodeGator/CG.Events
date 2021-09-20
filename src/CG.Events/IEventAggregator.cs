using CG.Events.Models;
using System;

namespace CG.Events
{
    /// <summary>
    /// This interface represents an object that manages event aggregation.
    /// </summary>
    public interface IEventAggregator : IDisposable
    {
        /// <summary>
        /// This method returns an instance of the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of associated event.</typeparam>
        /// <returns>An instance of the specified event.</returns>
        /// <exception cref="EventAggregatorException">This exception is thrown
        /// whenever this operation failes to produce an event.</exception>
        TEvent GetEvent<TEvent>()
            where TEvent : EventBase;
    }
}
