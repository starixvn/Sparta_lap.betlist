namespace Fanex.Athena.Extension.Tests.Utility
{
    using System;
    using System.Collections.Generic;
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class DictionayExtensionTest
    {
        [TestCase("SB", 0, 1000)]
        [TestCase("CS", 0, 143.22)]
        [TestCase("BT", 0, 0)]
        [TestCase("BT", 100, 100)]
        public void GetValueOrDefault_WhenCallWithDefaultValue_ValueIsGotCorrectly(string keyValue, object defaultValue, object expected)
        {
            // Arrange
            var dataList = CreateData();

            // Action
            var result = dataList.GetValueOrDefault(keyValue, Convert.ToDecimal(defaultValue));

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("SB", 0, 1000)]
        [TestCase("CS", 0, 143.22)]
        [TestCase("BT", 0, 0)]
        [TestCase("BT", 100, 100)]
        public void GetValueOrDefault_WhenCallWithDefaultValueProvider_ValueIsGotCorrectly(string keyValue, object defaultValue, object expected)
        {
            // Arrange
            var dataList = CreateData();

            // Action
            var result = dataList.GetValueOrDefault(keyValue, () => Convert.ToDecimal(defaultValue));

            // Assert
            Assert.AreEqual(expected, result);
        }

        private IDictionary<string, decimal> CreateData()
        {
            var dictionary = new Dictionary<string, decimal>();
            dictionary.Add("SB", 1000m);
            dictionary.Add("CS", 143.22m);

            return dictionary;
        }
    }
}