namespace CitiesManager.WebAPI.Models.DTOs
{
    /// <summary>
    /// DTO for sending responses with city details.
    /// </summary>
    public class CityResponse
    {
        /// <summary>
        /// The unique identifier of the city.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the city.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
