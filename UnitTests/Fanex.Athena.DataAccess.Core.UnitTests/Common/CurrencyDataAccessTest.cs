namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class CurrencyDataAccessTest : BaseDataAccessHelperTest<CurrencyDataAccess>
    {
        private const string GetCurrenciesSp = "Lrf_SelCurrencyByLicensee";
        private const string AssignCurrenciesByUserSp = "Lrf_CurrencyCustIns";
        private const string GetCurrenciesByUser = "Lrf_SelCurrencyByLicenseeUser";

        [Test]
        public void GetCurrencies_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            const int LicenseeId = 1;

            DataAccess.GetObjectDb(Arg.Is(GetCurrenciesSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetCurrencies(LicenseeId);

            // Assert
            ObjectDb.ReceivedWithAnyArgs().Query<CurrencyUser>();
        }

        [Test]
        public void GetCurrencies_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            const int LicenseeId = 1;

            ObjectDb.Query<CurrencyUser>().Returns(new List<CurrencyUser>
                {
                    new CurrencyUser { CurrencyId = 1, AgentId = 100 },
                    new CurrencyUser { CurrencyId = 1, AgentId = 200 }
                });
            DataAccess.GetObjectDb(Arg.Is(GetCurrenciesSp)).Returns(ObjectDb);

            // Action
            var result = DataAccess.GetCurrencies(LicenseeId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].AgentIdList.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public void GetCurrencies_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(GetCurrenciesSp, () => DataAccess.GetCurrencies(1));
        }

        [Test]
        public void AssignCurrenciesByUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(AssignCurrenciesByUserSp)).Returns(ObjectDb);
            var dataModel = new AssignCurrencyMetaModel
            {
                UserId = 1,
                LicenseeId = 1,
                CurrencyList = "USD, UUS, EUR",
                ModifyBy = 2,
                LangKey = "en-US",
                IP = "IP"
            };

            // Action
            DataAccess.AssignCurrenciesByUser(dataModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("UserID", DbType.Int32, dataModel.UserId);
                ObjectDb.Received(1).AddInParameter("LicenseeID", DbType.Int32, dataModel.LicenseeId);
                ObjectDb.Received(1).AddInParameter("CurrencyXML", DbType.String, dataModel.CurrencyList);
                ObjectDb.Received(1).AddInParameter("ModifyBy", DbType.Int32, dataModel.ModifyBy);
                ObjectDb.Received(1).AddInParameter("LangKey", DbType.String, dataModel.LangKey);
                ObjectDb.Received(1).AddInParameter("IP", DbType.String, dataModel.IP);
                ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void AssignCurrenciesByUser_WhenCall_ResultInfoReturnCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(AssignCurrenciesByUserSp)).Returns(ObjectDb);
            ResultInfoHelper.MockResultInfo(ObjectDb);

            // Action
            var result = DataAccess.AssignCurrenciesByUser(new AssignCurrencyMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
                Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
            });
        }

        [Test]
        public void GetCurrenciesByUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int LicenseeId = 1;
            const int UserId = 2;
            DataAccess.GetObjectDb(Arg.Is(GetCurrenciesByUser)).Returns(ObjectDb);

            // Action
            DataAccess.GetCurrenciesByUser(LicenseeId, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("LicenseeID", DbType.Int32, LicenseeId);
                ObjectDb.Received(1).AddInParameter("UserID", DbType.Int32, UserId);
            });
        }

        [Test]
        public void GetCurrenciesByUser_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(GetCurrenciesByUser).Query<CurrencyUser>()
                .Returns(new List<CurrencyUser>
                {
                    new CurrencyUser { CurrencyId = 1, AgentId = 100 },
                    new CurrencyUser { CurrencyId = 1, AgentId = 200 }
                });

            // Action
            var currencyList = DataAccess.GetCurrenciesByUser(0, 0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(currencyList.Count, Is.EqualTo(1));
                Assert.That(currencyList[0].AgentIdList.Count(), Is.EqualTo(2));
            });
        }
    }
}