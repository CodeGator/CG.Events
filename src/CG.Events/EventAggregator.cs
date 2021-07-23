using CG.Events.Models;
using CG.Validations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace CG.Events
{
    /// <summary>
    /// This class is a default implementation of the <see cref="IEventAggregator"/>
    /// interface.
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains a table of events, by event-type.
        /// </summary>
        private readonly ConcurrentDictionary<Type, EventBase> _events;

        /// <summary>
        /// This field contains a service provider instance.
        /// </summary>
        private readonly IServiceProvider? _serviceProvider;

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new intance of the <see cref="EventAggregator"/>
        /// class
        /// </summary>
        public EventAggregator()
        {
            // Save the references.
            _serviceProvider = null;
            _events = new ConcurrentDictionary<Type, EventBase>();
        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="EventAggregator"/>
        /// class
        /// </summary>
        [ActivatorUtilitiesConstructor] 
        public EventAggregator(
            IServiceProvider serviceProvider
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider));

            // Save the references.
            _serviceProvider = serviceProvider;
            _events = new ConcurrentDictionary<Type, EventBase>();            
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public virtual TEvent GetEvent<TEvent>() 
            where TEvent : EventBase
        {
            // Look for an existing event.
            if (false == _events.TryGetValue(typeof(TEvent), out var ev))
            {
                // If we get here then we need to create an event instance.

                // How should we create the event?
                if (null == _serviceProvider)
                {
                    // Create the event the old fashioned way.
                    ev = Activator.CreateInstance<TEvent>();
                }
                else
                {
                    // Create the event with full DI support.
                    ev = ActivatorUtilities.CreateInstance<TEvent>(
                        _serviceProvider
                        );
                }

                // Did we fail to save our event?
                if (!_events.TryAdd(typeof(TEvent), ev))
                {
                    // If we get here then another thread/task saved their
                    //   event instance in the _events table before we had 
                    //   a chance to save ours. So, let's use theirs instead.

                    if (!_events.TryGetValue(typeof(TEvent), out ev))
                    {
                        // If we get here then the world no longer makes sense,
                        //    white is black, up is down, and we can't save an
                        //    event object to our table, for some reason.

                        // Panic!!
                        throw new EventAggregatorException(
                            message: $"Failed to save the event: '{typeof(TEvent).Name}'"
                            );
                    }
                }
            }

            // Return the results.
            return (TEvent)ev;
        }

        #endregion
    }
}
