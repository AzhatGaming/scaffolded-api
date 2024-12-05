using ScaffoldedApi;
using ScaffoldedApi.Interfaces;
using WebApiExample.Models;

namespace WebApiExample.Controllers
{
    public class CityController : ScaffoldedController<City>
    {
        public CityController(IScaffoldedDataService<City> dataService) : base(dataService)
        {

        }
    }
}
