using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class LogTypePlaceholderResolverTests
    {
        #region Tests for Resolve method

        [TestCase(LogType.Warning, "$LogType", "Warning")]
        [TestCase(LogType.Success, "11$LogType11", "11Success11")]
        [TestCase(LogType.Error,  "qwerty$LogTypeqwerty", "qwertyErrorqwerty")]
        [TestCase(LogType.Failure, "qwerty$LogTypeqwerty", "qwertyFailureqwerty")]
        [TestCase(LogType.Info, "qwertyInfoqwerty", "qwertyInfoqwerty")] 
        [TestCase(LogType.Warning, "$LogType$LogType$LogType", "WarningWarningWarning")]
        [TestCase(LogType.Event, "$LogType-$LogType-$LogType", "Event-Event-Event")]
        [TestCase(LogType.Error, "$LogType(3)", "Err")]
        [TestCase(LogType.Warning, "$LogType(3)", "War")]
        [TestCase(LogType.Info, "$LogType(44)", "Info")]
        [TestCase(LogType.Warning, "$LogType(0)", "")]
        [TestCase(LogType.Error, "$LogType(1)", "E")]
        [TestCase(LogType.Critical, "$LogType(1t)", "Critical(1t)")]
        public void Resolve_WhenCalled_ReplacesAccordingSubstringsInTemplate(LogType logType, string template, string expectedResult)
        {
            // Arrange
            LogTypePlaceholderResolver logTypePlaceholderResolver = new LogTypePlaceholderResolver();
            LogModel logModel = new LogModel() { LogType = logType };
            string initialString = template;

            // Act
            logTypePlaceholderResolver.Resolve(logModel, ref initialString);

            // Assert
            Assert.AreEqual(expectedResult, initialString);
        }

        [Test]
        public void Resolve_WhenCalledWithNullLogModel_ThrowsArgumentNullException()
        {
            // Arrange           
            LogTypePlaceholderResolver logTypePlaceholderResolver = new LogTypePlaceholderResolver();
            string initialString = "template";

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logTypePlaceholderResolver.Resolve(null, ref initialString);
            });
        }

        [Test]
        public void Resolve_WhenCalledWithNullTemplate_ThrowsArgumentNullException()
        {
            // Arrange
            LogTypePlaceholderResolver logTypePlaceholderResolver = new LogTypePlaceholderResolver();
            LogModel logModel = new LogModel();
            string initialString = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logTypePlaceholderResolver.Resolve(logModel, ref initialString);
            });
        }

        #endregion
    }
}
