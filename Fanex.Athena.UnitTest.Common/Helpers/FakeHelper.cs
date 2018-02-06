using System.Collections.Generic;
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

        public static IEnumerable<T> CreateMany<T>()
        {
            return _fixture.CreateMany<T>();
        }
    }
}