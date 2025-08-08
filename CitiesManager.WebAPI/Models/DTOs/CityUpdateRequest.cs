using CitiesManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs
{
    public class CityUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public City ToCity()
        {
            return new City { Id = Id, Name = Name };
        }
    }
}
