using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using TacitusLogger.Components.Json;
using TacitusLogger.Serializers;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.LogSerializerTests
{
    public class CustomType
    {
        public string Name { get; set; }
        public LogItem[] LogItems { get; set; }
    }

    [TestFixture]
    public class JsonLogSerializerTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Converter_And_Json_Settings_When_Called_Sets_Converter_And_Json_Settings_And_Json_Serializer_Facade()
        {
            // Arrange
            LogModelFunc<Object> converter = d => new CustomType();
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(converter, jsonSerializerSettings);

            // Assert
            Assert.AreEqual(converter, jsonLogSerializer.Converter);
            Assert.AreEqual(jsonSerializerSettings, jsonLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(jsonLogSerializer.JsonSerializerFacade);
        }

        [Test]
        public void Ctor_Taking_Converter_And_Json_Settings_When_Called_With_Null_Converter_Throws_ArgumentNullException()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(null, jsonSerializerSettings);
            });
        }

        [Test]
        public void Ctor_Taking_Converter_And_Json_Settings_When_Called_With_Null_Json_Settings_Throws_ArgumentNullException()
        {
            // Arrange
            LogModelFunc<Object> converter = d => new CustomType();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(converter, null);
            });
        }

        [Test]
        public void Ctor_Taking_Converter_When_Called_Sets_Converter_And_Default_Json_Settings()
        {
            // Arrange
            LogModelFunc<Object> converter = d => new CustomType();

            // Act
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(converter);

            // Assert
            Assert.AreEqual(converter, jsonLogSerializer.Converter);
            Assert.AreEqual(JsonLogSerializer.DefaultJsonSerializerSettings, jsonLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(jsonLogSerializer.JsonSerializerFacade);
        }

        [Test]
        public void Ctor_Taking_Converter_When_Called_With_Null_Converter_Throws_ArgumentNullException()
        {
            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(null as LogModelFunc<Object>);
            });
        }

        [Test]
        public void Ctor_Taking_Json_Settings_When_Called_Sets_Json_Settings_And_Default_Converter()
        {
            // Arrange
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(jsonSerializerSettings);

            // Assert
            Assert.AreEqual(JsonLogSerializer.DefaultConverter, jsonLogSerializer.Converter);
            Assert.AreEqual(jsonSerializerSettings, jsonLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(jsonLogSerializer.JsonSerializerFacade);
        }

        [Test]
        public void Ctor_Taking_Json_Settings_When_Called_With_Null_Json_Settings_Throws_ArgumentNullException()
        {
            // Arrange
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(null as JsonSerializerSettings);
            });
        }

        [Test]
        public void Ctor_Default_When_Called_Sets_Converter_And_Json_Settings_To_Defaults()
        {
            // Act
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer();

            // Assert
            Assert.AreEqual(JsonLogSerializer.DefaultConverter, jsonLogSerializer.Converter);
            Assert.AreEqual(JsonLogSerializer.DefaultJsonSerializerSettings, jsonLogSerializer.JsonSerializerSettings);
            Assert.IsInstanceOf<NewtonsoftJsonSerializerFacade>(jsonLogSerializer.JsonSerializerFacade);
        }

        #endregion

        #region Tests for Serialize method

        [Test]
        public void Serialize_When_Called_Send_Object_Returned_From_Converter_And_Json_Settings_To_Json_Serializer_Facade_And_Returns_Its_Result()
        {
            // Arrange
            var objReturnedFromConvertor = new CustomType();
            LogModelFunc<Object> converter = d => objReturnedFromConvertor;
            //
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            //
            var jsonSerializerFacadeMock = new Mock<IJsonSerializerFacade>();
            jsonSerializerFacadeMock.Setup(x => x.Serialize(It.IsAny<Object>(), It.IsAny<JsonSerializerSettings>())).Returns("json string");
            //
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(converter, jsonSerializerSettings);
            jsonLogSerializer.ResetJsonSerializerFacade(jsonSerializerFacadeMock.Object);
            //

            // Act
            string resultJson = jsonLogSerializer.Serialize(new LogModel());

            // Assert
            jsonSerializerFacadeMock.Verify(x => x.Serialize(objReturnedFromConvertor, jsonSerializerSettings), Times.Once);
        }

        [Test]
        public void Serialize_When_Called_Given_That_Default_Converter_Is_Used_Sends_LogModel_Returned_From_Converter_And_Json_Settings_To_Json_Serializer_Facade_And_Returns_Its_Result()
        {
            // Arrange  
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            //
            var jsonSerializerFacadeMock = new Mock<IJsonSerializerFacade>();
            jsonSerializerFacadeMock.Setup(x => x.Serialize(It.IsAny<Object>(), It.IsAny<JsonSerializerSettings>())).Returns("json string");
            //
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(jsonSerializerSettings);
            jsonLogSerializer.ResetJsonSerializerFacade(jsonSerializerFacadeMock.Object);
            //
            var logModel = Samples.LogModels.Standard();

            // Act
            string resultJson = jsonLogSerializer.Serialize(logModel);

            // Assert
            jsonSerializerFacadeMock.Verify(x => x.Serialize(It.Is<SerializableLogModel>(s => s.Context == logModel.Context &&
                                                                                              s.Description == logModel.Description &&
                                                                                              s.LogDate == logModel.LogDate &&
                                                                                              s.LogId == logModel.LogId &&
                                                                                              s.LogItems == logModel.LogItems &&
                                                                                              s.LogType == logModel.LogType &&
                                                                                              s.Source == logModel.Source &&
                                                                                              s.Tags == logModel.Tags), jsonSerializerSettings), Times.Once);
        }

        [Test]
        public void Serialize_When_Called_Creates_Json_Resresentation_Of_Object_Returned_By_Converter()
        {
            // Arrange
            LogModelFunc<Object> converter = d => new CustomType() { Name = d.Context, LogItems = d.LogItems };
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer(converter, jsonSerializerSettings);
            LogModel logModel = new LogModel()
            {
                Context = "Context1",
                LogItems = new LogItem[]
                {
                     new LogItem("", ""),
                     new LogItem("", ""),
                }
            };

            // Act
            string resultJson = jsonLogSerializer.Serialize(logModel);

            // Assert
            string expectedJson = JsonConvert.SerializeObject(converter(logModel));
            Assert.AreEqual(expectedJson, resultJson);
        }

        [Test]
        public void Serialize_When_Called_Given_That_Default_Converter_Is_Used_Creates_Json_Resresentation_Of_LogModel()
        {
            // Arrange
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer();
            LogModel logModel = new LogModel()
            {
                Context = "Context1",
                LogItems = new LogItem[]
                {
                     new LogItem("", ""),
                     new LogItem("", ""),
                }
            };

            // Act
            string resultJson = jsonLogSerializer.Serialize(logModel);

            // Assert
            string expectedJson = JsonConvert.SerializeObject(new SerializableLogModel(logModel));
            Assert.AreEqual(expectedJson, resultJson);
        }

        #endregion

        #region Tests for ResetJsonSerializerFacade method

        [Test]
        public void ResetJsonSerializerFacade_When_Called_Sets_Specified_Object_As_New_Json_Serializer_Facade()
        {
            // Arrange
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer();
            var jsonSerializerFacade = new Mock<IJsonSerializerFacade>().Object;

            // Act
            jsonLogSerializer.ResetJsonSerializerFacade(jsonSerializerFacade);

            // Assert
            Assert.AreEqual(jsonSerializerFacade, jsonLogSerializer.JsonSerializerFacade);
        }

        [Test]
        public void ResetJsonSerializerFacade_When_Called_With_Null_Json_Serializer_Facade_Throws_ArgumentNullException()
        {
            // Arrange
            JsonLogSerializer jsonLogSerializer = new JsonLogSerializer();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                jsonLogSerializer.ResetJsonSerializerFacade(null as IJsonSerializerFacade);
            });
        }

        #endregion
    }
}
