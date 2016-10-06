namespace Fanex.Athena.DataAccess.Core.UnitTests.Utilities
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Utilities;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SafeDataReaderTest
    {
        private IDataReader _dataReader;
        private IEnumerable<string> _columns = new string[] { "Column1", "Column2" };

        [SetUp]
        public void Setup()
        {
            _dataReader = Substitute.For<IDataReader>();
            _dataReader.FieldCount.Returns(1);
            _dataReader.GetName(0).Returns("Column1");
            _dataReader.GetName(1).Returns("Column2");
        }

        [Test]
        public void GetValue_WhenCallGenericMethodAndContainColumn_ReturnCorrectValue()
        {
            // Arrange
            var column = "Column1";
            var expected = "Result";
            _dataReader[column].Returns(expected);
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(column, x => expected, "Default");

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetValue_WhenCallGenericMethodAndNotContainColumn_ReturnDefaultValue()
        {
            // Arrange
            var column = "Column3";
            var expected = "Default";
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(column, x => "Value", expected);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetValue_WhenCallNormalMethodAndContainColumn_ReturnCorrectValue()
        {
            // Arrange
            var column = "Column1";
            var expected = "Result";
            _dataReader[column].Returns(expected);
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(column);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetValue_WhenCallNormalMethodAndNotContainColumn_ReturnDefaultValue()
        {
            // Arrange
            var column = "Column3";
            var expected = "Default";
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(column, expected);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}