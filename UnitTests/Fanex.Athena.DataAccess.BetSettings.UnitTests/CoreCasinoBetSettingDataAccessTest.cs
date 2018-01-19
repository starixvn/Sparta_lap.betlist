namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.Common.Constants;
    using Core.Common.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Model.BetSettings.MetaModel;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoreCasinoBetSettingDataAccessTest : BaseDataAccessHelperTest<CoreCasinoBetSettingDataAccess>
    {
        private IUserLogDataAccess _userLogDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            _userLogDataAccess = Substitute.For<IUserLogDataAccess>();
            DataAccess = Substitute.ForPartsOf<CoreCasinoBetSettingDataAccess>(_userLogDataAccess);
        }

        [Test]
        public void UpdateCasinoStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdCasinoStatus")).Returns(ObjectDb);
            var metaModel = new CoreCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = 0,
                Language = "LanguageTest",
                Ip = "::1"
            };
            var sportId = 3;

            // Action
            DataAccess.UpdateCasinoStatus(metaModel, sportId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, metaModel.CustId);
                ObjectDb.Received().AddInParameter("sportid", DbType.Int32, sportId);
                ObjectDb.Received().AddInParameter("language", DbType.String, metaModel.Language);
                ObjectDb.Received().AddInParameter("ip", DbType.String, metaModel.Ip);
                ObjectDb.Received().AddInParameter("status", DbType.Int32, metaModel.Status);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.String, 5);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 6000);
            });
        }

        [TestCase(1, 0)]
        [TestCase(2, 1)]
        public void WriteLogUpdatingAction_WhenCall_ParametersPassedCorrectly(int status, int expectedTrackingStatus1)
        {
            // Arrange
            const string TrackingValueTemplate = "<Lrf_CustomerUpdCasinoStatus UserID=\"1\" CustID=\"2\" SportID=\"3\" "
                + "Status=\"{0}\" Ip=\"::1\" Lang=\"abc\" RoleID=\"1\" ErrCode=\"0\" ErrMsg=\"\"/>";
            const string SPName = "Lrf_CustomerUpdCasinoStatus";
            DataAccess.GetObjectDb(Arg.Is(SPName)).Returns(ObjectDb);
            var metaModel = new CoreCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = status,
                Language = "abc",
                Ip = "::1"
            };
            const int SportId = 3;

            // Action
            DataAccess.UpdateCasinoStatus(metaModel, SportId);

            // Assert
            _userLogDataAccess.Received(1).WriteLog(Arg.Is<UserLogMetaModel>(x =>
                x.ActionID == UserLogAction.LiveCasinoBetSetting &&
                x.CustID == metaModel.CustId &&
                x.UserId == metaModel.UserId &&
                x.SPName == SPName &&
                x.TrackingValue1 == string.Format(TrackingValueTemplate, expectedTrackingStatus1) &&
                x.TrackingValue2 == string.Format(TrackingValueTemplate, status)));
        }
    }
}