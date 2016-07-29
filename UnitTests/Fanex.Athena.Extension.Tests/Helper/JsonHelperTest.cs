namespace Fanex.Athena.Extension.Tests.Helper
{
    using Fanex.Athena.Extension.Helper;
    using NUnit.Framework;

    [TestFixture]
    public class JsonHelperTest
    {
        [Test]
        public void ToJson_WithObject_ReturnCorrectResult()
        {
            //Arrange
            var obj = new { Id = 1, Name = "Test1" };
            var expected = "{\"Id\":1,\"Name\":\"Test1\"}";

            //Action
            var result = obj.ToJson();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToJson_WithCollection_ReturnCorrectResult()
        {
            //Arrange
            var list = new[] {
                        new { Id = 1, Name = "Test1" },
                        new { Id = 2, Name = "Test2" },
                        new { Id = 3, Name = "Test3" }
                        };

            var expected = "[{\"Id\":1,\"Name\":\"Test1\"},{\"Id\":2,\"Name\":\"Test2\"},{\"Id\":3,\"Name\":\"Test3\"}]";

            //Action
            var result = list.ToJson();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToJson_WithComplexObject_ReturnCorrectResult()
        {
            //Arrange
            var list = new[] {
                        new { Id = 1, Name = "Test1" },
                        new { Id = 2, Name = "Test2" },
                        new { Id = 3, Name = "Test3" }
                        };
            var obj = new { Id = 0, Name = "Object", DataList = list };

            var expected = "{\"Id\":0,\"Name\":\"Object\",\"DataList\":[{\"Id\":1,\"Name\":\"Test1\"},{\"Id\":2,\"Name\":\"Test2\"},{\"Id\":3,\"Name\":\"Test3\"}]}";

            //Action
            var result = obj.ToJson();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToJson_WithNullValue_ReturnCorrectResult()
        {
            //Arrange
            string obj = null;
            var expected = "null";

            //Action
            var result = obj.ToJson();

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}