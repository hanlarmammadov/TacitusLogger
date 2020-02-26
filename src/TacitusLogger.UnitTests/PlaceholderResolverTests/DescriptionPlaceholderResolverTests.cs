using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class DescriptionPlaceholderResolverTests
    {
        #region Tests for Resolve method

        [TestCase("Description1", "$Description", "Description1")]
        [TestCase("$MyDescription", "$Description", "$MyDescription")]
        [TestCase("Description1", "qwerty$Descriptionqwertyu", "qwertyDescription1qwertyu")]
        [TestCase("", "qwerty$Descriptionqwertyu", "qwertyqwertyu")]
        [TestCase(null, "qwerty$Descriptionqwertyu", "qwertyqwertyu")]
        [TestCase("456", "123$Description789", "123456789")]
        [TestCase("Description1", "$Description$Description$Description", "Description1Description1Description1")]
        [TestCase("Description1", "$Description-$Description-$Description", "Description1-Description1-Description1")]
        [TestCase("Description1", "$Description(3)", "Des")]
        [TestCase("Description1", "$Description(100)", "Description1")]
        [TestCase(null, "$Description(3)", "")]
        [TestCase("Description1", "$Description(44)", "Description1")]
        [TestCase("Description1", "$Description(0)", "")]
        [TestCase("Description1", "$Description(1)", "D")]
        [TestCase("Description1", "$Description(1t)", "Description1(1t)")]
        public void Resolve_WhenCalled_ReplacesAccordingSubstringsInTemplate(string description, string template, string expectedResult)
        {
            // Arrange
            DescriptionPlaceholderResolver descriptionPlaceholderResolver = new DescriptionPlaceholderResolver();
            LogModel logModel = new LogModel() { Description = description };
            string initialString = template;

            // Act
            descriptionPlaceholderResolver.Resolve(logModel, ref initialString);

            // Assert
            Assert.AreEqual(expectedResult, initialString);
        }

        [Test]
        public void Resolve_WhenCalledWithNullLogModel_ThrowsArgumentNullException()
        {
            // Arrange           
            DescriptionPlaceholderResolver descriptionPlaceholderResolver = new DescriptionPlaceholderResolver();
            string initialString = "template";

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                descriptionPlaceholderResolver.Resolve(null, ref initialString);
            });
        }

        [Test]
        public void Resolve_WhenCalledWithNullTemplate_ThrowsArgumentNullException()
        {
            // Arrange
            DescriptionPlaceholderResolver descriptionPlaceholderResolver = new DescriptionPlaceholderResolver();
            LogModel logModel = new LogModel();
            string initialString = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                descriptionPlaceholderResolver.Resolve(logModel, ref initialString);
            });
        }

        #endregion
    }
}
