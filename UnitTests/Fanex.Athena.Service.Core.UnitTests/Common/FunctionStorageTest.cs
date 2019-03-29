namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    public class FunctionStorageTest
    {
        private FunctionStorage _functionStorage;

        private IFunctionDataAccess _functionDataAccess;

        private IList<Function> _functions = new List<Function>
        {
            new Function { FunctionId = 1, ParentId = 0, DisplayOrder = 1 },
            new Function { FunctionId = 2, ParentId = 0, DisplayOrder = 2 },
            new Function { FunctionId = 3, ParentId = 1, DisplayOrder = 3 },
            new Function { FunctionId = 4, ParentId = 1, DisplayOrder = 4 }
        };

        [SetUp]
        public void SetUp()
        {
            _functionDataAccess = Substitute.For<IFunctionDataAccess>();
            _functionStorage = new FunctionStorage(_functionDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _functionDataAccess = null;
            _functionStorage = null;
        }

        [Test]
        public async Task FunctionList_DataFromDataAccessIsNull_ReturnNull()
        {
            // Arrange
            _functionDataAccess.GetFunctionListAsync().Returns(Task.FromResult<IList<Function>>(null));

            // Action
            var result = await _functionStorage.GetFunctionList();

            // Assert
            Assert.Multiple(() =>
            {
                _functionDataAccess.Received().GetFunctionListAsync();
                Assert.That(result, Is.Null);
            });
        }

        [Test]
        public async Task FunctionList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            _functionDataAccess.GetFunctionListAsync().Returns(_functions);

            // Action
            var resultFirstTime = await _functionStorage.GetFunctionList();
            var resultSecondTime = await _functionStorage.GetFunctionList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultFirstTime, Is.EqualTo(_functions));
                Assert.That(resultSecondTime, Is.EqualTo(resultFirstTime));
            });
        }

        [Test]
        public async Task MasterFunctions_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedFunctionIds = _functions.Where(x => x.ParentId == 0).Select(x => x.FunctionId);

            // Action
            var resultFirstTime = await _functionStorage.GetMasterFunctions();
            var resultSecondTime = await _functionStorage.GetMasterFunctions();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultFirstTime.Select(x => x.FunctionId), Is.EqualTo(expectedFunctionIds));
                Assert.That(resultSecondTime, Is.EqualTo(resultFirstTime));
            });
        }

        [Test]
        public async Task SubFunctions_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedFunction = new Dictionary<int, IEnumerable<Function>>
            {
                {
                    1,
                    new List<Function>
                    {
                        new Function { FunctionId = 3, ParentId = 1, DisplayOrder = 3 },
                        new Function { FunctionId = 4, ParentId = 1, DisplayOrder = 4 }
                    }
                },
                {
                    2,
                    new List<Function>()
                }
            };

            // Action
            var resultFirstTime = await _functionStorage.GetSubFunctions();
            var resultSecondTime = await _functionStorage.GetSubFunctions();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultFirstTime.Keys, Is.EqualTo(expectedFunction.Keys));
                Assert.That(
                    resultFirstTime.First().Value.Select(x => x.FunctionId),
                    Is.EqualTo(expectedFunction.First().Value.Select(x => x.FunctionId)));
                Assert.That(resultSecondTime, Is.EqualTo(resultFirstTime));
            });
        }
    }
}