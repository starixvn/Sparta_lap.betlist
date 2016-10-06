namespace Fanex.Athena.Extension.UnitTests.Utility
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
            Assert.That(firstObject, Is.EqualTo(dataList[0]));
            Assert.That(secondObject, Is.EqualTo(dataList[1]));
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
            Assert.That(firstObject, Is.EqualTo(dataList[1]));
            Assert.That(secondObject, Is.EqualTo(dataList[0]));
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
            Assert.That(firstObject, Is.EqualTo(dataList[1]));
            Assert.That(secondObject, Is.EqualTo(dataList[0]));
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
            Assert.That(firstObject, Is.EqualTo(dataList[0]));
            Assert.That(secondObject, Is.EqualTo(dataList[1]));
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