namespace Fanex.Athena.DataAccess.Core.UnitTests.Configuration
{
    using System.Data;
    using Core.Configuration;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MoveBetDataAccessTest
    {
        [Test]
        public void GetMoveBet_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const string ExpectedContentEn = "hello";
            const string ExpectedContentCs = "nee how";
            const string ExpectedContentCh = "ne ho";
            const string ExpectedContentTh = "sawa dee-krap";
            const string ExpectedContentJp = "konnichi ha";
            const string ExpectedContentKo = "ahn nyeong ha se yo";
            const string ExpectedContentVi = "xin chao";

            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true);
            reader["Content_EN"].Returns(ExpectedContentEn);
            reader["Content_CS"].Returns(ExpectedContentCs);
            reader["Content_CH"].Returns(ExpectedContentCh);
            reader["Content_TH"].Returns(ExpectedContentTh);
            reader["Content_JP"].Returns(ExpectedContentJp);
            reader["Content_KO"].Returns(ExpectedContentKo);
            reader["Content_VI"].Returns(ExpectedContentVi);

            var objectDb = Substitute.For<IObjectDb>();
            objectDb.ExecuteReader().Returns(reader);
            var dataAccess = Substitute.ForPartsOf<MoveBetDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Acc_ShowNotificationWhenProcessingData")).Returns(objectDb);

            // Action
            var result = dataAccess.GetMoveBetNotification();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ContentByLanguage["en-US"], Is.EqualTo(ExpectedContentEn));
                Assert.That(result.ContentByLanguage["zh-CN"], Is.EqualTo(ExpectedContentCs));
                Assert.That(result.ContentByLanguage["zh-TW"], Is.EqualTo(ExpectedContentCh));
                Assert.That(result.ContentByLanguage["th-TH"], Is.EqualTo(ExpectedContentTh));
                Assert.That(result.ContentByLanguage["ja-JP"], Is.EqualTo(ExpectedContentJp));
                Assert.That(result.ContentByLanguage["ko-KR"], Is.EqualTo(ExpectedContentKo));
                Assert.That(result.ContentByLanguage["vi-VN"], Is.EqualTo(ExpectedContentVi));
            });
        }
    }
}