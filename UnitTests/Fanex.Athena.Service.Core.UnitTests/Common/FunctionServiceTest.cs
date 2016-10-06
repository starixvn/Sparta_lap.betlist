namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class FunctionServiceTest
    {
        private readonly IReadOnlyCollection<Function> _functionList =
            new ReadOnlyCollection<Function>(new List<Function>()
            {
                new Function() { FunctionId = 1 },
                new Function() { FunctionId = 2, ParentId = 1 }
            });

        private IFunctionDataAccess _functionDataAccess;
        private IFunctionService _service;

        [SetUp]
        public void SetUp()
        {
            _functionDataAccess = Substitute.For<IFunctionDataAccess>();
            _service = new FunctionService(_functionDataAccess);
            _functionDataAccess.GetFunctionList().Returns(_functionList.ToList());
        }

        [Test]
        public void Functions_WhenAccessed_ReturnResultFromDataAccess()
        {
            // Assert
            Assert.That(_service.Functions.Count, Is.EqualTo(_functionList.Count));
        }

        [Test]
        public void GetLicenseeFunctionList_WhenCall_ReturnExistFunctionsFromDataAccess()
        {
            // Arrange
            var licenseeId = 1;

            var licenseeFunctionList = new List<Function>()
            {
                new Function() { FunctionId = 1 },
                new Function() { FunctionId = 3 }
            };

            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);

            // Action
            var result = _service.GetLicenseeFunctionList(licenseeId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FunctionId, Is.EqualTo(1));
        }

        [Test]
        public void GetUserFunctionList_WhenCall_ReturnCorrectFunctions()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var userFunctionList = new List<Function>()
            {
                new Function() { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>()
            {
                new Function() { FunctionId = 1 },
                new Function() { FunctionId = 2 }
            };
            _functionDataAccess.GetUserFunctionList(licenseeId, userId).Returns(userFunctionList);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);

            // Action
            var result = _service.GetUserFunctionList(licenseeId, userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FunctionId, Is.EqualTo(2));
            Assert.That(result[1].FunctionId, Is.EqualTo(1));
        }

        [Test]
        public void AssignUserFunctions_WhenCall_CallAssignUserFunctionFromDataAccessCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var langKey = "en-US";
            var ip = string.Empty;
            var functionIdList = new List<int>() { 1 };

            // Action
            _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            _functionDataAccess.Received().AssignUserFunction(licenseeId, custId, functionIdList[0], userId, ip, langKey);
        }

        [Test]
        public void AssignUserFunctions_WhenCall_CallRemoveUserFunctionFromDataAccessCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var langKey = "en-US";
            var ip = string.Empty;
            var functionIdList = new List<int>() { 1 };
            var userFunctionList = new List<Function>()
            {
                new Function() { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>()
            {
                new Function() { FunctionId = 1 },
                new Function() { FunctionId = 2 }
            };
            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFunctionList);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);
            _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo() { });

            // Action
            _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            _functionDataAccess.Received().RemoveUserFunction(licenseeId, userId, custId, ip, 2, langKey);
        }

        [Test]
        public void GetMasterFunctions_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var functionList = new List<Function>()
            {
                new Function() { FunctionId = 1 }
            };

            // Action
            var result = _service.GetMasterFunctions(functionList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FunctionId, Is.EqualTo(1));
        }

        [Test]
        public void GetSubMenus_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var functionList = new List<Function>()
            {
                new Function() { FunctionId = 2, ParentId = 1, IsMenu = true }
            };

            // Action
            var result = _service.GetSubMenus(functionList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[1].Count, Is.EqualTo(1));
            Assert.That(result[1][0].FunctionId, Is.EqualTo(2));
        }

        [Test]
        public void GetSubFunctions_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var functionList = new List<Function>()
            {
                new Function() { FunctionId = 2, ParentId = 1, IsFunction = true }
            };

            // Action
            var result = _service.GetSubFunctions(functionList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[1].Count, Is.EqualTo(1));
            Assert.That(result[1][0].FunctionId, Is.EqualTo(2));
        }
    }
}