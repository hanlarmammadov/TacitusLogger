using NUnit.Framework;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class TagsPlaceholderResolverTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_RegExp()
        {
            // Act
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();

            // Assert
            Assert.AreEqual(@"(\$Tags).*?", tagsPlaceholderResolver.MainRegex.ToString());
        }

        #endregion

        #region Tests for Resolve method

        [Test]
        public void Resolve_When_Called_Replaces_Placeholder_In_Template_With_Tag()
        {
            // Arrange
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1" } };
            string template = "$Tags";

            // Act 
            tagsPlaceholderResolver.Resolve(logModel, ref template);

            // Assert
            string expectedString = "tag1";
            Assert.AreEqual(expectedString, template);
        }
        [Test]
        public void Resolve_When_Called_Given_That_Tags_Array_Is_Null_Replaces_Placeholder_In_Template_With_Empty_String()
        {
            // Arrange
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();
            LogModel logModel = new LogModel() { Tags = null };
            string template = "$Tags";

            // Act 
            tagsPlaceholderResolver.Resolve(logModel, ref template);

            // Assert 
            Assert.AreEqual("", template);
        }
        [Test]
        public void Resolve_When_Called_Given_That_Tags_Array_Is_Empty_Replaces_Placeholder_In_Template_With_Empty_String()
        {
            // Arrange
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();
            LogModel logModel = new LogModel() { Tags = new string[0] };
            string template = "$Tags";

            // Act 
            tagsPlaceholderResolver.Resolve(logModel, ref template);

            // Assert 
            Assert.AreEqual("", template);
        }
        [Test]
        public void Resolve_When_Called_Given_That_Tags_Contain_Several_Elements_Replaces_Placeholder_In_Template_Correctly()
        {
            // Arrange
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2", "tag3" } };
            string template = "$Tags";

            // Act 
            tagsPlaceholderResolver.Resolve(logModel, ref template);

            // Assert
            string expectedString = "tag1 tag2 tag3";
            Assert.AreEqual(expectedString, template);
        }
        [Test]
        public void Resolve_When_Called_Given_That_Tags_Contain_Several_Elements_Containing_Nulls_Replaces_Placeholder_In_Template_Correctly()
        {
            // Arrange
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", null, "tag3" } };
            string template = "$Tags";

            // Act 
            tagsPlaceholderResolver.Resolve(logModel, ref template);

            // Assert
            string expectedString = "tag1  tag3";
            Assert.AreEqual(expectedString, template);
        }
        [Test]
        public void Resolve_When_Called_Given_That_Template_Contains_Several_Tags_Placeholders_Replaces_Placeholder_In_Template_Correctly()
        {
            // Arrange
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2", "tag3" } };
            string template = "$Tags$Tags$Tags";

            // Act 
            tagsPlaceholderResolver.Resolve(logModel, ref template);

            // Assert
            string expectedString = "tag1 tag2 tag3tag1 tag2 tag3tag1 tag2 tag3";
            Assert.AreEqual(expectedString, template);
        }
        [TestCase("qwerty$Tagsqwerty", "qwertytag1 tag2 tag3qwerty")]
        [TestCase(" $Tags ", " tag1 tag2 tag3 ")]
        [TestCase("$$Tags$", "$tag1 tag2 tag3$")]
        [TestCase("Tags", "Tags")]
        [TestCase("$tags", "$tags")]
        [TestCase("$TAGS", "$TAGS")]
        [TestCase("$Tags$Tags$Tags", "tag1 tag2 tag3tag1 tag2 tag3tag1 tag2 tag3")]
        [TestCase("$Tags-$Tags-$Tags", "tag1 tag2 tag3-tag1 tag2 tag3-tag1 tag2 tag3")]
        [TestCase("", "")]
        public void Resolve_When_Called_Different_Templates_Replaces_Placeholders_In_Templates_Correctly(string template, string expectedResult)
        {
            // Arrange
            TagsPlaceholderResolver tagsPlaceholderResolver = new TagsPlaceholderResolver();
            LogModel logModel = new LogModel() { Tags = new string[] { "tag1", "tag2", "tag3" } };

            // Act 
            tagsPlaceholderResolver.Resolve(logModel, ref template);

            // Assert 
            Assert.AreEqual(expectedResult, template);
        }

        #endregion

    }
}
