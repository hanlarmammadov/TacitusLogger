using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Exceptions;

namespace TacitusLogger.UnitTests.ExceptionTests
{
    [TestFixture]
    public class LogCacheExceptionTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Message_When_Called_Sets_Exception_Message()
        {
            // Act
            LogCacheException logCacheException = new LogCacheException("message1");

            // Assert
            Assert.AreEqual("message1", logCacheException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_With_Null_Sets_Null_Exception_Message()
        {
            // Act
            LogCacheException logCacheException = new LogCacheException(null as string);

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.LogCacheException' was thrown.", logCacheException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_When_Called_InnerException_Is_Null()
        {
            // Act
            LogCacheException logCacheException = new LogCacheException("message1");

            // Assert
            Assert.AreEqual(null, logCacheException.InnerException);
        }

        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_Sets_Exception_Message_And_InnerException()
        {
            // Arrange
            Exception innerEx = new Exception();

            // Act
            LogCacheException logCacheException = new LogCacheException("message1", innerEx);

            // Assert
            Assert.AreEqual("message1", logCacheException.Message);
            Assert.AreEqual(innerEx, logCacheException.InnerException);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Message_Sets_Null_Exception_Message()
        {
            // Act
            LogCacheException logCacheException = new LogCacheException(null as string, new Exception());

            // Assert
            Assert.AreEqual("Exception of type 'TacitusLogger.Exceptions.LogCacheException' was thrown.", logCacheException.Message);
        }
        [Test]
        public void Ctor_Taking_Message_And_InnerException_When_Called_With_Null_Inner_Exception_Sets_Null_Inner_Exception()
        {
            // Act
            LogCacheException logCacheException = new LogCacheException("message1", null as Exception);

            // Assert
            Assert.AreEqual(null, logCacheException.InnerException);
        }

        #endregion
    }
}
