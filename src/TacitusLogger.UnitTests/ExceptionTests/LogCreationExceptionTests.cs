using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Exceptions;

namespace TacitusLogger.UnitTests.ExceptionTests
{
    [TestFixture]
    public class LogCreationExceptionTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Message_When_Called_Sets_Exception_Message()
        {
            // Act
            LogCreationException logCreationException = new LogCreationException("message1");

            // Assert
            Assert.AreEqual("message1", logCreationException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_With_Null_Sets_Null_Exception_Message()
        {
            // Act
            LogCreationException logCreationException = new LogCreationException(null as string);

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.LogCreationException' was thrown.", logCreationException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_InnerException_Is_Null()
        {
            // Act
            LogCreationException logCreationException = new LogCreationException("message1");

            // Assert
            Assert.AreEqual(null, logCreationException.InnerException);
        }

        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_Sets_Exception_Message_And_InnerException()
        {
            // Arrange
            Exception innerEx = new Exception();

            // Act
            LogCreationException logCreationException = new LogCreationException("message1", innerEx);

            // Assert
            Assert.AreEqual("message1", logCreationException.Message);
            Assert.AreEqual(innerEx, logCreationException.InnerException);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Message_Sets_Null_Exception_Message()
        {
            // Act
            LogCreationException logCreationException = new LogCreationException(null as string, new Exception());

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.LogCreationException' was thrown.", logCreationException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Inner_Exception_Sets_Null_Inner_Exception()
        {
            // Act
            LogCreationException logCreationException = new LogCreationException("message1", null as Exception);

            // Assert
            Assert.AreEqual(null, logCreationException.InnerException);
        }

        #endregion
    }
}
