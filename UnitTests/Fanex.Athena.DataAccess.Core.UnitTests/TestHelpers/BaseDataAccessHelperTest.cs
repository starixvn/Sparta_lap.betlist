namespace Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Exception;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;

    public class BaseDataAccessHelperTest<T> where T : BaseDataAccess
    {
        protected const int CurrencyId = 123;

        protected const string CurrencyName = "aaaaaaaaa";

        protected IObjectDb ObjectDb { get; set; }

        protected IDataReader Reader { get; set; }

        protected T DataAccess { get; set; }

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
        }

        [TearDown]
        public virtual void TearDown()
        {
            DataAccess.ClearReceivedCalls();
            Reader = null;
        }

        protected ISessionStorage CreateSessionStorage()
        {
            var sessionStorage = Substitute.For<ISessionStorage>();
            sessionStorage.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { CurrencyId, CurrencyName }
                }
            });

            return sessionStorage;
        }

        protected virtual void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<T>();
        }

        protected void MapReaderData(object readerDataMapping)
        {
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);
        }

        protected void TestException_WhenObjectDbThrowsException_ThrowDataAccessException(string uspName, TestDelegate testDelegate)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(x => { throw new ObjectDbException(); });

            // Assert
            Assert.Throws<DataAccessException>(testDelegate, Arg.Is<string>(x => x.Contains(uspName)));
        }

        protected void StubGetResultInfo(int errCode, string errMsg)
        {
            ObjectDb.GetParameterValue("ErrCode").Returns(errCode);
            ObjectDb.GetParameterValue("ErrMsg").Returns(errMsg);
        }
    }
}