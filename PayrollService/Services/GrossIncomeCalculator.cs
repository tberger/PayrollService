using PayrollService.Services.Interfaces;

namespace PayrollService.Services
{
    public class GrossIncomeCalculator : IGrossIncomeCalculator
    {
        public decimal Calculate(decimal hoursWorked, decimal hourlyRate)
        {
            return hoursWorked * hourlyRate;
        }
    }
}