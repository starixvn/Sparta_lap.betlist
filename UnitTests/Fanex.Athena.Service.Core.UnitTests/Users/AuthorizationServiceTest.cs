namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Common.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using StatelessCore;
    using StatelessCore.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class AuthorizationServiceTest
    {
        private IAthorizationDataAccess _authorizationDataAccess;
        private IFunctionService _functionService;
        private IAuthorizationService _authorizationService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationDataAccess = Substitute.For<IAthorizationDataAccess>();
            _functionService = Substitute.For<IFunctionService>();
            _authorizationService = new AuthorizationService(_authorizationDataAccess, _functionService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _authorizationDataAccess = null;
            _functionService = null;
            _authorizationService = null;
        }

        [Test]
        public void HasPermissions_WhenCallWithFunctionIds_FunctionIsEmpty_ReturnFalse()
        {
            // Arrange
            var allFunctions = new List<Function>();
            var userFunctions = FakeHelper.CreateMany<int>(10);
            var permissionKey = FakeHelper.Create<string>();

            _functionService.Functions.Returns(allFunctions);

            // Action
            var result = _authorizationService.HasPermissions(userFunctions, permissionKey);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasPermissions_WhenCallWithFunctionIds_PermissionKeyIsNotInAllFunctions_ReturnFalse()
        {
            // Arrange
            var allFunctions = new List<Function> { new Function { FunctionKey = "RealKey" } };
            var userFunctions = FakeHelper.CreateMany<int>(10);
            var permissionKey = "FakeKey";

            _functionService.Functions.Returns(allFunctions);

            // Action
            var result = _authorizationService.HasPermissions(userFunctions, permissionKey);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasPermissions_WhenCallWithFunctionIds_UserFunctionIsNotInAllFunctions_ReturnFalse()
        {
            // Arrange
            var permissionKey = "Key";
            var userFunctions = new[] { 1, 2, 3 };
            var allFunctions = new List<Function> { new Function { FunctionKey = permissionKey, FunctionId = 4 } };

            _functionService.Functions.Returns(allFunctions);

            // Action
            var result = _authorizationService.HasPermissions(userFunctions, permissionKey);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasPermissions_WhenCallWithFunctionIds_ValidFunction_ReturnTrue()
        {
            // Arrange
            var permissionKey = "Key";
            var userFunctions = FakeHelper.CreateMany<int>(10);
            var allFunctions = new List<Function> { new Function { FunctionKey = permissionKey, FunctionId = userFunctions.ElementAt(0) } };

            _functionService.Functions.Returns(allFunctions);

            // Action
            var result = _authorizationService.HasPermissions(userFunctions, permissionKey);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasPermissions_WhenCallWithFunctionKey_UserFunctionsDoNotHavePermision_ReturnFalse()
        {
            // Arrange
            var userFunctions = new List<string> { "FakeFunction1", "FakeFunction2" };
            var permissionKeys = new[] { "RealFunction" };

            // Action
            var result = _authorizationService.HasPermissions(userFunctions, permissionKeys);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasPermissions_WhenCallWithFunctionKey_UserFunctionsHavePermision_ReturnTrue()
        {
            // Arrange
            var userFunctions = new List<string> { "Function1", "Function2" };
            var permissionKeys = new[] { "RealFunction", userFunctions[0] };

            // Action
            var result = _authorizationService.HasPermissions(userFunctions, permissionKeys);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasPermissionOnData_LicenseeIdIsZero_ThrowException()
        {
            // Arrange
            const int LicenseeId = 0;

            // Action
            TestDelegate testDelegate = () => _authorizationService.HasPermissionOnData(LicenseeId, 1, 1);

            // Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void HasPermissionOnData_CustIdAndTransIdAreZero_ReturnTrue()
        {
            // Arrange
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) + 1;
            const int CustId = 0;
            const int TransId = 0;

            // Action
            var result = _authorizationService.HasPermissionOnData(licenseeId, CustId, TransId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasPermissionOnData_AllParamsAreGreaterThanZero_ReturnFromDataAccess()
        {
            // Arrange
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) + 1;
            var custId = Math.Abs(FakeHelper.Create<int>()) + 1;
            var transId = Math.Abs(FakeHelper.Create<int>()) + 1;

            // Action
            var result = _authorizationService.HasPermissionOnData(licenseeId, custId, transId);

            // Assert
            _authorizationDataAccess.Received().IsCustomerBelonging(licenseeId, custId, transId);
        }
    }
}