namespace Fanex.Athena.Api.Base.UnitTests
{
    using System.IO;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [TestFixture]
    public class BaseApiControllerTest
    {
        private ILanguageService _langService;

        private IProductService _productService;

        private ISessionService _sessionService;

        [SetUp]
        public void SetUp()
        {
            _langService = Substitute.For<ILanguageService>();
            _productService = Substitute.For<IProductService>();
            _sessionService = Substitute.For<ISessionService>();
        }

        [Test]
        public void ExcelStream_UserSessionIsNull_XFrameOptionToSameOrigin()
        {
            // Arrange
            const string FileName = "excel.sml";
            var workbook = Substitute.For<IWorkbook>();
            var controller = new BaseApiController(_sessionService, _productService, _langService);

            // Action
            var result = controller.ExcelStream(workbook, FileName);

            // Assert
            using (var stream = new MemoryStream())
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result.Content.Headers.ContentType.MediaType, Is.EqualTo("application/octet-stream"));
                    Assert.That(result.Content.Headers.ContentDisposition.FileName, Is.EqualTo(FileName));
                });
            }
        }
    }
}