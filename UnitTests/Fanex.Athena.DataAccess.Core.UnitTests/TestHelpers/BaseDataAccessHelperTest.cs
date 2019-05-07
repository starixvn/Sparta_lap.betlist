namespace Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class BaseDataAccessHelperTest<T> where T : BaseDataAccess
    {
        protected const int CurrencyId = 123;

        protected const string CurrencyName = "aaaaaaaaa";

        private readonly IDictionary<int, string> _userCurrencyList = new Dictionary<int, string>
        {
            { CurrencyId, CurrencyName }, { 1, "USD" }
        };

        protected IObjectDb ObjectDb { get; set; }

        protected IDataReader Reader { get; set; }

        protected T DataAccess { get; set; }

        protected IDictionary<int, string> UserCurrencyList => _userCurrencyList;

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            SetUpDataAccess();
        }

        [OneTimeTearDown]
        public virtual void FixtureTearDown()
        {
            ObjectDb = null;
            DataAccess = null;
        }

        [SetUp]
        public virtual void SetUp()
        {
            ObjectDb.ClearReceivedCalls();
            Reader = Substitute.For<IDataReader>();
            ObjectDb.ExecuteReader().Returns(Reader);
            ObjectDb.ExecuteReader(Arg.Any<object>()).Returns(Reader);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            ObjectDb.ExecuteReaderAsync(Arg.Any<object>()).Returns(Reader);
        }

        [TearDown]
        public virtual void TearDown()
        {
            DataAccess.ClearReceivedCalls();
            Reader = null;
        }

        protected virtual void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<T>();
        }

        protected void MapReaderData(object readerDataMapping)
        {
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);
        }

        protected void StubGetResultInfo(int errCode, string errMsg)
        {
            ObjectDb.GetParameterValue("ErrCode").Returns(errCode);
            ObjectDb.GetParameterValue("ErrMsg").Returns(errMsg);
        }
    }
}