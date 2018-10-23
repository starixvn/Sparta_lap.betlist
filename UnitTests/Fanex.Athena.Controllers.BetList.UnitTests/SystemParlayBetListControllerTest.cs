namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    internal class SystemParlayBetListControllerTest : BetListBaseControllerHelperTest
    {
        private IBetListHTMLBuilder _betListBuilder;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _betListBuilder = Substitute.For<IBetListHTMLBuilder>();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new SystemParlayDetailMetaModel();

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/SystemParlayDetail/Main.cshtml"));
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new SystemParlayDetailMetaModel();
                var systemParlayData = new SystemParlayData();
                var systemParlayText = "sdfsdl;fklskhflskdhlfkjshdlfkjhslkdjfhlskjdhf|";
                BetListService.GetSystemParlayDetail(metaModel).Returns(systemParlayData);
                _betListBuilder.BuildSystemParlayDetail(Arg.Any<ITicketHelper>(), systemParlayData).Returns(systemParlayText);

                // Action
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (SystemParlayDetailViewModel)result.Model;

                // Assert
                Assert.That(model.DetailText, Is.EqualTo(systemParlayText));
            }
        }

        private SystemParlayBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<SystemParlayBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter,
                Substitute.For<ITicketHelper>(),
                _betListBuilder);
            return controller;
        }
    }
}