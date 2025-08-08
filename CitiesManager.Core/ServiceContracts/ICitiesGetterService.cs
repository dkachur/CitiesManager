using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.ServiceContracts
{
    /// <summary>
    /// Defines a service for retrieving city information.
    /// </summary>
    /// <remarks>This interface provides methods to retrieve details of a specific city by its unique
    /// identifier or to retrieve a list of all available cities. Implementations of this service are expected to handle
    /// data retrieval from the underlying data source.</remarks>
    public interface ICitiesGetterService
    {
        /// <summary>
        /// Retrieves the details of a city based on the specified city identifier.
        /// </summary>
        /// <param name="cityId">The unique identifier of the city to retrieve. Should not be null or empty.</param>
        /// <returns>The <see cref="City"/> object corresponding to the specified identifier. If there is no object with given ID, returns <c>null</c>.</returns>
        Task<City?> GetAsync(Guid cityId);

        /// <summary>
        /// Retrieves a list of all cities.
        /// </summary>
        /// <returns>The list of objects representing all cities.</returns>
        Task<List<City>> GetAllAsync();
    }
}
