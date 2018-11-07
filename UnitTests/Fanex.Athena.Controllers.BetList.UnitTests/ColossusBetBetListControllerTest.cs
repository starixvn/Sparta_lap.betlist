namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Web.Mvc;
    using DataAccess.BetList.Interfaces;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Utility.Interfaces;

    public class ColossusBetBetListControllerTest : BetListBaseControllerHelperTest
    {
        private ICoreLogger _logger;

        private IColossusBetBetListDataAccess _dataAccess;

        private IWebClientService _webClientService;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ICoreLogger>();
            _dataAccess = Substitute.For<IColossusBetBetListDataAccess>();
            _webClientService = Substitute.For<IWebClientService>();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new ColossusBetDetailMetaModel();

                _dataAccess.GetData(metaModel).Returns(new ColossusBetDetailViewModel());

                SetUpForGetIframeUrl(metaModel);

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/ColossusBetDetail/Main.cshtml"));
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new ColossusBetDetailMetaModel();

                var viewModel = new ColossusBetDetailViewModel();
                _dataAccess.GetData(metaModel).Returns(viewModel);

                var expectedIframeUrl = SetUpForGetIframeUrl(metaModel);

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(viewModel.IframeUrl, Is.EqualTo(expectedIframeUrl));
            }
        }

        [Test]
        public void Detail_GetTokenThrowException_LogError()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var exception = new Exception();
                var metaModel = new ColossusBetDetailMetaModel();

                var viewModel = new ColossusBetDetailViewModel();
                _dataAccess.GetData(metaModel).Returns(viewModel);

                _webClientService.When(x => x.DownloadString(Arg.Any<string>())).Throw(exception);

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                _logger.Received(1).Error(exception, Arg.Any<string>());
            }
        }

        private string SetUpForGetIframeUrl(ColossusBetDetailMetaModel metaModel)
        {
            const int SiteId = 1;
            const string Url = "{0} {1} {2}";
            const string BetSiteId = "Colossus";
            const string Api = "API{0}{1}";
            const string Token = "token";

            _dataAccess.GetSiteId(metaModel.CustId).Returns(SiteId);

            StubGetValueAppSettings("CBIframeUrlSiteId1", Url);
            StubGetValueAppSettings("ColossusBetSiteId", BetSiteId);
            StubGetValueAppSettings("ColossusBetTokenAPI", Api);

            _webClientService.DownloadString(Arg.Any<string>()).Returns("{'data':{'authorization':'token'}}");

            return string.Format(Url, metaModel.BetCheck, BetSiteId, Token);
        }

        private ColossusBetBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<ColossusBetBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter,
                _logger,
                _webClientService,
                _dataAccess);
            return controller;
        }
    }
}