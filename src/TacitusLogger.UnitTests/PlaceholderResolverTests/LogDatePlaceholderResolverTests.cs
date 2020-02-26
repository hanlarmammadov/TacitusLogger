using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class LogDatePlaceholderResolverTests
    {
        #region Test for Ctor

        [Test]
        public void Ctor_WhenCalled_SetsDefaultDateFormat()
        {
            // Arrange           
            string defaultDateFormat = "dd-MM-yyyy";

            // Act
            LogDatePlaceholderResolver logDatePlaceholderResolver = new LogDatePlaceholderResolver(defaultDateFormat);

            // Assert
            Assert.AreEqual(defaultDateFormat, logDatePlaceholderResolver.DefaultDateFormat);
        }

        [Test]
        public void Ctor_WhenCalledWithNullDefaultDateFormat_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act  
                LogDatePlaceholderResolver logDatePlaceholderResolver = new LogDatePlaceholderResolver(null);
            });
        }

        #endregion

        #region Tests for Resolve method

        [TestCase("13.02.2019 21:11:30:443", "$LogDate", "13-02-2019")]
        [TestCase("13.02.2019 21:11:30:443", "$$LogDate$", "$13-02-2019$")]
        [TestCase("13.02.2019 21:11:30:443", "qwerty$LogDateqwertyu", "qwerty13-02-2019qwertyu")]
        [TestCase("13.02.2019 21:11:30:443", "123($LogDate)789", "123(13-02-2019)789")]
        [TestCase("13.02.2019 21:11:30:443", "$LogDate$LogDate$LogDate", "13-02-201913-02-201913-02-2019")]
        [TestCase("13.02.2019 21:11:30:443", "$LogDate-$LogDate-$LogDate", "13-02-2019-13-02-2019-13-02-2019")]
        [TestCase("13.02.2019 21:11:30:443", "$LogDate(MMM-dd-yyyy)", "Feb-13-2019")]
        [TestCase("13.02.2019 21:11:30:443", "$LogDate(MMM.dd.yyyy)", "Feb.13.2019")]
        [TestCase("13.02.2019 21:11:30:443", "$LogDate(dd.MM.yyyy HH:mm:ss:fff)", "13.02.2019 21:11:30:443")]
        [TestCase("13.02.2019 21:11:30:443", "$LogDate()", "13-02-2019()")]
        public void Resolve_WhenCalled_ReplacesAccordingSubstringsInTemplate(string dateStr, string template, string expectedResult)
        {
            // Arrange
            LogDatePlaceholderResolver logDatePlaceholderResolver = new LogDatePlaceholderResolver("dd-MM-yyyy");
            LogModel logModel = new LogModel() { LogDate = DateTime.ParseExact(dateStr, "dd.MM.yyyy HH:mm:ss:fff", null) };
            string initialString = template;

            // Act
            logDatePlaceholderResolver.Resolve(logModel, ref initialString);

            // Assert
            Assert.AreEqual(expectedResult, initialString);
        }
         
        [TestCase("MM/dd/yyyy")]
        [TestCase("dddd, dd MMMM yyyy")]
        [TestCase("dddd, dd MMMM yyyy")]
        [TestCase("dddd, dd MMMM yyyy")]
        [TestCase("dddd, dd MMMM yyyy")]
        [TestCase("dddd, dd MMMM yyyy")]
        [TestCase("dddd, dd MMMM yyyy HH:mm:ss")]
        [TestCase("MM/dd/yyyy HH:mm")]
        [TestCase("MM/dd/yyyy hh:mm tt")]
        [TestCase("MM/dd/yyyy H:mm")]
        [TestCase("MM/dd/yyyy h:mm tt")]
        [TestCase("MM/dd/yyyy HH:mm:ss")]
        [TestCase("MMMM dd")]
        [TestCase("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK")]
        [TestCase("ddd, dd MMM yyy HH':'mm':'ss 'GMT'")]
        [TestCase("yyyy'-'MM'-'dd'T'HH':'mm':'ss")]
        [TestCase("HH:mm")]
        [TestCase("hh:mm tt")]
        [TestCase("H:mm")]
        [TestCase("h:mm tt")]
        [TestCase("HH:mm:ss")]
        [TestCase("yyyy MMMM")]
        public void Resolve_WhenCalled_GivenThat_Template_Contains_LogDate_In_Given_Format_Resolves_LogDate_Correctly(string dateFormat)
        {
            // Arrange
            LogDatePlaceholderResolver logDatePlaceholderResolver = new LogDatePlaceholderResolver("dd-MM-yyyy");
            LogModel logModel = new LogModel() { LogDate = DateTime.Now };
            string initialString = $"$LogDate({dateFormat})";

            // Act
            logDatePlaceholderResolver.Resolve(logModel, ref initialString);

            // Assert
            string expectedResult = logModel.LogDate.ToString(dateFormat);
            Assert.AreEqual(expectedResult, initialString);
        }

        [Test]
        public void Resolve_WhenCalledWithNullLogModel_ThrowsArgumentNullException()
        {
            // Arrange           
            LogDatePlaceholderResolver logDatePlaceholderResolver = new LogDatePlaceholderResolver("dd-MM-yyyy");
            string initialString = "template";

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logDatePlaceholderResolver.Resolve(null, ref initialString);
            });
        }

        [Test]
        public void Resolve_WhenCalledWithNullTemplate_ThrowsArgumentNullException()
        {
            // Arrange         
            LogDatePlaceholderResolver logDatePlaceholderResolver = new LogDatePlaceholderResolver("dd-MM-yyyy");

            LogModel logModel = new LogModel();
            string initialString = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logDatePlaceholderResolver.Resolve(logModel, ref initialString);
            });
        }

        #endregion
    }
}
