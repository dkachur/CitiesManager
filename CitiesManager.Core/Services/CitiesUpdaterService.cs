using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Core.Results;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services
{
    public class CitiesUpdaterService : ICitiesUpdaterService
    {
        private readonly ICitiesRepository _citiesRepository;

        public CitiesUpdaterService(ICitiesRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;
        }

        public async Task<UpdateCityResult> UpdateAsync(City city)
        {
            if (city.Id == Guid.Empty)
                return UpdateCityResult.Fail(Status.InvalidInput);

            var targetCity = await _citiesRepository.GetByIdAsync(city.Id);

            if (targetCity is null)
                return UpdateCityResult.Fail(Status.NotFound);
            else if (targetCity.Name == city.Name)
                return UpdateCityResult.Success(targetCity);

            if (await _citiesRepository.ExistsByNameAsync(city.Name))
                return UpdateCityResult.Fail(Status.NameAlreadyExists);

            var updatedCity = await _citiesRepository.UpdateAsync(city);

            return UpdateCityResult.Success(updatedCity);
        }
    }
}
