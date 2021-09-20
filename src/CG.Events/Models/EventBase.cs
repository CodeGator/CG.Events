using CG.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CG.Events.Models
{
    /// <summary>
    /// This class is a base implementation of an <see cref="IEventAggregator"/> 
    /// event. 
    /// </summary>
    public abstract class EventBase : DisposableBase, IDisposable
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains a list of event subscriptions.
        /// </summary>
        protected readonly IList<IEventSubscription> _subscriptions;

        /// <summary>
        /// This field contains a synchronization object.
        /// </summary>
        protected readonly object _sync;

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This class creates a new instance of the <see cref="EventBase"/>
        /// class.
        /// </summary>
        protected EventBase()
        {
            // Setup default values.
            _subscriptions = new List<IEventSubscription>();
            _sync = new object();
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method publishes the event to all current subscribers.
        /// </summary>
        /// <param name="args">Optional arguments for the event.</param>
        /// <exception cref="AggregateException">This exception is thrown 
        /// whenever one or more errors are detected whil publishing.</exception>
        public virtual void Publish(
            params object[] args
            )
        {
            try
            {
                // Get all the currently active subscriptions.
                var activeSubscriptions = _subscriptions.Where(
                    x => x.IsAlive
                    );

                // Loop through the active subscriptions.
                var errors = new List<Exception>();
                foreach (var sub in activeSubscriptions)
                {
                    try
                    {
                        // Invoke the subscription's event handler.
                        sub.Invoke(args);
                    }
                    catch (Exception ex)
                    {
                        // Remember the error.
                        errors.Add(ex);
                    }
                }

                // Did we fail?
                if (errors.Any())
                {
                    throw new AggregateException(
                        message: $"Encountered one or more errors while " +
                            $"publishing event: '{GetType().Name}'. See " +
                            $"inner exceptions for more detail.",
                        innerExceptions: errors
                        );
                }
            }
            finally
            {
                // Get a list of dead subscriptions.
                var deadSubscriptions = _subscriptions.Where(
                    x => !x.IsAlive
                    ).ToList();

                lock (_sync)
                {
                    // Loop through the dead subscriptions.
                    foreach (var deadSub in deadSubscriptions)
                    {
                        // Remove the subscription.
                        _subscriptions.Remove(deadSub);
                    }
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method publishes the event to all current subscribers.
        /// </summary>
        /// <param name="args">Optional arguments for the event.</param>
        /// <exception cref="AggregateException">This exception is thrown 
        /// whenever one or more errors are detected whil publishing.</exception>
        public virtual Task PublishAsync(
            params object[] args
            )
        {
            // Get all the currently active subscriptions.
            var activeSubscriptions = _subscriptions.Where(
                x => x.IsAlive
                );

            // Loop through the active subscriptions.
            foreach (var sub in activeSubscriptions)
            {
                // Invoke the subscription's event handler.
                Task.Run(() => sub.Invoke(args));
            }

            // Get a list of dead subscriptions.
            var deadSubscriptions = _subscriptions.Where(
                x => !x.IsAlive
                ).ToList();

            lock (_sync)
            {
                // Loop through the dead subscriptions.
                foreach (var deadSub in deadSubscriptions)
                {
                    // Remove the subscription.
                    _subscriptions.Remove(deadSub);
                }
            }

            // Return the completed task.
            return Task.CompletedTask;
        }

        // *******************************************************************

        /// <summary>
        /// This method subscribes to an event using the specified delegate. 
        /// That delegate is then called by the event whenever the <see cref="Publish"/> 
        /// method is called.
        /// </summary>
        /// <param name="action">The delegate to use for the subscription.</param>
        /// <param name="strongReference">Indicates whether to maintain a strong
        /// or weak reference to the action.</param>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more arguments are missing, or invalid.</exception>
        public virtual IDisposable Subscribe(
            Action<object[]> action,
            bool strongReference = false
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            // Create the event subscription.
            var subscription = new EventSubscription(
                action,
                strongReference
                );

            lock (_sync)
            {
                // Save the subscription.
                _subscriptions.Add(subscription);
            }

            // Return the subscription.
            return subscription;
        }

        // *******************************************************************

        /// <summary>
        /// This method subscribes to an event. When the <see cref="Publish"/> 
        /// method is then called on that event, the event's <see cref="OnInvoke"/> 
        /// method is then called.
        /// </summary>
        /// <param name="strongReference">True to keep a strong reference to
        /// the event; false otherwise.</param>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more arguments are missing, or invalid.</exception>
        public virtual IDisposable Subscribe(
            bool strongReference = false
            )
        {
            // Create the event subscription.
            var subscription = new EventSubscription(
                (args) => OnInvoke(args),
                strongReference
                );

            lock (_sync)
            {
                // Save the subscription.
                _subscriptions.Add(subscription);
            }

            // Return the subscription.
            return subscription;
        }

        // *******************************************************************

        /// <summary>
        /// This method removes a subscription from the event aggregator.
        /// </summary>
        /// <param name="subscription">The subscription to use for the operation.</param>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more arguments are missing, or invalid.</exception>
        public virtual void Unsubscribe(
            IDisposable subscription
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(subscription, nameof(subscription));

            // Cleanup the subscription.
            subscription.Dispose();
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method may be called, by the event aggregator, by subscribing 
        /// to an event using the overload that doesn't accept an <see cref="Action"/> 
        /// parameter. When the <see cref="Publish"/> method is called on that
        /// event, the aggregator will call this method.
        /// </summary>
        /// <param name="args">Optional arguments for the event.</param>
        protected virtual void OnInvoke(
            params object[] args
            )
        {
            // TODO : implement in a derived type.
        }

        #endregion
    }
}
