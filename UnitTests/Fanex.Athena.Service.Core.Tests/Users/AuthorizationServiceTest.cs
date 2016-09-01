namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using Core.Common.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Users;
    using Users.Interfaces;

    [TestFixture]
    public class AuthorizationServiceTest
    {
        private IAthorizationDataAccess _authorizationDataAccess;
        private IFunctionService _functionService;
        private IAuthorizationService _authorizationService;

        [SetUp]
        public void SetUp()
        {
            _authorizationDataAccess = Substitute.For<IAthorizationDataAccess>();
            _functionService = Substitute.For<IFunctionService>();
            _authorizationService = new AuthorizationService(_authorizationDataAccess, _functionService);
        }

        [TestCase(1, "Function1", true)]
        [TestCase(1, "Function2", false)]
        [TestCase(1, "Function3", false)]
        public void HasPermissions_WhenCall_ReturnCorrectResult(int userFunction, string permissionKey, bool expectedResult)
        {
            // Arrange
            var allFunctions = new List<Function>()
            {
                new Function() { FunctionId = 1, FunctionKey = "Function1" },
                new Function() { FunctionId = 2, FunctionKey = "Function2" }
            };
            var userFunctions = new int[] { userFunction };
            var permissionKeys = new string[] { permissionKey };
            _functionService.Functions.Returns(allFunctions);

            // Action
            var result = _authorizationService.HasPermissions(userFunctions, permissionKeys);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void HasPermissionOnData_LicenseeIdIsZero_ThrowException()
        {
            // Arrange
            var licenseeId = 0;

            // Action
            TestDelegate testDelegate = () => _authorizationService.HasPermissionOnData(licenseeId, 1, 1);

            // Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void HasPermissionOnData_CustIdAndTransIdAreZero_ReturnTrue()
        {
            // Action
            var result = _authorizationService.HasPermissionOnData(1, 0, 0);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasPermissionOnData_AllParamsIsGreaterThanZero_CallToDataAccess()
        {
            // Action
            var result = _authorizationService.HasPermissionOnData(1, 1, 1);

            // Assert
            _authorizationDataAccess.Received().IsCustomerBelonging(1, 1, 1);
        }
    }
}