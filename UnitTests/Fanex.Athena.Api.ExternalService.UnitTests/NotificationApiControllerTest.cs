namespace Fanex.Athena.Api.ExternalService.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using ExternalService;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;

    public class NotificationApiControllerTest
    {
        private IMoveBetService _moveBetService;

        private ILanguageService _languageService;

        private INotificationService _notificationService;

        private NotificationApiController _apiController;

        private JavaScriptSerializer _serializer = new JavaScriptSerializer();

        [SetUp]
        public void SetUp()
        {
            _moveBetService = Substitute.For<IMoveBetService>();
            _languageService = Substitute.For<ILanguageService>();
            _notificationService = Substitute.For<INotificationService>();
            _apiController = new NotificationApiController(_moveBetService, _languageService, _notificationService);
        }

        [TearDown]
        public void TearDown()
        {
            _moveBetService = null;
            _languageService = null;
            _notificationService = null;
        }

        [Test]
        public void Index_WhenCall_ServiceIsCalledWithCorrectParameter()
        {
            // Arrange
            const string CurrentLanguageCode = "en";
            _languageService.GetCurrentLanguage().Returns(CurrentLanguageCode);

            // Action
            _apiController.Index();

            // Assert
            _moveBetService.Received().GetMoveBetNotification(CurrentLanguageCode);
        }

        [Test]
        public void GetNotificationMessages_WhenCall_ReturnExpectedResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                LicenseeName = "12bet",
                UserId = 1,
                Language = "en"
            };

            var expectedResultModel = new ReportResultModel<NotificationMessage>
            {
                DataList = new List<NotificationMessage>(),
                TotalRows = 1
            };
            _notificationService.GetNotificationMessages(metaModel).Returns(expectedResultModel);

            var expectedResult = _serializer.Serialize(expectedResultModel);

            // Action
            var notificationMessages = _apiController.GetNotificationMessages(metaModel);
            var result = _serializer.Serialize(notificationMessages);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MarkNotificationAsRead_WhenCall_ReturnExpectedResult()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel
            {
                UserId = "1",
                LicenseeName = "12bet",
                MessageId = 1,
                MessageType = "Amendment"
            };

            var resultModel = new ResultInfo
            {
                Code = 0,
                Message = "Success"
            };
            _notificationService.MarkNotificationAsRead(metaModel).Returns(resultModel);

            var expectedResultModel = new
            {
                Code = resultModel.Code,
                Message = resultModel.Message,
                IsSuccessful = resultModel.IsSuccessful
            };
            var expectedResult = _serializer.Serialize(expectedResultModel);

            // Action
            var markNotificationAsReadResult = _apiController.MarkNotificationAsRead(metaModel);
            var result = _serializer.Serialize(markNotificationAsReadResult);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}