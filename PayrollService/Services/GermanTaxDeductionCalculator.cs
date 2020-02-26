using PayrollService.Services.Interfaces;

namespace PayrollService.Services
{
    public class GermanTaxDeductionCalculator : ICountryTaxCalculator
    {
        public decimal CalculateTaxesDeduction(decimal grossIncome)
        {
            var tax = 0;

            return tax;
        }
    }
}