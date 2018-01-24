using AutoFixture;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class FakeHelper
    {
        private static Fixture _fixture = new Fixture();

        public static T Create<T>()
        {
            return _fixture.Create<T>();
        }
    }
}