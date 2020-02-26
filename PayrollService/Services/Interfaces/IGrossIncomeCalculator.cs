namespace PayrollService.Services.Interfaces
{
    public interface IGrossIncomeCalculator
    {
        decimal Calculate(decimal hoursWorked, decimal hourlyRate);
    }
}
