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

        public PayrollServiceController(
            IGrossIncomeCalculator grossIncomeCalculator)
        {
            _grossIncomeCalculator = grossIncomeCalculator;
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
            decimal taxesDeduction = 0;
            if (countryCode.Equals("ESP"))
            {
                taxesDeduction = this.CalculateSpainTaxesDeduction(hourlyRate, hoursWorked);
            }
            else if (countryCode.Equals("ITA"))
            {
                taxesDeduction = this.ClculateItalianTaxesDeduction(hourlyRate, hoursWorked);
            }
            return Ok(new IncomeInformation
            {
                CountryCode = countryCode,
                GrossIncome = _grossIncomeCalculator.Calculate(hoursWorked, hourlyRate),
                TaxesDeduction = taxesDeduction
            });
        }

        private decimal ClculateItalianTaxesDeduction(decimal hourlyRate, decimal hoursWorked)
        {
            var grossIncome = _grossIncomeCalculator.Calculate(hourlyRate, hoursWorked);
            var tax = this.CalculateItalianTax(grossIncome);
            var inps = this.CalculateItalianInps(grossIncome);

            return tax + inps;
        }

        private decimal CalculateItalianTax(decimal grossIncome)
        {
            return grossIncome * 0.25m;
        }

        private decimal CalculateItalianInps(decimal grossIncome)
        {
            return grossIncome * 0.0919m;
        }

        private decimal CalculateSpainTaxesDeduction(decimal hourlyRate, decimal hoursWorked)
        {
            var grossIncome = _grossIncomeCalculator.Calculate(hourlyRate, hoursWorked);
            var tax = this.CalculateSpainTax(grossIncome);
            var socialCharge = this.CalculateSpainSocialCharge(grossIncome);
            var pensionContribution = this.CalculateSpainPensionContribution(grossIncome);

            return tax + socialCharge+ pensionContribution;
        }

        private decimal CalculateSpainTax(decimal grossIncome)
        {
            if(grossIncome <= 600)
            {
                return grossIncome * 0.25m;
            }
            return 600 * 0.25m + (grossIncome - 600) * 0.4m;
        }

        private decimal CalculateSpainSocialCharge(decimal grossIncome)
        {
            if (grossIncome <= 500)
            {
                return grossIncome * 0.07m;
            }
            return (500 * 0.07m) + ((grossIncome - 500) * 0.08m);
        }

        private decimal CalculateSpainPensionContribution(decimal grossIncome)
        {
            return grossIncome * 0.04m;
        }
    }
}
