using CitiesManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs
{
    /// <summary>
    /// DTO for adding a new city. Contains the information required to create a city entry.
    /// </summary>
    public class CityAddRequest
    {
        /// <summary>
        /// The name of the city to add. This field is required and cannot be empty or whitespace.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
