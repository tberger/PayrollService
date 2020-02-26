using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollService.Controllers;
using PayrollService.Models;
using System.Web.Http.Results;

namespace PayrollService.Tests
{
    [TestClass]
    public class PayrollServiceTest
    {
        [TestMethod]
        public void PayrollService_ShouldExist()
        {
            // Arrange
            var controller = new PayrollServiceController();

            // Assert
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void PayrollService_ShouldHaveAGetMethod()
        {
            // Arrange
            var controller = new PayrollServiceController();

            // Acr
            var result = controller.Get("DEU", 10m, 10m);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PayrollService_ShouldReturnCorrectType()
        {
            // Arrange
            var controller = new PayrollServiceController();

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
            var controller = new PayrollServiceController();

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
            var controller = new PayrollServiceController();

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
            var controller = new PayrollServiceController();

            // Acr
            var result = controller.Get("DEU", (decimal)hoursWorked, (decimal)hourlyRate) 
                as OkNegotiatedContentResult<IncomeInformation>;

            // Assert
            Assert.AreEqual((decimal)expectedGrossIncome, result.Content.GrossIncome);
        }
    }
}
