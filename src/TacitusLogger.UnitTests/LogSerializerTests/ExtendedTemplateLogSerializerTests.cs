using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TacitusLogger.Components;
using TacitusLogger.Serializers;
using TacitusLogger.Components.TemplateResolving;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;
using TacitusLogger.Components.Json;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.LogSerializerTests
{
    [TestFixture]
    public class ExtendedTemplateLogSerializerTests
    {
        private void AssertIfPlaceholderResolversAreSetRight(ExtendedTemplateLogSerializer extendedTemplateLogSerializer, JsonSerializerSettings expectedJsonSerializerSettings)
        {
            LogModelTemplateResolver logModelTemplateResolver = (LogModelTemplateResolver)extendedTemplateLogSerializer.LogModelTemplateResolver;
            Assert.AreEqual(9, logModelTemplateResolver.PlaceholderResolvers.Count);

            Assert.IsInstanceOf<NewLinePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[0]);
            Assert.IsInstanceOf<SourcePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[1]);
            Assert.IsInstanceOf<ContextPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[2]);
            Assert.IsInstanceOf<TagsPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[3]);
            Assert.IsInstanceOf<LogIdPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[4]);
            Assert.IsInstanceOf<LogTypePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[5]);
            Assert.IsInstanceOf<LogDatePlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[6]);
            Assert.IsInstanceOf<DescriptionPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[7]);
            Assert.IsInstanceOf<LogItemsPlaceholderResolver>(logModelTemplateResolver.PlaceholderResolvers[8]);

            Assert.AreEqual(ExtendedTemplateLogSerializer.DefaultDateFormat, (logModelTemplateResolver.PlaceholderResolvers[6] as LogDatePlaceholderResolver).DefaultDateFormat);
            Assert.AreEqual(expectedJsonSerializerSettings, (logModelTemplateResolver.PlaceholderResolvers[8] as LogItemsPlaceholderResolver).JsonSerializerSettings);
            Assert.AreEqual(extendedTemplateLogSerializer.JsonSerializerFacade, (logModelTemplateResolver.PlaceholderResolvers[8] as LogItemsPlaceholderResolver).JsonSerializerFacade);
        }

        #region Ctors tests

        [Test]
        public void Ctor_Taking_Template_And_JsonSettings_WhenCalled_SetsTemplate_And_JsonSettins_And_LogModelTemplateResolver()
        {
            // Arrange
            string template = "template";
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template, jsonSerializerSettings);

            // Assert  
            Assert.AreEqual(template, extendedTemplateLogSerializer.Template);
            Assert.AreEqual(jsonSerializerSettings, extendedTemplateLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<LogModelTemplateResolver>(extendedTemplateLogSerializer.LogModelTemplateResolver);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(extendedTemplateLogSerializer.JsonSerializerFacade);
        }

        [Test]
        public void Ctor_Taking_Template_And_JsonSettings_WhenCalled_Sets_LogModelTemplateResolver_With_The_Right_Number_And_Types_Of_PlaceholderResolvers()
        {
            // Arrange
            string template = "template";
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template, jsonSerializerSettings);

            // Assert  
            AssertIfPlaceholderResolversAreSetRight(extendedTemplateLogSerializer, jsonSerializerSettings);
        }

        [Test]
        public void Ctor_Taking_Template_And_JsonSettings_WhenCalled_With_Null_Template_Throws_ArgumentNullException()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(null, jsonSerializerSettings);
            });
        }

        [Test]
        public void Ctor_Taking_Template_And_JsonSettings_WhenCalled_With_Null_JsonSettings_Throws_ArgumentNullException()
        {
            // Arrange 
            string template = "template";

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template, null);
            });
        }

        [Test]
        public void Ctor_Taking_Template_WhenCalled_Sets_Template_JsonSettings_And_LogModelTemplateResolver_With_The_Right_Number_And_Types_Of_PlaceholderResolvers()
        {
            // Arrange
            string template = "template";

            // Act
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);

            // Assert  
            Assert.AreEqual(template, extendedTemplateLogSerializer.Template);
            Assert.AreEqual(ExtendedTemplateLogSerializer.DefaultJsonSerializerSettings, extendedTemplateLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(extendedTemplateLogSerializer.JsonSerializerFacade);
            AssertIfPlaceholderResolversAreSetRight(extendedTemplateLogSerializer, ExtendedTemplateLogSerializer.DefaultJsonSerializerSettings);
        }

        [Test]
        public void Ctor_Taking_Template_WhenCalled_With_Null_Template_Throws_ArgumentNullException()
        {
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(null as string);
            });
        }

        [Test]
        public void Ctor_Taking_JsonSettings_WhenCalled_Sets_Template_JsonSettings_And_LogModelTemplateResolver_With_The_Right_Number_And_Types_Of_PlaceholderResolvers()
        {
            // Arrange
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(jsonSerializerSettings);

            // Assert  
            Assert.AreEqual(ExtendedTemplateLogSerializer.DefaultTemplate, extendedTemplateLogSerializer.Template);
            Assert.AreEqual(jsonSerializerSettings, extendedTemplateLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(extendedTemplateLogSerializer.JsonSerializerFacade);
            AssertIfPlaceholderResolversAreSetRight(extendedTemplateLogSerializer, jsonSerializerSettings);
        }

        [Test]
        public void Ctor_Taking_JsonSettings_WhenCalled_With_Null_JsonSettings_Throws_ArgumentNullException()
        {
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(null as JsonSerializerSettings);
            });
        }

        [Test]
        public void Ctor_Default_WhenCalled_Sets_Template_JsonSettings_And_LogModelTemplateResolver_With_The_Right_Number_And_Types_Of_PlaceholderResolvers()
        {
            // Act
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer();

            // Assert
            Assert.AreEqual(ExtendedTemplateLogSerializer.DefaultTemplate, extendedTemplateLogSerializer.Template);
            Assert.AreEqual(ExtendedTemplateLogSerializer.DefaultJsonSerializerSettings, extendedTemplateLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(extendedTemplateLogSerializer.JsonSerializerFacade);
            AssertIfPlaceholderResolversAreSetRight(extendedTemplateLogSerializer, ExtendedTemplateLogSerializer.DefaultJsonSerializerSettings);
        }

        #endregion

        #region Serialize method tests

        [Test]
        public void Serialize_When_Called_Calls_LogModelTemplateResolvers_Resolve_Method()
        {
            // Arrange
            string template = "template";
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            var logModelTemplateResolverMock = new Mock<ILogModelTemplateResolver>();
            string expectedReturnedString = "serialized";
            logModelTemplateResolverMock.Setup(x => x.Resolve(It.IsAny<LogModel>(), It.IsAny<string>())).Returns(expectedReturnedString);
            extendedTemplateLogSerializer.ResetLogModelTemplateResolver(logModelTemplateResolverMock.Object);
            LogModel logModel = new LogModel();

            // Act
            var returnedStr = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert
            // Resolve method was called with provided logModel and template.
            logModelTemplateResolverMock.Verify(x => x.Resolve(logModel, template), Times.Once);
            // Serialize method return string that was returned from Resolve method.
            Assert.AreEqual(expectedReturnedString, returnedStr);
        }
        
        [TestCase("LogId: $LogId Context: $Context Source: $Source Tags: $Tags Description: $Description LogType: $LogType LogDate: $LogDate", "LogId: LogId1 Context: Context1 Source: Source1 Tags: Tag1 Tag2 Description: Description1 LogType: Error LogDate: 10-Dec-19 22:54:31")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate", "LogId1Context1Source1Description1Error10-Dec-19 22:54:31")]
        [TestCase("$LogId-$Context-$Source-$Description-$LogItems-$LogType-$LogDate", "LogId1-Context1-Source1-Description1-Name1: Value1\r\nName2: Value2-Error-10-Dec-19 22:54:31")]
        [TestCase("$$LogId$$Context$$Source$$Description$$LogType$$LogDate-$LogItems", "$LogId1$Context1$Source1$Description1$Error$10-Dec-19 22:54:31-Name1: Value1\r\nName2: Value2")]
        [TestCase("$LogId()$Context()$Source()$Description()$LogType()$LogDate()", "LogId1()Context1()Source1()Description1()Error()10-Dec-19 22:54:31()")]
        [TestCase("$LogId$LogId$Context$Context$Source$Source$Description$Description$LogType$LogType$LogDate$LogDate", "LogId1LogId1Context1Context1Source1Source1Description1Description1ErrorError10-Dec-19 22:54:3110-Dec-19 22:54:31")]
        [TestCase("$LogId(100)$Context(100)$Tags(3)$Source(100)$Description(100)$LogType(100)$LogDate", "LogId1Context1Tag1 Tag2(3)Source1Description1Error10-Dec-19 22:54:31")]
        [TestCase("$LogId(2)$Context(2)$Source(2)$Description(2)$LogType(2)$LogDate", "LoCoSoDeEr10-Dec-19 22:54:31")]
        [TestCase("$LogId$Context$Source$Description$Tags$LogType$LogDate(dd)", "LogId1Context1Source1Description1Tag1 Tag2Error10")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate(ddMMyyyHHmmss)", "LogId1Context1Source1Description1Error10122019225431")]
        [TestCase("$LogId$Context$Source$Description$LogType$LogDate(MMM-dd-yyyy)$LogItems", "LogId1Context1Source1Description1ErrorDec-10-2019Name1: Value1\r\nName2: Value2")]
        [TestCase("$LogId", "LogId1")]
        [TestCase("$Context", "Context1")]
        [TestCase("$Source", "Source1")]
        [TestCase("$Tags", "Tag1 Tag2")]
        [TestCase("$Description", "Description1")]
        [TestCase("$LogType", "Error")]
        [TestCase("$LogDate", "10-Dec-19 22:54:31")]
        [TestCase("$LogDate(dd-MM-yyyy HH-mm)", "10-12-2019 22-54")]
        public void Serialize_WhenCalled_With_Custom_Template_CreatesExpectedStringCorrectly(string template, string expectedResult)
        {
            // Arrange  
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = "LogId1",
                Context = "Context1",
                Tags = new string[] { "Tag1", "Tag2" },
                Source = "Source1",
                Description = "Description1",
                LogType = LogType.Error,
                LogDate = new DateTime(2019, 12, 10, 22, 54, 31),
                LogItems = new LogItem[] {
                    new LogItem("Name1", "Value1"),
                    new LogItem("Name2", "Value2"),
                }
            };

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [TestCase("$LogId$Context$Source$Tags$Description$LogItems", "")]
        [TestCase("$LogId-$Context-$Source-$Tags-$Description-$LogItems", "-----")]
        public void Serialize_When_Called_With_Custom_Template_Given_That_LogModel_String_Properties_Are_Null_Creates_Expected_String_Correctly(string template, string expectedResult)
        {
            // Arrange  
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = null,
                Context = null,
                Tags = null,
                Source = null,
                Description = null,
                LogItems = null
            };

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [TestCase("$LogId$Context$Source$LogItems$Description", "")]
        [TestCase("$LogId-$Context-$Source-$LogItems-$Description", "----")]
        public void Serialize_When_Called_With_Custom_Template_Given_That_LogModel_String_Properties_AreEmpty_Creates_Expected_String_Correctly(string template, string expectedResult)
        {
            // Arrange  
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = "",
                Context = "",
                Source = "",
                Description = "",
                LogItems = new LogItem[0]
            };

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [Test]
        public void Serialize_WhenCalled_With_Custom_Template_Containig_LoggingObject_Serializes_LoggingObject_Correctly1()
        {
            // Arrange  
            string template = "$LogId$LogItems";
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = "LogId1",
                LogItems = new LogItem[]
                {
                     new LogItem("Item1", new {Name = "Value" })
                }
            };
            string expectedResult = $"LogId1Item1: {JsonConvert.SerializeObject(logModel.LogItems[0].Value)}";

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [Test]
        public void Serialize_WhenCalled_With_Custom_Template_Containig_LoggingObject_Serializes_LoggingObject_Correctly2()
        {
            // Arrange  
            string template = "$LogId$LogItems";
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                LogId = "LogId1",
                LogItems = new LogItem[]
                {
                     new LogItem("Item1", new ClassWithCircularReference())
                }
            };
            string expectedResult = $"LogId1Item1: {JsonConvert.SerializeObject(logModel.LogItems[0].Value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}";

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [Test]
        public void Serialize_WhenCalled_With_Custom_Template_Containig_LoggingObject_Serializes_LoggingObject_Correctly3()
        {
            // Arrange  
            string template = "$LogItems$Context(3)";
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                Context = "Context1",
                LogItems = new LogItem[]
                {
                     new LogItem("Item1", new
                     {
                        Name1 = "Value1",
                        Name2 = "Value2",
                        Inner1 = new
                        {
                            Name1 = "Value1",
                            Name2 = "Value2",
                            Inner2 = new
                            {
                                Name1 = "Value1",
                                Name2 = "Value2",
                                Inner3 = new
                                {
                                    Name1 = "Value1",
                                    Name2 = "Value2",
                                }
                            }
                        }
                     })
                }
            };
            string expectedResult = $"Item1: {JsonConvert.SerializeObject(logModel.LogItems[0].Value)}Con";

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [Test]
        public void Serialize_WhenCalled_With_Custom_Template_Containig_NewLine_Serializes_NewLine_Correctly1()
        {
            // Arrange  
            string template = "$NewLine$LogId$NewLine$LogItems$NewLine$Context$NewLine";
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel()
            {
                Context = "Context1",
                LogId = "LogId1",
                LogItems = new LogItem[]
                {
                    new LogItem("Item1", new { Name1 = "Value1", Name2 = "Value2"})
                }
            };
            string expectedResult = $"{Environment.NewLine}LogId1{Environment.NewLine}Item1: {JsonConvert.SerializeObject(logModel.LogItems[0].Value)}{Environment.NewLine}Context1{Environment.NewLine}";

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [Test]
        public void Serialize_WhenCalled_With_Custom_Template_Containig_NewLine_Serializes_NewLine_Correctly2()
        {
            // Arrange  
            string template = "$NewLine$NewLine(5)$NewLine(100)$NewLine()$NewLine(a)";
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template);
            LogModel logModel = new LogModel();
            string expectedResult = $"{Environment.NewLine}{Environment.NewLine}(5){Environment.NewLine}(100){Environment.NewLine}(){Environment.NewLine}(a)";

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        [Test]
        public void Serialize_When_Called_With_Default_Template_Creates_Expected_String_Correctly()
        {
            // Arrange  
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer();
            LogModel logModel = new LogModel()
            {
                LogId = "LogId1",
                Context = "Context1",
                Tags = new string[] { "Tag1", "Tag2", "Tag3" },
                Source = "Source1",
                Description = "Description1",
                LogType = LogType.Error,
                LogDate = new DateTime(2019, 12, 10, 22, 54, 31),
                LogItems = new LogItem[]
                {
                    new LogItem("First item", new {Name1 = "Value1",Name2 = "Value2" } ),
                    new LogItem("Second item", new {Name3 = "Value3", Name4 = "Value4" } ),
                }
            };
            string expectedResult = $"Log Id:        LogId1{Environment.NewLine}Log type:      Error{Environment.NewLine}Description:   Description1{Environment.NewLine}Source:        Source1{Environment.NewLine}Context:       Context1{Environment.NewLine}Tags:          Tag1 Tag2 Tag3{Environment.NewLine}Log date:      10-Dec-19 22:54:31{Environment.NewLine}First item: {JsonConvert.SerializeObject(logModel.LogItems[0].Value)}{Environment.NewLine}Second item: {JsonConvert.SerializeObject(logModel.LogItems[1].Value)}{Environment.NewLine}-------------------------------------{Environment.NewLine}";

            // Act
            var serialized = extendedTemplateLogSerializer.Serialize(logModel);

            // Assert  
            Assert.AreEqual(expectedResult, serialized);
        }

        #endregion

        #region Tests for ResetJsonSerializerFacade method

        [Test]
        public void ResetJsonSerializerFacade_When_Called_Sets_Specified_Object_As_New_Json_Serializer_Facade()
        {
            // Arrange
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer();
            var jsonSerializerFacade = new Mock<IJsonSerializerFacade>().Object;

            // Act
            extendedTemplateLogSerializer.ResetJsonSerializerFacade(jsonSerializerFacade);

            // Assert
            Assert.AreEqual(jsonSerializerFacade, extendedTemplateLogSerializer.JsonSerializerFacade);
        }

        [Test]
        public void ResetJsonSerializerFacade_When_Called_With_Null_Json_Serializer_Facade_Throws_ArgumentNullException()
        {
            // Arrange
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                extendedTemplateLogSerializer.ResetJsonSerializerFacade(null as IJsonSerializerFacade);
            });
        }

        #endregion

        #region Tests for ToString method 

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Serializer()
        {
            // Arrange
            string template = "Some template";
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            ExtendedTemplateLogSerializer extendedTemplateLogSerializer = new ExtendedTemplateLogSerializer(template, jsonSerializerSettings);

            // Act
            var result = extendedTemplateLogSerializer.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Serializers.ExtendedTemplateLogSerializer"));
            Assert.IsTrue(result.Contains("Template: Some template"));
            Assert.IsTrue(result.Contains($"Default date format: {ExtendedTemplateLogSerializer.DefaultDateFormat}"));
            Assert.IsTrue(result.Contains("Json serializer settings: Newtonsoft.Json.JsonSerializerSettings"));
        }

        #endregion

        #region for DefaultTemplate and DefaultDateFormat properties

        [Test]
        public void DefaultTemplate_ShouldBeConsistent()
        { 
            Assert.AreEqual(Templates.Extended.Default, ExtendedTemplateLogSerializer.DefaultTemplate);
        }

        [Test]
        public void DefaultDateFormat_ShouldBeConsistent()
        {
            Assert.AreEqual(Templates.DateFormats.Default, ExtendedTemplateLogSerializer.DefaultDateFormat);
        }

        #endregion


    }
}
