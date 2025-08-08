using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services
{
    public class CitiesDeleterService : ICitiesDeleterService
    {
        private readonly ICitiesRepository _citiesRepository;

        public CitiesDeleterService(ICitiesRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;
        }

        public async Task<bool> DeleteAsync(Guid cityId)
        {
            var city = await _citiesRepository.GetByIdAsync(cityId);

            if (city is null)
                return false;

            return await _citiesRepository.DeleteAsync(city);
        }
    }
}
