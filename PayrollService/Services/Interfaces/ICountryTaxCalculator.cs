namespace PayrollService.Services.Interfaces
{
    public interface ICountryTaxCalculator
    {
        decimal CalculateTaxesDeduction(decimal grossIncome);
    }
}