namespace Fanex.Athena.Extension.Tests.Utility
{
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class PropertyComparerTest
    {
        [TestCase("Id", -1)]
        [TestCase("NickName", 0)]
        [TestCase("Name", 1)]       
        public void Compare_AscendingDirection_ReturnCorrectValue(string propertyName, int expected)
        {
            // Arrange
            var comparer = new PropertyComparer<MyClass>(propertyName, System.ComponentModel.ListSortDirection.Ascending);
            var firstObject = new MyClass { Id = 1, Name = "Walter", NickName = "xxx" };
            var secondObject = new MyClass { Id = 2, Name = "Ralph", NickName = "xxx" };

            // Action
            var result = comparer.Compare(firstObject, secondObject);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("Id", 1)]
        [TestCase("NickName", 0)]
        [TestCase("Name", -1)]
        public void Compare_DescendingDirection_ReturnCorrectValue(string propertyName, int expected)
        {
            // Arrange
            var comparer = new PropertyComparer<MyClass>(propertyName, System.ComponentModel.ListSortDirection.Descending);
            var firstObject = new MyClass { Id = 1, Name = "Walter", NickName = "xxx" };
            var secondObject = new MyClass { Id = 2, Name = "Ralph", NickName = "xxx" };

            // Action
            var result = comparer.Compare(firstObject, secondObject);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Compare_PropertyNotExistInFirstObject_ReturnCorrectValue()
        {
            // Arrange
            var comparer = new PropertyComparer<MyClass>("NickName", System.ComponentModel.ListSortDirection.Ascending);
            var firstObject = new MyClass { Id = 1, Name = "Walter" };
            var secondObject = new MyClass { Id = 2, Name = "Ralph", NickName = "xxx" };
            const int Expected = -1;

            // Action
            var result = comparer.Compare(firstObject, secondObject);

            // Assert
            Assert.AreEqual(Expected, result);
        }

        [Test]
        public void Compare_PropertyNotExistInSecondObject_ReturnCorrectValue()
        {
            // Arrange
            var comparer = new PropertyComparer<MyClass>("NickName", System.ComponentModel.ListSortDirection.Ascending);
            var firstObject = new MyClass { Id = 1, Name = "Walter", NickName = "xxx" };
            var secondObject = new MyClass { Id = 2, Name = "Ralph" };
            const int Expected = 0;

            // Action
            var result = comparer.Compare(firstObject, secondObject);

            // Assert
            Assert.AreEqual(Expected, result);
        }

        internal class MyClass
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string NickName { get; set; }
        }
    }   
}