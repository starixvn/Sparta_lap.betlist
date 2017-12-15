namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.Common.Constants;
    using Core.Common.Interfaces;
    using Data;
    using Model.BetSettings.MetaModel;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoreCasinoBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private IUserLogDataAccess _userLogDataAccess;

        private CoreCasinoBetSettingDataAccess _dataAccess;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _userLogDataAccess = Substitute.For<IUserLogDataAccess>();
            _dataAccess = Substitute.ForPartsOf<CoreCasinoBetSettingDataAccess>(_userLogDataAccess);
        }

        [Test]
        public void UpdateCasinoStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdCasinoStatus")).Returns(_objectDb);
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
            _dataAccess.UpdateCasinoStatus(metaModel, sportId);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, metaModel.CustId);
                _objectDb.Received().AddInParameter("sportid", DbType.Int32, sportId);
                _objectDb.Received().AddInParameter("language", DbType.String, metaModel.Language);
                _objectDb.Received().AddInParameter("ip", DbType.String, metaModel.Ip);
                _objectDb.Received().AddInParameter("status", DbType.Int32, metaModel.Status);
                _objectDb.Received().AddOutParameter("ErrCode", DbType.String, 5);
                _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 6000);
            });
        }

        [Test]
        public void WriteLogUpdatingAction_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const string SPName = "Lrf_CustomerUpdCasinoStatus";
            _dataAccess.GetObjectDb(Arg.Is(SPName)).Returns(_objectDb);
            var metaModel = new CoreCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = 0,
                Language = "abc",
                Ip = "::1"
            };
            var sportId = 3;

            // Action
            _dataAccess.UpdateCasinoStatus(metaModel, sportId);

            // Assert
            _userLogDataAccess.Received(1).WriteLog(Arg.Is<UserLogMetaModel>(x =>
                x.ActionID == UserLogAction.LiveCasinoBetSetting &&
                x.CustID == metaModel.CustId &&
                x.UserId == metaModel.UserId &&
                x.SPName == SPName &&
                x.TrackingValue1 == 
                "<Lrf_CustomerUpdCasinoStatus UserID=\"1\" CustID=\"2\" SportID=\"3\" Status=\"1\" Ip=\"::1\" Lang=\"abc\" RoleID=\"1\" ErrCode=\"0\" ErrMsg=\"\"/>" &&
                x.TrackingValue2 == 
                "<Lrf_CustomerUpdCasinoStatus UserID=\"1\" CustID=\"2\" SportID=\"3\" Status=\"0\" Ip=\"::1\" Lang=\"abc\" RoleID=\"1\" ErrCode=\"0\" ErrMsg=\"\"/>"));
        }
    }
}