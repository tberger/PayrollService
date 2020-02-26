using PayrollService.Models;
using PayrollService.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayrollService.Controllers
{
    public class PayrollServiceController : ApiController
    {
        private static readonly string[] _countryCodes = new string[] { "DEU", "ITA", "ESP" };
        private readonly IGrossIncomeCalculator _grossIncomeCalculator;
        private readonly ITaxesDeductionCalculator _taxesDeductionCalculator;

        public PayrollServiceController(
            IGrossIncomeCalculator grossIncomeCalculator,
            ITaxesDeductionCalculator taxesDeductionCalculator)
        {
            _grossIncomeCalculator = grossIncomeCalculator;
            _taxesDeductionCalculator = taxesDeductionCalculator;
        }

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
            var grossIncome = _grossIncomeCalculator.CalculateGrossIncome(
                hoursWorked, 
                hourlyRate);
            decimal taxesDeduction = _taxesDeductionCalculator.CalculateTax(
                countryCode, 
                hoursWorked, 
                hourlyRate);
            return Ok(new IncomeInformation
            {
                CountryCode = countryCode,
                GrossIncome = grossIncome,
                TaxesDeduction = taxesDeduction
            });
        }
    }
}
