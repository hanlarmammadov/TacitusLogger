using Newtonsoft.Json;
using NUnit.Framework;
using TacitusLogger.Components.Json;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class NewtonsoftJsonSerializerFacadeTests
    {
        [Test]
        public void Serialize_When_Called_Returns_Json_Representation_Of_Object()
        {
            // Arrange
            var obj = new
            {
                Property1 = "Value1",
                ChildObj1 = new
                {
                    Property1 = "Value1",
                    Property2 = "Value2",
                    ChildObj2 = new
                    {
                        Property1 = "Value1",
                        Property2 = "Value2"
                    }
                },
            };
            NewtonsoftJsonSerializerFacade newtonsoftJsonSerializerFacade = new NewtonsoftJsonSerializerFacade();

            //Act
            var jsonReturned = newtonsoftJsonSerializerFacade.Serialize(obj, new JsonSerializerSettings());

            //Assert
            string jsonExpected = JsonConvert.SerializeObject(obj, new JsonSerializerSettings());
            Assert.AreEqual(jsonExpected, jsonReturned);
        }

        [Test]
        public void Serialize_When_Called_With_Null_Object()
        {
            // Arrange 
            NewtonsoftJsonSerializerFacade newtonsoftJsonSerializerFacade = new NewtonsoftJsonSerializerFacade();

            //Act
            var jsonReturned = newtonsoftJsonSerializerFacade.Serialize(null, new JsonSerializerSettings());

            //Assert
            Assert.AreEqual("null", jsonReturned);
        }

        [Test]
        public void Serialize_When_Called_With_Object_Containing_Reference_Loop_And_Json_Settings_With_Default_Reference_Loop_Handling()
        {
            // Arrange
            var obj = new ClassWithCircularReference();
            NewtonsoftJsonSerializerFacade newtonsoftJsonSerializerFacade = new NewtonsoftJsonSerializerFacade();

            Assert.Catch<Newtonsoft.Json.JsonSerializationException>(() =>
            {
                //Act
                var jsonReturned = newtonsoftJsonSerializerFacade.Serialize(obj, new JsonSerializerSettings());
            });
        }

        [Test]
        public void Serialize_When_Called_With_Object_Containing_Reference_Loop_And_Json_Settings_With_Custom_Reference_Loop_Handling_No_Exception_Is_Thrown()
        {
            // Arrange
            var obj = new ClassWithCircularReference();
            NewtonsoftJsonSerializerFacade newtonsoftJsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
             
            //Act
            var jsonReturned = newtonsoftJsonSerializerFacade.Serialize(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}
