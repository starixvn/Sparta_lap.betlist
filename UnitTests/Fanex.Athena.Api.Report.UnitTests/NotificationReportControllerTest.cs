namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using Model.Core.DTO.Common;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Report.Interfaces;

    public class NotificationReportControllerTest : BaseApiReportControllerHelperTest
    {
        private static readonly DateTime MinDate = new DateTime(1990, 1, 1);

        private static readonly DateTime MaxDate = new DateTime(3000, 1, 1);

        private INotificationService _service;

        private NotificationReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();

            _service = Substitute.For<INotificationService>();
            _controller = Substitute.ForPartsOf<NotificationReportController>(_service, SessionService, ProductService);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _service = null;
            _controller = null;
        }

        [Test]
        public void GetNotificationMessages_WhenCall_CallServiceWithCorrectParameter()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();
            _service.GetNotificationMessages(Arg.Do<NotificationMetaModel>(x => metaModel = x));

            // Action
            _controller.GetNotificationMessages();

            // Asset
            Assert.Multiple(() =>
            {
                _service.Received().GetNotificationMessages(Arg.Any<NotificationMetaModel>());
                Assert.That(metaModel.FromDate, Is.EqualTo(MinDate));
                Assert.That(metaModel.ToDate, Is.EqualTo(MaxDate));
                Assert.That(metaModel.IsOnlyUnread, Is.True);
                Assert.That(metaModel.PageIndex, Is.EqualTo(1));
                Assert.That(metaModel.PageSize, Is.EqualTo(10));
            });
        }

        [Test]
        public void Index_InValidMetaModel_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1)
            };

            // Action
            var result = _controller.Index(metaModel);

            // Asset
            Assert.Multiple(() =>
            {
                _service.DidNotReceive().GetGeneral(Arg.Any<NotificationMetaModel>());
                Assert.That(result.TotalRows, Is.Zero);
                Assert.That(result.DataList, Is.Empty);
            });
        }

        [Test]
        public void Index_WhenCall_CallServiceWithCorrectParameter()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            // Action
            _controller.Index(metaModel);

            // Asset
            Assert.Multiple(() =>
            {
                _service.Received().GetGeneral(Arg.Any<NotificationMetaModel>());
                Assert.That(metaModel.UserId, Is.EqualTo(SessionUserId));
                Assert.That(metaModel.Language, Is.EqualTo(SessionLanguage));
                Assert.That(metaModel.LicenseeName, Is.EqualTo(SessionLicenseeName));
            });
        }

        [Test]
        public void ResultAmendment_InValidMetaModel_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1)
            };

            // Action
            var result = _controller.ResultAmendment(metaModel);

            // Asset
            Assert.Multiple(() =>
            {
                _service.DidNotReceive().GetResultAmendment(Arg.Any<NotificationMetaModel>());
                Assert.That(result.TotalRows, Is.Zero);
                Assert.That(result.DataList, Is.Empty);
            });
        }

        [Test]
        public void ResultAmendment_WhenCall_CallServiceWithCorrectParameter()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            // Action
            _controller.ResultAmendment(metaModel);

            // Asset
            Assert.Multiple(() =>
            {
                _service.Received().GetResultAmendment(Arg.Any<NotificationMetaModel>());
                Assert.That(metaModel.UserId, Is.EqualTo(SessionUserId));
                Assert.That(metaModel.Language, Is.EqualTo(SessionLanguage));
                Assert.That(metaModel.LicenseeName, Is.EqualTo(SessionLicenseeName));
            });
        }

        [Test]
        public void MarkNotificationAsRead_WhenCallSuccessfully_ReturnTrue()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel();
            _service.MarkNotificationAsRead(metaModel).Returns(new ResultInfo
            {
                Code = 0
            });

            // Action
            var result = _controller.MarkNotificationAsRead(metaModel);

            // Asset
            Assert.Multiple(() =>
            {
                _service.Received().MarkNotificationAsRead(Arg.Any<NotificationMarkAsReadMetaModel>());
                Assert.That(result.IsSuccessful, Is.True);
            });
        }

        [Test]
        public void MarkNotificationAsRead_WhenCallFailed_ReturnFalse()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel();
            _service.MarkNotificationAsRead(metaModel).Returns(new ResultInfo
            {
                Code = 1,
                Message = "Failure"
            });

            // Action
            var result = _controller.MarkNotificationAsRead(metaModel);

            // Asset
            Assert.Multiple(() =>
            {
                _service.Received().MarkNotificationAsRead(Arg.Any<NotificationMarkAsReadMetaModel>());
                Assert.That(result.IsSuccessful, Is.False);
            });
        }
    }
}