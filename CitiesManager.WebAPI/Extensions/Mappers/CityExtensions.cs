using CitiesManager.Core.Domain.Entities;
using CitiesManager.WebAPI.Models.DTOs;

namespace CitiesManager.WebAPI.Extensions.Mappers
{
    /// <summary>
    /// Provides extension methods for converting <see cref="City"/> object into other objects.
    /// </summary>
    public static class CityExtensions
    {
        /// <summary>
        /// Converts the <see cref="City"/> object into the <see cref="CityResponse"/> object
        /// </summary>
        /// <param name="city">The instance of the city to convert.</param>
        /// <returns>The converted <see cref="CityResponse"/> object.</returns>
        public static CityResponse ToCityResponse(this City city)
        {
            return new()
            {
                Id = city.Id,
                Name = city.Name,
            };
        }
    }
}
