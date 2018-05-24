namespace Fanex.Athena.Api.ExternalService.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Script.Serialization;
    using ExternalService;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;
    using Service.Report.Interfaces;
    using UnitTest.Common.Helpers;

    public class NotificationApiControllerTest
    {
        private const string UserName = "admin";

        private const string SiteName = "12bet";

        private const int MaximumMonthDistance = 5;

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
            _apiController = GetControllerWithActionArguments(_moveBetService, _languageService, _notificationService);
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
                Language = "en",
                IsOnlyUnread = true,
                PageSize = 10,
                PageIndex = 1,
                UserName = UserName,
                LicenseeName = SiteName
            };

            var expectedResultModel = new ReportResultModel<NotificationMessage>
            {
                DataList = new List<NotificationMessage>
                {
                    FakeHelper.Create<NotificationMessage>()
                },
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
        public void GetNotificationMessages_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "en"
            };
            var today = DateTime.Today;
            var expectedMetaModel = new NotificationMetaModel
            {
                FromDate = new DateTime(today.Year, today.Month, 1).AddMonths(-MaximumMonthDistance),
                ToDate = today.AddDays(1).AddMilliseconds(-1),
                Language = "en",
                IsOnlyUnread = true,
                PageSize = 10,
                PageIndex = 1,
                UserName = UserName,
                LicenseeName = SiteName
            };

            NotificationMetaModel passedParam = null;

            _notificationService.GetNotificationMessages(Arg.Do<NotificationMetaModel>(x => passedParam = x))
                                .Returns(new ReportResultModel<NotificationMessage>());

            var expectedResult = _serializer.Serialize(expectedMetaModel);

            // Action
            _apiController.GetNotificationMessages(metaModel);
            var result = _serializer.Serialize(passedParam);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MarkNotificationAsRead_WhenCall_ReturnExpectedResult()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel
            {
                MessageId = 1,
                MessageType = "Amendment",
                UserName = UserName,
                LicenseeName = SiteName
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

        [Test]
        public void MarkNotificationAsRead_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel
            {
                MessageId = 1,
                MessageType = "Amendment"
            };

            var expectedMetaModel = new NotificationMarkAsReadMetaModel
            {
                MessageId = 1,
                MessageType = "Amendment",
                UserName = UserName,
                LicenseeName = SiteName
            };

            NotificationMarkAsReadMetaModel passedParam = null;
            _notificationService.MarkNotificationAsRead(Arg.Do<NotificationMarkAsReadMetaModel>(x => passedParam = x))
                                .Returns(new ResultInfo());

            var expectedResult = _serializer.Serialize(expectedMetaModel);

            // Action
            _apiController.MarkNotificationAsRead(metaModel);
            var result = _serializer.Serialize(passedParam);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private NotificationApiController GetControllerWithActionArguments(
            IMoveBetService moveBetService,
            ILanguageService languageService,
            INotificationService notificationService)
        {
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments =
                {
                    { "username", "admin" },
                    { "sitename", "12bet" }
                }
            };

            return new NotificationApiController(moveBetService, languageService, notificationService)
            {
                ActionContext = mockActionContext
            };
        }
    }
}