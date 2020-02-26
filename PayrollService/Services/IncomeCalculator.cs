using PayrollService.Services.Interfaces;

namespace PayrollService.Services
{
    public class IncomeCalculator : IGrossIncomeCalculator, ITaxesDeductionCalculator
    {
        private readonly ITaxCalculatorFactory _taxCalculatorFactory;

        public IncomeCalculator(ITaxCalculatorFactory taxCalculatorFactory)
        {
            _taxCalculatorFactory = taxCalculatorFactory;
        }

        public decimal CalculateGrossIncome(decimal hoursWorked, decimal hourlyRate)
        {
            return hoursWorked * hourlyRate;
        }

        public decimal CalculateTax(string countryCode, decimal hoursWorked, decimal hourlyRate)
        {
            var grossIncome = this.CalculateGrossIncome(hoursWorked, hourlyRate);

            decimal taxesDeduction = _taxCalculatorFactory
                .GetTaxesDeductionCalculator(countryCode)
                .CalculateTaxesDeduction(grossIncome);

            return taxesDeduction;
        }
    }
}