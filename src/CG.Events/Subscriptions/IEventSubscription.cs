using System;

namespace CG.Events
{

    /// <summary>
    /// This interface represents an event subscription.
    /// </summary>
    public interface IEventSubscription : IDisposable
    {
        /// <summary>
        /// This property indicates whether the target of the subscription is
        /// still alive, or not.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// This method invokes the handler for the subscription.
        /// </summary>
        /// <param name="args">Optional arguments for the event.</param>
        void Invoke(
            params object[] args
            );
    }
}
