using CG.Validations;
using System;

namespace CG.Events
{
    /// <summary>
    /// This class is a default implementation of the <see cref="IEventSubscription"/>
    /// interface.
    /// </summary>
    public class EventSubscription : IEventSubscription
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        private object? _innerReference;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <inheritdoc/>
        public bool IsAlive
        {
            get
            {
                // How should we deal with the inner reference?
                if (_innerReference is WeakReference)
                {
                    // Is is alive?
                    return ((WeakReference)_innerReference).IsAlive;
                }
                else
                {
                    // Always alive.
                    return true;
                }
            }
        }
        
        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="EventSubscription"/>
        /// class.
        /// </summary>
        /// <param name="action">The action to associate with this subscription.</param>
        /// <param name="strongReference">Indicates whether to maintain a strong
        /// or weak reference to the action.</param>
        public EventSubscription(
            Action<object[]> action,
            bool strongReference
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            // Save the references.
            if (strongReference)
            {
                // Save the reference directly.
                _innerReference = action;
            }
            else
            {
                // Save the reference weakly.
                _innerReference = new WeakReference(
                    action
                    );
            }
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public virtual void Invoke(
            params object[] args
            )
        {
            // How should we deal with the inner reference?
            if (_innerReference is WeakReference)
            {
                // Invoke the target handler for the event.
                (((WeakReference)_innerReference).Target as Action<object[]>)?.Invoke(
                    args
                    );
            }
            else
            {
                // Invoke the target handler for the event.
                ((Action<object[]>?)_innerReference)?.Invoke(args);
            }                
        }

        // *******************************************************************

        /// <summary>
        /// This method is called whenever the object is disposed.
        /// </summary>
        /// <param name="disposing">True to cleanup managed resources; False 
        /// otherwise.</param>
        public void Dispose()
        {
            // How should we deal with the inner reference?
            if (_innerReference is WeakReference)
            {
                // Release the reference.
                ((WeakReference)_innerReference).Target = null;
            }
            else
            {
                // Release the reference.
                _innerReference = null;
            }
        }

        #endregion
    }
}
