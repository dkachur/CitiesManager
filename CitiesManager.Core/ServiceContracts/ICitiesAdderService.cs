using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.ServiceContracts
{
    /// <summary>
    /// Defines a service for adding cities to the repository.
    /// </summary>
    /// <remarks>This service ensures that city names are unique within the system. Use the <see cref="Add"/>
    /// method to add a new city and retrieve the resulting city object.</remarks>
    public interface ICitiesAdderService
    {
        /// <summary>
        /// Adds a new city to the repository and returns the added city.
        /// </summary>
        /// <remarks>The city name must be unique within the repository.</remarks>
        /// <param name="cityName">The name of the city to add. Should not be null or empty.</param>
        /// <returns>The added city object.</returns>
        Task<City?> AddAsync(string cityName);
    }
}
