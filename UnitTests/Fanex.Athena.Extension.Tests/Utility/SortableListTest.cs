namespace Fanex.Athena.Extension.Tests.Utility
{
    using System.Collections.Generic;
    using ClassTest;
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class SortableListTest
    {
        [Test]
        public void SortByDictionary_AscendingDirection_ListSortedCorrectly()
        {
            // Arrange            
            var firstObject = new Person { Id = 1, Name = "Walter", Children = CreateData("Ariana", "Selena", "Kross") };
            var secondObject = new Person { Id = 2, Name = "Ralph", Children = CreateData("Kyle", "Selena", "Alex") };
            var dataList = new SortableList<Person>() { firstObject, secondObject };

            // Action
            dataList.SortByDictionary<int, string>(10, "Children", true);

            // Assert
            Assert.AreEqual(firstObject, dataList[0]);
            Assert.AreEqual(secondObject, dataList[1]);
        }

        [Test]
        public void SortByDictionary_DescendingDirection_ListSortedCorrectly()
        {
            // Arrange            
            var firstObject = new Person { Id = 1, Name = "Walter", Children = CreateData("Ariana", "Selena", "Kross") };
            var secondObject = new Person { Id = 2, Name = "Ralph", Children = CreateData("Kyle", "Selena", "Alex") };
            var dataList = new SortableList<Person>() { firstObject, secondObject };

            // Action
            dataList.SortByDictionary<int, string>(10, "Children", false);

            // Assert
            Assert.AreEqual(secondObject, dataList[0]);
            Assert.AreEqual(firstObject, dataList[1]);
        }

        [Test]
        public void Sort_AscendingDirection_ListSortedCorrectly()
        {
            // Arrange            
            var firstObject = new Person { Id = 1, Name = "Walter" };
            var secondObject = new Person { Id = 2, Name = "Ralph" };
            var dataList = new SortableList<Person>() { firstObject, secondObject };

            // Action
            dataList.Sort("Name", true);

            // Assert
            Assert.AreEqual(secondObject, dataList[0]);
            Assert.AreEqual(firstObject, dataList[1]);
        }

        [Test]
        public void Sort_DescendingDirection_ListSortedCorrectly()
        {
            // Arrange            
            var firstObject = new Person { Id = 1, Name = "Walter" };
            var secondObject = new Person { Id = 2, Name = "Ralph" };
            var dataList = new SortableList<Person>() { firstObject, secondObject };

            // Action
            dataList.Sort("Name", false);

            // Assert
            Assert.AreEqual(firstObject, dataList[0]);
            Assert.AreEqual(secondObject, dataList[1]);
        }

        private IDictionary<int, string> CreateData(params string[] names)
        {
            var dictionary = new Dictionary<int, string>();
            dictionary.Add(10, names[0]);
            dictionary.Add(11, names[1]);
            dictionary.Add(12, names[2]);

            return dictionary;
        }
    }
}