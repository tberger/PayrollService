using System.Web.Http;

namespace PayrollService.Controllers
{
    public class PayrollServiceController : ApiController
    {
        [HttpGet]
        [Route("api/PayrollService/{countryCode}")]
        public IHttpActionResult Get(string countryCode)
        {
            return Ok();
        }
    }
}
