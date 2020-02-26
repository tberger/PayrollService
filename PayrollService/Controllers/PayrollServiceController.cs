using PayrollService.Models;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayrollService.Controllers
{
    public class PayrollServiceController : ApiController
    {
        [HttpGet]
        [Route("api/PayrollService/{countryCode}")]
        [ResponseType(typeof(IncomeInformation))]
        public IHttpActionResult Get(
            string countryCode, 
            [FromUri]decimal hoursWorked, 
            [FromUri]decimal hourlyRate)
        {
            return Ok(new IncomeInformation());
        }
    }
}
