namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Common.Utility.Services.Interfaces;
    using Excel;
    using Excel.Model;
    using Model.DTO;
    using Model.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using Utility;

    public class BettingTicketCenterZipBuilderTest
    {
        private IDependencyResolver _dependencyResolver;

        private IAppSettings _appSettings;

        private ITempFileService _tempFileService;

        private BettingTicketCenterZipBuilder _zipBuilder;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _tempFileService = Substitute.For<ITempFileService>();
            _zipBuilder = Substitute.ForPartsOf<BettingTicketCenterZipBuilder>(_appSettings, _tempFileService);
            _zipBuilder.When(x => x.SaveFile(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            _dependencyResolver = Substitute.For<IDependencyResolver>();
            DependencyResolver.SetResolver(_dependencyResolver);
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

        [Test]
        public void BuildZip_HaveData_SaveFile()
        {
            // Arrange
            const string DataPath = "abc.json";
            var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

            var ticketResult = new GroupTicketResult
            {
                DataPath = new List<string> { DataPath }
            };
            var title = FakeHelper.Create<string>();

            var httpResponse = SetUpForResponse(true);
            _tempFileService.ReadString(DataPath).Returns("{\"MainTicketList\":[{\"ExternalId\":\"ThienPham2\"}],\"TicketDataList\":[]}");
            var excelService = Substitute.For<IBettingTicketCenterExcel>();
            var workbook = Substitute.For<IWorkbook>();
            excelService.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);
            _dependencyResolver.GetService<IBettingTicketCenterExcel>().Returns(excelService);

            // Action
            var result = _zipBuilder.BuildZip(httpResponse, ticketResult, null, false, null);

            // Assert
            Assert.Multiple(() =>
            {
                excelService.Received(1).Export(Arg.Any<BettingTicketCenterExcelModel>());
                _zipBuilder.Received(1).SaveFile(workbook, DataPath + ".dat");
                Assert.That(result, Is.Not.Null);
            });
        }

        private HttpResponseBase SetUpForResponse(bool isClientConnected)
        {
            var response = Substitute.For<HttpResponseBase>();
            response.IsClientConnected.Returns(isClientConnected);
            return response;
        }
    }
}