namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class FunctionServiceTest
    {
        private IFunctionDataAccess _functionDataAccess;

        private IFunctionStorage _functionStorage;

        private IFunctionService _service;

        [SetUp]
        public void SetUp()
        {
            _functionDataAccess = Substitute.For<IFunctionDataAccess>();
            _functionStorage = Substitute.For<IFunctionStorage>();
            _service = new FunctionService(_functionDataAccess, _functionStorage);
        }

        [TearDown]
        public void TearDown()
        {
            _functionDataAccess = null;
            _functionStorage = null;
            _service = null;
        }

        [Test]
        public void Functions_WhenAccess_ReturnResultFromFunctionStorage()
        {
            // Arrange
            var expectedResult = FakeHelper.CreateMany<Function>(5);
            _functionStorage.FunctionList.Returns(expectedResult);

            // Action
            var result = _service.Functions;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetLicenseeFunctionList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var validFunctions = CreateFakeFunctions(new[] { 1, 2, 3 });

            var functions = CreateFakeFunctions(new[] { 4, 5, 6 }).Union(validFunctions);
            var licenseeFunctions = CreateFakeFunctions(new[] { 7, 8, 9 }).Union(validFunctions).ToList();

            var expectedResult = FakeHelper.CreateMany<Function>(5);
            _functionStorage.FunctionList.Returns(functions);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFunctions);

            // Action
            var result = _service.GetLicenseeFunctionList(licenseeId);

            // Assert
            Assert.That(
                result.Select(x => x.FunctionId),
                Is.EqualTo(validFunctions.Select(x => x.FunctionId)));
        }

        [Test]
        public void GetUserFunctionList_NoMasterFunction_ReturnCorrectFunctions()
        {
            // Arrange
            var validFuncs = CreateFakeFunctions(new[] { 1, 2, 3 });

            var licenseeUserFuncs = CreateFakeFunctions(new[] { 4, 5 }).Union(validFuncs);
            var userFuncs = CreateFakeFunctions(new[] { 6 }).Union(licenseeUserFuncs).ToList();
            var licenseeFuncs = CreateFakeFunctions(new[] { 7, 8 }).Union(licenseeUserFuncs).ToList();

            var functions = CreateFakeFunctions(new[] { 9, 10, 11 }).Union(validFuncs);

            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();

            _functionDataAccess.GetUserFunctionList(licenseeId, userId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(CreateFakeFunctions(new int[0]));

            // Action
            var result = _service.GetUserFunctionList(licenseeId, userId);

            // Assert
            Assert.That(
                 result.Select(x => x.FunctionId),
                 Is.EqualTo(validFuncs.Select(x => x.FunctionId)));
        }

        [Test]
        public void GetUserFunctionList_HaveAllSubFuncsOfAMasterFunc_MasterDoesNotExistInReturnList_AddMasterFunc()
        {
            // Arrange
            const int MasterId = 1;
            var masterFuncs = CreateFakeFunctions(new[] { MasterId });
            var subFuncs = new Dictionary<int, IEnumerable<Function>>
            {
                { MasterId, CreateFakeFunctions(new[] { 3, 4, 5 }, MasterId) },
            };

            var validFuncs = subFuncs[MasterId];
            var licenseeUserFuncs = CreateFakeFunctions(new[] { 6, 7 }).Union(validFuncs);
            var userFuncs = CreateFakeFunctions(new[] { 8 }).Union(licenseeUserFuncs).ToList();
            var licenseeFuncs = CreateFakeFunctions(new[] { 9, 10 }).Union(licenseeUserFuncs).ToList();
            var functions = CreateFakeFunctions(new[] { 11, 12, 13 }).Union(validFuncs);

            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();

            _functionDataAccess.GetUserFunctionList(licenseeId, userId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(masterFuncs);
            _functionStorage.SubFunctions.Returns(subFuncs);

            var expected = validFuncs.Union(CreateFakeFunctions(new[] { MasterId }));

            // Action
            var result = _service.GetUserFunctionList(licenseeId, userId);

            // Assert
            Assert.That(
                 result.Select(x => x.FunctionId),
                 Is.EqualTo(expected.Select(x => x.FunctionId)));
        }

        [Test]
        public void GetUserFunctionList_HaveAllSubFuncsOfAMasterFunc_MasterExistsInReturnList_DoNothing()
        {
            // Arrange
            const int MasterId1 = 1;
            const int MasterId2 = 2;
            var masterFuncs = CreateFakeFunctions(new[] { MasterId1, MasterId2 });
            var subFuncs = new Dictionary<int, IEnumerable<Function>>
            {
                { MasterId1, CreateFakeFunctions(new[] { 3, 4, 5 }, MasterId1) },
                { MasterId2, CreateFakeFunctions(new[] { 6, 7 }, MasterId2) }
            };

            var validFuncs = subFuncs[MasterId1].Union(CreateFakeFunctions(new[] { MasterId1 }));
            var licenseeUserFuncs = CreateFakeFunctions(new[] { 8, 9 }).Union(validFuncs);
            var userFuncs = CreateFakeFunctions(new[] { 10 }).Union(licenseeUserFuncs).ToList();
            var licenseeFuncs = CreateFakeFunctions(new[] { 11, 12 }).Union(licenseeUserFuncs).ToList();

            var functions = CreateFakeFunctions(new[] { 13, 14, 15 }).Union(validFuncs);

            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();

            _functionDataAccess.GetUserFunctionList(licenseeId, userId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(licenseeFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(masterFuncs);
            _functionStorage.SubFunctions.Returns(subFuncs);

            // Action
            var result = _service.GetUserFunctionList(licenseeId, userId);

            // Assert
            Assert.That(
                 result.Select(x => x.FunctionId),
                 Is.EqualTo(validFuncs.Select(x => x.FunctionId)));
        }

        [Test]
        public void AssignUserFunctions_WhenCall_CallFunctionFromDataAccessCorrectly()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var langKey = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            const int AddingFunc = 1;
            const int RemoveFunc = 2;

            var noChangeFuncs = new[] { 5, 6, 7 };
            var userFuncs = CreateFakeFunctions(noChangeFuncs).Union(CreateFakeFunctions(new[] { RemoveFunc })).ToList();
            var correctFuncs = noChangeFuncs.Union(new[] { AddingFunc });
            var functions = CreateFakeFunctions(new[] { 11 }).Union(CreateFakeFunctions(correctFuncs)).Union(userFuncs);
            var functionIdList = noChangeFuncs.Union(new[] { 12, 13, AddingFunc });

            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(userFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(CreateFakeFunctions(new int[0]));

            // Action
            _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.Multiple(() =>
            {
                _functionDataAccess.Received().AssignUserFunction(licenseeId, custId, AddingFunc, userId, ip, langKey);
                _functionDataAccess.Received().RemoveUserFunction(licenseeId, userId, custId, ip, RemoveFunc, langKey);
            });
        }

        [Test]
        public void AssignUserFunctions_HasFailureResultWhenRemoving_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var langKey = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            const int RemoveFunc1 = 1;
            const int RemoveFunc2 = 2;

            var noChangeFuncs = new[] { 5, 6, 7 };
            var userFuncs = CreateFakeFunctions(noChangeFuncs).Union(CreateFakeFunctions(new[] { RemoveFunc1, RemoveFunc2 })).ToList();
            var correctFuncs = noChangeFuncs;
            var functions = CreateFakeFunctions(new[] { 11 }).Union(CreateFakeFunctions(correctFuncs)).Union(userFuncs);
            var functionIdList = noChangeFuncs;

            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(userFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(CreateFakeFunctions(new int[0]));

            _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Error" });

            // Action
            var result = _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(100));
                Assert.That(result.Message, Is.EqualTo($"Error [{RemoveFunc1},{RemoveFunc2}];"));
            });
        }

        [Test]
        public void AssignUserFunctions_HasFailureResultWhenAdding_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var langKey = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            const int AddFunc1 = 1;
            const int AddFunc2 = 2;

            var noChangeFuncs = new[] { 5, 6, 7 };
            var userFuncs = CreateFakeFunctions(noChangeFuncs).ToList();
            var correctFuncs = noChangeFuncs.Union(new[] { AddFunc1, AddFunc2 });
            var functions = CreateFakeFunctions(new[] { 11 }).Union(CreateFakeFunctions(correctFuncs)).Union(userFuncs);
            var functionIdList = noChangeFuncs.Union(new[] { 12, 13, AddFunc1, AddFunc2 });

            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(userFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(CreateFakeFunctions(new int[0]));

            _functionDataAccess.AssignUserFunction(licenseeId, custId, Arg.Any<int>(), userId, ip, langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Error" });

            // Action
            var result = _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(100));
                Assert.That(result.Message, Is.EqualTo($"Error [{AddFunc1},{AddFunc2}];"));
            });
        }

        [Test]
        public void AssignUserFunctions_BothAddAndRemoveAreFailure_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var langKey = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            const int AddingFunc = 1;
            const int RemoveFunc = 2;

            var noChangeFuncs = new[] { 5, 6, 7 };
            var userFuncs = CreateFakeFunctions(noChangeFuncs).Union(CreateFakeFunctions(new[] { RemoveFunc })).ToList();
            var correctFuncs = noChangeFuncs.Union(new[] { AddingFunc });
            var functions = CreateFakeFunctions(new[] { 11 }).Union(CreateFakeFunctions(correctFuncs)).Union(userFuncs);
            var functionIdList = noChangeFuncs.Union(new[] { 12, 13, AddingFunc });

            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(userFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(CreateFakeFunctions(new int[0]));

            _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, RemoveFunc, langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Remove Error" });
            _functionDataAccess.AssignUserFunction(licenseeId, custId, AddingFunc, userId, ip, langKey)
                .Returns(new ResultInfo { Code = 100, Message = "Adding Error" });

            // Action
            var result = _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.That(result.Message, Is.EqualTo($"Remove Error [{RemoveFunc}]; Adding Error [{AddingFunc}];"));
        }

        [Test]
        public void AssignUserFunctions_BothAddAndRemoveAreSuccess_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var langKey = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            const int AddingFunc = 1;
            const int RemoveFunc = 2;

            var noChangeFuncs = new[] { 5, 6, 7 };
            var userFuncs = CreateFakeFunctions(noChangeFuncs).Union(CreateFakeFunctions(new[] { RemoveFunc })).ToList();
            var correctFuncs = noChangeFuncs.Union(new[] { AddingFunc });
            var functions = CreateFakeFunctions(new[] { 11 }).Union(CreateFakeFunctions(correctFuncs)).Union(userFuncs);
            var functionIdList = noChangeFuncs.Union(new[] { 12, 13, AddingFunc });

            _functionDataAccess.GetUserFunctionList(licenseeId, custId).Returns(userFuncs);
            _functionDataAccess.GetLicenseeFunctionList(licenseeId).Returns(userFuncs);
            _functionStorage.FunctionList.Returns(functions);
            _functionStorage.MasterFunctions.Returns(CreateFakeFunctions(new int[0]));

            _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, Arg.Any<int>(), langKey)
                .Returns(new ResultInfo());
            _functionDataAccess.AssignUserFunction(licenseeId, custId, Arg.Any<int>(), userId, ip, langKey)
                .Returns(new ResultInfo());

            // Action
            var result = _service.AssignUserFunctions(licenseeId, custId, functionIdList, userId, ip, langKey);

            // Assert
            Assert.That(result.Message, Is.EqualTo("Updating Functions is successul."));
        }

        [Test]
        public void GetMasterFunctions_WhenCall_ReturnCorrectData()
        {
            const int ValidMaster1 = 1;
            const int ValidMaster2 = 2;
            const int InValidMaster2 = 3;

            var functionList = CreateFakeFunctions(new[] { 4, 5, ValidMaster1 }).Union(CreateFakeFunctions(new[] { 6, 7 }, ValidMaster2));
            var masterFuncs = CreateFakeFunctions(new[] { ValidMaster1, ValidMaster2, InValidMaster2 });

            _functionStorage.MasterFunctions.Returns(masterFuncs);

            // Action
            var result = _service.GetMasterFunctions(functionList);

            // Assert
            Assert.That(result.Select(x => x.FunctionId), Is.EqualTo(new[] { ValidMaster1, ValidMaster2 }));
        }

        [Test]
        public void GetSubMenus_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int ValidMaster = 1;

            var menuSubFuncs = CreateFakeFunctions(new[] { 2, 3 }, ValidMaster, isMenu: true);
            var masterFuncs = CreateFakeFunctions(new[] { 4, 5, ValidMaster });

            var functionList = menuSubFuncs.Union(CreateFakeFunctions(new[] { 6, 7 }, ValidMaster));

            _functionStorage.MasterFunctions.Returns(masterFuncs);

            // Action
            var result = _service.GetSubMenus(functionList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[ValidMaster].Select(x => x.FunctionId), Is.EqualTo(menuSubFuncs.Select(x => x.FunctionId)));
            });
        }

        [Test]
        public void GetSubMenus_NoMatchSubMenu_ReturnCorrectData()
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
            const int ValidMaster = 1;

            var subFuncs = CreateFakeFunctions(new[] { 2, 3 }, ValidMaster, isFunction: true);
            var masterFuncs = CreateFakeFunctions(new[] { 4, 5, ValidMaster });

            var functionList = subFuncs.Union(CreateFakeFunctions(new[] { 6, 7 }, ValidMaster));

            _functionStorage.MasterFunctions.Returns(masterFuncs);

            // Action
            var result = _service.GetSubFunctions(functionList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[ValidMaster].Select(x => x.FunctionId), Is.EqualTo(subFuncs.Select(x => x.FunctionId)));
            });
        }

        [Test]
        public void GetSubFunctions_NoMatchSubFunction_ReturnCorrectData()
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

        private IEnumerable<Function> CreateFakeFunctions(IEnumerable<int> functionIds, int parentId = -1, bool isMenu = false, bool isFunction = false)
            => functionIds.Select(x => new Function
            {
                FunctionId = x, DisplayOrder = x, ParentId = parentId, IsMenu = isMenu, IsFunction = isFunction
            });
    }
}