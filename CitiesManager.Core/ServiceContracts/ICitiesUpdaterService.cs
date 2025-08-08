using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.Results;

namespace CitiesManager.Core.ServiceContracts
{
    /// <summary>
    /// Defines the contract for updating existing cities in the system.
    /// </summary>
    public interface ICitiesUpdaterService
    {
        /// <summary>
        /// Updates the details of an existing city and returns the updated entity.
        /// </summary>
        /// <param name="updatedCity">The city entity with updated information. Must include a valid City ID.</param>
        /// <returns>The <see cref="UpdateCityResult"/> object which contains details about result status and updated city object.</returns>
        Task<UpdateCityResult> UpdateAsync(City updatedCity);
    }
}
