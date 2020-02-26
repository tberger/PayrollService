using PayrollService.Services.Interfaces;

namespace PayrollService.Services
{
    public class GermanTaxDeductionCalculator : ICountryTaxCalculator
    {
        public decimal CalculateTaxesDeduction(decimal grossIncome)
        {
            var tax = this.CalculateTax(grossIncome);
            var pensionContribution = this.CalculatePensionContribution(grossIncome);

            return tax + pensionContribution;
        }

        private decimal CalculateTax(decimal grossIncome)
        {
            if (grossIncome <= 400)
            {
                return grossIncome * 0.25m;
            }
            return 400 * 0.25m + (grossIncome - 400) * 0.32m;
        }

        private decimal CalculatePensionContribution(decimal grossIncome)
        {
            return grossIncome * 0.02m;
        }
    }
}