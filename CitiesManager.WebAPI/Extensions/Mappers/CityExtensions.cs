using CitiesManager.Core.Domain.Entities;
using CitiesManager.WebAPI.Models.DTOs;

namespace CitiesManager.WebAPI.Extensions.Mappers
{
    public static class CityExtensions
    {
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
