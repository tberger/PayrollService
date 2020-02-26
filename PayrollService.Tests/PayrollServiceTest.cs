using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollService.Controllers;

namespace PayrollService.Tests
{
    [TestClass]
    public class PayrollServiceTest
    {
        [TestMethod]
        public void ThereShouldBeAPayrollServiceController()
        {
            // Arrange
            var controller = new PayrollServiceController();

            // Assert
            Assert.IsNotNull(controller);
        }
    }
}
