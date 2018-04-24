using CasinoLibrary.Model.UserException;
using Fanex.Athena.Service.Casino.Wrappers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Casino.UnitTests.Wrappers
{
    [TestFixture]
    public class ApiMethodOperationHandlerTest
    {
        private T SuccessCodeBlock<T>(object input, T expectedValue)
        {
            return expectedValue;
        }

        private T ThrowErrorExceptionCodeBlock<T>(object input)
        {
            throw new ErrorException("Unit Text Error");
        }
        private T ThrowOtherExceptionCodeBlock<T>(object input)
        {
            throw new ArgumentException("Invalid input");
        }

        [TestCase(true)]
        [TestCase(false)]
        [TestCase(null)]
        [TestCase(100)]
        public void Handle_WhenCall_ReturnExpectedValue(object expectedValue)
        {
            // Arrange
            var handler = new ApiMethodOperationHandler<object>();
            // Action
            var result = handler.Handle(Arg.Any<object>(), (obj) => SuccessCodeBlock<object>(obj, expectedValue));

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
        [Test]
        public void Handle_WhenCall_ThrowError()
        {
            // Arrange
            var handler = new ApiMethodOperationHandler<object>();
            // Action
            TestDelegate result = () => handler.Handle(Arg.Any<object>(), (obj) => ThrowOtherExceptionCodeBlock<object>(obj));

            // Assert
            Assert.Throws<ArgumentException>(result);
        }

        [Test]
        public void Handle_WhenCall_ReturnDefaultValueOfOutPutType()
        {
            // Arrange
            var handler = new ApiMethodOperationHandler<DataTable>();
            // Action
            var result = handler.Handle(Arg.Any<object>(), (obj) => ThrowErrorExceptionCodeBlock<DataTable>(obj));

            // Assert
            Assert.AreEqual(default(DataTable), result);
        }
    }
}
