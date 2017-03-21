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
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2, ParentId = 1 }
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

        [TearDown]
        public void TearDown()
        {
            _functionDataAccess = null;
            _service = null;
        }

        [Test]
        public void Functions_WhenAccessedFirstTime_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = _functionList;

            // Action
            var result = _service.Functions;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Functions_WhenAccessedSecondTime_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = _functionList;
            var firstGet = _service.Functions;

            // Action
            var result = _service.Functions;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetLicenseeFunctionList_WhenCall_ReturnExistFunctionsFromDataAccess()
        {
            // Arrange
            var licenseeId = 1;

            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 3 }
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
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
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
        public void GetUserFunctionList_SubFunctionIsExisting_ReturnCorrectFunctions()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var subFunctions = new Dictionary<int, IList<Function>> { { 1, new List<Function> { new Function { FunctionId = 2, ParentId = 1 } } } };
            var service = new FunctionService(_functionDataAccess, null, null, subFunctions);
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            _functionDataAccess.GetUserFunctionList(licenseeId, userId).Returns(userFunctionList);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);

            // Action
            var result = service.GetUserFunctionList(licenseeId, userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FunctionId, Is.EqualTo(2));
            Assert.That(result[1].FunctionId, Is.EqualTo(1));
        }

        [Test]
        public void GetUserFunctionList_ContainFunctionIdOfMasterFunction_ReturnCorrectFunctions()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            _functionDataAccess.GetUserFunctionList(licenseeId, userId).Returns(userFunctionList);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);

            // Action
            var result = _service.GetUserFunctionList(licenseeId, userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FunctionId, Is.EqualTo(1));
            Assert.That(result[1].FunctionId, Is.EqualTo(2));
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
            var functionIdList = new List<int> { 1 };

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
            var functionIdList = new List<int> { 1 };
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFunctionList);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);
            _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo());

            // Action
            _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            _functionDataAccess.Received().RemoveUserFunction(licenseeId, userId, custId, ip, 2, langKey);
        }

        [Test]
        public void AssignUserFunctions_HasFailureResultWhenRemoving_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var langKey = "en-US";
            var ip = string.Empty;
            var functionIdList = new List<int> { 1 };
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFunctionList);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);
            _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Error" });

            // Action
            var result = _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.That(result.Code, Is.EqualTo(100));
            Assert.That(result.Message, Is.EqualTo("Error [2];"));
        }

        [Test]
        public void AssignUserFunctions_HasFailureResultWhenAdding_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var langKey = "en-US";
            var ip = string.Empty;
            var functionIdList = new List<int> { 1 };
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 3 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFunctionList);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);
            _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo());
            _functionDataAccess.AssignUserFunction(licenseeId, custId, Arg.Any<int>(), userId, ip, langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Adding Error" });

            // Action
            var result = _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.That(result.Message, Is.EqualTo("Adding Error [1];"));
        }

        [Test]
        public void AssignUserFunctions_BothAddAndRemoveAreFailure_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var langKey = "en-US";
            var ip = string.Empty;
            var functionList = new ReadOnlyCollection<Function>(new List<Function>()
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2, ParentId = 1 },
                new Function { FunctionId = 3, ParentId = 1 }
            });
            var functionDataAccess = Substitute.For<IFunctionDataAccess>();
            var service = new FunctionService(functionDataAccess);
            functionDataAccess.GetFunctionList().Returns(functionList.ToList());

            var functionIdList = new List<int> { 2, 3 };
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFunctionList);
            functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);
            functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Remove Error" });
            functionDataAccess.AssignUserFunction(licenseeId, custId, Arg.Any<int>(), userId, ip, langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Adding Error" });

            // Action
            var result = service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.That(result.Message, Is.EqualTo("Remove Error [1]; Adding Error [3];"));
        }

        [Test]
        public void AssignUserFunctions_BothAddAndRemoveAreSuccess_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var langKey = "en-US";
            var ip = string.Empty;
            var functionList = new ReadOnlyCollection<Function>(new List<Function>()
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2, ParentId = 1 },
                new Function { FunctionId = 3, ParentId = 1 }
            });
            var functionDataAccess = Substitute.For<IFunctionDataAccess>();
            var service = new FunctionService(functionDataAccess);
            functionDataAccess.GetFunctionList().Returns(functionList.ToList());

            var functionIdList = new List<int> { 2, 3 };
            var userFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            var licenseeFunctionList = new List<Function>
            {
                new Function { FunctionId = 1 },
                new Function { FunctionId = 2 }
            };
            functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFunctionList);
            functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctionList);
            functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo());
            functionDataAccess.AssignUserFunction(licenseeId, custId, Arg.Any<int>(), userId, ip, langKey)
                .Returns(new ResultInfo());

            // Action
            var result = service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.That(result.Message, Is.EqualTo("Updating Functions is successul."));
        }

        [Test]
        public void GetMasterFunctions_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var functionList = new List<Function>
            {
                new Function { FunctionId = 1 }
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
            var functionList = new List<Function>
            {
                new Function { FunctionId = 2, ParentId = 1, IsMenu = true }
            };

            // Action
            var result = _service.GetSubMenus(functionList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[1].Count, Is.EqualTo(1));
            Assert.That(result[1][0].FunctionId, Is.EqualTo(2));
        }

        [Test]
        public void GetSubMenus_HaveNoMatchSubMenu_ReturnCorrectData()
        {
            // Arrange
            var functionList = new List<Function>
            {
                new Function()
            };

            // Action
            var result = _service.GetSubMenus(functionList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetSubFunctions_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var functionList = new List<Function>
            {
                new Function { FunctionId = 2, ParentId = 1, IsFunction = true }
            };

            // Action
            var result = _service.GetSubFunctions(functionList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[1].Count, Is.EqualTo(1));
            Assert.That(result[1][0].FunctionId, Is.EqualTo(2));
        }

        [Test]
        public void GetSubFunctions_HaveNoMatchSubFunction_ReturnCorrectData()
        {
            // Arrange
            var functionList = new List<Function>
            {
                new Function()
            };

            // Action
            var result = _service.GetSubFunctions(functionList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}