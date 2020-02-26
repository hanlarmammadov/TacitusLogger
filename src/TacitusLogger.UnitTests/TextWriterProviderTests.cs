using Moq;
using NUnit.Framework;
using System;
using System.IO;
using TacitusLogger.Destinations.TextWriter;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class TextWriterProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_TextWriter_When_Called_Sets_TextWriter()
        {
            // Arrange
            TextWriter textWriter = new Mock<TextWriter>().Object;

            // Act
            TextWriterProvider textWriterProvider = new TextWriterProvider(textWriter);

            // Assert
            Assert.AreEqual(textWriter, textWriterProvider.TextWriter);
        }

        [Test]
        public void Ctor_Taking_TextWriter_When_Called_With_Null_TextWriter_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                TextWriterProvider textWriterProvider = new TextWriterProvider(null as TextWriter);
            });
        }

        #endregion

        #region Tests for GetTextWriter method

        [Test]
        public void  GetTextWriter_When_Called_Returns_Assigned_TextWriter()
        {
            // Arrange
            TextWriter textWriter = new Mock<TextWriter>().Object;
            TextWriterProvider textWriterProvider = new TextWriterProvider(textWriter);

            // Act
            var returnedTextWriter = textWriterProvider.GetTextWriter(new LogModel());

            // Assert
            Assert.AreEqual(textWriter, returnedTextWriter);
        }

        [Test]
        public void GetTextWriter_When_Called_With_Null_LogModel_Returns_Assigned_TextWriter()
        {
            // Arrange
            TextWriter textWriter = new Mock<TextWriter>().Object;
            TextWriterProvider textWriterProvider = new TextWriterProvider(textWriter);

            // Act
            var returnedTextWriter = textWriterProvider.GetTextWriter(null);

            // Assert
            Assert.AreEqual(textWriter, returnedTextWriter);
        }

        [Test]
        public void GetTextWriter_When_Called_Several_Times_Returns_The_Same_TextWriter_Each_Time()
        {
            // Arrange
            TextWriter textWriter = new Mock<TextWriter>().Object;
            TextWriterProvider textWriterProvider = new TextWriterProvider(textWriter);

            // Act
            var returnedTextWriter1 = textWriterProvider.GetTextWriter(new LogModel());
            var returnedTextWriter2 = textWriterProvider.GetTextWriter(new LogModel());

            // Assert
            Assert.AreEqual(textWriter, returnedTextWriter1);
            Assert.AreEqual(textWriter, returnedTextWriter2);
        }

        #endregion 
    }
}
