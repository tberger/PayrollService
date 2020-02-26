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
    }
}
