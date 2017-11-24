namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core.Utility.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Interfaces;
    using Logging;
    using Model.Core.DTO.Common;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

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
            _logger.ClearReceivedCalls();
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

        [TestCase("")]
        [TestCase("{}")]
        public void GetNotificationTypes_ApiReturnsNoValue_ReturnEmptyDictionary(string jsonString)
        {
            // Arrange
            _cache.Get<IDictionary<string, string>>(Arg.Any<string>()).Returns(x => { return null; });

            var url = "API5";
            _appSettings.NotificationTypeApiUrl.Returns(url);

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
                AssertWriteLogException(exceptionMsg, url);
            });
        }

        [Test]
        public void GetGeneral_ApiReturnsData_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "en"
            };

            var url = "API8";
            _appSettings.NotificationApiUrl.Returns(url);

            var jsonString = "{'TotalItems':'1',"
                + "'Data':[{'SubjectEn':'Subject','ContentEn':'Content','MessageId':'1','Date':'2017-01-01T12:00:00','IsRead':'true','MessageTypeId':'2'}]}";
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

        [Test]
        public void GetGeneral_LanguageIsNotSupported_ReturnDefaultLanguage()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "lang"
            };

            var url = "API8b";
            _appSettings.NotificationApiUrl.Returns(url);

            var jsonString = "{'TotalItems':'1',"
                + "'Data':[{'SubjectEn':'SubjectEn','ContentEn':'ContentEn','ContentCh':'ContentCh',"
                + "'MessageId':'1','Date':'2017-01-01T12:00:00','IsRead':'true','MessageTypeId':'2'}]}";
            _webClientService.DownloadString(url).Returns(jsonString);

            var expectedData = new GeneralNotification
            {
                Content = "ContentEn",
                Subject = "SubjectEn",
            };

            // Action
            var result = _notificationService.GetGeneral(metaModel);
            var data = result.DataList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(1));
                Assert.That(data.Subject, Is.EqualTo(expectedData.Subject));
                Assert.That(data.Content, Is.EqualTo(expectedData.Content));
            });
        }

        [TestCase("{}")]
        [TestCase("")]
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
        public void GetGeneral_ApiThrowsException_WriteLog()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            var url = "API10";
            _appSettings.NotificationApiUrl.Returns(url);

            var exceptionMsg = "Failure";
            _webClientService.When(x => x.DownloadString(url)).Do(x =>
            {
                throw new Exception(exceptionMsg);
            });

            // Action
            var result = _notificationService.GetGeneral(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
                AssertWriteLogException(exceptionMsg, url);
            });
        }

        [Test]
        public void GetResultAmendment_ApiReturnsData_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel
            {
                Language = "en"
            };

            var url = "API11";
            _appSettings.ResultAmendmentApiUrl.Returns(url);

            var jsonString = "{'TotalItems':'1','AttachmentFileApi':'AttachmentFileApi',"
                + "'Data':[{'SubjectEn':'Subject','ContentEn':'Content','MessageId':'1','Date':'2017-01-01T12:00:00','IsRead':'true','DataFilePath':'Path1'}]}";
            _webClientService.DownloadString(url).Returns(jsonString);

            var expectedData = new ResultAmendment
            {
                Id = 1,
                DateTime = new DateTime(2017, 01, 01, 12, 0, 0),
                DataFilePath = "Path1",
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
        [TestCase("")]
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
        public void GetResultAmendment_ApiThrowsException_WriteLog()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            var url = "API13";
            _appSettings.ResultAmendmentApiUrl.Returns(url);

            var exceptionMsg = "Failure";
            _webClientService.When(x => x.DownloadString(url)).Do(x =>
            {
                throw new Exception(exceptionMsg);
            });

            // Action
            var result = _notificationService.GetResultAmendment(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
                AssertWriteLogException(exceptionMsg, url);
            });
        }

        [Test]
        public void GetNotificationMessages_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new NotificationMetaModel();

            var url = "API14";
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

        [Test]
        public void MarkNotificationAsRead_WhenCall_CallApiWithCorrectParameter()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel
            {
                MessageType = "1",
                MessageId = 2,
                UserName = "3",
                LicenseeName = "Name"
            };

            var url = "http://API15";
            _appSettings.MarkNotificationAsReadApiUrl.Returns(url);

            // Action
            var result = _notificationService.MarkNotificationAsRead(metaModel);

            // Assert
            _webClientService.Received().UploadString(new Uri(url), "MessageType=1&MessageId=2&UserName=3&LicenseeName=Name");
        }

        [TestCase("{}")]
        [TestCase("")]        
        public void MarkNotificationAsRead_ApiReturnsNoData_CallApiWithCorrectParameter(string jsonString)
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel
            {
                MessageType = "1",
                MessageId = 2,
                UserName = "3",
                LicenseeName = "Name"
            };

            var url = "http://API16";
            _appSettings.MarkNotificationAsReadApiUrl.Returns(url);

            _webClientService.UploadString(new Uri(url), Arg.Any<string>()).Returns(jsonString);

            // Action
            var result = _notificationService.MarkNotificationAsRead(metaModel);

            // Assert
            _webClientService.Received().UploadString(new Uri(url), "MessageType=1&MessageId=2&UserName=3&LicenseeName=Name");
        }

        [Test]
        public void MarkNotificationAsRead_ApiReturnsData_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel
            {
                MessageType = "1",
                MessageId = 2,
                UserName = "3",
                LicenseeName = "Name"
            };

            var url = "http://API17";
            _appSettings.MarkNotificationAsReadApiUrl.Returns(url);

            var jsonString = "{'Code':'1','Message':'Error'}";
            _webClientService.UploadString(new Uri(url), Arg.Any<string>()).Returns(jsonString);

            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Error"
            };

            // Action
            var result = _notificationService.MarkNotificationAsRead(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void MarkNotificationAsRead_ApiThrowException_WriteLog()
        {
            // Arrange
            var metaModel = new NotificationMarkAsReadMetaModel
            {
                MessageType = "1",
                MessageId = 2,
                UserName = "3",
                LicenseeName = "Name"
            };

            var url = "http://API18";
            _appSettings.MarkNotificationAsReadApiUrl.Returns(url);

            var exceptionMsg = "Failure";
            _webClientService.When(x => x.UploadString(new Uri(url), Arg.Any<string>())).Do(x =>
            {
                throw new Exception(exceptionMsg);
            });

            // Action
            var result = _notificationService.MarkNotificationAsRead(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo(ViewResources.ErrorProcessingMsg));
                AssertWriteLogException(exceptionMsg, (new Uri(url)) + "\nParameter: MessageType=1&MessageId=2&UserName=3&LicenseeName=Name");
            });
        }

        [Test]
        public void GetFileAttachmentData_WhenCall_CallApiWithCorrectUrl()
        {
            // Arrange
            var metaModel = new NotificationAttachmentMetaModel
            {
                Api = "API",
                FileName = "FileName1"
            };
            var expectedUrl = "API?site=SpecialDelete&name=FileName1";

            // Action
            var result = _notificationService.GetFileAttachmentData(metaModel);

            // Assert
            _webClientService.Received().DownloadData(expectedUrl);
        }

        [Test]
        public void GetFileAttachmentData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new NotificationAttachmentMetaModel
            {
                Api = "API",
                FileName = "FileName2"
            };
            var url = "API?site=SpecialDelete&name=FileName2";
            var data = Encoding.ASCII.GetBytes("Test");

            _webClientService.DownloadData(url).Returns(data);

            // Action
            var result = _notificationService.GetFileAttachmentData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void GetFileAttachmentData_ApiThrowException_WriteLog()
        {
            // Arrange
            var metaModel = new NotificationAttachmentMetaModel
            {
                Api = "API",
                FileName = "FileName3"
            };
            var expectedUrl = "API?site=SpecialDelete&name=FileName3";

            var exceptionMsg = "Failure";
            _webClientService.When(x => x.DownloadData(expectedUrl)).Do(x =>
            {
                throw new Exception(exceptionMsg);
            });

            // Action
            var result = _notificationService.GetFileAttachmentData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Null);
                AssertWriteLogException(exceptionMsg, expectedUrl);
            });
        }

        private void AssertWriteLogException(string exceptionMsg, string api)
        {
            _logger.Received().Error($"{exceptionMsg}\nApiUrl: {api}", Arg.Any<Exception>());
        }
    }
}