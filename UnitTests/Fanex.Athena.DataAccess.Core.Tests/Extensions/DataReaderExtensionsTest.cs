namespace Fanex.Athena.DataAccess.Core.Tests.Extensions
{
    using System;
    using System.Data;
    using Core.Extensions;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DataReaderExtensionsTest
    {
        [Test]
        public void HasColumn_ReaderContainColumn_ReturnTrue()
        {
            // Arrange
            var reader = Substitute.For<IDataRecord>();
            reader.FieldCount.Returns(1);
            var columnName = "ColumnTest";
            reader.GetName(0).Returns(columnName);

            // Action
            var result = reader.HasColumn(columnName);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasColumn_ReaderNotContainColumn_ReturnFalse()
        {
            // Arrange
            var reader = Substitute.For<IDataRecord>();
            reader.FieldCount.Returns(1);
            var columnName = "ColumnTest";
            reader.GetName(0).Returns("OtherColumnTest");

            // Action
            var result = reader.HasColumn(columnName);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void ToNullableInt_ReaderContainValueOfFieldName_ReturnCorrectValue()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var fieldName = "FieldNameTest";
            var expected = 1;
            reader[fieldName].Returns(expected);

            // Action
            var result = reader.ToNullableInt(fieldName);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToNullableInt_ReaderNotContainValueOfFieldName_ReturnDefaultValue()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var fieldName = "FieldNameTest";
            var expected = 0;
            reader[fieldName].Returns(DBNull.Value);

            // Action
            var result = reader.ToNullableInt(fieldName, expected);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToNullableDecimal_ReaderContainValueOfFieldName_ReturnCorrectValue()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var fieldName = "FieldNameTest";
            var expected = 1.5m;
            reader[fieldName].Returns(expected);

            // Action
            var result = reader.ToNullableDecimal(fieldName);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToNullableDecimal_ReaderNotContainValueOfFieldName_ReturnDefaultValue()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var fieldName = "FieldNameTest";
            var expected = 0m;
            reader[fieldName].Returns(DBNull.Value);

            // Action
            var result = reader.ToNullableDecimal(fieldName, expected);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}