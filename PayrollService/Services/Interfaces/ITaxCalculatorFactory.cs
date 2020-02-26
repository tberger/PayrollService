using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollService.Services.Interfaces
{
    public interface ITaxCalculatorFactory
    {
        ICountryTaxCalculator GetTaxesDeductionCalculator(string countryCode);
    }
}
