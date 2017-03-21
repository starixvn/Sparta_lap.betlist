namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Collections.Generic;
    using DataAccess.Customer.Interfaces;
    using Model.Customer.DTO.CustomerLog;
    using Model.Customer.MetaModel.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerLogServiceTest
    {
        private ICustomerLogDataAccess _customerLogDataAccess;
        private CustomerLogService _customerLogService;

        [SetUp]
        public void SetUp()
        {
            _customerLogDataAccess = Substitute.For<ICustomerLogDataAccess>();
            _customerLogService = new CustomerLogService(_customerLogDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _customerLogDataAccess = null;
            _customerLogService = null;
        }

        [Test]
        public void GetCustomerSettingList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new List<CustomerLog> { new CustomerLog { LogId = 1 }, new CustomerLog { LogId = 2 } };
            _customerLogDataAccess.GetCustomerSettingList(Arg.Any<CustomerLogMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerLogService.GetCustomerSettingList(new CustomerLogMetaModel());

            // Assert
            Assert.That(result[0].LogId, Is.EqualTo(2));
            Assert.That(result[1].LogId, Is.EqualTo(1));
        }

        [Test]
        public void GetGroupCommList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new List<CustomerLog> { new CustomerLog { LogId = 1 }, new CustomerLog { LogId = 2 } };
            _customerLogDataAccess.GetGroupCommList(Arg.Any<CustomerLogMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerLogService.GetGroupCommList(new CustomerLogMetaModel());

            // Assert
            Assert.That(result[0].LogId, Is.EqualTo(1));
            Assert.That(result[1].LogId, Is.EqualTo(2));
        }
    }
}