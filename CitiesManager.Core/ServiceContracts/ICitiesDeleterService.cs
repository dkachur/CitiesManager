namespace CitiesManager.Core.ServiceContracts
{
    /// <summary>
    /// Defines the contract for deleting existing cities in the repository.
    /// </summary>
    public interface ICitiesDeleterService
    {
        /// <summary>
        /// Deletes the existing record of a city and returns a boolean indicating success of the operation.
        /// </summary>
        /// <param name="cityId">The <see cref="Guid"/> identifying the city to delete. Must be a valid City ID.</param>
        /// <returns><c>true</c> if deletion was successful; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid cityId);
    }
}
