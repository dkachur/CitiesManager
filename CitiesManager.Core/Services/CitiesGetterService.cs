using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services
{
    public class CitiesGetterService : ICitiesGetterService
    {
        private readonly ICitiesRepository _citiesRepository;

        public CitiesGetterService(ICitiesRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;
        }

        public async Task<City?> GetAsync(Guid cityId)
        {
            if (cityId == Guid.Empty)
                return null;
            //  throw new ArgumentException("City ID cannot be empty.", nameof(cityId));

            return await _citiesRepository.GetByIdAsync(cityId);
        }

        public async Task<List<City>> GetAllAsync()
        {
            return await _citiesRepository.GetAllAsync();
        }
    }
}
