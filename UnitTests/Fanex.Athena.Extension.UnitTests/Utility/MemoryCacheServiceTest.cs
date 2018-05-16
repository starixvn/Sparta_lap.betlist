namespace Fanex.Athena.Extension.UnitTests.Utility
{
    using Caching;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using NSubstitute;
    using NUnit.Framework;

    public class MemoryCacheServiceTest
    {
        private IMemoryCacheService _service;

        private ICache _cache;

        [OneTimeSetUp]
        public void SetUp()
        {
            _cache = Substitute.For<ICache>();
            _service = new MemoryCacheService(_cache);
        }

        [Test]
        public void Contains_WhenCall_CallRightMethodWithCorrectParameters()
        {
            // Action
            _service.Contains(string.Empty);

            // Assert
            _cache.Received().Contains(string.Empty);
        }

        [Test]
        public void Get_WhenCall_ReturnClonedObject()
        {
            // Arrange
            const string OriginalData = "Test Get";
            const string Key = "KeyGet";
            _cache.Get<string>(Key).Returns(OriginalData);

            // Action
            var result = _service.Get<string>(Key);

            // Assert
            Assert.That(result, Is.EqualTo(OriginalData));
        }

        [Test]
        public void Set_WhenCall_CallRightMethodWithCorrectParameters()
        {
            // Arrange
            const string Data = "Test Set";
            const string Key = "KeySet";
            const int TimeOut = 10;

            // Action
            _service.Set(Key, Data, TimeOut);

            // Assert
            _cache.Received().Set(Key, Data, TimeOut);
        }

        [Test]
        public void Clear_WhenCall_CallRightMethodWithCorrectParameters()
        {
            // Action
            _service.Clear();

            // Assert
            _cache.Received().Clear();
        }

        [Test]
        public void Delete_WhenCall_CallRightMethodWithCorrectParameters()
        {
            // Action
            _service.Delete(string.Empty);

            // Assert
            _cache.Remove(string.Empty);
        }
    }
}