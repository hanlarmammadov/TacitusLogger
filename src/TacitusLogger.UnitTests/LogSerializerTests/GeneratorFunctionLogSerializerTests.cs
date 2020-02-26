using NUnit.Framework;
using System; 
using TacitusLogger.Serializers;

namespace TacitusLogger.UnitTests.LogSerializerTests
{
    [TestFixture]
    public class GeneratorFunctionLogSerializerTests
    {
        #region Ctors tests

        [Test]
        public void ConstructorWithLogModelFunc_WhenCalled_AssignFactoryDelegateToProvidedOne()
        {
            // Arrange 
            LogModelFunc<string> generatorFunc = x => "test";

            // Act
            GeneratorFunctionLogSerializer generatorFunctionLogSerializer = new GeneratorFunctionLogSerializer(generatorFunc);

            // Assert  
            Assert.AreEqual(generatorFunc, generatorFunctionLogSerializer.GeneratorFunction);
        }
        [Test]
        public void ConstructorWithLogModelFunc_WhenCalledWithNullDelegate_ThrowsArgumentNullException()
        {
            // Arrange 
            LogModelFunc<string> generatorFunc = null;

            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                GeneratorFunctionLogSerializer generatorFunctionLogSerializer = new GeneratorFunctionLogSerializer(generatorFunc);
            });
        }
        
        #endregion

        #region Serialize method test

        [Test]
        public void Serialize_WhenCalled_ThrowsArgumentNullException()
        {
            // Arrange 
            bool delegateHasBeenCalled = false;
            LogModelFunc<string> generatorFunc = (ld) => { delegateHasBeenCalled = true; return "log string"; };
            GeneratorFunctionLogSerializer generatorFunctionLogSerializer = new GeneratorFunctionLogSerializer(generatorFunc);

            // Act
            string logStr = generatorFunctionLogSerializer.Serialize(new LogModel());

            // Assert  
            Assert.IsTrue(delegateHasBeenCalled);
            Assert.AreEqual("log string", logStr);
        }

        #endregion

        #region Tests for ToString method 

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Serializer()
        {
            // Arrange 
            GeneratorFunctionLogSerializer generatorFunctionLogSerializer = new GeneratorFunctionLogSerializer(m => null);

            // Act
            var result = generatorFunctionLogSerializer.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Serializers.GeneratorFunctionLogSerializer")); 
        }

        #endregion
    }
}
