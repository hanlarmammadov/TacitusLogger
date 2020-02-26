using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Exceptions;

namespace TacitusLogger.UnitTests.ExceptionTests
{
    [TestFixture]
    public class LogDestinationExceptionTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Message_When_Called_Sets_Exception_Message()
        {
            // Act
            LogDestinationException logDestinationException = new LogDestinationException("message1");

            // Assert
            Assert.AreEqual("message1", logDestinationException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_With_Null_Sets_Null_Exception_Message()
        {
            // Act
            LogDestinationException logDestinationException = new LogDestinationException(null as string);

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.LogDestinationException' was thrown.", logDestinationException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_InnerException_Is_Null()
        {
            // Act
            LogDestinationException logDestinationException = new LogDestinationException("message1");

            // Assert
            Assert.AreEqual(null, logDestinationException.InnerException);
        }

        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_Sets_Exception_Message_And_InnerException()
        {
            // Arrange
            Exception innerEx = new Exception();

            // Act
            LogDestinationException logDestinationException = new LogDestinationException("message1", innerEx);

            // Assert
            Assert.AreEqual("message1", logDestinationException.Message);
            Assert.AreEqual(innerEx, logDestinationException.InnerException);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Message_Sets_Null_Exception_Message()
        {
            // Act
            LogDestinationException logDestinationException = new LogDestinationException(null as string, new Exception());

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.LogDestinationException' was thrown.", logDestinationException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Inner_Exception_Sets_Null_Inner_Exception()
        {
            // Act
            LogDestinationException logDestinationException = new LogDestinationException("message1", null as Exception);

            // Assert
            Assert.AreEqual(null, logDestinationException.InnerException);
        }

        #endregion
    }
}
