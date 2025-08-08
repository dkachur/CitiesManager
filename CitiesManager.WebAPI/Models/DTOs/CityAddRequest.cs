using CitiesManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs
{
    public class CityAddRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
