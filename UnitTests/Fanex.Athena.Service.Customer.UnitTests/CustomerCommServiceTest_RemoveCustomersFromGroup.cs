namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerCommServiceTest
    {
        [Test]
        public async Task RemoveCustomersFromGroupAsync_CallApiSuccess_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { Ip = FakeHelper.Create<string>(), IsRemovePlayercomm = true };

            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _customerCommDataAccess.RemoveCustomersFromGroupAsync(metaModel).Returns(Task.FromResult<IResultInfo>(expectedResult));

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(true);

            // Action
            var result = await _customerCommService.RemoveCustomersFromGroupAsync(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task RemoveCustomersFromGroupAsync_customerIdListIsEmpty_NoCallApi_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { Ip = FakeHelper.Create<string>(), IsRemovePlayercomm = true };

            var expectedResult = new ResultInfo { ExtendedData = string.Empty };
            _customerCommDataAccess.RemoveCustomersFromGroupAsync(metaModel).Returns(Task.FromResult<IResultInfo>(expectedResult));

            // Action
            var result = await _customerCommService.RemoveCustomersFromGroupAsync(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task RemoveCustomersFromGroupAsync_CallApiFailed_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { Ip = FakeHelper.Create<string>(), IsRemovePlayercomm = true };

            var resultInfo = new ResultInfo { ExtendedData = "1,2,3" };
            _customerCommDataAccess.RemoveCustomersFromGroupAsync(metaModel).Returns(Task.FromResult<IResultInfo>(resultInfo));

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(false);

            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };

            // Action
            var result = await _customerCommService.RemoveCustomersFromGroupAsync(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task RemoveCustomersFromGroupAsync_RemoveFailed_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var expectedResult = new ResultInfo { Code = Math.Abs(FakeHelper.Create<int>()) + 1, Message = FakeHelper.Create<string>() };
            _customerCommDataAccess.RemoveCustomersFromGroupAsync(Arg.Any<RemoveFromGroupComMetaModel>()).Returns(Task.FromResult<IResultInfo>(expectedResult));

            // Action
            var result = await _customerCommService.RemoveCustomersFromGroupAsync(new RemoveFromGroupComMetaModel(), isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task RemoveCustomersFromGroup_IsRemovePlayercommInMetaModelIsFalse_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { IsRemovePlayercomm = false };
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerCommDataAccess.RemoveCustomersFromGroupAsync(metaModel).Returns(Task.FromResult<IResultInfo>(expectedResult));

            // Action
            var result = await _customerCommService.RemoveCustomersFromGroupAsync(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task RemoveCustomersFromGroup_IsAllowedRNGCasinoIsFalse_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = false;
            var metaModel = new RemoveFromGroupComMetaModel { IsRemovePlayercomm = true };
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerCommDataAccess.RemoveCustomersFromGroupAsync(metaModel).Returns(Task.FromResult<IResultInfo>(expectedResult));

            // Action
            var result = await _customerCommService.RemoveCustomersFromGroupAsync(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task RemoveCustomersFromGroup_ExtendedDataInResultIsEmpty_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { IsRemovePlayercomm = true };
            var expectedResult = new ResultInfo { ExtendedData = string.Empty };
            _customerCommDataAccess.RemoveCustomersFromGroupAsync(metaModel).Returns(Task.FromResult<IResultInfo>(expectedResult));

            // Action
            var result = await _customerCommService.RemoveCustomersFromGroupAsync(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}