using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.Results
{
    public class UpdateCityResult
    {
        public Status ResultStatus { get; set; }
        public City? City { get; set; }

        public static UpdateCityResult Success(City city) => new() { ResultStatus = Status.Success, City = city };
        public static UpdateCityResult Fail(Status status) => new() { ResultStatus = status, City = null };
    }
}
