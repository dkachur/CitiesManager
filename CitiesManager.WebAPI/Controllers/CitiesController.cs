using CitiesManager.Core.Results;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.WebAPI.Extensions.Mappers;
using CitiesManager.WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CitiesManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesAdderService _citiesAdderService;
        private readonly ICitiesGetterService _citiesGetterService;
        private readonly ICitiesUpdaterService _citiesUpdaterService;
        private readonly ICitiesDeleterService _citiesDeleterService;

        public CitiesController(ICitiesAdderService citiesAdderService,
            ICitiesGetterService citiesGetterService, ICitiesUpdaterService citiesUpdaterService,
            ICitiesDeleterService citiesDeleterService)
        {
            _citiesAdderService = citiesAdderService;
            _citiesGetterService = citiesGetterService;
            _citiesUpdaterService = citiesUpdaterService;
            _citiesDeleterService = citiesDeleterService;
        }


        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
        {
            var cities = await _citiesGetterService.GetAllAsync();

            return Ok(cities.Select(c => c.ToCityResponse()));
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CityResponse>> GetCity(Guid id)
        {
            var city = await _citiesGetterService.GetAsync(id);

            if (city is null)
                return NotFound();

            return city.ToCityResponse();
        }

        // POST api/Cities
        [HttpPost]
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
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: "A city with the same name already exists in the storage."),

                _ => Problem(
                    title: "Unexpected error while addind the city.",
                    statusCode: StatusCodes.Status500InternalServerError)
            };
        }

        // PUT api/<CitiesController>/5
        [HttpPut("{id}")]
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
                _ => StatusCode(500)
            };
        }

        // DELETE api/<CitiesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _citiesDeleterService.DeleteAsync(id);

            return deleted ? NoContent() : NotFound();
        }
    }
}
