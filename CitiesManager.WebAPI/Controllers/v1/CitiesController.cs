using Asp.Versioning;
using CitiesManager.Core.Results;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.WebAPI.Extensions.Mappers;
using CitiesManager.WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1
{
    /// <summary>
    /// Controller responsible for managing cities.
    /// Provides endpoints for adding, retrieving, updating, and deleting city records.
    /// </summary>
    [ApiVersion("1.0")]
    //[EnableCors("DefClient")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ICitiesAdderService _citiesAdderService;
        private readonly ICitiesGetterService _citiesGetterService;
        private readonly ICitiesUpdaterService _citiesUpdaterService;
        private readonly ICitiesDeleterService _citiesDeleterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitiesController"/> class.
        /// </summary>
        /// <param name="citiesAdderService">Service for adding new cities.</param>
        /// <param name="citiesGetterService">Service for retrieving existing cities.</param>
        /// <param name="citiesUpdaterService">Service for updating existing cities.</param>
        /// <param name="citiesDeleterService">Service for deleting existing cities</param>
        public CitiesController(ICitiesAdderService citiesAdderService,
            ICitiesGetterService citiesGetterService, ICitiesUpdaterService citiesUpdaterService,
            ICitiesDeleterService citiesDeleterService)
        {
            _citiesAdderService = citiesAdderService;
            _citiesGetterService = citiesGetterService;
            _citiesUpdaterService = citiesUpdaterService;
            _citiesDeleterService = citiesDeleterService;
        }

        /// <summary>
        /// Retrieves all cities.
        /// </summary>
        /// <returns>A collection of <see cref="CityResponse"/> containing details about all cities.</returns>
        // GET: api/Cities
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
        {
            var cities = await _citiesGetterService.GetAllAsync();

            return Ok(cities.Select(c => c.ToCityResponse()));
        }

        /// <summary>
        /// Retrieves the city with specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <returns>A <see cref="CityResponse"/> with city details if found; otherwise, <see cref="NotFoundResult"/>.</returns>
        // GET: api/Cities/5
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CityResponse>> GetCity(Guid id)
        {
            var city = await _citiesGetterService.GetAsync(id);

            if (city is null)
                return NotFound();

            return city.ToCityResponse();
        }

        /// <summary>
        /// Adds a new city to storage.
        /// </summary>
        /// <param name="request">A request containing details about the new city.</param>
        /// <returns>
        /// <see cref="CreatedAtActionResult"/> if successfully created;
        /// <see cref="ProblemDetails"/> with error message if city name is invalid or already exists;
        /// <see cref="ProblemDetails"/> with error message if unexpected error occurred.
        /// </returns>
        // POST api/Cities
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CityResponse>> Post([FromBody] CityAddRequest request)
        {
            var result = await _citiesAdderService.AddAsync(request.Name);

            return result.ResultStatus switch
            {
                Status.Success => CreatedAtAction(
                    nameof(GetCity), 
                    new { id = result.City!.Id }, 
                    result.City!.ToCityResponse()),

                Status.InvalidInput => Problem(
                    title: "The City name is not valid.",
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: "The city name cannot be null, empty, or contain only whitespace."),

                Status.NameAlreadyExists => Problem(
                    title: "The City name already exists",
                    statusCode: StatusCodes.Status409Conflict,
                    detail: "A city with the same name already exists in the storage."),

                _ => Problem(
                    title: "Unexpected error while adding the city.",
                    statusCode: StatusCodes.Status500InternalServerError)
            };
        }

        /// <summary>
        /// Updates an existing city.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <param name="request">
        /// A request containing details for updating the city.
        /// <see cref="CityUpdateRequest.Id"/> must match with the <paramref name="id"/> from route.
        /// </param>
        /// <returns>
        /// <see cref="OkObjectResult"/> with the updated city if updation is successful;
        /// <see cref="NotFoundResult"/> if city with specified id does not exist;
        /// <see cref="ConflictResult"/> if city with the same name already exists;
        /// <see cref="ProblemDetails"/> with error message if unexpected error occurred.
        /// </returns>
        // PUT api/<CitiesController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, CityUpdateRequest request)
        {
            if (id != request.Id)
                return Problem(
                    title: "City ID mismatch.",
                    detail: $"The City ID in the route ('{id}') does not match the City ID in the request body ('{request.Id}').",
                    statusCode: StatusCodes.Status400BadRequest
                );

            var updatedCity = await _citiesUpdaterService.UpdateAsync(request.ToCity());

            return updatedCity.ResultStatus switch
            {
                Status.Success => Ok(updatedCity.City),
                Status.InvalidInput => Problem(
                    title: "The City ID is empty.",
                    detail: "The City ID in the request body must not be empty",
                    statusCode: StatusCodes.Status400BadRequest),
                Status.NotFound => NotFound(),
                Status.NameAlreadyExists => Conflict("City with the same name already exists"),
                _ => Problem(
                    title: "Unexpected error while adding the city.",
                    statusCode: StatusCodes.Status500InternalServerError)
            };
        }

        /// <summary>
        /// Deletes the city with specified id.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <returns>
        /// <see cref="NoContentResult"/> if deletion is successful;
        /// otherwise, <see cref="NotFoundResult"/>.
        /// </returns>
        // DELETE api/<CitiesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _citiesDeleterService.DeleteAsync(id);

            return deleted ? NoContent() : NotFound();
        }
    }
}
