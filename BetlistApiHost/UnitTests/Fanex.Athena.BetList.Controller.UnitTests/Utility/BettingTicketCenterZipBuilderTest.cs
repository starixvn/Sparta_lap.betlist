namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Web;
    using Common.Utility.Services.Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using Utility;

    public class BettingTicketCenterZipBuilderTest
    {
        private IAppSettings _appSettings;

        private ITempFileService _tempFileService;

        private BettingTicketCenterZipBuilder _zipBuilder;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _tempFileService = Substitute.For<ITempFileService>();
            _zipBuilder = new BettingTicketCenterZipBuilder(_appSettings, _tempFileService);
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _tempFileService = null;
        }

        [Test]
        public void BuildZip_ClientIsNotConnected_ReturnNull()
        {
            // Arrange
            var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

            var ticketResult = new GroupTicketResult();
            var title = FakeHelper.Create<string>();

            var httpResponse = SetUpForResponse(false);

            // Action
            var result = _zipBuilder.BuildZip(httpResponse, ticketResult, null, false, null);

            // Assert
            Assert.That(result, Is.Null);
        }

        private HttpResponseBase SetUpForResponse(bool isClientConnected)
        {
            var response = Substitute.For<HttpResponseBase>();
            response.IsClientConnected.Returns(isClientConnected);
            return response;
        }
    }
}