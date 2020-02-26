using PayrollService.Services.Interfaces;

namespace PayrollService.Services
{
    public class IncomeCalculator : IGrossIncomeCalculator, ITaxesDeductionCalculator
    {
        public decimal CalculateGrossIncome(decimal hoursWorked, decimal hourlyRate)
        {
            return hoursWorked * hourlyRate;
        }

        public decimal CalculateTax(string countryCode, decimal hoursWorked, decimal hourlyRate)
        {
            decimal taxesDeduction = 0;
            var grossIncome = this.CalculateGrossIncome(hoursWorked, hourlyRate);
            if (countryCode.Equals("ESP"))
            {
                taxesDeduction = this.CalculateSpainTaxesDeduction(grossIncome);
            }
            else if (countryCode.Equals("ITA"))
            {
                taxesDeduction = this.ClculateItalianTaxesDeduction(grossIncome);
            }

            return taxesDeduction;
        }


        private decimal ClculateItalianTaxesDeduction(decimal grossIncome)
        {
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

        private decimal CalculateSpainTaxesDeduction(decimal grossIncome)
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