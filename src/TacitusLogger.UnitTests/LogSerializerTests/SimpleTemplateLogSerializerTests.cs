using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TacitusLogger.Serializers;
using TacitusLogger.Components.TemplateResolving;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.LogSerializerTests
{
    [TestFixture]
    public class SimpleTemplateLogSerializerTests
    {
        #region Ctors tests

        [Test]
        public void Ctor_Taking_Template_WhenCalled_SetsTemplateAndLogModelTemplateResolver()
        {
            // Arrange
            string template = "template";

            // Act
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);

            // Assert  
            Assert.AreEqual(template, simpleTemplateLogSerializer.Template);
            Assert.IsInstanceOf<LogModelTemplateResolver>(simpleTemplateLogSerializer.LogModelTemplateResolver);
        } 
        [Test]
        public void Ctor_Taking_Template_WhenCalled_Sets_LogModelTemplateResolver_With_The_Right_Number_And_Types_Of_PlaceholderResolvers()
        {
            // Arrange
            string template = "template";

            // Act
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);

            // Assert  
            LogModelTemplateResolver logModelTemplateResolver = (LogModelTemplateResolver)simpleTemplateLogSerializer.LogModelTemplateResolver;
            Assert.AreEqual(7, logModelTemplateResolver.PlaceholderResolvers.Count);
            Assert.IsInstanceOf<SourcePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[0]);
            Assert.IsInstanceOf<ContextPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[1]);
            Assert.IsInstanceOf<TagsPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[2]);
            Assert.IsInstanceOf<LogIdPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[3]);
            Assert.IsInstanceOf<LogTypePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[4]);
            Assert.IsInstanceOf<DescriptionPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[5]);
            Assert.IsInstanceOf<LogDatePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[6]);
            Assert.AreEqual(SimpleTemplateLogSerializer.DefaultDateFormat, (logModelTemplateResolver.PlaceholderResolvers[6] as LogDatePlaceholderResolver).DefaultDateFormat);
        } 
        [Test]
        public void Ctor_Taking_Template_WhenCalledWithNullTempalte_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(null);
            });
        } 
        [Test]
        public void Ctor_Default_WhenCalled_Sets_Template_And_LogModelTemplateResolver_To_Default()
        {
            // Act
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer();

            // Assert
            Assert.AreEqual(SimpleTemplateLogSerializer.DefaultTemplate, simpleTemplateLogSerializer.Template);
            Assert.IsInstanceOf<LogModelTemplateResolver>(simpleTemplateLogSerializer.LogModelTemplateResolver);
        }

        #endregion

        #region Serialize method tests

        [Test]
        public void Serialize_WhenCalled_Calls_LogModelTemplateResolvers_Resolve_Method()
        {
            // Arrange
            string template = "template";
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);
            var logModelTemplateResolverMock = new Mock<ILogModelTemplateResolver>();
            string expectedReturnedString = "serialized";
            logModelTemplateResolverMock.Setup(x => x.Resolve(It.IsAny<LogModel>(), It.IsAny<string>())).Returns(expectedReturnedString);
            simpleTemplateLogSerializer.ResetLogModelTemplateResolver(logModelTemplateResolverMock.Object);
            LogModel logModel = new LogModel();

            // Act
            var returnedStr = simpleTemplateLogSerializer.Serialize(logModel);

            // Assert
            // Resolve method was called with provided logModel and template.
            logModelTemplateResolverMock.Verify(x => x.Resolve(logModel, template), Times.Once);
            // Serialize method return string that was returned from Resolve method.
            Assert.AreEqual(expectedReturnedString, returnedStr);
        } 
        [TestCase("LogId: $LogId Context: $Context Source: $Source Tags: $Tags Description: $Description LogType: $LogType LogDate: $LogDate", "LogId: LogId1 Context: Context1 Source: Source1 Tags: Tag1 Tag2 Description: Description1 LogType: Error LogDate: 10-Dec-19 22:54:31")]
        [TestCase("$LogId$Context$Source$Description$Tags$LogType$LogDate", "LogId1Context1Source1Description1Tag1 Tag2Error10-Dec-19 22:54:31")]
        [TestCase("$LogId-$Context-$Source-$Description-$LogType-$LogDate", "LogId1-Context1-Source1-Description1-Error-10-Dec-19 22:54:31")]
        [TestCase("$$LogId$$Context$$Source$$Description$$LogType$$LogDate", "$LogId1$Context1$Source1$Description1$Error$10-Dec-19 22:54:31")]
        [TestCase("$LogId()$Context()$Source()$Description()$LogType()$LogDate()", "LogId1()Context1()Source1()Description1()Error()10-Dec-19 22:54:31()")]
        [TestCase("$LogId$LogId$Context$Context$Source$Source$Description$Description$LogType$LogType$LogDate$LogDate", "LogId1LogId1Context1Context1Source1Source1Description1Description1ErrorError10-Dec-19 22:54:3110-Dec-19 22:54:31")]
        [TestCase("$LogId(100)$Context(100)$Source(100)$Description(100)$LogType(100)$LogDate", "LogId1Context1Source1Description1Error10-Dec-19 22:54:31")]
        [TestCase("$LogId(2)$Context(2)$Source(2)$Description(2)$LogType(2)$LogDate", "LoCoSoDeEr10-Dec-19 22:54:31")]
        [TestCase("$LogId$Context$Source$Description$Tags$LogType$LogDate(dd)", "LogId1Context1Source1Description1Tag1 Tag2Error10")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate(ddMMyyyHHmmss)", "LogId1Context1Source1Description1Error10122019225431")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate(MMM-dd-yyyy)", "LogId1Context1Source1Description1ErrorDec-10-2019")]
        [TestCase("$LogId", "LogId1")]
        [TestCase("$Context", "Context1")]
        [TestCase("$Tags", "Tag1 Tag2")]
        [TestCase("$Source", "Source1")]
        [TestCase("$Description", "Description1")]
        [TestCase("$LogType", "Error")]
        [TestCase("$LogDate", "10-Dec-19 22:54:31")]
        [TestCase("$LogDate(dd-MM-yyyy HH-mm)", "10-12-2019 22-54")]
        public void Serialize_WhenCalled_With_Custom_Template_CreatesExpectedStringCorrectly(string template, string expectedResult)
        {
            // Arrange  
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = "LogId1",
                Context = "Context1",
                Tags = new string[] { "Tag1", "Tag2" },
                Source = "Source1",
                Description = "Description1",
                LogType = LogType.Error,
                LogDate = new DateTime(2019, 12, 10, 22, 54, 31)
            };

            // Act
            var serialized = simpleTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        } 
        [TestCase("$LogId$Context$Source$Tags$Description", "")]
        [TestCase("$LogId-$Context-$Source-$Tags-$Description", "----")]
        public void Serialize_WhenCalled_With_Custom_Template_GivenThat_LogModel_String_Properties_AreNull_CreatesExpectedStringCorrectly(string template, string expectedResult)
        {
            // Arrange  
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = null,
                Context = null,
                Tags = null,
                Source = null,
                Description = null
            };

            // Act
            var serialized = simpleTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        } 
        [TestCase("$LogId$Context$Source$Tags$Description", "")]
        [TestCase("$LogId-$Context-$Source-$Tags-$Description", "----")]
        public void Serialize_WhenCalled_With_Custom_Template_GivenThat_LogModel_String_Properties_AreEmpty_CreatesExpectedStringCorrectly(string template, string expectedResult)
        {
            // Arrange  
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = "",
                Context = "",
                Tags = new string[0],
                Source = "",
                Description = ""
            };

            // Act
            var serialized = simpleTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        } 
        [TestCase("$LogItems", "$LogItems")]
        public void Serialize_WhenCalled_With_Custom_Template_Containig_LogItems_Does_Not_Process_LogItems(string template, string expectedResult)
        {
            // Arrange  
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogItems = new LogItem[]
                {
                     new LogItem("", ""),
                     new LogItem("", ""),
                }
            };

            // Act
            var serialized = simpleTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }  
        [Test]
        public void Serialize_When_Called_With_Default_Template_Creates_Expected_String_Correctly()
        {
            // Arrange  
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer();
            LogModel logModel = new LogModel()
            {
                LogId = "fcdcf2dd27ac40b9b23193c020510e0d",
                Context = "UserController",
                Tags = new string[] { "Security", "User" },
                Source = "App1",
                Description = "Something bad happened",
                LogType = LogType.Error,
                LogDate = new DateTime(2019, 12, 10, 22, 54, 31)
            };
            string expectedResult = "[10-Dec-19 22:54:31]-[Error]-[Something bad happened]-[From: UserController]-[Src: App1]-[Tags: Security User]-[Id: fcdcf2dd27ac40b9b23193c020510e0d]";

            // Act
            var serialized = simpleTemplateLogSerializer.Serialize(logModel);

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
            SimpleTemplateLogSerializer simpleTemplateLogSerializer = new SimpleTemplateLogSerializer(template);

            // Act
            var result = simpleTemplateLogSerializer.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Serializers.SimpleTemplateLogSerializer"));
            Assert.IsTrue(result.Contains("Template: Some template"));
            Assert.IsTrue(result.Contains($"Default date format: {SimpleTemplateLogSerializer.DefaultDateFormat}"));
        }

        #endregion

        #region for DefaultTemplate and DefaultDateFormat properties

        [Test]
        public void DefaultTemplate_ShouldBeConsistent()
        {
            Assert.AreEqual(Templates.Simple.Default, SimpleTemplateLogSerializer.DefaultTemplate);
        }

        [Test]
        public void DefaultDateFormat_ShouldBeConsistent()
        {
            var expectedDefaultDateFormat = "dd-MMM-yy HH:mm:ss";
            Assert.AreEqual(expectedDefaultDateFormat, SimpleTemplateLogSerializer.DefaultDateFormat);
        }

        #endregion
    }
}
