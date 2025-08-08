using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing City entities.
    /// </summary>
    public interface ICitiesRepository
    {
        /// <summary>
        /// Inserts a new city object into the storage.
        /// </summary>
        /// <param name="city">City to insert.</param>
        /// <returns>The added <see cref="City"/> object.</returns>
        Task<City> AddAsync(City city);

        /// <summary>
        /// Retrieves a city object with specified ID from storage.
        /// </summary>
        /// <param name="cityId">ID of a city to retrieve.</param>
        /// <returns>The <see cref="City"/> with specified ID if found; otherwise, <c>null</c>.</returns>
        Task<City?> GetByIdAsync(Guid cityId);

        /// <summary>
        /// Retrieves a city object with specified name from storage.
        /// </summary>
        /// <param name="cityName">Name of a city to retrieve.</param>
        /// <returns>The <see cref="City"/> with specified name if found; otherwise, <c>null</c>.</returns>
        Task<City?> GetByNameAsync(string cityName);

        /// <summary>
        /// Retrieves all city objects from storage.
        /// </summary>
        /// <returns>The list of <see cref="City"/> objects.</returns>
        Task<List<City>> GetAllAsync();

        /// <summary>
        /// Updates a city record in storage.
        /// </summary>
        /// <param name="city">The updated city object. Must contain a valid ID of an existing city.</param>
        /// <returns>The updated <see cref="City"/> object.</returns>
        Task<City> UpdateAsync(City city);

        /// <summary>
        /// Deletes a city record in storage.
        /// </summary>
        /// <param name="city">The city object to delete.</param>
        /// <returns><c>true</c> if deletion is successful; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(City city);

        /// <summary>
        /// Indicates whether a city record with specified ID exists in the storage.
        /// </summary>
        /// <param name="cityId">The ID of the city to check.</param>
        /// <returns><c>true</c> if it exists; otherwise, <c>false</c>.</returns>
        Task<bool> ExistsByIdAsync(Guid cityId);

        /// <summary>
        /// Indicates whether a city record with specified name exists in the storage.
        /// </summary>
        /// <param name="cityName">The name of the city to check.</param>
        /// <returns><c>true</c> if it exists; otherwise, <c>false</c>.</returns>
        Task<bool> ExistsByNameAsync(string cityName);
    }
}
