using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Exceptions;

namespace TacitusLogger.UnitTests.ExceptionTests
{
    [TestFixture]
    public class DestinationFeedingExceptionTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Message_When_Called_Sets_Exception_Message()
        {
            // Act
            DestinationFeedingException destinationFeedingException = new DestinationFeedingException("message1");

            // Assert
            Assert.AreEqual("message1", destinationFeedingException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_With_Null_Sets_Null_Exception_Message()
        {
            // Act
            DestinationFeedingException destinationFeedingException = new DestinationFeedingException(null as string);

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.DestinationFeedingException' was thrown.", destinationFeedingException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_InnerException_Is_Null()
        {
            // Act
            DestinationFeedingException destinationFeedingException = new DestinationFeedingException("message1");

            // Assert
            Assert.AreEqual(null, destinationFeedingException.InnerException);
        }

        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_Sets_Exception_Message_And_InnerException()
        {
            // Arrange
            Exception innerEx = new Exception();

            // Act
            DestinationFeedingException destinationFeedingException = new DestinationFeedingException("message1", innerEx);

            // Assert
            Assert.AreEqual("message1", destinationFeedingException.Message);
            Assert.AreEqual(innerEx, destinationFeedingException.InnerException);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Message_Sets_Null_Exception_Message()
        {
            // Act
            DestinationFeedingException destinationFeedingException = new DestinationFeedingException(null as string, new Exception());

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.DestinationFeedingException' was thrown.", destinationFeedingException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Inner_Exception_Sets_Null_Inner_Exception()
        {
            // Act
            DestinationFeedingException destinationFeedingException = new DestinationFeedingException("message1", null as Exception);

            // Assert
            Assert.AreEqual(null, destinationFeedingException.InnerException);
        }

        #endregion
    }
}
