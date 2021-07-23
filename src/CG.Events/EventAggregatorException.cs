using System;
using System.Runtime.Serialization;

namespace CG.Events
{
    /// <summary>
    /// This class represents an event aggregator related exception.
    /// </summary>
    [Serializable]
    public class EventAggregatorException : Exception
    {
        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="EventAggregatorException"/>
        /// class.
        /// </summary>
        public EventAggregatorException()
        {

        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="EventAggregatorException"/>
        /// class.
        /// </summary>
        /// <param name="message">The message to use for the exception.</param>
        /// <param name="innerException">An optional inner exception reference.</param>
        public EventAggregatorException(
            string message,
            Exception innerException
            ) : base(message, innerException)
        {

        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="EventAggregatorException"/>
        /// class.
        /// </summary>
        /// <param name="message">The message to use for the exception.</param>
        public EventAggregatorException(
            string message
            ) : base(message)
        {

        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="EventAggregatorException"/>
        /// class.
        /// </summary>
        /// <param name="info">The serialization info to use for the exception.</param>
        /// <param name="context">The streaming context to use for the exception.</param>
        public EventAggregatorException(
            SerializationInfo info,
            StreamingContext context
            ) : base(info, context)
        {

        }

        #endregion
    }
}
