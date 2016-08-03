namespace Fanex.Athena.Extension.Tests.Utility
{
    using System.Collections.Generic;
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class GroupCommSettingTest
    {
        [Test]
        public void CommissionsProperty_CorrectValue()
        {
            // Arrange
            var keys = new decimal[] { 0, 0.05m, 15 };
            var values = new decimal[] { 0.0m, 0.0005m, 0.15m };
            var dataList = CreateData<decimal, decimal>(keys, values);

            // Action
            var commissions = GroupCommSetting.Commissions;

            // Assert
            Assert.AreEqual(dataList[0], commissions[0]);
            Assert.AreEqual(dataList[0.05m], commissions[0.05m]);
            Assert.AreEqual(dataList[15], commissions[15]);
        }

        [Test]
        public void ValidCommissionsProperty_CorrectValue()
        {
            // Arrange
            var keys = new decimal[] { 0, 0.05m, 15 };
            var values = new decimal[] { 0.0m, 0.0005m, 0.15m };
            var dataList = CreateData<decimal, decimal>(keys, values);

            // Action
            var validCommissions = GroupCommSetting.ValidCommissions;

            // Assert
            Assert.AreEqual(dataList[0], validCommissions[0]);
            Assert.AreEqual(dataList[0.05m], validCommissions[0.05m]);
            Assert.AreEqual(dataList[15], validCommissions[15]);
        }

        [TestCase(2)]
        [TestCase(10)]
        public void GetCommValue_WhenCall_ReturnCorrectValue(int increase)
        {
            // Arrange
            var max = 2;
            var keys = new int[] { 0 * increase, 1 * increase, 2 * increase };
            var values = new int[] { 0 * increase, 1 * increase, 2 * increase };
            var expected = CreateData<int, int>(keys, values);

            // Action
            var result = GroupCommSetting.GetCommValue(max, increase);

            // Assert
            Assert.AreEqual(expected, result);
        }

        private IDictionary<TKey, TValue> CreateData<TKey, TValue>(TKey[] keys, TValue[] values)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            for (int i = 0; i < 3; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }

            return dictionary;
        }
    }
}