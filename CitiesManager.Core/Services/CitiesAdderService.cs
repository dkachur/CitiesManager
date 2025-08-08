using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.Exceptions;
using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services
{
    public class CitiesAdderService : ICitiesAdderService
    {
        private readonly ICitiesRepository _citiesRepository;

        public CitiesAdderService(ICitiesRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;
        }

        public async Task<City?> AddAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return null;
            //  throw new ArgumentException("City name cannot be empty.", nameof(cityName));

            if (await _citiesRepository.ExistsByNameAsync(cityName))
                return null;
            //  throw new DuplicateCityNameException("A city with the same name already exists in the storage.", nameof(cityName));

            City newCity = new() { Name = cityName, Id = Guid.NewGuid() };

            return await _citiesRepository.AddAsync(newCity);
        }
    }
}
