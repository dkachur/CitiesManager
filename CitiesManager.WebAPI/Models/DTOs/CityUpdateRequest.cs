using CitiesManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs
{
    /// <summary>
    /// DTO for updating the existing city.
    /// </summary>
    public class CityUpdateRequest
    {
        /// <summary>
        /// The unique identifier of the city. This field cannot be null or empty.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the city. This field cannot be null or empty.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Converts <see cref="CityUpdateRequest"/> object into the <see cref="City"/> object.
        /// </summary>
        /// <returns>The converted <see cref="City"/> object.</returns>
        public City ToCity()
        {
            return new City { Id = Id, Name = Name };
        }
    }
}
