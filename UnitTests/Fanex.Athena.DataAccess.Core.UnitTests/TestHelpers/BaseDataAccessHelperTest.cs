namespace Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers
{
    using System.Data;
    using Core.Exception;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public abstract class BaseDataAccessHelperTest<T> where T : BaseDataAccess
    {
        protected IObjectDb ObjectDb { get; set; }

        protected IDataReader Reader { get; set; }

        protected T DataAccess { get; set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            DataAccess = Substitute.ForPartsOf<T>();
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
            Reader = Substitute.For<IDataReader>();
            ObjectDb.ExecuteReader().Returns(Reader);
            ObjectDb.ExecuteReader(Arg.Any<object>()).Returns(Reader);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Reader = null;
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