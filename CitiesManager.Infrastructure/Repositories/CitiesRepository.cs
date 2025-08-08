using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.Repositories
{
    public class CitiesRepository : ICitiesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CitiesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<City> AddAsync(City city)
        {
            await _dbContext.Cities.AddAsync(city);
            await _dbContext.SaveChangesAsync();

            return city;
        }

        public async Task<bool> DeleteAsync(City city)
        {
            _dbContext.Cities.Remove(city);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid cityId)
        {
            return await _dbContext.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<bool> ExistsByNameAsync(string cityName)
        {
            return await _dbContext.Cities.AnyAsync(c => c.Name.ToLower() == cityName.ToLower());
        }

        public async Task<List<City>> GetAllAsync()
        {
            return await _dbContext.Cities.AsNoTracking().ToListAsync();
        }

        public async Task<City?> GetByIdAsync(Guid cityId)
        {
            return await _dbContext.Cities.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<City?> GetByNameAsync(string cityName)
        {
            return await _dbContext.Cities.AsNoTracking().FirstOrDefaultAsync(c => string.Equals(c.Name, cityName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<City> UpdateAsync(City city)
        {
            _dbContext.Cities.Update(city);
            await _dbContext.SaveChangesAsync();

            return city;
        }
    }
}
