namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Base.UnitTests;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Utility.Interfaces;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterControllerTest : BaseApiControllerHelperTest
    {
        private IBetListApiClient _betListApiClient;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _betListApiClient = Substitute.For<IBetListApiClient>();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _betListApiClient = null;
        }

        [TearDown]
        public void TearDown()
        {
            _betListApiClient.ClearReceivedCalls();
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-80)]
        public async Task GetByRefno_RefNoIsInvalid_ReturnNull(int refNo)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RefNoBetListMetaModel
                {
                    RefNo = refNo
                };

                // Action
                var result = await controller.GetByRefno(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task GetByRefno_ValidMetaModel_CallBetListApiWithCorrectParameters()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ApiPath = "/BettingTicketCenter/GetByRefno";
                var metaModel = new RefNoBetListMetaModel
                {
                    RefNo = FakeHelper.Create<uint>()
                };

                var expect = new HttpResponseMessage();
                _betListApiClient.PostAsync(ApiPath, metaModel).Returns(Task.FromResult(expect));

                // Action
                var actual = await controller.GetByRefno(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    _betListApiClient.Received().PostAsync(ApiPath, metaModel);
                });
            }
        }

        [Test]
        public void GetByRunning_DateRangeIsInvalid_ThrowException()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BaseBettingTicketMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today.AddDays(-1)
                };

                // Action
                AsyncTestDelegate testDelegate = async () => await controller.GetByRunning(metaModel);

                // Assert
                Assert.ThrowsAsync<ArgumentException>(testDelegate);
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetByRunning_NoProduct_ThrowException(string productCode)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BaseBettingTicketMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    ProductCodeList = productCode
                };

                // Action
                AsyncTestDelegate testDelegate = async () => await controller.GetByRunning(metaModel);

                // Assert
                Assert.ThrowsAsync<ArgumentException>(testDelegate);
            }
        }

        [TestCase(null)]
        [TestCase("-100")]
        public void GetByRunning_InvalidCurrency_ThrowException(string currencyIdList)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BaseBettingTicketMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    ProductCodeList = "C1,C2",
                    CurrencyIdList = currencyIdList
                };

                // Action
                AsyncTestDelegate testDelegate = async () => await controller.GetByRunning(metaModel);

                // Assert
                Assert.ThrowsAsync<ArgumentException>(testDelegate);
            }
        }

        [Test]
        public async Task GetByRunning_ValidMetaModel_CallApiWithCorrectParameters()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ApiPath = "/BettingTicketCenter/GetByRunning";
                var metaModel = new BaseBettingTicketMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    ProductCodeList = "C1,C2",
                    CurrencyIdList = BaseCurrencyType.Eur.ToString()
                };

                var expect = new HttpResponseMessage();
                _betListApiClient.PostAsync(ApiPath, metaModel).Returns(Task.FromResult(expect));

                // Action
                var actual = await controller.GetByRunning(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    _betListApiClient.Received().PostAsync(ApiPath, metaModel);
                });
            }
        }

        [Test]
        public async Task GetBySingleCustomer_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new SingleCustomersBetListMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    ProductCodeList = "C1,C2",
                    CurrencyIdList = "-2",
                    UserName = FakeHelper.Create<string>()
                };

                // Action
                var result = await controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task GetBySingleCustomer_DateRangeIsInvalid_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new SingleCustomersBetListMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today.AddDays(-1),
                    UserName = FakeHelper.Create<string>()
                };

                // Action
                var result = await controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task GetBySingleCustomer_UserNameIsEmpty_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new SingleCustomersBetListMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today
                };

                // Action
                var result = await controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task GetBySingleCustomer_ValidMetaModel_CallApiWithCorrectParameters()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ApiPath = "/BettingTicketCenter/GetBySingleCustomer";
                var metaModel = new SingleCustomersBetListMetaModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    ProductCodeList = "C1,C2",
                    CurrencyIdList = SessionService.UserSession.CurrencyList.Select(x => x.Key).FirstOrDefault().ToString(),
                    UserName = FakeHelper.Create<string>()
                };

                var expect = new HttpResponseMessage();
                _betListApiClient.PostAsync(ApiPath, metaModel).Returns(Task.FromResult(expect));

                // Action
                var actual = await controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    _betListApiClient.Received().PostAsync(ApiPath, metaModel);
                });
            }
        }

        private BettingTicketCenterController CreateController()
        {
            return new BettingTicketCenterController(SessionService, ProductService, LangService, _betListApiClient);
        }
    }
}