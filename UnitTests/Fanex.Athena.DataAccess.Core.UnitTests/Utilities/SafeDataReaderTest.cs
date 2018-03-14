namespace Fanex.Athena.DataAccess.Core.UnitTests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Utilities;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SafeDataReaderTest
    {
        private IDataReader _dataReader;
        private IEnumerable<string> _columns = new[] { "Column1", "Column2" };

        [OneTimeSetUp]
        public void Setup()
        {
            _dataReader = Substitute.For<IDataReader>();
            _dataReader.FieldCount.Returns(1);
            _dataReader.GetName(0).Returns("Column1");
            _dataReader.GetName(1).Returns("Column2");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _columns = null;
            _dataReader = null;
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Read_WhenCall_ReturnCorrectValue(bool expectedValue)
        {
            // Arrange
            _dataReader.Read().Returns(expectedValue);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.Read();

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_WhenCallGenericMethodAndContainColumn_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            const string Expected = "Result";
            _dataReader[Column].Returns(Expected);
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(Column, x => Expected, "Default");

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetValue_WhenCallGenericMethodAndNotContainColumn_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            const string Expected = "Default";
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(Column, x => "Value", Expected);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetValue_WhenCallNormalMethodAndContainColumn_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            const string Expected = "Result";
            _dataReader[Column].Returns(Expected);
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(Column);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetValue_WhenCallNormalMethodAndNotContainColumn_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            const string Expected = "Default";
            var reader = new SafeDataReader(_dataReader, _columns);

            // Action
            var result = reader.GetValue(Column, Expected);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetString_ColumnNameDoesNotExist_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            const string Expected = "";

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetString(Column);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetString_ColumnNameExists_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            const string Expected = Column;

            _dataReader[Column].Returns(Expected);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetString(Column, Expected);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetInt_ColumnNameDoesNotExist_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            const int Expected = 0;

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetInt(Column);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetInt_ColumnNameExists_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            const int Expected = 2;

            _dataReader[Column].Returns(Expected);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetInt(Column, Expected);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetLong_ColumnNameDoesNotExist_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            const long Expected = 0;

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetLong(Column);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetLong_ColumnNameExists_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            const long Expected = 2000;

            _dataReader[Column].Returns(Expected);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetLong(Column, Expected);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetDecimal_ColumnNameDoesNotExist_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            const decimal Expected = 0;

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetDecimal(Column);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetDecimal_ColumnNameExists_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            const decimal Expected = 2000.123m;

            _dataReader[Column].Returns(Expected);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetDecimal(Column, Expected);

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }

        [Test]
        public void GetDateTime_ColumnNameDoesNotExist_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            var expected = new DateTime(2017, 1, 2);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetDateTime(Column, expected);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetDateTime_ColumnNameExists_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            var expected = new DateTime(2017, 1, 5);

            _dataReader[Column].Returns(expected);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetDateTime(Column, expected);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetNullableLong_ColumnNameDoesNotExist_ReturnDefaultValue()
        {
            // Arrange
            const string Column = "Column3";
            long? expected = null;

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetNullableLong(Column);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetNullableLong_ColumnNameExists_ReturnCorrectValue()
        {
            // Arrange
            const string Column = "Column1";
            long? expected = 123;

            _dataReader[Column].Returns(expected);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetNullableLong(Column, expected);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetNullableLong_ColumnNameExists_DataIsNull_ReturnNull()
        {
            // Arrange
            const string Column = "Column1";
            long? expected = null;

            _dataReader[Column].Returns(DBNull.Value);

            // Action
            var reader = new SafeDataReader(_dataReader, _columns);
            var result = reader.GetNullableLong(Column);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}