using CG.Validations;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IApplicationBuilder"/>
    /// type
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method runs any startup logic required by the event aggregator.
        /// </summary>
        /// <param name="applicationBuilder">The application builder to use 
        /// for the oepration.</param>
        /// <param name="hostEnvironment">The host environment to use for the 
        /// operation.</param>
        /// <returns>The value of the <paramref name="applicationBuilder"/>
        /// parameter, for chaining calls together.</returns>
        public static IApplicationBuilder UseEventAggregator(
            this IApplicationBuilder applicationBuilder,
            IHostEnvironment hostEnvironment
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(applicationBuilder, nameof(applicationBuilder))
                .ThrowIfNull(hostEnvironment, nameof(hostEnvironment));

            // Nothing to do here, yet.

            // Return the application builder.
            return applicationBuilder;
        }

        #endregion
    }
}
