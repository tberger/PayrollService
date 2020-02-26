using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollService.Controllers;

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
            var result = controller.Get("DEU");

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
