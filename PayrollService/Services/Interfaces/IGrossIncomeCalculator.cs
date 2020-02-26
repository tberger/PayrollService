namespace PayrollService.Services.Interfaces
{
    public interface IGrossIncomeCalculator
    {
        decimal CalculateGrossIncome(decimal hoursWorked, decimal hourlyRate);
    }
}
