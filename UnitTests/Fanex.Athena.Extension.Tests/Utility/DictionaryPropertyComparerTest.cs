namespace Fanex.Athena.Extension.Tests.Utility
{
    using System.Collections.Generic;
    using ClassTest;
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class DictionaryPropertyComparerTest
    {
        [TestCase(10, -1)]
        [TestCase(11, 0)]
        [TestCase(12, 1)]
        public void Compare_AscendingDirection_ReturnCorrecvalue(int keyValue, int expected)
        {
            // Arrange            
            var firstObject = new Person { Id = 1, Name = "Walter", Children = CreateData("Ariana", "Selena", "Kross") };
            var secondObject = new Person { Id = 2, Name = "Ralph", Children = CreateData("Kyle", "Selena", "Alex") };
            var comparer = new DictionaryPropertyComparer<Person, int, string>(keyValue, "Children", System.ComponentModel.ListSortDirection.Ascending);

            // Action
            var result = comparer.Compare(firstObject, secondObject);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(10, 1)]
        [TestCase(11, 0)]
        [TestCase(12, -1)]
        public void Compare_DescendingDirection_ReturnCorrecvalue(int keyValue, int expected)
        {
            // Arrange            
            var firstObject = new Person { Id = 1, Name = "Walter", Children = CreateData("Ariana", "Selena", "Kross") };
            var secondObject = new Person { Id = 2, Name = "Ralph", Children = CreateData("Kyle", "Selena", "Alex") };
            var comparer = new DictionaryPropertyComparer<Person, int, string>(keyValue, "Children", System.ComponentModel.ListSortDirection.Descending);

            // Action
            var result = comparer.Compare(firstObject, secondObject);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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