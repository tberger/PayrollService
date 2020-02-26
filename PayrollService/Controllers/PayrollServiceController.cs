using PayrollService.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayrollService.Controllers
{
    public class PayrollServiceController : ApiController
    {
        private static readonly string[] _countryCodes = new string[] { "DEU", "ITA", "ESP" };

        [HttpGet]
        [Route("api/PayrollService/{countryCode}")]
        [ResponseType(typeof(IncomeInformation))]
        public IHttpActionResult Get(
            string countryCode,
            [FromUri]decimal hoursWorked,
            [FromUri]decimal hourlyRate)
        {
            if (!_countryCodes.Contains(countryCode))
            {
                return BadRequest($"Only {string.Join(", ", _countryCodes)} country codes are supported");
            }
            return Ok(new IncomeInformation
            {
                CountryCode = countryCode
            });
        }
    }
}
