namespace Fanex.Athena.Extension.UnitTests
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    public class HashtableExtensionsTest
    {
        [Test]
        public void UpdateWith_DataIsUpdated()
        {
            // Arrange
            var firstKeys = new int[] { 1, 2, 3 };
            var firstValues = new string[] { "A", "B", "C" };
            var firstHashable = CreateData(firstKeys, firstValues);

            var secondKeys = new int[] { 1, 3, 4 };
            var secondValues = new string[] { "A", "D", "E" };
            var secondHashable = CreateData(secondKeys, secondValues);

            // Action
            firstHashable.UpdateWith(secondHashable);

            // Assert
            Assert.That(firstHashable[1], Is.EqualTo("A"));
            Assert.That(firstHashable[2], Is.EqualTo("B"));
            Assert.That(firstHashable[3], Is.EqualTo("D"));
        }

        private Hashtable CreateData(int[] keys, string[] values)
        {
            var hashtable = new Hashtable();
            for (int i = 0; i < 3; i++)
            {
                hashtable.Add(keys[i], values[i]);
            }

            return hashtable;
        }
    }
}