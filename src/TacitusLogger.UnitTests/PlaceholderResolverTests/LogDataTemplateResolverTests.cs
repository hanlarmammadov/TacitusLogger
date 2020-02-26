using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Components.TemplateResolving;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class LogModelTemplateResolverTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_WhenCalled_Sets_Placeholder_Resolvers_Correctly()
        {
            // Arrange
            List<IPlaceholderResolver> placeholderResolvers = new List<IPlaceholderResolver>()
            {
                new Mock<IPlaceholderResolver>().Object,
                new Mock<IPlaceholderResolver>().Object
            };

            // Act
            LogModelTemplateResolver logModelTemplateResolver = new LogModelTemplateResolver(placeholderResolvers);

            // Assert
            Assert.AreEqual(placeholderResolvers, logModelTemplateResolver.PlaceholderResolvers);
            Assert.AreEqual(2, logModelTemplateResolver.PlaceholderResolvers.Count);
        }

        [Test]
        public void Ctor_WhenCalled_With_Empty_Placeholder_Resolvers_List_Does_Not_Throw()
        {
            // Arrange
            List<IPlaceholderResolver> placeholderResolvers = new List<IPlaceholderResolver>();

            // Act
            LogModelTemplateResolver logModelTemplateResolver = new LogModelTemplateResolver(placeholderResolvers);

            // Assert
            Assert.AreEqual(placeholderResolvers, logModelTemplateResolver.PlaceholderResolvers);
            Assert.AreEqual(0, logModelTemplateResolver.PlaceholderResolvers.Count);

        }

        [Test]
        public void Ctor_WhenCalled_With_Null_Placeholder_Resolvers_List_Throw_ArgumentNullException()
        {
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                LogModelTemplateResolver logModelTemplateResolver = new LogModelTemplateResolver(null);
            });
        }

        #endregion

        #region Tests for Resolve method

        [Test]
        public void Resolve_WhenCalled_Calls_Resolve_Method_Of_All_Provided_Placeholder_Resolvers_And_Return_Result_String()
        {
            // Arrange
            LogModel logModel = new LogModel();
            var placeholderResolver1Mock = new Mock<IPlaceholderResolver>();
            var placeholderResolver2Mock = new Mock<IPlaceholderResolver>();
            var placeholderResolver3Mock = new Mock<IPlaceholderResolver>();
            string template = "initial";
            List<IPlaceholderResolver> placeholderResolvers = new List<IPlaceholderResolver>()
            {
                placeholderResolver1Mock.Object,
                placeholderResolver2Mock.Object,
                placeholderResolver3Mock.Object,
            };
            LogModelTemplateResolver logModelTemplateResolver = new LogModelTemplateResolver(placeholderResolvers);

            // Act
            var resultedString = logModelTemplateResolver.Resolve(logModel, template);

            // Assert
            placeholderResolver1Mock.Verify(x => x.Resolve(logModel, ref template), Times.Once);
            placeholderResolver2Mock.Verify(x => x.Resolve(logModel, ref template), Times.Once);
            placeholderResolver3Mock.Verify(x => x.Resolve(logModel, ref template), Times.Once);
            Assert.AreEqual(template, resultedString);
        }
        
        #endregion
    }
}
