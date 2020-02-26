using PayrollService.Services.Interfaces;

namespace PayrollService.Services
{
    public class SpainTaxDeductionCalculator : ICountryTaxCalculator
    {
        public decimal CalculateTaxesDeduction(decimal grossIncome)
        {
            var tax = this.CalculateSpainTax(grossIncome);
            var socialCharge = this.CalculateSpainSocialCharge(grossIncome);
            var pensionContribution = this.CalculateSpainPensionContribution(grossIncome);

            return tax + socialCharge + pensionContribution;
        }

        private decimal CalculateSpainTax(decimal grossIncome)
        {
            if (grossIncome <= 600)
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