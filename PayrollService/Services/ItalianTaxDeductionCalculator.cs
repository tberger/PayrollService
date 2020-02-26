using PayrollService.Services.Interfaces;

namespace PayrollService.Services
{
    public class ItalianTaxDeductionCalculator : ICountryTaxCalculator
    {
        public decimal CalculateTaxesDeduction(decimal grossIncome)
        {
            var tax = this.CalculateTax(grossIncome);
            var inps = this.CalculateInps(grossIncome);

            return tax + inps;
        }

        private decimal CalculateTax(decimal grossIncome)
        {
            return grossIncome * 0.25m;
        }

        private decimal CalculateInps(decimal grossIncome)
        {
            return grossIncome * 0.0919m;
        }
    }
}