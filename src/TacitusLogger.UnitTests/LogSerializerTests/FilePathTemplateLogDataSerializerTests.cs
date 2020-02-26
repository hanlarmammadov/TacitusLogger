using Moq;
using NUnit.Framework;
using System; 
using System.IO; 
using TacitusLogger.Serializers;
using TacitusLogger.Components.TemplateResolving;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.LogSerializerTests
{
    [TestFixture]
    public class FilePathTemplateLogSerializerTests
    { 
        #region Ctors tests

        [Test]
        public void Ctor_Taking_Template_WhenCalled_SetsTemplateAndLogModelTemplateResolver()
        {
            // Arrange
            string template = "template";

            // Act
            FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer(template);

            // Assert  
            Assert.AreEqual(template, filePathTemplateLogSerializer.Template);
            Assert.IsInstanceOf<LogModelTemplateResolver>(filePathTemplateLogSerializer.LogModelTemplateResolver);
        }

        [Test]
        public void Ctor_Taking_Template_WhenCalled_Sets_LogModelTemplateResolver_With_The_Right_Number_And_Types_Of_PlaceholderResolvers()
        {
            // Arrange
            string template = "template";

            // Act
            FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer(template);

            // Assert  
            LogModelTemplateResolver logModelTemplateResolver = (LogModelTemplateResolver)filePathTemplateLogSerializer.LogModelTemplateResolver;
            Assert.AreEqual(6, logModelTemplateResolver.PlaceholderResolvers.Count);
            Assert.IsInstanceOf<SourcePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[0]);
            Assert.IsInstanceOf<ContextPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[1]);
            Assert.IsInstanceOf<LogIdPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[2]);
            Assert.IsInstanceOf<LogTypePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[3]);
            Assert.IsInstanceOf<DescriptionPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[4]);
            Assert.IsInstanceOf<LogDatePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[5]);
            Assert.AreEqual(FilePathTemplateLogSerializer.DefaultDateFormat, (logModelTemplateResolver.PlaceholderResolvers[5] as LogDatePlaceholderResolver).DefaultDateFormat);
        }

        [Test]
        public void Ctor_Taking_Template_WhenCalledWithNullTempalte_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer(null);
            });
        }

        [Test]
        public void Ctor_Default_WhenCalled_Sets_Template_And_LogModelTemplateResolver_To_Default()
        {
            // Act
            FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer();

            // Assert
            Assert.AreEqual(FilePathTemplateLogSerializer.DefaultTemplate, filePathTemplateLogSerializer.Template);
            Assert.IsInstanceOf<LogModelTemplateResolver>(filePathTemplateLogSerializer.LogModelTemplateResolver);
        }

        #endregion

        #region Tests for Serialize method

        [Test]
        public void Serialize_WhenCalled_Calls_LogModelTemplateResolvers_Resolve_Method()
        {
            // Arrange
            string template = "template";
            FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer(template);
            var logModelTemplateResolverMock = new Mock<ILogModelTemplateResolver>();
            string expectedReturnedString = "serialized";
            logModelTemplateResolverMock.Setup(x => x.Resolve(It.IsAny<LogModel>(), It.IsAny<string>())).Returns(expectedReturnedString);
            filePathTemplateLogSerializer.ResetLogModelTemplateResolver(logModelTemplateResolverMock.Object);
            LogModel logModel = new LogModel();

            // Act
            var returnedStr = filePathTemplateLogSerializer.Serialize(logModel);

            // Assert
            // Resolve method was called with provided logModel and template.
            logModelTemplateResolverMock.Verify(x => x.Resolve(logModel, template), Times.Once);
            // Serialize method return string that was returned from Resolve method.
            Assert.AreEqual(expectedReturnedString, returnedStr);
        }

        [TestCase("$LogId/$Context/$Source/$Description/$LogType/$LogDate.log", "LogId1/Context1/Source1/Description1/Error/10-Dec-2019.log")]
        [TestCase("$$LogId$$Context$$Source$$Description$$LogType$$LogDate", "$LogId1$Context1$Source1$Description1$Error$10-Dec-2019")]
        [TestCase("$LogId()$Context()$Source()$Description()$LogType()$LogDate()", "LogId1()Context1()Source1()Description1()Error()10-Dec-2019()")]
        [TestCase("$LogId(100)$Context(100)$Source(100)$Description(100)$LogType(100)$LogDate", "LogId1Context1Source1Description1Error10-Dec-2019")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate(dd)", "LogId1Context1Source1Description1Error10")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate(ddMMyyyHHmmss)", "LogId1Context1Source1Description1Error10122019225431")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate(MMM-dd-yyyy)", "LogId1Context1Source1Description1ErrorDec-10-2019")]
        [TestCase(@"c:\temp\logfile.log", @"c:\temp\logfile.log")]
        [TestCase(@"c:\some folder\logfile", @"c:\some folder\logfile")]
        [TestCase(@"/some folder/logfile", @"/some folder/logfile")]
        [TestCase(@"D:\some folder with $ sign\logfile.log", @"D:\some folder with $ sign\logfile.log")]
        [TestCase(@".\$LogTypes.log", @".\Errors.log")]
        public void Serialize_WhenCalled_With_Custom_Template_CreatesExpectedStringCorrectly(string template, string expectedResult)
        {
            // Arrange  
            FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = "LogId1",
                Context = "Context1",
                Source = "Source1",
                Description = "Description1",
                LogType = LogType.Error,
                LogDate = new DateTime(2019, 12, 10, 22, 54, 31)
            };

            // Act
            var serialized = filePathTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [Test]
        public void Serialize_When_Called_With_Default_Template_Creates_Expected_String_Correctly()
        {
            // Arrange  
            FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer();
            LogModel logModel = new LogModel()
            {
                LogId = "LogId1",
                Context = "Context1",
                Source = "Source1",
                Description = "Description1",
                LogType = LogType.Error,
                LogDate = new DateTime(2019, 12, 10, 22, 54, 31)
            };
            string expectedResult = $".{Path.DirectorySeparatorChar}Logs-10-Dec-2019.log";

            // Act
            var serialized = filePathTemplateLogSerializer.Serialize(logModel); 

            // Assert  
            Assert.AreEqual(expectedResult, serialized); 
        }

        #endregion

        #region Tests for ToString method 

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Serializer()
        {
            // Arrange
            string template = "Some template"; 
            FilePathTemplateLogSerializer filePathTemplateLogSerializer = new FilePathTemplateLogSerializer(template);

            // Act
            var result = filePathTemplateLogSerializer.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Serializers.FilePathTemplateLogSerializer"));
            Assert.IsTrue(result.Contains("Template: Some template"));
            Assert.IsTrue(result.Contains($"Default date format: {FilePathTemplateLogSerializer.DefaultDateFormat}")); 
        }

        #endregion

        #region Tests for DefaultTemplate and DefaultDateFormat properties

        [Test]
        public void DefaultTemplate_ShouldBeConsistent()
        {
            Assert.AreEqual(Templates.FilePath.Default, FilePathTemplateLogSerializer.DefaultTemplate);
        }

        [Test]
        public void DefaultDateFormat_ShouldBeConsistent()
        { 
            Assert.AreEqual(Templates.DateFormats.DefaultFileName, FilePathTemplateLogSerializer.DefaultDateFormat);
        }

        #endregion
    }
}
