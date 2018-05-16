namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using Extension.Utility;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class BaseDataAccessTest
    {
        private BaseDataAccess _baseDataAccess;

        [SetUp]
        public void SetUp()
        {
            _baseDataAccess = Substitute.ForPartsOf<BaseDataAccess>(Substitute.For<IAppSettings>(), null);
        }

        [Test]
        public void GetSpNameForFakeLicensee_FakeLicensee_ReturnCorrectSP()
        {
            // Arrange
            var spname = FakeHelper.Create<string>();
            _baseDataAccess.When(x => x.IsFakeLicensee(Arg.Any<int>())).DoNotCallBase();
            _baseDataAccess.IsFakeLicensee(Arg.Any<int>()).Returns(true);

            // Action
            var result = _baseDataAccess.GetSpNameForFakeLicensee(0, spname);

            // Assert
            Assert.That(result, Is.EqualTo(spname + "_BySourced"));
        }
    }
}