namespace Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers
{
    using System.Data;
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
    }
}