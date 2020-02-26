using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class ContextPlaceholderResolverTests
    {
        #region Tests for Resolve method

        [TestCase("Context1", "$Context", "Context1")]
        [TestCase("$MyContext", "$Context", "$MyContext")]
        [TestCase("Context1", "qwerty$Contextqwertyu", "qwertyContext1qwertyu")]
        [TestCase("", "qwerty$Contextqwertyu", "qwertyqwertyu")]
        [TestCase(null, "qwerty$Contextqwertyu", "qwertyqwertyu")]
        [TestCase("456", "123$Context789", "123456789")]
        [TestCase("Context1", "$Context$Context$Context", "Context1Context1Context1")]
        [TestCase("Context1", "$Context-$Context-$Context", "Context1-Context1-Context1")]
        [TestCase("Context1", "$Context(3)", "Con")]
        [TestCase(null, "$Context(3)", "")]
        [TestCase("Context1", "$Context(44)", "Context1")]
        [TestCase("Context1", "$Context(100)$Source(100)", "Context1$Source(100)")]
        [TestCase("Context1", "$Context(0)", "")]
        [TestCase("Context1", "$Context(1)", "C")]
        [TestCase("Context1", "$Context(1t)", "Context1(1t)")]
        public void Resolve_WhenCalled_ReplacesAccordingSubstringsInTemplate(string context, string template, string expectedResult)
        {
            // Arrange
            ContextPlaceholderResolver contextPlaceholderResolver = new ContextPlaceholderResolver();
            LogModel logModel = new LogModel() { Context = context };
            string initialString = template;

            // Act
            contextPlaceholderResolver.Resolve(logModel, ref initialString);

            // Assert
            Assert.AreEqual(expectedResult, initialString);
        }

        [Test]
        public void Resolve_WhenCalledWithNullLogModel_ThrowsArgumentNullException()
        {
            // Arrange           
            ContextPlaceholderResolver contextPlaceholderResolver = new ContextPlaceholderResolver();
            string initialString = "template";

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                contextPlaceholderResolver.Resolve(null, ref initialString);
            });
        }

        [Test]
        public void Resolve_WhenCalledWithNullTemplate_ThrowsArgumentNullException()
        {
            // Arrange
            ContextPlaceholderResolver contextPlaceholderResolver = new ContextPlaceholderResolver();
            LogModel logModel = new LogModel();
            string initialString = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                contextPlaceholderResolver.Resolve(logModel, ref initialString);
            });
        }

        #endregion
    }
}
