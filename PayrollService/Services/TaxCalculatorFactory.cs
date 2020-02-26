using PayrollService.Services.Interfaces;
using System;

namespace PayrollService.Services
{
    public class TaxCalculatorFactory : ITaxCalculatorFactory
    {
        public ICountryTaxCalculator GetTaxesDeductionCalculator(string countryCode)
        {
            switch (countryCode)
            {
                case "ITA":
                    return new ItalianTaxDeductionCalculator();
                case "ESP":
                    return new SpainTaxDeductionCalculator();
                case "DEU":
                default:
                    throw new NotImplementedException();
            }
        }
    }
}