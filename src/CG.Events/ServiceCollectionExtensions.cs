using CG.Events;
using CG.Validations;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IServiceCollection"/>
    /// type.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method registers the services required to support event 
        /// aggregation.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use for
        /// the operation.</param>
        /// <returns>the value of the <paramref name="serviceCollection"/>
        /// parameter. for chaining calls together.</returns>
        public static IServiceCollection AddEventAggregation(
            this IServiceCollection serviceCollection
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection));

            // Register the service.
            serviceCollection.AddSingleton<IEventAggregator, EventAggregator>();

            // Return the service collection.
            return serviceCollection;
        }

        #endregion
    }
}
