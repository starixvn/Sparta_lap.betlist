namespace Fanex.Athena.DataAccess.Core.UnitTests.Configuration
{
    using Core.Configuration;
    using Data;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class MoveBetDataAccessTest : BaseDataAccessHelperTest<MoveBetDataAccess>
    {
        private const string UspName = "Acc_ShowNotificationWhenProcessingData";

        private ILogger _logger;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _logger = Substitute.For<ILogger>();
            base.FixtureSetUp();
        }

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<MoveBetDataAccess>(_logger);
        }

        [Test]
        public void GetMoveBet_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var data = new
            {
                Content_EN = "hello",
                Content_CS = "nee how",
                Content_CH = "ne ho",
                Content_CN = "nee how",
                Content_TH = "sawa dee-krap",
                Content_JP = "konnichi ha",
                Content_KO = "ahn nyeong ha se yo",
                Content_VI = "xin chao",
            };
            MapReaderData(data);
            Reader.Read().Returns(true, false);
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            // Action
            var result = DataAccess.GetMoveBetNotification();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ContentByLanguage["en-US"], Is.EqualTo(data.Content_EN));
                Assert.That(result.ContentByLanguage["zh-CN"], Is.EqualTo(data.Content_CS));
                Assert.That(result.ContentByLanguage["zh-TW"], Is.EqualTo(data.Content_CH));
                Assert.That(result.ContentByLanguage["zh-Hans"], Is.EqualTo(data.Content_CN));
                Assert.That(result.ContentByLanguage["th-TH"], Is.EqualTo(data.Content_TH));
                Assert.That(result.ContentByLanguage["ja-JP"], Is.EqualTo(data.Content_JP));
                Assert.That(result.ContentByLanguage["ko-KR"], Is.EqualTo(data.Content_KO));
                Assert.That(result.ContentByLanguage["vi-VN"], Is.EqualTo(data.Content_VI));
            });
        }

        [Test]
        public void GetMoveBet_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            Reader.Read().Returns(false);

            // Action
            var result = DataAccess.GetMoveBetNotification();

            // Assert
            Assert.That(result.ContentByLanguage, Is.Empty);
        }

        [Test]
        public void GetMoveBetNotification_WhenObjectDbThrowsException_WriteLog()
        {
            // Arrange
            var sqlException = new ObjectDbException("Error");
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(x => { throw sqlException; });

            // Action
            DataAccess.GetMoveBetNotification();

            // Assert
            _logger.Received().Error("MoveBetNotification: " + sqlException.Message, sqlException);
        }
    }
}