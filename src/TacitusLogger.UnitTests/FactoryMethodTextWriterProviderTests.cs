using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using TacitusLogger.Destinations.TextWriter;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class FactoryMethodTextWriterProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_FactoryMethod_When_Called_Sets_FactoryMethod()
        {
            // Arrange 
            LogModelFunc<TextWriter> factoryMethod = d => null;
             
            // Act
            FactoryMethodTextWriterProvider factoryMethodTextWriterProvider = new FactoryMethodTextWriterProvider(factoryMethod);

            // Assert
            Assert.AreEqual(factoryMethod, factoryMethodTextWriterProvider.FactoryMethod);
        }

        [Test]
        public void Ctor_Taking_FactoryMethod_When_Called_With_Null_FactoryMethod_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                FactoryMethodTextWriterProvider factoryMethodTextWriterProvider = new FactoryMethodTextWriterProvider(null);
            });
        }

        #endregion

        #region Tests for GetTextWriter method

        [Test]
        public void  GetTextWriter_When_Called_Returns_TextWriter_Using_FactoryMethod()
        {
            // Arrange
            LogModel logModel = new LogModel();
            TextWriter textWriter = new Mock<TextWriter>().Object; 
            LogModelFunc<TextWriter> factoryMethod = d => 
            {
                if (d == logModel)
                    return textWriter;
                else
                    return null;
            }; 
            FactoryMethodTextWriterProvider factoryMethodTextWriterProvider = new FactoryMethodTextWriterProvider(factoryMethod);

            // Act
            var textWriterReturned = factoryMethodTextWriterProvider.GetTextWriter(logModel);

            // Assert 
            Assert.AreEqual(textWriter, textWriterReturned);
        }
        
        #endregion 
    }
}
