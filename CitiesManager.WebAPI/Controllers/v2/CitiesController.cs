using Asp.Versioning;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.WebAPI.Models.DTOs.City;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v2
{
    /// <summary>
    /// Controller responsible for managing cities.
    /// Provides endpoints for retrieving city records.
    /// </summary>
    [ApiVersion("2.0")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ICitiesGetterService _citiesGetterService;


        /// <summary>
        /// Initializes a new instance of the <see cref="CitiesController"/> class.
        /// </summary>
        /// <param name="citiesGetterService">Service for retrieving existing cities.</param>
        public CitiesController(ICitiesGetterService citiesGetterService)
        {
            _citiesGetterService = citiesGetterService;
        }

        /// <summary>
        /// Retrieves all cities.
        /// </summary>
        /// <returns>A collection of <see cref="CityResponse"/> containing details about all cities.</returns>
        // GET: api/Cities
        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> GetCities()
        {
            var cities = await _citiesGetterService.GetAllAsync();

            return Ok(cities.Select(c => c.Name));
        }

        /// <summary>
        /// Retrieves the city with specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <returns>A <see cref="CityResponse"/> with city details if found; otherwise, <see cref="NotFoundResult"/>.</returns>
        // GET: api/Cities/5
        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetCity(Guid id)
        {
            var city = await _citiesGetterService.GetAsync(id);

            if (city is null)
                return NotFound();

            return city.Name;
        }
    }
}
