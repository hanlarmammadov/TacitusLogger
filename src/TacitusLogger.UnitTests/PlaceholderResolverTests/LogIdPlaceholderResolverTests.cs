using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class LogIdPlaceholderResolverTests
    {
        #region Tests for Resolve method

        [TestCase("1234567", "$LogId", "1234567")]
        [TestCase("$MyLogId", "$LogId", "$MyLogId")]
        [TestCase("LogId1", "qwerty$LogIdqwertyu", "qwertyLogId1qwertyu")]
        [TestCase("", "qwerty$LogIdqwertyu", "qwertyqwertyu")]
        [TestCase(null, "qwerty$LogIdqwertyu", "qwertyqwertyu")]
        [TestCase("456", "123$LogId789", "123456789")]
        [TestCase("LogId1", "$LogId$LogId$LogId", "LogId1LogId1LogId1")]
        [TestCase("LogId1", "$LogId-$LogId-$LogId", "LogId1-LogId1-LogId1")]
        [TestCase("LogId1", "$LogId(3)", "Log")]
        [TestCase(null, "$LogId(3)", "")]
        [TestCase("LogId1", "$LogId(44)", "LogId1")]
        [TestCase("LogId1", "$LogId(0)", "")]
        [TestCase("LogId1", "$LogId(1)", "L")]
        [TestCase("LogId1", "$LogId(1t)", "LogId1(1t)")]
        public void Resolve_WhenCalled_ReplacesAccordingSubstringsInTemplate(string logId, string template, string expectedResult)
        {
            // Arrange
            LogIdPlaceholderResolver logIdPlaceholderResolver = new LogIdPlaceholderResolver();
            LogModel logModel = new LogModel() { LogId = logId };
            string initialString = template;

            // Act
            logIdPlaceholderResolver.Resolve(logModel, ref initialString);

            // Assert
            Assert.AreEqual(expectedResult, initialString);
        }

        [Test]
        public void Resolve_WhenCalledWithNullLogModel_ThrowsArgumentNullException()
        {
            // Arrange           
            LogIdPlaceholderResolver logIdPlaceholderResolver = new LogIdPlaceholderResolver();
            string initialString = "template";

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logIdPlaceholderResolver.Resolve(null, ref initialString);
            });
        }

        [Test]
        public void Resolve_WhenCalledWithNullTemplate_ThrowsArgumentNullException()
        {
            // Arrange
            LogIdPlaceholderResolver logIdPlaceholderResolver = new LogIdPlaceholderResolver();
            LogModel logModel = new LogModel();
            string initialString = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logIdPlaceholderResolver.Resolve(logModel, ref initialString);
            });
        }

        #endregion
    }
}
