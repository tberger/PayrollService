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
                    builder.RegisterType<GrossIncomeCalculator>().As<IGrossIncomeCalculator>();

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
    }
}
