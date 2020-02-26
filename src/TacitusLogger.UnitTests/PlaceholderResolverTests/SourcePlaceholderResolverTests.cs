using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class SourcePlaceholderResolverTests
    {
        #region Tests for Resolve method

        [TestCase("Source1", "$Source", "Source1")]
        [TestCase("$MySource", "$Source", "$MySource")]
        [TestCase("Source1", "qwerty$Sourceqwertyu", "qwertySource1qwertyu")]
        [TestCase("", "qwerty$Sourceqwertyu", "qwertyqwertyu")]
        [TestCase(null, "qwerty$Sourceqwertyu", "qwertyqwertyu")]
        [TestCase("456", "123$Source789", "123456789")]
        [TestCase("Source1", "$Source$Source$Source", "Source1Source1Source1")]
        [TestCase("Source1", "$Source-$Source-$Source", "Source1-Source1-Source1")]
        [TestCase("Source1", "$Source(3)", "Sou")]
        [TestCase(null, "$Source(3)", "")]
        [TestCase("Source1", "$Source(44)", "Source1")]
        [TestCase("Source1", "$Source(0)", "")]
        [TestCase("Source1", "$Source(1)", "S")]
        [TestCase("Source1", "$Source(1t)", "Source1(1t)")]
        public void Resolve_WhenCalled_ReplacesAccordingSubstringsInTemplate(string source, string template, string expectedResult)
        {
            // Arrange
            SourcePlaceholderResolver sourcePlaceholderResolver = new SourcePlaceholderResolver();
            LogModel logModel = new LogModel() { Source = source };
            string initialString = template;

            // Act
            sourcePlaceholderResolver.Resolve(logModel, ref initialString);

            // Assert
            Assert.AreEqual(expectedResult, initialString);
        }

        [Test]
        public void Resolve_WhenCalledWithNullLogModel_ThrowsArgumentNullException()
        {
            // Arrange           
            SourcePlaceholderResolver sourcePlaceholderResolver = new SourcePlaceholderResolver();
            string initialString = "template";

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                sourcePlaceholderResolver.Resolve(null, ref initialString);
            });
        }

        [Test]
        public void Resolve_WhenCalledWithNullTemplate_ThrowsArgumentNullException()
        {
            // Arrange
            SourcePlaceholderResolver sourcePlaceholderResolver = new SourcePlaceholderResolver();
            LogModel logModel = new LogModel();
            string initialString = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                sourcePlaceholderResolver.Resolve(logModel, ref initialString);
            });
        }

        #endregion
    }
}
