namespace PayrollService.Models
{
    public class IncomeInformation
    {
        public string CountryCode { get; set; }

        public decimal GrossIncome { get; set; }

        public decimal NetIncome { get; set; }

        public decimal TaxesDeduction { get; set; }
    }
}