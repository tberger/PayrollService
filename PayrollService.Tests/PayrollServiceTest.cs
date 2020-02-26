using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollService.Controllers;
using PayrollService.Models;
using PayrollService.Services;
using PayrollService.Services.Interfaces;
using System.Web.Http.Results;

namespace PayrollService.Tests
{
    [TestClass]
    public class PayrollServiceTest
    {
        private IContainer _autofacContainer;
        protected IContainer AutofacContainer
        {
            get
            {
                if (_autofacContainer == null)
                {
                    var builder = new ContainerBuilder();
                    builder.RegisterType<PayrollServiceController>().As<PayrollServiceController>();
                    builder.RegisterType<IncomeCalculator>().As<IGrossIncomeCalculator>();
                    builder.RegisterType<IncomeCalculator>().As<ITaxesDeductionCalculator>();
                    builder.RegisterType<TaxCalculatorFactory>().As<ITaxCalculatorFactory>();

                    var container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        protected PayrollServiceController PayrollServiceController
        {
            get
            {
                return AutofacContainer.Resolve<PayrollServiceController>();
            }
        }

        [TestMethod]
        public void PayrollService_ShouldExist()
        {
            // Arrange
            var controller = PayrollServiceController;

            // Assert
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void PayrollService_ShouldHaveAGetMethod()
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get("DEU", 10m, 10m);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PayrollService_ShouldReturnCorrectType()
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get("DEU", 10m, 10m) ;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IncomeInformation>));
        }

        [TestMethod]
        [DataRow("FRA")]
        [DataRow("BEL")]
        public void PayrollService_ShouldOnlyAcceptThreeCountryCodes(string countryCode)
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get(countryCode, 10m, 10m);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        [DataRow("DEU")]
        [DataRow("ITA")]
        [DataRow("ESP")]
        public void PayrollService_ShouldReturnGivenCountryCode(string countryCode)
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get(countryCode, 10m, 10m) as OkNegotiatedContentResult<IncomeInformation>;

            // Assert
            Assert.AreEqual(countryCode, result.Content.CountryCode);
        }

        [TestMethod]
        [DataRow(10, 10, 100)]
        [DataRow(20, 20, 400)]
        [DataRow(40, 100, 4000)]
        public void PayrollService_ShouldReturnGrossIncome(
            double hoursWorked, 
            double hourlyRate, 
            double expectedGrossIncome)
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get("DEU", (decimal)hoursWorked, (decimal)hourlyRate) 
                as OkNegotiatedContentResult<IncomeInformation>;

            // Assert
            Assert.AreEqual((decimal)expectedGrossIncome, result.Content.GrossIncome);
        }

        /// <summary>
        /// 10 hoursWorked, 10 hourlyRate:
        /// grossIncome = 100,
        /// tax = 25
        /// social charge = 7
        /// pension contribution = 4
        /// 
        /// 30 hoursWorked, 20 hourlyRate:
        /// grossIncome = 600,
        /// tax = 600 * 0,25 = 150
        /// social charge = (500 * 0,07)35 + (100 * 0,08)8
        /// pension contribution = (600 * 0,04)24
        /// </summary>
        /// <param name="hoursWorked"></param>
        /// <param name="hourlyRate"></param>
        /// <param name="expectedTaxesDecuction"></param>
        [TestMethod]
        [DataRow(10, 10, 36)]
        [DataRow(30, 20, 217)]
        public void PayrollService_SouldCalculateSpanishTaxRates(
            double hoursWorked,
            double hourlyRate,
            double expectedTaxesDecuction)
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get("ESP", (decimal)hoursWorked, (decimal)hourlyRate)
                as OkNegotiatedContentResult<IncomeInformation>;

            // Assert
            Assert.AreEqual((decimal)expectedTaxesDecuction, result.Content.TaxesDeduction);
        }

        /// <summary>
        /// 10 hoursWorked, 10 hourlyRate:
        /// grossIncome = 100,
        /// tax = 25
        /// inps = 9,19
        /// 
        /// 30 hoursWorked, 20 hourlyRate:
        /// grossIncome = 600,
        /// tax = 600 * 0,25 = 150
        /// inps = 600 * 0,0919 = 55,14
        /// </summary>
        /// <param name="hoursWorked"></param>
        /// <param name="hourlyRate"></param>
        /// <param name="expectedTaxesDecuction"></param>
        [TestMethod]
        [DataRow(10, 10, 34.19)]
        [DataRow(30, 20, 205.14)]
        public void PayrollService_SouldCalculateItalianTaxRates(
            double hoursWorked,
            double hourlyRate,
            double expectedTaxesDecuction)
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get("ITA", (decimal)hoursWorked, (decimal)hourlyRate)
                as OkNegotiatedContentResult<IncomeInformation>;

            // Assert
            Assert.AreEqual((decimal)expectedTaxesDecuction, result.Content.TaxesDeduction);
        }

        /// <summary>
        /// 10 hoursWorked, 10 hourlyRate:
        /// grossIncome = 100,
        /// tax = 25
        /// pension = 2
        /// 
        /// 30 hoursWorked, 20 hourlyRate:
        /// grossIncome = 600,
        /// tax = (400*0,25)100 + (200*0,32)64
        /// pension = 600 * 0,02 = 12
        /// </summary>
        /// <param name="hoursWorked"></param>
        /// <param name="hourlyRate"></param>
        /// <param name="expectedTaxesDecuction"></param>
        [TestMethod]
        [DataRow(10, 10, 27)]
        [DataRow(30, 20, 176)]
        public void PayrollService_SouldCalculateGermanTaxRates(
            double hoursWorked,
            double hourlyRate,
            double expectedTaxesDecuction)
        {
            // Arrange
            var controller = PayrollServiceController;

            // Acr
            var result = controller.Get("DEU", (decimal)hoursWorked, (decimal)hourlyRate)
                as OkNegotiatedContentResult<IncomeInformation>;

            // Assert
            Assert.AreEqual((decimal)expectedTaxesDecuction, result.Content.TaxesDeduction);
        }
    }
}
