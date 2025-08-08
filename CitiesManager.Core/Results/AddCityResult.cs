using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.Results
{
    public class AddCityResult
    {
        public Status ResultStatus { get; set; }
        public City? City { get; set; }

        public static AddCityResult Success(City city) => new() { ResultStatus = Status.Success, City = city };
        public static AddCityResult Fail(Status status) => new() { ResultStatus = status, City = null };
    }
}
