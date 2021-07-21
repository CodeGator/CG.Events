using CG.Events.Models;
using System;

namespace CG.Events
{
    /// <summary>
    /// This interface represents an object that manages event aggregation.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// This method returns an instance of the specified 
        /// event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of associated event.</typeparam>
        /// <returns>An instance of the specified event.</returns>
        TEvent GetEvent<TEvent>()
            where TEvent : EventBase;
    }
}
