namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Utility.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Interfaces;
    using Logging;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class NotificationServiceTest
    {
        private ILogger _logger;

        private IMemoryCacheService _cache;

        private IAppSettings _appSettings;

        private IWebClientService _webClientService;

        private INotificationService _notificationService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _logger = Substitute.For<ILogger>();
            _cache = Substitute.For<IMemoryCacheService>();
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();
            _notificationService = new NotificationService(_logger, _cache, _appSettings, _webClientService);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _logger = null;
            _cache = null;
            _appSettings = null;
            _webClientService = null;
            _notificationService = null;
        }

        [SetUp]
        public void SetUp()
        {
            _webClientService.ClearReceivedCalls();
            _cache.ClearReceivedCalls();
        }

        [Test]
        public void GetNotificationTypes_WhenCallWithEmptyCache_CallApi()
        {
            // Arrange
            _cache.Get<IDictionary<string, string>>(Arg.Any<string>()).Returns(x => { return null; });

            var url = "API1";
            _appSettings.NotificationTypeApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(string.Empty);

            // Action
            _notificationService.GetNotificationTypes();

            // Assert
            _webClientService.Received().DownloadString(Arg.Any<string>());
            _cache.Received().Set(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>());
        }

        [Test]
        public void GetNotificationTypes_WhenCallWithCache_GetDataFromCache()
        {
            // Arrange
            _cache.Get<IDictionary<string, string>>(Arg.Any<string>()).Returns(new Dictionary<string, string>());

            var url = "API2";
            _appSettings.NotificationTypeApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(string.Empty);

            // Action
            _notificationService.GetNotificationTypes();

            // Assert
            _webClientService.DidNotReceive().DownloadString(Arg.Any<string>());
            _cache.Received().DidNotReceive().Set(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>());
        }

        [Test]
        public void GetNotificationTypes_ApiReturnsEmpty_ReturnEmptyDictionary()
        {
            // Arrange
            _cache.Get<IDictionary<string, string>>(Arg.Any<string>()).Returns(x => { return null; });

            var url = "API3";
            _appSettings.NotificationTypeApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(string.Empty);

            // Action
            var result = _notificationService.GetNotificationTypes();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetNotificationTypes_ApiReturnsData_ReturnCorrectResult()
        {
            // Arrange
            _cache.Get<IDictionary<string, string>>(Arg.Any<string>()).Returns(x => { return null; });

            var url = "API4";
            _appSettings.NotificationTypeApiUrl.Returns(url);

            var jsonString = "{'1':'A','2':'B','3':'C'}";
            _webClientService.DownloadString(url).Returns(jsonString);

            var expectedResult = new Dictionary<string, string>
            {
                { "0", "All" },
                { "1", "A" },
                { "2", "B" },
                { "3", "C" },
            };

            // Action
            var result = _notificationService.GetNotificationTypes();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result.Union(expectedResult).Count, Is.EqualTo(expectedResult.Count));
            });
        }

        [Test]
        public void GetNotificationTypes_ApiReturnsNoValue_ReturnEmptyDictionary()
        {
            // Arrange
            _cache.Get<IDictionary<string, string>>(Arg.Any<string>()).Returns(x => { return null; });

            var url = "API5";
            _appSettings.NotificationTypeApiUrl.Returns(url);

            var jsonString = "{}";
            _webClientService.DownloadString(url).Returns(jsonString);

            // Action
            var result = _notificationService.GetNotificationTypes();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetNotificationTypes_ApiThrowsException_WriteLog()
        {
            // Arrange
            _cache.Get<IDictionary<string, string>>(Arg.Any<string>()).Returns(x => { return null; });

            var url = "API6";
            _appSettings.NotificationTypeApiUrl.Returns(url);

            var exceptionMsg = "Message";
            _webClientService.When(x => x.DownloadString(url)).Do(x =>
            {
                throw new Exception(exceptionMsg);
            });

            // Action
            var result = _notificationService.GetNotificationTypes();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Empty);
                _logger.Received().Error($"{exceptionMsg}\nApiUrl: {url}", Arg.Any<Exception>());
            });
        }

        [Test]
        public void GetGeneral_ApiReturnsEmpty_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            var url = "API7";
            _appSettings.NotificationApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(string.Empty);

            // Action
            var result = _notificationService.GetGeneral(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetGeneral_ApiReturnsData_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "lang"
            };

            var url = "API8";
            _appSettings.NotificationApiUrl.Returns(url);

            var jsonString = "{'TotalItems':'1',"
                + "'Data':[{'SubjectLang':'Subject','ContentLang':'Content','MessageId':'1','Date':'2017-01-01T12:00:00','IsRead':'true','MessageTypeId':'2'}]}";
            _webClientService.DownloadString(url).Returns(jsonString);

            var expectedData = new GeneralNotification
            {
                Id = 1,
                DateTime = new DateTime(2017, 01, 01, 12, 0, 0),
                MessageTypeId = 2,
                Content = "Content",
                Subject = "Subject",
                IsRead = true,
                Type = "General"
            };

            // Action
            var result = _notificationService.GetGeneral(metaModel);
            var data = result.DataList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(1));
                Assert.That(data.Id, Is.EqualTo(expectedData.Id));
                Assert.That(data.DateTime, Is.EqualTo(expectedData.DateTime));
                Assert.That(data.MessageTypeId, Is.EqualTo(expectedData.MessageTypeId));
                Assert.That(data.Content, Is.EqualTo(expectedData.Content));
                Assert.That(data.Subject, Is.EqualTo(expectedData.Subject));
                Assert.That(data.IsRead, Is.EqualTo(expectedData.IsRead));
                Assert.That(data.Type, Is.EqualTo(expectedData.Type));
            });
        }

        [TestCase("{}")]
        [TestCase("{'TotalItems':'0','Data':[]}")]
        public void GetGeneral_ApiReturnsNoValue_ReturnEmptyDictionary(string jsonString)
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "lang"
            };

            var url = "API9";
            _appSettings.NotificationApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(jsonString);

            // Action
            var result = _notificationService.GetGeneral(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetResultAmendment_ApiReturnsEmpty_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            var url = "API10";
            _appSettings.ResultAmendmentApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(string.Empty);

            // Action
            var result = _notificationService.GetResultAmendment(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetResultAmendment_ApiReturnsData_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "lang"
            };

            var url = "API11";
            _appSettings.ResultAmendmentApiUrl.Returns(url);

            var jsonString = "{'TotalItems':'1','AttachmentFileApi':'AttachmentFileApi',"
                + "'Data':[{'SubjectLang':'Subject','ContentLang':'Content','MessageId':'1','Date':'2017-01-01T12:00:00','IsRead':'true','DataFilePath':'Path1'}]}";
            _webClientService.DownloadString(url).Returns(jsonString);

            var expectedData = new ResultAmendment
            {
                Id = 1,
                DateTime = new DateTime(2017, 01, 01, 12, 0, 0),
                DataFilePath = $"AttachmentFileApi?site=SpecialDelete&name=Path1&saveAs=Amendment_01Jan2017_12-00-00-PM.xls",
                Content = "Content",
                Subject = "Subject",
                IsRead = true,
                Type = "Amendment"
            };

            // Action
            var result = _notificationService.GetResultAmendment(metaModel);
            var data = result.DataList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(1));
                Assert.That(data.Id, Is.EqualTo(expectedData.Id));
                Assert.That(data.DateTime, Is.EqualTo(expectedData.DateTime));
                Assert.That(data.DataFilePath, Is.EqualTo(expectedData.DataFilePath));
                Assert.That(data.Content, Is.EqualTo(expectedData.Content));
                Assert.That(data.Subject, Is.EqualTo(expectedData.Subject));
                Assert.That(data.IsRead, Is.EqualTo(expectedData.IsRead));
                Assert.That(data.Type, Is.EqualTo(expectedData.Type));
            });
        }

        [TestCase("{}")]
        [TestCase("{'TotalItems':'0','Data':[]}")]
        public void GetResultAmendment_ApiReturnsNoValue_ReturnEmptyDictionary(string jsonString)
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "lang"
            };

            var url = "API12";
            _appSettings.ResultAmendmentApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(jsonString);

            // Action
            var result = _notificationService.GetResultAmendment(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetNotificationMessages_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            var url = "API13";
            _appSettings.ResultAmendmentApiUrl.Returns(url);

            _webClientService.DownloadString(url).Returns(string.Empty);

            // Action
            var result = _notificationService.GetNotificationMessages(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }
    }
}
