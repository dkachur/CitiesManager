using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.Results;

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
        /// <returns>The <see cref="AddCityResult"/> object which contains details about status of the opearation and added <see cref="City"/> object.</returns>
        Task<AddCityResult> AddAsync(string cityName);
    }
}
