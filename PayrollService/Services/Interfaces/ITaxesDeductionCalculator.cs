namespace PayrollService.Services.Interfaces
{
    public interface ITaxesDeductionCalculator
    {
        decimal CalculateTax(string countryCode, decimal hoursWorked, decimal hourlyRate);
    }
}
